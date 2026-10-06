using System;

namespace Platform.Local
{
	// Token: 0x02001CF9 RID: 7417
	public class DownloadableContentValidator : IEntitlementValidator
	{
		// Token: 0x0600DC39 RID: 56377 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600DC3A RID: 56378 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsAvailableOnPlatform(EntitlementSetEnum _dlcSet)
		{
			return false;
		}

		// Token: 0x0600DC3B RID: 56379 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool HasEntitlement(EntitlementSetEnum _dlcSet)
		{
			return false;
		}

		// Token: 0x0600DC3C RID: 56380 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsEntitlementPurchasable(EntitlementSetEnum _dlcSet)
		{
			return false;
		}

		// Token: 0x0600DC3D RID: 56381 RVA: 0x000200DA File Offset: 0x0001E2DA
		public string GetEntitlementSetId(EntitlementSetEnum _entitlementSet)
		{
			return null;
		}

		// Token: 0x0600DC3E RID: 56382 RVA: 0x004E8734 File Offset: 0x004E6934
		public DateTime? GetAcquiredDate(EntitlementSetEnum _entitlementSet)
		{
			return null;
		}

		// Token: 0x0600DC3F RID: 56383 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsFetchingEntitlements()
		{
			return false;
		}

		// Token: 0x0600DC40 RID: 56384 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool OpenStore(EntitlementSetEnum _dlcSet, Action<EntitlementSetEnum> _onDlcPurchased)
		{
			return false;
		}
	}
}
