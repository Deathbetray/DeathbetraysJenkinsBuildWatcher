using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DeathbetraysJenkinsBuildWatcher
{
    public class Preferences
    {
        static public Preferences Prefs
        {
            get { return s_prefs; }
            set { s_prefs = value; }
        }
        static Preferences s_prefs = new Preferences();



        public int TimerInterval
        {
            get { return m_timerInterval; }
            set
            {
                if (value != m_timerInterval)
                {
                    if (OnTimerIntervalChangedHandler != null)
                    {
                        OnTimerIntervalChangedHandler(m_timerInterval, value);
                    }
                    m_timerInterval = value;
                }
            }
        }
        public delegate void OnTimerIntervalChanged(int _prevValue, int _newValue);
        public OnTimerIntervalChanged OnTimerIntervalChangedHandler;
        private int m_timerInterval = 0;

        public bool EnableLogging
        {
            get { return m_enableLogging; }
            set
            {
                if (value != m_enableLogging)
                {
                    if (OnEnableLoggingChangedHandler != null)
                    {
                        OnEnableLoggingChangedHandler(m_enableLogging, value);
                    }
                    m_enableLogging = value;
                }
            }
        }
        public delegate void OnEnableLoggingChanged(bool _prevValue, bool _newValue);
        public OnEnableLoggingChanged OnEnableLoggingChangedHandler;
        private bool m_enableLogging = true;

        public bool EnableLoggingVerbose
        {
            get { return m_enableLoggingVerbose; }
            set
            {
                if (value != m_enableLoggingVerbose)
                {
                    if (OnEnableLoggingVerboseChangedHandler != null)
                    {
                        OnEnableLoggingVerboseChangedHandler(m_enableLoggingVerbose, value);
                    }
                    m_enableLoggingVerbose = value;
                }
            }
        }
        public delegate void OnEnableLoggingVerboseChanged(bool _prevValue, bool _newValue);
        public OnEnableLoggingVerboseChanged OnEnableLoggingVerboseChangedHandler;
        private bool m_enableLoggingVerbose = true;

        public bool EnableBuildBreakerNotifications
        {
            get { return m_enableBuildBreakerNotifications; }
            set
            {
                if (value != m_enableBuildBreakerNotifications)
                {
                    if (OnEnableBuildBreakerNotificationsChangedHandler != null)
                    {
                        OnEnableBuildBreakerNotificationsChangedHandler(m_enableBuildBreakerNotifications, value);
                    }
                    m_enableBuildBreakerNotifications = value;
                }
            }
        }
        public delegate void OnEnableBuildBreakerNotificationsChanged(bool _prevValue, bool _newValue);
        public OnEnableBuildBreakerNotificationsChanged OnEnableBuildBreakerNotificationsChangedHandler;
        private bool m_enableBuildBreakerNotifications = true;

        // Public function to create a Preferences object (so it can't be done accidentely).
        static public Preferences CreateInstance()
        {
            return new Preferences();
        }

        public void Copy(Preferences _prefs)
        {
            TimerInterval = _prefs.TimerInterval;

            EnableLogging = _prefs.EnableLogging;
            EnableLoggingVerbose = _prefs.EnableLoggingVerbose;

            EnableBuildBreakerNotifications = _prefs.EnableBuildBreakerNotifications;
        }

        // Private constructor: there should be one static instance that everything uses and any
        // temporary instances should only really exist when changing preferences.
        private Preferences()
        {
            ResetToDefaults();
        }

        public void ResetToDefaults()
        {
            TimerInterval = 60 * 1000;  // one minute

            EnableLogging = true;
            EnableLoggingVerbose = true;
        }

        public void Serialise(XmlWriter _writer)
        {
            _writer.WriteStartElement("Preferences");

            _writer.WriteStartElement("TimerInterval");
            _writer.WriteAttributeString("ms", TimerInterval.ToString());
            _writer.WriteEndElement();

            _writer.WriteStartElement("Logging");
            _writer.WriteAttributeString("enabled", EnableLogging.ToString());
            _writer.WriteAttributeString("verbose", EnableLoggingVerbose.ToString());
            _writer.WriteEndElement();

            _writer.WriteStartElement("Notifications");
            _writer.WriteAttributeString("buildBreakerNotifications", EnableBuildBreakerNotifications.ToString());
            _writer.WriteEndElement();

            _writer.WriteEndElement();
        }

        public void Deserialise(XmlNode _xml)
        {
            if (_xml.Name != "Preferences")
            {
                MessageBox.Show("Failed to load config due to missing Preferences element.");
                return;
            }

            foreach (XmlNode node in _xml.ChildNodes)
            {
                if (node.Name == "TimerInterval")
                {
                    TimerInterval = int.Parse(node.Attributes.GetNamedItem("ms").Value);
                }
                else if (node.Name == "Logging")
                {
                    EnableLogging = bool.Parse(node.Attributes.GetNamedItem("enabled").Value);
                    EnableLoggingVerbose = bool.Parse(node.Attributes.GetNamedItem("verbose").Value);
                }
                else if (node.Name == "Notifications")
                {
                    EnableBuildBreakerNotifications = bool.Parse(node.Attributes.GetNamedItem("buildBreakerNotifications").Value);
                }
                else
                {
                    MessageBox.Show("Unknown Preferences node: {0}", node.Name);
                }
            }
        }
    }
}
