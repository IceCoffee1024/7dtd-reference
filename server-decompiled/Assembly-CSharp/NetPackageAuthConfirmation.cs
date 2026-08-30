using System;
using UnityEngine.Scripting;

// Token: 0x020007AC RID: 1964
[Preserve]
public class NetPackageAuthConfirmation : NetPackage
{
	// Token: 0x17000608 RID: 1544
	// (get) Token: 0x06003A66 RID: 14950 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000609 RID: 1545
	// (get) Token: 0x06003A67 RID: 14951 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003A68 RID: 14952 RVA: 0x0012A165 File Offset: 0x00128365
	public NetPackageAuthConfirmation Setup()
	{
		return this;
	}

	// Token: 0x06003A69 RID: 14953 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _reader)
	{
	}

	// Token: 0x06003A6A RID: 14954 RVA: 0x0017BC33 File Offset: 0x00179E33
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
	}

	// Token: 0x06003A6B RID: 14955 RVA: 0x0017BC3C File Offset: 0x00179E3C
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			AuthFinalizer.Instance.ReplyReceived(base.Sender);
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageAuthConfirmation>().Setup(), false);
		}
	}

	// Token: 0x06003A6C RID: 14956 RVA: 0x00131719 File Offset: 0x0012F919
	public override int GetLength()
	{
		return 9;
	}
}
