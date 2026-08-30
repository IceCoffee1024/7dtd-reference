using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Platform.Steam
{
	// Token: 0x02001C85 RID: 7301
	public class DownloadableContentValidator : IEntitlementValidator
	{
		// Token: 0x0600D876 RID: 55414 RVA: 0x004DC9D4 File Offset: 0x004DABD4
		public void Init(IPlatform _owner)
		{
			this._owner = _owner;
		}

		// Token: 0x0600D877 RID: 55415 RVA: 0x004DC9DD File Offset: 0x004DABDD
		public bool IsAvailableOnPlatform(EntitlementSetEnum _dlcSet)
		{
			return _dlcSet == EntitlementSetEnum.None || DownloadableContentValidator.entitlementMap.ContainsKey(_dlcSet);
		}

		// Token: 0x0600D878 RID: 55416 RVA: 0x004DC9EF File Offset: 0x004DABEF
		public bool IsEntitlementPurchasable(EntitlementSetEnum _dlcSet)
		{
			return DLCTitleStorageManager.Instance.IsDLCPurchasable(_dlcSet, DLCEnvironmentFlags.Dev | DLCEnvironmentFlags.Cert | DLCEnvironmentFlags.Retail);
		}

		// Token: 0x0600D879 RID: 55417 RVA: 0x004DCA00 File Offset: 0x004DAC00
		public string GetEntitlementSetId(EntitlementSetEnum _entitlementSet)
		{
			uint num;
			if (!DownloadableContentValidator.entitlementMap.TryGetValue(_entitlementSet, out num))
			{
				return null;
			}
			return num.ToString();
		}

		// Token: 0x0600D87A RID: 55418 RVA: 0x004DCA28 File Offset: 0x004DAC28
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

		// Token: 0x0600D87B RID: 55419 RVA: 0x004DCAC0 File Offset: 0x004DACC0
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

		// Token: 0x0600D87C RID: 55420 RVA: 0x004DCB14 File Offset: 0x004DAD14
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

		// Token: 0x0600D87D RID: 55421 RVA: 0x004DCB99 File Offset: 0x004DAD99
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

		// Token: 0x0400A462 RID: 42082
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
			}
		};

		// Token: 0x0400A463 RID: 42083
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform _owner;

		// Token: 0x0400A464 RID: 42084
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<EntitlementSetEnum> pendingDlcChecks = new HashSet<EntitlementSetEnum>();

		// Token: 0x0400A465 RID: 42085
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<EntitlementSetEnum, Action<EntitlementSetEnum>> dlcPurchaseCallbacks = new Dictionary<EntitlementSetEnum, Action<EntitlementSetEnum>>();
	}
}
