using System;
using UnityEngine.Scripting;

// Token: 0x020007C8 RID: 1992
[Preserve]
public class NetPackageCloseAllWindows : NetPackage
{
	// Token: 0x17000642 RID: 1602
	// (get) Token: 0x06003B14 RID: 15124 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B15 RID: 15125 RVA: 0x0017E3E1 File Offset: 0x0017C5E1
	public NetPackageCloseAllWindows Setup(int entityToClose)
	{
		this._playerIdToClose = entityToClose;
		return this;
	}

	// Token: 0x06003B16 RID: 15126 RVA: 0x0017E3EB File Offset: 0x0017C5EB
	public override void read(PooledBinaryReader _reader)
	{
		this._playerIdToClose = _reader.ReadInt32();
	}

	// Token: 0x06003B17 RID: 15127 RVA: 0x0017E3F9 File Offset: 0x0017C5F9
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this._playerIdToClose);
	}

	// Token: 0x06003B18 RID: 15128 RVA: 0x0017E410 File Offset: 0x0017C610
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			EntityPlayerLocal localPlayerFromID = GameManager.Instance.World.GetLocalPlayerFromID(this._playerIdToClose);
			if (localPlayerFromID != null)
			{
				localPlayerFromID.PlayerUI.windowManager.CloseAllOpenModalWindows(null, false);
			}
		}
	}

	// Token: 0x0400301D RID: 12317
	[PublicizedFrom(EAccessModifier.Private)]
	public int _playerIdToClose = -1;
}
