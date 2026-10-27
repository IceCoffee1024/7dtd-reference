using System;
using UnityEngine.Scripting;

// Token: 0x020007B3 RID: 1971
[Preserve]
public class NetPackageEncryptionSharedKey : NetPackage
{
	// Token: 0x1700062E RID: 1582
	// (get) Token: 0x06003AA1 RID: 15009 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003AA2 RID: 15010 RVA: 0x0017CFD5 File Offset: 0x0017B1D5
	public NetPackageEncryptionSharedKey Setup(byte[] encryptionKey, byte[] integrityKey)
	{
		this.EncryptionKey = encryptionKey;
		this.IntegrityKey = integrityKey;
		return this;
	}

	// Token: 0x06003AA3 RID: 15011 RVA: 0x0017CFE8 File Offset: 0x0017B1E8
	public override void read(PooledBinaryReader _br)
	{
		this.EncryptionKey = new byte[_br.ReadInt32()];
		_br.Read(this.EncryptionKey, 0, this.EncryptionKey.Length);
		this.IntegrityKey = new byte[_br.ReadInt32()];
		_br.Read(this.IntegrityKey, 0, this.IntegrityKey.Length);
	}

	// Token: 0x06003AA4 RID: 15012 RVA: 0x0017D043 File Offset: 0x0017B243
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.EncryptionKey.Length);
		_bw.Write(this.EncryptionKey);
		_bw.Write(this.IntegrityKey.Length);
		_bw.Write(this.IntegrityKey);
	}

	// Token: 0x06003AA5 RID: 15013 RVA: 0x0017D080 File Offset: 0x0017B280
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.AntiCheatEncryptionAuthClient.CompleteKeyExchange(this.EncryptionKey, this.IntegrityKey);
	}

	// Token: 0x04002FE0 RID: 12256
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] EncryptionKey;

	// Token: 0x04002FE1 RID: 12257
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] IntegrityKey;
}
