using System;
using UnityEngine.Scripting;

// Token: 0x020007AB RID: 1963
[Preserve]
public class NetPackageEncryptionSharedKey : NetPackage
{
	// Token: 0x1700062A RID: 1578
	// (get) Token: 0x06003A8A RID: 14986 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003A8B RID: 14987 RVA: 0x0017C03D File Offset: 0x0017A23D
	public NetPackageEncryptionSharedKey Setup(byte[] encryptionKey, byte[] integrityKey)
	{
		this.EncryptionKey = encryptionKey;
		this.IntegrityKey = integrityKey;
		return this;
	}

	// Token: 0x06003A8C RID: 14988 RVA: 0x0017C050 File Offset: 0x0017A250
	public override void read(PooledBinaryReader _br)
	{
		this.EncryptionKey = new byte[_br.ReadInt32()];
		_br.Read(this.EncryptionKey, 0, this.EncryptionKey.Length);
		this.IntegrityKey = new byte[_br.ReadInt32()];
		_br.Read(this.IntegrityKey, 0, this.IntegrityKey.Length);
	}

	// Token: 0x06003A8D RID: 14989 RVA: 0x0017C0AB File Offset: 0x0017A2AB
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.EncryptionKey.Length);
		_bw.Write(this.EncryptionKey);
		_bw.Write(this.IntegrityKey.Length);
		_bw.Write(this.IntegrityKey);
	}

	// Token: 0x06003A8E RID: 14990 RVA: 0x0017C0E8 File Offset: 0x0017A2E8
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.AntiCheatEncryptionAuthClient.CompleteKeyExchange(this.EncryptionKey, this.IntegrityKey);
	}

	// Token: 0x04002FC4 RID: 12228
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] EncryptionKey;

	// Token: 0x04002FC5 RID: 12229
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] IntegrityKey;
}
