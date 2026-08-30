using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005A4 RID: 1444
[Preserve]
public class ItemActionDisconnectPower : ItemAction
{
	// Token: 0x06002F41 RID: 12097 RVA: 0x0012DFA5 File Offset: 0x0012C1A5
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionDisconnectPower.MyInventoryData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002F42 RID: 12098 RVA: 0x0012DFAE File Offset: 0x0012C1AE
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
	}

	// Token: 0x06002F43 RID: 12099 RVA: 0x0012DFB7 File Offset: 0x0012C1B7
	public override void StopHolding(ItemActionData _data)
	{
		base.StopHolding(_data);
		((ItemActionDisconnectPower.MyInventoryData)_data).StartDisconnect = false;
	}

	// Token: 0x06002F44 RID: 12100 RVA: 0x0012DFCC File Offset: 0x0012C1CC
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

	// Token: 0x06002F45 RID: 12101 RVA: 0x0012E000 File Offset: 0x0012C200
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		ItemActionDisconnectPower.MyInventoryData myInventoryData = (ItemActionDisconnectPower.MyInventoryData)_actionData;
		return myInventoryData.StartDisconnect && Time.time - myInventoryData.lastUseTime < 2f * AnimationDelayData.AnimationDelay[myInventoryData.invData.item.HoldType.Value].RayCast;
	}

	// Token: 0x06002F46 RID: 12102 RVA: 0x0012E058 File Offset: 0x0012C258
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

	// Token: 0x06002F47 RID: 12103 RVA: 0x0012E2CC File Offset: 0x0012C4CC
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

	// Token: 0x020005A5 RID: 1445
	[PublicizedFrom(EAccessModifier.Private)]
	public class MyInventoryData : ItemActionAttackData
	{
		// Token: 0x06002F49 RID: 12105 RVA: 0x00129694 File Offset: 0x00127894
		public MyInventoryData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x0400249D RID: 9373
		public bool StartDisconnect;
	}
}
