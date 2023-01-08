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
    static public class HttpHelper
    {
		//static public HttpClient BuildHttpClient(string _url)
		//{
		//	//string clientParams = "?namespace=profile-" + _region + "&locale=en_US&access_token=" + g_accessToken;
		//	//_covenantId = "-1";
		//	//
		//	//// Construct the URL.
		//	//StringBuilder url = new StringBuilder("https://");
		//	//url.Append(_region);
		//	//url.Append(".api.blizzard.com/profile/wow/character/");
		//	//url.Append(_realm);
		//	//url.Append("/");
		//	//url.Append(_characterName.ToLower());

		//	//HttpClient client = new HttpClient()
		//	//{
		//	//	BaseAddress = new Uri(_url)
		//	//};
		//	//
		//	//client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

		//	string username = "ed.willoughby";
		//	string password = "054fG7JMA2";
		//	byte[] bytes = Encoding.ASCII.GetBytes(username + ":" + password);

		//	HttpClient client = new HttpClient { Timeout = Timeout.InfiniteTimeSpan };
		//	client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));

		//	return client;
		//}

		static public string BuildClientParams(List<Tuple<string, string>> _params)
		{
			StringBuilder clientParams = new StringBuilder();
			for (int i = 0; i < _params.Count; ++i)
			{
				char separator = '&';
				if (i == 0)
				{
					separator = '?';
				}

				Tuple<string, string> arg = _params[i];
				clientParams.AppendFormat("%s%s=%s", separator, arg.Item1, arg.Item2);
			}

			return clientParams.ToString();
		}

		static public HttpStatusCode ClientRequest(string _url, ref string _jsonResponse)
		{
			throw new NotImplementedException();

			string username = "";
			string password = "";
			byte[] bytes = Encoding.ASCII.GetBytes(username + ":" + password);

			HttpClient client = new HttpClient { Timeout = Timeout.InfiniteTimeSpan };
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));

			// Interpret response.
			HttpResponseMessage response = null;
			_jsonResponse = null;
			try
			{
				Logger.Log(Logger.LogLevel.Spam, "[Client Request] Calling GetAsync to {0}.", _url);
				var r = client.GetAsync(_url + "/crumbIssuer/api/xml?xpath=concat(//crumbRequestField,\":\",//crumb)");
				r.Wait();
				Logger.Log(Logger.LogLevel.Spam, "[Client Request] Finished GetAsync to {0}.", _url);
				response = r.Result;
			}
			catch (Exception e)  // HttpRequestException, SocketException
			{
				Logger.Log(Logger.LogLevel.Warning, "Request failed for {0}: {1}", _url, e.Message);
				client.Dispose();
				return HttpStatusCode.Forbidden;
			}

			if (response == null)
			{
				Logger.Log(Logger.LogLevel.Warning, "Request succeeded but response was null for {0}.", _url);
				client.Dispose();
				return HttpStatusCode.PartialContent;
			}
			else if (response.IsSuccessStatusCode)
			{
				Logger.Log(Logger.LogLevel.Spam, "[Client Request] Getting ReadAsStringAsync result from {0}.", _url);
				var r = response.Content.ReadAsStringAsync();
				r.Wait();
				Logger.Log(Logger.LogLevel.Spam, "[Client Request] Completed result from {0} as \n{1}", _url, _jsonResponse);
				_jsonResponse = r.Result;
				Logger.Log(Logger.LogLevel.Spam, "[Client Request] Setting result from {0} as \n{1}", _url, _jsonResponse);
				//string jsonResult = response.Content.ReadAsStringAsync().Result;
				//if (jsonResult != null)
				//{
				//	try
				//	{
				//		string covProg = Utilities.Find(JsonDocument.Parse(jsonResult).RootElement, "covenant_progress");
				//		if (covProg != null)
				//		{
				//			string covChosen = Utilities.Find(JsonDocument.Parse(covProg).RootElement, "chosen_covenant");
				//			if (covChosen != null)
				//			{
				//				_covenantId = Utilities.Find(JsonDocument.Parse(covChosen).RootElement, "id");
				//			}
				//		}

				//		if (_covenantId == "-1")
				//		{
				//			Logger.Log(Logger.LogLevel.Info, "Request for {0}-{1} returned no covenant data.", _characterName, _realm);
				//		}
				//	}
				//	catch (JsonException e)
				//	{
				//		Logger.Log(Logger.LogLevel.Warning, "Failed to parse json for {0}-{1}: {2}\n{3}", _characterName, _realm, e.Message, jsonResult);
				//		client.Dispose();
				//		return HttpStatusCode.PartialContent;
				//	}
				//}
			}
			else
			{
				Logger.Log(Logger.LogLevel.Spam, "[Client Request] Failed request to {0} with code {1}.", _url, response.StatusCode.ToString());
				Logger.LogLevel level = Logger.LogLevel.Error;
				if (response.StatusCode == HttpStatusCode.NotFound)
				{
					level = Logger.LogLevel.Info;
				}

				Logger.Log(level, "Failed request {0} with reason: {1} ({2})",
					_url, response.ReasonPhrase, (int)response.StatusCode);
			}
			client.Dispose();

			return response.StatusCode;
		}

		//static private string ClientGet(string _url)
		//{
		//	Logger.Log(Logger.LogLevel.Spam, "[ClientGet] Called with url {0}.", _url);
		//	HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
		//	Logger.Log(Logger.LogLevel.Spam, "[ClientGet] Created request.");
		//	HttpWebResponse httpResponse = null;
		//	try
		//	{
		//		httpResponse = (HttpWebResponse)request.GetResponse();
		//	}
		//	catch (Exception e)
		//	{
		//		Logger.Log(Logger.LogLevel.Spam, "[ClientGet] Failed to get response: {0}", e.Message);
		//	}
		//	Logger.Log(Logger.LogLevel.Spam, "[ClientGet] Got response.");
		//	System.IO.Stream resStream = httpResponse.GetResponseStream();
		//	Logger.Log(Logger.LogLevel.Spam, "[ClientGet] Got response stream.");
		//	System.IO.StreamReader reader = new System.IO.StreamReader(resStream);
		//	Logger.Log(Logger.LogLevel.Spam, "[ClientGet] Got stream reader.");
		//	string responseString = reader.ReadToEnd();
		//	Logger.Log(Logger.LogLevel.Spam, "[ClientGet] Read to end.");
		//	resStream.Close();
		//	reader.Close();

		//	Logger.Log(Logger.LogLevel.Spam, "[ClientGet] Returning response:\n{0}.", responseString);
		//	return responseString;
		//}


		public delegate void HttpRequestCallback(HttpStatusCode _status, string _errorReason, string _jsonResponse);
		static public void HttpRequest(string _username, string _password, string _url, HttpRequestCallback _callback)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(_username + ":" + _password);

			HttpClient client = new HttpClient { Timeout = Timeout.InfiniteTimeSpan };
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));

			// Interpret response.
			HttpResponseMessage response = null;
			string jsonResponse = null;
			try
			{
				Logger.Log(Logger.LogLevel.Spam, "Calling GetASync {0} at {1}.", _username, _url);
				Task<HttpResponseMessage> r = client.GetAsync(_url + "/crumbIssuer/api/xml?xpath=concat(//crumbRequestField,\":\",//crumb)");
				r.Wait();
				Logger.Log(Logger.LogLevel.Spam, "Finished GetAsync for {0} to {1}.", _username, _url);
				response = r.Result;
			}
			catch (Exception e)  // HttpRequestException, SocketException
			{
				Logger.Log(Logger.LogLevel.Warning, "Request failed for {0}: {1}", _url, e.Message);
				client.Dispose();
				if (_callback != null)
				{
					_callback(HttpStatusCode.InternalServerError, e.Message, null);
				}
				return;
			}

			HttpStatusCode codeResponse = HttpStatusCode.OK;
			string errorReason = null;
			if (response == null)
			{
				Logger.Log(Logger.LogLevel.Warning, "Request succeeded but response was null for {0}.", _url);
				codeResponse = HttpStatusCode.PartialContent;
				errorReason = "Failed to get a response from URL.";
			}
			else if (response.IsSuccessStatusCode)
			{
				Task<string> r = response.Content.ReadAsStringAsync();
				r.Wait();
				codeResponse = response.StatusCode;
				jsonResponse = r.Result;
			}
			else
			{
				Logger.LogLevel level = Logger.LogLevel.Error;
				if (response.StatusCode == HttpStatusCode.NotFound)
				{
					level = Logger.LogLevel.Info;
				}

				Logger.Log(level, "Failed request {0} with reason: {1} ({2})",
					_url, response.ReasonPhrase, response.StatusCode.ToString());

				codeResponse = response.StatusCode;
				errorReason = response.ReasonPhrase;
			}

			client.Dispose();
			if (_callback != null)
			{
				_callback(codeResponse, errorReason, jsonResponse);
			}
		}
	}
}
