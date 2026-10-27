using System;
using System.Collections;
using Audio;
using UnityEngine;

// Token: 0x02000578 RID: 1400
public sealed class Hand
{
	// Token: 0x170004C7 RID: 1223
	// (get) Token: 0x06002D19 RID: 11545 RVA: 0x0012100C File Offset: 0x0011F20C
	public ItemInventoryData Held
	{
		get
		{
			ItemInventoryData itemInventoryData = this.bareHandData;
			if (this.mode == Hand.HoldingMode.Transient && this.transientData.HasItem)
			{
				itemInventoryData = this.transientData;
			}
			else if (this.mode == Hand.HoldingMode.Current && this.toolbelt != null)
			{
				ItemInventoryData itemInventoryData2 = this.slotDatas[this.toolbelt.SelectedSlot];
				if (itemInventoryData2.item != null)
				{
					itemInventoryData = itemInventoryData2;
				}
			}
			itemInventoryData.EnsureActionData();
			return itemInventoryData;
		}
	}

	// Token: 0x170004C8 RID: 1224
	// (get) Token: 0x06002D1A RID: 11546 RVA: 0x00121073 File Offset: 0x0011F273
	public ItemClass Item
	{
		get
		{
			return this.Held.item;
		}
	}

	// Token: 0x170004C9 RID: 1225
	// (get) Token: 0x06002D1B RID: 11547 RVA: 0x00121080 File Offset: 0x0011F280
	public ItemValue ItemValue
	{
		get
		{
			return this.Held.itemValue;
		}
	}

	// Token: 0x170004CA RID: 1226
	// (get) Token: 0x06002D1C RID: 11548 RVA: 0x0012108D File Offset: 0x0011F28D
	public ItemStack Stack
	{
		get
		{
			return this.Held.itemStack;
		}
	}

	// Token: 0x170004CB RID: 1227
	// (get) Token: 0x06002D1D RID: 11549 RVA: 0x0012109A File Offset: 0x0011F29A
	public int Count
	{
		get
		{
			return this.Held.itemStack.count;
		}
	}

	// Token: 0x170004CC RID: 1228
	// (get) Token: 0x06002D1E RID: 11550 RVA: 0x001210AC File Offset: 0x0011F2AC
	public bool IsBusy
	{
		get
		{
			return this.isSwitching || this.mode == Hand.HoldingMode.Transient || (this.Item != null && this.Item.IsActionRunning(this.Held));
		}
	}

	// Token: 0x170004CD RID: 1229
	// (get) Token: 0x06002D1F RID: 11551 RVA: 0x001210DC File Offset: 0x0011F2DC
	public bool IsSwitching
	{
		get
		{
			return this.isSwitching;
		}
	}

	// Token: 0x170004CE RID: 1230
	// (get) Token: 0x06002D20 RID: 11552 RVA: 0x001210E4 File Offset: 0x0011F2E4
	public bool IsHolstered
	{
		get
		{
			return this.mode == Hand.HoldingMode.Bare;
		}
	}

	// Token: 0x170004CF RID: 1231
	// (get) Token: 0x06002D21 RID: 11553 RVA: 0x001210EF File Offset: 0x0011F2EF
	public ItemValue BareHandItemValue
	{
		get
		{
			return this.bareHandData.itemValue;
		}
	}

	// Token: 0x170004D0 RID: 1232
	// (get) Token: 0x06002D22 RID: 11554 RVA: 0x001210FC File Offset: 0x0011F2FC
	public ItemClass BareHandItem
	{
		get
		{
			return this.bareHandData.item;
		}
	}

	// Token: 0x170004D1 RID: 1233
	// (get) Token: 0x06002D23 RID: 11555 RVA: 0x00121109 File Offset: 0x0011F309
	public int DataSlotNoBarehandFallback
	{
		get
		{
			return this.Held.slotIdx;
		}
	}

	// Token: 0x170004D2 RID: 1234
	// (get) Token: 0x06002D24 RID: 11556 RVA: 0x00121116 File Offset: 0x0011F316
	public Transform ModelTransform
	{
		get
		{
			if (this.equipped != null)
			{
				return this.equipped.model;
			}
			if (this.holstering != null)
			{
				return this.holstering.model;
			}
			return null;
		}
	}

