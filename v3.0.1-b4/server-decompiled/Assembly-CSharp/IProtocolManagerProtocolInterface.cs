using System;

// Token: 0x02000886 RID: 2182
public interface IProtocolManagerProtocolInterface
{
	// Token: 0x17000691 RID: 1681
	// (get) Token: 0x06003F6D RID: 16237
	bool IsServer { get; }

	// Token: 0x17000692 RID: 1682
	// (get) Token: 0x06003F6E RID: 16238
	bool IsClient { get; }

	// Token: 0x06003F6F RID: 16239
	void InvalidPasswordEv();

	// Token: 0x06003F70 RID: 16240
	void ConnectionFailedEv(string _msg);

	// Token: 0x06003F71 RID: 16241
	void DisconnectedFromServerEv(string _msg);
}
