using System;
using UnityEngine.Scripting;

// Token: 0x020007B1 RID: 1969
[Preserve]
public class NetPackageEncryptionRequest : NetPackage
{
	// Token: 0x1700062C RID: 1580
	// (get) Token: 0x06003A96 RID: 14998 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003A97 RID: 14999 RVA: 0x0012B879 File Offset: 0x00129A79
	public NetPackageEncryptionRequest Setup()
	{
		return this;
	}

	// Token: 0x06003A98 RID: 15000 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _reader)
	{
	}

	// Token: 0x06003A99 RID: 15001 RVA: 0x0017CEC6 File Offset: 0x0017B0C6
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.AntiCheatEncryptionAuthClient.StartKeyExchange();
	}
}
