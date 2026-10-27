using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007EB RID: 2027
[Preserve]
public class NetPackageEntityMapMarkerRemove : NetPackage
{
	// Token: 0x1700065C RID: 1628
	// (get) Token: 0x06003BCB RID: 15307 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003BCC RID: 15308 RVA: 0x00180A17 File Offset: 0x0017EC17
	public NetPackageEntityMapMarkerRemove Setup(EnumMapObjectType _mapObjectType, int _entityId)
	{
		this.mapObjectType = _mapObjectType;
		this.entityId = _entityId;
		this.RemoveByType = NetPackageEntityMapMarkerRemove.RemoveByTypes.EntityID;
		return this;
	}

	// Token: 0x06003BCD RID: 15309 RVA: 0x00180A2F File Offset: 0x0017EC2F
	public NetPackageEntityMapMarkerRemove Setup(EnumMapObjectType _mapObjectType, Vector3 _position)
	{
		this.mapObjectType = _mapObjectType;
		this.position = _position;
		this.RemoveByType = NetPackageEntityMapMarkerRemove.RemoveByTypes.Position;
		return this;
	}

	// Token: 0x06003BCE RID: 15310 RVA: 0x00180A47 File Offset: 0x0017EC47
	public override void read(PooledBinaryReader _reader)
	{
		this.RemoveByType = (NetPackageEntityMapMarkerRemove.RemoveByTypes)_reader.ReadInt32();
		if (this.RemoveByType == NetPackageEntityMapMarkerRemove.RemoveByTypes.EntityID)
		{
			this.entityId = _reader.ReadInt32();
		}
		else
		{
			this.position = StreamUtils.ReadVector3(_reader);
		}
		this.mapObjectType = (EnumMapObjectType)_reader.ReadInt32();
	}

	// Token: 0x06003BCF RID: 15311 RVA: 0x00180A84 File Offset: 0x0017EC84
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((int)this.RemoveByType);
		if (this.RemoveByType == NetPackageEntityMapMarkerRemove.RemoveByTypes.EntityID)
		{
			_writer.Write(this.entityId);
		}
		else
		{
			StreamUtils.Write(_writer, this.position);
		}
		_writer.Write((int)this.mapObjectType);
	}

	// Token: 0x06003BD0 RID: 15312 RVA: 0x00180AD2 File Offset: 0x0017ECD2
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (this.RemoveByType == NetPackageEntityMapMarkerRemove.RemoveByTypes.EntityID)
		{
			_world.ObjectOnMapRemove(this.mapObjectType, this.entityId);
			return;
		}
		_world.ObjectOnMapRemove(this.mapObjectType, this.position);
	}

	// Token: 0x040030A1 RID: 12449
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x040030A2 RID: 12450
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 position;

	// Token: 0x040030A3 RID: 12451
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumMapObjectType mapObjectType;

	// Token: 0x040030A4 RID: 12452
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEntityMapMarkerRemove.RemoveByTypes RemoveByType;

	// Token: 0x020007EC RID: 2028
	[PublicizedFrom(EAccessModifier.Private)]
	public enum RemoveByTypes
	{
		// Token: 0x040030A6 RID: 12454
		EntityID,
		// Token: 0x040030A7 RID: 12455
		Position
	}
}
