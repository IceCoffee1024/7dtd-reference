using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Audio;
using UnityEngine;

// Token: 0x0200056E RID: 1390
public class Inventory : InventoryBase
{
	// Token: 0x170004CC RID: 1228
	// (get) Token: 0x06002D1C RID: 11548 RVA: 0x000811EE File Offset: 0x0007F3EE
	public int PUBLIC_SLOTS_PLAYMODE
	{
		get
		{
			return 10;
		}
	}

	// Token: 0x170004CD RID: 1229
	// (get) Token: 0x06002D1D RID: 11549 RVA: 0x0011FBA0 File Offset: 0x0011DDA0
	public int PUBLIC_SLOTS_PREFABEDITOR
	{
		get
		{
			return 2 * this.PUBLIC_SLOTS_PLAYMODE;
		}
	}

	// Token: 0x170004CE RID: 1230
	// (get) Token: 0x06002D1E RID: 11550 RVA: 0x000811EE File Offset: 0x0007F3EE
	public int SHIFT_KEY_SLOT_OFFSET
	{
		get
		{
			return 10;
		}
	}

	// Token: 0x170004CF RID: 1231
	// (get) Token: 0x06002D1F RID: 11551 RVA: 0x0011FBAA File Offset: 0x0011DDAA
	public int PUBLIC_SLOTS
	{
		get
		{
			if (PrefabEditModeManager.Instance == null || !PrefabEditModeManager.Instance.IsActive())
			{
				return this.PUBLIC_SLOTS_PLAYMODE;
			}
			return this.PUBLIC_SLOTS_PREFABEDITOR;
		}
	}

	// Token: 0x170004D0 RID: 1232
	// (get) Token: 0x06002D20 RID: 11552 RVA: 0x0011FBCC File Offset: 0x0011DDCC
	public int INVENTORY_SLOTS
	{
		get
		{
			return this.PUBLIC_SLOTS + 1;
		}
	}

	// Token: 0x170004D1 RID: 1233
	// (get) Token: 0x06002D21 RID: 11553 RVA: 0x0011FBD6 File Offset: 0x0011DDD6
	public int DUMMY_SLOT_IDX
	{
		get
		{
			return this.INVENTORY_SLOTS - 1;
		}
	}

	// Token: 0x14000037 RID: 55
	// (add) Token: 0x06002D22 RID: 11554 RVA: 0x0011FBE0 File Offset: 0x0011DDE0
	// (remove) Token: 0x06002D23 RID: 11555 RVA: 0x0011FC18 File Offset: 0x0011DE18
	public event XUiEvent_ToolbeltItemsChangedInternal OnToolbeltItemsChangedInternal;

	// Token: 0x06002D24 RID: 11556 RVA: 0x0011FC50 File Offset: 0x0011DE50
	public Inventory(IGameManager _gameManager, EntityAlive _entity)
	{
		this.m_LastDrawnHoldingItemIndex = this.DUMMY_SLOT_IDX;
		this.m_HoldingItemIdx = this.DUMMY_SLOT_IDX;
		this.preferredItemSlots = new int[this.PUBLIC_SLOTS];
		this.models = new Transform[this.INVENTORY_SLOTS];
		this.slots = new ItemInventoryData[this.INVENTORY_SLOTS];
		this.entity = _entity;
		this.gameManager = _gameManager;
		this.emptyItem = new ItemInventoryData(null, ItemStack.Empty, _gameManager, _entity, 0);
		this.Clear();
		this.m_HoldingItemIdx = 0;
		this.m_LastDrawnHoldingItemIndex = -1;
		this.previousHeldItemValue = null;
		this.previousHeldItemSlotIdx = -1;
	}

	// Token: 0x06002D25 RID: 11557 RVA: 0x0011FD1C File Offset: 0x0011DF1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitInactiveItemsObject()
	{
		if (!this.inactiveItems)
		{
			GameObject gameObject = new GameObject("InactiveItems");
			gameObject.SetActive(false);
			this.inactiveItems = gameObject.transform;
			this.inactiveItems.SetParent(this.entity.transform, false);
		}
	}

	// Token: 0x06002D26 RID: 11558 RVA: 0x0011FD6B File Offset: 0x0011DF6B
	[PublicizedFrom(EAccessModifier.Private)]
	public void onInventoryChanged()
	{
		if (this.OnToolbeltItemsChangedInternal != null)
		{
			this.OnToolbeltItemsChangedInternal();
		}
	}

	// Token: 0x06002D27 RID: 11559 RVA: 0x0011FD80 File Offset: 0x0011DF80
	public void CallOnToolbeltChangedInternal()
	{
		this.onInventoryChanged();
	}

	// Token: 0x06002D28 RID: 11560 RVA: 0x0011FD88 File Offset: 0x0011DF88
	public bool UsingBareHand()
	{
		return this.bareHandItem == this.holdingItem;
	}

	// Token: 0x06002D29 RID: 11561 RVA: 0x0011FD98 File Offset: 0x0011DF98
	public virtual ItemClass GetBareHandItem()
	{
		return this.bareHandItem;
	}

	// Token: 0x06002D2A RID: 11562 RVA: 0x0011FDA0 File Offset: 0x0011DFA0
	public virtual ItemValue GetBareHandItemValue()
	{
		return this.bareHandItemValue;
	}

	// Token: 0x06002D2B RID: 11563 RVA: 0x0011FDA8 File Offset: 0x0011DFA8
	public virtual void SetBareHandItem(ItemValue _bareHandItemValue)
	{
		this.bareHandItemValue = _bareHandItemValue;
		this.bareHandItem = ItemClass.GetForId(this.bareHandItemValue.type);
		this.bareHandItemInventoryData = this.bareHandItem.CreateInventoryData(new ItemStack(_bareHandItemValue, 1), this.gameManager, this.entity, 0);
	}

	// Token: 0x06002D2C RID: 11564 RVA: 0x0011FDF8 File Offset: 0x0011DFF8
	public virtual void SetSlots(ItemStack[] _slots, bool _allowSettingDummySlot = true)
	{
		int num = 0;
		while (num < _slots.Length && num < (_allowSettingDummySlot ? this.INVENTORY_SLOTS : this.PUBLIC_SLOTS))
		{
			this.SetItem(num, _slots[num].itemValue, _slots[num].count, false);
			num++;
		}
		this.notifyListeners();
	}

	// Token: 0x06002D2D RID: 11565 RVA: 0x0011FE45 File Offset: 0x0011E045
	public virtual ItemActionData GetItemActionDataInSlot(int _slotIdx, int _actionIdx)
	{
		if (_slotIdx == this.holdingItemIdx)
		{
			return this.holdingItemData.actionData[_actionIdx];
		}
		return this.slots[_slotIdx].actionData[_actionIdx];
	}

	// Token: 0x06002D2E RID: 11566 RVA: 0x0011FE78 File Offset: 0x0011E078
	public virtual ItemAction GetItemActionInSlot(int _slotIdx, int _actionIdx)
	{
		if (_slotIdx == this.holdingItemIdx)
		{
			return this.holdingItem.Actions[_actionIdx];
		}
		if (this.slots[_slotIdx] == null || this.slots[_slotIdx].item == null)
		{
			return null;
		}
		return this.slots[_slotIdx].item.Actions[_actionIdx];
	}

	// Token: 0x06002D2F RID: 11567 RVA: 0x0011FECB File Offset: 0x0011E0CB
	public virtual ItemClass GetItemInSlot(int _idx)
	{
		if (this.slots[_idx].item == null)
		{
			return this.bareHandItem;
		}
		return this.slots[_idx].item;
	}

	// Token: 0x06002D30 RID: 11568 RVA: 0x0011FEF0 File Offset: 0x0011E0F0
	public ItemInventoryData GetItemDataInSlot(int _idx)
	{
		if (this.slots[_idx].item == null)
		{
			return this.bareHandItemInventoryData;
		}
		return this.slots[_idx];
	}

	// Token: 0x170004D2 RID: 1234
	public virtual ItemValue this[int _idx]
	{
		get
		{
			return this.slots[_idx].itemStack.itemValue;
		}
		set
		{
			this.slots[_idx].itemStack.itemValue = value;
			this.notifyListeners();
		}
	}

	// Token: 0x06002D33 RID: 11571 RVA: 0x0011FF3F File Offset: 0x0011E13F
	public ItemStack GetItemStack(int _idx)
	{
		return this.slots[_idx].itemStack;
	}

