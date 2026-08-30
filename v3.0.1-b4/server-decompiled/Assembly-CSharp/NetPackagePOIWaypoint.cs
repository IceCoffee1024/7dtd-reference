using System;
using UnityEngine.Scripting;

// Token: 0x02000829 RID: 2089
[Preserve]
public class NetPackagePOIWaypoint : NetPackage
{
	// Token: 0x06003D4C RID: 15692 RVA: 0x00184C3F File Offset: 0x00182E3F
	public NetPackagePOIWaypoint Setup(NetPackagePOIWaypoint.OperationType _operation, int _entityId, int _prefabInstanceId = -1, bool _hiddenOnCompass = true)
	{
		this.operation = _operation;
		this.entityId = _entityId;
		this.prefabInstanceId = _prefabInstanceId;
		this.hiddenOnCompass = _hiddenOnCompass;
		return this;
	}

	// Token: 0x06003D4D RID: 15693 RVA: 0x00184C60 File Offset: 0x00182E60
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

	// Token: 0x06003D4E RID: 15694 RVA: 0x00184CC4 File Offset: 0x00182EC4
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

	// Token: 0x06003D4F RID: 15695 RVA: 0x00184D30 File Offset: 0x00182F30
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

	// Token: 0x17000666 RID: 1638
	// (get) Token: 0x06003D50 RID: 15696 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003D51 RID: 15697 RVA: 0x00080B3A File Offset: 0x0007ED3A
	public override int GetLength()
	{
		return 10;
	}

	// Token: 0x04003168 RID: 12648
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackagePOIWaypoint.OperationType operation;

	// Token: 0x04003169 RID: 12649
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x0400316A RID: 12650
	[PublicizedFrom(EAccessModifier.Private)]
	public int prefabInstanceId;

	// Token: 0x0400316B RID: 12651
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hiddenOnCompass;

	// Token: 0x0200082A RID: 2090
	public enum OperationType : byte
	{
		// Token: 0x0400316D RID: 12653
		TrySet,
		// Token: 0x0400316E RID: 12654
		Remove,
		// Token: 0x0400316F RID: 12655
		ClearAll
	}
}
