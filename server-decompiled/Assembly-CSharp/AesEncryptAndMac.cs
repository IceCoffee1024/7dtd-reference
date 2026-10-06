using System;
using System.IO;
using System.Security.Cryptography;

// Token: 0x02000769 RID: 1897
public class AesEncryptAndMac : IEncryptionModule
{
	// Token: 0x170005B6 RID: 1462
	// (get) Token: 0x0600387B RID: 14459 RVA: 0x00171A3B File Offset: 0x0016FC3B
	public byte[] EncryptionKey
	{
		get
		{
			return this.aes.Key;
		}
	}

	// Token: 0x170005B7 RID: 1463
	// (get) Token: 0x0600387C RID: 14460 RVA: 0x00171A48 File Offset: 0x0016FC48
	public byte[] IntegrityKey
	{
		get
		{
			return this.hmac.Key;
		}
	}

	// Token: 0x0600387D RID: 14461 RVA: 0x00171A58 File Offset: 0x0016FC58
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

	// Token: 0x0600387E RID: 14462 RVA: 0x00171AD0 File Offset: 0x0016FCD0
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

	// Token: 0x0600387F RID: 14463 RVA: 0x00171B4C File Offset: 0x0016FD4C
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

	// Token: 0x06003880 RID: 14464 RVA: 0x00171C84 File Offset: 0x0016FE84
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

	// Token: 0x04002E37 RID: 11831
	[PublicizedFrom(EAccessModifier.Private)]
	public Aes aes;

	// Token: 0x04002E38 RID: 11832
	[PublicizedFrom(EAccessModifier.Private)]
	public ICryptoTransform encryptor;

	// Token: 0x04002E39 RID: 11833
	[PublicizedFrom(EAccessModifier.Private)]
	public ICryptoTransform decryptor;

	// Token: 0x04002E3A RID: 11834
	[PublicizedFrom(EAccessModifier.Private)]
	public HMAC hmac;

	// Token: 0x04002E3B RID: 11835
	[PublicizedFrom(EAccessModifier.Private)]
	public RandomNumberGenerator random;

	// Token: 0x04002E3C RID: 11836
	[PublicizedFrom(EAccessModifier.Private)]
	public object lockObj = new object();
}