	// Token: 0x06002D25 RID: 11557 RVA: 0x00121141 File Offset: 0x0011F341
	public Hand(EntityAlive _entity)
	{
		this.entity = _entity;
		this.bareHandData = ItemInventoryData.Create(_entity, null, -1);
		this.transientData = ItemInventoryData.Create(_entity, null, -1);
	}

	// Token: 0x06002D26 RID: 11558 RVA: 0x0012117C File Offset: 0x0011F37C
	public void BindToolbelt(Inventory _toolbelt)
	{
		if (this.slotDatas == null)
		{
			_toolbelt.ItemGrid.SlotChanged += this.OnSlotChanged;
			this.slotDatas = new ItemInventoryData[_toolbelt.Length];
			for (int i = 0; i < this.slotDatas.Length; i++)
			{
				this.slotDatas[i] = ItemInventoryData.Create(this.entity, null, i);
			}
		}
		this.toolbelt = _toolbelt;
		for (int j = 0; j < this.slotDatas.Length; j++)
		{
			this.slotDatas[j].SetStack(this.toolbelt.ItemGrid[j]);
			this.slotDatas[j].Rebuild();
		}
	}

	// Token: 0x06002D27 RID: 11559 RVA: 0x00121228 File Offset: 0x0011F428
	public void Cleanup()
	{
		if (this.switchingCoroutine != null)
		{
			GameManager.Instance.StopCoroutine(this.switchingCoroutine);
			this.switchingCoroutine = null;
		}
		if (this.toolbelt != null)
		{
			this.toolbelt.ItemGrid.SlotChanged -= this.OnSlotChanged;
		}
		int num = 0;
		while (this.slotDatas != null && num < this.slotDatas.Length)
		{
			this.slotDatas[num].Teardown();
			num++;
		}
		this.bareHandData.Teardown();
		this.transientData.Teardown();
		this.holstering = null;
		this.mode = Hand.HoldingMode.Current;
	}

	// Token: 0x06002D28 RID: 11560 RVA: 0x001212C4 File Offset: 0x0011F4C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnSlotChanged(int _idx)
	{
		this.slotDatas[_idx].Rebuild();
		this.Reconcile();
	}

	// Token: 0x06002D29 RID: 11561 RVA: 0x001212DA File Offset: 0x0011F4DA
	public ItemInventoryData GetSlotData(int _idx)
	{
		if (this.slotDatas[_idx].item == null)
		{
			return null;
		}
		return this.slotDatas[_idx];
	}

	// Token: 0x06002D2A RID: 11562 RVA: 0x001212F8 File Offset: 0x0011F4F8
	public void SelectSlot(int _slot, bool _applyHolsterTime)
	{
		ItemInventoryData itemInventoryData = this.slotDatas[_slot];
		ItemClass item = this.Item;
		if (this.entity != null && this.entity.emodel != null && this.entity.emodel.avatarController != null)
		{
			this.entity.emodel.avatarController.TriggerEvent(AvatarController.itemHasChangedTriggerHash);
		}
		for (int i = 0; i < item.Actions.Length; i++)
		{
			if (item.Actions[i] is ItemActionAttack)
			{
				Manager.BroadcastStop(this.entity.entityId, item.Actions[i].GetSoundStart());
			}
		}
		if (this.equipped == itemInventoryData)
		{
			item.OnHoldingReset(itemInventoryData);
		}
		this.toolbelt.SetSelectedSlot(_slot);
		this.SelectHoldingMode(Hand.HoldingMode.Current, _applyHolsterTime ? 0.2f : 0f);
	}

	// Token: 0x06002D2B RID: 11563 RVA: 0x001213DC File Offset: 0x0011F5DC
	public void SelectHoldingMode(Hand.HoldingMode _mode, float _holsterTime = 0f)
	{
		ItemInventoryData itemInventoryData = this.equipped;
		ItemClass itemClass;
		if (itemInventoryData == null)
		{
			itemClass = null;
		}
		else
		{
			ItemValue physicalItemValue = itemInventoryData.PhysicalItemValue;
			itemClass = ((physicalItemValue != null) ? physicalItemValue.ItemClass : null);
		}
		ItemClass itemClass2 = itemClass ?? this.Item;
		this.isSwitching = true;
		this.CancelAnimatorActions();
		this.mode = _mode;
		ItemInventoryData held = this.Held;
		if (this.equipped != null && (this.equipped != held || held.NeedsRebuild))
		{
			this.Unequip();
		}
		if (this.mode != Hand.HoldingMode.Transient && (this.transientData.HasItem || this.transientData.NeedsRebuild))
		{
			this.transientData.SetStack(null);
			this.transientData.Rebuild();
		}
		held.Rebuild();
		if (this.entity.isEntityRemote)
		{
			this.CompleteTransition();
			return;
		}
		if (this.switchingCoroutine != null)
		{
			GameManager.Instance.StopCoroutine(this.switchingCoroutine);
		}
		this.switchingCoroutine = GameManager.Instance.StartCoroutine(this.TransitionRoutine(itemClass2, _holsterTime));
	}

