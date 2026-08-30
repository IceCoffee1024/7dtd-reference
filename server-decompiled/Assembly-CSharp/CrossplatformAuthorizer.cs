using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200078B RID: 1931
[Preserve]
public class CrossplatformAuthorizer : AuthorizerAbs
{
	// Token: 0x170005DF RID: 1503
	// (get) Token: 0x06003983 RID: 14723 RVA: 0x00178133 File Offset: 0x00176333
	public override int Order
	{
		get
		{
			return 490;
		}
	}

	// Token: 0x170005E0 RID: 1504
	// (get) Token: 0x06003984 RID: 14724 RVA: 0x0017813A File Offset: 0x0017633A
	public override string AuthorizerName
	{
		get
		{
			return "CrossplatformAuth";
		}
	}

	// Token: 0x170005E1 RID: 1505
	// (get) Token: 0x06003985 RID: 14725 RVA: 0x00178141 File Offset: 0x00176341
	public override string StateLocalizationKey
	{
		get
		{
			return "authstate_crossplatform";
		}
	}

	// Token: 0x170005E2 RID: 1506
	// (get) Token: 0x06003986 RID: 14726 RVA: 0x00178148 File Offset: 0x00176348
	public override bool AuthorizerActive
	{
		get
		{
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			return ((crossplatformPlatform != null) ? crossplatformPlatform.AuthenticationServer : null) != null;
		}
	}

	// Token: 0x06003987 RID: 14727 RVA: 0x00178160 File Offset: 0x00176360
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

	// Token: 0x06003988 RID: 14728 RVA: 0x001781E8 File Offset: 0x001763E8
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

	// Token: 0x06003989 RID: 14729 RVA: 0x00178258 File Offset: 0x00176458
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

	// Token: 0x0600398A RID: 14730 RVA: 0x00178024 File Offset: 0x00176224
	[PublicizedFrom(EAccessModifier.Private)]
	public void authPlayerSteamSuccessfulCallback(ClientInfo _clientInfo)
	{
		this.authResponsesHandler.AuthorizationAccepted(this, _clientInfo);
	}

	// Token: 0x0600398B RID: 14731 RVA: 0x00178033 File Offset: 0x00176233
	[PublicizedFrom(EAccessModifier.Private)]
	public void kickPlayerCallback(ClientInfo _cInfo, GameUtils.KickPlayerData _kickData)
	{
		this.authResponsesHandler.AuthorizationDenied(this, _cInfo, _kickData);
	}

	// Token: 0x0600398C RID: 14732 RVA: 0x00178363 File Offset: 0x00176563
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
