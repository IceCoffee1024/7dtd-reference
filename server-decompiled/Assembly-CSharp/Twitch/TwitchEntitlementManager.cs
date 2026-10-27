using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Platform;
using UnityEngine.Networking;

namespace Twitch
{
	// Token: 0x0200181E RID: 6174
	public class TwitchEntitlementManager : IEntitlementValidator
	{
		// Token: 0x170017DC RID: 6108
		// (get) Token: 0x0600BF5D RID: 48989 RVA: 0x00467884 File Offset: 0x00465A84
		public string PlatformID
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.prefix + this.owner.User.PlatformUserId.ReadablePlatformUserIdentifier;
			}
		}

		// Token: 0x0600BF5E RID: 48990 RVA: 0x004678A6 File Offset: 0x00465AA6
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			if (!TwitchEntitlementManager.platformIdentifierToPrefix.TryGetValue(_owner.PlatformIdentifier, out this.prefix))
			{
				Log.Warning("could not get platform prefix in Twitch Entitlements Manager");
				return;
			}
		}

		// Token: 0x0600BF5F RID: 48991 RVA: 0x004678D4 File Offset: 0x00465AD4
		public void Init()
		{
			TwitchDropAvailabilityManager.Instance.Updated -= this.OnDropsUpdated;
			TwitchDropAvailabilityManager.Instance.Updated += this.OnDropsUpdated;
			TwitchDropAvailabilityManager.Instance.RegisterSource("rfs://drops.xml");
			TwitchDropAvailabilityManager.Instance.UpdateAll(true);
		}

		// Token: 0x0600BF60 RID: 48992 RVA: 0x00467928 File Offset: 0x00465B28
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDropsUpdated(TwitchDropAvailabilityManager mgr)
		{
			List<TwitchDropAvailabilityManager.TwitchDropEntry> list = new List<TwitchDropAvailabilityManager.TwitchDropEntry>();
			mgr.GetEntries(new List<string> { "rfs://drops.xml" }, list);
			TwitchEntitlementManager.EntitlementSetToTwitchMap.Clear();
			foreach (TwitchDropAvailabilityManager.TwitchDropEntry twitchDropEntry in list)
			{
				if (twitchDropEntry.IsAvailable(DateTime.Now))
				{
					TwitchEntitlementManager.EntitlementSetToTwitchMap.Add(twitchDropEntry.EntitlementSet, twitchDropEntry.BenefitId);
				}
			}
			this.FetchEntitlements();
		}

		// Token: 0x0600BF61 RID: 48993 RVA: 0x004679C0 File Offset: 0x00465BC0
		public void FetchEntitlements()
		{
			this.isFetching = true;
			ThreadManager.StartCoroutine(this.FetchEntitlementsCo());
		}

		// Token: 0x0600BF62 RID: 48994 RVA: 0x004679D5 File Offset: 0x00465BD5
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator FetchEntitlementsCo()
		{
			UnityWebRequest request = UnityWebRequest.Get("https://xjjvn6hovg33dqetux65clszte0vhysi.lambda-url.us-east-2.on.aws/?platform_id=" + this.PlatformID);
			Log.Out("fetching Twitch entitlements for " + this.PlatformID);
			yield return request.SendWebRequest();
			if (request.result != UnityWebRequest.Result.Success)
			{
				Log.Warning("Failed to fetch Twitch entitlements: " + request.error);
				this.isFetching = false;
				yield break;
			}
			JArray jarray = (JArray)JObject.Parse(request.downloadHandler.text)["data"];
			TwitchEntitlementManager.entitlements.Clear();
			foreach (JToken jtoken in jarray)
			{
				string text = jtoken.Value<string>("id");
				string text2 = jtoken.Value<string>("benefit_id");
				string text3 = jtoken.Value<string>("fulfillment_status");
				TwitchEntitlementManager.entitlements.Add(new Entitlement
				{
					id = text,
					benefit_id = text2,
					fulfillment_status = text3
				});
			}
			yield return this.FulfillEntitlementsCo(null);
			this.isFetching = false;
			Log.Out("Successfully fetched Twitch entitlements");
			yield break;
		}

		// Token: 0x0600BF63 RID: 48995 RVA: 0x004679E4 File Offset: 0x00465BE4
		public void FulfillEntitlements(Action onSuccess = null)
		{
			GameManager.Instance.StartCoroutine(this.FulfillEntitlementsCo(onSuccess));
		}

		// Token: 0x0600BF64 RID: 48996 RVA: 0x004679F8 File Offset: 0x00465BF8
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator FulfillEntitlementsCo(Action onSuccess)
		{
			List<string> list = new List<string>();
			foreach (Entitlement entitlement in TwitchEntitlementManager.entitlements)
			{
				if (entitlement.fulfillment_status == "CLAIMED")
				{
					list.Add(entitlement.id);
				}
			}
			if (list.Count == 0)
			{
				yield break;
			}
			string text = JsonConvert.SerializeObject(new FulfillmentPayload
			{
				platform_id = this.PlatformID,
				entitlement_ids = list
			});
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			UnityWebRequest request = new UnityWebRequest("https://ev2dltb7u2pdtwuaphayq5icdy0nmtsx.lambda-url.us-east-2.on.aws/?platform_id=" + this.PlatformID, "POST")
			{
				uploadHandler = new UploadHandlerRaw(bytes),
				downloadHandler = new DownloadHandlerBuffer()
			};
			request.SetRequestHeader("Content-Type", "application/json");
			Log.Out("fulfilling Twitch entitlements for " + this.PlatformID);
			yield return request.SendWebRequest();
			if (request.result != UnityWebRequest.Result.Success)
			{
				Log.Warning("Failed to fulfill twitch entitlements: " + request.downloadHandler.text);
			}
			else
			{
				foreach (JToken jtoken in ((JArray)JObject.Parse(request.downloadHandler.text)["data"]))
				{
					string id = jtoken.Value<string>("id");
					string text2 = jtoken.Value<string>("benefit_id");
					string text3 = jtoken.Value<string>("fulfillment_status");
					Entitlement entitlement2 = TwitchEntitlementManager.entitlements.Find((Entitlement e) => e.id == id);
					if (entitlement2 != null)
					{
						entitlement2.fulfillment_status = text3;
						entitlement2.benefit_id = text2;
					}
					else
					{
						TwitchEntitlementManager.entitlements.Add(new Entitlement
						{
							id = id,
							benefit_id = text2,
							fulfillment_status = text3
						});
					}
				}
				if (onSuccess != null)
				{
					onSuccess();
				}
			}
			yield break;
		}

		// Token: 0x0600BF65 RID: 48997 RVA: 0x00467A0E File Offset: 0x00465C0E
		public void SerializeEntitlements()
		{
			JsonConvert.SerializeObject(new EntitlementListWrapper
			{
				entitlements = TwitchEntitlementManager.entitlements
			}, Formatting.Indented);
		}

		// Token: 0x0600BF66 RID: 48998 RVA: 0x00467A27 File Offset: 0x00465C27
		public bool IsAvailableOnPlatform(EntitlementSetEnum _set)
		{
			return TwitchEntitlementManager.EntitlementSetToTwitchMap.ContainsKey(_set);
		}

		// Token: 0x0600BF67 RID: 48999 RVA: 0x00467A34 File Offset: 0x00465C34
		public bool HasEntitlement(EntitlementSetEnum _set)
		{
			string id;
			return TwitchEntitlementManager.EntitlementSetToTwitchMap.TryGetValue(_set, out id) && TwitchEntitlementManager.entitlements.Exists((Entitlement e) => e.benefit_id == id);
		}

		// Token: 0x0600BF68 RID: 49000 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsEntitlementPurchasable(EntitlementSetEnum _set)
		{
			return false;
		}

		// Token: 0x0600BF69 RID: 49001 RVA: 0x00467A72 File Offset: 0x00465C72
		public string GetEntitlementSetId(EntitlementSetEnum _entitlementSet)
		{
			return TwitchEntitlementManager.EntitlementSetToTwitchMap.GetValueOrDefault(_entitlementSet);
		}

		// Token: 0x0600BF6A RID: 49002 RVA: 0x00467A80 File Offset: 0x00465C80
		public DateTime? GetAcquiredDate(EntitlementSetEnum _entitlementSet)
		{
			return null;
		}

		// Token: 0x0600BF6B RID: 49003 RVA: 0x00467A96 File Offset: 0x00465C96
		public bool IsFetchingEntitlements()
		{
			return this.isFetching;
		}

		// Token: 0x0600BF6C RID: 49004 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool OpenStore(EntitlementSetEnum _set, Action<EntitlementSetEnum> _onDlcPurchased)
		{
			return false;
		}

		// Token: 0x04008F0E RID: 36622
		[PublicizedFrom(EAccessModifier.Private)]
		public const string FETCH_URL = "https://xjjvn6hovg33dqetux65clszte0vhysi.lambda-url.us-east-2.on.aws/?platform_id=";

		// Token: 0x04008F0F RID: 36623
		[PublicizedFrom(EAccessModifier.Private)]
		public const string FULFILLMENT_URL = "https://ev2dltb7u2pdtwuaphayq5icdy0nmtsx.lambda-url.us-east-2.on.aws/?platform_id=";

		// Token: 0x04008F10 RID: 36624
		[PublicizedFrom(EAccessModifier.Private)]
		public static EnumDictionary<EntitlementSetEnum, string> EntitlementSetToTwitchMap = new EnumDictionary<EntitlementSetEnum, string>();

		// Token: 0x04008F11 RID: 36625
		[PublicizedFrom(EAccessModifier.Private)]
		public static EnumDictionary<EPlatformIdentifier, string> platformIdentifierToPrefix = new EnumDictionary<EPlatformIdentifier, string>
		{
			{
				EPlatformIdentifier.PSN,
				"p"
			},
			{
				EPlatformIdentifier.XBL,
				"x"
			},
			{
				EPlatformIdentifier.Steam,
				"s"
			}
		};

		// Token: 0x04008F12 RID: 36626
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly List<Entitlement> entitlements = new List<Entitlement>();

		// Token: 0x04008F13 RID: 36627
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x04008F14 RID: 36628
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isFetching;

		// Token: 0x04008F15 RID: 36629
		[PublicizedFrom(EAccessModifier.Private)]
		public string prefix;
	}
}
