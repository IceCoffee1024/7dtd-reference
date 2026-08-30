using System;

// Token: 0x02000892 RID: 2194
public interface IProtocolManagerProtocolInterface
{
	// Token: 0x1700069D RID: 1693
	// (get) Token: 0x06003FD2 RID: 16338
	bool IsServer { get; }

	// Token: 0x1700069E RID: 1694
	// (get) Token: 0x06003FD3 RID: 16339
	bool IsClient { get; }

	// Token: 0x06003FD4 RID: 16340
	void InvalidPasswordEv();

	// Token: 0x06003FD5 RID: 16341
	void ConnectionFailedEv(string _msg);

	// Token: 0x06003FD6 RID: 16342
	void DisconnectedFromServerEv(string _msg);
}
