using System;
using System.Collections.Generic;
using PrefabVolumes;
using UnityEngine;

// Token: 0x02000BC0 RID: 3008
public class TraderArea
{
	// Token: 0x17000977 RID: 2423
	// (get) Token: 0x06005A9F RID: 23199 RVA: 0x0022D94D File Offset: 0x0022BB4D
	public bool IsInitialized
	{
		get
		{
			return this.owningTrader != null;
		}
	}

	// Token: 0x06005AA0 RID: 23200 RVA: 0x0022D95C File Offset: 0x0022BB5C
	public TraderArea(Vector3i _pos, Vector3i _size, Vector3i _protectPadding, PrefabTeleportVolumeList _teleportVolumes)
	{
		this.Position = _pos;
		this.PrefabSize = _size;
		this.ProtectSize = _size + _protectPadding;
		this.ProtectSize.x = this.ProtectSize.x + 2;
		this.ProtectSize.z = this.ProtectSize.z + 2;
		this.ProtectPosition = _pos + _size / 2 - this.ProtectSize / 2;
		this.ProtectBounds = new BoundsInt(this.ProtectPosition, this.ProtectSize);
		this.TeleportVolumes.CopyFrom(_teleportVolumes);
	}

	// Token: 0x06005AA1 RID: 23201 RVA: 0x0022DA10 File Offset: 0x0022BC10
	public Vector3i GetProtectPadding()
	{
		Vector3i vector3i = this.ProtectSize - this.PrefabSize;
		vector3i.x -= 2;
		vector3i.z -= 2;
		return vector3i;
	}

	// Token: 0x06005AA2 RID: 23202 RVA: 0x0022DA48 File Offset: 0x0022BC48
	public bool IsWithinProtectArea(Vector3 _pos)
	{
		return (float)this.ProtectBounds.xMin <= _pos.x && _pos.x <= (float)this.ProtectBounds.xMax && (float)this.ProtectBounds.yMin <= _pos.y && _pos.y <= (float)this.ProtectBounds.yMax && (float)this.ProtectBounds.zMin <= _pos.z && _pos.z <= (float)this.ProtectBounds.zMax;
	}

	// Token: 0x06005AA3 RID: 23203 RVA: 0x0022DAD4 File Offset: 0x0022BCD4
	public bool IsWithinTeleportArea(Vector3 _pos, out PrefabTeleportVolume tpVolume)
	{
		if (World.SandboxUseTraderArea != TraderAreaStates.Default)
		{
			tpVolume = null;
			return false;
		}
		for (int i = 0; i < this.TeleportVolumes.Count; i++)
		{
			PrefabTeleportVolume prefabTeleportVolume = this.TeleportVolumes[i];
			Vector3i vector3i = this.Position + prefabTeleportVolume.startPos;
			if ((float)vector3i.x <= _pos.x && _pos.x <= (float)(vector3i.x + prefabTeleportVolume.size.x) && (float)vector3i.y <= _pos.y && _pos.y <= (float)(vector3i.y + prefabTeleportVolume.size.y) && (float)vector3i.z <= _pos.z && _pos.z <= (float)(vector3i.z + prefabTeleportVolume.size.z))
			{
				tpVolume = prefabTeleportVolume;
				return true;
			}
		}
		tpVolume = null;
		return false;
	}

