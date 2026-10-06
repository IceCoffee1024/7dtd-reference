using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Platform.Steam
{
	// Token: 0x02001CB7 RID: 7351
	public class DownloadableContentValidator : IEntitlementValidator
	{
		// Token: 0x0600D9EF RID: 55791 RVA: 0x004E1510 File Offset: 0x004DF710
		public void Init(IPlatform _owner)
		{
			this._owner = _owner;
		}

		// Token: 0x0600D9F0 RID: 55792 RVA: 0x004E1519 File Offset: 0x004DF719
		public bool IsAvailableOnPlatform(EntitlementSetEnum _dlcSet)
		{
			return _dlcSet == EntitlementSetEnum.None || DownloadableContentValidator.entitlementMap.ContainsKey(_dlcSet);
		}

		// Token: 0x0600D9F1 RID: 55793 RVA: 0x004E152B File Offset: 0x004DF72B
		public bool IsEntitlementPurchasable(EntitlementSetEnum _dlcSet)
		{
			return DLCTitleStorageManager.Instance.IsDLCPurchasable(_dlcSet, DLCEnvironmentFlags.Dev | DLCEnvironmentFlags.Cert | DLCEnvironmentFlags.Retail);
		}

		// Token: 0x0600D9F2 RID: 55794 RVA: 0x004E153C File Offset: 0x004DF73C
		public string GetEntitlementSetId(EntitlementSetEnum _entitlementSet)
		{
			uint num;
			if (!DownloadableContentValidator.entitlementMap.TryGetValue(_entitlementSet, out num))
			{
				return null;
			}
			return num.ToString();
		}

		// Token: 0x0600D9F3 RID: 55795 RVA: 0x004E1564 File Offset: 0x004DF764
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

		// Token: 0x0600D9F4 RID: 55796 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsFetchingEntitlements()
		{
			return false;
		}

		// Token: 0x0600D9F5 RID: 55797 RVA: 0x004E15FC File Offset: 0x004DF7FC
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

		// Token: 0x0600D9F6 RID: 55798 RVA: 0x004E1650 File Offset: 0x004DF850
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

		// Token: 0x0600D9F7 RID: 55799 RVA: 0x004E16D5 File Offset: 0x004DF8D5
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

		// Token: 0x0400A537 RID: 42295
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
			}
		};

		// Token: 0x0400A538 RID: 42296
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform _owner;

		// Token: 0x0400A539 RID: 42297
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<EntitlementSetEnum> pendingDlcChecks = new HashSet<EntitlementSetEnum>();

		// Token: 0x0400A53A RID: 42298
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<EntitlementSetEnum, Action<EntitlementSetEnum>> dlcPurchaseCallbacks = new Dictionary<EntitlementSetEnum, Action<EntitlementSetEnum>>();
	}
}
