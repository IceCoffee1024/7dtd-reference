using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007E1 RID: 2017
[Preserve]
public class NetPackageEntityMapMarkerRemove : NetPackage
{
	// Token: 0x17000656 RID: 1622
	// (get) Token: 0x06003BA8 RID: 15272 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003BA9 RID: 15273 RVA: 0x0017F913 File Offset: 0x0017DB13
	public NetPackageEntityMapMarkerRemove Setup(EnumMapObjectType _mapObjectType, int _entityId)
	{
		this.mapObjectType = _mapObjectType;
		this.entityId = _entityId;
		this.RemoveByType = NetPackageEntityMapMarkerRemove.RemoveByTypes.EntityID;
		return this;
	}

	// Token: 0x06003BAA RID: 15274 RVA: 0x0017F92B File Offset: 0x0017DB2B
	public NetPackageEntityMapMarkerRemove Setup(EnumMapObjectType _mapObjectType, Vector3 _position)
	{
		this.mapObjectType = _mapObjectType;
		this.position = _position;
		this.RemoveByType = NetPackageEntityMapMarkerRemove.RemoveByTypes.Position;
		return this;
	}

	// Token: 0x06003BAB RID: 15275 RVA: 0x0017F943 File Offset: 0x0017DB43
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

	// Token: 0x06003BAC RID: 15276 RVA: 0x0017F980 File Offset: 0x0017DB80
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

	// Token: 0x06003BAD RID: 15277 RVA: 0x0017F9CE File Offset: 0x0017DBCE
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

	// Token: 0x04003081 RID: 12417
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003082 RID: 12418
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 position;

	// Token: 0x04003083 RID: 12419
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumMapObjectType mapObjectType;

	// Token: 0x04003084 RID: 12420
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEntityMapMarkerRemove.RemoveByTypes RemoveByType;

	// Token: 0x020007E2 RID: 2018
	[PublicizedFrom(EAccessModifier.Private)]
	public enum RemoveByTypes
	{
		// Token: 0x04003086 RID: 12422
		EntityID,
		// Token: 0x04003087 RID: 12423
		Position
	}
}
