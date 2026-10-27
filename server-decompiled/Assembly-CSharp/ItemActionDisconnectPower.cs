using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005B1 RID: 1457
[Preserve]
public class ItemActionDisconnectPower : ItemAction
{
	// Token: 0x06002FC0 RID: 12224 RVA: 0x001306D9 File Offset: 0x0012E8D9
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionDisconnectPower.MyInventoryData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002FC1 RID: 12225 RVA: 0x001306E2 File Offset: 0x0012E8E2
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
	}

	// Token: 0x06002FC2 RID: 12226 RVA: 0x001306EB File Offset: 0x0012E8EB
	public override void StopHolding(ItemActionData _data)
	{
		base.StopHolding(_data);
		((ItemActionDisconnectPower.MyInventoryData)_data).StartDisconnect = false;
	}

	// Token: 0x06002FC3 RID: 12227 RVA: 0x00130700 File Offset: 0x0012E900
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

	// Token: 0x06002FC4 RID: 12228 RVA: 0x00130734 File Offset: 0x0012E934
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		ItemActionDisconnectPower.MyInventoryData myInventoryData = (ItemActionDisconnectPower.MyInventoryData)_actionData;
		return myInventoryData.StartDisconnect && Time.time - myInventoryData.lastUseTime < 2f * AnimationDelayData.AnimationDelay[myInventoryData.invData.item.HoldType.Value].RayCast;
	}

	// Token: 0x06002FC5 RID: 12229 RVA: 0x0013078C File Offset: 0x0012E98C
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

	// Token: 0x06002FC6 RID: 12230 RVA: 0x00130A00 File Offset: 0x0012EC00
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

	// Token: 0x020005B2 RID: 1458
	[PublicizedFrom(EAccessModifier.Private)]
	public class MyInventoryData : ItemActionAttackData
	{
		// Token: 0x06002FC8 RID: 12232 RVA: 0x0012BE18 File Offset: 0x0012A018
		public MyInventoryData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x040024E5 RID: 9445
		public bool StartDisconnect;
	}
}
