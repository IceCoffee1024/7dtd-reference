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
	// Token: 0x0200185D RID: 6237
	public class TwitchAuthentication
	{
		// Token: 0x1400010A RID: 266
		// (add) Token: 0x0600C097 RID: 49303 RVA: 0x0046D8FC File Offset: 0x0046BAFC
		// (remove) Token: 0x0600C098 RID: 49304 RVA: 0x0046D930 File Offset: 0x0046BB30
		public static event TwitchAuth_QRCodeGenerated QRCodeGenerated;

		// Token: 0x0600C099 RID: 49305 RVA: 0x0046D964 File Offset: 0x0046BB64
		public TwitchAuthentication()
		{
			this.TwitchListener = new HttpListener();
			this.TwitchListener.Prefixes.Add("http://localhost:56207/");
			this.TwitchListener.Prefixes.Add("http://localhost:56207/auth/");
		}

		// Token: 0x0600C09A RID: 49306 RVA: 0x0046D9E3 File Offset: 0x0046BBE3
		public Task<TwitchAuthentication.AuthenticationValues> GetAuthenticationValuesAsync()
		{
			return Task.Run<TwitchAuthentication.AuthenticationValues>(() => this.GetAuthenticationValues());
		}

		// Token: 0x0600C09B RID: 49307 RVA: 0x0046D9F8 File Offset: 0x0046BBF8
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

		// Token: 0x0600C09C RID: 49308 RVA: 0x0046DA38 File Offset: 0x0046BC38
		public void StopListener()
		{
			this.TwitchListener.Stop();
		}

		// Token: 0x0600C09D RID: 49309 RVA: 0x0046DA48 File Offset: 0x0046BC48
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

		// Token: 0x0600C09E RID: 49310 RVA: 0x0046DC74 File Offset: 0x0046BE74
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

		// Token: 0x0600C09F RID: 49311 RVA: 0x0046DCF4 File Offset: 0x0046BEF4
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

		// Token: 0x0600C0A0 RID: 49312 RVA: 0x0046DF9F File Offset: 0x0046C19F
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchAuthentication.AuthenticationValues GetModel(string token, string scopes)
		{
			return new TwitchAuthentication.AuthenticationValues
			{
				Token = token,
				Scopes = scopes
			};
		}

		// Token: 0x0600C0A1 RID: 49313 RVA: 0x0046DFB4 File Offset: 0x0046C1B4
		public void SendRequestToBrowser(string ClientID)
		{
			string url = TwitchAuthentication.GetUrl(ClientID);
			new Uri(url);
			Process.Start(url);
		}

		// Token: 0x0600C0A2 RID: 49314 RVA: 0x0046DFCC File Offset: 0x0046C1CC
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

		// Token: 0x0600C0A3 RID: 49315 RVA: 0x0046E044 File Offset: 0x0046C244
		public void GetToken()
		{
			GameManager.Instance.StartCoroutine(this.DeviceAuthenticate());
		}

		// Token: 0x0600C0A4 RID: 49316 RVA: 0x0046E057 File Offset: 0x0046C257
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

		// Token: 0x0600C0A5 RID: 49317 RVA: 0x0046E066 File Offset: 0x0046C266
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

		// Token: 0x0600C0A6 RID: 49318 RVA: 0x0046E083 File Offset: 0x0046C283
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

		// Token: 0x0600C0A7 RID: 49319 RVA: 0x0046E092 File Offset: 0x0046C292
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

		// Token: 0x0600C0A8 RID: 49320 RVA: 0x0046E0A8 File Offset: 0x0046C2A8
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

		// Token: 0x040090C9 RID: 37065
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HttpListener TwitchListener;

		// Token: 0x040090CA RID: 37066
		[PublicizedFrom(EAccessModifier.Private)]
		public const string ReturnUrl = "http://localhost:56207";

		// Token: 0x040090CB RID: 37067
		public string oauth = "";

		// Token: 0x040090CC RID: 37068
		public string oauthwithHeader = "";

		// Token: 0x040090CD RID: 37069
		public string userName = "";

		// Token: 0x040090CE RID: 37070
		public string userID = "";

		// Token: 0x040090CF RID: 37071
		[PublicizedFrom(EAccessModifier.Private)]
		public string refresh_token = "";

		// Token: 0x040090D0 RID: 37072
		public static bool bFirstLogin = true;

		// Token: 0x040090D1 RID: 37073
		public bool completed;

		// Token: 0x040090D3 RID: 37075
		public static string client_id = "qtex0oyygtptfirybmxqxm27cdcdpf";

		// Token: 0x0200185E RID: 6238
		public class AuthenticationValues
		{
			// Token: 0x040090D4 RID: 37076
			public string Token;

			// Token: 0x040090D5 RID: 37077
			public string Scopes;
		}

		// Token: 0x0200185F RID: 6239
		[PublicizedFrom(EAccessModifier.Private)]
		[Serializable]
		public class ValidationResponseData
		{
			// Token: 0x040090D6 RID: 37078
			public string login;

			// Token: 0x040090D7 RID: 37079
			public string user_id;
		}

		// Token: 0x02001860 RID: 6240
		[PublicizedFrom(EAccessModifier.Private)]
		[Serializable]
		public class DeviceAuthIntitationData
		{
			// Token: 0x040090D8 RID: 37080
			public string device_code;

			// Token: 0x040090D9 RID: 37081
			public int expires_in;

			// Token: 0x040090DA RID: 37082
			public int interval;

			// Token: 0x040090DB RID: 37083
			public string user_code;

			// Token: 0x040090DC RID: 37084
			public string verification_uri;
		}

		// Token: 0x02001861 RID: 6241
		[PublicizedFrom(EAccessModifier.Private)]
		[Serializable]
		public class PollingIncompleteResponse
		{
			// Token: 0x040090DD RID: 37085
			public string message;
		}

		// Token: 0x02001862 RID: 6242
		[PublicizedFrom(EAccessModifier.Private)]
		[Serializable]
		public class PollingSuccessResponse
		{
			// Token: 0x040090DE RID: 37086
			public string access_token;

			// Token: 0x040090DF RID: 37087
			public int expires_in;

			// Token: 0x040090E0 RID: 37088
			public string refresh_token;
		}
	}
}
