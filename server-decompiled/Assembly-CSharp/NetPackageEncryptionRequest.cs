using System;
using UnityEngine.Scripting;

// Token: 0x020007A6 RID: 1958
[Preserve]
public class NetPackageEncryptionRequest : NetPackage
{
	// Token: 0x17000603 RID: 1539
	// (get) Token: 0x06003A3E RID: 14910 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003A3F RID: 14911 RVA: 0x0012A165 File Offset: 0x00128365
	public NetPackageEncryptionRequest Setup()
	{
		return this;
	}

	// Token: 0x06003A40 RID: 14912 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _reader)
	{
	}

	// Token: 0x06003A41 RID: 14913 RVA: 0x0017B776 File Offset: 0x00179976
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.AntiCheatEncryptionAuthClient.StartKeyExchange();
	}

	// Token: 0x06003A42 RID: 14914 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}
}
