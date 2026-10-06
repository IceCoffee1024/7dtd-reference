using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000578 RID: 1400
public sealed class Inventory : InventoryBase
{
	// Token: 0x170004D7 RID: 1239
	// (get) Token: 0x06002D4C RID: 11596 RVA: 0x00121CFA File Offset: 0x0011FEFA
	public int Length
	{
		get
		{
			return this.ItemGrid.Length;
		}
	}

	// Token: 0x14000037 RID: 55
	// (add) Token: 0x06002D4D RID: 11597 RVA: 0x00121D08 File Offset: 0x0011FF08
	// (remove) Token: 0x06002D4E RID: 11598 RVA: 0x00121D40 File Offset: 0x0011FF40
	public event XUiEvent_ToolbeltItemsChangedInternal OnToolbeltItemsChangedInternal;

	// Token: 0x170004D8 RID: 1240
	// (get) Token: 0x06002D4F RID: 11599 RVA: 0x00121D75 File Offset: 0x0011FF75
	public int SlotCount { get; }

	// Token: 0x170004D9 RID: 1241
	// (get) Token: 0x06002D50 RID: 11600 RVA: 0x00121D7D File Offset: 0x0011FF7D
	public ItemStackGrid ItemGrid { get; }

	// Token: 0x170004DA RID: 1242
	// (get) Token: 0x06002D51 RID: 11601 RVA: 0x00121D85 File Offset: 0x0011FF85
	public Hand Hand
	{
		get
		{
			return this.entity.Hand;
		}
	}

	// Token: 0x170004DB RID: 1243
	// (get) Token: 0x06002D52 RID: 11602 RVA: 0x00121D92 File Offset: 0x0011FF92
	// (set) Token: 0x06002D53 RID: 11603 RVA: 0x00121D9A File Offset: 0x0011FF9A
	public int SelectedSlot
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170004DC RID: 1244
	// (get) Token: 0x06002D54 RID: 11604 RVA: 0x00121DA3 File Offset: 0x0011FFA3
	// (set) Token: 0x06002D55 RID: 11605 RVA: 0x00121DAB File Offset: 0x0011FFAB
	public int FocusedSlot
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170004DD RID: 1245
	// (get) Token: 0x06002D56 RID: 11606 RVA: 0x00121DB4 File Offset: 0x0011FFB4
	public ItemValue holdingItemItemValue
	{
		get
		{
			return this.Hand.ItemValue;
		}
	}

	// Token: 0x170004DE RID: 1246
	// (get) Token: 0x06002D57 RID: 11607 RVA: 0x00121DC1 File Offset: 0x0011FFC1
	public ItemStack holdingItemStack
	{
		get
		{
			return this.Hand.Stack;
		}
	}

	// Token: 0x170004DF RID: 1247
	// (get) Token: 0x06002D58 RID: 11608 RVA: 0x00121DCE File Offset: 0x0011FFCE
	public ItemClass holdingItem
	{
		get
		{
			return this.Hand.Item;
		}
	}

	// Token: 0x170004E0 RID: 1248
	// (get) Token: 0x06002D59 RID: 11609 RVA: 0x00121DDB File Offset: 0x0011FFDB
	public int holdingCount
	{
		get
		{
			return this.Hand.Count;
		}
	}

	// Token: 0x170004E1 RID: 1249
	// (get) Token: 0x06002D5A RID: 11610 RVA: 0x00121DE8 File Offset: 0x0011FFE8
	public ItemInventoryData holdingItemData
	{
		get
		{
			return this.Hand.Held;
		}
	}

	// Token: 0x170004E2 RID: 1250
	// (get) Token: 0x06002D5B RID: 11611 RVA: 0x00121DF5 File Offset: 0x0011FFF5
	public int holdingItemIdx
	{
		get
		{
			return this.SelectedSlot;
		}
	}

	// Token: 0x170004E3 RID: 1251
	// (get) Token: 0x06002D5C RID: 11612 RVA: 0x00121DFD File Offset: 0x0011FFFD
	public int holdingItemDataSlotNoBarehandFallback
	{
		get
		{
			return this.Hand.DataSlotNoBarehandFallback;
		}
	}

