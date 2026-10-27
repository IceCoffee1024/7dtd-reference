using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000799 RID: 1945
[Preserve]
public class AntiCheatEncryptionAgreementAuthorizer : AuthorizerAbs
{
	// Token: 0x17000617 RID: 1559
	// (get) Token: 0x060039FD RID: 14845 RVA: 0x00179D5C File Offset: 0x00177F5C
	public override int Order
	{
		get
		{
			return 601;
		}
	}

	// Token: 0x17000618 RID: 1560
	// (get) Token: 0x060039FE RID: 14846 RVA: 0x00179D63 File Offset: 0x00177F63
	public override string AuthorizerName
	{
		get
		{
			return "Encryption";
		}
	}

	// Token: 0x17000619 RID: 1561
	// (get) Token: 0x060039FF RID: 14847 RVA: 0x00179D6A File Offset: 0x00177F6A
	public override string StateLocalizationKey
	{
		get
		{
			return "authstate_encryption";
		}
	}

	// Token: 0x06003A00 RID: 14848 RVA: 0x00179D71 File Offset: 0x00177F71
	public override void ServerStart()
	{
		base.ServerStart();
		SingletonMonoBehaviour<ConnectionManager>.Instance.AntiCheatEncryptionAuthServer.Start(new AntiCheatEncryptionAuthServer.KeyExchangeCompleteDelegate(this.KeyExchangeCompleted), new AntiCheatEncryptionAuthServer.KeyExchangeFailedDelegate(this.KeyExchangeFailed));
	}

	// Token: 0x06003A01 RID: 14849 RVA: 0x00179DA0 File Offset: 0x00177FA0
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

	// Token: 0x06003A02 RID: 14850 RVA: 0x00179E59 File Offset: 0x00178059
	[PublicizedFrom(EAccessModifier.Private)]
	public void KeyExchangeCompleted(ClientInfo _clientInfo, IEncryptionModule _encryptionModule)
	{
		_clientInfo.SetAntiCheatEncryption(_encryptionModule);
		this.authResponsesHandler.AuthorizationAccepted(this, _clientInfo);
	}

	// Token: 0x06003A03 RID: 14851 RVA: 0x00179783 File Offset: 0x00177983
	[PublicizedFrom(EAccessModifier.Private)]
	public void KeyExchangeFailed(ClientInfo _clientInfo, GameUtils.KickPlayerData _reason)
	{
		this.authResponsesHandler.AuthorizationDenied(this, _clientInfo, _reason);
	}

	// Token: 0x06003A04 RID: 14852 RVA: 0x00179E6F File Offset: 0x0017806F
	public override void ServerStop()
	{
		base.ServerStop();
		SingletonMonoBehaviour<ConnectionManager>.Instance.AntiCheatEncryptionAuthServer.Stop();
	}
}
