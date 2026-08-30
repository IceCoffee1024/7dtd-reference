using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007DE RID: 2014
[Preserve]
public class NetPackageEntityMapMarkerRemove : NetPackage
{
	// Token: 0x17000631 RID: 1585
	// (get) Token: 0x06003B95 RID: 15253 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B96 RID: 15254 RVA: 0x0017F3CF File Offset: 0x0017D5CF
	public NetPackageEntityMapMarkerRemove Setup(EnumMapObjectType _mapObjectType, int _entityId)
	{
		this.mapObjectType = _mapObjectType;
		this.entityId = _entityId;
		this.RemoveByType = NetPackageEntityMapMarkerRemove.RemoveByTypes.EntityID;
		return this;
	}

	// Token: 0x06003B97 RID: 15255 RVA: 0x0017F3E7 File Offset: 0x0017D5E7
	public NetPackageEntityMapMarkerRemove Setup(EnumMapObjectType _mapObjectType, Vector3 _position)
	{
		this.mapObjectType = _mapObjectType;
		this.position = _position;
		this.RemoveByType = NetPackageEntityMapMarkerRemove.RemoveByTypes.Position;
		return this;
	}

	// Token: 0x06003B98 RID: 15256 RVA: 0x0017F3FF File Offset: 0x0017D5FF
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

	// Token: 0x06003B99 RID: 15257 RVA: 0x0017F43C File Offset: 0x0017D63C
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

	// Token: 0x06003B9A RID: 15258 RVA: 0x0017F48A File Offset: 0x0017D68A
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

	// Token: 0x06003B9B RID: 15259 RVA: 0x00080550 File Offset: 0x0007E750
	public override int GetLength()
	{
		return 4;
	}

	// Token: 0x04003088 RID: 12424
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003089 RID: 12425
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 position;

	// Token: 0x0400308A RID: 12426
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumMapObjectType mapObjectType;

	// Token: 0x0400308B RID: 12427
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEntityMapMarkerRemove.RemoveByTypes RemoveByType;

	// Token: 0x020007DF RID: 2015
	[PublicizedFrom(EAccessModifier.Private)]
	public enum RemoveByTypes
	{
		// Token: 0x0400308D RID: 12429
		EntityID,
		// Token: 0x0400308E RID: 12430
		Position
	}
}
