using System;

namespace Platform
{
	// Token: 0x02001B31 RID: 6961
	public interface IApplicationStateController
	{
		// Token: 0x1400011B RID: 283
		// (add) Token: 0x0600D036 RID: 53302
		// (remove) Token: 0x0600D037 RID: 53303
		event IApplicationStateController.ApplicationStateChanged OnApplicationStateChanged;

		// Token: 0x1400011C RID: 284
		// (add) Token: 0x0600D038 RID: 53304
		// (remove) Token: 0x0600D039 RID: 53305
		event IApplicationStateController.NetworkStateChanged OnNetworkStateChanged;

		// Token: 0x17001963 RID: 6499
		// (get) Token: 0x0600D03A RID: 53306
		bool NetworkConnectionState { get; }

		// Token: 0x17001964 RID: 6500
		// (get) Token: 0x0600D03B RID: 53307
		ApplicationState CurrentApplicationState { get; }

		// Token: 0x0600D03C RID: 53308
		void Init(IPlatform owner);

		// Token: 0x0600D03D RID: 53309
		void Destroy();

		// Token: 0x0600D03E RID: 53310
		void Update();

		// Token: 0x02001B32 RID: 6962
		// (Invoke) Token: 0x0600D040 RID: 53312
		public delegate void ApplicationStateChanged(ApplicationState newState);

		// Token: 0x02001B33 RID: 6963
		// (Invoke) Token: 0x0600D044 RID: 53316
		public delegate void NetworkStateChanged(bool connectionState);
	}
}
