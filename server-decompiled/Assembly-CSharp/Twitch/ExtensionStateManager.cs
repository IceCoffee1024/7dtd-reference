using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Twitch
{
	// Token: 0x0200181A RID: 6170
	public class ExtensionStateManager
	{
		// Token: 0x0600BF2C RID: 48940 RVA: 0x00467D24 File Offset: 0x00465F24
		public void Init()
		{
			this.userId = TwitchManager.Current.Authentication.userID;
			this.gettingJWT = true;
			GameManager.Instance.StartCoroutine(this.GetJWT(TwitchManager.Current.Authentication.oauth.Substring(6)));
			this.ecm = new ExtensionConfigManager();
			this.ecm.Init();
			this.epm = new ExtensionPubSubManager();
		}

		// Token: 0x0600BF2D RID: 48941 RVA: 0x00467D94 File Offset: 0x00465F94
		public void OnPartyChanged()
		{
			ExtensionConfigManager extensionConfigManager = this.ecm;
			if (extensionConfigManager == null)
			{
				return;
			}
			extensionConfigManager.OnPartyChanged();
		}

		// Token: 0x0600BF2E RID: 48942 RVA: 0x00467DA6 File Offset: 0x00465FA6
		public void PushUserBalance(ValueTuple<string, int> userBalance)
		{
			ExtensionPubSubManager extensionPubSubManager = this.epm;
			if (extensionPubSubManager == null)
			{
				return;
			}
			extensionPubSubManager.PushUserBalance(userBalance);
		}

		// Token: 0x0600BF2F RID: 48943 RVA: 0x00467DB9 File Offset: 0x00465FB9
		public void PushViewerChatState(string id, bool hasChatted)
		{
			ExtensionPubSubManager extensionPubSubManager = this.epm;
			if (extensionPubSubManager == null)
			{
				return;
			}
			extensionPubSubManager.PushViewerChatState(id, hasChatted);
		}

		// Token: 0x0600BF30 RID: 48944 RVA: 0x00467DCD File Offset: 0x00465FCD
		public bool CanUseBitCommands()
		{
			return this.ecm.CanUseBitCommands();
		}

		// Token: 0x0600BF31 RID: 48945 RVA: 0x00467DDC File Offset: 0x00465FDC
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

		// Token: 0x0600BF32 RID: 48946 RVA: 0x00467E79 File Offset: 0x00466079
		public void RetrieveJWT()
		{
			GameManager.Instance.StartCoroutine(this.GetJWT(TwitchManager.Current.Authentication.oauth.Substring(6)));
		}

		// Token: 0x0600BF33 RID: 48947 RVA: 0x00467EA1 File Offset: 0x004660A1
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

		// Token: 0x0600BF34 RID: 48948 RVA: 0x00467EB7 File Offset: 0x004660B7
		public void Cleanup()
		{
			this.ecm.Cleanup();
			this.ecm = null;
			this.epm = null;
		}

		// Token: 0x04008F0D RID: 36621
		[PublicizedFrom(EAccessModifier.Private)]
		public string userId;

		// Token: 0x04008F0E RID: 36622
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastUpdate = Time.realtimeSinceStartup;

		// Token: 0x04008F0F RID: 36623
		[PublicizedFrom(EAccessModifier.Private)]
		public ExtensionConfigManager ecm;

		// Token: 0x04008F10 RID: 36624
		[PublicizedFrom(EAccessModifier.Private)]
		public ExtensionPubSubManager epm;

		// Token: 0x04008F11 RID: 36625
		[PublicizedFrom(EAccessModifier.Private)]
		public string jwt = string.Empty;

		// Token: 0x04008F12 RID: 36626
		[PublicizedFrom(EAccessModifier.Private)]
		public long jwtRefreshTime;

		// Token: 0x04008F13 RID: 36627
		[PublicizedFrom(EAccessModifier.Private)]
		public bool gettingJWT;
	}
}
