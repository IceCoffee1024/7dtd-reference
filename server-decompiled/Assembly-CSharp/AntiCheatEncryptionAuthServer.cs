using System;
using System.Collections.Generic;
using System.Security.Cryptography;

// Token: 0x02000765 RID: 1893
public class AntiCheatEncryptionAuthServer
{
	// Token: 0x06003866 RID: 14438 RVA: 0x00171547 File Offset: 0x0016F747
	public void Start(AntiCheatEncryptionAuthServer.KeyExchangeCompleteDelegate completeDelegate, AntiCheatEncryptionAuthServer.KeyExchangeFailedDelegate failedDelegate)
	{
		this.OnExchangeCompleted = completeDelegate;
		this.OnExchangeFailed = failedDelegate;
		this.pendingEncryptionModules = new Dictionary<ClientInfo, IEncryptionModule>();
		ConnectionManager.OnClientDisconnected += this.OnClientDisconnected;
	}

	// Token: 0x06003867 RID: 14439 RVA: 0x00171574 File Offset: 0x0016F774
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

	// Token: 0x06003868 RID: 14440 RVA: 0x001715D8 File Offset: 0x0016F7D8
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

	// Token: 0x06003869 RID: 14441 RVA: 0x00171758 File Offset: 0x0016F958
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

	// Token: 0x0600386A RID: 14442 RVA: 0x00171835 File Offset: 0x0016FA35
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnClientDisconnected(ClientInfo clientInfo)
	{
		this.CancelKeyExchange(clientInfo);
	}

	// Token: 0x0600386B RID: 14443 RVA: 0x0017183E File Offset: 0x0016FA3E
	public void CancelKeyExchange(ClientInfo clientInfo)
	{
		this.pendingEncryptionModules.Remove(clientInfo);
	}

	// Token: 0x0600386C RID: 14444 RVA: 0x0017184D File Offset: 0x0016FA4D
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

	// Token: 0x04002E32 RID: 11826
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

	// Token: 0x04002E33 RID: 11827
	[PublicizedFrom(EAccessModifier.Private)]
	public AntiCheatEncryptionAuthServer.KeyExchangeCompleteDelegate OnExchangeCompleted;

	// Token: 0x04002E34 RID: 11828
	[PublicizedFrom(EAccessModifier.Private)]
	public AntiCheatEncryptionAuthServer.KeyExchangeFailedDelegate OnExchangeFailed;

	// Token: 0x04002E35 RID: 11829
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<ClientInfo, IEncryptionModule> pendingEncryptionModules;

	// Token: 0x02000766 RID: 1894
	// (Invoke) Token: 0x06003870 RID: 14448
	public delegate void KeyExchangeCompleteDelegate(ClientInfo clientInfo, IEncryptionModule encryptionModule);

	// Token: 0x02000767 RID: 1895
	// (Invoke) Token: 0x06003874 RID: 14452
	public delegate void KeyExchangeFailedDelegate(ClientInfo clientInfo, GameUtils.KickPlayerData reason);
}
