using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Twitch
{
	// Token: 0x020017EE RID: 6126
	public class ExtensionStateManager
	{
		// Token: 0x0600BDD4 RID: 48596 RVA: 0x004637E8 File Offset: 0x004619E8
		public void Init()
		{
			this.userId = TwitchManager.Current.Authentication.userID;
			this.gettingJWT = true;
			GameManager.Instance.StartCoroutine(this.GetJWT(TwitchManager.Current.Authentication.oauth.Substring(6)));
			this.ecm = new ExtensionConfigManager();
			this.ecm.Init();
			this.epm = new ExtensionPubSubManager();
		}

		// Token: 0x0600BDD5 RID: 48597 RVA: 0x00463858 File Offset: 0x00461A58
		public void OnPartyChanged()
		{
			ExtensionConfigManager extensionConfigManager = this.ecm;
			if (extensionConfigManager == null)
			{
				return;
			}
			extensionConfigManager.OnPartyChanged();
		}

		// Token: 0x0600BDD6 RID: 48598 RVA: 0x0046386A File Offset: 0x00461A6A
		public void PushUserBalance(ValueTuple<string, int> userBalance)
		{
			ExtensionPubSubManager extensionPubSubManager = this.epm;
			if (extensionPubSubManager == null)
			{
				return;
			}
			extensionPubSubManager.PushUserBalance(userBalance);
		}

		// Token: 0x0600BDD7 RID: 48599 RVA: 0x0046387D File Offset: 0x00461A7D
		public void PushViewerChatState(string id, bool hasChatted)
		{
			ExtensionPubSubManager extensionPubSubManager = this.epm;
			if (extensionPubSubManager == null)
			{
				return;
			}
			extensionPubSubManager.PushViewerChatState(id, hasChatted);
		}

		// Token: 0x0600BDD8 RID: 48600 RVA: 0x00463891 File Offset: 0x00461A91
		public bool CanUseBitCommands()
		{
			return this.ecm.CanUseBitCommands();
		}

		// Token: 0x0600BDD9 RID: 48601 RVA: 0x004638A0 File Offset: 0x00461AA0
		public void Update()
		{
			if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() > this.jwtRefreshTime && !this.gettingJWT)
			{
				this.gettingJWT = true;
				GameManager.Instance.StartCoroutine(this.GetJWT(TwitchManager.Current.Authentication.oauth.Substring(6)));
			}
			if (this.jwt != string.Empty && Time.realtimeSinceStartup - this.lastUpdate >= 1f)
			{
				this.epm.Update(this.ecm.UpdatedConfig());
				this.lastUpdate = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x0600BDDA RID: 48602 RVA: 0x0046393D File Offset: 0x00461B3D
		public void RetrieveJWT()
		{
			GameManager.Instance.StartCoroutine(this.GetJWT(TwitchManager.Current.Authentication.oauth.Substring(6)));
		}

		// Token: 0x0600BDDB RID: 48603 RVA: 0x00463965 File Offset: 0x00461B65
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator GetJWT(string token)
		{
			using (UnityWebRequest req = UnityWebRequest.Get("https://2v3d0ewjcg.execute-api.us-east-1.amazonaws.com/prod/jwt/broadcaster"))
			{
				req.SetRequestHeader("Authorization", this.userId + " " + token);
				yield return req.SendWebRequest();
				if (req.result != UnityWebRequest.Result.Success)
				{
					Log.Warning(string.Format("Could not retrieve JWT: {0}", req.result));
				}
				else
				{
					try
					{
						JObject jobject = JObject.Parse(req.downloadHandler.text);
						if (jobject != null)
						{
							JToken jtoken;
							if (jobject.TryGetValue("token", out jtoken))
							{
								this.jwt = jtoken.ToString();
								this.epm.SetJWT(this.jwt);
								Log.Out("received jwt");
							}
							else
							{
								Log.Warning("Could not parse JWT in message body");
							}
							JToken jtoken2;
							if (jobject.TryGetValue("refreshTime", out jtoken2))
							{
								this.jwtRefreshTime = long.Parse(jtoken2.ToString());
								Log.Out(string.Format("will refresh jwt at {0}", this.jwtRefreshTime));
							}
						}
					}
					catch (Exception ex)
					{
						Log.Warning(ex.Message);
					}
				}
			}
			UnityWebRequest req = null;
			this.gettingJWT = false;
			yield break;
			yield break;
		}

		// Token: 0x0600BDDC RID: 48604 RVA: 0x0046397B File Offset: 0x00461B7B
		public void Cleanup()
		{
			this.ecm.Cleanup();
			this.ecm = null;
			this.epm = null;
		}

		// Token: 0x04008E47 RID: 36423
		[PublicizedFrom(EAccessModifier.Private)]
		public string userId;

		// Token: 0x04008E48 RID: 36424
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastUpdate = Time.realtimeSinceStartup;

		// Token: 0x04008E49 RID: 36425
		[PublicizedFrom(EAccessModifier.Private)]
		public ExtensionConfigManager ecm;

		// Token: 0x04008E4A RID: 36426
		[PublicizedFrom(EAccessModifier.Private)]
		public ExtensionPubSubManager epm;

		// Token: 0x04008E4B RID: 36427
		[PublicizedFrom(EAccessModifier.Private)]
		public string jwt = string.Empty;

		// Token: 0x04008E4C RID: 36428
		[PublicizedFrom(EAccessModifier.Private)]
		public long jwtRefreshTime;

		// Token: 0x04008E4D RID: 36429
		[PublicizedFrom(EAccessModifier.Private)]
		public bool gettingJWT;
	}
}
