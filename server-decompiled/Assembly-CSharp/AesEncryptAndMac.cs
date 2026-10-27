using System;
using System.IO;
using System.Security.Cryptography;

// Token: 0x02000771 RID: 1905
public class AesEncryptAndMac : IEncryptionModule
{
	// Token: 0x170005BA RID: 1466
	// (get) Token: 0x06003892 RID: 14482 RVA: 0x001729D3 File Offset: 0x00170BD3
	public byte[] EncryptionKey
	{
		get
		{
			return this.aes.Key;
		}
	}

	// Token: 0x170005BB RID: 1467
	// (get) Token: 0x06003893 RID: 14483 RVA: 0x001729E0 File Offset: 0x00170BE0
	public byte[] IntegrityKey
	{
		get
		{
			return this.hmac.Key;
		}
	}

	// Token: 0x06003894 RID: 14484 RVA: 0x001729F0 File Offset: 0x00170BF0
	public AesEncryptAndMac()
	{
		this.aes = Aes.Create();
		this.aes.GenerateKey();
		this.aes.GenerateIV();
		this.encryptor = this.aes.CreateEncryptor();
		this.decryptor = this.aes.CreateDecryptor();
		this.hmac = new HMACSHA256();
		this.random = RandomNumberGenerator.Create();
	}

	// Token: 0x06003895 RID: 14485 RVA: 0x00172A68 File Offset: 0x00170C68
	public AesEncryptAndMac(byte[] encryptionKey, byte[] integrityKey)
	{
		this.aes = Aes.Create();
		this.aes.Key = encryptionKey;
		this.aes.GenerateIV();
		this.encryptor = this.aes.CreateEncryptor();
		this.decryptor = this.aes.CreateDecryptor();
		this.hmac = new HMACSHA256(integrityKey);
		this.random = RandomNumberGenerator.Create();
	}

	// Token: 0x06003896 RID: 14486 RVA: 0x00172AE4 File Offset: 0x00170CE4
	public unsafe bool EncryptStream(ClientInfo _cInfo, MemoryStream _stream)
	{
		int inputBlockSize = this.encryptor.InputBlockSize;
		Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)inputBlockSize], inputBlockSize);
		PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(false);
		int num = (int)_stream.Length;
		object obj = this.lockObj;
		byte[] hash;
		lock (obj)
		{
			using (CryptoStream cryptoStream = new CryptoStream(pooledExpandableMemoryStream, this.hmac, CryptoStreamMode.Write, true))
			{
				using (CryptoStream cryptoStream2 = new CryptoStream(cryptoStream, this.encryptor, CryptoStreamMode.Write))
				{
					this.random.GetBytes(span);
					cryptoStream2.Write(span);
					StreamUtils.Write(cryptoStream2, num);
					cryptoStream2.Write(_stream.GetBuffer(), 0, num);
				}
			}
			hash = this.hmac.Hash;
		}
		_stream.SetLength(0L);
		_stream.Write(hash);
		_stream.Write(pooledExpandableMemoryStream.GetBuffer(), 0, (int)pooledExpandableMemoryStream.Length);
		_stream.Position = 0L;
		MemoryPools.poolMemoryStream.FreeSync(pooledExpandableMemoryStream);
		return true;
	}

	// Token: 0x06003897 RID: 14487 RVA: 0x00172C1C File Offset: 0x00170E1C
	public unsafe bool DecryptStream(ClientInfo _cInfo, MemoryStream _stream)
	{
		int num = this.hmac.HashSize / 8;
		Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)num], num);
		_stream.Read(span);
		num = this.decryptor.OutputBlockSize;
		Span<byte> span2 = new Span<byte>(stackalloc byte[(UIntPtr)num], num);
		int num2 = 0;
		object obj = this.lockObj;
		byte[] array;
		byte[] hash;
		lock (obj)
		{
			using (CryptoStream cryptoStream = new CryptoStream(_stream, this.hmac, CryptoStreamMode.Read, true))
			{
				using (CryptoStream cryptoStream2 = new CryptoStream(cryptoStream, this.decryptor, CryptoStreamMode.Read))
				{
					cryptoStream2.Read(span2);
					num2 = StreamUtils.ReadInt32(cryptoStream2);
					array = MemoryPools.poolByte.Alloc(num2);
					cryptoStream2.Read(array, 0, num2);
				}
			}
			hash = this.hmac.Hash;
		}
		if (span.Length != hash.Length)
		{
			Log.Error(string.Format("[EncryptionAgreement] decryption failure, MAC bytes not the same length. Calculated {0}, Remote {1}", this.hmac.Hash.Length, span.Length));
			return false;
		}
		for (int i = 0; i < span.Length; i++)
		{
			if (*span[i] != hash[i])
			{
				Log.Error("[EncryptionAgreement] decryption failure, MAC did not match");
				return false;
			}
		}
		_stream.SetLength(0L);
		_stream.Write(array, 0, num2);
		_stream.Position = 0L;
		MemoryPools.poolByte.Free(array);
		return true;
	}

	// Token: 0x04002E53 RID: 11859
	[PublicizedFrom(EAccessModifier.Private)]
	public Aes aes;

	// Token: 0x04002E54 RID: 11860
	[PublicizedFrom(EAccessModifier.Private)]
	public ICryptoTransform encryptor;

	// Token: 0x04002E55 RID: 11861
	[PublicizedFrom(EAccessModifier.Private)]
	public ICryptoTransform decryptor;

	// Token: 0x04002E56 RID: 11862
	[PublicizedFrom(EAccessModifier.Private)]
	public HMAC hmac;

	// Token: 0x04002E57 RID: 11863
	[PublicizedFrom(EAccessModifier.Private)]
	public RandomNumberGenerator random;

	// Token: 0x04002E58 RID: 11864
	[PublicizedFrom(EAccessModifier.Private)]
	public object lockObj = new object();
}
