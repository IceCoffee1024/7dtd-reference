using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Platform.Steam
{
	// Token: 0x02001CE8 RID: 7400
	public class DownloadableContentValidator : IEntitlementValidator
	{
		// Token: 0x0600DB03 RID: 56067 RVA: 0x004E5060 File Offset: 0x004E3260
		public void Init(IPlatform _owner)
		{
			this._owner = _owner;
		}

		// Token: 0x0600DB04 RID: 56068 RVA: 0x004E5069 File Offset: 0x004E3269
		public bool IsAvailableOnPlatform(EntitlementSetEnum _dlcSet)
		{
			return _dlcSet == EntitlementSetEnum.None || DownloadableContentValidator.entitlementMap.ContainsKey(_dlcSet);
		}

		// Token: 0x0600DB05 RID: 56069 RVA: 0x004E507B File Offset: 0x004E327B
		public bool IsEntitlementPurchasable(EntitlementSetEnum _dlcSet)
		{
			return DLCTitleStorageManager.Instance.IsDLCPurchasable(_dlcSet, DLCEnvironmentFlags.Dev | DLCEnvironmentFlags.Cert | DLCEnvironmentFlags.Retail);
		}

		// Token: 0x0600DB06 RID: 56070 RVA: 0x004E508C File Offset: 0x004E328C
		public string GetEntitlementSetId(EntitlementSetEnum _entitlementSet)
		{
			uint num;
			if (!DownloadableContentValidator.entitlementMap.TryGetValue(_entitlementSet, out num))
			{
				return null;
			}
			return num.ToString();
		}

		// Token: 0x0600DB07 RID: 56071 RVA: 0x004E50B4 File Offset: 0x004E32B4
		public DateTime? GetAcquiredDate(EntitlementSetEnum _entitlementSet)
		{
			if (_entitlementSet == EntitlementSetEnum.None)
			{
				return null;
			}
			if (!DownloadableContentValidator.entitlementMap.ContainsKey(_entitlementSet))
			{
				return null;
			}
			if (this._owner.Api.ClientApiStatus != EApiStatus.Ok)
			{
				Log.Out("[DownloadableContentValidator] Ignored, game not fully loaded yet");
				return null;
			}
			uint earliestPurchaseUnixTime = SteamApps.GetEarliestPurchaseUnixTime(new AppId_t(DownloadableContentValidator.entitlementMap[_entitlementSet]));
			if (earliestPurchaseUnixTime == 0U)
			{
				return null;
			}
			return new DateTime?(DateTimeOffset.FromUnixTimeSeconds((long)((ulong)earliestPurchaseUnixTime)).ToUniversalTime().DateTime);
		}

		// Token: 0x0600DB08 RID: 56072 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsFetchingEntitlements()
		{
			return false;
		}

		// Token: 0x0600DB09 RID: 56073 RVA: 0x004E514C File Offset: 0x004E334C
		public bool HasEntitlement(EntitlementSetEnum _dlcSet)
		{
			if (_dlcSet == EntitlementSetEnum.None)
			{
				return true;
			}
			if (!DownloadableContentValidator.entitlementMap.ContainsKey(_dlcSet))
			{
				return false;
			}
			if (this._owner.Api.ClientApiStatus != EApiStatus.Ok)
			{
				Log.Out("[DownloadableContentValidator] Ignored, game not fully loaded yet");
				return false;
			}
			return SteamApps.BIsDlcInstalled(new AppId_t(DownloadableContentValidator.entitlementMap[_dlcSet]));
		}

		// Token: 0x0600DB0A RID: 56074 RVA: 0x004E51A0 File Offset: 0x004E33A0
		public bool OpenStore(EntitlementSetEnum _dlcSet, Action<EntitlementSetEnum> _onDlcPurchased)
		{
			if (!DownloadableContentValidator.entitlementMap.ContainsKey(_dlcSet))
			{
				return false;
			}
			if (this._owner.Api.ClientApiStatus != EApiStatus.Ok)
			{
				Log.Out("[DownloadableContentValidator] Ignored, game not fully loaded yet");
				return true;
			}
			SteamFriends.ActivateGameOverlayToStore(new AppId_t(DownloadableContentValidator.entitlementMap[_dlcSet]), EOverlayToStoreFlag.k_EOverlayToStoreFlag_AddToCartAndShow);
			this.pendingDlcChecks.Add(_dlcSet);
			this.dlcPurchaseCallbacks[_dlcSet] = _onDlcPurchased;
			if (this.pendingDlcChecks.Count == 1)
			{
				ThreadManager.StartCoroutine(this.CheckDlcPurchases());
			}
			return true;
		}

		// Token: 0x0600DB0B RID: 56075 RVA: 0x004E5225 File Offset: 0x004E3425
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator CheckDlcPurchases()
		{
			while (this.pendingDlcChecks.Count > 0)
			{
				yield return new WaitForSeconds(2f);
				List<EntitlementSetEnum> list = new List<EntitlementSetEnum>();
				foreach (EntitlementSetEnum entitlementSetEnum in this.pendingDlcChecks)
				{
					if (SteamApps.BIsDlcInstalled(new AppId_t(DownloadableContentValidator.entitlementMap[entitlementSetEnum])))
					{
						list.Add(entitlementSetEnum);
					}
				}
				foreach (EntitlementSetEnum entitlementSetEnum2 in list)
				{
					this.pendingDlcChecks.Remove(entitlementSetEnum2);
					if (this.dlcPurchaseCallbacks.ContainsKey(entitlementSetEnum2))
					{
						Action<EntitlementSetEnum> action = this.dlcPurchaseCallbacks[entitlementSetEnum2];
						if (action != null)
						{
							action(entitlementSetEnum2);
						}
						this.dlcPurchaseCallbacks.Remove(entitlementSetEnum2);
					}
				}
			}
			yield break;
		}

		// Token: 0x0400A620 RID: 42528
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<EntitlementSetEnum, uint> entitlementMap = new Dictionary<EntitlementSetEnum, uint>
		{
			{
				EntitlementSetEnum.MarauderCosmetic,
				3486400U
			},
			{
				EntitlementSetEnum.HoarderCosmetic,
				3314750U
			},
			{
				EntitlementSetEnum.DesertCosmetic,
				3635260U
			},
			{
				EntitlementSetEnum.ClassicSurvivorCosmetic,
				4206290U
			},
			{
				EntitlementSetEnum.PirateCosmetic,
				4234590U
			},
			{
				EntitlementSetEnum.ChristmasCosmetics,
				4234580U
			},
			{
				EntitlementSetEnum.HellreaverCosmetic,
				4234570U
			},
			{
				EntitlementSetEnum.ButcherCosmetic,
				4234560U
			},
			{
				EntitlementSetEnum.BeachCosmetic,
				4767590U
			},
			{
				EntitlementSetEnum.WorkingStiffCosmetic,
				4767600U
			},
			{
				EntitlementSetEnum.HenpocalypseCosmetic,
				4924490U
			},
			{
				EntitlementSetEnum.OktoberfestCosmetic,
				5239930U
			},
			{
				EntitlementSetEnum.WraithWoodWitch,
				5403990U
			},
			{
				EntitlementSetEnum.ScarletScoundrel,
				5404000U
			},
			{
				EntitlementSetEnum.RedRidingRogue,
				5404020U
			},
			{
				EntitlementSetEnum.ShotgunMessiahStore,
				5456760U
			},
			{
				EntitlementSetEnum.WorkingStiffsStore,
				5456740U
			},
			{
				EntitlementSetEnum.PopNPillsStore,
				5456660U
			}
		};

		// Token: 0x0400A621 RID: 42529
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform _owner;

		// Token: 0x0400A622 RID: 42530
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<EntitlementSetEnum> pendingDlcChecks = new HashSet<EntitlementSetEnum>();

		// Token: 0x0400A623 RID: 42531
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<EntitlementSetEnum, Action<EntitlementSetEnum>> dlcPurchaseCallbacks = new Dictionary<EntitlementSetEnum, Action<EntitlementSetEnum>>();
	}
}
