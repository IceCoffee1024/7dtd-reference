using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using ZXing;
using ZXing.QrCode;

namespace Twitch
{
	// Token: 0x020017DA RID: 6106
	public class TwitchAuthentication
	{
		// Token: 0x1400010A RID: 266
		// (add) Token: 0x0600BCA6 RID: 48294 RVA: 0x0045FC78 File Offset: 0x0045DE78
		// (remove) Token: 0x0600BCA7 RID: 48295 RVA: 0x0045FCAC File Offset: 0x0045DEAC
		public static event TwitchAuth_QRCodeGenerated QRCodeGenerated;

		// Token: 0x0600BCA8 RID: 48296 RVA: 0x0045FCE0 File Offset: 0x0045DEE0
		public TwitchAuthentication()
		{
			this.TwitchListener = new HttpListener();
			this.TwitchListener.Prefixes.Add("http://localhost:56207/");
			this.TwitchListener.Prefixes.Add("http://localhost:56207/auth/");
		}

		// Token: 0x0600BCA9 RID: 48297 RVA: 0x0045FD5F File Offset: 0x0045DF5F
		public Task<TwitchAuthentication.AuthenticationValues> GetAuthenticationValuesAsync()
		{
			return Task.Run<TwitchAuthentication.AuthenticationValues>(() => this.GetAuthenticationValues());
		}

		// Token: 0x0600BCAA RID: 48298 RVA: 0x0045FD74 File Offset: 0x0045DF74
		[PublicizedFrom(EAccessModifier.Private)]
		public bool StartListener()
		{
			this.completed = false;
			try
			{
				this.TwitchListener.Start();
			}
			catch (HttpListenerException)
			{
			}
			return this.TwitchListener.IsListening;
		}

		// Token: 0x0600BCAB RID: 48299 RVA: 0x0045FDB4 File Offset: 0x0045DFB4
		public void StopListener()
		{
			this.TwitchListener.Stop();
		}

		// Token: 0x0600BCAC RID: 48300 RVA: 0x0045FDC4 File Offset: 0x0045DFC4
		public TwitchAuthentication.AuthenticationValues GetAuthenticationValues()
		{
			while (this.TwitchListener.IsListening)
			{
				HttpListenerContext httpListenerContext = null;
				try
				{
					httpListenerContext = this.TwitchListener.GetContext();
				}
				catch (HttpListenerException)
				{
					this.StopListener();
					Log.Warning("Could not get context on TwitchListener");
					return null;
				}
				if (!(httpListenerContext.Request.HttpMethod == "GET") || !(httpListenerContext.Request.Url.LocalPath == "/auth/"))
				{
					byte[] bytes = Encoding.UTF8.GetBytes(this.GetResponse());
					httpListenerContext.Response.StatusCode = 200;
					httpListenerContext.Response.KeepAlive = false;
					httpListenerContext.Response.ContentLength64 = (long)bytes.Length;
					httpListenerContext.Response.OutputStream.Write(bytes, 0, bytes.Length);
					httpListenerContext.Response.Close();
					continue;
				}
				string text = httpListenerContext.Request.QueryString.Get("scope");
				string text2 = httpListenerContext.Request.QueryString.Get("access_token");
				if (text == null || text2 == null)
				{
					httpListenerContext.Response.ContentType = "application/json";
					httpListenerContext.Response.StatusCode = 400;
					string text3 = JsonConvert.SerializeObject(new
					{
						message = "missing data"
					});
					byte[] bytes2 = Encoding.UTF8.GetBytes(text3);
					httpListenerContext.Response.ContentLength64 = (long)bytes2.Length;
					Stream outputStream = httpListenerContext.Response.OutputStream;
					outputStream.Write(bytes2, 0, bytes2.Length);
					outputStream.Close();
					continue;
				}
				TwitchAuthentication.AuthenticationValues model = this.GetModel(text2, text);
				httpListenerContext.Response.ContentType = "application/json";
				httpListenerContext.Response.StatusCode = 200;
				string text4 = JsonConvert.SerializeObject(new
				{
					message = "success"
				});
				byte[] bytes3 = Encoding.UTF8.GetBytes(text4);
				httpListenerContext.Response.ContentLength64 = (long)bytes3.Length;
				Stream outputStream2 = httpListenerContext.Response.OutputStream;
				outputStream2.Write(bytes3, 0, bytes3.Length);
				outputStream2.Close();
				this.StopListener();
				if (model != null)
				{
					return model;
				}
				Log.Warning("did not successfully get authentication values");
				return null;
			}
			return null;
		}

