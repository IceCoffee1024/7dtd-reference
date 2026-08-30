using System;
using UnityEngine.Scripting;

// Token: 0x0200079D RID: 1949
[Preserve]
public class NetPackageEncryptionPublicKey : NetPackage
{
	// Token: 0x170005FA RID: 1530
	// (get) Token: 0x060039EC RID: 14828 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060039ED RID: 14829 RVA: 0x00179507 File Offset: 0x00177707
	public NetPackageEncryptionPublicKey Setup(string keyParamsXml, byte[] hash, byte[] signedHash)
	{
		this.ExchangePublicKeyParamsXml = keyParamsXml;
		this.Hash = hash;
		this.SignedHash = signedHash;
		return this;
	}

	// Token: 0x060039EE RID: 14830 RVA: 0x00179520 File Offset: 0x00177720
	public override void read(PooledBinaryReader _br)
	{
		this.ExchangePublicKeyParamsXml = _br.ReadString();
		this.Hash = new byte[_br.ReadInt32()];
		_br.Read(this.Hash, 0, this.Hash.Length);
		this.SignedHash = new byte[_br.ReadInt32()];
		_br.Read(this.SignedHash, 0, this.SignedHash.Length);
	}

	// Token: 0x060039EF RID: 14831 RVA: 0x00179588 File Offset: 0x00177788
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.ExchangePublicKeyParamsXml);
		_bw.Write(this.Hash.Length);
		_bw.Write(this.Hash);
		_bw.Write(this.SignedHash.Length);
		_bw.Write(this.SignedHash);
	}

	// Token: 0x060039F0 RID: 14832 RVA: 0x001795DC File Offset: 0x001777DC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.AntiCheatEncryptionAuthServer.SendSharedKey(base.Sender, this.ExchangePublicKeyParamsXml, this.Hash, this.SignedHash);
	}

	// Token: 0x060039F1 RID: 14833 RVA: 0x00179608 File Offset: 0x00177808
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

	// Token: 0x04002F4E RID: 12110
	[PublicizedFrom(EAccessModifier.Private)]
	public string ExchangePublicKeyParamsXml;

	// Token: 0x04002F4F RID: 12111
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] Hash;

	// Token: 0x04002F50 RID: 12112
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] SignedHash;
}
