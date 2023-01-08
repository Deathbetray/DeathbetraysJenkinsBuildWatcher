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

namespace DeathbetraysJenkinsBuildWatcher
{
    public enum BuildState
    {
        Success,
        Failed,
        Unknown,
    }

    public class JenkinsBuildChannel
    {
        public string Name
        {
            get { return m_name; }
            private set { m_name = value; }
        }
        private string m_name;

        public string Url
        {
            get { return m_credentials.Url + "/job/" + Name; }
        }

        public bool HasValidData
        {
            get { return Url != null; }
        }

        JenkinsCredentials m_credentials = null;

        public BuildState State
        {
            get { return m_latestBuildResult; }
        }
        private BuildState m_latestBuildResult = BuildState.Unknown;

        public bool IsGreen
        {
            get { return m_latestBuildResult == BuildState.Success; }
        }

        public bool IsRed
        {
            get { return m_latestBuildResult == BuildState.Failed; }
        }

        public bool UserIsCulprit
        {
            get { return m_userIsCulprit; }
            private set
            {
                if (m_userIsCulprit != value)
                {
                    Logger.Log(Logger.LogLevel.Info, "[JenkinsBuildChannel] User is now {0}a culprit for {1}.", value ? "" : "not ", Name);
                    m_userIsCulprit = value;
                    OnStatusChangeHandler(this);
                }
            }
        }
        private bool m_userIsCulprit = false;

        public bool IsBeingWatched
        {
            get { return WatchedRefCount > 0; }
        }

        private int WatchedRefCount
        {
            get { return m_watchedRefCount; }
            set
            {
                bool prevIsBeingWatched = IsBeingWatched;
                m_watchedRefCount = value;
                if (!prevIsBeingWatched && IsBeingWatched)
                {
                    // Is now being watched, so start the timer.
                    StartTimer();
                }
                else if (prevIsBeingWatched && !IsBeingWatched)
                {
                    // Is no longer being watched, so stop the timer.
                    StopTimer();
                }
            }
        }
        private int m_watchedRefCount = 0;

        private System.Threading.Timer m_timer = null;
        private Thread m_thread = null;
        private DateTime m_threadStartedAt;

        private void OnStatusChangeIgnore(JenkinsBuildChannel _build) { }
        public delegate void OnStatusChange(JenkinsBuildChannel _build);
        public OnStatusChange OnStatusChangeHandler;


        public JenkinsBuildChannel(JenkinsCredentials _credentials, string _name)
        {
            m_name = _name;
            m_credentials = _credentials;
            m_credentials.OnValidationCompleteHandler += (JenkinsCredentials _creds) =>
            {
                if (_creds.IsValid)
                {
                    StartTimer();
                }
                else
                {
                    StopTimer();
                }
            };

            OnStatusChangeHandler += OnStatusChangeIgnore;

            Preferences.Prefs.OnTimerIntervalChangedHandler += (int _prevValue, int _newValue) =>
            {
                UpdateTimer();
            };
        }

        public void UpdateBuildStatus()
        {
            if (m_thread == null || !m_thread.IsAlive)
            {
                m_threadStartedAt = DateTime.Now;
                m_thread = new Thread(UpdateBuildStatusThread);
                m_thread.Start();
            }
            else if (m_thread != null && m_thread.IsAlive)
            {
                Logger.Log(Logger.LogLevel.Warning, "[JenkinsBuildChannel] Attempted to update while an update was in flight: {0}.", Name);
                TimeSpan timeSinceStarted = DateTime.Now - m_threadStartedAt;
                if (timeSinceStarted.TotalMinutes >= 60)
                {
                    Logger.Log(Logger.LogLevel.Warning, "[JenkinsBuildChannel] Cancelled build update because it's been running for longer than an hour: {0}.", Name);
                    m_thread.Abort();
                }
            }
        }

        public void OpenUrlLastFailed()
        {
            Process.Start(Url + "/lastFailedBuild");
        }

        public void StartWatching()
        {
            ++WatchedRefCount;
        }

        public void StopWatching()
        {
            --WatchedRefCount;
        }

        private void StartTimer()
        {
            if (!m_credentials.IsValid || TestValues.g_disableTimer)
            {
                return;
            }

            if (m_timer != null)
            {
                return;
            }

            m_timer = new System.Threading.Timer((object _state) => { UpdateBuildStatus(); }, null, 0, Preferences.Prefs.TimerInterval);
            Logger.Log(Logger.LogLevel.Info, "[JenkinsBuildChannel] Started update timer with interval of {0} for {1}.", Preferences.Prefs.TimerInterval, Name);
        }

        private void UpdateTimer()
        {
            if (m_timer != null)
            {
                m_timer.Change(0, Preferences.Prefs.TimerInterval);
                Logger.Log(Logger.LogLevel.Info, "[JenkinsBuildChannel] Updated update timer with interval of {0} for {1}.", Preferences.Prefs.TimerInterval, Name);
            }
        }

        private void StopTimer()
        {
            m_timer = null;
            Logger.Log(Logger.LogLevel.Info, "[JenkinsBuildChannel] Stopped update timer for {0}.", Name);
        }

