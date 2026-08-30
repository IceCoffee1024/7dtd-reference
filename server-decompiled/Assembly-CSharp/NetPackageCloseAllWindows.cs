using System;
using UnityEngine.Scripting;

// Token: 0x020007BD RID: 1981
[Preserve]
public class NetPackageCloseAllWindows : NetPackage
{
	// Token: 0x17000619 RID: 1561
	// (get) Token: 0x06003AD0 RID: 15056 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003AD1 RID: 15057 RVA: 0x0017CE0D File Offset: 0x0017B00D
	public NetPackageCloseAllWindows Setup(int entityToClose)
	{
		this._playerIdToClose = entityToClose;
		return this;
	}

	// Token: 0x06003AD2 RID: 15058 RVA: 0x0017CE17 File Offset: 0x0017B017
	public override void read(PooledBinaryReader _reader)
	{
		this._playerIdToClose = _reader.ReadInt32();
	}

	// Token: 0x06003AD3 RID: 15059 RVA: 0x0017CE25 File Offset: 0x0017B025
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this._playerIdToClose);
	}

	// Token: 0x06003AD4 RID: 15060 RVA: 0x0017CE3C File Offset: 0x0017B03C
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

	// Token: 0x06003AD5 RID: 15061 RVA: 0x00080550 File Offset: 0x0007E750
	public override int GetLength()
	{
		return 4;
	}

	// Token: 0x04003007 RID: 12295
	[PublicizedFrom(EAccessModifier.Private)]
	public int _playerIdToClose = -1;
}
