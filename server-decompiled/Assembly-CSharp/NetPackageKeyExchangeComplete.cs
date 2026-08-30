using System;
using UnityEngine.Scripting;

// Token: 0x0200079F RID: 1951
[Preserve]
public class NetPackageKeyExchangeComplete : NetPackage
{
	// Token: 0x170005FC RID: 1532
	// (get) Token: 0x060039FA RID: 14842 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060039FB RID: 14843 RVA: 0x001797E7 File Offset: 0x001779E7
	public NetPackageKeyExchangeComplete Setup(bool _wasSuccessful)
	{
		this.wasSuccessful = _wasSuccessful;
		return this;
	}

	// Token: 0x060039FC RID: 14844 RVA: 0x001797F1 File Offset: 0x001779F1
	public override void read(PooledBinaryReader _br)
	{
		this.wasSuccessful = _br.ReadBoolean();
	}

	// Token: 0x060039FD RID: 14845 RVA: 0x001797FF File Offset: 0x001779FF
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.wasSuccessful);
	}

	// Token: 0x060039FE RID: 14846 RVA: 0x00179814 File Offset: 0x00177A14
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.AntiCheatEncryptionAuthServer.CompleteKeyExchange(base.Sender, this.wasSuccessful);
	}

	// Token: 0x060039FF RID: 14847 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override int GetLength()
	{
		return 1;
	}

	// Token: 0x04002F53 RID: 12115
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasSuccessful;
}
