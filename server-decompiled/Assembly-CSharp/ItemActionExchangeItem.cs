using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005B8 RID: 1464
[Preserve]
public class ItemActionExchangeItem : ItemAction
{
	// Token: 0x06002FE4 RID: 12260 RVA: 0x00131AAC File Offset: 0x0012FCAC
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

	// Token: 0x06002FE5 RID: 12261 RVA: 0x00131BC4 File Offset: 0x0012FDC4
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

	// Token: 0x06002FE6 RID: 12262 RVA: 0x00131C10 File Offset: 0x0012FE10
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

	// Token: 0x06002FE7 RID: 12263 RVA: 0x0012BD98 File Offset: 0x00129F98
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return _actionData.lastUseTime != 0f && Time.time - _actionData.lastUseTime < this.Delay;
	}

	// Token: 0x06002FE8 RID: 12264 RVA: 0x0012C5CA File Offset: 0x0012A7CA
	public override void StopHolding(ItemActionData _data)
	{
		base.StopHolding(_data);
		_data.lastUseTime = 0f;
	}

	// Token: 0x06002FE9 RID: 12265 RVA: 0x00131D0C File Offset: 0x0012FF0C
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

	// Token: 0x040024F7 RID: 9463
	[PublicizedFrom(EAccessModifier.Protected)]
	public string changeItemToItem;

	// Token: 0x040024F8 RID: 9464
	[PublicizedFrom(EAccessModifier.Protected)]
	public string changeBlockTo;

	// Token: 0x040024F9 RID: 9465
	[PublicizedFrom(EAccessModifier.Protected)]
	public string doBlockAction;

	// Token: 0x040024FA RID: 9466
	[PublicizedFrom(EAccessModifier.Protected)]
	public BlockValue hitLiquidBlock;

	// Token: 0x040024FB RID: 9467
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3i hitLiquidPos;

	// Token: 0x040024FC RID: 9468
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<BlockValue> focusedBlocks = new List<BlockValue>();
}
