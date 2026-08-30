using System;
using UnityEngine.Scripting;

// Token: 0x0200079E RID: 1950
[Preserve]
public class NetPackageEncryptionSharedKey : NetPackage
{
	// Token: 0x170005FB RID: 1531
	// (get) Token: 0x060039F3 RID: 14835 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060039F4 RID: 14836 RVA: 0x001796BC File Offset: 0x001778BC
	public NetPackageEncryptionSharedKey Setup(byte[] encryptionKey, byte[] integrityKey)
	{
		this.EncryptionKey = encryptionKey;
		this.IntegrityKey = integrityKey;
		return this;
	}

	// Token: 0x060039F5 RID: 14837 RVA: 0x001796D0 File Offset: 0x001778D0
	public override void read(PooledBinaryReader _br)
	{
		this.EncryptionKey = new byte[_br.ReadInt32()];
		_br.Read(this.EncryptionKey, 0, this.EncryptionKey.Length);
		this.IntegrityKey = new byte[_br.ReadInt32()];
		_br.Read(this.IntegrityKey, 0, this.IntegrityKey.Length);
	}

	// Token: 0x060039F6 RID: 14838 RVA: 0x0017972B File Offset: 0x0017792B
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.EncryptionKey.Length);
		_bw.Write(this.EncryptionKey);
		_bw.Write(this.IntegrityKey.Length);
		_bw.Write(this.IntegrityKey);
	}

	// Token: 0x060039F7 RID: 14839 RVA: 0x00179768 File Offset: 0x00177968
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.AntiCheatEncryptionAuthClient.CompleteKeyExchange(this.EncryptionKey, this.IntegrityKey);
	}

	// Token: 0x060039F8 RID: 14840 RVA: 0x00179788 File Offset: 0x00177988
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

	// Token: 0x04002F51 RID: 12113
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] EncryptionKey;

	// Token: 0x04002F52 RID: 12114
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] IntegrityKey;
}
