using System;

namespace Platform
{
	// Token: 0x02001B43 RID: 6979
	public interface IMasterServerAnnouncer
	{
		// Token: 0x0600D083 RID: 53379
		void Init(IPlatform _owner);

		// Token: 0x0600D084 RID: 53380
		void Update();

		// Token: 0x1700196B RID: 6507
		// (get) Token: 0x0600D085 RID: 53381
		bool GameServerInitialized { get; }

		// Token: 0x0600D086 RID: 53382
		string GetServerPorts();

		// Token: 0x0600D087 RID: 53383
		void AdvertiseServer(Action _onServerRegistered);

		// Token: 0x0600D088 RID: 53384
		void StopServer();
	}
}
