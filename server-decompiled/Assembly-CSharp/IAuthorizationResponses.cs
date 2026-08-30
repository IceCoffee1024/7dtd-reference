using System;

// Token: 0x0200075E RID: 1886
public interface IAuthorizationResponses
{
	// Token: 0x060037E8 RID: 14312
	void AuthorizationDenied(IAuthorizer _authorizer, ClientInfo _clientInfo, GameUtils.KickPlayerData _kickPlayerData);

	// Token: 0x060037E9 RID: 14313
	void AuthorizationAccepted(IAuthorizer _authorizer, ClientInfo _clientInfo);
}