	// Token: 0x06002D34 RID: 11572 RVA: 0x0011FF50 File Offset: 0x0011E150
	public void ModifyValue(ItemValue _originalItemValue, PassiveEffects _passiveEffect, ref float _base_val, ref float _perc_val, FastTags<TagGroup.Global> tags)
	{
		if (this.holdingItemItemValue != null && !this.holdingItemItemValue.Equals(_originalItemValue) && !this.holdingItemItemValue.ItemClass.ItemTags.Test_AnySet(this.ignoreWhenHeld))
		{
			this.holdingItemItemValue.ModifyValue(this.entity, _originalItemValue, _passiveEffect, ref _base_val, ref _perc_val, tags, true, false);
		}
	}

	// Token: 0x06002D35 RID: 11573 RVA: 0x0011FFAC File Offset: 0x0011E1AC
	public void OnUpdate()
	{
		if (!this.isSwitchingHeldItem && !this.entity.IsDead() && this.entity.IsSpawned())
		{
			this.holdingItem.OnHoldingUpdate(this.holdingItemData);
			ItemValue itemValue = this.holdingItemData.itemValue;
			this.entity.MinEventContext.ItemValue = itemValue;
			itemValue.FireEvent(MinEventTypes.onSelfEquipUpdate, this.entity.MinEventContext);
		}
		if (this.entity is EntityPlayer)
		{
			if (this.holdingCount <= 0)
			{
				this.clearSlotByIndex(this.m_HoldingItemIdx);
			}
			if (this.entity.emodel && this.entity.emodel.avatarController)
			{
				this.entity.emodel.avatarController.CancelEvent(AvatarController.itemHasChangedTriggerHash);
			}
		}
	}

