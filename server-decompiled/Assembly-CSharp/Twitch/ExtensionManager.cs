using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

namespace Twitch
{
	// Token: 0x02001844 RID: 6212
	public class ExtensionManager
	{
		// Token: 0x0600C010 RID: 49168 RVA: 0x0046A8D8 File Offset: 0x00468AD8
		public void Init()
		{
			this.extensionStateManager = new ExtensionStateManager();
			this.extensionCommandPoller = new ExtensionCommandPoller();
			this.extensionStateManager.Init();
			this.extensionCommandPoller.Init();
		}

		// Token: 0x0600C011 RID: 49169 RVA: 0x0046A906 File Offset: 0x00468B06
		public void OnPartyChanged()
		{
			ExtensionStateManager extensionStateManager = this.extensionStateManager;
			if (extensionStateManager == null)
			{
				return;
			}
			extensionStateManager.OnPartyChanged();
		}

		// Token: 0x0600C012 RID: 49170 RVA: 0x0046A918 File Offset: 0x00468B18
		public void TwitchEnabledChanged(EntityPlayer _ep)
		{
			EntityPlayerLocal localPlayer = TwitchManager.Current.LocalPlayer;
			if (_ep != localPlayer && localPlayer.Party != null && localPlayer.Party.ContainsMember(_ep))
			{
				this.extensionStateManager.OnPartyChanged();
			}
		}

		// Token: 0x0600C013 RID: 49171 RVA: 0x0046A95A File Offset: 0x00468B5A
		public void PushUserBalance(ValueTuple<string, int> userBalance)
		{
			ExtensionStateManager extensionStateManager = this.extensionStateManager;
			if (extensionStateManager == null)
			{
				return;
			}
			extensionStateManager.PushUserBalance(userBalance);
		}

		// Token: 0x0600C014 RID: 49172 RVA: 0x0046A96D File Offset: 0x00468B6D
		public void PushViewerChatState(string id, bool hasChatted)
		{
			ExtensionStateManager extensionStateManager = this.extensionStateManager;
			if (extensionStateManager == null)
			{
				return;
			}
			extensionStateManager.PushViewerChatState(id, hasChatted);
		}

		// Token: 0x0600C015 RID: 49173 RVA: 0x0046A981 File Offset: 0x00468B81
		public bool CanUseBitCommands()
		{
			return this.extensionStateManager.CanUseBitCommands();
		}

		// Token: 0x0600C016 RID: 49174 RVA: 0x0046A98E File Offset: 0x00468B8E
		public void Update()
		{
			this.extensionStateManager.Update();
			this.extensionCommandPoller.Update();
		}

		// Token: 0x0600C017 RID: 49175 RVA: 0x0046A9A6 File Offset: 0x00468BA6
		public bool HasCommand()
		{
			return this.extensionCommandPoller.HasCommand();
		}

		// Token: 0x0600C018 RID: 49176 RVA: 0x0046A9B3 File Offset: 0x00468BB3
		public ExtensionAction GetCommand()
		{
			return this.extensionCommandPoller.GetCommand();
		}

		// Token: 0x0600C019 RID: 49177 RVA: 0x0046A9C0 File Offset: 0x00468BC0
		public void RetrieveJWT()
		{
			this.extensionStateManager.RetrieveJWT();
		}

		// Token: 0x0600C01A RID: 49178 RVA: 0x0046A9CD File Offset: 0x00468BCD
		public void Cleanup()
		{
			this.extensionCommandPoller.Cleanup();
			this.extensionStateManager.Cleanup();
			this.extensionCommandPoller = null;
			this.extensionStateManager = null;
		}

		// Token: 0x0600C01B RID: 49179 RVA: 0x0046A9F3 File Offset: 0x00468BF3
		public static void CheckExtensionInstalled(Action<bool> _cb)
		{
			GameManager.Instance.StartCoroutine(ExtensionManager.CheckExtensionInstall(_cb));
		}

		// Token: 0x0600C01C RID: 49180 RVA: 0x0046AA06 File Offset: 0x00468C06
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerator CheckExtensionInstall(Action<bool> _cb)
		{
			using (UnityWebRequest req = UnityWebRequest.Get("https://api.twitch.tv/helix/users/extensions?user_id=" + TwitchManager.Current.Authentication.userID))
			{
				req.SetRequestHeader("Authorization", "Bearer " + TwitchManager.Current.Authentication.oauth.Substring(6));
				req.SetRequestHeader("Client-Id", TwitchAuthentication.client_id);
				yield return req.SendWebRequest();
				if (req.result != UnityWebRequest.Result.Success)
				{
					Log.Warning("InBeta Check Failed: " + req.downloadHandler.text);
				}
				else
				{
					try
					{
						JObject jobject = JObject.Parse(req.downloadHandler.text);
						foreach (JToken jtoken in jobject["data"]["panel"].ToObject<JObject>().Values())
						{
							JObject jobject2 = jtoken.ToObject<JObject>();
							JToken jtoken2;
							if (jobject2.TryGetValue("id", out jtoken2) && jtoken2.ToString() == "k6ji189bf7i4ge8il4iczzw7kpgmjt" && jobject2["active"].ToString() == bool.TrueString)
							{
								_cb(true);
								yield break;
							}
						}
						foreach (JToken jtoken3 in jobject["data"]["overlay"].ToObject<JObject>().Values())
						{
							JObject jobject3 = jtoken3.ToObject<JObject>();
							JToken jtoken4;
							if (jobject3.TryGetValue("version", out jtoken4))
							{
								ExtensionManager.Version = jtoken4.ToString();
							}
							JToken jtoken5;
							if (jobject3.TryGetValue("id", out jtoken5) && jtoken5.ToString() == "k6ji189bf7i4ge8il4iczzw7kpgmjt" && jobject3["active"].ToString() == bool.TrueString)
							{
								_cb(true);
								yield break;
							}
						}
					}
					catch (Exception)
					{
						Log.Warning("could not read extension check data");
					}
				}
			}
			UnityWebRequest req = null;
			_cb(false);
			yield break;
			yield break;
		}

		// Token: 0x04008FC2 RID: 36802
		public const string API_STAGE = "prod";

		// Token: 0x04008FC3 RID: 36803
		public const string EXTENSION_ID = "k6ji189bf7i4ge8il4iczzw7kpgmjt";

		// Token: 0x04008FC4 RID: 36804
		public static string Version = "2.0.2";

		// Token: 0x04008FC5 RID: 36805
		[PublicizedFrom(EAccessModifier.Private)]
		public ExtensionStateManager extensionStateManager;

		// Token: 0x04008FC6 RID: 36806
		[PublicizedFrom(EAccessModifier.Private)]
		public ExtensionCommandPoller extensionCommandPoller;
	}
}