	// Token: 0x06002D5D RID: 11613 RVA: 0x00121E0C File Offset: 0x0012000C
	public Inventory(EntityAlive _entity, int _slotCount)
	{
		this.entity = _entity;
		this.SlotCount = _slotCount;
		this.ItemGrid = ItemStackGrid.Create(new Vector2i(this.SlotCount, 1), XUiC_ItemStack.StackLocationTypes.ToolBelt, false, false, _entity);
		this.preferredItemTypes = new int[_slotCount];
		this.Hand.BindToolbelt(this);
		this.ItemGrid.SlotChanged += this.OnSlotChanged;
	}

	// Token: 0x06002D5E RID: 11614 RVA: 0x00121E84 File Offset: 0x00120084
	public Inventory(ItemStackGrid _grid, int _selectedSlot)
	{
		this.SlotCount = _grid.Length;
		this.ItemGrid = _grid;
		this.preferredItemTypes = new int[this.SlotCount];
		this.SelectedSlot = _selectedSlot;
		this.FocusedSlot = _selectedSlot;
	}

	// Token: 0x06002D5F RID: 11615 RVA: 0x00121ED4 File Offset: 0x001200D4
	public void SetSelectedSlot(int _slot)
	{
		this.SelectedSlot = _slot;
		this.FocusedSlot = _slot;
	}

	// Token: 0x06002D60 RID: 11616 RVA: 0x00121EE4 File Offset: 0x001200E4
	public ItemStack GetStackAt(int _idx)
	{
		return this.ItemGrid[_idx];
	}

	// Token: 0x06002D61 RID: 11617 RVA: 0x00121EF2 File Offset: 0x001200F2
	public int ChangeSlotCount(int _idx, int _delta)
	{
		return this.ItemGrid.ChangeCount(_idx, _delta);
	}

	// Token: 0x06002D62 RID: 11618 RVA: 0x00121F04 File Offset: 0x00120104
	public void SetStackAt(int _idx, ItemStack _stack)
	{
		ItemStack itemStack = _stack ?? ItemStack.Empty;
		if (_idx < this.preferredItemTypes.Length && itemStack.itemValue.type != 0 && itemStack.count != 0)
		{
			this.preferredItemTypes[_idx] = itemStack.itemValue.type;
		}
		this.ItemGrid.SetItem(_idx, itemStack);
	}

	// Token: 0x06002D63 RID: 11619 RVA: 0x00121F5C File Offset: 0x0012015C
	public List<ItemStack> ReadInto(PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		_br.ReadByte();
		this.isReading = true;
		List<ItemStack> list;
		try
		{
			this.ItemGrid.ReadInto(_br, _eStreamMode);
			list = this.ItemGrid.Resize(new Vector2i(this.SlotCount, 1));
		}
		finally
		{
			this.isReading = false;
		}
		this.SetSelectedSlot(Utils.FastClamp((int)_br.ReadByte(), 0, this.SlotCount - 1));
		this.Hand.BindToolbelt(this);
		this.SetHoldingItemIdx(this.SelectedSlot);
		return list;
	}

