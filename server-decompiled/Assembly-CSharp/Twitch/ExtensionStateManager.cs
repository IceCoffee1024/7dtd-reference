using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Twitch
{
	// Token: 0x0200184B RID: 6219
	public class ExtensionStateManager
	{
		// Token: 0x0600C040 RID: 49216 RVA: 0x0046B6D0 File Offset: 0x004698D0
		public void Init()
		{
			this.userId = TwitchManager.Current.Authentication.userID;
			this.gettingJWT = true;
			GameManager.Instance.StartCoroutine(this.GetJWT(TwitchManager.Current.Authentication.oauth.Substring(6)));
			this.ecm = new ExtensionConfigManager();
			this.ecm.Init();
			this.epm = new ExtensionPubSubManager();
		}

		// Token: 0x0600C041 RID: 49217 RVA: 0x0046B740 File Offset: 0x00469940
		public void OnPartyChanged()
		{
			ExtensionConfigManager extensionConfigManager = this.ecm;
			if (extensionConfigManager == null)
			{
				return;
			}
			extensionConfigManager.OnPartyChanged();
		}

		// Token: 0x0600C042 RID: 49218 RVA: 0x0046B752 File Offset: 0x00469952
		public void PushUserBalance(ValueTuple<string, int> userBalance)
		{
			ExtensionPubSubManager extensionPubSubManager = this.epm;
			if (extensionPubSubManager == null)
			{
				return;
			}
			extensionPubSubManager.PushUserBalance(userBalance);
		}

		// Token: 0x0600C043 RID: 49219 RVA: 0x0046B765 File Offset: 0x00469965
		public void PushViewerChatState(string id, bool hasChatted)
		{
			ExtensionPubSubManager extensionPubSubManager = this.epm;
			if (extensionPubSubManager == null)
			{
				return;
			}
			extensionPubSubManager.PushViewerChatState(id, hasChatted);
		}

		// Token: 0x0600C044 RID: 49220 RVA: 0x0046B779 File Offset: 0x00469979
		public bool CanUseBitCommands()
		{
			return this.ecm.CanUseBitCommands();
		}

		// Token: 0x0600C045 RID: 49221 RVA: 0x0046B788 File Offset: 0x00469988
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

		// Token: 0x0600C046 RID: 49222 RVA: 0x0046B825 File Offset: 0x00469A25
		public void RetrieveJWT()
		{
			GameManager.Instance.StartCoroutine(this.GetJWT(TwitchManager.Current.Authentication.oauth.Substring(6)));
		}

		// Token: 0x0600C047 RID: 49223 RVA: 0x0046B84D File Offset: 0x00469A4D
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

		// Token: 0x0600C048 RID: 49224 RVA: 0x0046B863 File Offset: 0x00469A63
		public void Cleanup()
		{
			this.ecm.Cleanup();
			this.ecm = null;
			this.epm = null;
		}

		// Token: 0x04008FF6 RID: 36854
		[PublicizedFrom(EAccessModifier.Private)]
		public string userId;

		// Token: 0x04008FF7 RID: 36855
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastUpdate = Time.realtimeSinceStartup;

		// Token: 0x04008FF8 RID: 36856
		[PublicizedFrom(EAccessModifier.Private)]
		public ExtensionConfigManager ecm;

		// Token: 0x04008FF9 RID: 36857
		[PublicizedFrom(EAccessModifier.Private)]
		public ExtensionPubSubManager epm;

		// Token: 0x04008FFA RID: 36858
		[PublicizedFrom(EAccessModifier.Private)]
		public string jwt = string.Empty;

		// Token: 0x04008FFB RID: 36859
		[PublicizedFrom(EAccessModifier.Private)]
		public long jwtRefreshTime;

		// Token: 0x04008FFC RID: 36860
		[PublicizedFrom(EAccessModifier.Private)]
		public bool gettingJWT;
	}
}
