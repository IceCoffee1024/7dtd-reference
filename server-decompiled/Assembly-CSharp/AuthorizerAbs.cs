using System;
using Platform;

// Token: 0x02000789 RID: 1929
public abstract class AuthorizerAbs : IAuthorizer
{
	// Token: 0x170005E1 RID: 1505
	// (get) Token: 0x06003993 RID: 14739
	public abstract int Order { get; }

	// Token: 0x170005E2 RID: 1506
	// (get) Token: 0x06003994 RID: 14740
	public abstract string AuthorizerName { get; }

	// Token: 0x170005E3 RID: 1507
	// (get) Token: 0x06003995 RID: 14741
	public abstract string StateLocalizationKey { get; }

	// Token: 0x170005E4 RID: 1508
	// (get) Token: 0x06003996 RID: 14742 RVA: 0x000816F5 File Offset: 0x0007F8F5
	public virtual EPlatformIdentifier PlatformRestriction
	{
		get
		{
			return EPlatformIdentifier.Count;
		}
	}

	// Token: 0x170005E5 RID: 1509
	// (get) Token: 0x06003997 RID: 14743 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool AuthorizerActive
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003998 RID: 14744 RVA: 0x0017903F File Offset: 0x0017723F
	public virtual void Init(IAuthorizationResponses _authResponsesHandler)
	{
		this.authResponsesHandler = _authResponsesHandler;
	}

	// Token: 0x06003999 RID: 14745 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Cleanup()
	{
	}

	// Token: 0x0600399A RID: 14746 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ServerStart()
	{
	}

	// Token: 0x0600399B RID: 14747 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ServerStop()
	{
	}

	// Token: 0x0600399C RID: 14748
	public abstract ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo);

	// Token: 0x0600399D RID: 14749 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Disconnect(ClientInfo _clientInfo)
	{
	}

	// Token: 0x0600399E RID: 14750 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public AuthorizerAbs()
	{
	}

	// Token: 0x04002F73 RID: 12147
	[PublicizedFrom(EAccessModifier.Protected)]
	public IAuthorizationResponses authResponsesHandler;
}