	// Token: 0x06002D2C RID: 11564 RVA: 0x001214D2 File Offset: 0x0011F6D2
	public void SetBareHandItem(ItemValue _itemValue)
	{
		this.bareHandData.SetStack(new ItemStack(_itemValue, 0));
		this.bareHandData.Rebuild();
		if (this.equipped == this.bareHandData)
		{
			this.Reconcile();
		}
	}

	// Token: 0x06002D2D RID: 11565 RVA: 0x00121506 File Offset: 0x0011F706
	public bool UsingBareHand()
	{
		return this.Held == this.bareHandData;
	}

	// Token: 0x06002D2E RID: 11566 RVA: 0x00121516 File Offset: 0x0011F716
	public IEnumerator SimulateActionExecution(int _actionIdx, ItemStackGrid _sourceGrid, int _sourceIdx)
	{
		Hand.<>c__DisplayClass52_0 CS$<>8__locals1 = new Hand.<>c__DisplayClass52_0();
		CS$<>8__locals1._sourceIdx = _sourceIdx;
		CS$<>8__locals1.<>4__this = this;
		while (this.mode == Hand.HoldingMode.Transient)
		{
			yield return null;
		}
		ItemStack itemStack = _sourceGrid[CS$<>8__locals1._sourceIdx];
		if (this.Item == null || itemStack.IsEmpty())
		{
			yield break;
		}
		this.transientData.SetStack(itemStack);
		this.SelectHoldingMode(Hand.HoldingMode.Transient, 0.2f);
		Action<int> onSourceSlotChanged = delegate(int _changedIdx)
		{
			if (_changedIdx == CS$<>8__locals1._sourceIdx)
			{
				CS$<>8__locals1.<>4__this.Reconcile();
			}
		};
		_sourceGrid.SlotChanged += onSourceSlotChanged;
		try
		{
			yield return new WaitForSeconds(0.2f);
			while (this.isSwitching)
			{
				yield return new WaitForSeconds(0.1f);
			}
			if (!CS$<>8__locals1.<SimulateActionExecution>g__IsHoldingTransient|1())
			{
				yield break;
			}
			this.Execute(_actionIdx, false, null);
			yield return new WaitForSeconds(0.1f);
			if (!CS$<>8__locals1.<SimulateActionExecution>g__IsHoldingTransient|1())
			{
				yield break;
			}
			this.Execute(_actionIdx, true, null);
			if (!CS$<>8__locals1.<SimulateActionExecution>g__IsHoldingTransient|1())
			{
				yield break;
			}
			ItemClass item = this.transientData.item;
			if (item != null && item.Actions.Length > _actionIdx && item.Actions[_actionIdx] != null)
			{
				item.Actions[_actionIdx].OnHoldingUpdate(this.transientData.actionData[_actionIdx]);
				while (this.IsActionRunning())
				{
					yield return new WaitForSeconds(0.1f);
				}
				if (!CS$<>8__locals1.<SimulateActionExecution>g__IsHoldingTransient|1())
				{
					yield break;
				}
			}
			while (this.isSwitching)
			{
				yield return new WaitForSeconds(0.1f);
			}
		}
		finally
		{
			_sourceGrid.SlotChanged -= onSourceSlotChanged;
			if (CS$<>8__locals1.<SimulateActionExecution>g__IsHoldingTransient|1())
			{
				this.SelectHoldingMode(Hand.HoldingMode.Current, 0.2f);
			}
		}
		yield break;
		yield break;
	}

