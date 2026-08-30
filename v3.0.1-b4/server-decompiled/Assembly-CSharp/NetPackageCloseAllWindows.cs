using System;
using UnityEngine.Scripting;

// Token: 0x020007B3 RID: 1971
[Preserve]
public class NetPackageCloseAllWindows : NetPackage
{
	// Token: 0x1700060F RID: 1551
	// (get) Token: 0x06003A78 RID: 14968 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003A79 RID: 14969 RVA: 0x0017AB8D File Offset: 0x00178D8D
	public NetPackageCloseAllWindows Setup(int entityToClose)
	{
		this._playerIdToClose = entityToClose;
		return this;
	}

	// Token: 0x06003A7A RID: 14970 RVA: 0x0017AB97 File Offset: 0x00178D97
	public override void read(PooledBinaryReader _reader)
	{
		this._playerIdToClose = _reader.ReadInt32();
	}

	// Token: 0x06003A7B RID: 14971 RVA: 0x0017ABA5 File Offset: 0x00178DA5
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this._playerIdToClose);
	}

	// Token: 0x06003A7C RID: 14972 RVA: 0x0017ABBC File Offset: 0x00178DBC
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

	// Token: 0x06003A7D RID: 14973 RVA: 0x0007FE9C File Offset: 0x0007E09C
	public override int GetLength()
	{
		return 4;
	}

	// Token: 0x04002F8E RID: 12174
	[PublicizedFrom(EAccessModifier.Private)]
	public int _playerIdToClose = -1;
}
