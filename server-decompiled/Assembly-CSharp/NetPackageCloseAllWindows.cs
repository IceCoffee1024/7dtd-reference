using System;
using UnityEngine.Scripting;

// Token: 0x020007C0 RID: 1984
[Preserve]
public class NetPackageCloseAllWindows : NetPackage
{
	// Token: 0x1700063E RID: 1598
	// (get) Token: 0x06003AFD RID: 15101 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003AFE RID: 15102 RVA: 0x0017D449 File Offset: 0x0017B649
	public NetPackageCloseAllWindows Setup(int entityToClose)
	{
		this._playerIdToClose = entityToClose;
		return this;
	}

	// Token: 0x06003AFF RID: 15103 RVA: 0x0017D453 File Offset: 0x0017B653
	public override void read(PooledBinaryReader _reader)
	{
		this._playerIdToClose = _reader.ReadInt32();
	}

	// Token: 0x06003B00 RID: 15104 RVA: 0x0017D461 File Offset: 0x0017B661
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this._playerIdToClose);
	}

	// Token: 0x06003B01 RID: 15105 RVA: 0x0017D478 File Offset: 0x0017B678
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

	// Token: 0x04003001 RID: 12289
	[PublicizedFrom(EAccessModifier.Private)]
	public int _playerIdToClose = -1;
}
