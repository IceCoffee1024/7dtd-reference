using System;
using Platform;

// Token: 0x02000774 RID: 1908
public interface IAuthorizer
{
	// Token: 0x170005A9 RID: 1449
	// (get) Token: 0x060038D7 RID: 14551
	int Order { get; }

	// Token: 0x170005AA RID: 1450
	// (get) Token: 0x060038D8 RID: 14552
	string AuthorizerName { get; }

	// Token: 0x170005AB RID: 1451
	// (get) Token: 0x060038D9 RID: 14553
	string StateLocalizationKey { get; }

	// Token: 0x170005AC RID: 1452
	// (get) Token: 0x060038DA RID: 14554
	EPlatformIdentifier PlatformRestriction { get; }

	// Token: 0x170005AD RID: 1453
	// (get) Token: 0x060038DB RID: 14555
	bool AuthorizerActive { get; }

	// Token: 0x060038DC RID: 14556
	void Init(IAuthorizationResponses _authResponsesHandler);

	// Token: 0x060038DD RID: 14557
	void Cleanup();

	// Token: 0x060038DE RID: 14558
	void ServerStart();

	// Token: 0x060038DF RID: 14559
	void ServerStop();

	// Token: 0x060038E0 RID: 14560
	ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo);

	// Token: 0x060038E1 RID: 14561
	void Disconnect(ClientInfo _clientInfo);
}
