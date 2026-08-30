using System;
using UnityEngine.Scripting;

// Token: 0x020007A7 RID: 1959
[Preserve]
public class NetPackageEncryptionPublicKey : NetPackage
{
	// Token: 0x17000604 RID: 1540
	// (get) Token: 0x06003A44 RID: 14916 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003A45 RID: 14917 RVA: 0x0017B787 File Offset: 0x00179987
	public NetPackageEncryptionPublicKey Setup(string keyParamsXml, byte[] hash, byte[] signedHash)
	{
		this.ExchangePublicKeyParamsXml = keyParamsXml;
		this.Hash = hash;
		this.SignedHash = signedHash;
		return this;
	}

	// Token: 0x06003A46 RID: 14918 RVA: 0x0017B7A0 File Offset: 0x001799A0
	public override void read(PooledBinaryReader _br)
	{
		this.ExchangePublicKeyParamsXml = _br.ReadString();
		this.Hash = new byte[_br.ReadInt32()];
		_br.Read(this.Hash, 0, this.Hash.Length);
		this.SignedHash = new byte[_br.ReadInt32()];
		_br.Read(this.SignedHash, 0, this.SignedHash.Length);
	}

	// Token: 0x06003A47 RID: 14919 RVA: 0x0017B808 File Offset: 0x00179A08
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.ExchangePublicKeyParamsXml);
		_bw.Write(this.Hash.Length);
		_bw.Write(this.Hash);
		_bw.Write(this.SignedHash.Length);
		_bw.Write(this.SignedHash);
	}

	// Token: 0x06003A48 RID: 14920 RVA: 0x0017B85C File Offset: 0x00179A5C
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.AntiCheatEncryptionAuthServer.SendSharedKey(base.Sender, this.ExchangePublicKeyParamsXml, this.Hash, this.SignedHash);
	}

	// Token: 0x06003A49 RID: 14921 RVA: 0x0017B888 File Offset: 0x00179A88
	public override int GetLength()
	{
		string exchangePublicKeyParamsXml = this.ExchangePublicKeyParamsXml;
		if (exchangePublicKeyParamsXml != null)
		{
			return exchangePublicKeyParamsXml.Length;
		}
		byte[] hash = this.Hash;
		int? num = ((hash != null) ? new int?(hash.Length) : null);
		int? num2 = ((num != null) ? new int?(num.GetValueOrDefault()) : null);
		if (num2 == null)
		{
			byte[] signedHash = this.SignedHash;
			num = ((signedHash != null) ? new int?(signedHash.Length) : null);
			return ((num != null) ? new int?(num.GetValueOrDefault()) : null).GetValueOrDefault();
		}
		return num2.GetValueOrDefault();
	}

	// Token: 0x04002FC7 RID: 12231
	[PublicizedFrom(EAccessModifier.Private)]
	public string ExchangePublicKeyParamsXml;

	// Token: 0x04002FC8 RID: 12232
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] Hash;

	// Token: 0x04002FC9 RID: 12233
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] SignedHash;
}
