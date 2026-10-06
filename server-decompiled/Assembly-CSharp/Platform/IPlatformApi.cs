using System;

namespace Platform
{
	// Token: 0x02001BA6 RID: 7078
	public interface IPlatformApi
	{
		// Token: 0x17001AB2 RID: 6834
		// (get) Token: 0x0600D3E3 RID: 54243
		EApiStatus ClientApiStatus { get; }

		// Token: 0x14000121 RID: 289
		// (add) Token: 0x0600D3E4 RID: 54244
		// (remove) Token: 0x0600D3E5 RID: 54245
		event Action ClientApiInitialized;

		// Token: 0x0600D3E6 RID: 54246
		void Init(IPlatform _owner);

		// Token: 0x0600D3E7 RID: 54247
		bool InitClientApis();

		// Token: 0x0600D3E8 RID: 54248
		bool InitServerApis();

		// Token: 0x0600D3E9 RID: 54249
		void ServerApiLoaded();

		// Token: 0x0600D3EA RID: 54250
		void Update();

		// Token: 0x0600D3EB RID: 54251
		void Destroy();

		// Token: 0x0600D3EC RID: 54252
		float GetScreenBoundsValueFromSystem();
	}
}
