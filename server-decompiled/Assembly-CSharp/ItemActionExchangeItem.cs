using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005B1 RID: 1457
[Preserve]
public class ItemActionExchangeItem : ItemAction
{
	// Token: 0x06002FD6 RID: 12246 RVA: 0x00130EB8 File Offset: 0x0012F0B8
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		if (!_props.Values.ContainsKey("Change_item_to"))
		{
			throw new Exception("Missing attribute 'change_item_to' in use_action 'ExchangeItem'");
		}
		this.changeItemToItem = _props.Values["Change_item_to"];
		if (_props.Values.ContainsKey("Change_block_to"))
		{
			this.changeBlockTo = _props.Values["Change_block_to"];
		}
		if (_props.Values.ContainsKey("Do_block_action"))
		{
			this.doBlockAction = _props.Values["Do_block_action"];
		}
		int num = 1;
		while (_props.Values.ContainsKey("Focused_blockname_" + num.ToString()))
		{
			string text = _props.Values["Focused_blockname_" + num.ToString()];
			BlockValue blockValue = ItemClass.GetItem(text, false).ToBlockValue(false);
			if (blockValue.Equals(BlockValue.Air))
			{
				throw new Exception("Unknown block name '" + text + "' in use_action!");
			}
			this.focusedBlocks.Add(blockValue);
			num++;
		}
	}

	// Token: 0x06002FD7 RID: 12247 RVA: 0x00130FD0 File Offset: 0x0012F1D0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isFocusingBlock(WorldRayHitInfo _hitInfo)
	{
		for (int i = 0; i < this.focusedBlocks.Count; i++)
		{
			BlockValue blockValue = this.focusedBlocks[i];
			if (_hitInfo.hit.blockValue.Equals(blockValue))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002FD8 RID: 12248 RVA: 0x0013101C File Offset: 0x0012F21C
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		if (!_bReleased)
		{
			return;
		}
		if (_actionData.lastUseTime > 0f)
		{
			return;
		}
		ItemInventoryData invData = _actionData.invData;
		Ray lookRay = invData.holdingEntity.GetLookRay();
		lookRay.origin += lookRay.direction.normalized * 0.5f;
		if (!Voxel.Raycast(invData.world, lookRay, Constants.cDigAndBuildDistance, -538480653, 4095, 0f))
		{
			return;
		}
		if (Voxel.voxelRayHitInfo.bHitValid && this.isFocusingBlock(Voxel.voxelRayHitInfo))
		{
			this.hitLiquidBlock = Voxel.voxelRayHitInfo.hit.blockValue;
			this.hitLiquidPos = Voxel.voxelRayHitInfo.hit.blockPos;
			_actionData.lastUseTime = Time.time;
			invData.holdingEntity.RightArmAnimationUse = true;
			if (this.soundStart != null)
			{
				invData.holdingEntity.PlayOneShot(this.soundStart, false, false, false, null, 1f);
			}
		}
	}

	// Token: 0x06002FD9 RID: 12249 RVA: 0x0012B1A4 File Offset: 0x001293A4
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return _actionData.lastUseTime != 0f && Time.time - _actionData.lastUseTime < this.Delay;
	}

	// Token: 0x06002FDA RID: 12250 RVA: 0x0012B9D6 File Offset: 0x00129BD6
	public override void StopHolding(ItemActionData _data)
	{
		base.StopHolding(_data);
		_data.lastUseTime = 0f;
	}

	// Token: 0x06002FDB RID: 12251 RVA: 0x00131118 File Offset: 0x0012F318
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
		if (_actionData.lastUseTime == 0f || this.IsActionRunning(_actionData))
		{
			return;
		}
		QuestEventManager.Current.ExchangedFromItem(_actionData.invData.itemStack);
		ItemValue item = ItemClass.GetItem(this.changeItemToItem, false);
		_actionData.invData.itemStack.Set(item, _actionData.invData.itemStack.count);
		if (this.doBlockAction != null && GameManager.Instance.World.IsWater(this.hitLiquidPos))
		{
			this.hitLiquidBlock.Block.DoExchangeAction(_actionData.invData.world, this.hitLiquidPos, this.hitLiquidBlock, this.doBlockAction, _actionData.invData.holdingEntity.inventory.holdingCount);
		}
		if (this.changeBlockTo != null)
		{
			Vector3i blockPos = ((EntityPlayerLocal)_actionData.invData.holdingEntity).HitInfo.hit.blockPos;
			_actionData.invData.world.GetBlock(blockPos);
			BlockValue blockValue = ItemClass.GetItem(this.changeBlockTo, false).ToBlockValue(false);
			_actionData.invData.world.SetBlockRPC(blockPos, blockValue);
		}
	}

	// Token: 0x040024DC RID: 9436
	[PublicizedFrom(EAccessModifier.Protected)]
	public string changeItemToItem;

	// Token: 0x040024DD RID: 9437
	[PublicizedFrom(EAccessModifier.Protected)]
	public string changeBlockTo;

	// Token: 0x040024DE RID: 9438
	[PublicizedFrom(EAccessModifier.Protected)]
	public string doBlockAction;

	// Token: 0x040024DF RID: 9439
	[PublicizedFrom(EAccessModifier.Protected)]
	public BlockValue hitLiquidBlock;

	// Token: 0x040024E0 RID: 9440
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3i hitLiquidPos;

	// Token: 0x040024E1 RID: 9441
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<BlockValue> focusedBlocks = new List<BlockValue>();
}
