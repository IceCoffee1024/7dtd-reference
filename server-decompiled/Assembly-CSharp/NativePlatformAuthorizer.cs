using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200078B RID: 1931
[Preserve]
public class NativePlatformAuthorizer : AuthorizerAbs
{
	// Token: 0x170005FD RID: 1533
	// (get) Token: 0x060039B5 RID: 14773 RVA: 0x0017862B File Offset: 0x0017682B
	public override int Order
	{
		get
		{
			return 400;
		}
	}

	// Token: 0x170005FE RID: 1534
	// (get) Token: 0x060039B6 RID: 14774 RVA: 0x00178632 File Offset: 0x00176832
	public override string AuthorizerName
	{
		get
		{
			return "PlatformAuth";
		}
	}

	// Token: 0x170005FF RID: 1535
	// (get) Token: 0x060039B7 RID: 14775 RVA: 0x00178639 File Offset: 0x00176839
	public override string StateLocalizationKey
	{
		get
		{
			return "authstate_nativeplatform";
		}
	}

	// Token: 0x060039B8 RID: 14776 RVA: 0x00178640 File Offset: 0x00176840
	public override void ServerStart()
	{
		base.ServerStart();
		foreach (KeyValuePair<EPlatformIdentifier, IPlatform> keyValuePair in PlatformManager.ServerPlatforms)
		{
			if (!keyValuePair.Value.IsCrossplatform)
			{
				IAuthenticationServer authenticationServer = keyValuePair.Value.AuthenticationServer;
				if (authenticationServer != null)
				{
					authenticationServer.StartServer(new AuthenticationSuccessfulCallbackDelegate(this.authPlayerSteamSuccessfulCallback), new KickPlayerDelegate(this.kickPlayerCallback));
				}
			}
		}
	}

	// Token: 0x060039B9 RID: 14777 RVA: 0x001786C8 File Offset: 0x001768C8
	public override void ServerStop()
	{
		base.ServerStop();
		foreach (KeyValuePair<EPlatformIdentifier, IPlatform> keyValuePair in PlatformManager.ServerPlatforms)
		{
			if (!keyValuePair.Value.IsCrossplatform)
			{
				IAuthenticationServer authenticationServer = keyValuePair.Value.AuthenticationServer;
				if (authenticationServer != null)
				{
					authenticationServer.StopServer();
				}
			}
		}
	}

	// Token: 0x060039BA RID: 14778 RVA: 0x00178738 File Offset: 0x00176938
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		EPlatformIdentifier platformIdentifier = _clientInfo.PlatformId.PlatformIdentifier;
		IPlatform platform = PlatformManager.InstanceForPlatformIdentifier(platformIdentifier);
		if (platform == null)
		{
			EAuthorizerSyncResult eauthorizerSyncResult = EAuthorizerSyncResult.SyncDeny;
			GameUtils.EKickReason ekickReason = GameUtils.EKickReason.UnsupportedPlatform;
			int num = 0;
			string text = platformIdentifier.ToStringCached<EPlatformIdentifier>();
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(eauthorizerSyncResult, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(ekickReason, num, default(DateTime), text)));
		}
		if (platform.AuthenticationServer == null)
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
		}
		EBeginUserAuthenticationResult ebeginUserAuthenticationResult = platform.AuthenticationServer.AuthenticateUser(_clientInfo);
		if (ebeginUserAuthenticationResult != EBeginUserAuthenticationResult.Ok)
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncDeny, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(GameUtils.EKickReason.PlatformAuthenticationBeginFailed, (int)ebeginUserAuthenticationResult, default(DateTime), "")));
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.WaitAsync, null);
	}

	// Token: 0x060039BB RID: 14779 RVA: 0x001787DC File Offset: 0x001769DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void authPlayerSteamSuccessfulCallback(ClientInfo _clientInfo)
	{
		this.authResponsesHandler.AuthorizationAccepted(this, _clientInfo);
	}

	// Token: 0x060039BC RID: 14780 RVA: 0x001787EB File Offset: 0x001769EB
	[PublicizedFrom(EAccessModifier.Private)]
	public void kickPlayerCallback(ClientInfo _cInfo, GameUtils.KickPlayerData _kickData)
	{
		this.authResponsesHandler.AuthorizationDenied(this, _cInfo, _kickData);
	}

	// Token: 0x060039BD RID: 14781 RVA: 0x001787FB File Offset: 0x001769FB
	public override void Disconnect(ClientInfo _clientInfo)
	{
		if (_clientInfo != null)
		{
			PlatformUserIdentifierAbs platformId = _clientInfo.PlatformId;
			if (((platformId != null) ? platformId.ReadablePlatformUserIdentifier : null) != null)
			{
				IPlatform platform = PlatformManager.InstanceForPlatformIdentifier(_clientInfo.PlatformId.PlatformIdentifier);
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
