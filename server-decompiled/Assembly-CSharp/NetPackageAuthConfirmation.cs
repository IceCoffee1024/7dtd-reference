using System;
using UnityEngine.Scripting;

// Token: 0x020007A2 RID: 1954
[Preserve]
public class NetPackageAuthConfirmation : NetPackage
{
	// Token: 0x170005FE RID: 1534
	// (get) Token: 0x06003A0E RID: 14862 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170005FF RID: 1535
	// (get) Token: 0x06003A0F RID: 14863 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003A10 RID: 14864 RVA: 0x001290F5 File Offset: 0x001272F5
	public NetPackageAuthConfirmation Setup()
	{
		return this;
	}

	// Token: 0x06003A11 RID: 14865 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _reader)
	{
	}

	// Token: 0x06003A12 RID: 14866 RVA: 0x001799B3 File Offset: 0x00177BB3
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
	}

	// Token: 0x06003A13 RID: 14867 RVA: 0x001799BC File Offset: 0x00177BBC
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

	// Token: 0x06003A14 RID: 14868 RVA: 0x001306A9 File Offset: 0x0012E8A9
	public override int GetLength()
	{
		return 9;
	}
}
