using System;

namespace Platform.Local
{
	// Token: 0x02001D2A RID: 7466
	public class DownloadableContentValidator : IEntitlementValidator
	{
		// Token: 0x0600DD4D RID: 56653 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600DD4E RID: 56654 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsAvailableOnPlatform(EntitlementSetEnum _dlcSet)
		{
			return false;
		}

		// Token: 0x0600DD4F RID: 56655 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool HasEntitlement(EntitlementSetEnum _dlcSet)
		{
			return false;
		}

		// Token: 0x0600DD50 RID: 56656 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsEntitlementPurchasable(EntitlementSetEnum _dlcSet)
		{
			return false;
		}

		// Token: 0x0600DD51 RID: 56657 RVA: 0x000200DA File Offset: 0x0001E2DA
		public string GetEntitlementSetId(EntitlementSetEnum _entitlementSet)
		{
			return null;
		}

		// Token: 0x0600DD52 RID: 56658 RVA: 0x004EC2D4 File Offset: 0x004EA4D4
		public DateTime? GetAcquiredDate(EntitlementSetEnum _entitlementSet)
		{
			return null;
		}

		// Token: 0x0600DD53 RID: 56659 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsFetchingEntitlements()
		{
			return false;
		}

		// Token: 0x0600DD54 RID: 56660 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool OpenStore(EntitlementSetEnum _dlcSet, Action<EntitlementSetEnum> _onDlcPurchased)
		{
			return false;
		}
	}
}
