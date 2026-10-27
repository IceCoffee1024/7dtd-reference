using System;

namespace Platform
{
	// Token: 0x02001BBB RID: 7099
	public interface IApplicationStateController
	{
		// Token: 0x1400011B RID: 283
		// (add) Token: 0x0600D44A RID: 54346
		// (remove) Token: 0x0600D44B RID: 54347
		event IApplicationStateController.ApplicationStateChanged OnApplicationStateChanged;

		// Token: 0x1400011C RID: 284
		// (add) Token: 0x0600D44C RID: 54348
		// (remove) Token: 0x0600D44D RID: 54349
		event IApplicationStateController.NetworkStateChanged OnNetworkStateChanged;

		// Token: 0x17001AC3 RID: 6851
		// (get) Token: 0x0600D44E RID: 54350
		bool NetworkConnectionState { get; }

		// Token: 0x17001AC4 RID: 6852
		// (get) Token: 0x0600D44F RID: 54351
		ApplicationState CurrentApplicationState { get; }

		// Token: 0x0600D450 RID: 54352
		void Init(IPlatform owner);

		// Token: 0x0600D451 RID: 54353
		void Destroy();

		// Token: 0x0600D452 RID: 54354
		void Update();

		// Token: 0x02001BBC RID: 7100
		// (Invoke) Token: 0x0600D454 RID: 54356
		public delegate void ApplicationStateChanged(ApplicationState newState);

		// Token: 0x02001BBD RID: 7101
		// (Invoke) Token: 0x0600D458 RID: 54360
		public delegate void NetworkStateChanged(bool connectionState);
	}
}
