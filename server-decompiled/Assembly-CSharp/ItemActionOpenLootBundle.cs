using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005BD RID: 1469
[Preserve]
public class ItemActionOpenLootBundle : ItemAction
{
	// Token: 0x06003015 RID: 12309 RVA: 0x00133543 File Offset: 0x00131743
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionOpenLootBundle.MyInventoryData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06003016 RID: 12310 RVA: 0x0013354C File Offset: 0x0013174C
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		if (_props.Values.ContainsKey("Consume"))
		{
			this.Consume = StringParsers.ParseBool(_props.Values["Consume"], 0, -1, true);
		}
		else
		{
			this.Consume = true;
		}
		_props.ParseString("LootList", ref this.lootListName);
		this.UseAnimation = false;
	}

	// Token: 0x06003017 RID: 12311 RVA: 0x001335B1 File Offset: 0x001317B1
	public override void StopHolding(ItemActionData _data)
	{
		base.StopHolding(_data);
		((ItemActionOpenLootBundle.MyInventoryData)_data).bEatingStarted = false;
	}

	// Token: 0x06003018 RID: 12312 RVA: 0x001335C8 File Offset: 0x001317C8
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		ItemActionOpenLootBundle.MyInventoryData myInventoryData = (ItemActionOpenLootBundle.MyInventoryData)_actionData;
		if (!_bReleased)
		{
			return;
		}
		if (Time.time - _actionData.lastUseTime < this.Delay)
		{
			return;
		}
		if (this.IsActionRunning(_actionData))
		{
			return;
		}
		EntityAlive holdingEntity = myInventoryData.invData.holdingEntity;
		if (EffectManager.GetValue(PassiveEffects.DisableItem, holdingEntity.inventory.holdingItemItemValue, 0f, holdingEntity, null, _actionData.invData.item.ItemTags, true, true, true, true, true, 1, true, false) > 0f)
		{
			_actionData.lastUseTime = Time.time + 1f;
			Manager.PlayInsidePlayerHead("twitch_no_attack", -1, 0f, false, false);
			return;
		}
		BlockValue blockValue = BlockValue.Air;
		if (this.ConditionBlockTypes != null)
		{
			Ray ray = holdingEntity.GetLookRay();
			int modelLayer = holdingEntity.GetModelLayer();
			holdingEntity.SetModelLayer(2, false, null);
			Voxel.Raycast(myInventoryData.invData.world, ray, 2.5f, 131, (holdingEntity is EntityPlayer) ? 0.2f : 0.4f);
			holdingEntity.SetModelLayer(modelLayer, false, null);
			WorldRayHitInfo voxelRayHitInfo = Voxel.voxelRayHitInfo;
			if (!GameUtils.IsBlockOrTerrain(voxelRayHitInfo.tag))
			{
				return;
			}
			HitInfoDetails hit = voxelRayHitInfo.hit;
			blockValue = voxelRayHitInfo.hit.blockValue;
			if (blockValue.isair || !this.ConditionBlockTypes.Contains(blockValue.type))
			{
				ray = myInventoryData.invData.holdingEntity.GetLookRay();
				ray.origin += ray.direction.normalized * 0.5f;
				if (!Voxel.Raycast(myInventoryData.invData.world, ray, 2.5f, -538480653, 4095, 0f))
				{
					return;
				}
				HitInfoDetails hit2 = voxelRayHitInfo.hit;
				blockValue = voxelRayHitInfo.hit.blockValue;
				if (blockValue.isair || !this.ConditionBlockTypes.Contains(blockValue.type))
				{
					return;
				}
			}
		}
		_actionData.lastUseTime = Time.time;
		this.ExecuteInstantAction(myInventoryData.invData.holdingEntity, myInventoryData.invData.itemStack, true, null);
	}

	// Token: 0x06003019 RID: 12313 RVA: 0x001337DC File Offset: 0x001319DC
	public override bool ExecuteInstantAction(EntityAlive ent, ItemStack stack, bool isHeldItem, XUiC_ItemStack stackController)
	{
		ent.MinEventContext.ItemValue = stack.itemValue;
		ent.MinEventContext.ItemValue.FireEvent(MinEventTypes.onSelfPrimaryActionStart, ent.MinEventContext);
		ent.FireEvent(MinEventTypes.onSelfPrimaryActionStart, false);
		if (this.soundStart != null)
		{
			ent.PlayOneShot(this.soundStart, false, false, false, null, 1f);
		}
		LootContainer lootContainer = LootContainer.GetLootContainer(this.lootListName, true);
		if (lootContainer == null)
		{
			return false;
		}
		if (this.Consume)
		{
			if (stack.itemValue.MaxUseTimes > 0 && stack.itemValue.UseTimes + 1f < (float)stack.itemValue.MaxUseTimes)
			{
				stack.itemValue.UseTimes += EffectManager.GetValue(PassiveEffects.DegradationPerUse, stack.itemValue, 1f, ent, null, stack.itemValue.ItemClass.ItemTags, true, true, true, true, true, 1, true, false) * ItemAction.ItemDegradationModifier;
				return true;
			}
			if (isHeldItem)
			{
				ent.inventory.DecHoldingItem(1);
			}
			else
			{
				int count = stack.count;
				stack.count = count - 1;
			}
		}
		ent.MinEventContext.ItemValue = stack.itemValue;
		ent.MinEventContext.ItemValue.FireEvent(MinEventTypes.onSelfPrimaryActionEnd, ent.MinEventContext);
		ent.FireEvent(MinEventTypes.onSelfPrimaryActionEnd, false);
		new List<ItemStack>();
		EntityPlayer entityPlayer = ent as EntityPlayer;
		if (entityPlayer != null)
		{
			IList<ItemStack> list = lootContainer.Spawn(ent.rand, 100, (float)entityPlayer.GetHighestPartyLootStage(0f, 0f), 0f, entityPlayer, FastTags<TagGroup.Global>.none, true, false, true);
			for (int i = 0; i < list.Count; i++)
			{
				ItemStack itemStack = list[i].Clone();
				XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(ent as EntityPlayerLocal).xui.PlayerInventory;
				if (itemStack.itemValue.HasQuality)
				{
					itemStack.itemValue.MaxDurabilityModifier = 1f;
				}
				if (!playerInventory.AddItem(itemStack))
				{
					ent.world.gameManager.ItemDropServer(itemStack, ent.GetPosition(), Vector3.zero, -1, 60f, false);
				}
			}
		}
		return true;
	}

	// Token: 0x04002500 RID: 9472
	public new bool Consume;

	// Token: 0x04002501 RID: 9473
	public HashSet<int> ConditionBlockTypes;

	// Token: 0x04002502 RID: 9474
	public string lootListName = "";

	// Token: 0x020005BE RID: 1470
	[PublicizedFrom(EAccessModifier.Private)]
	public class MyInventoryData : ItemActionAttackData
	{
		// Token: 0x0600301B RID: 12315 RVA: 0x0012B224 File Offset: 0x00129424
		public MyInventoryData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x04002503 RID: 9475
		public bool bEatingStarted;
	}
}
