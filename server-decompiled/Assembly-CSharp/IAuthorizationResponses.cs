using System;

// Token: 0x0200076A RID: 1898
public interface IAuthorizationResponses
{
	// Token: 0x06003881 RID: 14465
	void AuthorizationDenied(IAuthorizer _authorizer, ClientInfo _clientInfo, GameUtils.KickPlayerData _kickPlayerData);

	// Token: 0x06003882 RID: 14466
	void AuthorizationAccepted(IAuthorizer _authorizer, ClientInfo _clientInfo);
}
