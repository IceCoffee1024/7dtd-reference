using System;
using UnityEngine.Scripting;

// Token: 0x02000836 RID: 2102
[Preserve]
public class NetPackagePOIWaypoint : NetPackage
{
	// Token: 0x06003D68 RID: 15720 RVA: 0x00186798 File Offset: 0x00184998
	public NetPackagePOIWaypoint Setup(NetPackagePOIWaypoint.OperationType _operation, int _entityId, int _prefabInstanceId = -1, bool _hiddenOnCompass = true)
	{
		this.operation = _operation;
		this.entityId = _entityId;
		this.prefabInstanceId = _prefabInstanceId;
		this.hiddenOnCompass = _hiddenOnCompass;
		return this;
	}

	// Token: 0x06003D69 RID: 15721 RVA: 0x001867B8 File Offset: 0x001849B8
	public override void read(PooledBinaryReader _br)
	{
		this.operation = (NetPackagePOIWaypoint.OperationType)_br.ReadByte();
		this.entityId = _br.ReadInt32();
		switch (this.operation)
		{
		case NetPackagePOIWaypoint.OperationType.TrySet:
			this.prefabInstanceId = _br.ReadInt32();
			this.hiddenOnCompass = _br.ReadBoolean();
			return;
		case NetPackagePOIWaypoint.OperationType.Remove:
			this.prefabInstanceId = _br.ReadInt32();
			break;
		case NetPackagePOIWaypoint.OperationType.ClearAll:
			break;
		default:
			return;
		}
	}

	// Token: 0x06003D6A RID: 15722 RVA: 0x0018681C File Offset: 0x00184A1C
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.operation);
		_bw.Write(this.entityId);
		switch (this.operation)
		{
		case NetPackagePOIWaypoint.OperationType.TrySet:
			_bw.Write(this.prefabInstanceId);
			_bw.Write(this.hiddenOnCompass);
			return;
		case NetPackagePOIWaypoint.OperationType.Remove:
			_bw.Write(this.prefabInstanceId);
			break;
		case NetPackagePOIWaypoint.OperationType.ClearAll:
			break;
		default:
			return;
		}
	}

	// Token: 0x06003D6B RID: 15723 RVA: 0x00186888 File Offset: 0x00184A88
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		EntityPlayer entityPlayer = _world.GetEntity(this.entityId) as EntityPlayer;
		if (entityPlayer == null)
		{
			Log.Warning(string.Format("NetPackagePOIWaypoint: Could not find player with entityId {0}", this.entityId));
			return;
		}
		switch (this.operation)
		{
		case NetPackagePOIWaypoint.OperationType.TrySet:
			POIWaypoint.TrySet(entityPlayer, this.prefabInstanceId, this.hiddenOnCompass);
			return;
		case NetPackagePOIWaypoint.OperationType.Remove:
			POIWaypoint.Remove(entityPlayer, this.prefabInstanceId);
			return;
		case NetPackagePOIWaypoint.OperationType.ClearAll:
			POIWaypoint.ClearAll(entityPlayer);
			return;
		default:
			return;
		}
	}

	// Token: 0x17000691 RID: 1681
	// (get) Token: 0x06003D6C RID: 15724 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x040031CE RID: 12750
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackagePOIWaypoint.OperationType operation;

	// Token: 0x040031CF RID: 12751
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x040031D0 RID: 12752
	[PublicizedFrom(EAccessModifier.Private)]
	public int prefabInstanceId;

	// Token: 0x040031D1 RID: 12753
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hiddenOnCompass;

	// Token: 0x02000837 RID: 2103
	public enum OperationType : byte
	{
		// Token: 0x040031D3 RID: 12755
		TrySet,
		// Token: 0x040031D4 RID: 12756
		Remove,
		// Token: 0x040031D5 RID: 12757
		ClearAll
	}
}
