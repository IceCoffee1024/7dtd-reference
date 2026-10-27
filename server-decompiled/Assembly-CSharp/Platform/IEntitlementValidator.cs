using System;

namespace Platform
{
	// Token: 0x02001BC8 RID: 7112
	public interface IEntitlementValidator
	{
		// Token: 0x0600D481 RID: 54401
		void Init(IPlatform _owner);

		// Token: 0x0600D482 RID: 54402
		bool HasEntitlement(EntitlementSetEnum _entitlementSet);

		// Token: 0x0600D483 RID: 54403
		bool IsAvailableOnPlatform(EntitlementSetEnum _entitlementSet);

		// Token: 0x0600D484 RID: 54404
		bool IsEntitlementPurchasable(EntitlementSetEnum _entitlementSet);

		// Token: 0x0600D485 RID: 54405
		string GetEntitlementSetId(EntitlementSetEnum _entitlementSet);

		// Token: 0x0600D486 RID: 54406
		DateTime? GetAcquiredDate(EntitlementSetEnum _entitlementSet);

		// Token: 0x0600D487 RID: 54407
		bool IsFetchingEntitlements();

		// Token: 0x0600D488 RID: 54408
		bool OpenStore(EntitlementSetEnum _entitlementSet, Action<EntitlementSetEnum> _onPurchased);
	}
}
