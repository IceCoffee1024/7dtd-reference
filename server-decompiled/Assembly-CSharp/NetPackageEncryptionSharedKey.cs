using System;
using UnityEngine.Scripting;

// Token: 0x020007A8 RID: 1960
[Preserve]
public class NetPackageEncryptionSharedKey : NetPackage
{
	// Token: 0x17000605 RID: 1541
	// (get) Token: 0x06003A4B RID: 14923 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003A4C RID: 14924 RVA: 0x0017B93C File Offset: 0x00179B3C
	public NetPackageEncryptionSharedKey Setup(byte[] encryptionKey, byte[] integrityKey)
	{
		this.EncryptionKey = encryptionKey;
		this.IntegrityKey = integrityKey;
		return this;
	}

	// Token: 0x06003A4D RID: 14925 RVA: 0x0017B950 File Offset: 0x00179B50
	public override void read(PooledBinaryReader _br)
	{
		this.EncryptionKey = new byte[_br.ReadInt32()];
		_br.Read(this.EncryptionKey, 0, this.EncryptionKey.Length);
		this.IntegrityKey = new byte[_br.ReadInt32()];
		_br.Read(this.IntegrityKey, 0, this.IntegrityKey.Length);
	}

	// Token: 0x06003A4E RID: 14926 RVA: 0x0017B9AB File Offset: 0x00179BAB
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.EncryptionKey.Length);
		_bw.Write(this.EncryptionKey);
		_bw.Write(this.IntegrityKey.Length);
		_bw.Write(this.IntegrityKey);
	}

	// Token: 0x06003A4F RID: 14927 RVA: 0x0017B9E8 File Offset: 0x00179BE8
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.AntiCheatEncryptionAuthClient.CompleteKeyExchange(this.EncryptionKey, this.IntegrityKey);
	}

	// Token: 0x06003A50 RID: 14928 RVA: 0x0017BA08 File Offset: 0x00179C08
	public override int GetLength()
	{
		byte[] encryptionKey = this.EncryptionKey;
		if (encryptionKey == null)
		{
			byte[] integrityKey = this.IntegrityKey;
			int? num = ((integrityKey != null) ? new int?(integrityKey.Length) : null);
			return ((num != null) ? new int?(num.GetValueOrDefault()) : null).GetValueOrDefault();
		}
		return encryptionKey.Length;
	}

	// Token: 0x04002FCA RID: 12234
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] EncryptionKey;

	// Token: 0x04002FCB RID: 12235
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] IntegrityKey;
}
