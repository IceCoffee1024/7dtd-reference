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
	// Token: 0x02001800 RID: 6144
	public class TwitchAuthentication
	{
		// Token: 0x1400010B RID: 267
		// (add) Token: 0x0600BE2B RID: 48683 RVA: 0x00465A14 File Offset: 0x00463C14
		// (remove) Token: 0x0600BE2C RID: 48684 RVA: 0x00465A48 File Offset: 0x00463C48
		public static event TwitchAuth_QRCodeGenerated QRCodeGenerated;

		// Token: 0x0600BE2D RID: 48685 RVA: 0x00465A7C File Offset: 0x00463C7C
		public TwitchAuthentication()
		{
			this.TwitchListener = new HttpListener();
			this.TwitchListener.Prefixes.Add("http://localhost:56207/");
			this.TwitchListener.Prefixes.Add("http://localhost:56207/auth/");
		}

		// Token: 0x0600BE2E RID: 48686 RVA: 0x00465AFB File Offset: 0x00463CFB
		public Task<TwitchAuthentication.AuthenticationValues> GetAuthenticationValuesAsync()
		{
			return Task.Run<TwitchAuthentication.AuthenticationValues>(() => this.GetAuthenticationValues());
		}

		// Token: 0x0600BE2F RID: 48687 RVA: 0x00465B10 File Offset: 0x00463D10
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

		// Token: 0x0600BE30 RID: 48688 RVA: 0x00465B50 File Offset: 0x00463D50
		public void StopListener()
		{
			this.TwitchListener.Stop();
		}

		// Token: 0x0600BE31 RID: 48689 RVA: 0x00465B60 File Offset: 0x00463D60
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

		// Token: 0x0600BE32 RID: 48690 RVA: 0x00465D8C File Offset: 0x00463F8C
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

		// Token: 0x0600BE33 RID: 48691 RVA: 0x00465E0C File Offset: 0x0046400C
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

		// Token: 0x0600BE34 RID: 48692 RVA: 0x004660B7 File Offset: 0x004642B7
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchAuthentication.AuthenticationValues GetModel(string token, string scopes)
		{
			return new TwitchAuthentication.AuthenticationValues
			{
				Token = token,
				Scopes = scopes
			};
		}

		// Token: 0x0600BE35 RID: 48693 RVA: 0x004660CC File Offset: 0x004642CC
		public void SendRequestToBrowser(string ClientID)
		{
			string url = TwitchAuthentication.GetUrl(ClientID);
			new Uri(url);
			Process.Start(url);
		}

		// Token: 0x0600BE36 RID: 48694 RVA: 0x004660E4 File Offset: 0x004642E4
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

		// Token: 0x0600BE37 RID: 48695 RVA: 0x0046615C File Offset: 0x0046435C
		public void GetToken()
		{
			GameManager.Instance.StartCoroutine(this.DeviceAuthenticate());
		}

		// Token: 0x0600BE38 RID: 48696 RVA: 0x0046616F File Offset: 0x0046436F
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

		// Token: 0x0600BE39 RID: 48697 RVA: 0x0046617E File Offset: 0x0046437E
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

		// Token: 0x0600BE3A RID: 48698 RVA: 0x0046619B File Offset: 0x0046439B
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

		// Token: 0x0600BE3B RID: 48699 RVA: 0x004661AA File Offset: 0x004643AA
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

		// Token: 0x0600BE3C RID: 48700 RVA: 0x004661C0 File Offset: 0x004643C0
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

		// Token: 0x04008F1A RID: 36634
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HttpListener TwitchListener;

		// Token: 0x04008F1B RID: 36635
		[PublicizedFrom(EAccessModifier.Private)]
		public const string ReturnUrl = "http://localhost:56207";

		// Token: 0x04008F1C RID: 36636
		public string oauth = "";

		// Token: 0x04008F1D RID: 36637
		public string oauthwithHeader = "";

		// Token: 0x04008F1E RID: 36638
		public string userName = "";

		// Token: 0x04008F1F RID: 36639
		public string userID = "";

		// Token: 0x04008F20 RID: 36640
		[PublicizedFrom(EAccessModifier.Private)]
		public string refresh_token = "";

		// Token: 0x04008F21 RID: 36641
		public static bool bFirstLogin = true;

		// Token: 0x04008F22 RID: 36642
		public bool completed;

		// Token: 0x04008F24 RID: 36644
		public static string client_id = "qtex0oyygtptfirybmxqxm27cdcdpf";

		// Token: 0x02001801 RID: 6145
		public class AuthenticationValues
		{
			// Token: 0x04008F25 RID: 36645
			public string Token;

			// Token: 0x04008F26 RID: 36646
			public string Scopes;
		}

		// Token: 0x02001802 RID: 6146
		[PublicizedFrom(EAccessModifier.Private)]
		[Serializable]
		public class ValidationResponseData
		{
			// Token: 0x04008F27 RID: 36647
			public string login;

			// Token: 0x04008F28 RID: 36648
			public string user_id;
		}

		// Token: 0x02001803 RID: 6147
		[PublicizedFrom(EAccessModifier.Private)]
		[Serializable]
		public class DeviceAuthIntitationData
		{
			// Token: 0x04008F29 RID: 36649
			public string device_code;

			// Token: 0x04008F2A RID: 36650
			public int expires_in;

			// Token: 0x04008F2B RID: 36651
			public int interval;

			// Token: 0x04008F2C RID: 36652
			public string user_code;

			// Token: 0x04008F2D RID: 36653
			public string verification_uri;
		}

		// Token: 0x02001804 RID: 6148
		[PublicizedFrom(EAccessModifier.Private)]
		[Serializable]
		public class PollingIncompleteResponse
		{
			// Token: 0x04008F2E RID: 36654
			public string message;
		}

		// Token: 0x02001805 RID: 6149
		[PublicizedFrom(EAccessModifier.Private)]
		[Serializable]
		public class PollingSuccessResponse
		{
			// Token: 0x04008F2F RID: 36655
			public string access_token;

			// Token: 0x04008F30 RID: 36656
			public int expires_in;

			// Token: 0x04008F31 RID: 36657
			public string refresh_token;
		}
	}
}
