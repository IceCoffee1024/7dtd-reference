using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000785 RID: 1925
[Preserve]
public class AntiCheatEncryptionAgreementAuthorizer : AuthorizerAbs
{
	// Token: 0x170005E4 RID: 1508
	// (get) Token: 0x0600394C RID: 14668 RVA: 0x0017638C File Offset: 0x0017458C
	public override int Order
	{
		get
		{
			return 601;
		}
	}

	// Token: 0x170005E5 RID: 1509
	// (get) Token: 0x0600394D RID: 14669 RVA: 0x00176393 File Offset: 0x00174593
	public override string AuthorizerName
	{
		get
		{
			return "Encryption";
		}
	}

	// Token: 0x170005E6 RID: 1510
	// (get) Token: 0x0600394E RID: 14670 RVA: 0x0017639A File Offset: 0x0017459A
	public override string StateLocalizationKey
	{
		get
		{
			return "authstate_encryption";
		}
	}

	// Token: 0x0600394F RID: 14671 RVA: 0x001763A1 File Offset: 0x001745A1
	public override void ServerStart()
	{
		base.ServerStart();
		SingletonMonoBehaviour<ConnectionManager>.Instance.AntiCheatEncryptionAuthServer.Start(new AntiCheatEncryptionAuthServer.KeyExchangeCompleteDelegate(this.KeyExchangeCompleted), new AntiCheatEncryptionAuthServer.KeyExchangeFailedDelegate(this.KeyExchangeFailed));
	}

	// Token: 0x06003950 RID: 14672 RVA: 0x001763D0 File Offset: 0x001745D0
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		IAntiCheatServer antiCheatServer = PlatformManager.MultiPlatform.AntiCheatServer;
		if (antiCheatServer != null && antiCheatServer.EncryptionAvailable() && _clientInfo.requiresAntiCheat)
		{
			if (_clientInfo.acAuthDone)
			{
				_clientInfo.SetAntiCheatEncryption(PlatformManager.MultiPlatform.AntiCheatServer);
			}
			else
			{
				Log.Warning("Server EAC AntiCheat encryption is available but " + _clientInfo.playerName + " did not complete EAC auth, encryption is disabled for this client");
			}
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
		}
		if (!GameManager.IsDedicatedServer)
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
		}
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.AntiCheatEncryptionAuthServer.TryStartKeyExchange(_clientInfo))
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.WaitAsync, null);
	}

	// Token: 0x06003951 RID: 14673 RVA: 0x00176489 File Offset: 0x00174689
	[PublicizedFrom(EAccessModifier.Private)]
	public void KeyExchangeCompleted(ClientInfo _clientInfo, IEncryptionModule _encryptionModule)
	{
		_clientInfo.SetAntiCheatEncryption(_encryptionModule);
		this.authResponsesHandler.AuthorizationAccepted(this, _clientInfo);
	}

	// Token: 0x06003952 RID: 14674 RVA: 0x00175DB3 File Offset: 0x00173FB3
	[PublicizedFrom(EAccessModifier.Private)]
	public void KeyExchangeFailed(ClientInfo _clientInfo, GameUtils.KickPlayerData _reason)
	{
		this.authResponsesHandler.AuthorizationDenied(this, _clientInfo, _reason);
	}

	// Token: 0x06003953 RID: 14675 RVA: 0x0017649F File Offset: 0x0017469F
	public override void ServerStop()
	{
		base.ServerStop();
		SingletonMonoBehaviour<ConnectionManager>.Instance.AntiCheatEncryptionAuthServer.Stop();
	}
}
