using System;
using System.Security.Cryptography;
using System.Text;

// Token: 0x02000770 RID: 1904
public class AntiCheatEncryptionAuthClient
{
	// Token: 0x0600388E RID: 14478 RVA: 0x000882A0 File Offset: 0x000864A0
	[PublicizedFrom(EAccessModifier.Private)]
	public RSA GetSigningKey()
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600388F RID: 14479 RVA: 0x00172844 File Offset: 0x00170A44
	public void StartKeyExchange()
	{
		Log.Out("[EncryptionAgreement] checking signing key");
		RSA signingKey;
		try
		{
			signingKey = this.GetSigningKey();
		}
		catch (Exception ex)
		{
			Log.Exception(ex);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageKeyExchangeComplete>().Setup(false), false);
			return;
		}
		Log.Out("[EncryptionAgreement] creating key exchange params");
		this.keyExchangeSessionPair = RSA.Create(2048);
		string text = this.keyExchangeSessionPair.ToXmlString(false);
		Log.Out("[EncryptionAgreement] signing params");
		using (SHA512 sha = SHA512.Create())
		{
			byte[] array = sha.ComputeHash(Encoding.UTF8.GetBytes(text));
			RSAPKCS1SignatureFormatter rsapkcs1SignatureFormatter = new RSAPKCS1SignatureFormatter(signingKey);
			rsapkcs1SignatureFormatter.SetHashAlgorithm("SHA512");
			byte[] array2 = rsapkcs1SignatureFormatter.CreateSignature(array);
			Log.Out("[EncryptionAgreement] sending params to server");
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageEncryptionPublicKey>().Setup(text, array, array2), false);
			signingKey.Dispose();
		}
	}

	// Token: 0x06003890 RID: 14480 RVA: 0x00172938 File Offset: 0x00170B38
	public void CompleteKeyExchange(byte[] protectedEncryptionKey, byte[] protectedIntegrityKey)
	{
		Log.Out("[EncryptionAgreement] received shared keys");
		byte[] array = this.keyExchangeSessionPair.Decrypt(protectedEncryptionKey, RSAEncryptionPadding.Pkcs1);
		byte[] array2 = this.keyExchangeSessionPair.Decrypt(protectedIntegrityKey, RSAEncryptionPadding.Pkcs1);
		AesEncryptAndMac aesEncryptAndMac = new AesEncryptAndMac(array, array2);
		INetConnection[] connectionToServer = SingletonMonoBehaviour<ConnectionManager>.Instance.GetConnectionToServer();
		for (int i = 0; i < connectionToServer.Length; i++)
		{
			connectionToServer[i].SetEncryptionModule(aesEncryptAndMac);
		}
		Log.Out("[EncryptionAgreement] sending reply");
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageKeyExchangeComplete>().Setup(true), false);
		RSA rsa = this.keyExchangeSessionPair;
		if (rsa != null)
		{
			rsa.Dispose();
		}
		this.keyExchangeSessionPair = null;
	}

	// Token: 0x04002E52 RID: 11858
	[PublicizedFrom(EAccessModifier.Private)]
	public RSA keyExchangeSessionPair;
}
