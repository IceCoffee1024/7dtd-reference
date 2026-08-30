using System;

namespace Platform
{
	// Token: 0x02001B6A RID: 7018
	public interface IMasterServerAnnouncer
	{
		// Token: 0x0600D20E RID: 53774
		void Init(IPlatform _owner);

		// Token: 0x0600D20F RID: 53775
		void Update();

		// Token: 0x170019DD RID: 6621
		// (get) Token: 0x0600D210 RID: 53776
		bool GameServerInitialized { get; }

		// Token: 0x0600D211 RID: 53777
		string GetServerPorts();

		// Token: 0x0600D212 RID: 53778
		void AdvertiseServer(Action _onServerRegistered);

		// Token: 0x0600D213 RID: 53779
		void StopServer();
	}
}
