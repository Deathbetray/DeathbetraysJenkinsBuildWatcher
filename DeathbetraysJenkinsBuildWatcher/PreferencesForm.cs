using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeathbetraysJenkinsBuildWatcher
{
    public partial class PreferencesForm : Form
    {
        private Preferences m_preferences;


        public PreferencesForm()
        {
            InitializeComponent();

            m_preferences = Preferences.CreateInstance();
            m_preferences.Copy(Preferences.Prefs);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Preferences.Prefs.Copy(m_preferences);

            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PreferencesForm_Shown(object sender, EventArgs e)
        {
            CenterToParent();

            RefreshView();
        }

        private void RefreshView()
        {
            ApplyTimerInterval(m_preferences.TimerInterval);

            cbLogging.Checked = m_preferences.EnableLogging;
            cbLoggingVerbose.Checked = m_preferences.EnableLoggingVerbose;

            cbBuildBreakerNotifications.Checked = m_preferences.EnableBuildBreakerNotifications;
        }

        private void ApplyTimerInterval(int timeInMs)
        {
            int seconds = timeInMs / 1000;

            int minutes = seconds / 60;
            if (minutes * 60 != seconds)
            {
                cbTimerUnits.SelectedIndex = 0;
                tbTimer.Text = seconds.ToString();
                return;
            }

            int hours = minutes / 60;
            if (hours * 60 != minutes)
            {
                cbTimerUnits.SelectedIndex = 1;
                tbTimer.Text = minutes.ToString();
                return;
            }

            cbTimerUnits.SelectedIndex = 2;
            tbTimer.Text = hours.ToString();
        }

        private int GetTimerInterval()
        {
            int time = int.Parse(tbTimer.Text);
            if (cbTimerUnits.SelectedIndex == 0)
            {
                return time * 1000;
            }
            else if (cbTimerUnits.SelectedIndex == 1)
            {
                return time * 1000 * 60;
            }
            else if (cbTimerUnits.SelectedIndex == 2)
            {
                return time * 1000 * 60 * 60;
            }

            Debug.Assert(false, "Invalid timer units selection.");
            return 0;
        }

        private void cbTimerUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_preferences.TimerInterval = GetTimerInterval();
        }

        private void tbTimer_Validating(object sender, CancelEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            string text = tb.Text;
            foreach (char c in text)
            {
                if (!Char.IsNumber(c))
                {
                    e.Cancel = true;
                }
            }

            m_preferences.TimerInterval = GetTimerInterval();
        }

        private void cbLogging_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            m_preferences.EnableLogging = cb.Checked;
        }

        private void cbLoggingVerbose_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            m_preferences.EnableLoggingVerbose = cb.Checked;
        }

        private void btnDefaults_Click(object sender, EventArgs e)
        {
            m_preferences.ResetToDefaults();
            RefreshView();
        }

		private void cbBuildBreakerNotifications_CheckedChanged(object sender, EventArgs e)
		{
            CheckBox cb = (CheckBox)sender;
            m_preferences.EnableBuildBreakerNotifications = cb.Checked;
		}
	}
}
