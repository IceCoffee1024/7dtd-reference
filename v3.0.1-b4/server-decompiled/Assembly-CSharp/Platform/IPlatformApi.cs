using System;

namespace Platform
{
	// Token: 0x02001B4D RID: 6989
	public interface IPlatformApi
	{
		// Token: 0x1700199A RID: 6554
		// (get) Token: 0x0600D0E0 RID: 53472
		EApiStatus ClientApiStatus { get; }

		// Token: 0x14000121 RID: 289
		// (add) Token: 0x0600D0E1 RID: 53473
		// (remove) Token: 0x0600D0E2 RID: 53474
		event Action ClientApiInitialized;

		// Token: 0x0600D0E3 RID: 53475
		void Init(IPlatform _owner);

		// Token: 0x0600D0E4 RID: 53476
		bool InitClientApis();

		// Token: 0x0600D0E5 RID: 53477
		bool InitServerApis();

		// Token: 0x0600D0E6 RID: 53478
		void ServerApiLoaded();

		// Token: 0x0600D0E7 RID: 53479
		void Update();

		// Token: 0x0600D0E8 RID: 53480
		void Destroy();

		// Token: 0x0600D0E9 RID: 53481
		float GetScreenBoundsValueFromSystem();
	}
}
