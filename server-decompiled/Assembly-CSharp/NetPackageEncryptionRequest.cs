using System;
using UnityEngine.Scripting;

// Token: 0x0200079C RID: 1948
[Preserve]
public class NetPackageEncryptionRequest : NetPackage
{
	// Token: 0x170005F9 RID: 1529
	// (get) Token: 0x060039E6 RID: 14822 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060039E7 RID: 14823 RVA: 0x001290F5 File Offset: 0x001272F5
	public NetPackageEncryptionRequest Setup()
	{
		return this;
	}

	// Token: 0x060039E8 RID: 14824 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _reader)
	{
	}

	// Token: 0x060039E9 RID: 14825 RVA: 0x001794F6 File Offset: 0x001776F6
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.AntiCheatEncryptionAuthClient.StartKeyExchange();
	}

	// Token: 0x060039EA RID: 14826 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}
}
