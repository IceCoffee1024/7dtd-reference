using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

namespace Twitch
{
	// Token: 0x020017E7 RID: 6119
	public class ExtensionManager
	{
		// Token: 0x0600BDA4 RID: 48548 RVA: 0x004629F0 File Offset: 0x00460BF0
		public void Init()
		{
			this.extensionStateManager = new ExtensionStateManager();
			this.extensionCommandPoller = new ExtensionCommandPoller();
			this.extensionStateManager.Init();
			this.extensionCommandPoller.Init();
		}

		// Token: 0x0600BDA5 RID: 48549 RVA: 0x00462A1E File Offset: 0x00460C1E
		public void OnPartyChanged()
		{
			ExtensionStateManager extensionStateManager = this.extensionStateManager;
			if (extensionStateManager == null)
			{
				return;
			}
			extensionStateManager.OnPartyChanged();
		}

		// Token: 0x0600BDA6 RID: 48550 RVA: 0x00462A30 File Offset: 0x00460C30
		public void TwitchEnabledChanged(EntityPlayer _ep)
		{
			EntityPlayerLocal localPlayer = TwitchManager.Current.LocalPlayer;
			if (_ep != localPlayer && localPlayer.Party != null && localPlayer.Party.ContainsMember(_ep))
			{
				this.extensionStateManager.OnPartyChanged();
			}
		}

		// Token: 0x0600BDA7 RID: 48551 RVA: 0x00462A72 File Offset: 0x00460C72
		public void PushUserBalance(ValueTuple<string, int> userBalance)
		{
			ExtensionStateManager extensionStateManager = this.extensionStateManager;
			if (extensionStateManager == null)
			{
				return;
			}
			extensionStateManager.PushUserBalance(userBalance);
		}

		// Token: 0x0600BDA8 RID: 48552 RVA: 0x00462A85 File Offset: 0x00460C85
		public void PushViewerChatState(string id, bool hasChatted)
		{
			ExtensionStateManager extensionStateManager = this.extensionStateManager;
			if (extensionStateManager == null)
			{
				return;
			}
			extensionStateManager.PushViewerChatState(id, hasChatted);
		}

		// Token: 0x0600BDA9 RID: 48553 RVA: 0x00462A99 File Offset: 0x00460C99
		public bool CanUseBitCommands()
		{
			return this.extensionStateManager.CanUseBitCommands();
		}

		// Token: 0x0600BDAA RID: 48554 RVA: 0x00462AA6 File Offset: 0x00460CA6
		public void Update()
		{
			this.extensionStateManager.Update();
			this.extensionCommandPoller.Update();
		}

		// Token: 0x0600BDAB RID: 48555 RVA: 0x00462ABE File Offset: 0x00460CBE
		public bool HasCommand()
		{
			return this.extensionCommandPoller.HasCommand();
		}

		// Token: 0x0600BDAC RID: 48556 RVA: 0x00462ACB File Offset: 0x00460CCB
		public ExtensionAction GetCommand()
		{
			return this.extensionCommandPoller.GetCommand();
		}

		// Token: 0x0600BDAD RID: 48557 RVA: 0x00462AD8 File Offset: 0x00460CD8
		public void RetrieveJWT()
		{
			this.extensionStateManager.RetrieveJWT();
		}

		// Token: 0x0600BDAE RID: 48558 RVA: 0x00462AE5 File Offset: 0x00460CE5
		public void Cleanup()
		{
			this.extensionCommandPoller.Cleanup();
			this.extensionStateManager.Cleanup();
			this.extensionCommandPoller = null;
			this.extensionStateManager = null;
		}

		// Token: 0x0600BDAF RID: 48559 RVA: 0x00462B0B File Offset: 0x00460D0B
		public static void CheckExtensionInstalled(Action<bool> _cb)
		{
			GameManager.Instance.StartCoroutine(ExtensionManager.CheckExtensionInstall(_cb));
		}

		// Token: 0x0600BDB0 RID: 48560 RVA: 0x00462B1E File Offset: 0x00460D1E
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

		// Token: 0x04008E13 RID: 36371
		public const string API_STAGE = "prod";

		// Token: 0x04008E14 RID: 36372
		public const string EXTENSION_ID = "k6ji189bf7i4ge8il4iczzw7kpgmjt";

		// Token: 0x04008E15 RID: 36373
		public static string Version = "2.0.2";

		// Token: 0x04008E16 RID: 36374
		[PublicizedFrom(EAccessModifier.Private)]
		public ExtensionStateManager extensionStateManager;

		// Token: 0x04008E17 RID: 36375
		[PublicizedFrom(EAccessModifier.Private)]
		public ExtensionCommandPoller extensionCommandPoller;
	}
}
