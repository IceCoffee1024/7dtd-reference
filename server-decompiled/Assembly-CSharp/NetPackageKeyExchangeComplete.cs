using System;
using UnityEngine.Scripting;

// Token: 0x020007A9 RID: 1961
[Preserve]
public class NetPackageKeyExchangeComplete : NetPackage
{
	// Token: 0x17000606 RID: 1542
	// (get) Token: 0x06003A52 RID: 14930 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003A53 RID: 14931 RVA: 0x0017BA67 File Offset: 0x00179C67
	public NetPackageKeyExchangeComplete Setup(bool _wasSuccessful)
	{
		this.wasSuccessful = _wasSuccessful;
		return this;
	}

	// Token: 0x06003A54 RID: 14932 RVA: 0x0017BA71 File Offset: 0x00179C71
	public override void read(PooledBinaryReader _br)
	{
		this.wasSuccessful = _br.ReadBoolean();
	}

	// Token: 0x06003A55 RID: 14933 RVA: 0x0017BA7F File Offset: 0x00179C7F
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.wasSuccessful);
	}

	// Token: 0x06003A56 RID: 14934 RVA: 0x0017BA94 File Offset: 0x00179C94
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.AntiCheatEncryptionAuthServer.CompleteKeyExchange(base.Sender, this.wasSuccessful);
	}

	// Token: 0x06003A57 RID: 14935 RVA: 0x00020021 File Offset: 0x0001E221
	public override int GetLength()
	{
		return 1;
	}

	// Token: 0x04002FCC RID: 12236
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasSuccessful;
}
