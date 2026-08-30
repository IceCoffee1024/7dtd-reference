using System;

namespace Platform
{
	// Token: 0x02001B74 RID: 7028
	public interface IPlatformApi
	{
		// Token: 0x17001A0C RID: 6668
		// (get) Token: 0x0600D26B RID: 53867
		EApiStatus ClientApiStatus { get; }

		// Token: 0x14000122 RID: 290
		// (add) Token: 0x0600D26C RID: 53868
		// (remove) Token: 0x0600D26D RID: 53869
		event Action ClientApiInitialized;

		// Token: 0x0600D26E RID: 53870
		void Init(IPlatform _owner);

		// Token: 0x0600D26F RID: 53871
		bool InitClientApis();

		// Token: 0x0600D270 RID: 53872
		bool InitServerApis();

		// Token: 0x0600D271 RID: 53873
		void ServerApiLoaded();

		// Token: 0x0600D272 RID: 53874
		void Update();

		// Token: 0x0600D273 RID: 53875
		void Destroy();

		// Token: 0x0600D274 RID: 53876
		float GetScreenBoundsValueFromSystem();
	}
}