	// Token: 0x06002D36 RID: 11574 RVA: 0x00120084 File Offset: 0x0011E284
	public void ShowHeldItemOld(bool show, float waitTime = 0.015f)
	{
		if (show)
		{
			this.entity.MinEventContext.ItemValue = this.holdingItemItemValue;
			EntityPlayerLocal entityPlayerLocal = this.entity as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				entityPlayerLocal.HolsterWeapon(false);
			}
		}
		this.HoldingItemHasChanged();
		GameManager.Instance.StartCoroutine(this.delayedShowHideHeldItem(show, waitTime));
	}

	// Token: 0x06002D37 RID: 11575 RVA: 0x001200DF File Offset: 0x0011E2DF
	public void ShowHeldItem(float waitTime = 0.015f, bool hideFirst = false)
	{
		if (this.delayedShowHideHeldItemRoutine != null)
		{
			GameManager.Instance.StopCoroutine(this.delayedShowHideHeldItemRoutine);
			this.delayedShowHideHeldItemRoutine = null;
		}
		this.delayedShowHideHeldItemRoutine = GameManager.Instance.StartCoroutine(this.delayedShowHideHeldItem(hideFirst, waitTime));
	}

	// Token: 0x06002D38 RID: 11576 RVA: 0x00120118 File Offset: 0x0011E318
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator delayedShowHideHeldItem(bool hideFirst, float waitTime)
	{
		EntityPlayerLocal epl = this.entity as EntityPlayerLocal;
		if (epl != null)
		{
			while (epl.emodel.avatarController.IsAnimationPlayerFPRevivePlaying())
			{
				yield return new WaitForSeconds(0.1f);
			}
			if (hideFirst)
			{
				epl.HolsterWeapon(true);
				this.entity.StopOneShot(this.holdingItem.SoundUnholster);
				this.entity.PlayOneShot(this.holdingItem.SoundHolster, false, false, false, null, 1f);
			}
			if (waitTime < 0.15f)
			{
				waitTime = 0.15f;
			}
			yield return new WaitForSeconds(waitTime);
			this.entity.MinEventContext.ItemValue = this.holdingItemItemValue;
			this.HoldingItemHasChanged();
			this.entity.StopOneShot(this.holdingItem.SoundHolster);
			this.entity.PlayOneShot(this.holdingItem.SoundUnholster, false, false, false, null, 1f);
			if (this.entity.emodel && this.entity.emodel.avatarController)
			{
				this.entity.emodel.avatarController.TriggerEvent(AvatarController.itemHasChangedTriggerHash);
			}
			this.updateHoldingItem();
			epl.HolsterWeapon(false);
			if (epl.bFirstPersonView)
			{
				epl.ShowHoldingItemLayer(true);
			}
			this.ShowRightHand(true);
		}
		else
		{
			this.updateHoldingItem();
		}
		this.SetIsFinishedSwitchingHeldItem();
		yield return new WaitForSeconds(Mathf.Max(waitTime, 0.3f));
		if (this.previousHeldItemSlotIdx != this.holdingItemIdx && this.holdingItemIdx != this.DUMMY_SLOT_IDX && !this.holdingItem.Equals(this.bareHandItem) && !this.slots[this.holdingItemIdx].itemStack.IsEmpty())
		{
			this.quickSwapItemValue = this.previousHeldItemValue;
			this.quickSwapSlotIdx = this.previousHeldItemSlotIdx;
			this.previousHeldItemValue = this.holdingItemItemValue;
			this.previousHeldItemSlotIdx = this.holdingItemIdx;
		}
		yield break;
	}

	// Token: 0x06002D39 RID: 11577 RVA: 0x00120138 File Offset: 0x0011E338
	public void ShowRightHand(bool _show)
	{
		if (this.entity.emodel && this.entity.emodel.avatarController)
		{
			Transform rightHandTransform = this.entity.emodel.avatarController.GetRightHandTransform();
			if (rightHandTransform)
			{
				rightHandTransform.gameObject.SetActive(_show);
			}
		}
	}

	// Token: 0x06002D3A RID: 11578 RVA: 0x00120198 File Offset: 0x0011E398
	public void SetRightHandAsModel()
	{
		if (this.entity.emodel && this.entity.emodel.avatarController)
		{
			this.models[this.m_HoldingItemIdx] = this.entity.emodel.avatarController.GetRightHandTransform();
		}
	}

	// Token: 0x170004D3 RID: 1235
	// (get) Token: 0x06002D3B RID: 11579 RVA: 0x001201F0 File Offset: 0x0011E3F0
	public virtual ItemValue holdingItemItemValue
	{
		get
		{
			ItemValue itemValue = this.slots[this.m_HoldingItemIdx].itemStack.itemValue;
			if (!itemValue.IsEmpty())
			{
				return itemValue;
			}
			return this.bareHandItemValue;
		}
	}

	// Token: 0x170004D4 RID: 1236
	// (get) Token: 0x06002D3C RID: 11580 RVA: 0x00120228 File Offset: 0x0011E428
	public virtual ItemStack holdingItemStack
	{
		get
		{
			ItemStack itemStack = this.slots[this.m_HoldingItemIdx].itemStack;
			if (!itemStack.IsEmpty())
			{
				return itemStack;
			}
			return new ItemStack(this.bareHandItemValue, 0);
		}
	}

	// Token: 0x170004D5 RID: 1237
	// (get) Token: 0x06002D3D RID: 11581 RVA: 0x00120260 File Offset: 0x0011E460
	public virtual ItemClass holdingItem
	{
		get
		{
			ItemValue itemValue = this.slots[this.m_HoldingItemIdx].itemStack.itemValue;
			if (!itemValue.IsEmpty() && ItemClass.list != null)
			{
				return ItemClass.GetForId(itemValue.type);
			}
			return this.bareHandItem;
		}
	}

	// Token: 0x170004D6 RID: 1238
	// (get) Token: 0x06002D3E RID: 11582 RVA: 0x001202A8 File Offset: 0x0011E4A8
	public virtual int holdingCount
	{
		get
		{
			ItemStack itemStack = this.slots[this.m_HoldingItemIdx].itemStack;
			if (!itemStack.itemValue.IsEmpty())
			{
				return itemStack.count;
			}
			return 0;
		}
	}

	// Token: 0x170004D7 RID: 1239
	// (get) Token: 0x06002D3F RID: 11583 RVA: 0x001202E0 File Offset: 0x0011E4E0
	public virtual ItemInventoryData holdingItemData
	{
		get
		{
			if (!this.slots[this.m_HoldingItemIdx].itemStack.itemValue.IsEmpty())
			{
				return this.slots[this.m_HoldingItemIdx];
			}
			this.bareHandItemInventoryData.slotIdx = this.holdingItemIdx;
			return this.bareHandItemInventoryData;
		}
	}

	// Token: 0x170004D8 RID: 1240
	// (get) Token: 0x06002D40 RID: 11584 RVA: 0x00120330 File Offset: 0x0011E530
	public virtual int holdingItemIdx
	{
		get
		{
			return this.m_HoldingItemIdx;
		}
	}

	// Token: 0x06002D41 RID: 11585 RVA: 0x00120338 File Offset: 0x0011E538
	public virtual bool IsHolsterDelayActive()
	{
		return this.isSwitchingHeldItem;
	}

	// Token: 0x06002D42 RID: 11586 RVA: 0x00120338 File Offset: 0x0011E538
	public virtual bool IsUnholsterDelayActive()
	{
		return this.isSwitchingHeldItem;
	}

	// Token: 0x06002D43 RID: 11587 RVA: 0x00120340 File Offset: 0x0011E540
	public void SetIsFinishedSwitchingHeldItem()
	{
		this.isSwitchingHeldItem = false;
		this.entity.MinEventContext.Self = this.entity;
		this.entity.MinEventContext.ItemValue = this.holdingItemItemValue;
		this.HoldingItemHasChanged();
	}

	// Token: 0x06002D44 RID: 11588 RVA: 0x0012037C File Offset: 0x0011E57C
	[PublicizedFrom(EAccessModifier.Private)]
	public void syncHeldItem()
	{
		this.entity.IsEquipping = true;
		if (this.holdingItemItemValue == null || this.holdingItemItemValue.IsEmpty())
		{
			return;
		}
		if (this.holdingItemItemValue.ItemClass == null)
		{
			if (ItemClass.list == null)
			{
				Log.Out("[Inventory:syncHeldItem] Cannot find item class for held item id '{0}'. Item list is null!", new object[] { this.holdingItemItemValue.type });
			}
			else
			{
				Log.Out("[Inventory:syncHeldItem] Cannot find item class for held item id '{0}'. Item id is out of range! ItemClass list length '{1}'", new object[]
				{
					this.holdingItemItemValue.type,
					ItemClass.list.Length
				});
			}
		}
		else if (!this.holdingItemItemValue.ItemClass.ItemTags.Test_AnySet(this.ignoreWhenHeld))
		{
			this.entity.MinEventContext.ItemValue = this.holdingItemItemValue;
			if (this.holdingItemItemValue.ItemClass != null && this.holdingItemItemValue.ItemClass.HasTrigger(MinEventTypes.onSelfItemActivate))
			{
				if (this.holdingItemItemValue.Activated)
				{
					this.holdingItemItemValue.FireEvent(MinEventTypes.onSelfItemActivate, this.entity.MinEventContext);
				}
				else
				{
					this.holdingItemItemValue.FireEvent(MinEventTypes.onSelfItemDeactivate, this.entity.MinEventContext);
				}
			}
			if (this.holdingItemItemValue.Modifications.Length != 0)
			{
				ItemValue itemValue = this.entity.MinEventContext.ItemValue;
				for (int i = 0; i < this.holdingItemItemValue.Modifications.Length; i++)
				{
					ItemValue itemValue2 = this.holdingItemItemValue.Modifications[i];
					if (itemValue2 != null && itemValue2.ItemClass != null)
					{
						ItemClass itemClass = itemValue2.ItemClass;
						this.entity.MinEventContext.ItemValue = itemValue2;
						if (itemClass.HasTrigger(MinEventTypes.onSelfItemActivate))
						{
							if (itemValue2.Activated)
							{
								itemValue2.FireEvent(MinEventTypes.onSelfItemActivate, this.entity.MinEventContext);
							}
							else
							{
								itemValue2.FireEvent(MinEventTypes.onSelfItemDeactivate, this.entity.MinEventContext);
							}
						}
					}
				}
				this.entity.MinEventContext.ItemValue = itemValue;
			}
		}
		this.CallOnToolbeltChangedInternal();
		this.entity.IsEquipping = false;
	}

	// Token: 0x06002D45 RID: 11589 RVA: 0x0012057A File Offset: 0x0011E77A
	public bool GetIsFinishedSwitchingHeldItem()
	{
		return !this.isSwitchingHeldItem;
	}

	// Token: 0x06002D46 RID: 11590 RVA: 0x00120585 File Offset: 0x0011E785
	public virtual int GetFocusedItemIdx()
	{
		return this.m_FocusedItemIdx;
	}

	// Token: 0x06002D47 RID: 11591 RVA: 0x0012058D File Offset: 0x0011E78D
	public virtual int SetFocusedItemIdx(int _idx)
	{
		while (_idx < 0)
		{
			_idx += this.PUBLIC_SLOTS;
		}
		while (_idx >= this.PUBLIC_SLOTS)
		{
			_idx -= this.PUBLIC_SLOTS;
		}
		this.m_FocusedItemIdx = _idx;
		return _idx;
	}

	// Token: 0x06002D48 RID: 11592 RVA: 0x001205BC File Offset: 0x0011E7BC
	public virtual bool IsHoldingItemActionRunning()
	{
		return this.holdingItem.IsActionRunning(this.holdingItemData);
	}

	// Token: 0x06002D49 RID: 11593 RVA: 0x001205CF File Offset: 0x0011E7CF
	public virtual void SetHoldingItemIdxNoHolsterTime(int _inventoryIdx)
	{
		this.setHeldItemByIndex(_inventoryIdx, false);
	}

	// Token: 0x06002D4A RID: 11594 RVA: 0x001205D9 File Offset: 0x0011E7D9
	public virtual void SetHoldingItemIdx(int _inventoryIdx)
	{
		this.setHeldItemByIndex(_inventoryIdx, true);
	}

	// Token: 0x06002D4B RID: 11595 RVA: 0x001205E3 File Offset: 0x0011E7E3
	public void BeginSwapHoldingItem()
	{
		this.isSwitchingHeldItem = true;
	}

	// Token: 0x06002D4C RID: 11596 RVA: 0x001205EC File Offset: 0x0011E7EC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void setHeldItemByIndex(int _inventoryIdx, bool _applyHolsterTime)
	{
		this.BeginSwapHoldingItem();
		while (_inventoryIdx < 0)
		{
			_inventoryIdx += this.slots.Length;
		}
		while (_inventoryIdx >= this.slots.Length)
		{
			_inventoryIdx -= this.slots.Length;
		}
		bool flag = this.flashlightOn && this.IsHoldingFlashlight;
		this.HoldingItemHasChanged();
		if (this.entity != null && this.entity.emodel != null && this.entity.emodel.avatarController != null)
		{
			this.entity.emodel.avatarController.TriggerEvent(AvatarController.itemHasChangedTriggerHash);
		}
		for (int i = 0; i < this.holdingItem.Actions.Length; i++)
		{
			if (this.holdingItem.Actions[i] is ItemActionAttack)
			{
				Manager.BroadcastStop(this.entity.entityId, this.holdingItem.Actions[i].GetSoundStart());
			}
		}
		this.m_HoldingItemIdx = _inventoryIdx;
		this.m_FocusedItemIdx = _inventoryIdx;
		if (this.entity.isEntityRemote)
		{
			this.updateHoldingItem();
			return;
		}
		this.ShowHeldItem((!_applyHolsterTime) ? 0f : 0.2f, true);
		if (flag)
		{
			bool flag2 = this.SetFlashlight(false);
			this.currActiveItemIndex = -1;
			if (flag2)
			{
				this.entity.PlayOneShot("flashlight_toggle", false, false, false, null, 1f);
			}
		}
	}

	// Token: 0x06002D4D RID: 11597 RVA: 0x00120748 File Offset: 0x0011E948
	public void HoldingItemHasChanged()
	{
		if (this.entity != null && this.entity.emodel != null && this.entity.emodel.avatarController != null)
		{
			this.entity.emodel.avatarController.CancelEvent("WeaponFire");
			this.entity.emodel.avatarController.CancelEvent("PowerAttack");
			this.entity.emodel.avatarController.CancelEvent("UseItem");
			this.entity.emodel.avatarController.CancelEvent("ItemUse");
			this.entity.emodel.avatarController.UpdateBool("Reload", false, true);
		}
	}

	// Token: 0x06002D4E RID: 11598 RVA: 0x0012081B File Offset: 0x0011EA1B
	public virtual bool IsHoldingGun()
	{
		return this.holdingItem != null && this.holdingItem.IsGun();
	}

	// Token: 0x06002D4F RID: 11599 RVA: 0x00120832 File Offset: 0x0011EA32
	public virtual bool IsHoldingDynamicMelee()
	{
		return this.holdingItem != null && this.holdingItem.IsDynamicMelee();
	}

	// Token: 0x06002D50 RID: 11600 RVA: 0x00120849 File Offset: 0x0011EA49
	public virtual bool IsHoldingBlock()
	{
		return this.holdingItem != null && this.holdingItem.IsBlock();
	}

	// Token: 0x06002D51 RID: 11601 RVA: 0x00120860 File Offset: 0x0011EA60
	public virtual ItemAction GetHoldingPrimary()
	{
		return this.holdingItem.Actions[0];
	}

	// Token: 0x06002D52 RID: 11602 RVA: 0x0012086F File Offset: 0x0011EA6F
	public virtual ItemAction GetHoldingSecondary()
	{
		return this.holdingItem.Actions[1];
	}

	// Token: 0x06002D53 RID: 11603 RVA: 0x0012087E File Offset: 0x0011EA7E
	public virtual ItemActionAttack GetHoldingGun()
	{
		return this.holdingItem.Actions[0] as ItemActionAttack;
	}

	// Token: 0x06002D54 RID: 11604 RVA: 0x00120892 File Offset: 0x0011EA92
	public virtual ItemActionDynamic GetHoldingDynamicMelee()
	{
		return this.holdingItem.Actions[0] as ItemActionDynamic;
	}

	// Token: 0x06002D55 RID: 11605 RVA: 0x001208A6 File Offset: 0x0011EAA6
	public virtual ItemClassBlock GetHoldingBlock()
	{
		return this.holdingItem as ItemClassBlock;
	}

	// Token: 0x06002D56 RID: 11606 RVA: 0x001208B3 File Offset: 0x0011EAB3
	public Transform GetHoldingItemTransform()
	{
		return this.models[this.m_HoldingItemIdx];
	}

	// Token: 0x06002D57 RID: 11607 RVA: 0x001208C4 File Offset: 0x0011EAC4
	public virtual int GetItemCount(ItemValue _itemValue, bool _bConsiderTexture = false, int _seed = -1, int _meta = -1, bool _ignoreModdedItems = true)
	{
		if (this.slots == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < this.slots.Length; i++)
		{
			if ((!_ignoreModdedItems || !this.slots[i].itemValue.HasModSlots || !this.slots[i].itemValue.HasMods()) && this.slots[i].itemStack.itemValue.type == _itemValue.type && (!_bConsiderTexture || this.slots[i].itemStack.itemValue.TextureFullArray == _itemValue.TextureFullArray) && (_seed == -1 || _seed == (int)this.slots[i].itemValue.Seed) && (_meta == -1 || _meta == this.slots[i].itemValue.Meta))
			{
				num += this.slots[i].itemStack.count;
			}
		}
		return num;
	}

	// Token: 0x06002D58 RID: 11608 RVA: 0x001209B8 File Offset: 0x0011EBB8
	public virtual int GetItemCount(FastTags<TagGroup.Global> itemTags, int _seed = -1, int _meta = -1, bool _ignoreModdedItems = true)
	{
		if (this.slots == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < this.slots.Length; i++)
		{
			if ((!_ignoreModdedItems || !this.slots[i].itemValue.HasModSlots || !this.slots[i].itemValue.HasMods()) && !this.slots[i].itemValue.IsEmpty() && this.slots[i].itemValue.ItemClass.ItemTags.Test_AnySet(itemTags) && (_seed == -1 || _seed == (int)this.slots[i].itemValue.Seed) && (_meta == -1 || _meta == this.slots[i].itemValue.Meta))
			{
				num += this.slots[i].itemStack.count;
			}
		}
		return num;
	}

	// Token: 0x06002D59 RID: 11609 RVA: 0x00120A94 File Offset: 0x0011EC94
	public override bool AddItem(ItemStack _itemStack)
	{
		int num;
		return this.AddItem(_itemStack, out num);
	}

	// Token: 0x06002D5A RID: 11610 RVA: 0x00120AAC File Offset: 0x0011ECAC
	public bool AddItem(ItemStack _itemStack, out int _slot)
	{
		if (!_itemStack.CanMoveTo(XUiC_ItemStack.StackLocationTypes.ToolBelt, -1))
		{
			_slot = -1;
			return false;
		}
		for (int i = 0; i < this.slots.Length - 1; i++)
		{
			if (this.slots[i].itemStack.itemValue.type == _itemStack.itemValue.type && this.slots[i].itemStack.CanStackWith(_itemStack, false))
			{
				this.slots[i].itemStack.count += _itemStack.count;
				this.notifyListeners();
				this.entity.bPlayerStatsChanged = !this.entity.isEntityRemote;
				_slot = i;
				return true;
			}
		}
		for (int j = 0; j < this.slots.Length - 1; j++)
		{
			if (this.slots[j].itemStack.IsEmpty())
			{
				this.SetItem(j, _itemStack.itemValue, _itemStack.count, true);
				this.notifyListeners();
				this.entity.bPlayerStatsChanged = !this.entity.isEntityRemote;
				_slot = j;
				return true;
			}
		}
		_slot = -1;
		return false;
	}

	// Token: 0x06002D5B RID: 11611 RVA: 0x00120BC8 File Offset: 0x0011EDC8
	public virtual bool ReturnItem(ItemStack _itemStack)
	{
		for (int i = 0; i < this.PUBLIC_SLOTS; i++)
		{
			i = this.PreferredItemSlot(_itemStack.itemValue.type, i);
			if (i < 0 || i >= this.PUBLIC_SLOTS)
			{
				return false;
			}
			if (this.AddItemAtSlot(_itemStack, i))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002D5C RID: 11612 RVA: 0x00120C18 File Offset: 0x0011EE18
	public bool AddItemAtSlot(ItemStack _itemStack, int _slot)
	{
		bool flag = false;
		if (_slot >= 0 && _slot < this.PUBLIC_SLOTS)
		{
			if (this.slots[_slot].itemStack.itemValue.type == _itemStack.itemValue.type && this.slots[_slot].itemStack.CanStackWith(_itemStack, false))
			{
				this.slots[_slot].itemStack.count += _itemStack.count;
				flag = true;
			}
			if (this.slots[_slot].itemStack.IsEmpty() && this.CanMoveToSlot(_itemStack, _slot))
			{
				this.SetItem(_slot, _itemStack.itemValue, _itemStack.count, true);
				flag = true;
			}
		}
		if (flag)
		{
			this.notifyListeners();
			this.entity.bPlayerStatsChanged = !this.entity.isEntityRemote;
			if (_slot == this.m_HoldingItemIdx)
			{
				this.HoldingItemHasChanged();
			}
		}
		return flag;
	}

	// Token: 0x06002D5D RID: 11613 RVA: 0x00120CFC File Offset: 0x0011EEFC
	public virtual int DecItem(ItemValue _itemValue, int _count, bool _ignoreModdedItems = false, IList<ItemStack> _removedItems = null)
	{
		int num = _count;
		int num2 = 0;
		while (_count > 0 && num2 < this.slots.Length - 1)
		{
			if (this.slots[num2].itemStack.itemValue.type == _itemValue.type && (!_ignoreModdedItems || !this.slots[num2].itemValue.HasModSlots || !this.slots[num2].itemValue.HasMods()))
			{
				if (ItemClass.GetForId(this.slots[num2].itemStack.itemValue.type).CanStack())
				{
					int count = this.slots[num2].itemStack.count;
					int num3 = ((count >= _count) ? _count : count);
					if (_removedItems != null)
					{
						_removedItems.Add(new ItemStack(this.slots[num2].itemStack.itemValue.Clone(), num3));
					}
					this.slots[num2].itemStack.count -= num3;
					_count -= num3;
					if (this.slots[num2].itemStack.count <= 0)
					{
						this.clearSlotByIndex(num2);
					}
				}
				else
				{
					if (_removedItems != null)
					{
						_removedItems.Add(this.slots[num2].itemStack.Clone());
					}
					this.clearSlotByIndex(num2);
					_count--;
				}
			}
			num2++;
		}
		this.notifyListeners();
		return num - _count;
	}

	// Token: 0x06002D5E RID: 11614 RVA: 0x00120E54 File Offset: 0x0011F054
	[PublicizedFrom(EAccessModifier.Private)]
	public void clearSlotByIndex(int _idx)
	{
		if (!this.slots[_idx].itemStack.itemValue.IsEmpty())
		{
			this.slots[_idx].itemStack = ItemStack.Empty;
		}
		Transform transform = this.models[_idx];
		if (transform)
		{
			this.HoldingItemHasChanged();
			transform.SetParent(null, false);
			transform.gameObject.SetActive(false);
			global::UnityEngine.Object.Destroy(transform.gameObject);
			this.models[_idx] = null;
		}
	}

	// Token: 0x06002D5F RID: 11615 RVA: 0x00120ECC File Offset: 0x0011F0CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual Transform createHeldItem(int _idx, ItemValue _itemValue)
	{
		this.InitInactiveItemsObject();
		Transform transform = _itemValue.ItemClass.CloneModel(this.entity.world, _itemValue, this.entity.GetPosition(), this.inactiveItems, BlockShape.MeshPurpose.Hold, default(TextureFullArray));
		if (transform != null)
		{
			transform.gameObject.SetActive(false);
		}
		return transform;
	}

	// Token: 0x06002D60 RID: 11616 RVA: 0x00120F28 File Offset: 0x0011F128
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual ItemInventoryData createInventoryData(int _idx, ItemValue _itemValue)
	{
		return ItemClass.GetForId(_itemValue.type).CreateInventoryData(ItemStack.Empty, this.gameManager, this.entity, _idx);
	}

	// Token: 0x06002D61 RID: 11617 RVA: 0x00120F4C File Offset: 0x0011F14C
	public virtual void SetItem(int _idx, ItemStack _itemStack)
	{
		this.SetItem(_idx, _itemStack.itemValue, _itemStack.count, true);
	}

	// Token: 0x06002D62 RID: 11618 RVA: 0x00120F64 File Offset: 0x0011F164
	public virtual void SetItem(int _idx, ItemValue _itemValue, int _count, bool _notifyListeners = true)
	{
		if (_idx == this.holdingItemIdx && !_itemValue.EqualsExceptUseTimesAndAmmo(this.slots[_idx].itemStack.itemValue))
		{
			this.ShowHeldItem(0.2f, true);
		}
		if ((ulong)_idx >= (ulong)((long)this.slots.Length))
		{
			return;
		}
		if (_itemValue.type != 0 && _itemValue.ItemClass == null)
		{
			Log.Warning("Inventory slot {0} {1} missing item class", new object[] { _idx, _itemValue.type });
			_itemValue.Clear();
		}
		bool flag = false;
		if (_idx < this.preferredItemSlots.Length)
		{
			if (_itemValue.type != 0 && _count != 0)
			{
				this.preferredItemSlots[_idx] = _itemValue.type;
			}
			else if (this.slots[_idx].itemStack.itemValue.type != 0)
			{
				this.preferredItemSlots[_idx] = this.slots[_idx].itemStack.itemValue.type;
			}
		}
		if (_count == 0)
		{
			_itemValue.Clear();
		}
		ItemClass itemClass = this.slots[_idx].itemStack.itemValue.ItemClass;
		ItemClass itemClass2 = _itemValue.ItemClass;
		if (itemClass == null || itemClass != itemClass2)
		{
			this.clearSlotByIndex(_idx);
			if (_itemValue.ItemClass != null)
			{
				this.models[_idx] = (_itemValue.ItemClass.CanHold() ? this.createHeldItem(_idx, _itemValue) : null);
				this.slots[_idx] = this.createInventoryData(_idx, _itemValue);
			}
			flag = true;
		}
		this.slots[_idx].itemStack.itemValue = _itemValue.Clone();
		this.slots[_idx].itemStack.count = _count;
		if (flag && _idx == this.holdingItemIdx)
		{
			this.updateHoldingItem();
		}
		if (_notifyListeners)
		{
			this.notifyListeners();
		}
	}

	// Token: 0x06002D63 RID: 11619 RVA: 0x00121104 File Offset: 0x0011F304
	public void FireEvent(MinEventTypes _eventType, MinEventParams _eventParms)
	{
		ItemValue itemValue = _eventParms.ItemValue;
		if (itemValue == null)
		{
			itemValue = this.holdingItemItemValue;
		}
		itemValue.FireEvent(_eventType, _eventParms);
	}

	// Token: 0x06002D64 RID: 11620 RVA: 0x0011FF3F File Offset: 0x0011E13F
	public virtual ItemStack GetItem(int _idx)
	{
		return this.slots[_idx].itemStack;
	}

	// Token: 0x06002D65 RID: 11621 RVA: 0x0012112A File Offset: 0x0011F32A
	public virtual int GetItemCount()
	{
		return this.slots.Length;
	}

	// Token: 0x06002D66 RID: 11622 RVA: 0x00121134 File Offset: 0x0011F334
	public virtual bool DecHoldingItem(int _count)
	{
		bool flag = true;
		if (ItemClass.GetForId(this.holdingItemItemValue.type).CanStack())
		{
			this.slots[this.m_HoldingItemIdx].itemStack.count -= _count;
			flag = this.slots[this.m_HoldingItemIdx].itemStack.count <= 0;
		}
		if (flag)
		{
			this.HandleTurningOffHoldingFlashlight();
			this.clearSlotByIndex(this.m_HoldingItemIdx);
		}
		this.updateHoldingItem();
		this.notifyListeners();
		return true;
	}

	// Token: 0x170004D9 RID: 1241
	// (get) Token: 0x06002D67 RID: 11623 RVA: 0x001211B9 File Offset: 0x0011F3B9
	public bool IsFlashlightOn
	{
		get
		{
			return this.flashlightOn;
		}
	}

	// Token: 0x06002D68 RID: 11624 RVA: 0x001211C1 File Offset: 0x0011F3C1
	public bool IsAnItemActive()
	{
		return this.currActiveItemIndex != -1;
	}

	// Token: 0x06002D69 RID: 11625 RVA: 0x001211CF File Offset: 0x0011F3CF
	public void SetActiveItemIndexOff()
	{
		this.currActiveItemIndex = -1;
	}

	// Token: 0x06002D6A RID: 11626 RVA: 0x001211D8 File Offset: 0x0011F3D8
	public void ResetActiveIndex()
	{
		this.currActiveItemIndex = -2;
	}

	// Token: 0x06002D6B RID: 11627 RVA: 0x00020021 File Offset: 0x0001E221
	public bool CycleActivatableItems()
	{
		return true;
	}

	// Token: 0x06002D6C RID: 11628 RVA: 0x000027FC File Offset: 0x000009FC
	public void HandleTurningOffHoldingFlashlight()
	{
	}

	// Token: 0x06002D6D RID: 11629 RVA: 0x000027FC File Offset: 0x000009FC
	public void TurnOffLightFlares()
	{
	}

	// Token: 0x06002D6E RID: 11630 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool SetFlashlight(bool on)
	{
		return false;
	}

	// Token: 0x170004DA RID: 1242
	// (get) Token: 0x06002D6F RID: 11631 RVA: 0x001211E4 File Offset: 0x0011F3E4
	public virtual bool IsHoldingFlashlight
	{
		get
		{
			if (this.holdingItem.IsLightSource())
			{
				ItemClass forId = ItemClass.GetForId(this.holdingItemItemValue.type);
				Transform transform = this.models[this.m_HoldingItemIdx].gameObject.transform.Find(forId.ActivateObject.Value);
				if (transform == null && this.models[this.m_HoldingItemIdx].gameObject.name.Equals(forId.ActivateObject.Value))
				{
					transform = this.models[this.m_HoldingItemIdx].gameObject.transform;
				}
				return transform != null && transform.parent.gameObject.activeInHierarchy;
			}
			if (this.holdingItemItemValue.HasQuality)
			{
				for (int i = 0; i < this.holdingItemItemValue.Modifications.Length; i++)
				{
					if (this.holdingItemItemValue.Modifications[i] != null)
					{
						ItemClass itemClass = this.holdingItemItemValue.Modifications[i].ItemClass;
						if (itemClass != null && itemClass.ActivateObject != null)
						{
							Transform transform2 = this.models[this.m_HoldingItemIdx].gameObject.transform.Find(itemClass.ActivateObject.Value);
							if (transform2 == null && this.models[this.m_HoldingItemIdx].gameObject.name.Equals(itemClass.ActivateObject.Value))
							{
								transform2 = this.models[this.m_HoldingItemIdx].gameObject.transform;
							}
							if (transform2 != null && transform2.parent.gameObject.activeInHierarchy)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}
	}

	// Token: 0x06002D70 RID: 11632 RVA: 0x001213A4 File Offset: 0x0011F5A4
	public float GetLightLevel()
	{
		float num = 0f;
		this.activatables.Clear();
		this.entity.CollectActivatableItems(this.activatables);
		for (int i = 0; i < this.activatables.Count; i++)
		{
			ItemValue itemValue = this.activatables[i];
			if (itemValue != null && itemValue.Activated)
			{
				ItemClass itemClass = itemValue.ItemClass;
				if (itemClass != null)
				{
					num += itemClass.lightValue;
				}
			}
		}
		ItemClass holdingItem = this.holdingItem;
		if (holdingItem.AlwaysActive != null && holdingItem.AlwaysActive.Value)
		{
			num += holdingItem.lightValue;
		}
		string propertyOverride = this.holdingItemItemValue.GetPropertyOverride("LightValue", string.Empty);
		if (propertyOverride.Length > 0)
		{
			num += float.Parse(propertyOverride);
		}
		return Mathf.Clamp01(num);
	}

	// Token: 0x06002D71 RID: 11633 RVA: 0x0012146E File Offset: 0x0011F66E
	public IEnumerator SimulateActionExecution(int _actionIdx, ItemStack _itemStack, Action<ItemStack> onComplete)
	{
		Inventory.<>c__DisplayClass138_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1._itemStack = _itemStack;
		CS$<>8__locals1.onComplete = onComplete;
		while (!this.GetItem(this.DUMMY_SLOT_IDX).IsEmpty())
		{
			yield return null;
		}
		this.SetItem(this.DUMMY_SLOT_IDX, CS$<>8__locals1._itemStack.Clone());
		yield return new WaitForSeconds(0.1f);
		CS$<>8__locals1.previousHoldingIdx = this.m_HoldingItemIdx;
		CS$<>8__locals1.previousFocusedIdx = this.m_FocusedItemIdx;
		this.SetHoldingItemIdx(this.DUMMY_SLOT_IDX);
		yield return new WaitForSeconds(0.1f);
		this.CallOnToolbeltChangedInternal();
		yield return new WaitForSeconds(0.1f);
		while (this.IsHolsterDelayActive())
		{
			yield return new WaitForSeconds(0.1f);
		}
		if (!this.<SimulateActionExecution>g__IsDummySlotActive|138_0(ref CS$<>8__locals1))
		{
			this.<SimulateActionExecution>g__HandleComplete|138_1(ref CS$<>8__locals1);
			yield break;
		}
		this.Execute(_actionIdx, false, null);
		yield return new WaitForSeconds(0.1f);
		if (!this.<SimulateActionExecution>g__IsDummySlotActive|138_0(ref CS$<>8__locals1))
		{
			this.<SimulateActionExecution>g__HandleComplete|138_1(ref CS$<>8__locals1);
			yield break;
		}
		this.Execute(_actionIdx, true, null);
		if (!this.<SimulateActionExecution>g__IsDummySlotActive|138_0(ref CS$<>8__locals1))
		{
			this.<SimulateActionExecution>g__HandleComplete|138_1(ref CS$<>8__locals1);
			yield break;
		}
		CS$<>8__locals1.dummyItem = this.GetItem(this.DUMMY_SLOT_IDX);
		if (CS$<>8__locals1.dummyItem.itemValue.ItemClass != null && CS$<>8__locals1.dummyItem.itemValue.ItemClass.Actions.Length > _actionIdx && CS$<>8__locals1.dummyItem.itemValue.ItemClass.Actions[_actionIdx] != null)
		{
			CS$<>8__locals1.dummyItem.itemValue.ItemClass.Actions[_actionIdx].OnHoldingUpdate(this.GetItemActionDataInSlot(this.DUMMY_SLOT_IDX, _actionIdx));
			while (this.IsHoldingItemActionRunning())
			{
				yield return new WaitForSeconds(0.1f);
			}
			if (!this.<SimulateActionExecution>g__IsDummySlotActive|138_0(ref CS$<>8__locals1))
			{
				this.<SimulateActionExecution>g__HandleComplete|138_1(ref CS$<>8__locals1);
				yield break;
			}
		}
		while (this.IsHolsterDelayActive())
		{
			yield return new WaitForSeconds(0.1f);
		}
		this.<SimulateActionExecution>g__HandleComplete|138_1(ref CS$<>8__locals1);
		yield break;
	}

	// Token: 0x06002D72 RID: 11634 RVA: 0x00121494 File Offset: 0x0011F694
	public void ForceHoldingItemUpdate()
	{
		if (this.models[this.holdingItemIdx] != null)
		{
			global::UnityEngine.Object.Destroy(this.models[this.holdingItemIdx].gameObject);
		}
		ItemStack holdingItemStack = this.holdingItemStack;
		ItemValue itemValue = holdingItemStack.itemValue.Clone();
		int count = holdingItemStack.count;
		if (itemValue.ItemClass != null)
		{
			this.models[this.holdingItemIdx] = (itemValue.ItemClass.CanHold() ? this.createHeldItem(this.holdingItemIdx, itemValue) : null);
			if (this.slots[this.holdingItemIdx] == null || !(this.slots[this.holdingItemIdx] is ItemClassBlock.ItemBlockInventoryData) || !(itemValue.ItemClass is ItemClassBlock))
			{
				this.slots[this.holdingItemIdx] = this.createInventoryData(this.holdingItemIdx, itemValue);
			}
		}
		this.slots[this.holdingItemIdx].itemStack.itemValue = itemValue;
		this.slots[this.holdingItemIdx].itemStack.count = count;
		this.m_LastDrawnHoldingItemIndex = -1;
		this.updateHoldingItem();
	}

	// Token: 0x06002D73 RID: 11635 RVA: 0x001215A0 File Offset: 0x0011F7A0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void updateHoldingItem()
	{
		ItemValue holdingItemItemValue = this.holdingItemItemValue;
		if (this.lastDrawnHoldingItemValue == holdingItemItemValue && this.m_LastDrawnHoldingItemIndex == this.holdingItemIdx)
		{
			this.holdingItem.OnHoldingReset(this.holdingItemData);
			return;
		}
		this.entity.bPlayerStatsChanged = !this.entity.isEntityRemote;
		if (this.lastdrawnHoldingItem != null)
		{
			this.entity.MinEventContext.ItemValue = this.lastDrawnHoldingItemValue;
			this.entity.MinEventContext.Transform = this.lastdrawnHoldingItemTransform;
			this.lastdrawnHoldingItem.StopHolding(this.lastdrawnHoldingItemData, this.lastdrawnHoldingItemTransform);
			if (!this.lastDrawnHoldingItemValue.ItemClass.ItemTags.Test_AnySet(this.ignoreWhenHeld))
			{
				this.lastDrawnHoldingItemValue.FireEvent(MinEventTypes.onSelfEquipStop, this.entity.MinEventContext);
			}
			if (this.lastdrawnHoldingItemTransform != null)
			{
				this.InitInactiveItemsObject();
				this.lastdrawnHoldingItemTransform.SetParent(this.inactiveItems, false);
				this.lastdrawnHoldingItemTransform.gameObject.SetActive(false);
			}
		}
		QuestEventManager.Current.HeldItem(this.holdingItemData.itemValue);
		this.holdingItem.StartHolding(this.holdingItemData, this.models[this.holdingItemIdx]);
		this.entity.MinEventContext.ItemValue = holdingItemItemValue;
		this.entity.MinEventContext.ItemValue.Seed = holdingItemItemValue.Seed;
		this.entity.MinEventContext.Transform = this.models[this.holdingItemIdx];
		this.setHoldingItemTransform(this.models[this.holdingItemIdx]);
		this.ShowRightHand(true);
		holdingItemItemValue.FireEvent(MinEventTypes.onSelfHoldingItemCreated, this.entity.MinEventContext);
		if (!holdingItemItemValue.ItemClass.ItemTags.Test_AnySet(this.ignoreWhenHeld))
		{
			holdingItemItemValue.FireEvent(MinEventTypes.onSelfEquipStart, this.entity.MinEventContext);
		}
		this.entity.OnHoldingItemChanged();
		this.m_LastDrawnHoldingItemIndex = this.m_HoldingItemIdx;
		this.lastdrawnHoldingItem = this.holdingItem;
		this.lastDrawnHoldingItemValue = holdingItemItemValue;
		this.lastdrawnHoldingItemTransform = this.models[this.holdingItemIdx];
		this.lastdrawnHoldingItemData = this.holdingItemData;
	}

	// Token: 0x06002D74 RID: 11636 RVA: 0x001217D0 File Offset: 0x0011F9D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void setHoldingItemTransform(Transform _t)
	{
		this.entity.SetHoldingItemTransform(_t);
		if (_t != null)
		{
			_t.position = Vector3.zero;
			_t.localPosition = (this.entity.emodel.IsFPV ? Vector3.zero : AnimationGunjointOffsetData.AnimationGunjointOffset[this.entity.inventory.holdingItem.HoldType.Value].position);
			_t.eulerAngles = Vector3.zero;
			_t.localEulerAngles = (this.entity.emodel.IsFPV ? Vector3.zero : AnimationGunjointOffsetData.AnimationGunjointOffset[this.entity.inventory.holdingItem.HoldType.Value].rotation);
			_t.localEulerAngles = _t.localRotation.eulerAngles;
			if (!this.holdingItem.GetCorrectionScale().Equals(Vector3.zero))
			{
				_t.localScale = this.holdingItem.GetCorrectionScale();
			}
			_t.gameObject.SetActive(!this.holdingItem.HoldingItemHidden);
		}
		this.syncHeldItem();
		this.lastdrawnHoldingItemTransform = _t;
	}

	// Token: 0x06002D75 RID: 11637 RVA: 0x00121900 File Offset: 0x0011FB00
	public int PreferredItemSlot(int _itemType, int _startSlotIdx)
	{
		for (int i = _startSlotIdx; i < this.preferredItemSlots.Length; i++)
		{
			if (this.preferredItemSlots[i] == _itemType)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06002D76 RID: 11638 RVA: 0x0012192E File Offset: 0x0011FB2E
	public void ClearPreferredItemInSlot(int _slotIdx)
	{
		if (_slotIdx < this.preferredItemSlots.Length)
		{
			this.preferredItemSlots[_slotIdx] = 0;
		}
	}

	// Token: 0x06002D77 RID: 11639 RVA: 0x00121944 File Offset: 0x0011FB44
	public bool CanStack(ItemStack _itemStack)
	{
		for (int i = 0; i < this.PUBLIC_SLOTS; i++)
		{
			if (this.CanMoveToSlot(_itemStack, i) && (this.slots[i].itemStack.IsEmpty() || this.slots[i].itemStack.CanStackWith(_itemStack, false)))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002D78 RID: 11640 RVA: 0x0012199C File Offset: 0x0011FB9C
	public bool CanStackNoEmpty(ItemStack _itemStack)
	{
		for (int i = 0; i < this.PUBLIC_SLOTS; i++)
		{
			int num;
			if (this.slots[i].itemStack.CanStackPartlyWith(_itemStack, out num))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002D79 RID: 11641 RVA: 0x001219D4 File Offset: 0x0011FBD4
	[return: TupleElementNames(new string[] { "anyMoved", "allMoved" })]
	public override ValueTuple<bool, bool> TryStackItem(int startIndex, ItemStack _itemStack)
	{
		bool flag = false;
		bool flag2 = false;
		for (int i = startIndex; i < this.PUBLIC_SLOTS; i++)
		{
			if (this.CanMoveToSlot(_itemStack, i))
			{
				int count = _itemStack.count;
				ItemStack itemStack = this.slots[i].itemStack;
				if (_itemStack.itemValue.type == itemStack.itemValue.type && !itemStack.IsEmpty() && itemStack.CanStackPartly(ref count))
				{
					flag = true;
					itemStack.count += count;
					_itemStack.count -= count;
					this.notifyListeners();
					this.entity.bPlayerStatsChanged = !this.entity.isEntityRemote;
					if (_itemStack.count == 0)
					{
						flag2 = true;
						break;
					}
				}
			}
		}
		return new ValueTuple<bool, bool>(flag, flag2);
	}

	// Token: 0x06002D7A RID: 11642 RVA: 0x00121AA0 File Offset: 0x0011FCA0
	public bool TryTakeItem(ItemStack _itemStack)
	{
		int num = 0;
		for (int i = 0; i < this.PUBLIC_SLOTS; i++)
		{
			num = _itemStack.count;
			ItemStack itemStack = this.slots[i].itemStack;
			if (itemStack.IsEmpty())
			{
				itemStack = _itemStack.Clone();
				this.notifyListeners();
				this.entity.bPlayerStatsChanged = !this.entity.isEntityRemote;
				return true;
			}
			if (_itemStack.itemValue.type == itemStack.itemValue.type && !itemStack.IsEmpty() && itemStack.CanStackPartly(ref num))
			{
				itemStack.count += num;
				_itemStack.count -= num;
				this.notifyListeners();
				this.entity.bPlayerStatsChanged = !this.entity.isEntityRemote;
				if (_itemStack.count == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06002D7B RID: 11643 RVA: 0x00121B7C File Offset: 0x0011FD7C
	public virtual bool CanTakeItem(ItemStack _itemStack)
	{
		for (int i = 0; i < this.slots.Length - 1; i++)
		{
			int num;
			if (this.slots[i].itemStack.CanStackPartlyWith(_itemStack, out num))
			{
				return true;
			}
			if (this.slots[i].itemStack.IsEmpty())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002D7C RID: 11644 RVA: 0x00121BCE File Offset: 0x0011FDCE
	public virtual void AddChangeListener(IInventoryChangedListener _listener)
	{
		if (this.listeners == null)
		{
			this.listeners = new HashSet<IInventoryChangedListener>();
		}
		this.listeners.Add(_listener);
		_listener.OnInventoryChanged(this);
	}

	// Token: 0x06002D7D RID: 11645 RVA: 0x00121BF7 File Offset: 0x0011FDF7
	public virtual void RemoveChangeListener(IInventoryChangedListener _listener)
	{
		if (this.listeners != null)
		{
			this.listeners.Remove(_listener);
		}
	}

	// Token: 0x06002D7E RID: 11646 RVA: 0x00121C0E File Offset: 0x0011FE0E
	public void Changed()
	{
		this.notifyListeners();
	}

	// Token: 0x06002D7F RID: 11647 RVA: 0x00121C18 File Offset: 0x0011FE18
	[PublicizedFrom(EAccessModifier.Private)]
	public void notifyListeners()
	{
		this.onInventoryChanged();
		if (this.listeners == null)
		{
			return;
		}
		foreach (IInventoryChangedListener inventoryChangedListener in this.listeners)
		{
			inventoryChangedListener.OnInventoryChanged(this);
		}
	}

	// Token: 0x06002D80 RID: 11648 RVA: 0x00121C78 File Offset: 0x0011FE78
	public virtual bool IsHUDDisabled()
	{
		return this.holdingItem.IsHUDDisabled(this.holdingItemData);
	}

	// Token: 0x06002D81 RID: 11649 RVA: 0x00121C8B File Offset: 0x0011FE8B
	public string CanInteract()
	{
		return this.holdingItem.CanInteract(this.holdingItemData);
	}

	// Token: 0x06002D82 RID: 11650 RVA: 0x00121C9E File Offset: 0x0011FE9E
	public void Interact()
	{
		this.holdingItem.Interact(this.holdingItemData);
	}

	// Token: 0x06002D83 RID: 11651 RVA: 0x00121CB1 File Offset: 0x0011FEB1
	public virtual void Execute(int _actionIdx, bool _bReleased, PlayerActionsLocal _playerActions = null)
	{
		if (this.IsHolsterDelayActive() || this.IsUnholsterDelayActive())
		{
			return;
		}
		if (this.WaitForSecondaryRelease && _actionIdx == 1)
		{
			if (!_bReleased)
			{
				return;
			}
			this.WaitForSecondaryRelease = false;
		}
		this.holdingItem.ExecuteAction(_actionIdx, this.holdingItemData, _bReleased, _playerActions);
	}

	// Token: 0x06002D84 RID: 11652 RVA: 0x00121CF4 File Offset: 0x0011FEF4
	public void ReleaseAll(PlayerActionsLocal _playerActions = null)
	{
		ItemClass holdingItem = this.holdingItem;
		for (int i = 0; i < holdingItem.Actions.Length; i++)
		{
			this.Execute(i, true, _playerActions);
		}
	}

	// Token: 0x06002D85 RID: 11653 RVA: 0x00121D24 File Offset: 0x0011FF24
	public void Clear()
	{
		for (int i = 0; i < this.slots.Length; i++)
		{
			this.slots[i] = this.emptyItem;
		}
	}

	// Token: 0x06002D86 RID: 11654 RVA: 0x00121D54 File Offset: 0x0011FF54
	public void Cleanup()
	{
		if (this.delayedShowHideHeldItemRoutine != null)
		{
			GameManager.Instance.StopCoroutine(this.delayedShowHideHeldItemRoutine);
			this.delayedShowHideHeldItemRoutine = null;
		}
		this.CleanupHoldingActions();
		this.ResetActiveIndex();
		this.entity = null;
		this.listeners = null;
		this.slots = null;
		this.models = null;
	}

	// Token: 0x06002D87 RID: 11655 RVA: 0x00121DA8 File Offset: 0x0011FFA8
	public void CleanupHoldingActions()
	{
		if (this.holdingItem != null && this.holdingItemData != null)
		{
			this.holdingItem.CleanupHoldingActions(this.holdingItemData);
		}
	}

	// Token: 0x06002D88 RID: 11656 RVA: 0x00121DCC File Offset: 0x0011FFCC
	public ItemStack[] GetSlots()
	{
		ItemStack[] array = new ItemStack[this.slots.Length];
		for (int i = 0; i < this.slots.Length; i++)
		{
			array[i] = this.slots[i].itemStack;
		}
		return array;
	}

	// Token: 0x06002D89 RID: 11657 RVA: 0x0012112A File Offset: 0x0011F32A
	public int GetSlotCount()
	{
		return this.slots.Length;
	}

	// Token: 0x06002D8A RID: 11658 RVA: 0x00121E0C File Offset: 0x0012000C
	public override void PerformActionOnSlots(Action<ItemStack> _action)
	{
		for (int i = 0; i < this.slots.Length; i++)
		{
			_action(this.slots[i].itemStack);
		}
	}

	// Token: 0x06002D8B RID: 11659 RVA: 0x00121E3F File Offset: 0x0012003F
	public override bool HasItem(ItemValue _item)
	{
		return this.GetItemCount(_item, false, -1, -1, true) > 0;
	}

	// Token: 0x06002D8C RID: 11660 RVA: 0x00121E50 File Offset: 0x00120050
	public int GetSlotWithItemValue(ItemValue _itemValue)
	{
		for (int i = 0; i < this.slots.Length; i++)
		{
			if (this.slots[i].itemValue.Equals(_itemValue))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06002D8D RID: 11661 RVA: 0x00121E88 File Offset: 0x00120088
	public List<int> GetSlotsWithBlock(Block _block)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.slots.Length; i++)
		{
			if (this.slots[i].item != null && this.slots[i].item.IsBlock())
			{
				if (_block.CanPickup)
				{
					if (this.slots[i].item.Name.Equals(_block.PickedUpItemValue))
					{
						list.Add(i);
					}
				}
				else if ((this.slots[i].item as ItemClassBlock).GetBlock().Equals(_block))
				{
					list.Add(i);
				}
			}
		}
		return list;
	}

	// Token: 0x06002D8E RID: 11662 RVA: 0x00121F2C File Offset: 0x0012012C
	public int GetBestQuickSwapSlot()
	{
		if (this.quickSwapSlotIdx == -1 || this.quickSwapItemValue == null)
		{
			return -1;
		}
		if (this.slots[this.quickSwapSlotIdx].itemValue.GetItemId() == this.quickSwapItemValue.GetItemId())
		{
			return this.quickSwapSlotIdx;
		}
		for (int i = 0; i < this.slots.Length; i++)
		{
			if (this.slots[i].itemValue.GetItemId() == this.quickSwapItemValue.GetItemId())
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06002D8F RID: 11663 RVA: 0x00121FAC File Offset: 0x001201AC
	public bool CanMoveToSlot(ItemStack _itemStack, int _slotIdx)
	{
		return _itemStack.itemValue.ItemClass == null || _itemStack.itemValue.ItemClass.CanMoveToSlot(XUiC_ItemStack.StackLocationTypes.ToolBelt, _slotIdx);
	}

	// Token: 0x06002D90 RID: 11664 RVA: 0x00121FCF File Offset: 0x001201CF
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <SimulateActionExecution>g__IsDummySlotActive|138_0(ref Inventory.<>c__DisplayClass138_0 A_1)
	{
		return this.m_HoldingItemIdx == this.DUMMY_SLOT_IDX && !this.GetItem(this.DUMMY_SLOT_IDX).IsEmpty();
	}

	// Token: 0x06002D91 RID: 11665 RVA: 0x00121FF8 File Offset: 0x001201F8
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <SimulateActionExecution>g__HandleComplete|138_1(ref Inventory.<>c__DisplayClass138_0 A_1)
	{
		A_1.dummyItem = this.GetItem(this.DUMMY_SLOT_IDX);
		ItemStack itemStack;
		if (object.Equals(A_1._itemStack, A_1.dummyItem))
		{
			itemStack = A_1._itemStack;
		}
		else
		{
			itemStack = A_1.dummyItem;
		}
		if (this.m_FocusedItemIdx == this.DUMMY_SLOT_IDX)
		{
			this.SetHoldingItemIdx(A_1.previousHoldingIdx);
			this.SetFocusedItemIdx(A_1.previousFocusedIdx);
		}
		this.SetItem(this.DUMMY_SLOT_IDX, ItemStack.Empty);
		A_1.onComplete(itemStack);
	}

	// Token: 0x04002363 RID: 9059
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly FastTags<TagGroup.Global> ignoreWhenHeld = FastTags<TagGroup.Global>.Parse("clothing,armor");

	// Token: 0x04002364 RID: 9060
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemInventoryData[] slots;

	// Token: 0x04002365 RID: 9061
	public Transform[] models;

	// Token: 0x04002366 RID: 9062
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] preferredItemSlots;

	// Token: 0x04002367 RID: 9063
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass wearingActiveItem;

	// Token: 0x04002368 RID: 9064
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue wearingActiveItemValue;

	// Token: 0x04002369 RID: 9065
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass lastdrawnHoldingItem;

	// Token: 0x0400236A RID: 9066
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue lastDrawnHoldingItemValue;

	// Token: 0x0400236B RID: 9067
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform lastdrawnHoldingItemTransform;

	// Token: 0x0400236C RID: 9068
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemInventoryData lastdrawnHoldingItemData;

	// Token: 0x0400236D RID: 9069
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool flashlightOn;

	// Token: 0x0400236E RID: 9070
	public ItemStack itemArmor;

	// Token: 0x0400236F RID: 9071
	public ItemStack itemOnBack;

	// Token: 0x04002370 RID: 9072
	[PublicizedFrom(EAccessModifier.Protected)]
	public int m_HoldingItemIdx;

	// Token: 0x04002371 RID: 9073
	[PublicizedFrom(EAccessModifier.Protected)]
	public int m_LastDrawnHoldingItemIndex;

	// Token: 0x04002372 RID: 9074
	public Transform inactiveItems;

	// Token: 0x04002373 RID: 9075
	[PublicizedFrom(EAccessModifier.Protected)]
	public EntityAlive entity;

	// Token: 0x04002374 RID: 9076
	[PublicizedFrom(EAccessModifier.Protected)]
	public IGameManager gameManager;

	// Token: 0x04002375 RID: 9077
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue bareHandItemValue;

	// Token: 0x04002376 RID: 9078
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass bareHandItem;

	// Token: 0x04002377 RID: 9079
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemInventoryData bareHandItemInventoryData;

	// Token: 0x04002378 RID: 9080
	[PublicizedFrom(EAccessModifier.Protected)]
	public HashSet<IInventoryChangedListener> listeners;

	// Token: 0x04002379 RID: 9081
	[PublicizedFrom(EAccessModifier.Protected)]
	public int m_FocusedItemIdx;

	// Token: 0x0400237A RID: 9082
	public Inventory.HeldItemState HoldState;

	// Token: 0x0400237B RID: 9083
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemInventoryData emptyItem;

	// Token: 0x0400237D RID: 9085
	public bool WaitForSecondaryRelease;

	// Token: 0x0400237E RID: 9086
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue previousHeldItemValue;

	// Token: 0x0400237F RID: 9087
	[PublicizedFrom(EAccessModifier.Private)]
	public int previousHeldItemSlotIdx;

	// Token: 0x04002380 RID: 9088
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue quickSwapItemValue;

	// Token: 0x04002381 RID: 9089
	[PublicizedFrom(EAccessModifier.Private)]
	public int quickSwapSlotIdx;

	// Token: 0x04002382 RID: 9090
	[PublicizedFrom(EAccessModifier.Private)]
	public Coroutine delayedShowHideHeldItemRoutine;

	// Token: 0x04002383 RID: 9091
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSwitchingHeldItem;

	// Token: 0x04002384 RID: 9092
	[PublicizedFrom(EAccessModifier.Private)]
	public int currActiveItemIndex = -2;

	// Token: 0x04002385 RID: 9093
	public bool bResetLightLevelWhenChanged = true;

	// Token: 0x04002386 RID: 9094
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ItemValue> activatables = new List<ItemValue>();

	// Token: 0x0200056F RID: 1391
	public enum HeldItemState
	{
		// Token: 0x04002388 RID: 9096
		None,
		// Token: 0x04002389 RID: 9097
		Unholstering,
		// Token: 0x0400238A RID: 9098
		Holding,
		// Token: 0x0400238B RID: 9099
		Holstering
	}

	// Token: 0x02000570 RID: 1392
	public enum ActiveIndex
	{
		// Token: 0x0400238D RID: 9101
		NOT_INITIALIZED = -2,
		// Token: 0x0400238E RID: 9102
		ALL_OFF
	}
}
