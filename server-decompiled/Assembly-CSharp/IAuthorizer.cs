using System;
using Platform;

// Token: 0x02000788 RID: 1928
public interface IAuthorizer
{
	// Token: 0x170005DC RID: 1500
	// (get) Token: 0x06003988 RID: 14728
	int Order { get; }

	// Token: 0x170005DD RID: 1501
	// (get) Token: 0x06003989 RID: 14729
	string AuthorizerName { get; }

	// Token: 0x170005DE RID: 1502
	// (get) Token: 0x0600398A RID: 14730
	string StateLocalizationKey { get; }

	// Token: 0x170005DF RID: 1503
	// (get) Token: 0x0600398B RID: 14731
	EPlatformIdentifier PlatformRestriction { get; }

	// Token: 0x170005E0 RID: 1504
	// (get) Token: 0x0600398C RID: 14732
	bool AuthorizerActive { get; }

	// Token: 0x0600398D RID: 14733
	void Init(IAuthorizationResponses _authResponsesHandler);

	// Token: 0x0600398E RID: 14734
	void Cleanup();

	// Token: 0x0600398F RID: 14735
	void ServerStart();

	// Token: 0x06003990 RID: 14736
	void ServerStop();

	// Token: 0x06003991 RID: 14737
	ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo);

	// Token: 0x06003992 RID: 14738
	void Disconnect(ClientInfo _clientInfo);
}
