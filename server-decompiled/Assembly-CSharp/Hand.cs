using System;
using System.Collections;
using Audio;
using UnityEngine;

// Token: 0x02000571 RID: 1393
public sealed class Hand
{
	// Token: 0x170004C7 RID: 1223
	// (get) Token: 0x06002D0E RID: 11534 RVA: 0x00120A4C File Offset: 0x0011EC4C
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
	// (get) Token: 0x06002D0F RID: 11535 RVA: 0x00120AB3 File Offset: 0x0011ECB3
	public ItemClass Item
	{
		get
		{
			return this.Held.item;
		}
	}

	// Token: 0x170004C9 RID: 1225
	// (get) Token: 0x06002D10 RID: 11536 RVA: 0x00120AC0 File Offset: 0x0011ECC0
	public ItemValue ItemValue
	{
		get
		{
			return this.Held.itemValue;
		}
	}

	// Token: 0x170004CA RID: 1226
	// (get) Token: 0x06002D11 RID: 11537 RVA: 0x00120ACD File Offset: 0x0011ECCD
	public ItemStack Stack
	{
		get
		{
			return this.Held.itemStack;
		}
	}

	// Token: 0x170004CB RID: 1227
	// (get) Token: 0x06002D12 RID: 11538 RVA: 0x00120ADA File Offset: 0x0011ECDA
	public int Count
	{
		get
		{
			return this.Held.itemStack.count;
		}
	}

	// Token: 0x170004CC RID: 1228
	// (get) Token: 0x06002D13 RID: 11539 RVA: 0x00120AEC File Offset: 0x0011ECEC
	public bool IsBusy
	{
		get
		{
			return this.isSwitching || this.mode == Hand.HoldingMode.Transient || (this.Item != null && this.Item.IsActionRunning(this.Held));
		}
	}

	// Token: 0x170004CD RID: 1229
	// (get) Token: 0x06002D14 RID: 11540 RVA: 0x00120B1C File Offset: 0x0011ED1C
	public bool IsSwitching
	{
		get
		{
			return this.isSwitching;
		}
	}

	// Token: 0x170004CE RID: 1230
	// (get) Token: 0x06002D15 RID: 11541 RVA: 0x00120B24 File Offset: 0x0011ED24
	public bool IsHolstered
	{
		get
		{
			return this.mode == Hand.HoldingMode.Bare;
		}
	}

	// Token: 0x170004CF RID: 1231
	// (get) Token: 0x06002D16 RID: 11542 RVA: 0x00120B2F File Offset: 0x0011ED2F
	public ItemValue BareHandItemValue
	{
		get
		{
			return this.bareHandData.itemValue;
		}
	}

	// Token: 0x170004D0 RID: 1232
	// (get) Token: 0x06002D17 RID: 11543 RVA: 0x00120B3C File Offset: 0x0011ED3C
	public ItemClass BareHandItem
	{
		get
		{
			return this.bareHandData.item;
		}
	}

	// Token: 0x170004D1 RID: 1233
	// (get) Token: 0x06002D18 RID: 11544 RVA: 0x00120B49 File Offset: 0x0011ED49
	public int DataSlotNoBarehandFallback
	{
		get
		{
			return this.Held.slotIdx;
		}
	}

	// Token: 0x170004D2 RID: 1234
	// (get) Token: 0x06002D19 RID: 11545 RVA: 0x00120B56 File Offset: 0x0011ED56
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

	// Token: 0x06002D1A RID: 11546 RVA: 0x00120B81 File Offset: 0x0011ED81
	public Hand(EntityAlive _entity)
	{
		this.entity = _entity;
		this.bareHandData = ItemInventoryData.Create(_entity, null, -1);
		this.transientData = ItemInventoryData.Create(_entity, null, -1);
	}

	// Token: 0x06002D1B RID: 11547 RVA: 0x00120BBC File Offset: 0x0011EDBC
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

	// Token: 0x06002D1C RID: 11548 RVA: 0x00120C68 File Offset: 0x0011EE68
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

	// Token: 0x06002D1D RID: 11549 RVA: 0x00120D04 File Offset: 0x0011EF04
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnSlotChanged(int _idx)
	{
		this.slotDatas[_idx].Rebuild();
		this.Reconcile();
	}

	// Token: 0x06002D1E RID: 11550 RVA: 0x00120D1A File Offset: 0x0011EF1A
	public ItemInventoryData GetSlotData(int _idx)
	{
		if (this.slotDatas[_idx].item == null)
		{
			return null;
		}
		return this.slotDatas[_idx];
	}

	// Token: 0x06002D1F RID: 11551 RVA: 0x00120D38 File Offset: 0x0011EF38
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

	// Token: 0x06002D20 RID: 11552 RVA: 0x00120E1C File Offset: 0x0011F01C
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

	// Token: 0x06002D21 RID: 11553 RVA: 0x00120F12 File Offset: 0x0011F112
	public void SetBareHandItem(ItemValue _itemValue)
	{
		this.bareHandData.SetStack(new ItemStack(_itemValue, 0));
		this.bareHandData.Rebuild();
		if (this.equipped == this.bareHandData)
		{
			this.Reconcile();
		}
	}

