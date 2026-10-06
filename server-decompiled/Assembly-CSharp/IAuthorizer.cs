using System;
using Platform;

// Token: 0x02000780 RID: 1920
public interface IAuthorizer
{
	// Token: 0x170005D8 RID: 1496
	// (get) Token: 0x06003971 RID: 14705
	int Order { get; }

	// Token: 0x170005D9 RID: 1497
	// (get) Token: 0x06003972 RID: 14706
	string AuthorizerName { get; }

	// Token: 0x170005DA RID: 1498
	// (get) Token: 0x06003973 RID: 14707
	string StateLocalizationKey { get; }

	// Token: 0x170005DB RID: 1499
	// (get) Token: 0x06003974 RID: 14708
	EPlatformIdentifier PlatformRestriction { get; }

	// Token: 0x170005DC RID: 1500
	// (get) Token: 0x06003975 RID: 14709
	bool AuthorizerActive { get; }

	// Token: 0x06003976 RID: 14710
	void Init(IAuthorizationResponses _authResponsesHandler);

	// Token: 0x06003977 RID: 14711
	void Cleanup();

	// Token: 0x06003978 RID: 14712
	void ServerStart();

	// Token: 0x06003979 RID: 14713
	void ServerStop();

	// Token: 0x0600397A RID: 14714
	ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo);

	// Token: 0x0600397B RID: 14715
	void Disconnect(ClientInfo _clientInfo);
}