        private void UpdateBuildStatusThread()
        {
            // Get the latest build info.
            int prevSuccessId, prevFailedId;
            HashSet<string> culprits;
            if (!GetBuildStatusLatestSuccess(out prevSuccessId))
            {
                Logger.Log(Logger.LogLevel.Warning, "[JenkinsBuildChannel] Failed to get latest build success for {0}", Name);
                return;
            }
            if (!GetBuildStatusLatestFailure(out prevFailedId, out culprits))
            {
                Logger.Log(Logger.LogLevel.Warning, "[JenkinsBuildChannel] Failed to get latest build failure for {0}", Name);
                return;
            }

            BuildState latestBuildResult = BuildState.Unknown;
            bool isCulprit = false;
            if (prevSuccessId < 0 && prevFailedId < 0)
            {
                latestBuildResult = BuildState.Unknown;
            }
            else if (prevSuccessId > prevFailedId)
            {
                latestBuildResult = BuildState.Success;
            }
            else if (prevFailedId > prevSuccessId)
            {
                latestBuildResult = BuildState.Failed;

                // Gather the culprits.
                int idFound;
                bool result;
                HashSet<string> c;
                for (int i = prevFailedId - 1; i > prevSuccessId && i > 0; --i)
                {
                    if (!GetBuildStatusById(i, out idFound, out result, out c))
                    {
                        Logger.Log(Logger.LogLevel.Warning, "[JenkinsBuildChannel] Failed to request build id {0} for {1} so early-ing out of update", i, Name);
                        return;
                    }
                    culprits.UnionWith(c);

                    // Early out (just in case the build channel has dozens of failed builds).
                    if (c.Contains(m_credentials.Username))
                    {
                        Logger.Log(Logger.LogLevel.Info, "[JenkinsBuildChannel] Culprit detected in build {0} id {1}", Name, i);
                        break;
                    }
                }

                isCulprit = culprits.Contains(m_credentials.Username);
            }

            UserIsCulprit = isCulprit;

            if (m_latestBuildResult != latestBuildResult)
            {
                m_latestBuildResult = latestBuildResult;
                OnStatusChangeHandler(this);
            }
        }

        private bool GetBuildStatusLatestSuccess(out int _idFound)
        {
            bool buildSuccess;
            HashSet<string> culprits;
            return GetBuildStatus("lastSuccessfulBuild", out _idFound, out buildSuccess, out culprits);
        }

        private bool GetBuildStatusLatestFailure(out int _idFound, out HashSet<string> _culprits)
        {
            bool buildSuccess;
            return GetBuildStatus("lastFailedBuild", out _idFound, out buildSuccess, out _culprits);
        }

        //private int GetBuildStatusLatest(out bool _result)
        //{
        //    return GetBuildStatus("lastBuild", out _result);
        //}

        private bool GetBuildStatusById(int _id, out int _idFound, out bool _buildSuccess, out HashSet<string> _culprits)
        {
            string id = _id <= 0 ? "lastBuild" : _id.ToString();
            return GetBuildStatus(id, out _idFound, out _buildSuccess, out _culprits);
        }

        private bool GetBuildStatus(string _buildId, out int _buildIdOut, out bool _buildSuccess, out HashSet<string> _culprits)
        {
            bool result = false;
            bool buildSuccess = false;
            HashSet<string> culprits = new HashSet<string>();
            string name = Name;
            string url = Url + "/" + _buildId + "/api/json";

            // The function is performed synchronously, so be sure this is on a thread.
            HttpHelper.HttpRequest(m_credentials.Username, m_credentials.Password, url, (HttpStatusCode _status, string _errorReason, string _jsonResponse) =>
            {
                if (_status == HttpStatusCode.OK)
                {
                    Logger.Log(Logger.LogLevel.Info, "[JenkinsBuildChannel] Successful request of build {0}'s ID {1} at {2}", name, _buildId, url);
                    JsonElement root = JsonDocument.Parse(_jsonResponse).RootElement;
                    _buildId = root.GetProperty("number").GetRawText();
                    buildSuccess = root.GetProperty("result").GetString() == "SUCCESS";
                    culprits = ExtractCulprits(_jsonResponse);
                    result = true;
                }
                else
                {
                    Logger.Log(Logger.LogLevel.Warning, "[JenkinsBuildChannel] Failed request of build {0}'s ID {1} at {2}:\n\tCode: {3}\n\tReason: {4}.",
                        name, _buildId, url, _status.ToString(), _errorReason);
                    _buildId = "-1";
                }
            });

            _buildIdOut = Int32.Parse(_buildId);
            _buildSuccess = buildSuccess;
            _culprits = culprits;
            return result;
        }

        private HashSet<string> ExtractCulprits(string _json)
        {
            HashSet<string> foundCulprits = new HashSet<string>();
            JsonElement root = JsonDocument.Parse(_json).RootElement;
            JsonElement culprits = root.GetProperty("culprits");
            foreach (var culprit in culprits.EnumerateArray())
            {
                string absUrl = culprit.GetProperty("absoluteUrl").GetRawText();
                string[] split = absUrl.Split(new char[] { '/', '\"' });
                if (split.Length > 1)
                {
                    foundCulprits.Add(split[split.Length - 2]);
                }
                else
                {
                    Logger.Log(Logger.LogLevel.Warning, "[JenkinsBuildChannel] Cannot extract name from absolute url of culprit: {0}.", absUrl);
                }
            }

            return foundCulprits;
        }
    }
}
