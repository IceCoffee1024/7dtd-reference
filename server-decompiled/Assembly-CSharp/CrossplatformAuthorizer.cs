using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000795 RID: 1941
[Preserve]
public class CrossplatformAuthorizer : AuthorizerAbs
{
	// Token: 0x17000608 RID: 1544
	// (get) Token: 0x060039DC RID: 14812 RVA: 0x00179883 File Offset: 0x00177A83
	public override int Order
	{
		get
		{
			return 490;
		}
	}

	// Token: 0x17000609 RID: 1545
	// (get) Token: 0x060039DD RID: 14813 RVA: 0x0017988A File Offset: 0x00177A8A
	public override string AuthorizerName
	{
		get
		{
			return "CrossplatformAuth";
		}
	}

	// Token: 0x1700060A RID: 1546
	// (get) Token: 0x060039DE RID: 14814 RVA: 0x00179891 File Offset: 0x00177A91
	public override string StateLocalizationKey
	{
		get
		{
			return "authstate_crossplatform";
		}
	}

	// Token: 0x1700060B RID: 1547
	// (get) Token: 0x060039DF RID: 14815 RVA: 0x00179898 File Offset: 0x00177A98
	public override bool AuthorizerActive
	{
		get
		{
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			return ((crossplatformPlatform != null) ? crossplatformPlatform.AuthenticationServer : null) != null;
		}
	}

	// Token: 0x060039E0 RID: 14816 RVA: 0x001798B0 File Offset: 0x00177AB0
	public override void ServerStart()
	{
		base.ServerStart();
		foreach (KeyValuePair<EPlatformIdentifier, IPlatform> keyValuePair in PlatformManager.ServerPlatforms)
		{
			if (keyValuePair.Value.IsCrossplatform)
			{
				IAuthenticationServer authenticationServer = keyValuePair.Value.AuthenticationServer;
				if (authenticationServer != null)
				{
					authenticationServer.StartServer(new AuthenticationSuccessfulCallbackDelegate(this.authPlayerSteamSuccessfulCallback), new KickPlayerDelegate(this.kickPlayerCallback));
				}
			}
		}
	}

	// Token: 0x060039E1 RID: 14817 RVA: 0x00179938 File Offset: 0x00177B38
	public override void ServerStop()
	{
		base.ServerStop();
		foreach (KeyValuePair<EPlatformIdentifier, IPlatform> keyValuePair in PlatformManager.ServerPlatforms)
		{
			if (keyValuePair.Value.IsCrossplatform)
			{
				IAuthenticationServer authenticationServer = keyValuePair.Value.AuthenticationServer;
				if (authenticationServer != null)
				{
					authenticationServer.StopServer();
				}
			}
		}
	}

	// Token: 0x060039E2 RID: 14818 RVA: 0x001799A8 File Offset: 0x00177BA8
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		if (_clientInfo.CrossplatformId == null)
		{
			EAuthorizerSyncResult eauthorizerSyncResult = EAuthorizerSyncResult.SyncDeny;
			GameUtils.EKickReason ekickReason = GameUtils.EKickReason.WrongCrossPlatform;
			int num = 0;
			string text = EPlatformIdentifier.None.ToStringCached<EPlatformIdentifier>();
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(eauthorizerSyncResult, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(ekickReason, num, default(DateTime), text)));
		}
		EPlatformIdentifier platformIdentifier = _clientInfo.CrossplatformId.PlatformIdentifier;
		IPlatform platform = PlatformManager.InstanceForPlatformIdentifier(platformIdentifier);
		if (platform == null)
		{
			EAuthorizerSyncResult eauthorizerSyncResult2 = EAuthorizerSyncResult.SyncDeny;
			GameUtils.EKickReason ekickReason2 = GameUtils.EKickReason.UnsupportedPlatform;
			int num2 = 0;
			string text = platformIdentifier.ToStringCached<EPlatformIdentifier>();
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(eauthorizerSyncResult2, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(ekickReason2, num2, default(DateTime), text)));
		}
		if (platform.PlatformIdentifier != PlatformManager.CrossplatformPlatform.PlatformIdentifier)
		{
			EAuthorizerSyncResult eauthorizerSyncResult3 = EAuthorizerSyncResult.SyncDeny;
			GameUtils.EKickReason ekickReason3 = GameUtils.EKickReason.WrongCrossPlatform;
			int num3 = 0;
			string text = platformIdentifier.ToStringCached<EPlatformIdentifier>();
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(eauthorizerSyncResult3, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(ekickReason3, num3, default(DateTime), text)));
		}
		if (platform.AuthenticationServer == null)
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
		}
		EBeginUserAuthenticationResult ebeginUserAuthenticationResult = platform.AuthenticationServer.AuthenticateUser(_clientInfo);
		if (ebeginUserAuthenticationResult != EBeginUserAuthenticationResult.Ok)
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncDeny, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(GameUtils.EKickReason.CrossPlatformAuthenticationBeginFailed, (int)ebeginUserAuthenticationResult, default(DateTime), "")));
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.WaitAsync, null);
	}

	// Token: 0x060039E3 RID: 14819 RVA: 0x00179774 File Offset: 0x00177974
	[PublicizedFrom(EAccessModifier.Private)]
	public void authPlayerSteamSuccessfulCallback(ClientInfo _clientInfo)
	{
		this.authResponsesHandler.AuthorizationAccepted(this, _clientInfo);
	}

	// Token: 0x060039E4 RID: 14820 RVA: 0x00179783 File Offset: 0x00177983
	[PublicizedFrom(EAccessModifier.Private)]
	public void kickPlayerCallback(ClientInfo _cInfo, GameUtils.KickPlayerData _kickData)
	{
		this.authResponsesHandler.AuthorizationDenied(this, _cInfo, _kickData);
	}

	// Token: 0x060039E5 RID: 14821 RVA: 0x00179AB3 File Offset: 0x00177CB3
	public override void Disconnect(ClientInfo _clientInfo)
	{
		if (_clientInfo != null)
		{
			PlatformUserIdentifierAbs crossplatformId = _clientInfo.CrossplatformId;
			if (((crossplatformId != null) ? crossplatformId.ReadablePlatformUserIdentifier : null) != null)
			{
				IPlatform platform = PlatformManager.InstanceForPlatformIdentifier(_clientInfo.CrossplatformId.PlatformIdentifier);
				if (platform == null)
				{
					return;
				}
				IAuthenticationServer authenticationServer = platform.AuthenticationServer;
				if (authenticationServer == null)
				{
					return;
				}
				authenticationServer.RemoveUser(_clientInfo);
			}
		}
	}
}
