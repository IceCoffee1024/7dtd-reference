using System;

// Token: 0x0200089F RID: 2207
public interface IProtocolManagerProtocolInterface
{
	// Token: 0x170006C2 RID: 1730
	// (get) Token: 0x06003F83 RID: 16259
	bool IsServer { get; }

	// Token: 0x170006C3 RID: 1731
	// (get) Token: 0x06003F84 RID: 16260
	bool IsClient { get; }

	// Token: 0x06003F85 RID: 16261
	void InvalidPasswordEv();

	// Token: 0x06003F86 RID: 16262
	void ConnectionFailedEv(string _msg);

	// Token: 0x06003F87 RID: 16263
	void DisconnectedFromServerEv(string _msg);
}
