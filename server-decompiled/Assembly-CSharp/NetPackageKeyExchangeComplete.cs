using System;
using UnityEngine.Scripting;

// Token: 0x020007B4 RID: 1972
[Preserve]
public class NetPackageKeyExchangeComplete : NetPackage
{
	// Token: 0x1700062F RID: 1583
	// (get) Token: 0x06003AA7 RID: 15015 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003AA8 RID: 15016 RVA: 0x0017D09D File Offset: 0x0017B29D
	public NetPackageKeyExchangeComplete Setup(bool _wasSuccessful)
	{
		this.wasSuccessful = _wasSuccessful;
		return this;
	}

	// Token: 0x06003AA9 RID: 15017 RVA: 0x0017D0A7 File Offset: 0x0017B2A7
	public override void read(PooledBinaryReader _br)
	{
		this.wasSuccessful = _br.ReadBoolean();
	}

	// Token: 0x06003AAA RID: 15018 RVA: 0x0017D0B5 File Offset: 0x0017B2B5
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.wasSuccessful);
	}

	// Token: 0x06003AAB RID: 15019 RVA: 0x0017D0CA File Offset: 0x0017B2CA
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.AntiCheatEncryptionAuthServer.CompleteKeyExchange(base.Sender, this.wasSuccessful);
	}

	// Token: 0x04002FE2 RID: 12258
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasSuccessful;
}
