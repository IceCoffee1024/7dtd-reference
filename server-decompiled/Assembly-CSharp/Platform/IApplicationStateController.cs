using System;

namespace Platform
{
	// Token: 0x02001B8A RID: 7050
	public interface IApplicationStateController
	{
		// Token: 0x1400011B RID: 283
		// (add) Token: 0x0600D336 RID: 54070
		// (remove) Token: 0x0600D337 RID: 54071
		event IApplicationStateController.ApplicationStateChanged OnApplicationStateChanged;

		// Token: 0x1400011C RID: 284
		// (add) Token: 0x0600D338 RID: 54072
		// (remove) Token: 0x0600D339 RID: 54073
		event IApplicationStateController.NetworkStateChanged OnNetworkStateChanged;

		// Token: 0x17001A7B RID: 6779
		// (get) Token: 0x0600D33A RID: 54074
		bool NetworkConnectionState { get; }

		// Token: 0x17001A7C RID: 6780
		// (get) Token: 0x0600D33B RID: 54075
		ApplicationState CurrentApplicationState { get; }

		// Token: 0x0600D33C RID: 54076
		void Init(IPlatform owner);

		// Token: 0x0600D33D RID: 54077
		void Destroy();

		// Token: 0x0600D33E RID: 54078
		void Update();

		// Token: 0x02001B8B RID: 7051
		// (Invoke) Token: 0x0600D340 RID: 54080
		public delegate void ApplicationStateChanged(ApplicationState newState);

		// Token: 0x02001B8C RID: 7052
		// (Invoke) Token: 0x0600D344 RID: 54084
		public delegate void NetworkStateChanged(bool connectionState);
	}
}