	// Token: 0x06002D2F RID: 11567 RVA: 0x0012153A File Offset: 0x0011F73A
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator TransitionRoutine(ItemClass _outgoing, float waitTime)
	{
		EntityPlayerLocal epl = this.entity as EntityPlayerLocal;
		if (epl != null)
		{
			while (epl.emodel.avatarController.IsAnimationPlayerFPRevivePlaying())
			{
				yield return new WaitForSeconds(0.1f);
			}
			epl.HolsterWeapon(true);
			this.entity.StopOneShot(_outgoing.SoundUnholster);
			this.entity.PlayOneShot(_outgoing.SoundHolster, false, false, false, null, 1f);
			if (waitTime > 0f)
			{
				if (waitTime < 0.15f)
				{
					waitTime = 0.15f;
				}
				yield return new WaitForSeconds(waitTime);
			}
			this.CompleteTransition();
			this.entity.StopOneShot(_outgoing.SoundHolster);
			this.entity.PlayOneShot(this.Item.SoundUnholster, false, false, false, null, 1f);
			if (this.entity.emodel && this.entity.emodel.avatarController)
			{
				this.entity.emodel.avatarController.TriggerEvent(AvatarController.itemHasChangedTriggerHash);
			}
			epl.HolsterWeapon(false);
			if (epl.bFirstPersonView)
			{
				epl.ShowHoldingItemLayer(true);
			}
			this.entity.ShowHoldingItem(true);
		}
		else
		{
			this.CompleteTransition();
		}
		yield return new WaitForSeconds(Mathf.Max(waitTime, 0.3f));
		if (this.mode == Hand.HoldingMode.Current && !this.UsingBareHand() && !this.Stack.IsEmpty())
		{
			if (this.previousHeldItemSlotIdx == this.Held.slotIdx)
			{
				yield break;
			}
			this.quickSwapItemValue = this.previousHeldItemValue;
			this.quickSwapSlotIdx = this.previousHeldItemSlotIdx;
			this.previousHeldItemValue = this.ItemValue;
			this.previousHeldItemSlotIdx = this.Held.slotIdx;
		}
		yield break;
	}

	// Token: 0x06002D30 RID: 11568 RVA: 0x00121558 File Offset: 0x0011F758
	[PublicizedFrom(EAccessModifier.Private)]
	public void CompleteTransition()
	{
		this.isSwitching = false;
		if (this.holstering != null)
		{
			this.holstering.UnseatModel();
			this.holstering = null;
		}
		this.Reconcile();
		this.entity.MinEventContext.Self = this.entity;
		this.entity.MinEventContext.ItemValue = this.ItemValue;
		this.CancelAnimatorActions();
	}

