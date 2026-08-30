using System;
using System.Collections.Generic;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL
{
	// Token: 0x02001C31 RID: 7217
	public class DownloadableContentValidator : IEntitlementValidator
	{
		// Token: 0x0600D624 RID: 54820 RVA: 0x004D0FA4 File Offset: 0x004CF1A4
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

		// Token: 0x0600D625 RID: 54821 RVA: 0x004D0FE0 File Offset: 0x004CF1E0
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

		// Token: 0x0600D626 RID: 54822 RVA: 0x004D106F File Offset: 0x004CF26F
		public bool IsAvailableOnPlatform(EntitlementSetEnum _dlcSet)
		{
			return _dlcSet == EntitlementSetEnum.None || DownloadableContentValidator.entitlementMap.ContainsKey(_dlcSet);
		}

		// Token: 0x0600D627 RID: 54823 RVA: 0x004D1084 File Offset: 0x004CF284
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

		// Token: 0x0600D628 RID: 54824 RVA: 0x004D10E1 File Offset: 0x004CF2E1
		public string GetEntitlementSetId(EntitlementSetEnum _entitlementSet)
		{
			return DownloadableContentValidator.entitlementMap.GetValueOrDefault(_entitlementSet);
		}

		// Token: 0x0600D629 RID: 54825 RVA: 0x004D10F0 File Offset: 0x004CF2F0
		public DateTime? GetAcquiredDate(EntitlementSetEnum _entitlementSet)
		{
			return null;
		}

		// Token: 0x0600D62A RID: 54826 RVA: 0x004D1108 File Offset: 0x004CF308
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

		// Token: 0x0600D62B RID: 54827 RVA: 0x004D1170 File Offset: 0x004CF370
		public bool OpenStore(EntitlementSetEnum _dlcSet, Action<EntitlementSetEnum> _onDlcPurchased)
		{
			DownloadableContentValidator.<>c__DisplayClass13_0 CS$<>8__locals1 = new DownloadableContentValidator.<>c__DisplayClass13_0();
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

		// Token: 0x0600D62C RID: 54828 RVA: 0x004D11F0 File Offset: 0x004CF3F0
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

		// Token: 0x0600D62D RID: 54829 RVA: 0x004D1288 File Offset: 0x004CF488
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

		// Token: 0x0400A31D RID: 41757
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
			}
		};

		// Token: 0x0400A31E RID: 41758
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<string> ownedEntitlements = new HashSet<string>();

		// Token: 0x0400A31F RID: 41759
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object lockObj = new object();

		// Token: 0x0400A320 RID: 41760
		[PublicizedFrom(EAccessModifier.Private)]
		public User user;

		// Token: 0x0400A321 RID: 41761
		[PublicizedFrom(EAccessModifier.Private)]
		public XStoreContext storeContext;

		// Token: 0x0400A322 RID: 41762
		[PublicizedFrom(EAccessModifier.Private)]
		public int remainingStoreOperations;
	}
}
