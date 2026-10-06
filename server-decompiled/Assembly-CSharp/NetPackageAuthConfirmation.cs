using System;
using UnityEngine.Scripting;

// Token: 0x020007AF RID: 1967
[Preserve]
public class NetPackageAuthConfirmation : NetPackage
{
	// Token: 0x1700062D RID: 1581
	// (get) Token: 0x06003AA1 RID: 15009 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700062E RID: 1582
	// (get) Token: 0x06003AA2 RID: 15010 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003AA3 RID: 15011 RVA: 0x0012AC85 File Offset: 0x00128E85
	public NetPackageAuthConfirmation Setup()
	{
		return this;
	}

	// Token: 0x06003AA4 RID: 15012 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _reader)
	{
	}

	// Token: 0x06003AA5 RID: 15013 RVA: 0x0017C2CF File Offset: 0x0017A4CF
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
	}

	// Token: 0x06003AA6 RID: 15014 RVA: 0x0017C2D8 File Offset: 0x0017A4D8
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
}
