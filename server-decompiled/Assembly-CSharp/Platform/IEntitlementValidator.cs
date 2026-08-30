using System;

namespace Platform
{
	// Token: 0x02001B3E RID: 6974
	public interface IEntitlementValidator
	{
		// Token: 0x0600D06D RID: 53357
		void Init(IPlatform _owner);

		// Token: 0x0600D06E RID: 53358
		bool HasEntitlement(EntitlementSetEnum _entitlementSet);

		// Token: 0x0600D06F RID: 53359
		bool IsAvailableOnPlatform(EntitlementSetEnum _entitlementSet);

		// Token: 0x0600D070 RID: 53360
		bool IsEntitlementPurchasable(EntitlementSetEnum _entitlementSet);

		// Token: 0x0600D071 RID: 53361
		bool OpenStore(EntitlementSetEnum _entitlementSet, Action<EntitlementSetEnum> _onPurchased);
	}
}
