using System;

namespace Platform
{
	// Token: 0x02001BCD RID: 7117
	public interface IMasterServerAnnouncer
	{
		// Token: 0x0600D49A RID: 54426
		void Init(IPlatform _owner);

		// Token: 0x0600D49B RID: 54427
		void Update();

		// Token: 0x17001ACB RID: 6859
		// (get) Token: 0x0600D49C RID: 54428
		bool GameServerInitialized { get; }

		// Token: 0x0600D49D RID: 54429
		string GetServerPorts();

		// Token: 0x0600D49E RID: 54430
		void AdvertiseServer(Action _onServerRegistered);

		// Token: 0x0600D49F RID: 54431
		void StopServer();
	}
}
