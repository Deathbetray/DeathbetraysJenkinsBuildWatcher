using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DeathbetraysJenkinsBuildWatcher
{
    public class JenkinsWatcher : IJenkinsWatcher
    {
        public Dictionary<string, JenkinsBuildChannel> Builds
        {
            get { return m_builds; }
            private set { m_builds = value; }
        }
        private Dictionary<string, JenkinsBuildChannel> m_builds = new Dictionary<string, JenkinsBuildChannel>();

        public Dictionary<string, JenkinsBuildGroup> BuildGroups
        {
            get { return m_buildGroups; }
            private set { m_buildGroups = value; }
        }
        private Dictionary<string, JenkinsBuildGroup> m_buildGroups = new Dictionary<string, JenkinsBuildGroup>();

        public Dictionary<string, JenkinsBuildChannel> WatchedBuilds
        {
            get
            {
                Dictionary<string, JenkinsBuildChannel> builds = new Dictionary<string, JenkinsBuildChannel>();
                foreach (JenkinsBuildGroup group in BuildGroups.Values)
                {
                    foreach (string b in group.Builds.Keys)
                    {
                        if (!builds.ContainsKey(b) && Builds.ContainsKey(b))
                        {
                            builds.Add(b, Builds[b]);
                        }
                    }
                }

                return builds;
            }
        }

        private JenkinsCredentials m_credentials = null;

        private Thread m_thread = null;


        private void OnBuildChannelStateChangeIgnore(JenkinsBuildChannel _buildChannel) { }
        public delegate void OnBuildChannelStateChange(JenkinsBuildChannel _buildChannel);
        public OnBuildChannelStateChange OnBuildChannelStateChangeHandler;

        private void OnBuildGroupStateChangeIgnore(JenkinsBuildGroup _buildGroup) { }
        public delegate void OnBuildGroupStateChange(JenkinsBuildGroup _buildGroup);
        public OnBuildGroupStateChange OnBuildGroupStateChangeHandler;

        private void OnBuildGroupBlameChangeIgnore(JenkinsBuildGroup _buildGroup) { }
        public delegate void OnBuildGroupBlameChange(JenkinsBuildGroup _buildGroup);
        public OnBuildGroupBlameChange OnBuildGroupBlameChangeHandler;
        

        public JenkinsBuildChannel GetBuild(string _buildName)
        {
            if (Builds.ContainsKey(_buildName))
            {
                return Builds[_buildName];
            }

            return null;
        }

        public JenkinsBuildGroup GetBuildGroup(string _buildGroup)
        {
            if (BuildGroups.ContainsKey(_buildGroup))
            {
                return BuildGroups[_buildGroup];
            }

            return null;
        }


        public bool Init(JenkinsCredentials _credentials)
        {
            Debug.Assert(_credentials != null);
            m_credentials = _credentials;
            m_credentials.OnValidationCompleteHandler += (JenkinsCredentials _creds) =>
            {
                if (_creds.IsValid)
                {
                    GatherBuildChannels();
                }
                else
                {
                    // If the credentials become invalid then clear the list of builds.
                    Builds = new Dictionary<string, JenkinsBuildChannel>();
                }
            };

            OnBuildChannelStateChangeHandler += OnBuildChannelStateChangeIgnore;
            OnBuildGroupStateChangeHandler += OnBuildGroupStateChangeIgnore;
            OnBuildGroupBlameChangeHandler += OnBuildGroupBlameChangeIgnore;

            return true;
        }

        public void CreateBuildGroup(string _name, bool _isMonitored = false)
        {
            if (BuildGroups.ContainsKey(_name))
            {
                return;
            }

            JenkinsBuildGroup group = new JenkinsBuildGroup(this, _name);
            group.IsMonitored = _isMonitored;
            group.OnStateChangeHandler += OnWatchedBuildsStateChange;
            group.OnBlameChangeHandler += OnWatchedBuildsBlameChange;
            BuildGroups.Add(_name, group);
            Logger.Log(Logger.LogLevel.Info, "[JenkinsWatcher] Created new build group: {0}.", _name);
        }

        public void DeleteBuildGroup(string _name)
        {
            JenkinsBuildGroup group = BuildGroups[_name];
            DeleteBuildGroup(group);
        }

        public void DeleteBuildGroup(JenkinsBuildGroup _group)
        {
            if (_group != null)
            {
                _group.OnStateChangeHandler -= OnWatchedBuildsStateChange;
                BuildGroups.Remove(_group.Name);
                Logger.Log(Logger.LogLevel.Info, "[JenkinsWatcher] Deleted a build group: {0}.", _group.Name);
            }
            else
            {
                Logger.Log(Logger.LogLevel.Warning, "[JenkinsWatcher] Attempted to delete a build group that doesn't exist: {0}.", _group.Name);
            }
        }

        public void AddBuildToGroup(string _buildName, string _buildGroup)
        {
            JenkinsBuildGroup group = BuildGroups[_buildGroup];
            if (group != null)
            {
                group.AddBuild(_buildName);
                OnBuildGroupStateChangeHandler(group);
            }
            else
            {
                Logger.Log(Logger.LogLevel.Warning, "[JenkinsWatcher] Attempted to add a build to a group that doesn't exist: {0} to {1}.", _buildName, _buildGroup);
            }
        }

        public void RemoveBuildFromGroup(string _buildName, string _buildGroup)
        {
            JenkinsBuildGroup group = BuildGroups[_buildGroup];
            if (group != null)
            {
                group.RemoveBuild(_buildName);
                Logger.Log(Logger.LogLevel.Spam, "[JenkinsWatcher] Removed build '{0}' from build group '{1}'.", _buildName, _buildGroup);
                OnBuildGroupStateChangeHandler(group);
            }
            else
            {
                Logger.Log(Logger.LogLevel.Warning, "[JenkinsWatcher] Attempted to add a build to a group that doesn't exist: {0} to {1}.", _buildName, _buildGroup);
            }
        }

        public void ResetBuildGroups()
        {
            List<JenkinsBuildGroup> groups = BuildGroups.Values.ToList();
            foreach (JenkinsBuildGroup group in groups)
            {
                DeleteBuildGroup(group);
            }
        }

        public void Refresh()
        {
            foreach (JenkinsBuildChannel build in WatchedBuilds.Values)
            {
                build.UpdateBuildStatus();
            }
        }

        public void Serialise(XmlWriter _writer)
        {
            _writer.WriteStartElement("BuildGroups");
            foreach (JenkinsBuildGroup group in BuildGroups.Values)
            {
                group.Serialise(_writer);
            }
            _writer.WriteEndElement();
        }

        public void Deserialise(XmlNode _xml)
        {
            if (_xml.Name != "BuildGroups")
            {
                Logger.Log(Logger.LogLevel.Error,
                    "[JenkinsWatcher] Failed to serialise because was given an xml node that wasn't correctly named: {0}",
                    _xml.Name);
                return;
            }

            foreach (XmlNode buildGroups in _xml.ChildNodes)
            {
                string buildGroupName = buildGroups.Attributes.GetNamedItem("name").Value;
                string isMonitoredStr = buildGroups.Attributes.GetNamedItem("isMonitored").Value;
                CreateBuildGroup(buildGroupName, bool.Parse(isMonitoredStr));

                XmlNode buildGroup = buildGroups.FirstChild;
                foreach (XmlNode builds in buildGroup.ChildNodes)
                {
                    string buildName = builds.InnerText;
                    AddBuildToGroup(buildName, buildGroupName);
                }
            }
        }

        private void OnWatchedBuildsStateChange(JenkinsBuildGroup _buildGroup)
        {
            OnBuildGroupStateChangeHandler(_buildGroup);
        }

        private void OnWatchedBuildsBlameChange(JenkinsBuildGroup _buildGroup)
        {
            OnBuildGroupBlameChangeHandler(_buildGroup);
        }

        private void OnBuildChannelUpdated(JenkinsBuildChannel _build)
        {
            string buildName = _build.Name;
            foreach (JenkinsBuildGroup group in BuildGroups.Values)
            {
                if (group.Contains(buildName))
                {
                    group.UpdateBuild(_build);
                }
            }

            OnBuildChannelStateChangeHandler(_build);
        }

        private void GatherBuildChannels()
        {
            if (m_thread == null)
            {
                m_thread = new Thread(GatherBuildChannelsThread);
                m_thread.Start();
            }
            else
            {
                Logger.Log(Logger.LogLevel.Warning, "[JenkinsWatcher] Attempted to gather build channels while a request was in flight.");
            }
        }

        private void GatherBuildChannelsThread()
        {
            string url = m_credentials.Url + "/api/json";
            HttpHelper.HttpRequest(m_credentials.Username, m_credentials.Password, url, (HttpStatusCode _status, string _errorReason, string _jsonResponse) =>
            {
                if (_status == HttpStatusCode.OK)
                {
                    Logger.Log(Logger.LogLevel.Info, "[JenkinsWatcher] Successfully requested builds for {0} from {1}.", m_credentials.Username, url);

                    string jobs = JsonUtilities.Find(JsonDocument.Parse(_jsonResponse).RootElement, "jobs");
                    JsonDocument jsonJobs = JsonDocument.Parse(jobs);
                    foreach (JsonElement item in jsonJobs.RootElement.EnumerateArray())
                    {
                        string name = item.GetProperty("name").GetString();
                        if (GetBuild(name) == null)
                        {
                            //string url = item.GetProperty("url").GetString();
                            JenkinsBuildChannel build = new JenkinsBuildChannel(m_credentials, name);
                            build.OnStatusChangeHandler += OnBuildChannelUpdated;
                            Builds.Add(name, build);

                            foreach (JenkinsBuildGroup buildGroup in BuildGroups.Values)
                            {
                                if (!buildGroup.IsMonitored)
                                {
                                    continue;
                                }

                                if (buildGroup.Contains(name))
                                {
                                    build.StartWatching();
                                }
                            }
                        }
                    }

                    // Update all of the found build channels.
                    Refresh();

                    // TODO (EdW): remove builds that weren't returned?
                }
                else
                {
                    Logger.Log(Logger.LogLevel.Warning, "[JenkinsWatcher] Failed to request builds from {0} for {1} with code {2} and reason {3}.",
                        url, m_credentials.Username, _status.ToString(), _errorReason);
                }
            });

            m_thread = null;
        }
    }
}
