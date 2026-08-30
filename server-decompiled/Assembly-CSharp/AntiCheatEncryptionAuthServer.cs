using System;
using System.Collections.Generic;
using System.Security.Cryptography;

// Token: 0x02000759 RID: 1881
public class AntiCheatEncryptionAuthServer
{
	// Token: 0x060037CD RID: 14285 RVA: 0x0016E9C7 File Offset: 0x0016CBC7
	public void Start(AntiCheatEncryptionAuthServer.KeyExchangeCompleteDelegate completeDelegate, AntiCheatEncryptionAuthServer.KeyExchangeFailedDelegate failedDelegate)
	{
		this.OnExchangeCompleted = completeDelegate;
		this.OnExchangeFailed = failedDelegate;
		this.pendingEncryptionModules = new Dictionary<ClientInfo, IEncryptionModule>();
		ConnectionManager.OnClientDisconnected += this.OnClientDisconnected;
	}

	// Token: 0x060037CE RID: 14286 RVA: 0x0016E9F4 File Offset: 0x0016CBF4
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

	// Token: 0x060037CF RID: 14287 RVA: 0x0016EA58 File Offset: 0x0016CC58
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

	// Token: 0x060037D0 RID: 14288 RVA: 0x0016EBD8 File Offset: 0x0016CDD8
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

	// Token: 0x060037D1 RID: 14289 RVA: 0x0016ECB5 File Offset: 0x0016CEB5
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnClientDisconnected(ClientInfo clientInfo)
	{
		this.CancelKeyExchange(clientInfo);
	}

	// Token: 0x060037D2 RID: 14290 RVA: 0x0016ECBE File Offset: 0x0016CEBE
	public void CancelKeyExchange(ClientInfo clientInfo)
	{
		this.pendingEncryptionModules.Remove(clientInfo);
	}

	// Token: 0x060037D3 RID: 14291 RVA: 0x0016ECCD File Offset: 0x0016CECD
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

	// Token: 0x04002DC1 RID: 11713
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

	// Token: 0x04002DC2 RID: 11714
	[PublicizedFrom(EAccessModifier.Private)]
	public AntiCheatEncryptionAuthServer.KeyExchangeCompleteDelegate OnExchangeCompleted;

	// Token: 0x04002DC3 RID: 11715
	[PublicizedFrom(EAccessModifier.Private)]
	public AntiCheatEncryptionAuthServer.KeyExchangeFailedDelegate OnExchangeFailed;

	// Token: 0x04002DC4 RID: 11716
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<ClientInfo, IEncryptionModule> pendingEncryptionModules;

	// Token: 0x0200075A RID: 1882
	// (Invoke) Token: 0x060037D7 RID: 14295
	public delegate void KeyExchangeCompleteDelegate(ClientInfo clientInfo, IEncryptionModule encryptionModule);

	// Token: 0x0200075B RID: 1883
	// (Invoke) Token: 0x060037DB RID: 14299
	public delegate void KeyExchangeFailedDelegate(ClientInfo clientInfo, GameUtils.KickPlayerData reason);
}
