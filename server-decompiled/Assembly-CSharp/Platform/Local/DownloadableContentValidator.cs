using System;

namespace Platform.Local
{
	// Token: 0x02001CC7 RID: 7367
	public class DownloadableContentValidator : IEntitlementValidator
	{
		// Token: 0x0600DABF RID: 55999 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600DAC0 RID: 56000 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsAvailableOnPlatform(EntitlementSetEnum _dlcSet)
		{
			return false;
		}

		// Token: 0x0600DAC1 RID: 56001 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool HasEntitlement(EntitlementSetEnum _dlcSet)
		{
			return false;
		}

		// Token: 0x0600DAC2 RID: 56002 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsEntitlementPurchasable(EntitlementSetEnum _dlcSet)
		{
			return false;
		}

		// Token: 0x0600DAC3 RID: 56003 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public string GetEntitlementSetId(EntitlementSetEnum _entitlementSet)
		{
			return null;
		}

		// Token: 0x0600DAC4 RID: 56004 RVA: 0x004E3BEC File Offset: 0x004E1DEC
		public DateTime? GetAcquiredDate(EntitlementSetEnum _entitlementSet)
		{
			return null;
		}

		// Token: 0x0600DAC5 RID: 56005 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool OpenStore(EntitlementSetEnum _dlcSet, Action<EntitlementSetEnum> _onDlcPurchased)
		{
			return false;
		}
	}
}
