using System;
using System.Collections.Generic;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL
{
	// Token: 0x02001C63 RID: 7267
	public class DownloadableContentValidator : IEntitlementValidator
	{
		// Token: 0x0600D79C RID: 55196 RVA: 0x004D5A8C File Offset: 0x004D3C8C
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

		// Token: 0x0600D79D RID: 55197 RVA: 0x004D5AC8 File Offset: 0x004D3CC8
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

		// Token: 0x0600D79E RID: 55198 RVA: 0x004D5B57 File Offset: 0x004D3D57
		public bool IsAvailableOnPlatform(EntitlementSetEnum _dlcSet)
		{
			return _dlcSet == EntitlementSetEnum.None || DownloadableContentValidator.entitlementMap.ContainsKey(_dlcSet);
		}

		// Token: 0x0600D79F RID: 55199 RVA: 0x004D5B6C File Offset: 0x004D3D6C
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

		// Token: 0x0600D7A0 RID: 55200 RVA: 0x004D5BC9 File Offset: 0x004D3DC9
		public string GetEntitlementSetId(EntitlementSetEnum _entitlementSet)
		{
			return DownloadableContentValidator.entitlementMap.GetValueOrDefault(_entitlementSet);
		}

		// Token: 0x0600D7A1 RID: 55201 RVA: 0x004D5BD8 File Offset: 0x004D3DD8
		public DateTime? GetAcquiredDate(EntitlementSetEnum _entitlementSet)
		{
			return null;
		}

		// Token: 0x0600D7A2 RID: 55202 RVA: 0x004D5BF0 File Offset: 0x004D3DF0
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

		// Token: 0x0600D7A3 RID: 55203 RVA: 0x004D5C38 File Offset: 0x004D3E38
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

		// Token: 0x0600D7A4 RID: 55204 RVA: 0x004D5CA0 File Offset: 0x004D3EA0
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

		// Token: 0x0600D7A5 RID: 55205 RVA: 0x004D5D20 File Offset: 0x004D3F20
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

		// Token: 0x0600D7A6 RID: 55206 RVA: 0x004D5DB8 File Offset: 0x004D3FB8
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

		// Token: 0x0400A3F2 RID: 41970
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
			}
		};

		// Token: 0x0400A3F3 RID: 41971
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<string> ownedEntitlements = new HashSet<string>();

		// Token: 0x0400A3F4 RID: 41972
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object lockObj = new object();

		// Token: 0x0400A3F5 RID: 41973
		[PublicizedFrom(EAccessModifier.Private)]
		public User user;

		// Token: 0x0400A3F6 RID: 41974
		[PublicizedFrom(EAccessModifier.Private)]
		public XStoreContext storeContext;

		// Token: 0x0400A3F7 RID: 41975
		[PublicizedFrom(EAccessModifier.Private)]
		public int remainingStoreOperations;
	}
}
