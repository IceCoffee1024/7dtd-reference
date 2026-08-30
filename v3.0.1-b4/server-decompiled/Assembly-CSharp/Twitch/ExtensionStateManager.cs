using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Twitch
{
	// Token: 0x020017C8 RID: 6088
	public class ExtensionStateManager
	{
		// Token: 0x0600BC4F RID: 48207 RVA: 0x0045DA4C File Offset: 0x0045BC4C
		public void Init()
		{
			this.userId = TwitchManager.Current.Authentication.userID;
			this.gettingJWT = true;
			GameManager.Instance.StartCoroutine(this.GetJWT(TwitchManager.Current.Authentication.oauth.Substring(6)));
			this.ecm = new ExtensionConfigManager();
			this.ecm.Init();
			this.epm = new ExtensionPubSubManager();
		}

		// Token: 0x0600BC50 RID: 48208 RVA: 0x0045DABC File Offset: 0x0045BCBC
		public void OnPartyChanged()
		{
			ExtensionConfigManager extensionConfigManager = this.ecm;
			if (extensionConfigManager == null)
			{
				return;
			}
			extensionConfigManager.OnPartyChanged();
		}

		// Token: 0x0600BC51 RID: 48209 RVA: 0x0045DACE File Offset: 0x0045BCCE
		public void PushUserBalance(ValueTuple<string, int> userBalance)
		{
			ExtensionPubSubManager extensionPubSubManager = this.epm;
			if (extensionPubSubManager == null)
			{
				return;
			}
			extensionPubSubManager.PushUserBalance(userBalance);
		}

		// Token: 0x0600BC52 RID: 48210 RVA: 0x0045DAE1 File Offset: 0x0045BCE1
		public void PushViewerChatState(string id, bool hasChatted)
		{
			ExtensionPubSubManager extensionPubSubManager = this.epm;
			if (extensionPubSubManager == null)
			{
				return;
			}
			extensionPubSubManager.PushViewerChatState(id, hasChatted);
		}

		// Token: 0x0600BC53 RID: 48211 RVA: 0x0045DAF5 File Offset: 0x0045BCF5
		public bool CanUseBitCommands()
		{
			return this.ecm.CanUseBitCommands();
		}

		// Token: 0x0600BC54 RID: 48212 RVA: 0x0045DB04 File Offset: 0x0045BD04
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

		// Token: 0x0600BC55 RID: 48213 RVA: 0x0045DBA1 File Offset: 0x0045BDA1
		public void RetrieveJWT()
		{
			GameManager.Instance.StartCoroutine(this.GetJWT(TwitchManager.Current.Authentication.oauth.Substring(6)));
		}

		// Token: 0x0600BC56 RID: 48214 RVA: 0x0045DBC9 File Offset: 0x0045BDC9
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

		// Token: 0x0600BC57 RID: 48215 RVA: 0x0045DBDF File Offset: 0x0045BDDF
		public void Cleanup()
		{
			this.ecm.Cleanup();
			this.ecm = null;
			this.epm = null;
		}

		// Token: 0x04008D2B RID: 36139
		[PublicizedFrom(EAccessModifier.Private)]
		public string userId;

		// Token: 0x04008D2C RID: 36140
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastUpdate = Time.realtimeSinceStartup;

		// Token: 0x04008D2D RID: 36141
		[PublicizedFrom(EAccessModifier.Private)]
		public ExtensionConfigManager ecm;

		// Token: 0x04008D2E RID: 36142
		[PublicizedFrom(EAccessModifier.Private)]
		public ExtensionPubSubManager epm;

		// Token: 0x04008D2F RID: 36143
		[PublicizedFrom(EAccessModifier.Private)]
		public string jwt = string.Empty;

		// Token: 0x04008D30 RID: 36144
		[PublicizedFrom(EAccessModifier.Private)]
		public long jwtRefreshTime;

		// Token: 0x04008D31 RID: 36145
		[PublicizedFrom(EAccessModifier.Private)]
		public bool gettingJWT;
	}
}
