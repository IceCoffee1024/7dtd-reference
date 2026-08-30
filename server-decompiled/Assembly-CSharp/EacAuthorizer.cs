using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200078E RID: 1934
[Preserve]
public class EacAuthorizer : AuthorizerAbs
{
	// Token: 0x170005EA RID: 1514
	// (get) Token: 0x06003999 RID: 14745 RVA: 0x0017852C File Offset: 0x0017672C
	public override int Order
	{
		get
		{
			return 600;
		}
	}

	// Token: 0x170005EB RID: 1515
	// (get) Token: 0x0600399A RID: 14746 RVA: 0x00178533 File Offset: 0x00176733
	public override string AuthorizerName
	{
		get
		{
			return "EAC";
		}
	}

	// Token: 0x170005EC RID: 1516
	// (get) Token: 0x0600399B RID: 14747 RVA: 0x0017853A File Offset: 0x0017673A
	public override string StateLocalizationKey
	{
		get
		{
			return "authstate_eac";
		}
	}

	// Token: 0x170005ED RID: 1517
	// (get) Token: 0x0600399C RID: 14748 RVA: 0x00178541 File Offset: 0x00176741
	public override bool AuthorizerActive
	{
		get
		{
			IAntiCheatServer antiCheatServer = PlatformManager.MultiPlatform.AntiCheatServer;
			return antiCheatServer != null && antiCheatServer.ServerEacEnabled();
		}
	}

	// Token: 0x0600399D RID: 14749 RVA: 0x00178558 File Offset: 0x00176758
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

	// Token: 0x0600399E RID: 14750 RVA: 0x0017858D File Offset: 0x0017678D
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

	// Token: 0x0600399F RID: 14751 RVA: 0x001785AC File Offset: 0x001767AC
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		IAntiCheatServer antiCheatServer = PlatformManager.MultiPlatform.AntiCheatServer;
		if (antiCheatServer != null)
		{
			antiCheatServer.RegisterUser(_clientInfo);
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.WaitAsync, null);
	}

	// Token: 0x060039A0 RID: 14752 RVA: 0x001785DF File Offset: 0x001767DF
	[PublicizedFrom(EAccessModifier.Private)]
	public void authPlayerEacSuccessfulCallback(ClientInfo _cInfo)
	{
		_cInfo.acAuthDone = true;
		this.authResponsesHandler.AuthorizationAccepted(this, _cInfo);
	}

	// Token: 0x060039A1 RID: 14753 RVA: 0x00178033 File Offset: 0x00176233
	[PublicizedFrom(EAccessModifier.Private)]
	public void kickPlayerCallback(ClientInfo _cInfo, GameUtils.KickPlayerData _kickData)
	{
		this.authResponsesHandler.AuthorizationDenied(this, _cInfo, _kickData);
	}

	// Token: 0x060039A2 RID: 14754 RVA: 0x001785F5 File Offset: 0x001767F5
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
