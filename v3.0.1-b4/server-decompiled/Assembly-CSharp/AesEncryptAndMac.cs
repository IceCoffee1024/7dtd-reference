using System;
using System.IO;
using System.Security.Cryptography;

// Token: 0x0200075D RID: 1885
public class AesEncryptAndMac : IEncryptionModule
{
	// Token: 0x17000587 RID: 1415
	// (get) Token: 0x060037E2 RID: 14306 RVA: 0x0016EEBB File Offset: 0x0016D0BB
	public byte[] EncryptionKey
	{
		get
		{
			return this.aes.Key;
		}
	}

	// Token: 0x17000588 RID: 1416
	// (get) Token: 0x060037E3 RID: 14307 RVA: 0x0016EEC8 File Offset: 0x0016D0C8
	public byte[] IntegrityKey
	{
		get
		{
			return this.hmac.Key;
		}
	}

	// Token: 0x060037E4 RID: 14308 RVA: 0x0016EED8 File Offset: 0x0016D0D8
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

	// Token: 0x060037E5 RID: 14309 RVA: 0x0016EF50 File Offset: 0x0016D150
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

	// Token: 0x060037E6 RID: 14310 RVA: 0x0016EFCC File Offset: 0x0016D1CC
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

	// Token: 0x060037E7 RID: 14311 RVA: 0x0016F104 File Offset: 0x0016D304
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

	// Token: 0x04002DC6 RID: 11718
	[PublicizedFrom(EAccessModifier.Private)]
	public Aes aes;

	// Token: 0x04002DC7 RID: 11719
	[PublicizedFrom(EAccessModifier.Private)]
	public ICryptoTransform encryptor;

	// Token: 0x04002DC8 RID: 11720
	[PublicizedFrom(EAccessModifier.Private)]
	public ICryptoTransform decryptor;

	// Token: 0x04002DC9 RID: 11721
	[PublicizedFrom(EAccessModifier.Private)]
	public HMAC hmac;

	// Token: 0x04002DCA RID: 11722
	[PublicizedFrom(EAccessModifier.Private)]
	public RandomNumberGenerator random;

	// Token: 0x04002DCB RID: 11723
	[PublicizedFrom(EAccessModifier.Private)]
	public object lockObj = new object();
}
