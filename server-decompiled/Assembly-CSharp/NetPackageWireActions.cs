using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200086B RID: 2155
[Preserve]
public class NetPackageWireActions : NetPackage
{
	// Token: 0x06003EF2 RID: 16114 RVA: 0x0018C12F File Offset: 0x0018A32F
	public NetPackageWireActions Setup(NetPackageWireActions.WireActions _operation, Vector3i _tileEntityPosition, List<Vector3i> _wireChildren, int wiringEntity = -1)
	{
		this.currentOperation = _operation;
		this.tileEntityPosition = _tileEntityPosition;
		this.wireChildren = _wireChildren;
		return this;
	}

	// Token: 0x06003EF3 RID: 16115 RVA: 0x0018C148 File Offset: 0x0018A348
	public override void read(PooledBinaryReader _br)
	{
		this.currentOperation = (NetPackageWireActions.WireActions)_br.ReadByte();
		this.tileEntityPosition = StreamUtils.ReadVector3i(_br);
		int num = (int)_br.ReadByte();
		this.wireChildren.Clear();
		for (int i = 0; i < num; i++)
		{
			this.wireChildren.Add(StreamUtils.ReadVector3i(_br));
		}
		if (this.currentOperation != NetPackageWireActions.WireActions.SendWires)
		{
			this.wiringEntityID = _br.ReadInt32();
		}
	}

	// Token: 0x06003EF4 RID: 16116 RVA: 0x0018C1B4 File Offset: 0x0018A3B4
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.currentOperation);
		StreamUtils.Write(_bw, this.tileEntityPosition);
		_bw.Write((byte)this.wireChildren.Count);
		for (int i = 0; i < this.wireChildren.Count; i++)
		{
			StreamUtils.Write(_bw, this.wireChildren[i]);
		}
		if (this.currentOperation != NetPackageWireActions.WireActions.SendWires)
		{
			_bw.Write(this.wiringEntityID);
		}
	}

	// Token: 0x06003EF5 RID: 16117 RVA: 0x0018C230 File Offset: 0x0018A430
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			switch (this.currentOperation)
			{
			case NetPackageWireActions.WireActions.SetParent:
			{
				TileEntityPowered poweredTileEntity = this.GetPoweredTileEntity(_world, this.tileEntityPosition);
				ushort num = 0;
				PowerItem powerItem = PowerManager.Instance.GetPowerItemByWorldPos(poweredTileEntity.ToWorldPos());
				if (powerItem == null)
				{
					powerItem = poweredTileEntity.CreatePowerItemForTileEntity(num);
					poweredTileEntity.SetModified();
					powerItem.AddTileEntity(poweredTileEntity);
				}
				TileEntityPowered poweredTileEntity2 = this.GetPoweredTileEntity(_world, this.wireChildren[0]);
				PowerItem powerItem2 = PowerManager.Instance.GetPowerItemByWorldPos(poweredTileEntity2.ToWorldPos());
				if (powerItem2 == null)
				{
					powerItem2 = poweredTileEntity2.CreatePowerItemForTileEntity(num);
					poweredTileEntity2.SetModified();
					powerItem2.AddTileEntity(poweredTileEntity2);
				}
				PowerItem parent = powerItem.Parent;
				PowerManager.Instance.SetParent(powerItem, powerItem2);
				if (parent != null && parent.TileEntity != null)
				{
					parent.TileEntity.CreateWireDataFromPowerItem();
					parent.TileEntity.SendWireData();
					parent.TileEntity.RemoveWires();
					parent.TileEntity.DrawWires();
				}
				if (powerItem2.TileEntity != null)
				{
					powerItem2.TileEntity.CreateWireDataFromPowerItem();
					powerItem2.TileEntity.SendWireData();
					powerItem2.TileEntity.RemoveWires();
					powerItem2.TileEntity.DrawWires();
					return;
				}
				break;
			}
			case NetPackageWireActions.WireActions.RemoveParent:
			{
				PowerItem powerItem3 = this.GetPoweredTileEntity(_world, this.tileEntityPosition).GetPowerItem();
				if (powerItem3.Parent != null)
				{
					PowerItem parent2 = powerItem3.Parent;
					powerItem3.RemoveSelfFromParent();
					if (parent2.TileEntity != null)
					{
						parent2.TileEntity.CreateWireDataFromPowerItem();
						parent2.TileEntity.SendWireData();
						parent2.TileEntity.RemoveWires();
						parent2.TileEntity.DrawWires();
						return;
					}
				}
				break;
			}
			case NetPackageWireActions.WireActions.SendWires:
				break;
			default:
				return;
			}
		}
		else if (_world.GetChunkFromWorldPos(this.tileEntityPosition.x, this.tileEntityPosition.y, this.tileEntityPosition.z) is Chunk)
		{
			IPowered powered = _world.GetTileEntity(this.tileEntityPosition) as IPowered;
			if (this.currentOperation == NetPackageWireActions.WireActions.SendWires && powered != null)
			{
				powered.SetWireData(this.wireChildren);
			}
		}
	}

	// Token: 0x06003EF6 RID: 16118 RVA: 0x0018C444 File Offset: 0x0018A644
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPowered GetPoweredTileEntity(World _world, Vector3i tileEntityPosition)
	{
		Chunk chunk = _world.GetChunkFromWorldPos(tileEntityPosition.x, tileEntityPosition.y, tileEntityPosition.z) as Chunk;
		TileEntityPowered tileEntityPowered = _world.GetTileEntity(tileEntityPosition) as TileEntityPowered;
		if (tileEntityPowered == null)
		{
			Block block = _world.GetBlock(tileEntityPosition).Block;
			if (block is BlockPowered)
			{
				tileEntityPowered = (block as BlockPowered).CreateTileEntity(chunk);
			}
			tileEntityPowered.localChunkPos = World.toBlock(tileEntityPosition);
			BlockEntityData blockEntity = chunk.GetBlockEntity(tileEntityPosition);
			if (blockEntity != null)
			{
				tileEntityPowered.BlockTransform = blockEntity.transform;
			}
			tileEntityPowered.InitializePowerData();
			chunk.AddTileEntity(tileEntityPowered);
		}
		return tileEntityPowered;
	}

	// Token: 0x06003EF7 RID: 16119 RVA: 0x00180278 File Offset: 0x0017E478
	public override int GetLength()
	{
		return 12;
	}

	// Token: 0x040032CD RID: 13005
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i tileEntityPosition;

	// Token: 0x040032CE RID: 13006
	[PublicizedFrom(EAccessModifier.Private)]
	public int wiringEntityID;

	// Token: 0x040032CF RID: 13007
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageWireActions.WireActions currentOperation;

	// Token: 0x040032D0 RID: 13008
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3i> wireChildren = new List<Vector3i>();

	// Token: 0x0200086C RID: 2156
	public enum WireActions
	{
		// Token: 0x040032D2 RID: 13010
		SetParent,
		// Token: 0x040032D3 RID: 13011
		RemoveParent,
		// Token: 0x040032D4 RID: 13012
		SendWires
	}
}
