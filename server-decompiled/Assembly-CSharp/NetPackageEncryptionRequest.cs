using System;
using UnityEngine.Scripting;

// Token: 0x020007A9 RID: 1961
[Preserve]
public class NetPackageEncryptionRequest : NetPackage
{
	// Token: 0x17000628 RID: 1576
	// (get) Token: 0x06003A7F RID: 14975 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003A80 RID: 14976 RVA: 0x0012AC85 File Offset: 0x00128E85
	public NetPackageEncryptionRequest Setup()
	{
		return this;
	}

	// Token: 0x06003A81 RID: 14977 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _reader)
	{
	}

	// Token: 0x06003A82 RID: 14978 RVA: 0x0017BF2E File Offset: 0x0017A12E
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.AntiCheatEncryptionAuthClient.StartKeyExchange();
	}
}
