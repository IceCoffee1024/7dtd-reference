using System;

// Token: 0x020008B2 RID: 2226
public readonly struct WorldAddress
{
	// Token: 0x06003FFB RID: 16379 RVA: 0x00192E19 File Offset: 0x00191019
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldAddress(WorldAddress.WorldObjectType _type, Vector3i _pos, string _featureName, int _entityId)
	{
		this.type = _type;
		this.pos = _pos;
		this.featureName = _featureName;
		this.entityId = _entityId;
	}

	// Token: 0x06003FFC RID: 16380 RVA: 0x00192E38 File Offset: 0x00191038
	public static WorldAddress Create(IHasWorldAddress _target)
	{
		if (_target == null)
		{
			return default(WorldAddress);
		}
		return _target.GetAddress();
	}

	// Token: 0x06003FFD RID: 16381 RVA: 0x00192E58 File Offset: 0x00191058
	public static WorldAddress CreateForTileEntity(Vector3i _pos)
	{
		return new WorldAddress(WorldAddress.WorldObjectType.TileEntity, _pos, null, -1);
	}

	// Token: 0x06003FFE RID: 16382 RVA: 0x00192E63 File Offset: 0x00191063
	public static WorldAddress CreateForTEFeature(Vector3i _pos, string _featureName)
	{
		return new WorldAddress(WorldAddress.WorldObjectType.TEFeature, _pos, _featureName ?? string.Empty, -1);
	}

	// Token: 0x06003FFF RID: 16383 RVA: 0x00192E77 File Offset: 0x00191077
	public static WorldAddress CreateForEntity(int _entityId)
	{
		return new WorldAddress(WorldAddress.WorldObjectType.Entity, Vector3i.zero, null, _entityId);
	}

	// Token: 0x06004000 RID: 16384 RVA: 0x00192E88 File Offset: 0x00191088
	public T Resolve<T>() where T : class, IHasWorldAddress
	{
		object obj = null;
		switch (this.type)
		{
		case WorldAddress.WorldObjectType.TileEntity:
			obj = GameManager.Instance.World.GetTileEntity(this.pos);
			if (obj == null)
			{
				Log.Warning(string.Format("[{0}] {1} not found (pos={2}).", "WorldAddress", "TileEntity", this.pos));
			}
			break;
		case WorldAddress.WorldObjectType.TEFeature:
		{
			TileEntityComposite tileEntityComposite = GameManager.Instance.World.GetTileEntity(this.pos) as TileEntityComposite;
			if (tileEntityComposite == null)
			{
				Log.Warning(string.Format("[{0}] {1} not found (pos={2}, name='{3}').", new object[] { "WorldAddress", "TileEntityComposite", this.pos, this.featureName }));
				return default(T);
			}
			obj = tileEntityComposite.GetFeature(this.featureName.AsMemory());
			if (obj == null)
			{
				Log.Warning(string.Format("[{0}] Feature '{1}' not found on parent (pos={2}).", "WorldAddress", this.featureName, this.pos));
			}
			break;
		}
		case WorldAddress.WorldObjectType.Entity:
			obj = ((this.entityId != -1) ? GameManager.Instance.World.GetEntity(this.entityId) : null);
			if (obj == null)
			{
				Log.Warning(string.Format("[{0}] {1} not found (id={2}).", "WorldAddress", "Entity", this.entityId));
			}
			break;
		}
		T t = obj as T;
		if (t != null)
		{
			return t;
		}
		Log.Warning("[WorldAddress] could not resolve world object to type " + typeof(T).Name + ".");
		return default(T);
	}

	// Token: 0x06004001 RID: 16385 RVA: 0x0019302C File Offset: 0x0019122C
	public void Write(PooledBinaryWriter _bw)
	{
		_bw.Write((byte)this.type);
		switch (this.type)
		{
		case WorldAddress.WorldObjectType.TileEntity:
			StreamUtils.Write(_bw, this.pos);
			return;
		case WorldAddress.WorldObjectType.TEFeature:
			StreamUtils.Write(_bw, this.pos);
			_bw.Write(this.featureName ?? string.Empty);
			return;
		case WorldAddress.WorldObjectType.Entity:
			_bw.Write(this.entityId);
			return;
		default:
			return;
		}
	}

	// Token: 0x06004002 RID: 16386 RVA: 0x0019309C File Offset: 0x0019129C
	public static WorldAddress Read(PooledBinaryReader _br)
	{
		WorldAddress.WorldObjectType worldObjectType = (WorldAddress.WorldObjectType)_br.ReadByte();
		switch (worldObjectType)
		{
		case WorldAddress.WorldObjectType.None:
			return default(WorldAddress);
		case WorldAddress.WorldObjectType.TileEntity:
			return new WorldAddress(worldObjectType, StreamUtils.ReadVector3i(_br), null, -1);
		case WorldAddress.WorldObjectType.TEFeature:
		{
			Vector3i vector3i = StreamUtils.ReadVector3i(_br);
			return new WorldAddress(worldObjectType, vector3i, _br.ReadString(), -1);
		}
		case WorldAddress.WorldObjectType.Entity:
			return new WorldAddress(worldObjectType, Vector3i.zero, null, _br.ReadInt32());
		default:
			Log.Warning(string.Format("[{0}] Unsupported type {1}.", "WorldAddress", worldObjectType));
			return default(WorldAddress);
		}
	}

	// Token: 0x040033E9 RID: 13289
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly WorldAddress.WorldObjectType type;

	// Token: 0x040033EA RID: 13290
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Vector3i pos;

	// Token: 0x040033EB RID: 13291
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string featureName;

	// Token: 0x040033EC RID: 13292
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int entityId;

	// Token: 0x020008B3 RID: 2227
	public enum WorldObjectType : byte
	{
		// Token: 0x040033EE RID: 13294
		None,
		// Token: 0x040033EF RID: 13295
		TileEntity,
		// Token: 0x040033F0 RID: 13296
		TEFeature,
		// Token: 0x040033F1 RID: 13297
		Entity
	}
}
