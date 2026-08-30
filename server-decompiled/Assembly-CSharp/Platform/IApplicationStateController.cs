using System;

namespace Platform
{
	// Token: 0x02001B58 RID: 7000
	public interface IApplicationStateController
	{
		// Token: 0x1400011C RID: 284
		// (add) Token: 0x0600D1BF RID: 53695
		// (remove) Token: 0x0600D1C0 RID: 53696
		event IApplicationStateController.ApplicationStateChanged OnApplicationStateChanged;

		// Token: 0x1400011D RID: 285
		// (add) Token: 0x0600D1C1 RID: 53697
		// (remove) Token: 0x0600D1C2 RID: 53698
		event IApplicationStateController.NetworkStateChanged OnNetworkStateChanged;

		// Token: 0x170019D5 RID: 6613
		// (get) Token: 0x0600D1C3 RID: 53699
		bool NetworkConnectionState { get; }

		// Token: 0x170019D6 RID: 6614
		// (get) Token: 0x0600D1C4 RID: 53700
		ApplicationState CurrentApplicationState { get; }

		// Token: 0x0600D1C5 RID: 53701
		void Init(IPlatform owner);

		// Token: 0x0600D1C6 RID: 53702
		void Destroy();

		// Token: 0x0600D1C7 RID: 53703
		void Update();

		// Token: 0x02001B59 RID: 7001
		// (Invoke) Token: 0x0600D1C9 RID: 53705
		public delegate void ApplicationStateChanged(ApplicationState newState);

		// Token: 0x02001B5A RID: 7002
		// (Invoke) Token: 0x0600D1CD RID: 53709
		public delegate void NetworkStateChanged(bool connectionState);
	}
}
