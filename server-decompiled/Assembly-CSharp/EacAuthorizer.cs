using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000790 RID: 1936
[Preserve]
public class EacAuthorizer : AuthorizerAbs
{
	// Token: 0x1700060F RID: 1551
	// (get) Token: 0x060039DB RID: 14811 RVA: 0x00178CE4 File Offset: 0x00176EE4
	public override int Order
	{
		get
		{
			return 600;
		}
	}

	// Token: 0x17000610 RID: 1552
	// (get) Token: 0x060039DC RID: 14812 RVA: 0x00178CEB File Offset: 0x00176EEB
	public override string AuthorizerName
	{
		get
		{
			return "EAC";
		}
	}

	// Token: 0x17000611 RID: 1553
	// (get) Token: 0x060039DD RID: 14813 RVA: 0x00178CF2 File Offset: 0x00176EF2
	public override string StateLocalizationKey
	{
		get
		{
			return "authstate_eac";
		}
	}

	// Token: 0x17000612 RID: 1554
	// (get) Token: 0x060039DE RID: 14814 RVA: 0x00178CF9 File Offset: 0x00176EF9
	public override bool AuthorizerActive
	{
		get
		{
			IAntiCheatServer antiCheatServer = PlatformManager.MultiPlatform.AntiCheatServer;
			return antiCheatServer != null && antiCheatServer.ServerEacEnabled();
		}
	}

	// Token: 0x060039DF RID: 14815 RVA: 0x00178D10 File Offset: 0x00176F10
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

	// Token: 0x060039E0 RID: 14816 RVA: 0x00178D45 File Offset: 0x00176F45
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

	// Token: 0x060039E1 RID: 14817 RVA: 0x00178D64 File Offset: 0x00176F64
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		IAntiCheatServer antiCheatServer = PlatformManager.MultiPlatform.AntiCheatServer;
		if (antiCheatServer != null)
		{
			antiCheatServer.RegisterUser(_clientInfo);
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.WaitAsync, null);
	}

	// Token: 0x060039E2 RID: 14818 RVA: 0x00178D97 File Offset: 0x00176F97
	[PublicizedFrom(EAccessModifier.Private)]
	public void authPlayerEacSuccessfulCallback(ClientInfo _cInfo)
	{
		_cInfo.acAuthDone = true;
		this.authResponsesHandler.AuthorizationAccepted(this, _cInfo);
	}

	// Token: 0x060039E3 RID: 14819 RVA: 0x001787EB File Offset: 0x001769EB
	[PublicizedFrom(EAccessModifier.Private)]
	public void kickPlayerCallback(ClientInfo _cInfo, GameUtils.KickPlayerData _kickData)
	{
		this.authResponsesHandler.AuthorizationDenied(this, _cInfo, _kickData);
	}

	// Token: 0x060039E4 RID: 14820 RVA: 0x00178DAD File Offset: 0x00176FAD
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
