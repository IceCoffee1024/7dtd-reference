using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000784 RID: 1924
[Preserve]
public class EacAuthorizer : AuthorizerAbs
{
	// Token: 0x170005E0 RID: 1504
	// (get) Token: 0x06003941 RID: 14657 RVA: 0x001762AC File Offset: 0x001744AC
	public override int Order
	{
		get
		{
			return 600;
		}
	}

	// Token: 0x170005E1 RID: 1505
	// (get) Token: 0x06003942 RID: 14658 RVA: 0x001762B3 File Offset: 0x001744B3
	public override string AuthorizerName
	{
		get
		{
			return "EAC";
		}
	}

	// Token: 0x170005E2 RID: 1506
	// (get) Token: 0x06003943 RID: 14659 RVA: 0x001762BA File Offset: 0x001744BA
	public override string StateLocalizationKey
	{
		get
		{
			return "authstate_eac";
		}
	}

	// Token: 0x170005E3 RID: 1507
	// (get) Token: 0x06003944 RID: 14660 RVA: 0x001762C1 File Offset: 0x001744C1
	public override bool AuthorizerActive
	{
		get
		{
			IAntiCheatServer antiCheatServer = PlatformManager.MultiPlatform.AntiCheatServer;
			return antiCheatServer != null && antiCheatServer.ServerEacEnabled();
		}
	}

	// Token: 0x06003945 RID: 14661 RVA: 0x001762D8 File Offset: 0x001744D8
	public override void ServerStart()
	{
		base.ServerStart();
		IAntiCheatServer antiCheatServer = PlatformManager.MultiPlatform.AntiCheatServer;
		if (antiCheatServer == null)
		{
			return;
		}
		antiCheatServer.StartServer(new AuthenticationSuccessfulCallbackDelegate(this.authPlayerEacSuccessfulCallback), new KickPlayerDelegate(this.kickPlayerCallback));
	}

	// Token: 0x06003946 RID: 14662 RVA: 0x0017630D File Offset: 0x0017450D
	public override void ServerStop()
	{
		base.ServerStop();
		IAntiCheatServer antiCheatServer = PlatformManager.MultiPlatform.AntiCheatServer;
		if (antiCheatServer == null)
		{
			return;
		}
		antiCheatServer.StopServer();
	}

	// Token: 0x06003947 RID: 14663 RVA: 0x0017632C File Offset: 0x0017452C
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		IAntiCheatServer antiCheatServer = PlatformManager.MultiPlatform.AntiCheatServer;
		if (antiCheatServer != null)
		{
			antiCheatServer.RegisterUser(_clientInfo);
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.WaitAsync, null);
	}

	// Token: 0x06003948 RID: 14664 RVA: 0x0017635F File Offset: 0x0017455F
	[PublicizedFrom(EAccessModifier.Private)]
	public void authPlayerEacSuccessfulCallback(ClientInfo _cInfo)
	{
		_cInfo.acAuthDone = true;
		this.authResponsesHandler.AuthorizationAccepted(this, _cInfo);
	}

	// Token: 0x06003949 RID: 14665 RVA: 0x00175DB3 File Offset: 0x00173FB3
	[PublicizedFrom(EAccessModifier.Private)]
	public void kickPlayerCallback(ClientInfo _cInfo, GameUtils.KickPlayerData _kickData)
	{
		this.authResponsesHandler.AuthorizationDenied(this, _cInfo, _kickData);
	}

	// Token: 0x0600394A RID: 14666 RVA: 0x00176375 File Offset: 0x00174575
	public override void Disconnect(ClientInfo _clientInfo)
	{
		IAntiCheatServer antiCheatServer = PlatformManager.MultiPlatform.AntiCheatServer;
		if (antiCheatServer == null)
		{
			return;
		}
		antiCheatServer.FreeUser(_clientInfo);
	}
}
