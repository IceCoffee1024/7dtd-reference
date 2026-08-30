using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

namespace Twitch
{
	// Token: 0x020017C1 RID: 6081
	public class ExtensionManager
	{
		// Token: 0x0600BC1F RID: 48159 RVA: 0x0045CC54 File Offset: 0x0045AE54
		public void Init()
		{
			this.extensionStateManager = new ExtensionStateManager();
			this.extensionCommandPoller = new ExtensionCommandPoller();
			this.extensionStateManager.Init();
			this.extensionCommandPoller.Init();
		}

		// Token: 0x0600BC20 RID: 48160 RVA: 0x0045CC82 File Offset: 0x0045AE82
		public void OnPartyChanged()
		{
			ExtensionStateManager extensionStateManager = this.extensionStateManager;
			if (extensionStateManager == null)
			{
				return;
			}
			extensionStateManager.OnPartyChanged();
		}

		// Token: 0x0600BC21 RID: 48161 RVA: 0x0045CC94 File Offset: 0x0045AE94
		public void TwitchEnabledChanged(EntityPlayer _ep)
		{
			EntityPlayerLocal localPlayer = TwitchManager.Current.LocalPlayer;
			if (_ep != localPlayer && localPlayer.Party != null && localPlayer.Party.ContainsMember(_ep))
			{
				this.extensionStateManager.OnPartyChanged();
			}
		}

		// Token: 0x0600BC22 RID: 48162 RVA: 0x0045CCD6 File Offset: 0x0045AED6
		public void PushUserBalance(ValueTuple<string, int> userBalance)
		{
			ExtensionStateManager extensionStateManager = this.extensionStateManager;
			if (extensionStateManager == null)
			{
				return;
			}
			extensionStateManager.PushUserBalance(userBalance);
		}

		// Token: 0x0600BC23 RID: 48163 RVA: 0x0045CCE9 File Offset: 0x0045AEE9
		public void PushViewerChatState(string id, bool hasChatted)
		{
			ExtensionStateManager extensionStateManager = this.extensionStateManager;
			if (extensionStateManager == null)
			{
				return;
			}
			extensionStateManager.PushViewerChatState(id, hasChatted);
		}

		// Token: 0x0600BC24 RID: 48164 RVA: 0x0045CCFD File Offset: 0x0045AEFD
		public bool CanUseBitCommands()
		{
			return this.extensionStateManager.CanUseBitCommands();
		}

		// Token: 0x0600BC25 RID: 48165 RVA: 0x0045CD0A File Offset: 0x0045AF0A
		public void Update()
		{
			this.extensionStateManager.Update();
			this.extensionCommandPoller.Update();
		}

		// Token: 0x0600BC26 RID: 48166 RVA: 0x0045CD22 File Offset: 0x0045AF22
		public bool HasCommand()
		{
			return this.extensionCommandPoller.HasCommand();
		}

		// Token: 0x0600BC27 RID: 48167 RVA: 0x0045CD2F File Offset: 0x0045AF2F
		public ExtensionAction GetCommand()
		{
			return this.extensionCommandPoller.GetCommand();
		}

		// Token: 0x0600BC28 RID: 48168 RVA: 0x0045CD3C File Offset: 0x0045AF3C
		public void RetrieveJWT()
		{
			this.extensionStateManager.RetrieveJWT();
		}

		// Token: 0x0600BC29 RID: 48169 RVA: 0x0045CD49 File Offset: 0x0045AF49
		public void Cleanup()
		{
			this.extensionCommandPoller.Cleanup();
			this.extensionStateManager.Cleanup();
			this.extensionCommandPoller = null;
			this.extensionStateManager = null;
		}

		// Token: 0x0600BC2A RID: 48170 RVA: 0x0045CD6F File Offset: 0x0045AF6F
		public static void CheckExtensionInstalled(Action<bool> _cb)
		{
			GameManager.Instance.StartCoroutine(ExtensionManager.CheckExtensionInstall(_cb));
		}

		// Token: 0x0600BC2B RID: 48171 RVA: 0x0045CD82 File Offset: 0x0045AF82
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

		// Token: 0x04008CF7 RID: 36087
		public const string API_STAGE = "prod";

		// Token: 0x04008CF8 RID: 36088
		public const string EXTENSION_ID = "k6ji189bf7i4ge8il4iczzw7kpgmjt";

		// Token: 0x04008CF9 RID: 36089
		public static string Version = "2.0.2";

		// Token: 0x04008CFA RID: 36090
		[PublicizedFrom(EAccessModifier.Private)]
		public ExtensionStateManager extensionStateManager;

		// Token: 0x04008CFB RID: 36091
		[PublicizedFrom(EAccessModifier.Private)]
		public ExtensionCommandPoller extensionCommandPoller;
	}
}
