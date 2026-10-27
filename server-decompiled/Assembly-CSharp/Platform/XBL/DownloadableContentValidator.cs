using System;
using System.Collections.Generic;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL
{
	// Token: 0x02001C94 RID: 7316
	public class DownloadableContentValidator : IEntitlementValidator
	{
		// Token: 0x0600D8B0 RID: 55472 RVA: 0x004D95B4 File Offset: 0x004D77B4
		public void Init(IPlatform _owner)
		{
			PlatformManager.NativePlatform.User.UserLoggedIn += delegate(IPlatform _)
			{
				this.user = (User)_owner.User;
				foreach (EntitlementSetEnum entitlementSetEnum in DownloadableContentValidator.entitlementMap.Keys)
				{
					this.FetchEntitlement(entitlementSetEnum, null);
				}
			};
		}

		// Token: 0x0600D8B1 RID: 55473 RVA: 0x004D95F0 File Offset: 0x004D77F0
		[PublicizedFrom(EAccessModifier.Private)]
		public void FetchEntitlement(EntitlementSetEnum _dlcSet, Action<EntitlementSetEnum> _onDlcFetched = null)
		{
			DownloadableContentValidator.<>c__DisplayClass7_0 CS$<>8__locals1 = new DownloadableContentValidator.<>c__DisplayClass7_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1._dlcSet = _dlcSet;
			CS$<>8__locals1._onDlcFetched = _onDlcFetched;
			if (CS$<>8__locals1._dlcSet == EntitlementSetEnum.None)
			{
				return;
			}
			if (!DownloadableContentValidator.entitlementMap.ContainsKey(CS$<>8__locals1._dlcSet))
			{
				Log.Warning(string.Format("[XBL] DLC map missing entry for DLC Set {0}", CS$<>8__locals1._dlcSet));
				return;
			}
			if (!this.StartStoreOperation())
			{
				return;
			}
			SDK.XStoreAcquireLicenseForDurablesAsync(this.storeContext, DownloadableContentValidator.entitlementMap[CS$<>8__locals1._dlcSet], new XStoreAcquireLicenseForDurablesCompleted(CS$<>8__locals1.<FetchEntitlement>g__licenseAcquired|0));
		}

		// Token: 0x0600D8B2 RID: 55474 RVA: 0x004D967F File Offset: 0x004D787F
		public bool IsAvailableOnPlatform(EntitlementSetEnum _dlcSet)
		{
			return _dlcSet == EntitlementSetEnum.None || DownloadableContentValidator.entitlementMap.ContainsKey(_dlcSet);
		}

		// Token: 0x0600D8B3 RID: 55475 RVA: 0x004D9694 File Offset: 0x004D7894
		public bool IsEntitlementPurchasable(EntitlementSetEnum _dlcSet)
		{
			DLCEnvironmentFlags dlcenvironmentFlags = DLCEnvironmentFlags.None;
			string text = ((this.user != null) ? this.user.SandboxHelper.SandboxId : null);
			if (text == null)
			{
				Log.Warning(string.Format("[XBL] {0} no sandbox id. Defaulting to {1}", "DLCEnvironmentFlags", DLCEnvironmentFlags.None));
			}
			else
			{
				dlcenvironmentFlags = XblSandboxHelper.SandboxIdToDLCEnvironment(text);
			}
			return DLCTitleStorageManager.Instance.IsDLCPurchasable(_dlcSet, dlcenvironmentFlags);
		}

		// Token: 0x0600D8B4 RID: 55476 RVA: 0x004D96F1 File Offset: 0x004D78F1
		public string GetEntitlementSetId(EntitlementSetEnum _entitlementSet)
		{
			return DownloadableContentValidator.entitlementMap.GetValueOrDefault(_entitlementSet);
		}

		// Token: 0x0600D8B5 RID: 55477 RVA: 0x004D9700 File Offset: 0x004D7900
		public DateTime? GetAcquiredDate(EntitlementSetEnum _entitlementSet)
		{
			return null;
		}

		// Token: 0x0600D8B6 RID: 55478 RVA: 0x004D9718 File Offset: 0x004D7918
		public bool IsFetchingEntitlements()
		{
			object obj = this.lockObj;
			bool flag2;
			lock (obj)
			{
				flag2 = this.storeContext != null;
			}
			return flag2;
		}

		// Token: 0x0600D8B7 RID: 55479 RVA: 0x004D9760 File Offset: 0x004D7960
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
			object obj = this.lockObj;
			bool flag2;
			lock (obj)
			{
				flag2 = this.ownedEntitlements.Contains(DownloadableContentValidator.entitlementMap[_dlcSet]);
			}
			return flag2;
		}

		// Token: 0x0600D8B8 RID: 55480 RVA: 0x004D97C8 File Offset: 0x004D79C8
		public bool OpenStore(EntitlementSetEnum _dlcSet, Action<EntitlementSetEnum> _onDlcPurchased)
		{
			DownloadableContentValidator.<>c__DisplayClass14_0 CS$<>8__locals1 = new DownloadableContentValidator.<>c__DisplayClass14_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1._onDlcPurchased = _onDlcPurchased;
			CS$<>8__locals1._dlcSet = _dlcSet;
			if (!DownloadableContentValidator.entitlementMap.ContainsKey(CS$<>8__locals1._dlcSet))
			{
				return false;
			}
			CS$<>8__locals1.dlcId = DownloadableContentValidator.entitlementMap[CS$<>8__locals1._dlcSet];
			if (!this.StartStoreOperation())
			{
				return true;
			}
			SDK.XStoreShowPurchaseUIAsync(this.storeContext, CS$<>8__locals1.dlcId, null, null, new XStoreShowPurchaseUICompleted(CS$<>8__locals1.<OpenStore>g__OnStoreClosed|0));
			return true;
		}

		// Token: 0x0600D8B9 RID: 55481 RVA: 0x004D9848 File Offset: 0x004D7A48
		[PublicizedFrom(EAccessModifier.Private)]
		public bool StartStoreOperation()
		{
			object obj = this.lockObj;
			bool flag2;
			lock (obj)
			{
				this.remainingStoreOperations++;
				if (this.storeContext == null)
				{
					int num = SDK.XStoreCreateContext(null, out this.storeContext);
					if (Unity.XGamingRuntime.Interop.HR.FAILED(num))
					{
						this.remainingStoreOperations--;
						Log.Error(string.Format("Failed to create store context with error {0}.", num));
						return false;
					}
				}
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x0600D8BA RID: 55482 RVA: 0x004D98E0 File Offset: 0x004D7AE0
		[PublicizedFrom(EAccessModifier.Private)]
		public void CompleteStoreOperation()
		{
			object obj = this.lockObj;
			lock (obj)
			{
				if (this.remainingStoreOperations > 0)
				{
					this.remainingStoreOperations--;
				}
				if (this.remainingStoreOperations == 0 && this.storeContext != null)
				{
					SDK.XStoreCloseContextHandle(this.storeContext);
					this.storeContext = null;
				}
			}
		}

		// Token: 0x0400A4DB RID: 42203
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<EntitlementSetEnum, string> entitlementMap = new Dictionary<EntitlementSetEnum, string>
		{
			{
				EntitlementSetEnum.MarauderCosmetic,
				"9P0P2QLB276Q"
			},
			{
				EntitlementSetEnum.HoarderCosmetic,
				"9NZ80ZC0SS1S"
			},
			{
				EntitlementSetEnum.DesertCosmetic,
				"9MZV1FWF2CGR"
			},
			{
				EntitlementSetEnum.ClassicSurvivorCosmetic,
				"9MWGC48ZGF76"
			},
			{
				EntitlementSetEnum.PirateCosmetic,
				"9P54R75H9N3Z"
			},
			{
				EntitlementSetEnum.ChristmasCosmetics,
				"9NB7W1P5CQ30"
			},
			{
				EntitlementSetEnum.HellreaverCosmetic,
				"9NJQPF9T4XLD"
			},
			{
				EntitlementSetEnum.ButcherCosmetic,
				"9N63SN1VPKR3"
			},
			{
				EntitlementSetEnum.BeachCosmetic,
				"9P1WF3JZN172"
			},
			{
				EntitlementSetEnum.WorkingStiffCosmetic,
				"9NT7PFJQ30S6"
			},
			{
				EntitlementSetEnum.HenpocalypseCosmetic,
				"9NL21ND88ZVM"
			},
			{
				EntitlementSetEnum.OktoberfestCosmetic,
				"9NQJBBMD3PFQ"
			},
			{
				EntitlementSetEnum.WraithWoodWitch,
				"9MWQDD1FMNN6"
			},
			{
				EntitlementSetEnum.ScarletScoundrel,
				"9NR0823XZJM7"
			},
			{
				EntitlementSetEnum.RedRidingRogue,
				"9PNHZG1GWC8G"
			}
		};

		// Token: 0x0400A4DC RID: 42204
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<string> ownedEntitlements = new HashSet<string>();

		// Token: 0x0400A4DD RID: 42205
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object lockObj = new object();

		// Token: 0x0400A4DE RID: 42206
		[PublicizedFrom(EAccessModifier.Private)]
		public User user;

		// Token: 0x0400A4DF RID: 42207
		[PublicizedFrom(EAccessModifier.Private)]
		public XStoreContext storeContext;

		// Token: 0x0400A4E0 RID: 42208
		[PublicizedFrom(EAccessModifier.Private)]
		public int remainingStoreOperations;
	}
}
