using System;
using UnityEngine.Scripting;

// Token: 0x020007AA RID: 1962
[Preserve]
public class NetPackageEncryptionPublicKey : NetPackage
{
	// Token: 0x17000629 RID: 1577
	// (get) Token: 0x06003A84 RID: 14980 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003A85 RID: 14981 RVA: 0x0017BF3F File Offset: 0x0017A13F
	public NetPackageEncryptionPublicKey Setup(string keyParamsXml, byte[] hash, byte[] signedHash)
	{
		this.ExchangePublicKeyParamsXml = keyParamsXml;
		this.Hash = hash;
		this.SignedHash = signedHash;
		return this;
	}

	// Token: 0x06003A86 RID: 14982 RVA: 0x0017BF58 File Offset: 0x0017A158
	public override void read(PooledBinaryReader _br)
	{
		this.ExchangePublicKeyParamsXml = _br.ReadString();
		this.Hash = new byte[_br.ReadInt32()];
		_br.Read(this.Hash, 0, this.Hash.Length);
		this.SignedHash = new byte[_br.ReadInt32()];
		_br.Read(this.SignedHash, 0, this.SignedHash.Length);
	}

	// Token: 0x06003A87 RID: 14983 RVA: 0x0017BFC0 File Offset: 0x0017A1C0
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.ExchangePublicKeyParamsXml);
		_bw.Write(this.Hash.Length);
		_bw.Write(this.Hash);
		_bw.Write(this.SignedHash.Length);
		_bw.Write(this.SignedHash);
	}

	// Token: 0x06003A88 RID: 14984 RVA: 0x0017C014 File Offset: 0x0017A214
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.AntiCheatEncryptionAuthServer.SendSharedKey(base.Sender, this.ExchangePublicKeyParamsXml, this.Hash, this.SignedHash);
	}

	// Token: 0x04002FC1 RID: 12225
	[PublicizedFrom(EAccessModifier.Private)]
	public string ExchangePublicKeyParamsXml;

	// Token: 0x04002FC2 RID: 12226
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] Hash;

	// Token: 0x04002FC3 RID: 12227
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] SignedHash;
}
