using System;
using UnityEngine.Scripting;

// Token: 0x020007AC RID: 1964
[Preserve]
public class NetPackageKeyExchangeComplete : NetPackage
{
	// Token: 0x1700062B RID: 1579
	// (get) Token: 0x06003A90 RID: 14992 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003A91 RID: 14993 RVA: 0x0017C105 File Offset: 0x0017A305
	public NetPackageKeyExchangeComplete Setup(bool _wasSuccessful)
	{
		this.wasSuccessful = _wasSuccessful;
		return this;
	}

	// Token: 0x06003A92 RID: 14994 RVA: 0x0017C10F File Offset: 0x0017A30F
	public override void read(PooledBinaryReader _br)
	{
		this.wasSuccessful = _br.ReadBoolean();
	}

	// Token: 0x06003A93 RID: 14995 RVA: 0x0017C11D File Offset: 0x0017A31D
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.wasSuccessful);
	}

	// Token: 0x06003A94 RID: 14996 RVA: 0x0017C132 File Offset: 0x0017A332
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.AntiCheatEncryptionAuthServer.CompleteKeyExchange(base.Sender, this.wasSuccessful);
	}

	// Token: 0x04002FC6 RID: 12230
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasSuccessful;
}