	// Token: 0x06002D31 RID: 11569 RVA: 0x001215C0 File Offset: 0x0011F7C0
	public int GetBestQuickSwapSlot()
	{
		if (this.quickSwapSlotIdx == -1 || this.quickSwapItemValue == null || this.toolbelt.GetStackAt(this.quickSwapSlotIdx).itemValue.ItemClass == null)
		{
			return -1;
		}
		if (this.toolbelt.GetStackAt(this.quickSwapSlotIdx).itemValue.GetItemId() == this.quickSwapItemValue.GetItemId())
		{
			return this.quickSwapSlotIdx;
		}
		for (int i = 0; i < this.toolbelt.Length; i++)
		{
			if (this.toolbelt.GetStackAt(i).itemValue.GetItemId() == this.quickSwapItemValue.GetItemId())
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06002D32 RID: 11570 RVA: 0x00121668 File Offset: 0x0011F868
	public void Reconcile()
	{
		if (this.isSwitching)
		{
			return;
		}
		ItemInventoryData held = this.Held;
		if (this.equipped != null && (this.equipped != held || held.NeedsRebuild))
		{
			this.Unequip();
		}
		held.Rebuild();
		if (this.equipped == null)
		{
			this.Equip(held);
			return;
		}
		Transform model = this.equipped.model;
		if (model != null)
		{
			global::UnityEngine.Object parent = model.parent;
			EModelBase emodel = this.entity.emodel;
			global::UnityEngine.Object @object;
			if (emodel == null)
			{
				@object = null;
			}
			else
			{
				AvatarController avatarController = emodel.avatarController;
				@object = ((avatarController != null) ? avatarController.GetRightHandTransform() : null);
			}
			if (parent != @object)
			{
				this.equipped.SeatModel();
			}
		}
	}

	// Token: 0x06002D33 RID: 11571 RVA: 0x0012170C File Offset: 0x0011F90C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Equip(ItemInventoryData _held)
	{
		ItemValue itemValue = _held.itemValue;
		this.equipped = _held;
		_held.OnEquipped();
		Transform model = _held.model;
		this.entity.bPlayerStatsChanged = !this.entity.isEntityRemote;
		QuestEventManager.Current.HeldItem(itemValue);
		_held.item.StartHolding(_held, model);
		this.entity.MinEventContext.ItemValue = itemValue;
		this.entity.MinEventContext.Transform = model;
		_held.SeatModel();
		this.FireActivationEvents();
		itemValue.FireEvent(MinEventTypes.onSelfHoldingItemCreated, this.entity.MinEventContext);
		if (!_held.item.ItemTags.Test_AnySet(Hand.IgnoreWhenHeld))
		{
			itemValue.FireEvent(MinEventTypes.onSelfEquipStart, this.entity.MinEventContext);
		}
		this.entity.OnHoldingItemChanged();
	}

	// Token: 0x06002D34 RID: 11572 RVA: 0x001217DC File Offset: 0x0011F9DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Unequip()
	{
		ItemInventoryData itemInventoryData = this.equipped;
		ItemValue physicalItemValue = itemInventoryData.PhysicalItemValue;
		ItemClass itemClass = physicalItemValue.ItemClass;
		this.equipped = null;
		itemInventoryData.OnUnequipped();
		Transform model = itemInventoryData.model;
		this.entity.bPlayerStatsChanged = !this.entity.isEntityRemote;
		this.entity.MinEventContext.ItemValue = physicalItemValue;
		this.entity.MinEventContext.Transform = model;
		if (!itemClass.ItemTags.Test_AnySet(Hand.IgnoreWhenHeld))
		{
			physicalItemValue.FireEvent(MinEventTypes.onSelfEquipStop, this.entity.MinEventContext);
		}
		itemClass.StopHolding(itemInventoryData, model);
		if (this.isSwitching)
		{
			this.holstering = itemInventoryData;
		}
		else
		{
			itemInventoryData.UnseatModel();
		}
		this.CancelAnimatorActions();
	}

	// Token: 0x06002D35 RID: 11573 RVA: 0x00121898 File Offset: 0x0011FA98
	[PublicizedFrom(EAccessModifier.Private)]
	public void FireActivationEvents()
	{
		if (this.ItemValue == null || this.ItemValue.IsEmpty())
		{
			return;
		}
		this.entity.IsEquipping = true;
		if (this.ItemValue.ItemClass == null)
		{
			Log.Out("[{0}] Cannot find item class for held item id '{1}'", new object[]
			{
				"Hand",
				this.ItemValue.type
			});
		}
		else if (!this.ItemValue.ItemClass.ItemTags.Test_AnySet(Hand.IgnoreWhenHeld))
		{
			this.entity.MinEventContext.ItemValue = this.ItemValue;
			if (this.ItemValue.ItemClass.HasTrigger(MinEventTypes.onSelfItemActivate))
			{
				this.ItemValue.FireEvent(this.ItemValue.Activated ? MinEventTypes.onSelfItemActivate : MinEventTypes.onSelfItemDeactivate, this.entity.MinEventContext);
			}
			if (this.ItemValue.ModificationCount > 0)
			{
				ItemValue itemValue = this.entity.MinEventContext.ItemValue;
				for (int i = 0; i < this.ItemValue.ModificationCount; i++)
				{
					ItemValue modification = this.ItemValue.GetModification(i);
					if (modification != null && modification.ItemClass != null)
					{
						this.entity.MinEventContext.ItemValue = modification;
						if (modification.ItemClass.HasTrigger(MinEventTypes.onSelfItemActivate))
						{
							modification.FireEvent(modification.Activated ? MinEventTypes.onSelfItemActivate : MinEventTypes.onSelfItemDeactivate, this.entity.MinEventContext);
						}
					}
				}
				this.entity.MinEventContext.ItemValue = itemValue;
			}
		}
		Inventory inventory = this.toolbelt;
		if (inventory != null)
		{
			inventory.CallOnToolbeltChangedInternal();
		}
		this.entity.IsEquipping = false;
	}

	// Token: 0x06002D36 RID: 11574 RVA: 0x00121A34 File Offset: 0x0011FC34
	[PublicizedFrom(EAccessModifier.Private)]
	public void CancelAnimatorActions()
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

	// Token: 0x06002D37 RID: 11575 RVA: 0x00121B08 File Offset: 0x0011FD08
	public void OnUpdate()
	{
		if (!this.isSwitching && !this.entity.IsDead() && this.entity.IsSpawned())
		{
			this.Item.OnHoldingUpdate(this.Held);
			ItemValue itemValue = this.Held.itemValue;
			this.entity.MinEventContext.ItemValue = itemValue;
			itemValue.FireEvent(MinEventTypes.onSelfEquipUpdate, this.entity.MinEventContext);
		}
		if (this.entity is EntityPlayer && this.entity.emodel && this.entity.emodel.avatarController)
		{
			this.entity.emodel.avatarController.CancelEvent(AvatarController.itemHasChangedTriggerHash);
		}
	}

	// Token: 0x06002D38 RID: 11576 RVA: 0x00121BC8 File Offset: 0x0011FDC8
	public void Execute(int _actionIdx, bool _bReleased, PlayerActionsLocal _playerActions = null)
	{
		if (this.isSwitching)
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
		this.Item.ExecuteAction(_actionIdx, this.Held, _bReleased, _playerActions);
	}

	// Token: 0x06002D39 RID: 11577 RVA: 0x00121C00 File Offset: 0x0011FE00
	public void ReleaseAll(PlayerActionsLocal _playerActions = null)
	{
		ItemClass item = this.Item;
		for (int i = 0; i < item.Actions.Length; i++)
		{
			this.Execute(i, true, _playerActions);
		}
	}

	// Token: 0x06002D3A RID: 11578 RVA: 0x00121C30 File Offset: 0x0011FE30
	public bool IsActionRunning()
	{
		return this.Item.IsActionRunning(this.Held);
	}

	// Token: 0x06002D3B RID: 11579 RVA: 0x00121C43 File Offset: 0x0011FE43
	public bool IsHUDDisabled()
	{
		return this.Item.IsHUDDisabled(this.Held);
	}

	// Token: 0x06002D3C RID: 11580 RVA: 0x00121C56 File Offset: 0x0011FE56
	public string CanInteract()
	{
		return this.Item.CanInteract(this.Held);
	}

	// Token: 0x06002D3D RID: 11581 RVA: 0x00121C69 File Offset: 0x0011FE69
	public void Interact()
	{
		this.Item.Interact(this.Held);
	}

	// Token: 0x04002386 RID: 9094
	public static readonly FastTags<TagGroup.Global> IgnoreWhenHeld = FastTags<TagGroup.Global>.Parse("clothing,armor");

	// Token: 0x04002387 RID: 9095
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cHolsterTime = 0.2f;

	// Token: 0x04002388 RID: 9096
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cQuickSwapSettleTime = 0.3f;

	// Token: 0x04002389 RID: 9097
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly EntityAlive entity;

	// Token: 0x0400238A RID: 9098
	[PublicizedFrom(EAccessModifier.Private)]
	public Inventory toolbelt;

	// Token: 0x0400238B RID: 9099
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemInventoryData[] slotDatas;

	// Token: 0x0400238C RID: 9100
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemInventoryData bareHandData;

	// Token: 0x0400238D RID: 9101
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemInventoryData transientData;

	// Token: 0x0400238E RID: 9102
	[PublicizedFrom(EAccessModifier.Private)]
	public Hand.HoldingMode mode;

	// Token: 0x0400238F RID: 9103
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue previousHeldItemValue;

	// Token: 0x04002390 RID: 9104
	[PublicizedFrom(EAccessModifier.Private)]
	public int previousHeldItemSlotIdx = -1;

	// Token: 0x04002391 RID: 9105
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue quickSwapItemValue;

	// Token: 0x04002392 RID: 9106
	[PublicizedFrom(EAccessModifier.Private)]
	public int quickSwapSlotIdx = -1;

	// Token: 0x04002393 RID: 9107
	public bool WaitForSecondaryRelease;

	// Token: 0x04002394 RID: 9108
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSwitching;

	// Token: 0x04002395 RID: 9109
	[PublicizedFrom(EAccessModifier.Private)]
	public Coroutine switchingCoroutine;

	// Token: 0x04002396 RID: 9110
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemInventoryData equipped;

	// Token: 0x04002397 RID: 9111
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemInventoryData holstering;

	// Token: 0x02000579 RID: 1401
	public enum HoldingMode
	{
		// Token: 0x04002399 RID: 9113
		Current,
		// Token: 0x0400239A RID: 9114
		Bare,
		// Token: 0x0400239B RID: 9115
		Transient
	}
}
