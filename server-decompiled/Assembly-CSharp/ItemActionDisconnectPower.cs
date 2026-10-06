using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005AA RID: 1450
[Preserve]
public class ItemActionDisconnectPower : ItemAction
{
	// Token: 0x06002FB2 RID: 12210 RVA: 0x0012FAE5 File Offset: 0x0012DCE5
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionDisconnectPower.MyInventoryData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002FB3 RID: 12211 RVA: 0x0012FAEE File Offset: 0x0012DCEE
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
	}

	// Token: 0x06002FB4 RID: 12212 RVA: 0x0012FAF7 File Offset: 0x0012DCF7
	public override void StopHolding(ItemActionData _data)
	{
		base.StopHolding(_data);
		((ItemActionDisconnectPower.MyInventoryData)_data).StartDisconnect = false;
	}

	// Token: 0x06002FB5 RID: 12213 RVA: 0x0012FB0C File Offset: 0x0012DD0C
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		if (!_bReleased)
		{
			return;
		}
		if (Time.time - _actionData.lastUseTime < this.Delay)
		{
			return;
		}
		_actionData.lastUseTime = Time.time;
		((ItemActionDisconnectPower.MyInventoryData)_actionData).StartDisconnect = true;
	}

	// Token: 0x06002FB6 RID: 12214 RVA: 0x0012FB40 File Offset: 0x0012DD40
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		ItemActionDisconnectPower.MyInventoryData myInventoryData = (ItemActionDisconnectPower.MyInventoryData)_actionData;
		return myInventoryData.StartDisconnect && Time.time - myInventoryData.lastUseTime < 2f * AnimationDelayData.AnimationDelay[myInventoryData.invData.item.HoldType.Value].RayCast;
	}

	// Token: 0x06002FB7 RID: 12215 RVA: 0x0012FB98 File Offset: 0x0012DD98
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
		ItemActionDisconnectPower.MyInventoryData myInventoryData = (ItemActionDisconnectPower.MyInventoryData)_actionData;
		if (!myInventoryData.StartDisconnect || Time.time - myInventoryData.lastUseTime < AnimationDelayData.AnimationDelay[myInventoryData.invData.item.HoldType.Value].RayCast)
		{
			return;
		}
		myInventoryData.StartDisconnect = false;
		ItemActionDisconnectPower.MyInventoryData myInventoryData2 = (ItemActionDisconnectPower.MyInventoryData)_actionData;
		ItemInventoryData invData = _actionData.invData;
		WorldRayHitInfo hitInfo = ((EntityPlayerLocal)invData.holdingEntity).HitInfo;
		Vector3i lastBlockPos = hitInfo.lastBlockPos;
		Vector3i blockPos = hitInfo.hit.blockPos;
		if (!hitInfo.bHitValid || hitInfo.tag.StartsWith("E_"))
		{
			return;
		}
		if (((ItemActionConnectPower)_actionData.invData.holdingEntity.inventory.holdingItem.Actions[1]).DisconnectWire((ItemActionConnectPower.ConnectPowerData)_actionData.invData.holdingEntity.inventory.holdingItemData.actionData[1]))
		{
			return;
		}
		if (!myInventoryData.invData.world.CanPlaceBlockAt(blockPos, myInventoryData.invData.world.gameManager.GetPersistentLocalPlayer(), false))
		{
			return;
		}
		IPowered poweredBlock = this.GetPoweredBlock(invData);
		if (poweredBlock == null)
		{
			((ItemActionConnectPower)_actionData.invData.holdingEntity.inventory.holdingItem.Actions[1]).DisconnectWire((ItemActionConnectPower.ConnectPowerData)_actionData.invData.holdingEntity.inventory.holdingItemData.actionData[1]);
			return;
		}
		if (myInventoryData.invData.itemValue.MaxUseTimes > 0 && myInventoryData.invData.itemValue.UseTimes >= (float)myInventoryData.invData.itemValue.MaxUseTimes)
		{
			EntityPlayerLocal entityPlayerLocal = _actionData.invData.holdingEntity as EntityPlayerLocal;
			base.HandleJamSound(_actionData.invData.itemValue, this.item, entityPlayerLocal);
			return;
		}
		if (myInventoryData.invData.itemValue.MaxUseTimes > 0)
		{
			_actionData.invData.itemValue.UseTimes += EffectManager.GetValue(PassiveEffects.DegradationPerUse, _actionData.invData.itemValue, 1f, invData.holdingEntity, null, _actionData.invData.itemValue.ItemClass.ItemTags, true, true, true, true, true, 1, true, false) * ItemAction.ItemDegradationModifier;
			base.HandleItemBreak(_actionData);
		}
		_actionData.invData.holdingEntity.RightArmAnimationAttack = true;
		poweredBlock.RemoveParentWithWiringTool(_actionData.invData.holdingEntity.entityId);
	}

	// Token: 0x06002FB8 RID: 12216 RVA: 0x0012FE0C File Offset: 0x0012E00C
	[PublicizedFrom(EAccessModifier.Private)]
	public IPowered GetPoweredBlock(ItemInventoryData data)
	{
		WorldRayHitInfo hitInfo = ((EntityPlayerLocal)data.holdingEntity).HitInfo;
		Block block = data.world.GetBlock(hitInfo.hit.blockPos).Block;
		if (!(block is BlockPowered) && !(block is BlockPowerSource))
		{
			return null;
		}
		Vector3i blockPos = hitInfo.hit.blockPos;
		ChunkCluster chunkCache = data.world.ChunkCache;
		if (chunkCache == null)
		{
			return null;
		}
		Chunk chunk = (Chunk)chunkCache.GetChunkSync(World.toChunkXZ(blockPos.x), blockPos.y, World.toChunkXZ(blockPos.z));
		if (chunk == null)
		{
			return null;
		}
		TileEntity tileEntity = chunk.GetTileEntity(World.toBlock(blockPos));
		if (tileEntity == null)
		{
			if (block is BlockPowered)
			{
				tileEntity = (block as BlockPowered).CreateTileEntity(chunk);
			}
			else if (block is BlockPowerSource)
			{
				tileEntity = (block as BlockPowerSource).CreateTileEntity(chunk);
			}
			tileEntity.localChunkPos = World.toBlock(blockPos);
			BlockEntityData blockEntity = chunk.GetBlockEntity(blockPos);
			if (blockEntity != null)
			{
				((TileEntityPowered)tileEntity).BlockTransform = blockEntity.transform;
			}
			((TileEntityPowered)tileEntity).InitializePowerData();
			chunk.AddTileEntity(tileEntity);
		}
		return tileEntity as IPowered;
	}

	// Token: 0x020005AB RID: 1451
	[PublicizedFrom(EAccessModifier.Private)]
	public class MyInventoryData : ItemActionAttackData
	{
		// Token: 0x06002FBA RID: 12218 RVA: 0x0012B224 File Offset: 0x00129424
		public MyInventoryData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x040024CA RID: 9418
		public bool StartDisconnect;
	}
}
