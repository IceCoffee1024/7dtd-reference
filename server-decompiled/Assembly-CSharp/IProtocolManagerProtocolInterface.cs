using System;

// Token: 0x02000895 RID: 2197
public interface IProtocolManagerProtocolInterface
{
	// Token: 0x170006BC RID: 1724
	// (get) Token: 0x06003F60 RID: 16224
	bool IsServer { get; }

	// Token: 0x170006BD RID: 1725
	// (get) Token: 0x06003F61 RID: 16225
	bool IsClient { get; }

	// Token: 0x06003F62 RID: 16226
	void InvalidPasswordEv();

	// Token: 0x06003F63 RID: 16227
	void ConnectionFailedEv(string _msg);

	// Token: 0x06003F64 RID: 16228
	void DisconnectedFromServerEv(string _msg);
}
