using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200078D RID: 1933
[Preserve]
public class CrossplatformAuthorizer : AuthorizerAbs
{
	// Token: 0x17000604 RID: 1540
	// (get) Token: 0x060039C5 RID: 14789 RVA: 0x001788EB File Offset: 0x00176AEB
	public override int Order
	{
		get
		{
			return 490;
		}
	}

	// Token: 0x17000605 RID: 1541
	// (get) Token: 0x060039C6 RID: 14790 RVA: 0x001788F2 File Offset: 0x00176AF2
	public override string AuthorizerName
	{
		get
		{
			return "CrossplatformAuth";
		}
	}

	// Token: 0x17000606 RID: 1542
	// (get) Token: 0x060039C7 RID: 14791 RVA: 0x001788F9 File Offset: 0x00176AF9
	public override string StateLocalizationKey
	{
		get
		{
			return "authstate_crossplatform";
		}
	}

	// Token: 0x17000607 RID: 1543
	// (get) Token: 0x060039C8 RID: 14792 RVA: 0x00178900 File Offset: 0x00176B00
	public override bool AuthorizerActive
	{
		get
		{
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			return ((crossplatformPlatform != null) ? crossplatformPlatform.AuthenticationServer : null) != null;
		}
	}

	// Token: 0x060039C9 RID: 14793 RVA: 0x00178918 File Offset: 0x00176B18
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

	// Token: 0x060039CA RID: 14794 RVA: 0x001789A0 File Offset: 0x00176BA0
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

	// Token: 0x060039CB RID: 14795 RVA: 0x00178A10 File Offset: 0x00176C10
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

	// Token: 0x060039CC RID: 14796 RVA: 0x001787DC File Offset: 0x001769DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void authPlayerSteamSuccessfulCallback(ClientInfo _clientInfo)
	{
		this.authResponsesHandler.AuthorizationAccepted(this, _clientInfo);
	}

	// Token: 0x060039CD RID: 14797 RVA: 0x001787EB File Offset: 0x001769EB
	[PublicizedFrom(EAccessModifier.Private)]
	public void kickPlayerCallback(ClientInfo _cInfo, GameUtils.KickPlayerData _kickData)
	{
		this.authResponsesHandler.AuthorizationDenied(this, _cInfo, _kickData);
	}

	// Token: 0x060039CE RID: 14798 RVA: 0x00178B1B File Offset: 0x00176D1B
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