	// Token: 0x06005AA4 RID: 23204 RVA: 0x0022DBB8 File Offset: 0x0022BDB8
	public bool SetClosed(World _world, bool _bClosed, EntityTrader _trader, bool playSound = false)
	{
		this.owningTrader = _trader;
		this.IsClosed = _bClosed;
		int num = World.toChunkXZ(this.Position.x - 1);
		int num2 = World.toChunkXZ(this.Position.x + this.PrefabSize.x + 1);
		int num3 = World.toChunkXZ(this.Position.z - 1);
		int num4 = World.toChunkXZ(this.Position.z + this.PrefabSize.z + 1);
		for (int i = num3; i <= num4; i++)
		{
			for (int j = num; j <= num2; j++)
			{
				if (!(_world.GetChunkSync(j, i) is Chunk))
				{
					return false;
				}
			}
		}
		for (int k = num3; k <= num4; k++)
		{
			for (int l = num; l <= num2; l++)
			{
				Chunk chunk = _world.GetChunkSync(l, k) as Chunk;
				List<Vector3i> list = chunk.IndexedBlocks["TraderOnOff"];
				if (list != null)
				{
					for (int m = 0; m < list.Count; m++)
					{
						BlockValue block = chunk.GetBlock(list[m]);
						if (!block.ischild)
						{
							Vector3i vector3i = chunk.ToWorldPos(list[m]);
							if (this.ProtectBounds.Contains(vector3i))
							{
								Block block2 = block.Block;
								TileEntityComposite tileEntityComposite = _world.GetTileEntity(vector3i) as TileEntityComposite;
								TEFeatureDoor tefeatureDoor;
								if (tileEntityComposite.TryGetSelfOrFeature<TEFeatureDoor>(out tefeatureDoor))
								{
									if (_bClosed && tefeatureDoor.IsOpen())
									{
										tefeatureDoor.SetOpen(false, false);
									}
									TEFeatureLockable tefeatureLockable;
									if (tileEntityComposite.TryGetSelfOrFeature<TEFeatureLockable>(out tefeatureLockable))
									{
										if (_bClosed)
										{
											if (!tefeatureLockable.IsLocked())
											{
												tefeatureLockable.SetLocked(true);
											}
										}
										else if (tefeatureLockable.IsLocked())
										{
											tefeatureLockable.SetLocked(false);
										}
									}
								}
								else if (block2 is BlockLight)
								{
									block.meta = (byte)((!_bClosed) ? ((int)(block.meta | 2)) : ((int)block.meta & -3));
									_world.SetBlockRPC(vector3i, block);
								}
								else if (block2 is BlockSpeakerTrader && playSound)
								{
									BlockSpeakerTrader blockSpeakerTrader = block2 as BlockSpeakerTrader;
									if (_bClosed)
									{
										blockSpeakerTrader.PlayClose(vector3i, _trader);
									}
									else
									{
										blockSpeakerTrader.PlayOpen(vector3i, _trader);
									}
								}
							}
						}
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06005AA5 RID: 23205 RVA: 0x0022DE04 File Offset: 0x0022C004
	public void HandleWarning(World _world, EntityTrader _trader)
	{
		int num = World.toChunkXZ(this.Position.x - 1);
		int num2 = World.toChunkXZ(this.Position.x + this.PrefabSize.x + 1);
		int num3 = World.toChunkXZ(this.Position.z - 1);
		int num4 = World.toChunkXZ(this.Position.z + this.PrefabSize.z + 1);
		for (int i = num; i <= num2; i++)
		{
			for (int j = num3; j <= num4; j++)
			{
				Chunk chunk = _world.GetChunkSync(i, j) as Chunk;
				if (chunk != null)
				{
					List<Vector3i> list = chunk.IndexedBlocks["TraderOnOff"];
					if (list != null)
					{
						for (int k = 0; k < list.Count; k++)
						{
							BlockValue block = chunk.GetBlock(list[k]);
							if (!block.ischild)
							{
								BlockSpeakerTrader blockSpeakerTrader = block.Block as BlockSpeakerTrader;
								if (blockSpeakerTrader != null)
								{
									Vector3i vector3i = chunk.ToWorldPos(list[k]);
									blockSpeakerTrader.PlayWarning(vector3i, _trader);
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06005AA6 RID: 23206 RVA: 0x0022DF28 File Offset: 0x0022C128
	public bool Overlaps(Vector3i _min, Vector3i _max)
	{
		return _max.x >= this.ProtectPosition.x && _min.x < this.ProtectPosition.x + this.ProtectSize.x && _max.z >= this.ProtectPosition.z && _min.z < this.ProtectPosition.z + this.ProtectSize.z;
	}

	// Token: 0x06005AA7 RID: 23207 RVA: 0x0022DF9E File Offset: 0x0022C19E
	public int GetReadWriteSize()
	{
		return 21 + (1 + this.TeleportVolumes.Count * 6);
	}

	// Token: 0x06005AA8 RID: 23208 RVA: 0x0022DFB4 File Offset: 0x0022C1B4
	public static TraderArea Read(PooledBinaryReader _reader)
	{
		Vector3i vector3i;
		vector3i.x = _reader.ReadInt32();
		vector3i.y = _reader.ReadInt32();
		vector3i.z = _reader.ReadInt32();
		Vector3i vector3i2;
		vector3i2.x = (int)_reader.ReadInt16();
		vector3i2.y = (int)_reader.ReadInt16();
		vector3i2.z = (int)_reader.ReadInt16();
		Vector3i vector3i3;
		vector3i3.x = (int)_reader.ReadSByte();
		vector3i3.y = (int)_reader.ReadSByte();
		vector3i3.z = (int)_reader.ReadSByte();
		int num = (int)_reader.ReadByte();
		PrefabTeleportVolumeList prefabTeleportVolumeList = new PrefabTeleportVolumeList(null);
		for (int i = 0; i < num; i++)
		{
			Vector3i vector3i4;
			vector3i4.x = (int)_reader.ReadSByte();
			vector3i4.y = (int)_reader.ReadSByte();
			vector3i4.z = (int)_reader.ReadSByte();
			Vector3i vector3i5;
			vector3i5.x = (int)_reader.ReadByte();
			vector3i5.y = (int)_reader.ReadByte();
			vector3i5.z = (int)_reader.ReadByte();
			PrefabTeleportVolume prefabTeleportVolume = new PrefabTeleportVolume();
			prefabTeleportVolume.Use(vector3i4, vector3i5);
			prefabTeleportVolumeList.AddExistingVolume(prefabTeleportVolume);
		}
		return new TraderArea(vector3i, vector3i2, vector3i3, prefabTeleportVolumeList);
	}

	// Token: 0x06005AA9 RID: 23209 RVA: 0x0022E0C8 File Offset: 0x0022C2C8
	public void Write(PooledBinaryWriter _writer)
	{
		_writer.Write(this.Position.x);
		_writer.Write(this.Position.y);
		_writer.Write(this.Position.z);
		_writer.Write((short)this.PrefabSize.x);
		_writer.Write((short)this.PrefabSize.y);
		_writer.Write((short)this.PrefabSize.z);
		Vector3i protectPadding = this.GetProtectPadding();
		_writer.Write((sbyte)protectPadding.x);
		_writer.Write((sbyte)protectPadding.y);
		_writer.Write((sbyte)protectPadding.z);
		_writer.Write((byte)this.TeleportVolumes.Count);
		for (int i = 0; i < this.TeleportVolumes.Count; i++)
		{
			PrefabTeleportVolume prefabTeleportVolume = this.TeleportVolumes[i];
			_writer.Write((sbyte)prefabTeleportVolume.startPos.x);
			_writer.Write((sbyte)prefabTeleportVolume.startPos.y);
			_writer.Write((sbyte)prefabTeleportVolume.startPos.z);
			_writer.Write((byte)prefabTeleportVolume.size.x);
			_writer.Write((byte)prefabTeleportVolume.size.y);
			_writer.Write((byte)prefabTeleportVolume.size.z);
		}
	}

	// Token: 0x06005AAA RID: 23210 RVA: 0x0022E210 File Offset: 0x0022C410
	public EntityTrader GetTrader()
	{
		if (this.owningTrader != null)
		{
			return this.owningTrader;
		}
		List<Entity> entitiesInBounds = GameManager.Instance.World.GetEntitiesInBounds(null, new Bounds(this.ProtectBounds.center, this.ProtectBounds.size));
		if (entitiesInBounds.Count > 0)
		{
			for (int i = 0; i < entitiesInBounds.Count; i++)
			{
				EntityTrader entityTrader = entitiesInBounds[i] as EntityTrader;
				if (entityTrader != null)
				{
					return entityTrader;
				}
			}
		}
		return null;
	}

	// Token: 0x0400462B RID: 17963
	public Vector3i Position;

	// Token: 0x0400462C RID: 17964
	public Vector3i PrefabSize;

	// Token: 0x0400462D RID: 17965
	public Vector3i ProtectPosition;

	// Token: 0x0400462E RID: 17966
	public Vector3i ProtectSize;

	// Token: 0x0400462F RID: 17967
	public BoundsInt ProtectBounds;

	// Token: 0x04004630 RID: 17968
	public bool IsClosed = true;

	// Token: 0x04004631 RID: 17969
	public readonly PrefabTeleportVolumeList TeleportVolumes = new PrefabTeleportVolumeList(null);

	// Token: 0x04004632 RID: 17970
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cPadXZ = 2;

	// Token: 0x04004633 RID: 17971
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityTrader owningTrader;
}
