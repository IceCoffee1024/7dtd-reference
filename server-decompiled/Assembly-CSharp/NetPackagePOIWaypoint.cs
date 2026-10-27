using System;
using UnityEngine.Scripting;

// Token: 0x02000840 RID: 2112
[Preserve]
public class NetPackagePOIWaypoint : NetPackage
{
	// Token: 0x06003D8B RID: 15755 RVA: 0x0018789C File Offset: 0x00185A9C
	public NetPackagePOIWaypoint Setup(NetPackagePOIWaypoint.OperationType _operation, int _entityId, int _prefabInstanceId = -1, bool _hiddenOnCompass = true)
	{
		this.operation = _operation;
		this.entityId = _entityId;
		this.prefabInstanceId = _prefabInstanceId;
		this.hiddenOnCompass = _hiddenOnCompass;
		return this;
	}

	// Token: 0x06003D8C RID: 15756 RVA: 0x001878BC File Offset: 0x00185ABC
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

	// Token: 0x06003D8D RID: 15757 RVA: 0x00187920 File Offset: 0x00185B20
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

	// Token: 0x06003D8E RID: 15758 RVA: 0x0018798C File Offset: 0x00185B8C
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

	// Token: 0x17000697 RID: 1687
	// (get) Token: 0x06003D8F RID: 15759 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x040031EE RID: 12782
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackagePOIWaypoint.OperationType operation;

	// Token: 0x040031EF RID: 12783
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x040031F0 RID: 12784
	[PublicizedFrom(EAccessModifier.Private)]
	public int prefabInstanceId;

	// Token: 0x040031F1 RID: 12785
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hiddenOnCompass;

	// Token: 0x02000841 RID: 2113
	public enum OperationType : byte
	{
		// Token: 0x040031F3 RID: 12787
		TrySet,
		// Token: 0x040031F4 RID: 12788
		Remove,
		// Token: 0x040031F5 RID: 12789
		ClearAll
	}
}
