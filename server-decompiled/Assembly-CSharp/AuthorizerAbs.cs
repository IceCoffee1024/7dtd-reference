using System;
using Platform;

// Token: 0x02000781 RID: 1921
public abstract class AuthorizerAbs : IAuthorizer
{
	// Token: 0x170005DD RID: 1501
	// (get) Token: 0x0600397C RID: 14716
	public abstract int Order { get; }

	// Token: 0x170005DE RID: 1502
	// (get) Token: 0x0600397D RID: 14717
	public abstract string AuthorizerName { get; }

	// Token: 0x170005DF RID: 1503
	// (get) Token: 0x0600397E RID: 14718
	public abstract string StateLocalizationKey { get; }

	// Token: 0x170005E0 RID: 1504
	// (get) Token: 0x0600397F RID: 14719 RVA: 0x000814D1 File Offset: 0x0007F6D1
	public virtual EPlatformIdentifier PlatformRestriction
	{
		get
		{
			return EPlatformIdentifier.Count;
		}
	}

	// Token: 0x170005E1 RID: 1505
	// (get) Token: 0x06003980 RID: 14720 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool AuthorizerActive
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003981 RID: 14721 RVA: 0x001780A7 File Offset: 0x001762A7
	public virtual void Init(IAuthorizationResponses _authResponsesHandler)
	{
		this.authResponsesHandler = _authResponsesHandler;
	}

	// Token: 0x06003982 RID: 14722 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Cleanup()
	{
	}

	// Token: 0x06003983 RID: 14723 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ServerStart()
	{
	}

	// Token: 0x06003984 RID: 14724 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ServerStop()
	{
	}

	// Token: 0x06003985 RID: 14725
	public abstract ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo);

	// Token: 0x06003986 RID: 14726 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Disconnect(ClientInfo _clientInfo)
	{
	}

	// Token: 0x06003987 RID: 14727 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public AuthorizerAbs()
	{
	}

	// Token: 0x04002F57 RID: 12119
	[PublicizedFrom(EAccessModifier.Protected)]
	public IAuthorizationResponses authResponsesHandler;
}
