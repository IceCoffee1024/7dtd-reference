using System;

// Token: 0x020008A8 RID: 2216
public readonly struct WorldAddress
{
	// Token: 0x06003FD8 RID: 16344 RVA: 0x00191D71 File Offset: 0x0018FF71
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldAddress(WorldAddress.WorldObjectType _type, Vector3i _pos, string _featureName, int _entityId)
	{
		this.type = _type;
		this.pos = _pos;
		this.featureName = _featureName;
		this.entityId = _entityId;
	}

	// Token: 0x06003FD9 RID: 16345 RVA: 0x00191D90 File Offset: 0x0018FF90
	public static WorldAddress Create(IHasWorldAddress _target)
	{
		if (_target == null)
		{
			return default(WorldAddress);
		}
		return _target.GetAddress();
	}

	// Token: 0x06003FDA RID: 16346 RVA: 0x00191DB0 File Offset: 0x0018FFB0
	public static WorldAddress CreateForTileEntity(Vector3i _pos)
	{
		return new WorldAddress(WorldAddress.WorldObjectType.TileEntity, _pos, null, -1);
	}

	// Token: 0x06003FDB RID: 16347 RVA: 0x00191DBB File Offset: 0x0018FFBB
	public static WorldAddress CreateForTEFeature(Vector3i _pos, string _featureName)
	{
		return new WorldAddress(WorldAddress.WorldObjectType.TEFeature, _pos, _featureName ?? string.Empty, -1);
	}

	// Token: 0x06003FDC RID: 16348 RVA: 0x00191DCF File Offset: 0x0018FFCF
	public static WorldAddress CreateForEntity(int _entityId)
	{
		return new WorldAddress(WorldAddress.WorldObjectType.Entity, Vector3i.zero, null, _entityId);
	}

	// Token: 0x06003FDD RID: 16349 RVA: 0x00191DE0 File Offset: 0x0018FFE0
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

	// Token: 0x06003FDE RID: 16350 RVA: 0x00191F84 File Offset: 0x00190184
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

	// Token: 0x06003FDF RID: 16351 RVA: 0x00191FF4 File Offset: 0x001901F4
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

	// Token: 0x040033C9 RID: 13257
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly WorldAddress.WorldObjectType type;

	// Token: 0x040033CA RID: 13258
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Vector3i pos;

	// Token: 0x040033CB RID: 13259
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string featureName;

	// Token: 0x040033CC RID: 13260
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int entityId;

	// Token: 0x020008A9 RID: 2217
	public enum WorldObjectType : byte
	{
		// Token: 0x040033CE RID: 13262
		None,
		// Token: 0x040033CF RID: 13263
		TileEntity,
		// Token: 0x040033D0 RID: 13264
		TEFeature,
		// Token: 0x040033D1 RID: 13265
		Entity
	}
}
