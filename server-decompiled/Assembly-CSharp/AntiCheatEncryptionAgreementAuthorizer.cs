using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000791 RID: 1937
[Preserve]
public class AntiCheatEncryptionAgreementAuthorizer : AuthorizerAbs
{
	// Token: 0x17000613 RID: 1555
	// (get) Token: 0x060039E6 RID: 14822 RVA: 0x00178DC4 File Offset: 0x00176FC4
	public override int Order
	{
		get
		{
			return 601;
		}
	}

	// Token: 0x17000614 RID: 1556
	// (get) Token: 0x060039E7 RID: 14823 RVA: 0x00178DCB File Offset: 0x00176FCB
	public override string AuthorizerName
	{
		get
		{
			return "Encryption";
		}
	}

	// Token: 0x17000615 RID: 1557
	// (get) Token: 0x060039E8 RID: 14824 RVA: 0x00178DD2 File Offset: 0x00176FD2
	public override string StateLocalizationKey
	{
		get
		{
			return "authstate_encryption";
		}
	}

	// Token: 0x060039E9 RID: 14825 RVA: 0x00178DD9 File Offset: 0x00176FD9
	public override void ServerStart()
	{
		base.ServerStart();
		SingletonMonoBehaviour<ConnectionManager>.Instance.AntiCheatEncryptionAuthServer.Start(new AntiCheatEncryptionAuthServer.KeyExchangeCompleteDelegate(this.KeyExchangeCompleted), new AntiCheatEncryptionAuthServer.KeyExchangeFailedDelegate(this.KeyExchangeFailed));
	}

	// Token: 0x060039EA RID: 14826 RVA: 0x00178E08 File Offset: 0x00177008
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

	// Token: 0x060039EB RID: 14827 RVA: 0x00178EC1 File Offset: 0x001770C1
	[PublicizedFrom(EAccessModifier.Private)]
	public void KeyExchangeCompleted(ClientInfo _clientInfo, IEncryptionModule _encryptionModule)
	{
		_clientInfo.SetAntiCheatEncryption(_encryptionModule);
		this.authResponsesHandler.AuthorizationAccepted(this, _clientInfo);
	}

	// Token: 0x060039EC RID: 14828 RVA: 0x001787EB File Offset: 0x001769EB
	[PublicizedFrom(EAccessModifier.Private)]
	public void KeyExchangeFailed(ClientInfo _clientInfo, GameUtils.KickPlayerData _reason)
	{
		this.authResponsesHandler.AuthorizationDenied(this, _clientInfo, _reason);
	}

	// Token: 0x060039ED RID: 14829 RVA: 0x00178ED7 File Offset: 0x001770D7
	public override void ServerStop()
	{
		base.ServerStop();
		SingletonMonoBehaviour<ConnectionManager>.Instance.AntiCheatEncryptionAuthServer.Stop();
	}
}
