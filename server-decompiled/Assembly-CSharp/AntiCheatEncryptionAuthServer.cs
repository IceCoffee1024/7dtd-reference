using System;
using System.Collections.Generic;
using System.Security.Cryptography;

// Token: 0x0200076D RID: 1901
public class AntiCheatEncryptionAuthServer
{
	// Token: 0x0600387D RID: 14461 RVA: 0x001724DF File Offset: 0x001706DF
	public void Start(AntiCheatEncryptionAuthServer.KeyExchangeCompleteDelegate completeDelegate, AntiCheatEncryptionAuthServer.KeyExchangeFailedDelegate failedDelegate)
	{
		this.OnExchangeCompleted = completeDelegate;
		this.OnExchangeFailed = failedDelegate;
		this.pendingEncryptionModules = new Dictionary<ClientInfo, IEncryptionModule>();
		ConnectionManager.OnClientDisconnected += this.OnClientDisconnected;
	}

	// Token: 0x0600387E RID: 14462 RVA: 0x0017250C File Offset: 0x0017070C
	public bool TryStartKeyExchange(ClientInfo clientInfo)
	{
		if (!AntiCheatEncryptionAuthServer.platformPublicKeys.ContainsKey(clientInfo.device))
		{
			Log.Out(string.Format("[EncryptionAgreement] Unsupported client device {0}", clientInfo.device));
			return false;
		}
		Log.Out("[EncryptionAgreement] starting key exchange for " + clientInfo.playerName);
		clientInfo.SendPackage(NetPackageManager.GetPackage<NetPackageEncryptionRequest>().Setup());
		return true;
	}

	// Token: 0x0600387F RID: 14463 RVA: 0x00172570 File Offset: 0x00170770
	public void SendSharedKey(ClientInfo clientInfo, string exchangePublicKeyParamsXml, byte[] hash, byte[] signedHash)
	{
		string text;
		if (AntiCheatEncryptionAuthServer.platformPublicKeys.TryGetValue(clientInfo.device, out text))
		{
			Log.Out("[EncryptionAgreement] Client " + clientInfo.playerName + " authenticating device");
			using (RSA rsa = RSA.Create())
			{
				rsa.FromXmlString(text);
				RSAPKCS1SignatureDeformatter rsapkcs1SignatureDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
				rsapkcs1SignatureDeformatter.SetHashAlgorithm("SHA512");
				if (!rsapkcs1SignatureDeformatter.VerifySignature(hash, signedHash))
				{
					Log.Warning("[EncryptionAgreement] Client " + clientInfo.playerName + " signature failed to verify");
					AntiCheatEncryptionAuthServer.KeyExchangeFailedDelegate onExchangeFailed = this.OnExchangeFailed;
					if (onExchangeFailed != null)
					{
						onExchangeFailed(clientInfo, new GameUtils.KickPlayerData(GameUtils.EKickReason.EncryptionAgreementInvalidSignature, 0, default(DateTime), ""));
					}
				}
				else
				{
					AesEncryptAndMac aesEncryptAndMac = new AesEncryptAndMac();
					using (RSA rsa2 = RSA.Create())
					{
						rsa2.FromXmlString(exchangePublicKeyParamsXml);
						byte[] array = rsa2.Encrypt(aesEncryptAndMac.EncryptionKey, RSAEncryptionPadding.Pkcs1);
						byte[] array2 = rsa2.Encrypt(aesEncryptAndMac.IntegrityKey, RSAEncryptionPadding.Pkcs1);
						clientInfo.SendPackage(NetPackageManager.GetPackage<NetPackageEncryptionSharedKey>().Setup(array, array2));
						this.pendingEncryptionModules.Add(clientInfo, aesEncryptAndMac);
					}
				}
			}
			return;
		}
		Log.Error(string.Format("[EncryptionAgreement] Cannot complete key exchange for {0}, unsupported client device {1}", clientInfo.playerName, clientInfo.device));
		AntiCheatEncryptionAuthServer.KeyExchangeFailedDelegate onExchangeFailed2 = this.OnExchangeFailed;
		if (onExchangeFailed2 == null)
		{
			return;
		}
		onExchangeFailed2(clientInfo, new GameUtils.KickPlayerData(GameUtils.EKickReason.EncryptionAgreementError, 0, default(DateTime), ""));
	}

