using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200086D RID: 2157
[Preserve]
public class NetPackageWireToolActions : NetPackage
{
	// Token: 0x06003EF9 RID: 16121 RVA: 0x0018C4EB File Offset: 0x0018A6EB
	public NetPackageWireToolActions Setup(NetPackageWireToolActions.WireActions _operation, Vector3i _tileEntityPosition, int _entityID)
	{
		this.currentOperation = _operation;
		this.tileEntityPosition = _tileEntityPosition;
		this.entityID = _entityID;
		return this;
	}

	// Token: 0x06003EFA RID: 16122 RVA: 0x0018C503 File Offset: 0x0018A703
	public override void read(PooledBinaryReader _br)
	{
		this.currentOperation = (NetPackageWireToolActions.WireActions)_br.ReadByte();
		this.tileEntityPosition = StreamUtils.ReadVector3i(_br);
		this.entityID = _br.ReadInt32();
	}

	// Token: 0x06003EFB RID: 16123 RVA: 0x0018C529 File Offset: 0x0018A729
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.currentOperation);
		StreamUtils.Write(_bw, this.tileEntityPosition);
		_bw.Write(this.entityID);
	}

	// Token: 0x06003EFC RID: 16124 RVA: 0x0018C558 File Offset: 0x0018A758
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (!base.ValidEntityIdForSender(this.entityID, false))
		{
			return;
		}
		NetPackageWireToolActions.WireActions wireActions = this.currentOperation;
		if (wireActions != NetPackageWireToolActions.WireActions.AddWire)
		{
			if (wireActions != NetPackageWireToolActions.WireActions.RemoveWire)
			{
				return;
			}
			EntityPlayer entityPlayer = _world.GetEntity(this.entityID) as EntityPlayer;
			if (entityPlayer != null && entityPlayer.RootTransform.FindInChilds(entityPlayer.GetRightHandTransformName(), false) != null)
			{
				ItemActionConnectPower.ConnectPowerData connectPowerData = entityPlayer.inventory.holdingItemData.actionData[1] as ItemActionConnectPower.ConnectPowerData;
				if (connectPowerData != null && connectPowerData.wireNode != null)
				{
					global::UnityEngine.Object.Destroy(connectPowerData.wireNode.gameObject);
					connectPowerData.wireNode = null;
				}
			}
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageWireToolActions>().Setup(this.currentOperation, this.tileEntityPosition, this.entityID), false, -1, this.entityID, -1, null, 192, false);
			}
		}
		else
		{
			Chunk chunk = _world.GetChunkFromWorldPos(this.tileEntityPosition.x, this.tileEntityPosition.y, this.tileEntityPosition.z) as Chunk;
			if (chunk == null)
			{
				return;
			}
			TileEntityPowered tileEntityPowered = _world.GetTileEntity(this.tileEntityPosition) as TileEntityPowered;
			EntityPlayer entityPlayer2 = _world.GetEntity(this.entityID) as EntityPlayer;
			if (tileEntityPowered == null)
			{
				Block block = _world.GetBlock(this.tileEntityPosition).Block;
				if (block is BlockPowered)
				{
					tileEntityPowered = (block as BlockPowered).CreateTileEntity(chunk);
				}
				tileEntityPowered.localChunkPos = World.toBlock(this.tileEntityPosition);
				BlockEntityData blockEntity = chunk.GetBlockEntity(this.tileEntityPosition);
				if (blockEntity != null)
				{
					tileEntityPowered.BlockTransform = blockEntity.transform;
				}
				tileEntityPowered.InitializePowerData();
				chunk.AddTileEntity(tileEntityPowered);
			}
			if (tileEntityPowered != null && entityPlayer2 != null)
			{
				Transform transform = entityPlayer2.RootTransform.FindInChilds(entityPlayer2.GetRightHandTransformName(), false);
				if (transform != null)
				{
					ItemActionConnectPower.ConnectPowerData connectPowerData2 = (ItemActionConnectPower.ConnectPowerData)entityPlayer2.inventory.holdingItemData.actionData[1];
					WireNode component = ((GameObject)global::UnityEngine.Object.Instantiate(Resources.Load("Prefabs/WireNode"))).GetComponent<WireNode>();
					component.LocalPosition = tileEntityPowered.ToWorldPos().ToVector3() - Origin.position;
					component.localOffset = tileEntityPowered.GetWireOffset();
					WireNode wireNode = component;
					wireNode.localOffset.x = wireNode.localOffset.x + 0.5f;
					WireNode wireNode2 = component;
					wireNode2.localOffset.y = wireNode2.localOffset.y + 0.5f;
					WireNode wireNode3 = component;
					wireNode3.localOffset.z = wireNode3.localOffset.z + 0.5f;
					component.Source = transform.gameObject;
					component.TogglePulse(false);
					component.SetPulseSpeed(360f);
					connectPowerData2.wireNode = component;
				}
			}
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageWireToolActions>().Setup(this.currentOperation, this.tileEntityPosition, this.entityID), false, -1, this.entityID, -1, null, 192, false);
				return;
			}
		}
	}

	// Token: 0x06003EFD RID: 16125 RVA: 0x00180278 File Offset: 0x0017E478
	public override int GetLength()
	{
		return 12;
	}

	// Token: 0x040032D5 RID: 13013
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i tileEntityPosition;

	// Token: 0x040032D6 RID: 13014
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageWireToolActions.WireActions currentOperation;

	// Token: 0x040032D7 RID: 13015
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityID;

	// Token: 0x0200086E RID: 2158
	public enum WireActions
	{
		// Token: 0x040032D9 RID: 13017
		AddWire,
		// Token: 0x040032DA RID: 13018
		RemoveWire
	}
}