		// Token: 0x0600BCAD RID: 48301 RVA: 0x0045FFF0 File Offset: 0x0045E1F0
		public static NameValueCollection ParseQueryString(string s)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			if (s.Contains("?"))
			{
				s = s.Substring(s.IndexOf('?') + 2);
			}
			string[] array = Regex.Split(s, "&");
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = Regex.Split(array[i], "=");
				if (array2.Length == 2)
				{
					nameValueCollection.Add(array2[0], array2[1]);
				}
				else
				{
					nameValueCollection.Add(array2[0], string.Empty);
				}
			}
			return nameValueCollection;
		}

		// Token: 0x0600BCAE RID: 48302 RVA: 0x00460070 File Offset: 0x0045E270
		[PublicizedFrom(EAccessModifier.Private)]
		public string GetResponse()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<head>");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("<title>7 Days to Die Twitch Oauth</title>");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("</head>");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("<body>");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("<h1 id=\"status\">Authenticating...</h1>");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("</body>");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("<script language=\"JavaScript\">");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("if(window.location.hash) {");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("window.location.href = window.location.href.replace(\"/#\",\"?\");");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("}");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("const urlParams = new URLSearchParams(window.location.search);");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("const accessToken = urlParams.get('access_token');");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("const scope = urlParams.get('scope');");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("if (accessToken && scope) {");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("const url = `http://localhost:56207/auth/?access_token=${accessToken}&scope=${scope}`;");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("let attemptCount = 0;");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("const sendAuthValues = () => fetch(url, { method: 'GET' })");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append(".then(response => { if (response.ok) return response.json(); throw new Error('Network response was not ok.'); })");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append(".then(() => { document.getElementById('status').innerText = 'Success! You may now close this window.'; setTimeout(function() { window.close(); }, 2000);})");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append(".catch(() => { attemptCount++; if (attemptCount < 3) {setTimeout(sendAuthValues, 1000); return; } else { document.getElementById('status').innerText = 'authentication failed. Please try again.'; }});");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("sendAuthValues();");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("}");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("else {");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("document.getElementById('status').innerText = 'Authentication failed. Please try again.';");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("setTimeout(function () { window.close(); }, 2000);");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("}");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("</script>");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("</html>");
			return stringBuilder.ToString();
		}

		// Token: 0x0600BCAF RID: 48303 RVA: 0x0046031B File Offset: 0x0045E51B
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchAuthentication.AuthenticationValues GetModel(string token, string scopes)
		{
			return new TwitchAuthentication.AuthenticationValues
			{
				Token = token,
				Scopes = scopes
			};
		}

		// Token: 0x0600BCB0 RID: 48304 RVA: 0x00460330 File Offset: 0x0045E530
		public void SendRequestToBrowser(string ClientID)
		{
			string url = TwitchAuthentication.GetUrl(ClientID);
			new Uri(url);
			Process.Start(url);
		}

		// Token: 0x0600BCB1 RID: 48305 RVA: 0x00460348 File Offset: 0x0045E548
		[PublicizedFrom(EAccessModifier.Private)]
		public static string GetUrl(string ClientID)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("https://id.twitch.tv/oauth2/authorize");
			stringBuilder.Append("?response_type=token");
			stringBuilder.Append("&client_id=").Append(ClientID);
			stringBuilder.Append("&redirect_uri=").Append("http://localhost:56207");
			if (!TwitchAuthentication.bFirstLogin)
			{
				stringBuilder.Append("&force_verify=true");
			}
			stringBuilder.Append("&scope=channel_read+chat:read+chat:edit+user_blocks_read+channel_subscriptions+channel_check_subscription+channel:read:redemptions+channel:manage:redemptions+user_read+bits:read+channel:read:hype_train+channel:read:goals+user:read:broadcast");
			return stringBuilder.ToString();
		}

		// Token: 0x0600BCB2 RID: 48306 RVA: 0x004603C0 File Offset: 0x0045E5C0
		public void GetToken()
		{
			GameManager.Instance.StartCoroutine(this.DeviceAuthenticate());
		}

		// Token: 0x0600BCB3 RID: 48307 RVA: 0x004603D3 File Offset: 0x0045E5D3
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator DeviceAuthenticate()
		{
			Log.Out("packing data for device auth init request...");
			WWWForm wwwform = new WWWForm();
			wwwform.AddField("client_id", TwitchAuthentication.client_id);
			wwwform.AddField("scopes", "channel_read chat:read chat:edit user_blocks_read channel_subscriptions channel_check_subscription channel:read:redemptions channel:manage:redemptions user_read bits:read channel:read:hype_train channel:read:goals user:read:broadcast");
			string device_code = string.Empty;
			string text = string.Empty;
			string text2 = string.Empty;
			Log.Out("creating init request...");
			using (UnityWebRequest req = UnityWebRequest.Post("https://id.twitch.tv/oauth2/device", wwwform))
			{
				Log.Out("sending init request...");
				yield return req.SendWebRequest();
				if (req.result != UnityWebRequest.Result.Success)
				{
					Log.Warning("failed to initiate DCF Twitch OAuth endpoint with response: " + req.downloadHandler.text);
					this.completed = true;
					yield break;
				}
				TwitchAuthentication.DeviceAuthIntitationData deviceAuthIntitationData = JsonUtility.FromJson<TwitchAuthentication.DeviceAuthIntitationData>(req.downloadHandler.text);
				device_code = deviceAuthIntitationData.device_code;
				text2 = deviceAuthIntitationData.user_code;
				text = deviceAuthIntitationData.verification_uri;
			}
			UnityWebRequest req = null;
			Log.Out("successfully initialized auth!");
			Log.Out("generating qr code...");
			BarcodeWriter barcodeWriter = new BarcodeWriter
			{
				Format = BarcodeFormat.QR_CODE,
				Options = new QrCodeEncodingOptions
				{
					Height = 256,
					Width = 256,
					Margin = 1
				}
			};
			Texture2D texture2D = new Texture2D(256, 256);
			texture2D.SetPixels32(barcodeWriter.Write(text));
			texture2D.Apply();
			Log.Out("successfully generated qr code!");
			if (TwitchAuthentication.QRCodeGenerated != null)
			{
				TwitchAuthentication.QRCodeGenerated(texture2D, text2, text);
				Log.Out("successfully displayed qr code!");
				Application.OpenURL(text);
			}
			else
			{
				global::UnityEngine.Debug.LogWarning("No one listening for QR Code.");
			}
			bool keepPolling = true;
			bool retrievedToken = false;
			string token = string.Empty;
			Log.Out("initiating polling loop...");
			int pollLimit = 12;
			int pollCount = 0;
			while (keepPolling && pollCount < pollLimit)
			{
				Log.Out("packing data for token poll request");
				WWWForm pollData = new WWWForm();
				pollData.AddField("client_id", TwitchAuthentication.client_id);
				pollData.AddField("scopes", "channel_read chat:read chat:edit user_blocks_read channel_subscriptions channel_check_subscription channel:read:redemptions channel:manage:redemptions user_read bits:read channel:read:hype_train channel:read:goals user:read:broadcast");
				pollData.AddField("device_code", device_code);
				pollData.AddField("grant_type", "urn:ietf:params:oauth:grant-type:device_code");
				Log.Out("will poll token endpoint in 5s...");
				yield return new WaitForSecondsRealtime(5f);
				Log.Out("creating token poll request...");
				using (UnityWebRequest req = UnityWebRequest.Post("https://id.twitch.tv/oauth2/token", pollData))
				{
					Log.Out("sending token poll request");
					yield return req.SendWebRequest();
					if (req.result != UnityWebRequest.Result.Success)
					{
						TwitchAuthentication.PollingIncompleteResponse pollingIncompleteResponse = JsonUtility.FromJson<TwitchAuthentication.PollingIncompleteResponse>(req.downloadHandler.text);
						if (pollingIncompleteResponse.message != "authorization_pending")
						{
							Log.Warning("Failed to get device token with message: " + pollingIncompleteResponse.message);
							keepPolling = false;
						}
					}
					else
					{
						TwitchAuthentication.PollingSuccessResponse pollingSuccessResponse = JsonUtility.FromJson<TwitchAuthentication.PollingSuccessResponse>(req.downloadHandler.text);
						token = pollingSuccessResponse.access_token;
						this.oauth = "oauth:" + pollingSuccessResponse.access_token;
						this.refresh_token = pollingSuccessResponse.refresh_token;
						GameManager.Instance.StartCoroutine(this.RefreshToken((float)(pollingSuccessResponse.expires_in - 60), false));
						Log.Out("Successfully retrieved oauth token");
						retrievedToken = true;
						keepPolling = false;
					}
				}
				req = null;
				int num = pollCount;
				pollCount = num + 1;
				pollData = null;
			}
			if (!retrievedToken)
			{
				this.completed = true;
				yield break;
			}
			yield return this.ValidateToken(token);
			this.completed = true;
			Log.Out("authentication values have been received and validated!");
			yield break;
			yield break;
		}

		// Token: 0x0600BCB4 RID: 48308 RVA: 0x004603E2 File Offset: 0x0045E5E2
		public IEnumerator RefreshToken(float delay, bool skipNextRefresh = false)
		{
			yield return new WaitForSecondsRealtime(delay);
			Log.Out("invoking token refresh");
			this.oauth = "";
			string text = JsonConvert.SerializeObject(new
			{
				refreshToken = this.refresh_token
			});
			TwitchManager.Current.WaitForOAuth();
			using (UnityWebRequest req = UnityWebRequest.Post("https://hijg23d34qghpdonxdf6ytq63m0gezwl.lambda-url.us-east-1.on.aws/", text, "application/json"))
			{
				yield return req.SendWebRequest();
				if (req.result != UnityWebRequest.Result.Success)
				{
					Log.Warning("failed to refresh token: " + req.downloadHandler.text);
				}
				else
				{
					TwitchAuthentication.PollingSuccessResponse pollingSuccessResponse = JsonUtility.FromJson<TwitchAuthentication.PollingSuccessResponse>(req.downloadHandler.text);
					this.oauth = "oauth:" + pollingSuccessResponse.access_token;
					this.refresh_token = pollingSuccessResponse.refresh_token;
					Log.Out("Successfully refreshed oauth token");
					if (!skipNextRefresh)
					{
						GameManager.Instance.StartCoroutine(this.RefreshToken((float)(pollingSuccessResponse.expires_in - 60), false));
					}
				}
			}
			UnityWebRequest req = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BCB5 RID: 48309 RVA: 0x004603FF File Offset: 0x0045E5FF
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator Authenticate()
		{
			this.StartListener();
			if (!this.TwitchListener.IsListening)
			{
				Log.Warning("Twitch listener is not listening after start attempt");
				TwitchManager.Current.StopTwitchIntegration(TwitchManager.InitStates.Failed);
				yield break;
			}
			Task<TwitchAuthentication.AuthenticationValues> t = this.GetAuthenticationValuesAsync();
			yield return new WaitUntil(() => t.Status == TaskStatus.Running);
			this.SendRequestToBrowser(TwitchAuthentication.client_id);
			yield return new WaitUntil(() => t.IsCompletedSuccessfully || t.IsCompleted || t.IsCanceled || t.IsFaulted);
			TwitchAuthentication.AuthenticationValues result = t.Result;
			if (result == null)
			{
				Log.Warning("did not successfully retrieve auth values");
				TwitchManager.Current.StopTwitchIntegration(TwitchManager.InitStates.Failed);
				yield break;
			}
			this.oauth = "oauth:" + result.Token;
			yield return this.ValidateToken(result.Token);
			this.completed = true;
			Log.Out("authentication values have been received and validated!");
			yield break;
		}

		// Token: 0x0600BCB6 RID: 48310 RVA: 0x0046040E File Offset: 0x0045E60E
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator ValidateToken(string token)
		{
			Log.Out("creating token validation request");
			using (UnityWebRequest req = UnityWebRequest.Get("https://id.twitch.tv/oauth2/validate"))
			{
				req.SetRequestHeader("Authorization", "Bearer " + token);
				Log.Out("sending token validation request");
				yield return req.SendWebRequest();
				if (req.result != UnityWebRequest.Result.Success)
				{
					Log.Warning("failed to validate on Twitch oauth endpoint with response: " + req.downloadHandler.text);
				}
				else
				{
					try
					{
						Log.Out("token validated successfully!");
						TwitchAuthentication.ValidationResponseData validationResponseData = JsonUtility.FromJson<TwitchAuthentication.ValidationResponseData>(req.downloadHandler.text);
						this.userName = validationResponseData.login;
						this.userID = validationResponseData.user_id;
					}
					catch (Exception)
					{
						Log.Warning("failed to make use of Twitch validate response data: " + req.downloadHandler.text);
					}
				}
			}
			UnityWebRequest req = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BCB7 RID: 48311 RVA: 0x00460424 File Offset: 0x0045E624
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator RunGetAuthenticationValues(Task<TwitchAuthentication.AuthenticationValues> task)
		{
			if (task != null)
			{
				while (!task.IsCompleted)
				{
					yield return null;
				}
				if (task.IsFaulted)
				{
					throw task.Exception;
				}
				if (task.Result != null)
				{
					this.oauth = "oauth:" + task.Result.Token;
					using (UnityWebRequest req = UnityWebRequest.Get("https://id.twitch.tv/oauth2/validate"))
					{
						string token = task.Result.Token;
						req.SetRequestHeader("Authorization", "Bearer " + token);
						yield return req.SendWebRequest();
						if (req.result != UnityWebRequest.Result.Success)
						{
							Log.Warning("failed to validate on Twitch oauth endpoint with response: " + req.downloadHandler.text);
						}
						else
						{
							try
							{
								TwitchAuthentication.ValidationResponseData validationResponseData = JsonUtility.FromJson<TwitchAuthentication.ValidationResponseData>(req.downloadHandler.text);
								this.userName = validationResponseData.login;
								this.userID = validationResponseData.user_id;
							}
							catch (Exception)
							{
								Log.Warning("failed to make use of Twitch validate response data: " + req.downloadHandler.text);
							}
						}
					}
					UnityWebRequest req = null;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x04008DFE RID: 36350
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HttpListener TwitchListener;

		// Token: 0x04008DFF RID: 36351
		[PublicizedFrom(EAccessModifier.Private)]
		public const string ReturnUrl = "http://localhost:56207";

		// Token: 0x04008E00 RID: 36352
		public string oauth = "";

		// Token: 0x04008E01 RID: 36353
		public string oauthwithHeader = "";

		// Token: 0x04008E02 RID: 36354
		public string userName = "";

		// Token: 0x04008E03 RID: 36355
		public string userID = "";

		// Token: 0x04008E04 RID: 36356
		[PublicizedFrom(EAccessModifier.Private)]
		public string refresh_token = "";

		// Token: 0x04008E05 RID: 36357
		public static bool bFirstLogin = true;

		// Token: 0x04008E06 RID: 36358
		public bool completed;

		// Token: 0x04008E08 RID: 36360
		public static string client_id = "qtex0oyygtptfirybmxqxm27cdcdpf";

		// Token: 0x020017DB RID: 6107
		public class AuthenticationValues
		{
			// Token: 0x04008E09 RID: 36361
			public string Token;

			// Token: 0x04008E0A RID: 36362
			public string Scopes;
		}

		// Token: 0x020017DC RID: 6108
		[PublicizedFrom(EAccessModifier.Private)]
		[Serializable]
		public class ValidationResponseData
		{
			// Token: 0x04008E0B RID: 36363
			public string login;

			// Token: 0x04008E0C RID: 36364
			public string user_id;
		}

		// Token: 0x020017DD RID: 6109
		[PublicizedFrom(EAccessModifier.Private)]
		[Serializable]
		public class DeviceAuthIntitationData
		{
			// Token: 0x04008E0D RID: 36365
			public string device_code;

			// Token: 0x04008E0E RID: 36366
			public int expires_in;

			// Token: 0x04008E0F RID: 36367
			public int interval;

			// Token: 0x04008E10 RID: 36368
			public string user_code;

			// Token: 0x04008E11 RID: 36369
			public string verification_uri;
		}

		// Token: 0x020017DE RID: 6110
		[PublicizedFrom(EAccessModifier.Private)]
		[Serializable]
		public class PollingIncompleteResponse
		{
			// Token: 0x04008E12 RID: 36370
			public string message;
		}

		// Token: 0x020017DF RID: 6111
		[PublicizedFrom(EAccessModifier.Private)]
		[Serializable]
		public class PollingSuccessResponse
		{
			// Token: 0x04008E13 RID: 36371
			public string access_token;

			// Token: 0x04008E14 RID: 36372
			public int expires_in;

			// Token: 0x04008E15 RID: 36373
			public string refresh_token;
		}
	}
}
