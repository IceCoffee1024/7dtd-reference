using System;

// Token: 0x02000772 RID: 1906
public interface IAuthorizationResponses
{
	// Token: 0x06003898 RID: 14488
	void AuthorizationDenied(IAuthorizer _authorizer, ClientInfo _clientInfo, GameUtils.KickPlayerData _kickPlayerData);

	// Token: 0x06003899 RID: 14489
	void AuthorizationAccepted(IAuthorizer _authorizer, ClientInfo _clientInfo);
}
