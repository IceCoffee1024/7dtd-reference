using System;
using UnityEngine.Scripting;

// Token: 0x02000835 RID: 2101
[Preserve]
public class NetPackagePOIWaypoint : NetPackage
{
	// Token: 0x06003DB1 RID: 15793 RVA: 0x00186C60 File Offset: 0x00184E60
	public NetPackagePOIWaypoint Setup(NetPackagePOIWaypoint.OperationType _operation, int _entityId, int _prefabInstanceId = -1, bool _hiddenOnCompass = true)
	{
		this.operation = _operation;
		this.entityId = _entityId;
		this.prefabInstanceId = _prefabInstanceId;
		this.hiddenOnCompass = _hiddenOnCompass;
		return this;
	}

	// Token: 0x06003DB2 RID: 15794 RVA: 0x00186C80 File Offset: 0x00184E80
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

	// Token: 0x06003DB3 RID: 15795 RVA: 0x00186CE4 File Offset: 0x00184EE4
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

	// Token: 0x06003DB4 RID: 15796 RVA: 0x00186D50 File Offset: 0x00184F50
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

	// Token: 0x17000672 RID: 1650
	// (get) Token: 0x06003DB5 RID: 15797 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003DB6 RID: 15798 RVA: 0x000811EE File Offset: 0x0007F3EE
	public override int GetLength()
	{
		return 10;
	}

	// Token: 0x040031E7 RID: 12775
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackagePOIWaypoint.OperationType operation;

	// Token: 0x040031E8 RID: 12776
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x040031E9 RID: 12777
	[PublicizedFrom(EAccessModifier.Private)]
	public int prefabInstanceId;

	// Token: 0x040031EA RID: 12778
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hiddenOnCompass;

	// Token: 0x02000836 RID: 2102
	public enum OperationType : byte
	{
		// Token: 0x040031EC RID: 12780
		TrySet,
		// Token: 0x040031ED RID: 12781
		Remove,
		// Token: 0x040031EE RID: 12782
		ClearAll
	}
}
