using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000789 RID: 1929
[Preserve]
public class NativePlatformAuthorizer : AuthorizerAbs
{
	// Token: 0x170005D8 RID: 1496
	// (get) Token: 0x06003973 RID: 14707 RVA: 0x00177E73 File Offset: 0x00176073
	public override int Order
	{
		get
		{
			return 400;
		}
	}

	// Token: 0x170005D9 RID: 1497
	// (get) Token: 0x06003974 RID: 14708 RVA: 0x00177E7A File Offset: 0x0017607A
	public override string AuthorizerName
	{
		get
		{
			return "PlatformAuth";
		}
	}

	// Token: 0x170005DA RID: 1498
	// (get) Token: 0x06003975 RID: 14709 RVA: 0x00177E81 File Offset: 0x00176081
	public override string StateLocalizationKey
	{
		get
		{
			return "authstate_nativeplatform";
		}
	}

	// Token: 0x06003976 RID: 14710 RVA: 0x00177E88 File Offset: 0x00176088
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

	// Token: 0x06003977 RID: 14711 RVA: 0x00177F10 File Offset: 0x00176110
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

	// Token: 0x06003978 RID: 14712 RVA: 0x00177F80 File Offset: 0x00176180
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

	// Token: 0x06003979 RID: 14713 RVA: 0x00178024 File Offset: 0x00176224
	[PublicizedFrom(EAccessModifier.Private)]
	public void authPlayerSteamSuccessfulCallback(ClientInfo _clientInfo)
	{
		this.authResponsesHandler.AuthorizationAccepted(this, _clientInfo);
	}

	// Token: 0x0600397A RID: 14714 RVA: 0x00178033 File Offset: 0x00176233
	[PublicizedFrom(EAccessModifier.Private)]
	public void kickPlayerCallback(ClientInfo _cInfo, GameUtils.KickPlayerData _kickData)
	{
		this.authResponsesHandler.AuthorizationDenied(this, _cInfo, _kickData);
	}

	// Token: 0x0600397B RID: 14715 RVA: 0x00178043 File Offset: 0x00176243
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
