using System;

namespace Platform
{
	// Token: 0x02001B9C RID: 7068
	public interface IMasterServerAnnouncer
	{
		// Token: 0x0600D386 RID: 54150
		void Init(IPlatform _owner);

		// Token: 0x0600D387 RID: 54151
		void Update();

		// Token: 0x17001A83 RID: 6787
		// (get) Token: 0x0600D388 RID: 54152
		bool GameServerInitialized { get; }

		// Token: 0x0600D389 RID: 54153
		string GetServerPorts();

		// Token: 0x0600D38A RID: 54154
		void AdvertiseServer(Action _onServerRegistered);

		// Token: 0x0600D38B RID: 54155
		void StopServer();
	}
}
