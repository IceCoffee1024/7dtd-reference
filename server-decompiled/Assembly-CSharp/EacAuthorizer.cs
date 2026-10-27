using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000798 RID: 1944
[Preserve]
public class EacAuthorizer : AuthorizerAbs
{
	// Token: 0x17000613 RID: 1555
	// (get) Token: 0x060039F2 RID: 14834 RVA: 0x00179C7C File Offset: 0x00177E7C
	public override int Order
	{
		get
		{
			return 600;
		}
	}

	// Token: 0x17000614 RID: 1556
	// (get) Token: 0x060039F3 RID: 14835 RVA: 0x00179C83 File Offset: 0x00177E83
	public override string AuthorizerName
	{
		get
		{
			return "EAC";
		}
	}

	// Token: 0x17000615 RID: 1557
	// (get) Token: 0x060039F4 RID: 14836 RVA: 0x00179C8A File Offset: 0x00177E8A
	public override string StateLocalizationKey
	{
		get
		{
			return "authstate_eac";
		}
	}

	// Token: 0x17000616 RID: 1558
	// (get) Token: 0x060039F5 RID: 14837 RVA: 0x00179C91 File Offset: 0x00177E91
	public override bool AuthorizerActive
	{
		get
		{
			IAntiCheatServer antiCheatServer = PlatformManager.MultiPlatform.AntiCheatServer;
			return antiCheatServer != null && antiCheatServer.ServerEacEnabled();
		}
	}

	// Token: 0x060039F6 RID: 14838 RVA: 0x00179CA8 File Offset: 0x00177EA8
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

	// Token: 0x060039F7 RID: 14839 RVA: 0x00179CDD File Offset: 0x00177EDD
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

	// Token: 0x060039F8 RID: 14840 RVA: 0x00179CFC File Offset: 0x00177EFC
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		IAntiCheatServer antiCheatServer = PlatformManager.MultiPlatform.AntiCheatServer;
		if (antiCheatServer != null)
		{
			antiCheatServer.RegisterUser(_clientInfo);
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.WaitAsync, null);
	}

	// Token: 0x060039F9 RID: 14841 RVA: 0x00179D2F File Offset: 0x00177F2F
	[PublicizedFrom(EAccessModifier.Private)]
	public void authPlayerEacSuccessfulCallback(ClientInfo _cInfo)
	{
		_cInfo.acAuthDone = true;
		this.authResponsesHandler.AuthorizationAccepted(this, _cInfo);
	}

	// Token: 0x060039FA RID: 14842 RVA: 0x00179783 File Offset: 0x00177983
	[PublicizedFrom(EAccessModifier.Private)]
	public void kickPlayerCallback(ClientInfo _cInfo, GameUtils.KickPlayerData _kickData)
	{
		this.authResponsesHandler.AuthorizationDenied(this, _cInfo, _kickData);
	}

	// Token: 0x060039FB RID: 14843 RVA: 0x00179D45 File Offset: 0x00177F45
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
