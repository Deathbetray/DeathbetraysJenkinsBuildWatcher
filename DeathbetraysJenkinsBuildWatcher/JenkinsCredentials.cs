using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeathbetraysJenkinsBuildWatcher
{
    public class JenkinsCredentials
    {

        public string Username
        {
            get { return m_username; }
            private set { m_username = value; }
        }
        private string m_username = "";

        public string Password
        {
            get { return m_password; }
            private set { m_password = value; }
        }
        private string m_password = "";

        public string Url
        {
            get { return m_url; }
            private set { m_url = value; }
        }
        private string m_url = "";

        public bool IsValid
        {
            get { return m_valid; }
        }
        private bool m_valid = false;

        public string ErrorReason
        {
            get { return m_errorReason; }
            private set { m_errorReason = value; }
        }
        private string m_errorReason = "";

		private Thread m_validationRequest = null;


        private void OnValidationCompleteIgnore(JenkinsCredentials _credentials) { }
        public delegate void OnValidationComplete(JenkinsCredentials _credentials);
        public OnValidationComplete OnValidationCompleteHandler;



        public void Init(string _username, string _password, string _url)
        {
            OnValidationCompleteHandler += OnValidationCompleteIgnore;

            Username = _username;
            Password = _password;
			Url = _url;

            Validate();
        }

        public void Validate()
        {
			if (m_validationRequest == null)
			{
                m_valid = false;
				m_validationRequest = new Thread(ValidateThread);
				m_validationRequest.Start();
			}
			else
			{
				Logger.Log(Logger.LogLevel.Warning, "Attempted to validate credentials while a request was in flight.");
			}
		}

		private void ValidateThread()
		{
            HttpHelper.HttpRequest(Username, Password, Url, (HttpStatusCode _status, string _errorReason, string _jsonResponse) =>
            {
				if (_status == HttpStatusCode.OK)
				{
					Logger.Log(Logger.LogLevel.Info, "Successfully validated credentials for {0} to {1}.", Username, Url);
					m_valid = true;
				}
				else
				{
					Logger.Log(Logger.LogLevel.Info, "Failed to validate credentials for {0} to {1} with code {2} and reason {3}.",
						Username, Url, _status.ToString(), _errorReason);
					m_valid = false;
					ErrorReason = _errorReason;
				}

                OnValidationCompleteHandler(this);
			});

			m_validationRequest = null;
		}
	}
}
