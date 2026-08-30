using System;

namespace Platform
{
	// Token: 0x02001B65 RID: 7013
	public interface IEntitlementValidator
	{
		// Token: 0x0600D1F6 RID: 53750
		void Init(IPlatform _owner);

		// Token: 0x0600D1F7 RID: 53751
		bool HasEntitlement(EntitlementSetEnum _entitlementSet);

		// Token: 0x0600D1F8 RID: 53752
		bool IsAvailableOnPlatform(EntitlementSetEnum _entitlementSet);

		// Token: 0x0600D1F9 RID: 53753
		bool IsEntitlementPurchasable(EntitlementSetEnum _entitlementSet);

		// Token: 0x0600D1FA RID: 53754
		string GetEntitlementSetId(EntitlementSetEnum _entitlementSet);

		// Token: 0x0600D1FB RID: 53755
		DateTime? GetAcquiredDate(EntitlementSetEnum _entitlementSet);

		// Token: 0x0600D1FC RID: 53756
		bool OpenStore(EntitlementSetEnum _entitlementSet, Action<EntitlementSetEnum> _onPurchased);
	}
}
