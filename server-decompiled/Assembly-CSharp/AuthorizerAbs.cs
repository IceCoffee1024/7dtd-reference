using System;
using Platform;

// Token: 0x02000775 RID: 1909
public abstract class AuthorizerAbs : IAuthorizer
{
	// Token: 0x170005AE RID: 1454
	// (get) Token: 0x060038E2 RID: 14562
	public abstract int Order { get; }

	// Token: 0x170005AF RID: 1455
	// (get) Token: 0x060038E3 RID: 14563
	public abstract string AuthorizerName { get; }

	// Token: 0x170005B0 RID: 1456
	// (get) Token: 0x060038E4 RID: 14564
	public abstract string StateLocalizationKey { get; }

	// Token: 0x170005B1 RID: 1457
	// (get) Token: 0x060038E5 RID: 14565 RVA: 0x00080B69 File Offset: 0x0007ED69
	public virtual EPlatformIdentifier PlatformRestriction
	{
		get
		{
			return EPlatformIdentifier.Count;
		}
	}

	// Token: 0x170005B2 RID: 1458
	// (get) Token: 0x060038E6 RID: 14566 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool AuthorizerActive
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060038E7 RID: 14567 RVA: 0x0017566F File Offset: 0x0017386F
	public virtual void Init(IAuthorizationResponses _authResponsesHandler)
	{
		this.authResponsesHandler = _authResponsesHandler;
	}

	// Token: 0x060038E8 RID: 14568 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Cleanup()
	{
	}

	// Token: 0x060038E9 RID: 14569 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ServerStart()
	{
	}

	// Token: 0x060038EA RID: 14570 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ServerStop()
	{
	}

	// Token: 0x060038EB RID: 14571
	public abstract ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo);

	// Token: 0x060038EC RID: 14572 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Disconnect(ClientInfo _clientInfo)
	{
	}

	// Token: 0x060038ED RID: 14573 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public AuthorizerAbs()
	{
	}

	// Token: 0x04002EE4 RID: 12004
	[PublicizedFrom(EAccessModifier.Protected)]
	public IAuthorizationResponses authResponsesHandler;
}
