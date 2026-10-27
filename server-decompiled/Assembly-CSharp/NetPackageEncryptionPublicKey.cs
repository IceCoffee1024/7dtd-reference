using System;
using UnityEngine.Scripting;

// Token: 0x020007B2 RID: 1970
[Preserve]
public class NetPackageEncryptionPublicKey : NetPackage
{
	// Token: 0x1700062D RID: 1581
	// (get) Token: 0x06003A9B RID: 15003 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003A9C RID: 15004 RVA: 0x0017CED7 File Offset: 0x0017B0D7
	public NetPackageEncryptionPublicKey Setup(string keyParamsXml, byte[] hash, byte[] signedHash)
	{
		this.ExchangePublicKeyParamsXml = keyParamsXml;
		this.Hash = hash;
		this.SignedHash = signedHash;
		return this;
	}

	// Token: 0x06003A9D RID: 15005 RVA: 0x0017CEF0 File Offset: 0x0017B0F0
	public override void read(PooledBinaryReader _br)
	{
		this.ExchangePublicKeyParamsXml = _br.ReadString();
		this.Hash = new byte[_br.ReadInt32()];
		_br.Read(this.Hash, 0, this.Hash.Length);
		this.SignedHash = new byte[_br.ReadInt32()];
		_br.Read(this.SignedHash, 0, this.SignedHash.Length);
	}

	// Token: 0x06003A9E RID: 15006 RVA: 0x0017CF58 File Offset: 0x0017B158
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.ExchangePublicKeyParamsXml);
		_bw.Write(this.Hash.Length);
		_bw.Write(this.Hash);
		_bw.Write(this.SignedHash.Length);
		_bw.Write(this.SignedHash);
	}

	// Token: 0x06003A9F RID: 15007 RVA: 0x0017CFAC File Offset: 0x0017B1AC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.AntiCheatEncryptionAuthServer.SendSharedKey(base.Sender, this.ExchangePublicKeyParamsXml, this.Hash, this.SignedHash);
	}

	// Token: 0x04002FDD RID: 12253
	[PublicizedFrom(EAccessModifier.Private)]
	public string ExchangePublicKeyParamsXml;

	// Token: 0x04002FDE RID: 12254
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] Hash;

	// Token: 0x04002FDF RID: 12255
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] SignedHash;
}
