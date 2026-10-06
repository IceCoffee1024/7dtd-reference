using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005C8 RID: 1480
[Preserve]
public class ItemActionRepair : ItemActionAttack
{
	// Token: 0x0600307E RID: 12414 RVA: 0x00137952 File Offset: 0x00135B52
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionRepair.InventoryDataRepair(_invData, _indexInEntityOfAction);
	}

	// Token: 0x0600307F RID: 12415 RVA: 0x0013795C File Offset: 0x00135B5C
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		this.repairAmount = 0f;
		_props.ParseFloat("Repair_amount", ref this.repairAmount);
		this.hitCountOffset = 0f;
		_props.ParseFloat("Upgrade_hit_offset", ref this.hitCountOffset);
		this.repairActionSound = _props.GetString("Repair_action_sound");
		this.upgradeActionSound = _props.GetString("Upgrade_action_sound");
		this.allowedUpgradeItems = _props.GetString("Allowed_upgrade_items");
		this.restrictedUpgradeItems = _props.GetString("Restricted_upgrade_items");
		this.soundAnimActionSyncTimer = 0.3f;
	}

	// Token: 0x06003080 RID: 12416 RVA: 0x001379F8 File Offset: 0x00135BF8
	public override void StopHolding(ItemActionData _data)
	{
		((ItemActionRepair.InventoryDataRepair)_data).bUseStarted = false;
		this.bUpgradeCountChanged = false;
		this.blockUpgradeCount = 0;
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(_data.invData.holdingEntity as EntityPlayerLocal);
		if (uiforPlayer != null)
		{
			XUiC_FocusedBlockHealth.SetData(uiforPlayer, null, 0f);
		}
	}

	// Token: 0x06003081 RID: 12417 RVA: 0x00137A4A File Offset: 0x00135C4A
	public override void StartHolding(ItemActionData _data)
	{
		((ItemActionRepair.InventoryDataRepair)_data).bUseStarted = false;
		this.bUpgradeCountChanged = false;
		this.blockUpgradeCount = 0;
	}

	// Token: 0x06003082 RID: 12418 RVA: 0x00137A68 File Offset: 0x00135C68
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
		EntityPlayerLocal entityPlayerLocal = _actionData.invData.holdingEntity as EntityPlayerLocal;
		if (!entityPlayerLocal)
		{
			return;
		}
		WorldRayHitInfo hitInfo = entityPlayerLocal.HitInfo;
		if (hitInfo.bHitValid && hitInfo.hit.distanceSq > Constants.cDigAndBuildDistance * Constants.cDigAndBuildDistance)
		{
			return;
		}
		GUIWindowManager windowManager = LocalPlayerUI.GetUIForPlayer(entityPlayerLocal).windowManager;
		ItemActionRepair.InventoryDataRepair inventoryDataRepair = (ItemActionRepair.InventoryDataRepair)_actionData;
		if (windowManager.IsModalWindowOpen())
		{
			inventoryDataRepair.bUseStarted = false;
			inventoryDataRepair.repairType = ItemActionRepair.EnumRepairType.None;
			return;
		}
		if (_actionData.invData.holdingEntity != _actionData.invData.world.GetPrimaryPlayer())
		{
			return;
		}
		if (!inventoryDataRepair.bUseStarted)
		{
			return;
		}
		if (this.bUpgradeCountChanged)
		{
			BlockValue block = _actionData.invData.world.GetBlock(this.blockTargetPos);
			Block block2 = block.Block;
			int num;
			if (int.TryParse(block2.Properties.GetString("UpgradeBlock", "UpgradeHitCount"), out num))
			{
				num = (int)(((float)num + this.hitCountOffset < 1f) ? 1f : ((float)num + this.hitCountOffset));
				inventoryDataRepair.upgradePerc = (float)this.blockUpgradeCount / (float)num;
				if (this.blockUpgradeCount >= num)
				{
					if (this.RemoveRequiredResource(_actionData.invData, block))
					{
						BlockValue blockValue = Block.GetBlockValue(block2.Properties.GetString(Block.PropUpgradeBlockClass, Block.PropUpgradeBlockToBlock), false);
						blockValue.rotation = block.rotation;
						blockValue.meta = block.meta;
						QuestEventManager.Current.BlockUpgraded(block2.GetBlockName(), this.blockTargetPos);
						_actionData.invData.holdingEntity.MinEventContext.ItemActionData = _actionData;
						_actionData.invData.holdingEntity.MinEventContext.BlockValue = blockValue;
						_actionData.invData.holdingEntity.MinEventContext.Position = this.blockTargetPos.ToVector3();
						_actionData.invData.holdingEntity.FireEvent(MinEventTypes.onSelfUpgradedBlock, true);
						Block block3 = block.Block;
						block3.DamageBlock(_actionData.invData.world, this.blockTargetPos, block, -1, _actionData.invData.holdingEntity.entityId, null, false, false);
						int num2;
						if (int.TryParse(block2.Properties.GetString(Block.PropUpgradeBlockClass, Block.PropUpgradeBlockItemCount), out num2))
						{
							_actionData.invData.holdingEntity.Progression.AddLevelExp((int)(blockValue.Block.blockMaterial.Experience * (float)num2), "_xpFromUpgradeBlock", Progression.XPTypes.Upgrading, true, true, -1, null);
						}
						if (block3.UpgradeSound != null)
						{
							_actionData.invData.holdingEntity.PlayOneShot(block3.UpgradeSound, false, false, false, null, 1f);
						}
					}
					this.blockUpgradeCount = 0;
				}
			}
			string text = this.upgradeActionSound;
			string upgradeItemName = this.GetUpgradeItemName(block2);
			if (text.Length == 0 && this.item != null && upgradeItemName != null && upgradeItemName.Length > 0)
			{
				text = string.Format("ImpactSurface/{0}hit{1}", _actionData.invData.holdingEntity.inventory.holdingItem.MadeOfMaterial.SurfaceCategory, ItemClass.GetForId(ItemClass.GetItem(upgradeItemName, false).type).MadeOfMaterial.SurfaceCategory);
			}
			if (text.Length > 0)
			{
				_actionData.invData.holdingEntity.PlayOneShot(text, false, false, false, null, 1f);
			}
			this.bUpgradeCountChanged = false;
			return;
		}
		this.ExecuteAction(_actionData, false);
	}

	// Token: 0x06003083 RID: 12419 RVA: 0x00137DCC File Offset: 0x00135FCC
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		EntityPlayerLocal entityPlayerLocal = _actionData.invData.holdingEntity as EntityPlayerLocal;
		LocalPlayerUI.GetUIForPlayer(entityPlayerLocal);
		if (_bReleased)
		{
			((ItemActionRepair.InventoryDataRepair)_actionData).bUseStarted = false;
			((ItemActionRepair.InventoryDataRepair)_actionData).repairType = ItemActionRepair.EnumRepairType.None;
			return;
		}
		if (entityPlayerLocal)
		{
			entityPlayerLocal.StartTPCameraLockTimer();
		}
		if (Time.time - _actionData.lastUseTime < this.Delay)
		{
			return;
		}
		ItemInventoryData invData = _actionData.invData;
		WorldRayHitInfo hitInfo = entityPlayerLocal.HitInfo;
		if (hitInfo.bHitValid && hitInfo.hit.distanceSq > Constants.cDigAndBuildDistance * Constants.cDigAndBuildDistance)
		{
			return;
		}
		if (EffectManager.GetValue(PassiveEffects.DisableItem, entityPlayerLocal.inventory.holdingItemItemValue, 0f, entityPlayerLocal, null, _actionData.invData.item.ItemTags, true, true, true, true, true, 1, true, false) > 0f)
		{
			_actionData.lastUseTime = Time.time + 1f;
			Manager.PlayInsidePlayerHead("twitch_no_attack", -1, 0f, false, false);
			return;
		}
		_actionData.lastUseTime = Time.time;
		if (hitInfo.bHitValid && World.SandboxUseTraderArea == TraderAreaStates.Default && _actionData.invData.world.IsWithinTraderArea(hitInfo.hit.blockPos))
		{
			return;
		}
		if (hitInfo.bHitValid && GameUtils.IsBlockOrTerrain(hitInfo.tag))
		{
			this.blockTargetPos = hitInfo.hit.blockPos;
			BlockValue blockValue = invData.world.GetBlock(this.blockTargetPos);
			if (blockValue.ischild)
			{
				this.blockTargetPos = blockValue.Block.multiBlockPos.GetParentPos(this.blockTargetPos, blockValue);
				blockValue = _actionData.invData.world.GetBlock(this.blockTargetPos);
			}
			if ((invData.itemValue.MaxUseTimes > 0 && invData.itemValue.UseTimes >= (float)invData.itemValue.MaxUseTimes) || (invData.itemValue.UseTimes == 0f && invData.itemValue.MaxUseTimes == 0))
			{
				base.HandleJamSound(_actionData.invData.itemValue, this.item, entityPlayerLocal);
				return;
			}
			ItemActionRepair.InventoryDataRepair inventoryDataRepair = (ItemActionRepair.InventoryDataRepair)_actionData;
			Block block = blockValue.Block;
			if (block.CanRepair(blockValue))
			{
				int num = Utils.FastMin((int)this.repairAmount, blockValue.damage);
				float num2 = (float)num / (float)block.MaxDamage;
				List<Block.SItemNameCount> list = block.RepairItems;
				if (block.RepairItemsMeshDamage != null && block.shape.UseRepairDamageState(blockValue))
				{
					num = 1;
					num2 = 1f;
					list = block.RepairItemsMeshDamage;
				}
				if (list == null)
				{
					return;
				}
				if (inventoryDataRepair.lastHitPosition != this.blockTargetPos || inventoryDataRepair.lastHitBlockValue.type != blockValue.type || inventoryDataRepair.lastRepairItems != list)
				{
					inventoryDataRepair.lastHitPosition = this.blockTargetPos;
					inventoryDataRepair.lastHitBlockValue = blockValue;
					inventoryDataRepair.lastRepairItems = list;
					inventoryDataRepair.lastRepairItemsPercents = new float[list.Count];
				}
				inventoryDataRepair.blockDamagePerc = (float)blockValue.damage / (float)block.MaxDamage;
				EntityPlayerLocal entityPlayerLocal2 = inventoryDataRepair.invData.holdingEntity as EntityPlayerLocal;
				if (entityPlayerLocal2 == null)
				{
					return;
				}
				inventoryDataRepair.repairType = ItemActionRepair.EnumRepairType.Repair;
				float resourceScale = block.ResourceScale;
				bool flag = false;
				for (int i = 0; i < list.Count; i++)
				{
					string itemName = list[i].ItemName;
					float num3 = (float)list[i].Count * num2 * resourceScale;
					if (inventoryDataRepair.lastRepairItemsPercents[i] <= 0f)
					{
						int num4 = Utils.FastMax((int)num3, 1);
						ItemStack itemStack = new ItemStack(ItemClass.GetItem(itemName, false), num4);
						if (!this.canRemoveRequiredItem(inventoryDataRepair.invData, itemStack))
						{
							itemStack.count = 0;
							entityPlayerLocal2.AddUIHarvestingItem(itemStack, true);
							if (!flag)
							{
								flag = true;
							}
						}
					}
				}
				if (flag)
				{
					return;
				}
				inventoryDataRepair.invData.holdingEntity.RightArmAnimationUse = true;
				float num5 = 0f;
				for (int j = 0; j < list.Count; j++)
				{
					float num6 = (float)list[j].Count * num2 * resourceScale;
					if (inventoryDataRepair.lastRepairItemsPercents[j] <= 0f)
					{
						string itemName2 = list[j].ItemName;
						int num7 = Utils.FastMax((int)num6, 1);
						inventoryDataRepair.lastRepairItemsPercents[j] += (float)num7;
						inventoryDataRepair.lastRepairItemsPercents[j] -= num6;
						ItemStack itemStack2 = new ItemStack(ItemClass.GetItem(itemName2, false), num7);
						num5 += itemStack2.itemValue.ItemClass.MadeOfMaterial.Experience * (float)num7;
						this.removeRequiredItem(inventoryDataRepair.invData, itemStack2);
						itemStack2.count *= -1;
						entityPlayerLocal2.AddUIHarvestingItem(itemStack2, false);
					}
					else
					{
						inventoryDataRepair.lastRepairItemsPercents[j] -= num6;
					}
				}
				if (this.repairActionSound != null && this.repairActionSound.Length > 0)
				{
					invData.holdingEntity.PlayOneShot(this.repairActionSound, false, false, false, null, 1f);
				}
				else if (this.soundStart != null && this.soundStart.Length > 0)
				{
					invData.holdingEntity.PlayOneShot(this.soundStart, false, false, false, null, 1f);
				}
				if (invData.itemValue.MaxUseTimes > 0)
				{
					ItemValue itemValue = invData.itemValue;
					float useTimes = itemValue.UseTimes;
					itemValue.UseTimes = useTimes + 1f;
				}
				int num8 = blockValue.Block.DamageBlock(invData.world, this.blockTargetPos, blockValue, -num, invData.holdingEntity.entityId, null, false, false);
				inventoryDataRepair.bUseStarted = true;
				inventoryDataRepair.blockDamagePerc = (float)num8 / (float)blockValue.Block.MaxDamage;
				inventoryDataRepair.invData.holdingEntity.MinEventContext.ItemActionData = inventoryDataRepair;
				inventoryDataRepair.invData.holdingEntity.MinEventContext.BlockValue = blockValue;
				inventoryDataRepair.invData.holdingEntity.MinEventContext.Position = this.blockTargetPos.ToVector3();
				inventoryDataRepair.invData.holdingEntity.FireEvent(MinEventTypes.onSelfRepairBlock, true);
				entityPlayerLocal2.Progression.AddLevelExp((int)num5, "_xpFromRepairBlock", Progression.XPTypes.Repairing, true, true, -1, null);
				return;
			}
			else if (this.isUpgradeItem)
			{
				if (!this.CanRemoveRequiredResource(_actionData.invData, blockValue))
				{
					string upgradeItemName = this.GetUpgradeItemName(blockValue.Block);
					if (upgradeItemName != null)
					{
						ItemStack itemStack3 = new ItemStack(ItemClass.GetItem(upgradeItemName, false), 0);
						(_actionData.invData.holdingEntity as EntityPlayerLocal).AddUIHarvestingItem(itemStack3, true);
					}
					inventoryDataRepair.upgradePerc = 0f;
					return;
				}
				_actionData.invData.holdingEntity.RightArmAnimationUse = true;
				inventoryDataRepair.repairType = ItemActionRepair.EnumRepairType.Upgrade;
				if (this.blockTargetPos == this.lastBlockTargetPos)
				{
					this.blockUpgradeCount++;
				}
				else
				{
					this.blockUpgradeCount = 1;
				}
				this.lastBlockTargetPos = this.blockTargetPos;
				this.bUpgradeCountChanged = true;
				inventoryDataRepair.bUseStarted = true;
				return;
			}
			else
			{
				inventoryDataRepair.bUseStarted = false;
				inventoryDataRepair.repairType = ItemActionRepair.EnumRepairType.None;
			}
		}
	}

	// Token: 0x06003084 RID: 12420 RVA: 0x001384D3 File Offset: 0x001366D3
	public float GetRepairAmount()
	{
		return this.repairAmount;
	}

	// Token: 0x06003085 RID: 12421 RVA: 0x001384DC File Offset: 0x001366DC
	[PublicizedFrom(EAccessModifier.Private)]
	public string GetUpgradeItemName(Block block)
	{
		string text = block.Properties.GetString("UpgradeBlock", "Item");
		if (text != null && text.Length == 1 && text[0] == 'r')
		{
			text = block.RepairItems[0].ItemName;
		}
		return text;
	}

	// Token: 0x06003086 RID: 12422 RVA: 0x0013852C File Offset: 0x0013672C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CanRemoveRequiredResource(ItemInventoryData data, BlockValue blockValue)
	{
		Block block = blockValue.Block;
		string upgradeItemName = this.GetUpgradeItemName(block);
		bool flag = upgradeItemName != null && upgradeItemName.Length > 0;
		if (flag)
		{
			if (this.allowedUpgradeItems.Length > 0 && !this.allowedUpgradeItems.ContainsCaseInsensitive(upgradeItemName))
			{
				return false;
			}
			if (this.restrictedUpgradeItems.Length > 0 && this.restrictedUpgradeItems.ContainsCaseInsensitive(upgradeItemName))
			{
				return false;
			}
		}
		int num;
		if (!int.TryParse(block.Properties.GetString("UpgradeBlock", "UpgradeHitCount"), out num))
		{
			return false;
		}
		int num2;
		if (!int.TryParse(block.Properties.GetString(Block.PropUpgradeBlockClass, Block.PropUpgradeBlockItemCount), out num2) && flag)
		{
			return false;
		}
		if (block.GetBlockName() != null && flag)
		{
			ItemValue item = ItemClass.GetItem(upgradeItemName, false);
			if (data.holdingEntity.inventory.GetItemCount(item, false, -1, -1, true) >= num2)
			{
				return true;
			}
			if (data.holdingEntity.bag.GetItemCount(item, -1, -1, true) >= num2)
			{
				return true;
			}
		}
		else if (!flag)
		{
			return true;
		}
		return false;
	}

	// Token: 0x06003087 RID: 12423 RVA: 0x0013862C File Offset: 0x0013682C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool RemoveRequiredResource(ItemInventoryData data, BlockValue blockValue)
	{
		if (!this.CanRemoveRequiredResource(data, blockValue))
		{
			return false;
		}
		Block block = blockValue.Block;
		ItemValue item = ItemClass.GetItem(this.GetUpgradeItemName(block), false);
		int num;
		if (!int.TryParse(block.Properties.GetString(Block.PropUpgradeBlockClass, Block.PropUpgradeBlockItemCount), out num))
		{
			return false;
		}
		if (data.holdingEntity.inventory.DecItem(item, num, false, null) == num)
		{
			EntityPlayerLocal entityPlayerLocal = data.holdingEntity as EntityPlayerLocal;
			if (entityPlayerLocal != null && num != 0)
			{
				entityPlayerLocal.AddUIHarvestingItem(new ItemStack(item, -num), false);
			}
			return true;
		}
		if (data.holdingEntity.bag.DecItem(item, num, false, null) == num)
		{
			EntityPlayerLocal entityPlayerLocal2 = data.holdingEntity as EntityPlayerLocal;
			if (entityPlayerLocal2 != null)
			{
				entityPlayerLocal2.AddUIHarvestingItem(new ItemStack(item, -num), false);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06003088 RID: 12424 RVA: 0x001386FC File Offset: 0x001368FC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool canRemoveRequiredItem(ItemInventoryData _data, ItemStack _itemStack)
	{
		return _data.holdingEntity.inventory.GetItemCount(_itemStack.itemValue, false, -1, -1, true) >= _itemStack.count || _data.holdingEntity.bag.GetItemCount(_itemStack.itemValue, -1, -1, true) >= _itemStack.count;
	}

	// Token: 0x06003089 RID: 12425 RVA: 0x00138754 File Offset: 0x00136954
	[PublicizedFrom(EAccessModifier.Private)]
	public bool removeRequiredItem(ItemInventoryData _data, ItemStack _itemStack)
	{
		return _data.holdingEntity.inventory.DecItem(_itemStack.itemValue, _itemStack.count, false, null) == _itemStack.count || _data.holdingEntity.bag.DecItem(_itemStack.itemValue, _itemStack.count, false, null) == _itemStack.count;
	}

	// Token: 0x0600308A RID: 12426 RVA: 0x001387B4 File Offset: 0x001369B4
	public override ItemClass.EnumCrosshairType GetCrosshairType(ItemActionData _actionData)
	{
		EntityPlayerLocal entityPlayerLocal = _actionData.invData.holdingEntity as EntityPlayerLocal;
		if (entityPlayerLocal != null && !entityPlayerLocal.TPCameraCheckPassed)
		{
			return ItemClass.EnumCrosshairType.None;
		}
		ItemActionRepair.EnumRepairType repairType = ((ItemActionRepair.InventoryDataRepair)_actionData).repairType;
		if (repairType == ItemActionRepair.EnumRepairType.Repair)
		{
			return ItemClass.EnumCrosshairType.Repair;
		}
		if (repairType != ItemActionRepair.EnumRepairType.Upgrade)
		{
			return ItemClass.EnumCrosshairType.Plus;
		}
		return ItemClass.EnumCrosshairType.Upgrade;
	}

	// Token: 0x0600308B RID: 12427 RVA: 0x001387FC File Offset: 0x001369FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isShowOverlay(ItemActionData _actionData)
	{
		WorldRayHitInfo hitInfo = ((EntityPlayerLocal)_actionData.invData.holdingEntity).HitInfo;
		if (hitInfo.bHitValid && hitInfo.hit.distanceSq > Constants.cDigAndBuildDistance * Constants.cDigAndBuildDistance)
		{
			return false;
		}
		bool flag = false;
		ItemActionRepair.InventoryDataRepair inventoryDataRepair = (ItemActionRepair.InventoryDataRepair)_actionData;
		if (inventoryDataRepair.repairType == ItemActionRepair.EnumRepairType.None)
		{
			if (hitInfo.bHitValid)
			{
				int num;
				if (!hitInfo.hit.blockValue.ischild)
				{
					num = hitInfo.hit.blockValue.damage;
				}
				else
				{
					Vector3i parentPos = hitInfo.hit.blockValue.Block.multiBlockPos.GetParentPos(hitInfo.hit.blockPos, hitInfo.hit.blockValue);
					num = _actionData.invData.world.GetBlock(parentPos).damage;
				}
				flag = num > 0;
			}
		}
		else if (inventoryDataRepair.repairType == ItemActionRepair.EnumRepairType.Repair)
		{
			EntityPlayerLocal entityPlayerLocal = _actionData.invData.holdingEntity as EntityPlayerLocal;
			flag = entityPlayerLocal != null && entityPlayerLocal.HitInfo.bHitValid && Time.time - _actionData.lastUseTime <= 1.5f;
		}
		else if (inventoryDataRepair.repairType == ItemActionRepair.EnumRepairType.Upgrade)
		{
			EntityPlayerLocal entityPlayerLocal2 = _actionData.invData.holdingEntity as EntityPlayerLocal;
			flag = entityPlayerLocal2 != null && entityPlayerLocal2.HitInfo.bHitValid && Time.time - _actionData.lastUseTime <= 1.5f && inventoryDataRepair.upgradePerc > 0f;
		}
		return flag;
	}

	// Token: 0x0600308C RID: 12428 RVA: 0x00138988 File Offset: 0x00136B88
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void getOverlayData(ItemActionData _actionData, out float _perc, out string _text)
	{
		ItemActionRepair.InventoryDataRepair inventoryDataRepair = (ItemActionRepair.InventoryDataRepair)_actionData;
		WorldRayHitInfo hitInfo = ((EntityPlayerLocal)_actionData.invData.holdingEntity).HitInfo;
		if (inventoryDataRepair.repairType == ItemActionRepair.EnumRepairType.None)
		{
			BlockValue blockValue = hitInfo.hit.blockValue;
			if (blockValue.ischild)
			{
				Vector3i parentPos = blockValue.Block.multiBlockPos.GetParentPos(hitInfo.hit.blockPos, blockValue);
				blockValue = _actionData.invData.world.GetBlock(parentPos);
			}
			int shownMaxDamage = blockValue.Block.GetShownMaxDamage();
			_perc = ((float)shownMaxDamage - (float)blockValue.damage) / (float)shownMaxDamage;
			_text = string.Format("{0}/{1}", Utils.FastMax(0, shownMaxDamage - blockValue.damage), shownMaxDamage);
			return;
		}
		if (inventoryDataRepair.repairType == ItemActionRepair.EnumRepairType.Repair)
		{
			_perc = 1f - inventoryDataRepair.blockDamagePerc;
			_text = string.Format("{0}%", (_perc * 100f).ToCultureInvariantString("0"));
			return;
		}
		if (inventoryDataRepair.repairType == ItemActionRepair.EnumRepairType.Upgrade)
		{
			_perc = inventoryDataRepair.upgradePerc;
			_text = string.Format("{0}%", (_perc * 100f).ToCultureInvariantString("0"));
			return;
		}
		_perc = 0f;
		_text = string.Empty;
	}

	// Token: 0x0600308D RID: 12429 RVA: 0x00138ABC File Offset: 0x00136CBC
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		ItemActionRepair.InventoryDataRepair inventoryDataRepair = (ItemActionRepair.InventoryDataRepair)_actionData;
		return Time.time - inventoryDataRepair.lastUseTime < this.Delay + 0.1f;
	}

	// Token: 0x0600308E RID: 12430 RVA: 0x00138AED File Offset: 0x00136CED
	public override void GetItemValueActionInfo(ref List<string> _infoList, ItemValue _itemValue, XUi _xui, int _actionIndex = 0)
	{
		base.GetItemValueActionInfo(ref _infoList, _itemValue, _xui, _actionIndex);
		_infoList.Add(ItemAction.StringFormatHandler(Localization.Get("lblBlkRpr", false, null), this.GetRepairAmount().ToCultureInvariantString()));
	}

	// Token: 0x04002566 RID: 9574
	[PublicizedFrom(EAccessModifier.Protected)]
	public BlockValue targetBlock;

	// Token: 0x04002567 RID: 9575
	[PublicizedFrom(EAccessModifier.Protected)]
	public float repairAmount;

	// Token: 0x04002568 RID: 9576
	[PublicizedFrom(EAccessModifier.Protected)]
	public float hitCountOffset;

	// Token: 0x04002569 RID: 9577
	[PublicizedFrom(EAccessModifier.Protected)]
	public float soundAnimActionSyncTimer;

	// Token: 0x0400256A RID: 9578
	[PublicizedFrom(EAccessModifier.Protected)]
	public const float SOUND_LENGTH = 0.3f;

	// Token: 0x0400256B RID: 9579
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isUpgradeItem = true;

	// Token: 0x0400256C RID: 9580
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockTargetPos;

	// Token: 0x0400256D RID: 9581
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i lastBlockTargetPos;

	// Token: 0x0400256E RID: 9582
	[PublicizedFrom(EAccessModifier.Private)]
	public int blockUpgradeCount;

	// Token: 0x0400256F RID: 9583
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bUpgradeCountChanged;

	// Token: 0x04002570 RID: 9584
	[PublicizedFrom(EAccessModifier.Private)]
	public string repairActionSound;

	// Token: 0x04002571 RID: 9585
	[PublicizedFrom(EAccessModifier.Private)]
	public string upgradeActionSound;

	// Token: 0x04002572 RID: 9586
	[PublicizedFrom(EAccessModifier.Private)]
	public string allowedUpgradeItems;

	// Token: 0x04002573 RID: 9587
	[PublicizedFrom(EAccessModifier.Private)]
	public string restrictedUpgradeItems;

	// Token: 0x020005C9 RID: 1481
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum EnumRepairType
	{
		// Token: 0x04002575 RID: 9589
		None,
		// Token: 0x04002576 RID: 9590
		Repair,
		// Token: 0x04002577 RID: 9591
		Upgrade
	}

	// Token: 0x020005CA RID: 1482
	[PublicizedFrom(EAccessModifier.Protected)]
	public class InventoryDataRepair : ItemActionAttackData
	{
		// Token: 0x06003090 RID: 12432 RVA: 0x00138B2C File Offset: 0x00136D2C
		public InventoryDataRepair(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x04002578 RID: 9592
		public new bool uiOpenedByMe;

		// Token: 0x04002579 RID: 9593
		public ItemActionRepair.EnumRepairType repairType;

		// Token: 0x0400257A RID: 9594
		public float blockDamagePerc;

		// Token: 0x0400257B RID: 9595
		public bool bUseStarted;

		// Token: 0x0400257C RID: 9596
		public float upgradePerc;

		// Token: 0x0400257D RID: 9597
		public BlockValue lastHitBlockValue;

		// Token: 0x0400257E RID: 9598
		public Vector3i lastHitPosition = Vector3i.zero;

		// Token: 0x0400257F RID: 9599
		public List<Block.SItemNameCount> lastRepairItems;

		// Token: 0x04002580 RID: 9600
		public float[] lastRepairItemsPercents;
	}
}
