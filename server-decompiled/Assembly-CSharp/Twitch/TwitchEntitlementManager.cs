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
	// Token: 0x020017ED RID: 6125
	public class TwitchEntitlementManager : IEntitlementValidator
	{
		// Token: 0x17001794 RID: 6036
		// (get) Token: 0x0600BE49 RID: 48713 RVA: 0x00463ED8 File Offset: 0x004620D8
		public string PlatformID
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.prefix + this.owner.User.PlatformUserId.ReadablePlatformUserIdentifier;
			}
		}

		// Token: 0x0600BE4A RID: 48714 RVA: 0x00463EFA File Offset: 0x004620FA
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			if (!TwitchEntitlementManager.platformIdentifierToPrefix.TryGetValue(_owner.PlatformIdentifier, out this.prefix))
			{
				Log.Warning("could not get platform prefix in Twitch Entitlements Manager");
				return;
			}
		}

		// Token: 0x0600BE4B RID: 48715 RVA: 0x00463F28 File Offset: 0x00462128
		public void Init()
		{
			TwitchDropAvailabilityManager.Instance.Updated -= this.OnDropsUpdated;
			TwitchDropAvailabilityManager.Instance.Updated += this.OnDropsUpdated;
			TwitchDropAvailabilityManager.Instance.RegisterSource("rfs://drops.xml");
			TwitchDropAvailabilityManager.Instance.UpdateAll(true);
		}

		// Token: 0x0600BE4C RID: 48716 RVA: 0x00463F7C File Offset: 0x0046217C
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

		// Token: 0x0600BE4D RID: 48717 RVA: 0x00464014 File Offset: 0x00462214
		public void FetchEntitlements()
		{
			this.isFetching = true;
			ThreadManager.StartCoroutine(this.FetchEntitlementsCo());
		}

		// Token: 0x0600BE4E RID: 48718 RVA: 0x00464029 File Offset: 0x00462229
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

		// Token: 0x0600BE4F RID: 48719 RVA: 0x00464038 File Offset: 0x00462238
		public void FulfillEntitlements(Action onSuccess = null)
		{
			GameManager.Instance.StartCoroutine(this.FulfillEntitlementsCo(onSuccess));
		}

		// Token: 0x0600BE50 RID: 48720 RVA: 0x0046404C File Offset: 0x0046224C
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

		// Token: 0x0600BE51 RID: 48721 RVA: 0x00464062 File Offset: 0x00462262
		public void SerializeEntitlements()
		{
			JsonConvert.SerializeObject(new EntitlementListWrapper
			{
				entitlements = TwitchEntitlementManager.entitlements
			}, Formatting.Indented);
		}

		// Token: 0x0600BE52 RID: 48722 RVA: 0x0046407B File Offset: 0x0046227B
		public bool IsAvailableOnPlatform(EntitlementSetEnum _set)
		{
			return TwitchEntitlementManager.EntitlementSetToTwitchMap.ContainsKey(_set);
		}

		// Token: 0x0600BE53 RID: 48723 RVA: 0x00464088 File Offset: 0x00462288
		public bool HasEntitlement(EntitlementSetEnum _set)
		{
			string id;
			return TwitchEntitlementManager.EntitlementSetToTwitchMap.TryGetValue(_set, out id) && TwitchEntitlementManager.entitlements.Exists((Entitlement e) => e.benefit_id == id);
		}

		// Token: 0x0600BE54 RID: 48724 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsEntitlementPurchasable(EntitlementSetEnum _set)
		{
			return false;
		}

		// Token: 0x0600BE55 RID: 48725 RVA: 0x004640C6 File Offset: 0x004622C6
		public string GetEntitlementSetId(EntitlementSetEnum _entitlementSet)
		{
			return TwitchEntitlementManager.EntitlementSetToTwitchMap.GetValueOrDefault(_entitlementSet);
		}

		// Token: 0x0600BE56 RID: 48726 RVA: 0x004640D4 File Offset: 0x004622D4
		public DateTime? GetAcquiredDate(EntitlementSetEnum _entitlementSet)
		{
			return null;
		}

		// Token: 0x0600BE57 RID: 48727 RVA: 0x004640EA File Offset: 0x004622EA
		public bool IsFetchingEntitlements()
		{
			return this.isFetching;
		}

		// Token: 0x0600BE58 RID: 48728 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool OpenStore(EntitlementSetEnum _set, Action<EntitlementSetEnum> _onDlcPurchased)
		{
			return false;
		}

		// Token: 0x04008E25 RID: 36389
		[PublicizedFrom(EAccessModifier.Private)]
		public const string FETCH_URL = "https://xjjvn6hovg33dqetux65clszte0vhysi.lambda-url.us-east-2.on.aws/?platform_id=";

		// Token: 0x04008E26 RID: 36390
		[PublicizedFrom(EAccessModifier.Private)]
		public const string FULFILLMENT_URL = "https://ev2dltb7u2pdtwuaphayq5icdy0nmtsx.lambda-url.us-east-2.on.aws/?platform_id=";

		// Token: 0x04008E27 RID: 36391
		[PublicizedFrom(EAccessModifier.Private)]
		public static EnumDictionary<EntitlementSetEnum, string> EntitlementSetToTwitchMap = new EnumDictionary<EntitlementSetEnum, string>();

		// Token: 0x04008E28 RID: 36392
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

		// Token: 0x04008E29 RID: 36393
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly List<Entitlement> entitlements = new List<Entitlement>();

		// Token: 0x04008E2A RID: 36394
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x04008E2B RID: 36395
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isFetching;

		// Token: 0x04008E2C RID: 36396
		[PublicizedFrom(EAccessModifier.Private)]
		public string prefix;
	}
}
