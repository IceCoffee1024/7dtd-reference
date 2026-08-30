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
	// Token: 0x020017C1 RID: 6081
	public class TwitchEntitlementManager : IEntitlementValidator
	{
		// Token: 0x170016F2 RID: 5874
		// (get) Token: 0x0600BCF2 RID: 48370 RVA: 0x0045F9B8 File Offset: 0x0045DBB8
		public string PlatformID
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.prefix + this.owner.User.PlatformUserId.ReadablePlatformUserIdentifier;
			}
		}

		// Token: 0x0600BCF3 RID: 48371 RVA: 0x0045F9DA File Offset: 0x0045DBDA
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			if (!TwitchEntitlementManager.platformIdentifierToPrefix.TryGetValue(_owner.PlatformIdentifier, out this.prefix))
			{
				Log.Warning("could not get platform prefix in Twitch Entitlements Manager");
				return;
			}
		}

		// Token: 0x0600BCF4 RID: 48372 RVA: 0x0045FA08 File Offset: 0x0045DC08
		public void Init()
		{
			TwitchDropAvailabilityManager.Instance.Updated -= this.OnDropsUpdated;
			TwitchDropAvailabilityManager.Instance.Updated += this.OnDropsUpdated;
			TwitchDropAvailabilityManager.Instance.RegisterSource("rfs://drops.xml");
			TwitchDropAvailabilityManager.Instance.UpdateAll(true);
		}

		// Token: 0x0600BCF5 RID: 48373 RVA: 0x0045FA5C File Offset: 0x0045DC5C
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

		// Token: 0x0600BCF6 RID: 48374 RVA: 0x0045FAF4 File Offset: 0x0045DCF4
		public void FetchEntitlements()
		{
			ThreadManager.StartCoroutine(this.FetchEntitlementsCo());
		}

		// Token: 0x0600BCF7 RID: 48375 RVA: 0x0045FB02 File Offset: 0x0045DD02
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator FetchEntitlementsCo()
		{
			UnityWebRequest request = UnityWebRequest.Get("https://xjjvn6hovg33dqetux65clszte0vhysi.lambda-url.us-east-2.on.aws/?platform_id=" + this.PlatformID);
			Log.Out("fetching Twitch entitlements for " + this.PlatformID);
			yield return request.SendWebRequest();
			if (request.result != UnityWebRequest.Result.Success)
			{
				Log.Warning("Failed to fetch Twitch entitlements: " + request.error);
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
			Log.Out("Successfully fetched Twitch entitlements");
			yield break;
		}

		// Token: 0x0600BCF8 RID: 48376 RVA: 0x0045FB11 File Offset: 0x0045DD11
		public void FulfillEntitlements(Action onSuccess = null)
		{
			GameManager.Instance.StartCoroutine(this.FulfillEntitlementsCo(onSuccess));
		}

		// Token: 0x0600BCF9 RID: 48377 RVA: 0x0045FB25 File Offset: 0x0045DD25
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

		// Token: 0x0600BCFA RID: 48378 RVA: 0x0045FB3B File Offset: 0x0045DD3B
		public void SerializeEntitlements()
		{
			JsonConvert.SerializeObject(new EntitlementListWrapper
			{
				entitlements = TwitchEntitlementManager.entitlements
			}, Formatting.Indented);
		}

		// Token: 0x0600BCFB RID: 48379 RVA: 0x0045FB54 File Offset: 0x0045DD54
		public bool IsAvailableOnPlatform(EntitlementSetEnum _set)
		{
			return TwitchEntitlementManager.EntitlementSetToTwitchMap.ContainsKey(_set);
		}

		// Token: 0x0600BCFC RID: 48380 RVA: 0x0045FB64 File Offset: 0x0045DD64
		public bool HasEntitlement(EntitlementSetEnum _set)
		{
			string id;
			return TwitchEntitlementManager.EntitlementSetToTwitchMap.TryGetValue(_set, out id) && TwitchEntitlementManager.entitlements.Exists((Entitlement e) => e.benefit_id == id);
		}

		// Token: 0x0600BCFD RID: 48381 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsEntitlementPurchasable(EntitlementSetEnum _set)
		{
			return false;
		}

		// Token: 0x0600BCFE RID: 48382 RVA: 0x0045FBA2 File Offset: 0x0045DDA2
		public string GetEntitlementSetId(EntitlementSetEnum _entitlementSet)
		{
			return TwitchEntitlementManager.EntitlementSetToTwitchMap.GetValueOrDefault(_entitlementSet);
		}

		// Token: 0x0600BCFF RID: 48383 RVA: 0x0045FBB0 File Offset: 0x0045DDB0
		public DateTime? GetAcquiredDate(EntitlementSetEnum _entitlementSet)
		{
			return null;
		}

		// Token: 0x0600BD00 RID: 48384 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool OpenStore(EntitlementSetEnum _set, Action<EntitlementSetEnum> _onDlcPurchased)
		{
			return false;
		}

		// Token: 0x04008D60 RID: 36192
		[PublicizedFrom(EAccessModifier.Private)]
		public const string FETCH_URL = "https://xjjvn6hovg33dqetux65clszte0vhysi.lambda-url.us-east-2.on.aws/?platform_id=";

		// Token: 0x04008D61 RID: 36193
		[PublicizedFrom(EAccessModifier.Private)]
		public const string FULFILLMENT_URL = "https://ev2dltb7u2pdtwuaphayq5icdy0nmtsx.lambda-url.us-east-2.on.aws/?platform_id=";

		// Token: 0x04008D62 RID: 36194
		[PublicizedFrom(EAccessModifier.Private)]
		public static EnumDictionary<EntitlementSetEnum, string> EntitlementSetToTwitchMap = new EnumDictionary<EntitlementSetEnum, string>();

		// Token: 0x04008D63 RID: 36195
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

		// Token: 0x04008D64 RID: 36196
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly List<Entitlement> entitlements = new List<Entitlement>();

		// Token: 0x04008D65 RID: 36197
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x04008D66 RID: 36198
		[PublicizedFrom(EAccessModifier.Private)]
		public string prefix;
	}
}