	// Token: 0x06002D64 RID: 11620 RVA: 0x00121FEC File Offset: 0x001201EC
	public void Write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		_bw.Write(1);
		this.ItemGrid.Write(_bw, _eStreamMode);
		_bw.Write((byte)this.SelectedSlot);
	}

	// Token: 0x06002D65 RID: 11621 RVA: 0x0012200F File Offset: 0x0012020F
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnSlotChanged(int _idx)
	{
		if (this.isReading)
		{
			return;
		}
		this.CallOnToolbeltChangedInternal();
	}

	// Token: 0x06002D66 RID: 11622 RVA: 0x00122020 File Offset: 0x00120220
	[PublicizedFrom(EAccessModifier.Private)]
	public int FindPreferredSlot(int _itemType, int _startSlotIdx)
	{
		for (int i = _startSlotIdx; i < this.preferredItemTypes.Length; i++)
		{
			if (this.preferredItemTypes[i] == _itemType)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06002D67 RID: 11623 RVA: 0x0012204E File Offset: 0x0012024E
	public void CallOnToolbeltChangedInternal()
	{
		if (this.OnToolbeltItemsChangedInternal != null)
		{
			this.OnToolbeltItemsChangedInternal();
		}
	}

	// Token: 0x06002D68 RID: 11624 RVA: 0x00122064 File Offset: 0x00120264
	public void SetSlots(ItemStack[] _slots)
	{
		int num = 0;
		while (num < _slots.Length && num < this.Length)
		{
			this.SetItem(num, _slots[num].itemValue, _slots[num].count);
			num++;
		}
	}

	// Token: 0x06002D69 RID: 11625 RVA: 0x0012209F File Offset: 0x0012029F
	public ItemClass GetItemInSlot(int _idx)
	{
		return this.GetStackAt(_idx).itemValue.ItemClass;
	}

	// Token: 0x06002D6A RID: 11626 RVA: 0x001220B2 File Offset: 0x001202B2
	public ItemInventoryData GetItemDataInSlot(int _idx)
	{
		return this.Hand.GetSlotData(_idx);
	}

	// Token: 0x170004E4 RID: 1252
	public ItemStack this[int _idx]
	{
		get
		{
			return this.GetStackAt(_idx);
		}
		set
		{
			this.SetStackAt(_idx, value);
		}
	}

	// Token: 0x06002D6D RID: 11629 RVA: 0x001220C0 File Offset: 0x001202C0
	public ItemStack GetItemStack(int _idx)
	{
		return this.GetStackAt(_idx);
	}

	// Token: 0x06002D6E RID: 11630 RVA: 0x001220D4 File Offset: 0x001202D4
	public void ModifyValue(ItemValue _originalItemValue, PassiveEffects _passiveEffect, ref float _base_val, ref float _perc_val, FastTags<TagGroup.Global> tags)
	{
		if (this.holdingItemItemValue != null && !this.holdingItemItemValue.Equals(_originalItemValue) && !this.holdingItemItemValue.ItemClass.ItemTags.Test_AnySet(Hand.IgnoreWhenHeld))
		{
			this.holdingItemItemValue.ModifyValue(this.entity, _originalItemValue, _passiveEffect, ref _base_val, ref _perc_val, tags, true, false);
		}
	}

	// Token: 0x06002D6F RID: 11631 RVA: 0x0012212D File Offset: 0x0012032D
	public bool IsHandBusy()
	{
		return this.Hand.IsBusy;
	}

	// Token: 0x06002D70 RID: 11632 RVA: 0x0012213A File Offset: 0x0012033A
	public bool IsHandSwitching()
	{
		return this.Hand.IsSwitching;
	}

	// Token: 0x06002D71 RID: 11633 RVA: 0x00122147 File Offset: 0x00120347
	public int GetFocusedItemIdx()
	{
		return this.FocusedSlot;
	}

	// Token: 0x06002D72 RID: 11634 RVA: 0x0012214F File Offset: 0x0012034F
	public int SetFocusedItemIdx(int _idx)
	{
		this.FocusedSlot = _idx;
		return _idx;
	}

	// Token: 0x06002D73 RID: 11635 RVA: 0x00122159 File Offset: 0x00120359
	public bool IsHoldingItemActionRunning()
	{
		return this.Hand.IsActionRunning();
	}

	// Token: 0x06002D74 RID: 11636 RVA: 0x00122166 File Offset: 0x00120366
	public void SetHoldingItemIdxNoHolsterTime(int _inventoryIdx)
	{
		this.Hand.SelectSlot(_inventoryIdx, false);
	}

	// Token: 0x06002D75 RID: 11637 RVA: 0x00122175 File Offset: 0x00120375
	public void SetHoldingItemIdx(int _inventoryIdx)
	{
		this.Hand.SelectSlot(_inventoryIdx, true);
	}

	// Token: 0x06002D76 RID: 11638 RVA: 0x00122184 File Offset: 0x00120384
	public void ReconcileHeldItem()
	{
		this.Hand.Reconcile();
	}

	// Token: 0x06002D77 RID: 11639 RVA: 0x00122191 File Offset: 0x00120391
	public Transform GetHoldingItemTransform()
	{
		return this.Hand.ModelTransform;
	}

	// Token: 0x06002D78 RID: 11640 RVA: 0x0012219E File Offset: 0x0012039E
	public int GetBestQuickSwapSlot()
	{
		return this.Hand.GetBestQuickSwapSlot();
	}

	// Token: 0x06002D79 RID: 11641 RVA: 0x001221AB File Offset: 0x001203AB
	public void Execute(int _actionIdx, bool _bReleased, PlayerActionsLocal _playerActions = null)
	{
		this.Hand.Execute(_actionIdx, _bReleased, _playerActions);
	}

	// Token: 0x06002D7A RID: 11642 RVA: 0x001221BB File Offset: 0x001203BB
	public void ReleaseAll(PlayerActionsLocal _playerActions = null)
	{
		this.Hand.ReleaseAll(_playerActions);
	}

	// Token: 0x06002D7B RID: 11643 RVA: 0x001221C9 File Offset: 0x001203C9
	public bool IsHUDDisabled()
	{
		return this.Hand.IsHUDDisabled();
	}

	// Token: 0x06002D7C RID: 11644 RVA: 0x001221D6 File Offset: 0x001203D6
	public string CanInteract()
	{
		return this.Hand.CanInteract();
	}

	// Token: 0x06002D7D RID: 11645 RVA: 0x001221E3 File Offset: 0x001203E3
	public void Interact()
	{
		this.Hand.Interact();
	}

	// Token: 0x06002D7E RID: 11646 RVA: 0x001221F0 File Offset: 0x001203F0
	public bool IsHoldingGun()
	{
		return this.holdingItem != null && this.holdingItem.IsGun();
	}

	// Token: 0x06002D7F RID: 11647 RVA: 0x00122207 File Offset: 0x00120407
	public bool IsHoldingDynamicMelee()
	{
		return this.holdingItem != null && this.holdingItem.IsDynamicMelee();
	}

	// Token: 0x06002D80 RID: 11648 RVA: 0x0012221E File Offset: 0x0012041E
	public bool IsHoldingBlock()
	{
		return this.holdingItem != null && this.holdingItem.IsBlock();
	}

	// Token: 0x06002D81 RID: 11649 RVA: 0x00122235 File Offset: 0x00120435
	public ItemAction GetHoldingPrimary()
	{
		return this.holdingItem.Actions[0];
	}

	// Token: 0x06002D82 RID: 11650 RVA: 0x00122244 File Offset: 0x00120444
	public ItemAction GetHoldingSecondary()
	{
		return this.holdingItem.Actions[1];
	}

	// Token: 0x06002D83 RID: 11651 RVA: 0x00122253 File Offset: 0x00120453
	public ItemActionAttack GetHoldingGun()
	{
		return this.holdingItem.Actions[0] as ItemActionAttack;
	}

	// Token: 0x06002D84 RID: 11652 RVA: 0x00122267 File Offset: 0x00120467
	public ItemActionDynamic GetHoldingDynamicMelee()
	{
		return this.holdingItem.Actions[0] as ItemActionDynamic;
	}

	// Token: 0x06002D85 RID: 11653 RVA: 0x0012227B File Offset: 0x0012047B
	public ItemClassBlock GetHoldingBlock()
	{
		return this.holdingItem as ItemClassBlock;
	}

	// Token: 0x06002D86 RID: 11654 RVA: 0x00122288 File Offset: 0x00120488
	public int GetItemCount(ItemValue _itemValue, bool _bConsiderTexture = false, int _seed = -1, int _meta = -1, bool _ignoreModdedItems = true)
	{
		int num = 0;
		for (int i = 0; i < this.Length; i++)
		{
			ItemStack stackAt = this.GetStackAt(i);
			if ((!_ignoreModdedItems || !stackAt.itemValue.HasModSlots || !stackAt.itemValue.HasMods()) && stackAt.itemValue.type == _itemValue.type && (!_bConsiderTexture || stackAt.itemValue.TextureFullArray == _itemValue.TextureFullArray) && (_seed == -1 || _seed == (int)stackAt.itemValue.Seed) && (_meta == -1 || _meta == stackAt.itemValue.Meta))
			{
				num += stackAt.count;
			}
		}
		return num;
	}

	// Token: 0x06002D87 RID: 11655 RVA: 0x00122334 File Offset: 0x00120534
	public int GetItemCount(FastTags<TagGroup.Global> itemTags, int _seed = -1, int _meta = -1, bool _ignoreModdedItems = true)
	{
		int num = 0;
		for (int i = 0; i < this.Length; i++)
		{
			ItemStack stackAt = this.GetStackAt(i);
			if (stackAt.itemValue.ItemClass != null && (!_ignoreModdedItems || !stackAt.itemValue.HasModSlots || !stackAt.itemValue.HasMods()) && !stackAt.itemValue.IsEmpty() && stackAt.itemValue.ItemClass.ItemTags.Test_AnySet(itemTags) && (_seed == -1 || _seed == (int)stackAt.itemValue.Seed) && (_meta == -1 || _meta == stackAt.itemValue.Meta))
			{
				num += stackAt.count;
			}
		}
		return num;
	}

	// Token: 0x06002D88 RID: 11656 RVA: 0x001223E0 File Offset: 0x001205E0
	public override bool AddItem(ItemStack _itemStack)
	{
		int num;
		return this.AddItem(_itemStack, out num);
	}

	// Token: 0x06002D89 RID: 11657 RVA: 0x001223F6 File Offset: 0x001205F6
	public bool AddItem(ItemStack _itemStack, out int _slot)
	{
		return this.AddItem(_itemStack, 0, this.Length, out _slot);
	}

	// Token: 0x06002D8A RID: 11658 RVA: 0x00122408 File Offset: 0x00120608
	public bool AddItem(ItemStack _itemStack, int _startSlot, int _slotCount, out int _slot)
	{
		if (!_itemStack.CanMoveTo(XUiC_ItemStack.StackLocationTypes.ToolBelt, -1))
		{
			_slot = -1;
			return false;
		}
		if (_startSlot < 0)
		{
			_startSlot = 0;
		}
		int num = Utils.FastMin(_startSlot + _slotCount, this.Length);
		for (int i = _startSlot; i < num; i++)
		{
			if (this.GetStackAt(i).itemValue.type == _itemStack.itemValue.type && this.GetStackAt(i).CanStackWith(_itemStack, false))
			{
				this.ChangeSlotCount(i, _itemStack.count);
				this.entity.bPlayerStatsChanged = !this.entity.isEntityRemote;
				_slot = i;
				return true;
			}
		}
		for (int j = _startSlot; j < num; j++)
		{
			if (this.GetStackAt(j).IsEmpty())
			{
				this.SetItem(j, _itemStack.itemValue, _itemStack.count);
				this.entity.bPlayerStatsChanged = !this.entity.isEntityRemote;
				_slot = j;
				return true;
			}
		}
		_slot = -1;
		return false;
	}

	// Token: 0x06002D8B RID: 11659 RVA: 0x001224F4 File Offset: 0x001206F4
	public bool ReturnItem(ItemStack _itemStack)
	{
		for (int i = 0; i < this.Length; i++)
		{
			i = this.FindPreferredSlot(_itemStack.itemValue.type, i);
			if (i < 0 || i >= this.Length)
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

	// Token: 0x06002D8C RID: 11660 RVA: 0x00122544 File Offset: 0x00120744
	public bool AddItemAtSlot(ItemStack _itemStack, int _slot)
	{
		bool flag = false;
		if (_slot >= 0 && _slot < this.Length)
		{
			if (this.GetStackAt(_slot).itemValue.type == _itemStack.itemValue.type && this.GetStackAt(_slot).CanStackWith(_itemStack, false))
			{
				this.ChangeSlotCount(_slot, _itemStack.count);
				flag = true;
			}
			if (this.GetStackAt(_slot).IsEmpty() && this.CanMoveToSlot(_itemStack, _slot))
			{
				this.SetItem(_slot, _itemStack.itemValue, _itemStack.count);
				flag = true;
			}
		}
		if (flag)
		{
			this.entity.bPlayerStatsChanged = !this.entity.isEntityRemote;
		}
		return flag;
	}

	// Token: 0x06002D8D RID: 11661 RVA: 0x001225E8 File Offset: 0x001207E8
	public int DecItem(ItemValue _itemValue, int _count, bool _ignoreModdedItems = false, IList<ItemStack> _removedItems = null)
	{
		int num = _count;
		int num2 = 0;
		while (_count > 0 && num2 < this.Length)
		{
			if (this.GetStackAt(num2).itemValue.type == _itemValue.type && (!_ignoreModdedItems || !this.GetStackAt(num2).itemValue.HasModSlots || !this.GetStackAt(num2).itemValue.HasMods()))
			{
				if (ItemClass.GetForId(this.GetStackAt(num2).itemValue.type).CanStack())
				{
					int count = this.GetStackAt(num2).count;
					int num3 = ((count >= _count) ? _count : count);
					if (_removedItems != null)
					{
						_removedItems.Add(new ItemStack(this.GetStackAt(num2).itemValue.Clone(), num3));
					}
					_count += this.ChangeSlotCount(num2, -num3);
				}
				else
				{
					if (_removedItems != null)
					{
						_removedItems.Add(this.GetStackAt(num2).Clone());
					}
					this.clearSlotByIndex(num2);
					_count--;
				}
			}
			num2++;
		}
		return num - _count;
	}

	// Token: 0x06002D8E RID: 11662 RVA: 0x001226E4 File Offset: 0x001208E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void clearSlotByIndex(int _idx)
	{
		this.SetStackAt(_idx, ItemStack.Empty);
	}

	// Token: 0x06002D8F RID: 11663 RVA: 0x001226F2 File Offset: 0x001208F2
	public void SetItem(int _idx, ItemStack _itemStack)
	{
		this.SetItem(_idx, _itemStack.itemValue, _itemStack.count);
	}

	// Token: 0x06002D90 RID: 11664 RVA: 0x00122708 File Offset: 0x00120908
	public void SetItem(int _idx, ItemValue _itemValue, int _count)
	{
		if ((ulong)_idx >= (ulong)((long)this.Length))
		{
			return;
		}
		if (_itemValue.type != 0 && _itemValue.ItemClass == null)
		{
			Log.Warning("Inventory slot {0} {1} missing item class", new object[] { _idx, _itemValue.type });
			_itemValue = ItemValue.None;
		}
		if (_count == 0)
		{
			_itemValue = ItemValue.None;
		}
		this.SetStackAt(_idx, new ItemStack(_itemValue, _count));
	}

	// Token: 0x06002D91 RID: 11665 RVA: 0x00122778 File Offset: 0x00120978
	public void FireEvent(MinEventTypes _eventType, MinEventParams _eventParms)
	{
		ItemValue itemValue = _eventParms.ItemValue;
		if (itemValue == null)
		{
			itemValue = this.holdingItemItemValue;
		}
		itemValue.FireEvent(_eventType, _eventParms);
	}

	// Token: 0x06002D92 RID: 11666 RVA: 0x001220C0 File Offset: 0x001202C0
	public ItemStack GetItem(int _idx)
	{
		return this.GetStackAt(_idx);
	}

	// Token: 0x06002D93 RID: 11667 RVA: 0x001227A0 File Offset: 0x001209A0
	public bool DecHoldingItem(int _count)
	{
		if (this.Hand.UsingBareHand())
		{
			return true;
		}
		ItemStack itemStack = this.holdingItemData.itemStack;
		int num = (this.holdingItem.CanStack() ? Utils.FastMin(_count, itemStack.count) : itemStack.count);
		if (itemStack.count - num <= 0)
		{
			itemStack.Clear();
		}
		else
		{
			itemStack.count -= num;
		}
		if (!itemStack.IsEmpty())
		{
			this.holdingItem.OnHoldingReset(this.holdingItemData);
		}
		this.Hand.Reconcile();
		return true;
	}

	// Token: 0x06002D94 RID: 11668 RVA: 0x00122830 File Offset: 0x00120A30
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

	// Token: 0x06002D95 RID: 11669 RVA: 0x001228FA File Offset: 0x00120AFA
	public IEnumerator SimulateActionExecution(int _actionIdx, ItemStackGrid _sourceGrid, int _sourceIdx)
	{
		return this.Hand.SimulateActionExecution(_actionIdx, _sourceGrid, _sourceIdx);
	}

	// Token: 0x06002D96 RID: 11670 RVA: 0x0012290C File Offset: 0x00120B0C
	public bool CanStack(ItemStack _itemStack)
	{
		for (int i = 0; i < this.Length; i++)
		{
			if (this.CanMoveToSlot(_itemStack, i) && (this.GetStackAt(i).IsEmpty() || this.GetStackAt(i).CanStackWith(_itemStack, false)))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002D97 RID: 11671 RVA: 0x00122958 File Offset: 0x00120B58
	public bool CanStackNoEmpty(ItemStack _itemStack)
	{
		for (int i = 0; i < this.Length; i++)
		{
			int num;
			if (this.GetStackAt(i).CanStackPartlyWith(_itemStack, out num))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002D98 RID: 11672 RVA: 0x0012298C File Offset: 0x00120B8C
	[return: TupleElementNames(new string[] { "anyMoved", "allMoved" })]
	public override ValueTuple<bool, bool> TryStackItem(int startIndex, ItemStack _itemStack)
	{
		bool flag = false;
		bool flag2 = false;
		for (int i = startIndex; i < this.Length; i++)
		{
			if (this.CanMoveToSlot(_itemStack, i))
			{
				int count = _itemStack.count;
				ItemStack stackAt = this.GetStackAt(i);
				if (_itemStack.itemValue.type == stackAt.itemValue.type && !stackAt.IsEmpty() && stackAt.CanStackPartly(ref count))
				{
					flag = true;
					_itemStack.count -= this.ChangeSlotCount(i, count);
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

	// Token: 0x06002D99 RID: 11673 RVA: 0x00122A40 File Offset: 0x00120C40
	public bool CanTakeItem(ItemStack _itemStack)
	{
		for (int i = 0; i < this.Length; i++)
		{
			int num;
			if (this.GetStackAt(i).CanStackPartlyWith(_itemStack, out num))
			{
				return true;
			}
			if (this.GetStackAt(i).IsEmpty())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002D9A RID: 11674 RVA: 0x00122A82 File Offset: 0x00120C82
	public void Cleanup()
	{
		this.ItemGrid.SlotChanged -= this.OnSlotChanged;
		this.entity = null;
	}

	// Token: 0x06002D9B RID: 11675 RVA: 0x00122AA4 File Offset: 0x00120CA4
	public override void PerformActionOnSlots(Action<ItemStack> _action)
	{
		for (int i = 0; i < this.Length; i++)
		{
			_action(this.GetStackAt(i));
		}
	}

	// Token: 0x06002D9C RID: 11676 RVA: 0x00122ACF File Offset: 0x00120CCF
	public override bool HasItem(ItemValue _item)
	{
		return this.GetItemCount(_item, false, -1, -1, true) > 0;
	}

	// Token: 0x06002D9D RID: 11677 RVA: 0x00122ADF File Offset: 0x00120CDF
	public bool CanMoveToSlot(ItemStack _itemStack, int _slotIdx)
	{
		return _itemStack.itemValue.ItemClass == null || _itemStack.itemValue.ItemClass.CanMoveToSlot(XUiC_ItemStack.StackLocationTypes.Backpack, _slotIdx);
	}

	// Token: 0x04002391 RID: 9105
	public const byte Version = 1;

	// Token: 0x04002392 RID: 9106
	public EntityAlive entity;

	// Token: 0x04002394 RID: 9108
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] preferredItemTypes;

	// Token: 0x04002395 RID: 9109
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isReading;

	// Token: 0x0400239A RID: 9114
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ItemValue> activatables = new List<ItemValue>();
}
