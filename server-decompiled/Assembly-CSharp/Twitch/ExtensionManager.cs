using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

namespace Twitch
{
	// Token: 0x02001813 RID: 6163
	public class ExtensionManager
	{
		// Token: 0x0600BEFC RID: 48892 RVA: 0x00466F2C File Offset: 0x0046512C
		public void Init()
		{
			this.extensionStateManager = new ExtensionStateManager();
			this.extensionCommandPoller = new ExtensionCommandPoller();
			this.extensionStateManager.Init();
			this.extensionCommandPoller.Init();
		}

		// Token: 0x0600BEFD RID: 48893 RVA: 0x00466F5A File Offset: 0x0046515A
		public void OnPartyChanged()
		{
			ExtensionStateManager extensionStateManager = this.extensionStateManager;
			if (extensionStateManager == null)
			{
				return;
			}
			extensionStateManager.OnPartyChanged();
		}

		// Token: 0x0600BEFE RID: 48894 RVA: 0x00466F6C File Offset: 0x0046516C
		public void TwitchEnabledChanged(EntityPlayer _ep)
		{
			EntityPlayerLocal localPlayer = TwitchManager.Current.LocalPlayer;
			if (_ep != localPlayer && localPlayer.Party != null && localPlayer.Party.ContainsMember(_ep))
			{
				this.extensionStateManager.OnPartyChanged();
			}
		}

		// Token: 0x0600BEFF RID: 48895 RVA: 0x00466FAE File Offset: 0x004651AE
		public void PushUserBalance(ValueTuple<string, int> userBalance)
		{
			ExtensionStateManager extensionStateManager = this.extensionStateManager;
			if (extensionStateManager == null)
			{
				return;
			}
			extensionStateManager.PushUserBalance(userBalance);
		}

		// Token: 0x0600BF00 RID: 48896 RVA: 0x00466FC1 File Offset: 0x004651C1
		public void PushViewerChatState(string id, bool hasChatted)
		{
			ExtensionStateManager extensionStateManager = this.extensionStateManager;
			if (extensionStateManager == null)
			{
				return;
			}
			extensionStateManager.PushViewerChatState(id, hasChatted);
		}

		// Token: 0x0600BF01 RID: 48897 RVA: 0x00466FD5 File Offset: 0x004651D5
		public bool CanUseBitCommands()
		{
			return this.extensionStateManager.CanUseBitCommands();
		}

		// Token: 0x0600BF02 RID: 48898 RVA: 0x00466FE2 File Offset: 0x004651E2
		public void Update()
		{
			this.extensionStateManager.Update();
			this.extensionCommandPoller.Update();
		}

		// Token: 0x0600BF03 RID: 48899 RVA: 0x00466FFA File Offset: 0x004651FA
		public bool HasCommand()
		{
			return this.extensionCommandPoller.HasCommand();
		}

		// Token: 0x0600BF04 RID: 48900 RVA: 0x00467007 File Offset: 0x00465207
		public ExtensionAction GetCommand()
		{
			return this.extensionCommandPoller.GetCommand();
		}

		// Token: 0x0600BF05 RID: 48901 RVA: 0x00467014 File Offset: 0x00465214
		public void RetrieveJWT()
		{
			this.extensionStateManager.RetrieveJWT();
		}

		// Token: 0x0600BF06 RID: 48902 RVA: 0x00467021 File Offset: 0x00465221
		public void Cleanup()
		{
			this.extensionCommandPoller.Cleanup();
			this.extensionStateManager.Cleanup();
			this.extensionCommandPoller = null;
			this.extensionStateManager = null;
		}

		// Token: 0x0600BF07 RID: 48903 RVA: 0x00467047 File Offset: 0x00465247
		public static void CheckExtensionInstalled(Action<bool> _cb)
		{
			GameManager.Instance.StartCoroutine(ExtensionManager.CheckExtensionInstall(_cb));
		}

		// Token: 0x0600BF08 RID: 48904 RVA: 0x0046705A File Offset: 0x0046525A
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

		// Token: 0x04008ED9 RID: 36569
		public const string API_STAGE = "prod";

		// Token: 0x04008EDA RID: 36570
		public const string EXTENSION_ID = "k6ji189bf7i4ge8il4iczzw7kpgmjt";

		// Token: 0x04008EDB RID: 36571
		public static string Version = "2.0.2";

		// Token: 0x04008EDC RID: 36572
		[PublicizedFrom(EAccessModifier.Private)]
		public ExtensionStateManager extensionStateManager;

		// Token: 0x04008EDD RID: 36573
		[PublicizedFrom(EAccessModifier.Private)]
		public ExtensionCommandPoller extensionCommandPoller;
	}
}
