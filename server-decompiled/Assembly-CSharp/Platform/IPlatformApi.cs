using System;

namespace Platform
{
	// Token: 0x02001BD7 RID: 7127
	public interface IPlatformApi
	{
		// Token: 0x17001AFA RID: 6906
		// (get) Token: 0x0600D4F7 RID: 54519
		EApiStatus ClientApiStatus { get; }

		// Token: 0x14000121 RID: 289
		// (add) Token: 0x0600D4F8 RID: 54520
		// (remove) Token: 0x0600D4F9 RID: 54521
		event Action ClientApiInitialized;

		// Token: 0x0600D4FA RID: 54522
		void Init(IPlatform _owner);

		// Token: 0x0600D4FB RID: 54523
		bool InitClientApis();

		// Token: 0x0600D4FC RID: 54524
		bool InitServerApis();

		// Token: 0x0600D4FD RID: 54525
		void ServerApiLoaded();

		// Token: 0x0600D4FE RID: 54526
		void Update();

		// Token: 0x0600D4FF RID: 54527
		void Destroy();

		// Token: 0x0600D500 RID: 54528
		float GetScreenBoundsValueFromSystem();
	}
}
