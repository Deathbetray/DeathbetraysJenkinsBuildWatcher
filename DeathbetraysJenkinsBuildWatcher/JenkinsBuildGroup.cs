using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DeathbetraysJenkinsBuildWatcher
{
    public class JenkinsBuildGroup
    {
        private IJenkinsWatcher m_watcher;

        public string Name
        {
            get { return m_name; }
            private set { m_name = value; }
        }
        private string m_name = "";

        public bool IsMonitored
        {
            get { return m_isMonitored; }
            set
            {
                if (m_isMonitored != value)
                {
                    bool prevIsMonitored = IsMonitored;
                    m_isMonitored = value;
                    int changeType = 0;  // mainly so I don't have to duplicate the foreach loop
                    if (!prevIsMonitored && IsMonitored)
                    {
                        changeType = 1;
                    }
                    else if (prevIsMonitored && !IsMonitored)
                    {
                        changeType = 2;
                    }

                    if (changeType > 0 && changeType < 3)
                    {
                        foreach (string buildName in Builds.Keys)
                        {
                            JenkinsBuildChannel build = m_watcher.GetBuild(buildName);
                            if (changeType == 1)
                            {
                                build.StartWatching();
                            }
                            else if (changeType == 2)
                            {
                                build.StopWatching();
                            }
						}
                    }
                }
            }
        }
        private bool m_isMonitored = false;

        public Dictionary<string, BuildState> Builds
        {
            get { return m_builds; }
        }
        private Dictionary<string, BuildState> m_builds = new Dictionary<string, BuildState>();

        public BuildState State
        {
            get { return m_state; }
        }
        private BuildState m_state = BuildState.Unknown;

        public bool UserIsCulprit
        {
            get { return m_userIsCulprit; }
            private set
            {
                if (m_userIsCulprit != value)
                {
                    m_userIsCulprit = value;
                    OnBlameChangeHandler(this);
                }
            }
        }
        private bool m_userIsCulprit = false;

        private void OnStateChangeIgnore(JenkinsBuildGroup _buildGroup) { }
        public delegate void OnStateChange(JenkinsBuildGroup _buildGroup);
        public OnStateChange OnStateChangeHandler;

        private void OnBlameChangeIgnore(JenkinsBuildGroup _buildGroup) { }
        public delegate void OnBlameChange(JenkinsBuildGroup _buildGroup);
        public OnBlameChange OnBlameChangeHandler;


        public JenkinsBuildGroup(IJenkinsWatcher _watcher, string _name)
        {
            m_watcher = _watcher;
            m_name = _name;

            OnStateChangeHandler += OnStateChangeIgnore;
            OnBlameChangeHandler += OnBlameChangeIgnore;
        }

        public void AddBuild(string _buildName)
        {
            if (!Builds.ContainsKey(_buildName))
            {
                JenkinsBuildChannel build = m_watcher.GetBuild(_buildName);
                BuildState state = BuildState.Unknown;
                if (build != null)
                {
                    state = build.State;
                    if (IsMonitored)
                    {
                        build.StartWatching();
                    }
                }

                Builds.Add(_buildName, state);
                UpdateState();
            }
            else
            {
                Logger.Log(Logger.LogLevel.Warning, "[JenkinsBuildGroup] Attempted to add a build to a group that it's already in: {0} to {1}.", _buildName, Name);
            }
        }

        public void AddBuild(JenkinsBuildChannel _build)
        {
            Debug.Assert(_build != null);
            if (IsMonitored)
            {
                _build.StartWatching();
            }
            AddBuild(_build.Name);
        }

        public void RemoveBuild(string _buildName)
        {
            if (Builds.ContainsKey(_buildName))
            {
                JenkinsBuildChannel build = m_watcher.GetBuild(_buildName);
                if (IsMonitored)
                {
                    build.StopWatching();
                }

                Builds.Remove(_buildName);
                UpdateState();
            }
            else
            {
                Logger.Log(Logger.LogLevel.Warning, "[JenkinsBuildGroup] Attempted to remove a build from a group that it's not in: {0} to {1}.", _buildName, Name);
            }
        }

        public void RemoveBuild(JenkinsBuildChannel _build)
        {
            Debug.Assert(_build != null);
            if (IsMonitored)
            {
                _build.StopWatching();
            }
            RemoveBuild(_build.Name);
        }

        public bool Contains(JenkinsBuildChannel _build)
        {
            return Contains(_build.Name);
        }

        public bool Contains(string _buildName)
        {
            return Builds.ContainsKey(_buildName);
        }

        public void UpdateBuild(JenkinsBuildChannel _build)
        {
            BuildState state = BuildState.Unknown;
            if (_build == null || !_build.HasValidData)
            {
                state = BuildState.Unknown;
            }
            else if (_build.IsGreen)
            {
                state = BuildState.Success;
            }
            else if (_build.IsRed)
            {
                state = BuildState.Failed;
            }

            Builds[_build.Name] = state;
            UpdateState();
        }

        public void Serialise(XmlWriter _writer)
        {
            _writer.WriteStartElement("BuildGroup");
            _writer.WriteAttributeString("name", Name);
            _writer.WriteAttributeString("isMonitored", IsMonitored.ToString());

            _writer.WriteStartElement("Builds");
            foreach (string build in Builds.Keys)
            {
                _writer.WriteStartElement("Build");
                _writer.WriteString(build);
                _writer.WriteEndElement();  // Build
            }
            _writer.WriteEndElement();  // Builds

            _writer.WriteEndElement();  // BuildGroup
        }

        private void OnBuildStatusChange(JenkinsBuildChannel _buildChannel)
        {
            if (!Builds.ContainsKey(_buildChannel.Name))
            {
                // Shouldn't be listening for this build.
                _buildChannel.OnStatusChangeHandler -= OnBuildStatusChange;
            }
            else
            {
                Builds[_buildChannel.Name] = _buildChannel.IsGreen ? BuildState.Success : BuildState.Failed;
                UpdateState();
            }
        }

        private void UpdateState(bool _forceRefresh = false)
        {
            if (_forceRefresh)
            {
                List<string> buildNames = Builds.Keys.ToList();
                foreach (string buildName in buildNames)
                {
                    JenkinsBuildChannel build = m_watcher.GetBuild(buildName);
                    UpdateBuild(build);
                }
            }

            BuildState prevState = m_state;
            m_state = Builds.Count == 0 ? BuildState.Unknown : BuildState.Success;
            foreach (BuildState value in Builds.Values)
            {
                if (value == BuildState.Failed)
                {
                    m_state = BuildState.Failed;
                    break;
                }
                else if (value == BuildState.Unknown)
                {
                    m_state = BuildState.Unknown;
                }
            }

            if (prevState != m_state)
            {
                OnStateChangeHandler(this);
            }

            // Check if we've become a culprit.
            if (m_state == BuildState.Failed)
            {
                bool isCulprit = false;
                List<string> buildNames = Builds.Keys.ToList();
                foreach (string buildName in buildNames)
                {
                    JenkinsBuildChannel build = m_watcher.GetBuild(buildName);
                    if (build.UserIsCulprit)
                    {
                        isCulprit = true;
                        break;
                    }
                }

                UserIsCulprit = isCulprit;
            }
        }
    }
}
