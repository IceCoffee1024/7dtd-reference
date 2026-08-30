using System;

namespace Platform.Local
{
	// Token: 0x02001CA0 RID: 7328
	public class DownloadableContentValidator : IEntitlementValidator
	{
		// Token: 0x0600D927 RID: 55591 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600D928 RID: 55592 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsAvailableOnPlatform(EntitlementSetEnum _dlcSet)
		{
			return false;
		}

		// Token: 0x0600D929 RID: 55593 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool HasEntitlement(EntitlementSetEnum _dlcSet)
		{
			return false;
		}

		// Token: 0x0600D92A RID: 55594 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsEntitlementPurchasable(EntitlementSetEnum _dlcSet)
		{
			return false;
		}

		// Token: 0x0600D92B RID: 55595 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool OpenStore(EntitlementSetEnum _dlcSet, Action<EntitlementSetEnum> _onDlcPurchased)
		{
			return false;
		}
	}
}