	// Token: 0x06003880 RID: 14464 RVA: 0x001726F0 File Offset: 0x001708F0
	public void CompleteKeyExchange(ClientInfo clientInfo, bool wasSuccessful)
	{
		IEncryptionModule encryptionModule;
		if (!wasSuccessful)
		{
			Log.Error("[EncryptionAgreement] Client " + clientInfo.playerName + " had an error during key exchange");
			this.pendingEncryptionModules.Remove(clientInfo);
			AntiCheatEncryptionAuthServer.KeyExchangeFailedDelegate onExchangeFailed = this.OnExchangeFailed;
			if (onExchangeFailed == null)
			{
				return;
			}
			onExchangeFailed(clientInfo, new GameUtils.KickPlayerData(GameUtils.EKickReason.EncryptionAgreementError, 0, default(DateTime), ""));
			return;
		}
		else if (!this.pendingEncryptionModules.TryGetValue(clientInfo, out encryptionModule))
		{
			Log.Error("[EncryptionAgreement] Client " + clientInfo.playerName + " tried to complete key exchange but had no encryption module ready");
			AntiCheatEncryptionAuthServer.KeyExchangeFailedDelegate onExchangeFailed2 = this.OnExchangeFailed;
			if (onExchangeFailed2 == null)
			{
				return;
			}
			onExchangeFailed2(clientInfo, new GameUtils.KickPlayerData(GameUtils.EKickReason.EncryptionAgreementError, 0, default(DateTime), ""));
			return;
		}
		else
		{
			Log.Out("[EncryptionAgreement] Client " + clientInfo.playerName + " enabling encryption");
			AntiCheatEncryptionAuthServer.KeyExchangeCompleteDelegate onExchangeCompleted = this.OnExchangeCompleted;
			if (onExchangeCompleted == null)
			{
				return;
			}
			onExchangeCompleted(clientInfo, encryptionModule);
			return;
		}
	}

	// Token: 0x06003881 RID: 14465 RVA: 0x001727CD File Offset: 0x001709CD
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnClientDisconnected(ClientInfo clientInfo)
	{
		this.CancelKeyExchange(clientInfo);
	}

	// Token: 0x06003882 RID: 14466 RVA: 0x001727D6 File Offset: 0x001709D6
	public void CancelKeyExchange(ClientInfo clientInfo)
	{
		this.pendingEncryptionModules.Remove(clientInfo);
	}

	// Token: 0x06003883 RID: 14467 RVA: 0x001727E5 File Offset: 0x001709E5
	public void Stop()
	{
		ConnectionManager.OnClientDisconnected -= this.OnClientDisconnected;
		Dictionary<ClientInfo, IEncryptionModule> dictionary = this.pendingEncryptionModules;
		if (dictionary != null)
		{
			dictionary.Clear();
		}
		this.pendingEncryptionModules = null;
		this.OnExchangeCompleted = null;
		this.OnExchangeFailed = null;
	}

	// Token: 0x04002E4E RID: 11854
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<ClientInfo.EDeviceType, string> platformPublicKeys = new Dictionary<ClientInfo.EDeviceType, string>
	{
		{
			ClientInfo.EDeviceType.Xbox,
			"<RSAKeyValue><Modulus>o/zBUXWrPeOf/slmoNVAAtXZwk59QG3tVlcYafh2TcXZxsFLeWEpluIANHDQrRe9O1RGfCAxSd/ikpe/lfmsh8zP4Zcu7ZjQF8IMkQOIYJhbAAM+nVKM6FA06eKm15OyTJGvFWAbFD/TtZN1VFEZ25wveksFfYPEQYrA8LaY9iU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"
		},
		{
			ClientInfo.EDeviceType.PlayStation,
			"<RSAKeyValue><Modulus>gClxi32aMHwKq/AWx83x6s8TWZ1Hdi/ONARtm34O5C8JJ4ma96m8iL9ITkn5p8tpunFPRXjdjxbxjGTm+E7vnpTA+aR0SQzcFdYpRkHDA3BwEY86Qr10Bj7TK9prxlf7Jf3eGyb/tV52jTC7YD+w5yaE8EzO5bJTRKrwGIiIZOk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"
		}
	};

	// Token: 0x04002E4F RID: 11855
	[PublicizedFrom(EAccessModifier.Private)]
	public AntiCheatEncryptionAuthServer.KeyExchangeCompleteDelegate OnExchangeCompleted;

	// Token: 0x04002E50 RID: 11856
	[PublicizedFrom(EAccessModifier.Private)]
	public AntiCheatEncryptionAuthServer.KeyExchangeFailedDelegate OnExchangeFailed;

	// Token: 0x04002E51 RID: 11857
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<ClientInfo, IEncryptionModule> pendingEncryptionModules;

	// Token: 0x0200076E RID: 1902
	// (Invoke) Token: 0x06003887 RID: 14471
	public delegate void KeyExchangeCompleteDelegate(ClientInfo clientInfo, IEncryptionModule encryptionModule);

	// Token: 0x0200076F RID: 1903
	// (Invoke) Token: 0x0600388B RID: 14475
	public delegate void KeyExchangeFailedDelegate(ClientInfo clientInfo, GameUtils.KickPlayerData reason);
}
