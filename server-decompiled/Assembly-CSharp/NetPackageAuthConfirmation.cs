using System;
using UnityEngine.Scripting;

// Token: 0x020007B7 RID: 1975
[Preserve]
public class NetPackageAuthConfirmation : NetPackage
{
	// Token: 0x17000631 RID: 1585
	// (get) Token: 0x06003AB8 RID: 15032 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000632 RID: 1586
	// (get) Token: 0x06003AB9 RID: 15033 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003ABA RID: 15034 RVA: 0x0012B879 File Offset: 0x00129A79
	public NetPackageAuthConfirmation Setup()
	{
		return this;
	}

	// Token: 0x06003ABB RID: 15035 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _reader)
	{
	}

	// Token: 0x06003ABC RID: 15036 RVA: 0x0017D267 File Offset: 0x0017B467
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
	}

	// Token: 0x06003ABD RID: 15037 RVA: 0x0017D270 File Offset: 0x0017B470
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
