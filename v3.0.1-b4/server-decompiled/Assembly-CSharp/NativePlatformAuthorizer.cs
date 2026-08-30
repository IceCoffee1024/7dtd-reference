using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200077F RID: 1919
[Preserve]
public class NativePlatformAuthorizer : AuthorizerAbs
{
	// Token: 0x170005CE RID: 1486
	// (get) Token: 0x0600391B RID: 14619 RVA: 0x00175BF3 File Offset: 0x00173DF3
	public override int Order
	{
		get
		{
			return 400;
		}
	}

	// Token: 0x170005CF RID: 1487
	// (get) Token: 0x0600391C RID: 14620 RVA: 0x00175BFA File Offset: 0x00173DFA
	public override string AuthorizerName
	{
		get
		{
			return "PlatformAuth";
		}
	}

	// Token: 0x170005D0 RID: 1488
	// (get) Token: 0x0600391D RID: 14621 RVA: 0x00175C01 File Offset: 0x00173E01
	public override string StateLocalizationKey
	{
		get
		{
			return "authstate_nativeplatform";
		}
	}

	// Token: 0x0600391E RID: 14622 RVA: 0x00175C08 File Offset: 0x00173E08
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

	// Token: 0x0600391F RID: 14623 RVA: 0x00175C90 File Offset: 0x00173E90
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

	// Token: 0x06003920 RID: 14624 RVA: 0x00175D00 File Offset: 0x00173F00
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

	// Token: 0x06003921 RID: 14625 RVA: 0x00175DA4 File Offset: 0x00173FA4
	[PublicizedFrom(EAccessModifier.Private)]
	public void authPlayerSteamSuccessfulCallback(ClientInfo _clientInfo)
	{
		this.authResponsesHandler.AuthorizationAccepted(this, _clientInfo);
	}

	// Token: 0x06003922 RID: 14626 RVA: 0x00175DB3 File Offset: 0x00173FB3
	[PublicizedFrom(EAccessModifier.Private)]
	public void kickPlayerCallback(ClientInfo _cInfo, GameUtils.KickPlayerData _kickData)
	{
		this.authResponsesHandler.AuthorizationDenied(this, _cInfo, _kickData);
	}

	// Token: 0x06003923 RID: 14627 RVA: 0x00175DC3 File Offset: 0x00173FC3
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