	// Token: 0x06002D22 RID: 11554 RVA: 0x00120F46 File Offset: 0x0011F146
	public bool UsingBareHand()
	{
		return this.Held == this.bareHandData;
	}

	// Token: 0x06002D23 RID: 11555 RVA: 0x00120F56 File Offset: 0x0011F156
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

	// Token: 0x06002D24 RID: 11556 RVA: 0x00120F7A File Offset: 0x0011F17A
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

	// Token: 0x06002D25 RID: 11557 RVA: 0x00120F98 File Offset: 0x0011F198
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

	// Token: 0x06002D26 RID: 11558 RVA: 0x00121000 File Offset: 0x0011F200
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

	// Token: 0x06002D27 RID: 11559 RVA: 0x001210A8 File Offset: 0x0011F2A8
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

	// Token: 0x06002D28 RID: 11560 RVA: 0x0012114C File Offset: 0x0011F34C
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

	// Token: 0x06002D29 RID: 11561 RVA: 0x0012121C File Offset: 0x0011F41C
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

	// Token: 0x06002D2A RID: 11562 RVA: 0x001212D8 File Offset: 0x0011F4D8
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

	// Token: 0x06002D2B RID: 11563 RVA: 0x00121474 File Offset: 0x0011F674
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

	// Token: 0x06002D2C RID: 11564 RVA: 0x00121548 File Offset: 0x0011F748
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

	// Token: 0x06002D2D RID: 11565 RVA: 0x00121608 File Offset: 0x0011F808
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

	// Token: 0x06002D2E RID: 11566 RVA: 0x00121640 File Offset: 0x0011F840
	public void ReleaseAll(PlayerActionsLocal _playerActions = null)
	{
		ItemClass item = this.Item;
		for (int i = 0; i < item.Actions.Length; i++)
		{
			this.Execute(i, true, _playerActions);
		}
	}

	// Token: 0x06002D2F RID: 11567 RVA: 0x00121670 File Offset: 0x0011F870
	public bool IsActionRunning()
	{
		return this.Item.IsActionRunning(this.Held);
	}

	// Token: 0x06002D30 RID: 11568 RVA: 0x00121683 File Offset: 0x0011F883
	public bool IsHUDDisabled()
	{
		return this.Item.IsHUDDisabled(this.Held);
	}

	// Token: 0x06002D31 RID: 11569 RVA: 0x00121696 File Offset: 0x0011F896
	public string CanInteract()
	{
		return this.Item.CanInteract(this.Held);
	}

	// Token: 0x06002D32 RID: 11570 RVA: 0x001216A9 File Offset: 0x0011F8A9
	public void Interact()
	{
		this.Item.Interact(this.Held);
	}

	// Token: 0x0400236B RID: 9067
	public static readonly FastTags<TagGroup.Global> IgnoreWhenHeld = FastTags<TagGroup.Global>.Parse("clothing,armor");

	// Token: 0x0400236C RID: 9068
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cHolsterTime = 0.2f;

	// Token: 0x0400236D RID: 9069
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cQuickSwapSettleTime = 0.3f;

	// Token: 0x0400236E RID: 9070
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly EntityAlive entity;

	// Token: 0x0400236F RID: 9071
	[PublicizedFrom(EAccessModifier.Private)]
	public Inventory toolbelt;

	// Token: 0x04002370 RID: 9072
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemInventoryData[] slotDatas;

	// Token: 0x04002371 RID: 9073
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemInventoryData bareHandData;

	// Token: 0x04002372 RID: 9074
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemInventoryData transientData;

	// Token: 0x04002373 RID: 9075
	[PublicizedFrom(EAccessModifier.Private)]
	public Hand.HoldingMode mode;

	// Token: 0x04002374 RID: 9076
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue previousHeldItemValue;

	// Token: 0x04002375 RID: 9077
	[PublicizedFrom(EAccessModifier.Private)]
	public int previousHeldItemSlotIdx = -1;

	// Token: 0x04002376 RID: 9078
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue quickSwapItemValue;

	// Token: 0x04002377 RID: 9079
	[PublicizedFrom(EAccessModifier.Private)]
	public int quickSwapSlotIdx = -1;

	// Token: 0x04002378 RID: 9080
	public bool WaitForSecondaryRelease;

	// Token: 0x04002379 RID: 9081
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSwitching;

	// Token: 0x0400237A RID: 9082
	[PublicizedFrom(EAccessModifier.Private)]
	public Coroutine switchingCoroutine;

	// Token: 0x0400237B RID: 9083
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemInventoryData equipped;

	// Token: 0x0400237C RID: 9084
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemInventoryData holstering;

	// Token: 0x02000572 RID: 1394
	public enum HoldingMode
	{
		// Token: 0x0400237E RID: 9086
		Current,
		// Token: 0x0400237F RID: 9087
		Bare,
		// Token: 0x04002380 RID: 9088
		Transient
	}
}
