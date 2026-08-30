using System;
using System.Collections.Generic;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL
{
	// Token: 0x02001C0A RID: 7178
	public class DownloadableContentValidator : IEntitlementValidator
	{
		// Token: 0x0600D496 RID: 54422 RVA: 0x004CABB0 File Offset: 0x004C8DB0
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

		// Token: 0x0600D497 RID: 54423 RVA: 0x004CABEC File Offset: 0x004C8DEC
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

		// Token: 0x0600D498 RID: 54424 RVA: 0x004CAC7B File Offset: 0x004C8E7B
		public bool IsAvailableOnPlatform(EntitlementSetEnum _dlcSet)
		{
			return _dlcSet == EntitlementSetEnum.None || DownloadableContentValidator.entitlementMap.ContainsKey(_dlcSet);
		}

		// Token: 0x0600D499 RID: 54425 RVA: 0x004CAC90 File Offset: 0x004C8E90
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

		// Token: 0x0600D49A RID: 54426 RVA: 0x004CACF0 File Offset: 0x004C8EF0
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

		// Token: 0x0600D49B RID: 54427 RVA: 0x004CAD58 File Offset: 0x004C8F58
		public bool OpenStore(EntitlementSetEnum _dlcSet, Action<EntitlementSetEnum> _onDlcPurchased)
		{
			DownloadableContentValidator.<>c__DisplayClass11_0 CS$<>8__locals1 = new DownloadableContentValidator.<>c__DisplayClass11_0();
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

		// Token: 0x0600D49C RID: 54428 RVA: 0x004CADD8 File Offset: 0x004C8FD8
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

		// Token: 0x0600D49D RID: 54429 RVA: 0x004CAE70 File Offset: 0x004C9070
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

		// Token: 0x0400A1EC RID: 41452
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
			}
		};

		// Token: 0x0400A1ED RID: 41453
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<string> ownedEntitlements = new HashSet<string>();

		// Token: 0x0400A1EE RID: 41454
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object lockObj = new object();

		// Token: 0x0400A1EF RID: 41455
		[PublicizedFrom(EAccessModifier.Private)]
		public User user;

		// Token: 0x0400A1F0 RID: 41456
		[PublicizedFrom(EAccessModifier.Private)]
		public XStoreContext storeContext;

		// Token: 0x0400A1F1 RID: 41457
		[PublicizedFrom(EAccessModifier.Private)]
		public int remainingStoreOperations;
	}
}
