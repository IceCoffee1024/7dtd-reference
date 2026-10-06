using System;

namespace Platform
{
	// Token: 0x02001B97 RID: 7063
	public interface IEntitlementValidator
	{
		// Token: 0x0600D36D RID: 54125
		void Init(IPlatform _owner);

		// Token: 0x0600D36E RID: 54126
		bool HasEntitlement(EntitlementSetEnum _entitlementSet);

		// Token: 0x0600D36F RID: 54127
		bool IsAvailableOnPlatform(EntitlementSetEnum _entitlementSet);

		// Token: 0x0600D370 RID: 54128
		bool IsEntitlementPurchasable(EntitlementSetEnum _entitlementSet);

		// Token: 0x0600D371 RID: 54129
		string GetEntitlementSetId(EntitlementSetEnum _entitlementSet);

		// Token: 0x0600D372 RID: 54130
		DateTime? GetAcquiredDate(EntitlementSetEnum _entitlementSet);

		// Token: 0x0600D373 RID: 54131
		bool IsFetchingEntitlements();

		// Token: 0x0600D374 RID: 54132
		bool OpenStore(EntitlementSetEnum _entitlementSet, Action<EntitlementSetEnum> _onPurchased);
	}
}
