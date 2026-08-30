using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007D3 RID: 2003
[Preserve]
public class NetPackageEntityMapMarkerRemove : NetPackage
{
	// Token: 0x17000626 RID: 1574
	// (get) Token: 0x06003B36 RID: 15158 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B37 RID: 15159 RVA: 0x0017CFF7 File Offset: 0x0017B1F7
	public NetPackageEntityMapMarkerRemove Setup(EnumMapObjectType _mapObjectType, int _entityId)
	{
		this.mapObjectType = _mapObjectType;
		this.entityId = _entityId;
		this.RemoveByType = NetPackageEntityMapMarkerRemove.RemoveByTypes.EntityID;
		return this;
	}

	// Token: 0x06003B38 RID: 15160 RVA: 0x0017D00F File Offset: 0x0017B20F
	public NetPackageEntityMapMarkerRemove Setup(EnumMapObjectType _mapObjectType, Vector3 _position)
	{
		this.mapObjectType = _mapObjectType;
		this.position = _position;
		this.RemoveByType = NetPackageEntityMapMarkerRemove.RemoveByTypes.Position;
		return this;
	}

	// Token: 0x06003B39 RID: 15161 RVA: 0x0017D027 File Offset: 0x0017B227
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

	// Token: 0x06003B3A RID: 15162 RVA: 0x0017D064 File Offset: 0x0017B264
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

	// Token: 0x06003B3B RID: 15163 RVA: 0x0017D0B2 File Offset: 0x0017B2B2
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

	// Token: 0x06003B3C RID: 15164 RVA: 0x0007FE9C File Offset: 0x0007E09C
	public override int GetLength()
	{
		return 4;
	}

	// Token: 0x04003009 RID: 12297
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x0400300A RID: 12298
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 position;

	// Token: 0x0400300B RID: 12299
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumMapObjectType mapObjectType;

	// Token: 0x0400300C RID: 12300
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEntityMapMarkerRemove.RemoveByTypes RemoveByType;

	// Token: 0x020007D4 RID: 2004
	[PublicizedFrom(EAccessModifier.Private)]
	public enum RemoveByTypes
	{
		// Token: 0x0400300E RID: 12302
		EntityID,
		// Token: 0x0400300F RID: 12303
		Position
	}
}
