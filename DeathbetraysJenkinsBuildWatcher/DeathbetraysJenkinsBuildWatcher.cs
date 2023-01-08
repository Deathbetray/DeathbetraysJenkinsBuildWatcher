using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DeathbetraysJenkinsBuildWatcher
{
    public partial class DeathbetraysJenkinsBuildWatcher : Form
    {
        public JenkinsBuildGroup SelectedBuildGroup
        {
            get
            {
                if (m_watcher == null)
                {
                    return null;
                }

                if (m_watcher.BuildGroups == null)
                {
                    return null;
                }

                if (SelectedBuildGroupName == null)
                {
                    return null;
                }

                if (!m_watcher.BuildGroups.ContainsKey(SelectedBuildGroupName))
                {
                    return null;
                }

                return m_watcher.BuildGroups[SelectedBuildGroupName];
            }
        }

        public string SelectedBuildGroupName
        {
            get
            {
                if (cclbBuildGroups == null)
                {
                    return null;
                }

                if (cclbBuildGroups.SelectedItem == null)
                {
                    return null;
                }

                return cclbBuildGroups.SelectedItem.ToString();
            }
        }

        public List<JenkinsBuildGroup> MonitoredBuildGroups
        {
            get
            {
                List<JenkinsBuildGroup> groups = new List<JenkinsBuildGroup>();
                if (m_watcher == null)
                {
                    return groups;
                }

                foreach (object item in cclbBuildGroups.CheckedItems)
                {
                    JenkinsBuildGroup group = m_watcher.GetBuildGroup(item.ToString());
                    if (group != null)
                    {
                        groups.Add(group);
                    }
                }

                return groups;
            }
        }

        public List<JenkinsBuildChannel> MonitoredBuilds
        {
            get
            {
                List<JenkinsBuildChannel> builds = new List<JenkinsBuildChannel>();
                if (m_watcher == null)
                {
                    return builds;
                }

                foreach (string build in MonitoredBuildNames)
                {
                    JenkinsBuildChannel b = m_watcher.GetBuild(build);
                    if (b != null)
                    {
                        if (!builds.Contains(b))
                        {
                            builds.Add(b);
                        }
                    }
                }

                return builds;
            }
        }

        public List<string> MonitoredBuildNames
        {
            get
            {
                HashSet<string> builds = new HashSet<string>();

                foreach (JenkinsBuildGroup group in MonitoredBuildGroups)
                {
                    builds.UnionWith(group.Builds.Keys.ToHashSet());
                }

                return builds.ToList();
            }
        }

        public bool IsCulpritForBrokenBuild
        {
            get
            {
                List<JenkinsBuildChannel> builds = MonitoredBuilds;
                foreach (JenkinsBuildChannel build in builds)
                {
                    if (build.UserIsCulprit)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private JenkinsCredentials m_credentials = new JenkinsCredentials();
        private JenkinsWatcher m_watcher = null;

        private EnterNameForm m_enterNameForm = null;

        private Icon m_iconGreen = Icon.FromHandle(Properties.Resources.tray_icon_green.GetHicon());
        private Icon m_iconYellow = Icon.FromHandle(Properties.Resources.tray_icon_yellow.GetHicon());
        private Icon m_iconRed = Icon.FromHandle(Properties.Resources.tray_icon_red.GetHicon());
        private int m_trayTooltipDuration = (int)TimeSpan.FromSeconds(5).TotalMilliseconds;

        private bool m_muteBlameNotifications = false;

        private string m_configFile = "config.xml";



        public DeathbetraysJenkinsBuildWatcher(string _url)
        {
            InitializeComponent();

            this.Text = Globals.AppName + " (v" + Globals.Version + ")";

            // Uncomment to output the test value as pretty-fied json.
            //System.IO.File.WriteAllText("json0.txt", JsonUtilities.PrettyJson(TestValues.g_buildOutput21243));
            //System.IO.File.WriteAllText("json1.txt", JsonUtilities.PrettyJson(TestValues.g_buildOutput21244));
            //System.IO.File.WriteAllText("json2.txt", JsonUtilities.PrettyJson(TestValues.g_buildOutput21245));

            //HashSet<string> culprits = new HashSet<string>();
            //culprits.UnionWith(ExtractCulprits(TestValues.g_buildOutput21243));
            //culprits.UnionWith(ExtractCulprits(TestValues.g_buildOutput21244));
            //culprits.UnionWith(ExtractCulprits(TestValues.g_buildOutput21245));

            m_watcher = new JenkinsWatcher();
            m_watcher.OnBuildChannelStateChangeHandler += OnBuildChannelStateChange;
            m_watcher.OnBuildGroupStateChangeHandler += OnBuildGroupStateChange;
            m_watcher.OnBuildGroupBlameChangeHandler += OnBuildGroupBlameChange;
            m_watcher.Init(m_credentials);
            Logger.Log(Logger.LogLevel.Spam, "Finished initialising watcher.");

            m_credentials.OnValidationCompleteHandler += (JenkinsCredentials _credentials) =>
            {
                if (_credentials.IsValid)
                {
                    ThreadHelperClass.SetTextAndColor(this, lblConnectionStatus, "Connected", Color.Green);
                }
                else
                {
                    ThreadHelperClass.SetTextAndColor(this, lblConnectionStatus, "Disconnected", Color.Red);
                }
            };

            m_enterNameForm = new EnterNameForm();

            cmsBuildGroups.Items.Add("Create", null, (object _sender, EventArgs _e) => { OpenAddGroupForm(); });
            cmsBuildGroups.Items.Add("Delete All", null, (object _sender, EventArgs _e) => { DeleteAllBuildGroups(); });

            //notifyIcon.Icon = Icon;
            notifyIcon.Icon = m_iconYellow;
            notifyIcon.Visible = true;
            notifyIcon.Text = "DB's Jenkins Build Watcher";
            cmsNotifyIcon.Items.Add("Open", null, (object _sender, EventArgs _e) => { notifyIcon_MouseDoubleClick(null, null); });
            cmsNotifyIcon.Items.Add("Exit", null, (object _sender, EventArgs _e) => { Application.Exit(); });

            cclbBuildGroups.DetermineItemColourHandler += (ColouredCheckedListBox _object, int _itemIdx) =>
            {
                if (m_watcher == null)
                {
                    return Color.Black;
                }

                string text = _object.GetItemText(_object.Items[_itemIdx]);
                JenkinsBuildGroup buildGroup = m_watcher.GetBuildGroup(text);
                if (buildGroup == null || buildGroup.State == BuildState.Unknown)
                {
                    return Color.Yellow;
                }
                else if (buildGroup.State == BuildState.Success)
                {
                    return Color.Green;
                }
                else
                {
                    return Color.Red;
                }
            };

            cclbBuildListWatched.DetermineItemColourHandler += (ColouredCheckedListBox _object, int _itemIdx) =>
            {
                if (m_watcher == null)
                {
                    return Color.Black;
                }

                string text = _object.GetItemText(_object.Items[_itemIdx]);
                JenkinsBuildChannel build = m_watcher.GetBuild(text);
                if (build == null || !build.HasValidData)
                {
                    return Color.Yellow;
                }
                else if (build.IsGreen)
                {
                    return Color.Green;
                }
                else
                {
                    return Color.Red;
                }
            };

            cclbBuildList.DetermineItemColourHandler += (ColouredCheckedListBox _object, int _itemIdx) =>
            {
                if (m_watcher == null)
                {
                    return Color.Black;
                }

                string text = _object.GetItemText(_object.Items[_itemIdx]);
                JenkinsBuildChannel build = m_watcher.GetBuild(text);
                if (build == null || !build.HasValidData)
                {
                    return Color.Yellow;
                }
                else if (build.IsGreen)
                {
                    return Color.Green;
                }
                else
                {
                    return Color.Red;
                }
            };

            ToggleBuildBreakerNotification(false);
            Preferences.Prefs.OnEnableBuildBreakerNotificationsChangedHandler += (bool _prevValue, bool _newValue) =>
            {
                if (IsCulpritForBrokenBuild)
                {
                    ToggleBuildBreakerNotification(_newValue);
                }
            };

            // Hide this by default at startup.
            btnViewBrokenBuilds.Visible = false;
        }

        private void EnableNonCredentialElements(bool _enabled)
        {
            if (ThreadHelperClass.Recall(this, this, () => { EnableNonCredentialElements(_enabled); }))
            {
                return;
            }

            //cbBuildGroups.Enabled = _enabled;
            //btnGroupAdd.Enabled = _enabled;
            //btnGroupDelete.Enabled = _enabled;
            //
            //clbBuildList.Enabled = _enabled;
            //clbBuildListWatched.Enabled = _enabled;
            //btnMoveBuild.Enabled = _enabled;

            //btnRefresh.Enabled = _enabled;
        }

        private void UpdateBuildListView()
        {
            if (ThreadHelperClass.Recall(this, this, () => { UpdateBuildListView(); }))
            {
                return;
            }

            string selectedBuild = SelectedBuildName(cclbBuildList);
            string selectedWatched = SelectedBuildName(cclbBuildListWatched);
            cclbBuildList.Items.Clear();
            cclbBuildListWatched.Items.Clear();

            // Get the selected build group.
            JenkinsBuildGroup buildGroup = SelectedBuildGroup;

            // Fill in the watched build list.
            if (buildGroup != null)
            {
                foreach (string build in buildGroup.Builds.Keys)
                {
                    cclbBuildListWatched.Items.Add(build);
                    if (build == selectedWatched)
                    {
                        cclbBuildListWatched.SetSelected(cclbBuildListWatched.Items.Count - 1, true);
                    }
                }
            }

            // Fill in the build list, excluding those that are being watched.
            foreach (string build in m_watcher.Builds.Keys)
            {
                if (buildGroup != null)
                {
                    if (buildGroup.Contains(build))
                    {
                        continue;
                    }
                }

                cclbBuildList.Items.Add(build);

                if (build == selectedBuild)
                {
                    cclbBuildList.SetSelected(cclbBuildList.Items.Count - 1, true);
                }
            }
        }

        private void UpdateBuildGroupListView()
        {
            if (ThreadHelperClass.Recall(this, this, () => { UpdateBuildGroupListView(); }))
            {
                return;
            }

            if (m_watcher == null)
            {
                return;
            }

            JenkinsBuildGroup prevGroup = SelectedBuildGroup;
            string prevGroupName = null;
            if (prevGroup != null)
            {
                prevGroupName = prevGroup.Name;
            }

            cclbBuildGroups.Items.Clear();

            int idx = 0;
            foreach (JenkinsBuildGroup group in m_watcher.BuildGroups.Values)
            {
                cclbBuildGroups.Items.Add(group.Name, group.IsMonitored);

                if (group.Name == prevGroupName)
                {
                    idx = cclbBuildGroups.Items.Count - 1;
                }
            }

            UpdateBuildGroupStatus();

            cclbBuildGroups.Text = "";
            if (idx < cclbBuildGroups.Items.Count)
            {
                cclbBuildGroups.SelectedIndex = idx;
            }
        }

        private void UpdateBuildGroupStatus()
        {
            if (ThreadHelperClass.Recall(this, this, () => { UpdateBuildGroupStatus(); }))
            {
                return;
            }

            List<JenkinsBuildGroup> buildGroups = MonitoredBuildGroups;
            bool hasUnknown = buildGroups.Count == 0;
            bool hasRed = false;
            foreach (JenkinsBuildGroup group in buildGroups)
            {
                if (group.State == BuildState.Failed)
                {
                    hasRed = true;
                }
                else if (group.State == BuildState.Unknown)
                {
                    hasUnknown = true;
                }
            }

            bool showBrokenBuildButton = false;
            if (hasRed)
            {
                ThreadHelperClass.SetTextAndColor(this, lblBuildStatusColoured, "Red", Color.Red);
                ShowTrayTooltip(m_iconRed, ToolTipIcon.Error, "Red");
                showBrokenBuildButton = true;
            }
            else if (hasUnknown)
            {
                ThreadHelperClass.SetTextAndColor(this, lblBuildStatusColoured, "Unknown", Color.Yellow);
                ShowTrayTooltip(m_iconYellow, ToolTipIcon.Warning, "Unknown");
            }
            else
            {
                ThreadHelperClass.SetTextAndColor(this, lblBuildStatusColoured, "Green", Color.Green);
                ShowTrayTooltip(m_iconGreen, ToolTipIcon.Info, "Green");
            }

            btnViewBrokenBuilds.Visible = showBrokenBuildButton;
            if (!showBrokenBuildButton)
            {
                // If the broken build button is hidden we can be sure to reset the notification
                // muter.
                ToggleBuildBreakerNotification(false);
            }
        }

        private void OnBuildChannelStateChange(JenkinsBuildChannel _buildChannel)
        {
            UpdateBuildListView();
        }

        private void OnBuildGroupStateChange(JenkinsBuildGroup _buildGroup)
        {
            UpdateBuildGroupListView();
            UpdateBuildGroupStatus();
        }

        private void OnBuildGroupBlameChange(JenkinsBuildGroup _buildGroup)
        {
            if (!Preferences.Prefs.EnableBuildBreakerNotifications)
            {
                return;
            }

            bool hasBecomeCulprit = _buildGroup.UserIsCulprit;
            if (hasBecomeCulprit && m_muteBlameNotifications)
            {
                // Do nothing if we're currently muting notifications.
                return;
            }

            bool isCulprit = IsCulpritForBrokenBuild;
            if (!isCulprit)
            {
                // If they're not a culprit but there was a change that triggered this callback
                // then it means they WERE a culprit, so let's reset.
                ToggleBuildBreakerNotification(false);
            }
            else if (hasBecomeCulprit)
            {
                // Notifications haven't been muted if we got here.
                ToggleBuildBreakerNotification(true);
            }
            else
            {
                // Do nothing if this event is because of the build channel being fixed.
            }
        }

        private void ToggleBuildBreakerNotification(bool _show)
        {
            if (_show && !Preferences.Prefs.EnableBuildBreakerNotifications)
            {
                // Do nothing if we're trying to show notifications but have them disabled in
                // preferences.
                return;
            }

            if (ThreadHelperClass.Recall(this, this, () => { ToggleBuildBreakerNotification(_show); }))
            {
                return;
            }

            if (_show)
            {
                // Maximise and flash the window.
                notifyIcon_MouseDoubleClick(null, null);
                this.FlashStart();
            }
            else
            {
                this.FlashStop();
                m_muteBlameNotifications = false;
            }

            lblBuildBreaker.Visible = _show;
            btnNotMe.Visible = !m_muteBlameNotifications && _show;
        }

        private string SelectedBuildName(CheckedListBox _clb)
        {
            if (_clb == null)
            {
                return null;
            }

            if (_clb.SelectedItem == null)
            {
                return null;
            }

            return _clb.SelectedItem.ToString();
        }

        private void Serialise()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;

            XmlWriter writer = XmlWriter.Create(m_configFile, settings);
            writer.WriteStartDocument();

            // Version.
            writer.WriteStartElement(Globals.AppNameCompact);
            writer.WriteAttributeString("version", Globals.Version);

            // Credentials.
            writer.WriteStartElement("Credentials");
            writer.WriteAttributeString("username", m_credentials.Username);
            // Don't store password.
            writer.WriteAttributeString("url", m_credentials.Url);
            writer.WriteEndElement();

            // Build groups.
            m_watcher.Serialise(writer);

            // Preferences.
            Preferences.Prefs.Serialise(writer);

            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Close();
        }

        private bool Deserialise()
        {
            FileInfo fi = new FileInfo(m_configFile);
            if (!fi.Exists)
            {
                MessageBox.Show("Can't load config as none was found.");
                return false;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(m_configFile);

            XmlElement root = doc.DocumentElement;
            if (root.Name != "DeathbetraysJenkinsBuildWatcher")
            {
                MessageBox.Show("Failed to load config due to root element name mismatch.");
                return false;
            }

            string version = root.GetAttribute("version");
            if (version != Globals.Version)
            {
                // TODO (EdW): Do proper version check.
                MessageBox.Show("Failed to load config due to different version number.");
                return false;
            }

            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name == "Credentials")
                {
                    string username = node.Attributes.GetNamedItem("username").Value;
                    if (!string.IsNullOrEmpty(username))
                    {
                        tbUsername.Text = username;
                    }

                    string url = node.Attributes.GetNamedItem("url").Value;
                    if (!string.IsNullOrEmpty(url))
                    {
                        tbUrl.Text = url;
                    }
                }
                else if (node.Name == "BuildGroups")
                {
                    m_watcher.Deserialise(node);
                }
                else if (node.Name == "Preferences")
                {
                    Preferences.Prefs.Deserialise(node);
                }
                else
                {
                    Logger.Log(Logger.LogLevel.Warning, "Failed to deserialise '{0}' from the config.", node.Name);
                }
            }

            MessageBox.Show("Loaded config.");
            return true;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (m_watcher != null)
            {
                m_watcher.Refresh();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (TestCredentials.g_useTestCredentials)
            {
                tbUsername.Text = TestCredentials.g_username;
                tbPassword.Text = TestCredentials.g_password;
                tbUrl.Text = TestCredentials.g_url;
            }

            string username = tbUsername.Text.Trim();
            if (username == null || username.Length == 0)
            {
                Logger.Log(Logger.LogLevel.Warning, "Attempted to login with no username.");
                return;
            }

            string password = tbPassword.Text.Trim();
            if (password == null || password.Length == 0)
            {
                Logger.Log(Logger.LogLevel.Warning, "Attempted to login with no password.");
                return;
            }

            string url = tbUrl.Text.Trim();
            if (url == null || url.Length == 0)
            {
                Logger.Log(Logger.LogLevel.Warning, "Attempted to login with no url.");
                return;
            }

            m_credentials.Init(username, password, url);
        }

        private void OpenAddGroupForm()
        {
            if (m_watcher == null)
            {
                return;
            }

            m_enterNameForm.Init(m_watcher.BuildGroups.Keys.ToList());
            m_enterNameForm.ShowDialog(this);

            if (!m_enterNameForm.Cancelled)
            {
                if (!m_watcher.BuildGroups.ContainsKey(m_enterNameForm.NewName))
                {
                    m_watcher.CreateBuildGroup(m_enterNameForm.NewName);
                    UpdateBuildGroupListView();
                }
            }
        }

        private void DeathbetraysJenkinsBuildWatcher_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void ShowTrayTooltip(Icon _icon, ToolTipIcon _balloonIcon, string _text)
        {
            if (notifyIcon.Visible)
            {
                notifyIcon.Icon = _icon;
                notifyIcon.BalloonTipIcon = _balloonIcon;
                notifyIcon.BalloonTipText = "Build is " + _text + ".";

                if (WindowState == FormWindowState.Minimized)
                {
                    notifyIcon.ShowBalloonTip(m_trayTooltipDuration);
                }

                // Update the text.
                StringBuilder str = new StringBuilder(Globals.AppName);
                if (m_watcher.BuildGroups.Count > 0)
                {
                    str = new StringBuilder(Globals.AppNameShort);  // abbreviate to save space
                    StringBuilder strRed = new StringBuilder();
                    StringBuilder strUnknown = new StringBuilder();
                    foreach (JenkinsBuildGroup group in MonitoredBuildGroups)
                    {
                        StringBuilder s = null;
                        if (group.State == BuildState.Failed)
                        {
                            s = strRed;
                        }
                        else if (group.State == BuildState.Unknown)
                        {
                            s = strUnknown;
                        }

                        if (s != null)
                        {
                            s.Append("\n-" + group.Name);
                        }
                    }

                    if (strRed.Length > 0)
                    {
                        str.AppendFormat("\nRed{0}", strRed.ToString());
                    }
                    if (strUnknown.Length > 0)
                    {
                        str.AppendFormat("\nUnknown{0}", strUnknown.ToString());
                    }
                }

                // We are only allowed 64 characters, so cull anything extra.
                if (str.Length > 64)
                {
                    str.Remove(61, str.Length - 61);
                    str.Append("...");
                }

                notifyIcon.Text = str.ToString();
            }
        }

        private void savePreferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_watcher == null || !m_credentials.IsValid)
            {
                MessageBox.Show("Connect to Jenkins before trying to save the config.");
                return;
            }

            Serialise();
            MessageBox.Show("Saved config.");
        }

        private void loadPreferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_watcher.BuildGroups.Clear();

            if (Deserialise())
            {
                UpdateBuildGroupListView();
                UpdateBuildListView();
            }
        }

        private void openPreferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PreferencesForm preferences = new PreferencesForm();
            preferences.ShowDialog(this);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm about = new AboutForm();
            about.ShowDialog(this);
        }

        private void cclbBuildList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ColouredCheckedListBox cclb = (ColouredCheckedListBox)sender;
            if (cclb == null)
            {
                return;
            }

            string item = (string)cclb.SelectedItem;
            if (item == null)
            {
                return;
            }

            JenkinsBuildGroup buildGroup = SelectedBuildGroup;
            if (buildGroup == null)
            {
                return;
            }

            if (buildGroup.Contains(item))
            {
                buildGroup.RemoveBuild(item);
            }
            else
            {
                buildGroup.AddBuild(item);
            }

            UpdateBuildListView();
            UpdateBuildGroupListView();
            UpdateBuildGroupStatus();
        }

        private void cclbBuildGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateBuildListView();
            UpdateBuildGroupStatus();
        }

        private void cclbBuildGroups_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (m_watcher == null)
            {
                return;
            }

            string name = SelectedBuildGroupName;
            if (name == null)
            {
                return;
            }

            JenkinsBuildGroup group = m_watcher.GetBuildGroup(name);
            if (group == null)
            {
                return;
            }

            group.IsMonitored = !group.IsMonitored;

            this.BeginInvoke((MethodInvoker)(() => UpdateBuildGroupStatus()));
        }

        private void cclbBuildGroups_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }

            CheckedListBox clb = (CheckedListBox)sender;
            if (clb == null)
            {
                return;
            }

            if (clb.SelectedItem == null)
            {
                return;
            }

            if (e.KeyCode == Keys.Enter)
            {
                // Toggle checked for selected item.
                bool isChecked = clb.GetItemChecked(clb.SelectedIndex);
                clb.SetItemChecked(clb.SelectedIndex, !isChecked);

                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                // Delete selected group.
                if (m_watcher == null)
                {
                    return;
                }

                string groupName = SelectedBuildGroupName;
                if (groupName != null)
                {
                    m_watcher.DeleteBuildGroup(groupName);

                    UpdateBuildGroupListView();
                    UpdateBuildListView();
                }

                e.Handled = true;
            }
        }

		private void DeleteAllBuildGroups()
		{
			if (m_watcher == null)
			{
				return;
			}

			m_watcher.ResetBuildGroups();

			UpdateBuildListView();
			UpdateBuildGroupListView();
			UpdateBuildGroupStatus();
		}

		private void btnViewBrokenBuilds_Click(object sender, EventArgs e)
		{
            // Open a URL for each watched, broken build channel.
            List<JenkinsBuildChannel> builds = MonitoredBuilds;
            foreach (JenkinsBuildChannel build in builds)
            {
                if (!build.IsGreen)
                {
                    build.OpenUrlLastFailed();
                }
            }
        }

		private void btnNotMe_Click(object sender, EventArgs e)
		{
            ((Button)sender).Visible = false;
            m_muteBlameNotifications = true;
		}
	}
}
