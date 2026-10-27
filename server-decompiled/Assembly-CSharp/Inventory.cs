using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BhvrAnalyticsServices.Interfaces;
using Services;
using Services.Analytics;
using Services.Analytics.Events;
using UnityEngine;

// Token: 0x0200057F RID: 1407
public sealed class Inventory : InventoryBase
{
	// Token: 0x170004D7 RID: 1239
	// (get) Token: 0x06002D57 RID: 11607 RVA: 0x001222BA File Offset: 0x001204BA
	public int Length
	{
		get
		{
			return this.ItemGrid.Length;
		}
	}

	// Token: 0x14000037 RID: 55
	// (add) Token: 0x06002D58 RID: 11608 RVA: 0x001222C8 File Offset: 0x001204C8
	// (remove) Token: 0x06002D59 RID: 11609 RVA: 0x00122300 File Offset: 0x00120500
	public event XUiEvent_ToolbeltItemsChangedInternal OnToolbeltItemsChangedInternal;

	// Token: 0x170004D8 RID: 1240
	// (get) Token: 0x06002D5A RID: 11610 RVA: 0x00122335 File Offset: 0x00120535
	public int SlotCount { get; }

	// Token: 0x170004D9 RID: 1241
	// (get) Token: 0x06002D5B RID: 11611 RVA: 0x0012233D File Offset: 0x0012053D
	public ItemStackGrid ItemGrid { get; }

	// Token: 0x170004DA RID: 1242
	// (get) Token: 0x06002D5C RID: 11612 RVA: 0x00122345 File Offset: 0x00120545
	public Hand Hand
	{
		get
		{
			return this.entity.Hand;
		}
	}

	// Token: 0x170004DB RID: 1243
	// (get) Token: 0x06002D5D RID: 11613 RVA: 0x00122352 File Offset: 0x00120552
	// (set) Token: 0x06002D5E RID: 11614 RVA: 0x0012235A File Offset: 0x0012055A
	public int SelectedSlot
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170004DC RID: 1244
	// (get) Token: 0x06002D5F RID: 11615 RVA: 0x00122363 File Offset: 0x00120563
	// (set) Token: 0x06002D60 RID: 11616 RVA: 0x0012236B File Offset: 0x0012056B
	public int FocusedSlot
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170004DD RID: 1245
	// (get) Token: 0x06002D61 RID: 11617 RVA: 0x00122374 File Offset: 0x00120574
	public ItemValue holdingItemItemValue
	{
		get
		{
			return this.Hand.ItemValue;
		}
	}

	// Token: 0x170004DE RID: 1246
	// (get) Token: 0x06002D62 RID: 11618 RVA: 0x00122381 File Offset: 0x00120581
	public ItemStack holdingItemStack
	{
		get
		{
			return this.Hand.Stack;
		}
	}

	// Token: 0x170004DF RID: 1247
	// (get) Token: 0x06002D63 RID: 11619 RVA: 0x0012238E File Offset: 0x0012058E
	public ItemClass holdingItem
	{
		get
		{
			return this.Hand.Item;
		}
	}

	// Token: 0x170004E0 RID: 1248
	// (get) Token: 0x06002D64 RID: 11620 RVA: 0x0012239B File Offset: 0x0012059B
	public int holdingCount
	{
		get
		{
			return this.Hand.Count;
		}
	}

	// Token: 0x170004E1 RID: 1249
	// (get) Token: 0x06002D65 RID: 11621 RVA: 0x001223A8 File Offset: 0x001205A8
	public ItemInventoryData holdingItemData
	{
		get
		{
			return this.Hand.Held;
		}
	}

	// Token: 0x170004E2 RID: 1250
	// (get) Token: 0x06002D66 RID: 11622 RVA: 0x001223B5 File Offset: 0x001205B5
	public int holdingItemIdx
	{
		get
		{
			return this.SelectedSlot;
		}
	}

	// Token: 0x170004E3 RID: 1251
	// (get) Token: 0x06002D67 RID: 11623 RVA: 0x001223BD File Offset: 0x001205BD
	public int holdingItemDataSlotNoBarehandFallback
	{
		get
		{
			return this.Hand.DataSlotNoBarehandFallback;
		}
	}

	// Token: 0x06002D68 RID: 11624 RVA: 0x001223CC File Offset: 0x001205CC
	public Inventory(EntityAlive _entity, int _slotCount)
	{
		this.entity = _entity;
		this.SlotCount = _slotCount;
		this.ItemGrid = ItemStackGrid.Create(new Vector2i(this.SlotCount, 1), XUiC_ItemStack.StackLocationTypes.ToolBelt, false, false, _entity);
		this.preferredItemTypes = new int[_slotCount];
		this.Hand.BindToolbelt(this);
		this.ItemGrid.SlotChanged += this.OnSlotChanged;
	}

	// Token: 0x06002D69 RID: 11625 RVA: 0x00122444 File Offset: 0x00120644
	public Inventory(ItemStackGrid _grid, int _selectedSlot)
	{
		this.SlotCount = _grid.Length;
		this.ItemGrid = _grid;
		this.preferredItemTypes = new int[this.SlotCount];
		this.SelectedSlot = _selectedSlot;
		this.FocusedSlot = _selectedSlot;
	}

	// Token: 0x06002D6A RID: 11626 RVA: 0x00122494 File Offset: 0x00120694
	public void SetSelectedSlot(int _slot)
	{
		this.SelectedSlot = _slot;
		this.FocusedSlot = _slot;
	}

	// Token: 0x06002D6B RID: 11627 RVA: 0x001224A4 File Offset: 0x001206A4
	public ItemStack GetStackAt(int _idx)
	{
		return this.ItemGrid[_idx];
	}

	// Token: 0x06002D6C RID: 11628 RVA: 0x001224B2 File Offset: 0x001206B2
	public int ChangeSlotCount(int _idx, int _delta)
	{
		return this.ItemGrid.ChangeCount(_idx, _delta);
	}

	// Token: 0x06002D6D RID: 11629 RVA: 0x001224C4 File Offset: 0x001206C4
	public void SetStackAt(int _idx, ItemStack _stack)
	{
		ItemStack itemStack = _stack ?? ItemStack.Empty;
		if (_idx < this.preferredItemTypes.Length && itemStack.itemValue.type != 0 && itemStack.count != 0)
		{
			this.preferredItemTypes[_idx] = itemStack.itemValue.type;
		}
		if (_stack != null && !_stack.itemValue.Equals(this.ItemGrid.GetItem(_idx).itemValue) && this.entity is EntityPlayerLocal)
		{
			if (_stack.itemValue.ItemClass == null)
			{
				this.LogToolBeltUsageEvent(null, this.ItemGrid.GetItem(_idx).itemValue, _idx, _stack.count);
			}
			else
			{
				this.LogToolBeltUsageEvent(_stack.itemValue, this.ItemGrid.GetItem(_idx).itemValue, _idx, _stack.count);
			}
		}
		this.ItemGrid.SetItem(_idx, itemStack);
	}

	// Token: 0x06002D6E RID: 11630 RVA: 0x0012259C File Offset: 0x0012079C
	[PublicizedFrom(EAccessModifier.Private)]
	public void LogToolBeltUsageEvent(ItemValue itemValue, ItemValue previousItemValue, int index, int stack_count)
	{
		EntityPlayerLocal entityPlayerLocal = this.entity as EntityPlayerLocal;
		if (entityPlayerLocal == null)
		{
			return;
		}
		if (Helper.IsEditingOrPlaytesting())
		{
			return;
		}
		List<ToolBeltUsageEventData.StatsData> list = new List<ToolBeltUsageEventData.StatsData>();
		if (((itemValue != null) ? itemValue.Stats : null) != null)
		{
			foreach (ItemValue.Stat stat in itemValue.Stats)
			{
				list.Add(new ToolBeltUsageEventData.StatsData(stat.type.ToString(), stat.isBoosted, stat.value));
			}
		}
		List<ToolBeltUsageEventData.ModificationData> list2 = new List<ToolBeltUsageEventData.ModificationData>();
		int num = 0;
		while (itemValue != null && num < itemValue.ModificationCount)
		{
			ItemValue modification = itemValue.GetModification(num);
			if (modification != null && !modification.IsEmpty())
			{
				List<ToolBeltUsageEventData.ModificationData> list3 = list2;
				ItemClass itemClass = modification.ItemClass;
				list3.Add(new ToolBeltUsageEventData.ModificationData((itemClass != null) ? itemClass.Name : null, (modification.Quality != 0) ? new int?((int)modification.Quality) : null));
			}
			num++;
		}
		Dictionary<ValueTuple<PassiveEffects, PassiveEffect.ValueModifierTypes>, float> dictionary = new Dictionary<ValueTuple<PassiveEffects, PassiveEffect.ValueModifierTypes>, float>();
		MinEffectController minEffectController;
		if (itemValue == null)
		{
			minEffectController = null;
		}
		else
		{
			ItemClass itemClass2 = itemValue.ItemClass;
			minEffectController = ((itemClass2 != null) ? itemClass2.Effects : null);
		}
		MinEffectController minEffectController2 = minEffectController;
		if (minEffectController2 != null)
		{
			foreach (MinEffectGroup minEffectGroup in minEffectController2.EffectGroups)
			{
				foreach (PassiveEffect passiveEffect in minEffectGroup.PassiveEffects)
				{
					if (!dictionary.ContainsKey(new ValueTuple<PassiveEffects, PassiveEffect.ValueModifierTypes>(passiveEffect.Type, passiveEffect.Modifier)))
					{
						dictionary.Add(new ValueTuple<PassiveEffects, PassiveEffect.ValueModifierTypes>(passiveEffect.Type, passiveEffect.Modifier), (float)Math.Round((double)EffectManager.GetValue(passiveEffect.Type, itemValue, 1f, null, null, passiveEffect.Tags, true, true, true, true, true, 1, true, false), 3));
					}
				}
			}
		}
		Dictionary<string, float> dictionary2 = new Dictionary<string, float>();
		bool flag;
		if (itemValue == null)
		{
			flag = null != null;
		}
		else
		{
			ItemClass itemClass3 = itemValue.ItemClass;
			flag = ((itemClass3 != null) ? itemClass3.Effects : null) != null;
		}
		if (flag)
		{
			ItemClass itemClass4 = itemValue.ItemClass;
			IReadOnlyList<MinEventActionBase> readOnlyList = ((itemClass4 != null) ? itemClass4.Effects.EffectGroups[0].GetTriggeredEffects(MinEventTypes.onSelfPrimaryActionEnd) : null);
			if (readOnlyList != null)
			{
				foreach (MinEventActionBase minEventActionBase in readOnlyList)
				{
					MinEventActionModifyCVar minEventActionModifyCVar = minEventActionBase as MinEventActionModifyCVar;
					if (minEventActionModifyCVar != null && !dictionary2.ContainsKey(minEventActionModifyCVar.cvarName))
					{
						dictionary2.Add(minEventActionModifyCVar.cvarName, minEventActionModifyCVar.value);
					}
				}
			}
		}
		Dictionary<string, float> dictionary3 = new Dictionary<string, float>();
		bool flag2;
		if (itemValue == null)
		{
			flag2 = null != null;
		}
		else
		{
			ItemClass itemClass5 = itemValue.ItemClass;
			flag2 = ((itemClass5 != null) ? itemClass5.Effects : null) != null;
		}
		if (flag2)
		{
			foreach (KeyValuePair<string, EffectDisplayValue> keyValuePair in itemValue.ItemClass.Effects.EffectGroups[0].EffectDisplayValues)
			{
				dictionary3.Add(keyValuePair.Key, keyValuePair.Value.Values[0]);
			}
		}
		string text;
		if (previousItemValue == null)
		{
			text = null;
		}
		else
		{
			ItemClass itemClass6 = previousItemValue.ItemClass;
			text = ((itemClass6 != null) ? itemClass6.GetItemName() : null);
		}
		string text2 = text;
		List<KeyValuePair<string, float>> list4 = ((dictionary2 != null) ? dictionary2.Concat<KeyValuePair<string, float>>(dictionary3).ToList<KeyValuePair<string, float>>() : null);
		ToolBeltUsageEventData toolBeltUsageEventData = new ToolBeltUsageEventData();
		toolBeltUsageEventData.ServerId = Helper.GetServerId();
		toolBeltUsageEventData.SaveId = Helper.GetSaveId();
		toolBeltUsageEventData.ToolbeltChangeTimestamp = DateTime.UtcNow.ToString("O");
		toolBeltUsageEventData.PlayerLevel = entityPlayerLocal.Progression.Level;
		toolBeltUsageEventData.PersonalGameStage = entityPlayerLocal.gameStage;
		toolBeltUsageEventData.ToolbeltSlot = index + 1;
		string text3;
		if (itemValue == null)
		{
			text3 = null;
		}
		else
		{
			ItemClass itemClass7 = itemValue.ItemClass;
			text3 = ((itemClass7 != null) ? itemClass7.GetItemName() : null);
		}
		toolBeltUsageEventData.ItemName = text3;
		toolBeltUsageEventData.Stats = ((list.Count > 0) ? list : null);
		toolBeltUsageEventData.PassiveEffects = ((dictionary.Count > 0) ? dictionary : null);
		ushort? num2 = ((itemValue != null) ? new ushort?(itemValue.Quality) : null);
		toolBeltUsageEventData.Quality = ((num2 != null) ? new int?((int)num2.GetValueOrDefault()) : null);
		toolBeltUsageEventData.ModSlots = ((list2.Count > 0) ? list2 : null);
		object obj;
		if (itemValue == null)
		{
			obj = null;
		}
		else
		{
			ItemClass itemClass8 = itemValue.ItemClass;
			obj = ((itemClass8 != null) ? itemClass8.ItemTags.GetTagNames() : null);
		}
		toolBeltUsageEventData.Tags = obj;
		toolBeltUsageEventData.CurrentDegradation = ((itemValue != null) ? new float?(itemValue.UseTimes) : null);
		string text4;
		if (itemValue == null)
		{
			text4 = null;
		}
		else
		{
			ItemClass itemClass9 = itemValue.ItemClass;
			text4 = ((itemClass9 != null) ? itemClass9.DisplayType : null);
		}
		toolBeltUsageEventData.ItemType = text4;
		toolBeltUsageEventData.StackSize = ((stack_count != 0) ? new int?(stack_count) : null);
		toolBeltUsageEventData.PreviousItemName = text2;
		object obj2;
		if (itemValue == null)
		{
			obj2 = null;
		}
		else
		{
			ItemClass itemClass10 = itemValue.ItemClass;
			obj2 = ((itemClass10 != null) ? itemClass10.Groups : null);
		}
		toolBeltUsageEventData.Groups = obj2;
		toolBeltUsageEventData.Properties = ((list4.Count > 0) ? list4 : null);
		ToolBeltUsageEventData toolBeltUsageEventData2 = toolBeltUsageEventData;
		ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(toolBeltUsageEventData2);
	}

	// Token: 0x06002D6F RID: 11631 RVA: 0x00122AD8 File Offset: 0x00120CD8
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

	// Token: 0x06002D70 RID: 11632 RVA: 0x00122B68 File Offset: 0x00120D68
	public void Write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		_bw.Write(1);
		this.ItemGrid.Write(_bw, _eStreamMode);
		_bw.Write((byte)this.SelectedSlot);
	}

	// Token: 0x06002D71 RID: 11633 RVA: 0x00122B8B File Offset: 0x00120D8B
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnSlotChanged(int _idx)
	{
		if (this.isReading)
		{
			return;
		}
		this.CallOnToolbeltChangedInternal();
	}

	// Token: 0x06002D72 RID: 11634 RVA: 0x00122B9C File Offset: 0x00120D9C
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

	// Token: 0x06002D73 RID: 11635 RVA: 0x00122BCA File Offset: 0x00120DCA
	public void CallOnToolbeltChangedInternal()
	{
		if (this.OnToolbeltItemsChangedInternal != null)
		{
			this.OnToolbeltItemsChangedInternal();
		}
	}

	// Token: 0x06002D74 RID: 11636 RVA: 0x00122BE0 File Offset: 0x00120DE0
	public void SetSlots(ItemStack[] _slots)
	{
		int num = 0;
		while (num < _slots.Length && num < this.Length)
		{
			this.SetItem(num, _slots[num].itemValue, _slots[num].count);
			num++;
		}
	}

	// Token: 0x06002D75 RID: 11637 RVA: 0x00122C1B File Offset: 0x00120E1B
	public ItemClass GetItemInSlot(int _idx)
	{
		return this.GetStackAt(_idx).itemValue.ItemClass;
	}

	// Token: 0x06002D76 RID: 11638 RVA: 0x00122C2E File Offset: 0x00120E2E
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

	// Token: 0x06002D79 RID: 11641 RVA: 0x00122C3C File Offset: 0x00120E3C
	public ItemStack GetItemStack(int _idx)
	{
		return this.GetStackAt(_idx);
	}

	// Token: 0x06002D7A RID: 11642 RVA: 0x00122C50 File Offset: 0x00120E50
	public void ModifyValue(ItemValue _originalItemValue, PassiveEffects _passiveEffect, ref float _base_val, ref float _perc_val, FastTags<TagGroup.Global> tags)
	{
		if (this.holdingItemItemValue != null && !this.holdingItemItemValue.Equals(_originalItemValue) && !this.holdingItemItemValue.ItemClass.ItemTags.Test_AnySet(Hand.IgnoreWhenHeld))
		{
			this.holdingItemItemValue.ModifyValue(this.entity, _originalItemValue, _passiveEffect, ref _base_val, ref _perc_val, tags, true, false);
		}
	}

	// Token: 0x06002D7B RID: 11643 RVA: 0x00122CA9 File Offset: 0x00120EA9
	public bool IsHandBusy()
	{
		return this.Hand.IsBusy;
	}

	// Token: 0x06002D7C RID: 11644 RVA: 0x00122CB6 File Offset: 0x00120EB6
	public bool IsHandSwitching()
	{
		return this.Hand.IsSwitching;
	}

	// Token: 0x06002D7D RID: 11645 RVA: 0x00122CC3 File Offset: 0x00120EC3
	public int GetFocusedItemIdx()
	{
		return this.FocusedSlot;
	}

	// Token: 0x06002D7E RID: 11646 RVA: 0x00122CCB File Offset: 0x00120ECB
	public int SetFocusedItemIdx(int _idx)
	{
		this.FocusedSlot = _idx;
		return _idx;
	}

	// Token: 0x06002D7F RID: 11647 RVA: 0x00122CD5 File Offset: 0x00120ED5
	public bool IsHoldingItemActionRunning()
	{
		return this.Hand.IsActionRunning();
	}

	// Token: 0x06002D80 RID: 11648 RVA: 0x00122CE2 File Offset: 0x00120EE2
	public void SetHoldingItemIdxNoHolsterTime(int _inventoryIdx)
	{
		this.Hand.SelectSlot(_inventoryIdx, false);
	}

	// Token: 0x06002D81 RID: 11649 RVA: 0x00122CF1 File Offset: 0x00120EF1
	public void SetHoldingItemIdx(int _inventoryIdx)
	{
		this.Hand.SelectSlot(_inventoryIdx, true);
	}

	// Token: 0x06002D82 RID: 11650 RVA: 0x00122D00 File Offset: 0x00120F00
	public void ReconcileHeldItem()
	{
		this.Hand.Reconcile();
	}

	// Token: 0x06002D83 RID: 11651 RVA: 0x00122D0D File Offset: 0x00120F0D
	public Transform GetHoldingItemTransform()
	{
		return this.Hand.ModelTransform;
	}

	// Token: 0x06002D84 RID: 11652 RVA: 0x00122D1A File Offset: 0x00120F1A
	public int GetBestQuickSwapSlot()
	{
		return this.Hand.GetBestQuickSwapSlot();
	}

	// Token: 0x06002D85 RID: 11653 RVA: 0x00122D27 File Offset: 0x00120F27
	public void Execute(int _actionIdx, bool _bReleased, PlayerActionsLocal _playerActions = null)
	{
		this.Hand.Execute(_actionIdx, _bReleased, _playerActions);
	}

	// Token: 0x06002D86 RID: 11654 RVA: 0x00122D37 File Offset: 0x00120F37
	public void ReleaseAll(PlayerActionsLocal _playerActions = null)
	{
		this.Hand.ReleaseAll(_playerActions);
	}

	// Token: 0x06002D87 RID: 11655 RVA: 0x00122D45 File Offset: 0x00120F45
	public bool IsHUDDisabled()
	{
		return this.Hand.IsHUDDisabled();
	}

	// Token: 0x06002D88 RID: 11656 RVA: 0x00122D52 File Offset: 0x00120F52
	public string CanInteract()
	{
		return this.Hand.CanInteract();
	}

	// Token: 0x06002D89 RID: 11657 RVA: 0x00122D5F File Offset: 0x00120F5F
	public void Interact()
	{
		this.Hand.Interact();
	}

	// Token: 0x06002D8A RID: 11658 RVA: 0x00122D6C File Offset: 0x00120F6C
	public bool IsHoldingGun()
	{
		return this.holdingItem != null && this.holdingItem.IsGun();
	}

	// Token: 0x06002D8B RID: 11659 RVA: 0x00122D83 File Offset: 0x00120F83
	public bool IsHoldingDynamicMelee()
	{
		return this.holdingItem != null && this.holdingItem.IsDynamicMelee();
	}

	// Token: 0x06002D8C RID: 11660 RVA: 0x00122D9A File Offset: 0x00120F9A
	public bool IsHoldingBlock()
	{
		return this.holdingItem != null && this.holdingItem.IsBlock();
	}

	// Token: 0x06002D8D RID: 11661 RVA: 0x00122DB1 File Offset: 0x00120FB1
	public ItemAction GetHoldingPrimary()
	{
		return this.holdingItem.Actions[0];
	}

	// Token: 0x06002D8E RID: 11662 RVA: 0x00122DC0 File Offset: 0x00120FC0
	public ItemAction GetHoldingSecondary()
	{
		return this.holdingItem.Actions[1];
	}

	// Token: 0x06002D8F RID: 11663 RVA: 0x00122DCF File Offset: 0x00120FCF
	public ItemActionAttack GetHoldingGun()
	{
		return this.holdingItem.Actions[0] as ItemActionAttack;
	}

	// Token: 0x06002D90 RID: 11664 RVA: 0x00122DE3 File Offset: 0x00120FE3
	public ItemActionDynamic GetHoldingDynamicMelee()
	{
		return this.holdingItem.Actions[0] as ItemActionDynamic;
	}

	// Token: 0x06002D91 RID: 11665 RVA: 0x00122DF7 File Offset: 0x00120FF7
	public ItemClassBlock GetHoldingBlock()
	{
		return this.holdingItem as ItemClassBlock;
	}

	// Token: 0x06002D92 RID: 11666 RVA: 0x00122E04 File Offset: 0x00121004
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

	// Token: 0x06002D93 RID: 11667 RVA: 0x00122EB0 File Offset: 0x001210B0
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

	// Token: 0x06002D94 RID: 11668 RVA: 0x00122F5C File Offset: 0x0012115C
	public override bool AddItem(ItemStack _itemStack)
	{
		int num;
		return this.AddItem(_itemStack, out num);
	}

	// Token: 0x06002D95 RID: 11669 RVA: 0x00122F72 File Offset: 0x00121172
	public bool AddItem(ItemStack _itemStack, out int _slot)
	{
		return this.AddItem(_itemStack, 0, this.Length, out _slot);
	}

	// Token: 0x06002D96 RID: 11670 RVA: 0x00122F84 File Offset: 0x00121184
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

	// Token: 0x06002D97 RID: 11671 RVA: 0x00123070 File Offset: 0x00121270
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

	// Token: 0x06002D98 RID: 11672 RVA: 0x001230C0 File Offset: 0x001212C0
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

	// Token: 0x06002D99 RID: 11673 RVA: 0x00123164 File Offset: 0x00121364
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

	// Token: 0x06002D9A RID: 11674 RVA: 0x00123260 File Offset: 0x00121460
	[PublicizedFrom(EAccessModifier.Private)]
	public void clearSlotByIndex(int _idx)
	{
		this.SetStackAt(_idx, ItemStack.Empty);
	}

	// Token: 0x06002D9B RID: 11675 RVA: 0x0012326E File Offset: 0x0012146E
	public void SetItem(int _idx, ItemStack _itemStack)
	{
		this.SetItem(_idx, _itemStack.itemValue, _itemStack.count);
	}

	// Token: 0x06002D9C RID: 11676 RVA: 0x00123284 File Offset: 0x00121484
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

	// Token: 0x06002D9D RID: 11677 RVA: 0x001232F4 File Offset: 0x001214F4
	public void FireEvent(MinEventTypes _eventType, MinEventParams _eventParms)
	{
		ItemValue itemValue = _eventParms.ItemValue;
		if (itemValue == null)
		{
			itemValue = this.holdingItemItemValue;
		}
		itemValue.FireEvent(_eventType, _eventParms);
	}

	// Token: 0x06002D9E RID: 11678 RVA: 0x00122C3C File Offset: 0x00120E3C
	public ItemStack GetItem(int _idx)
	{
		return this.GetStackAt(_idx);
	}

	// Token: 0x06002D9F RID: 11679 RVA: 0x0012331C File Offset: 0x0012151C
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

	// Token: 0x06002DA0 RID: 11680 RVA: 0x001233AC File Offset: 0x001215AC
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

	// Token: 0x06002DA1 RID: 11681 RVA: 0x00123476 File Offset: 0x00121676
	public IEnumerator SimulateActionExecution(int _actionIdx, ItemStackGrid _sourceGrid, int _sourceIdx)
	{
		return this.Hand.SimulateActionExecution(_actionIdx, _sourceGrid, _sourceIdx);
	}

	// Token: 0x06002DA2 RID: 11682 RVA: 0x00123488 File Offset: 0x00121688
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

	// Token: 0x06002DA3 RID: 11683 RVA: 0x001234D4 File Offset: 0x001216D4
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

	// Token: 0x06002DA4 RID: 11684 RVA: 0x00123508 File Offset: 0x00121708
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

	// Token: 0x06002DA5 RID: 11685 RVA: 0x001235BC File Offset: 0x001217BC
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

	// Token: 0x06002DA6 RID: 11686 RVA: 0x001235FE File Offset: 0x001217FE
	public void Cleanup()
	{
		this.ItemGrid.SlotChanged -= this.OnSlotChanged;
		this.entity = null;
	}

	// Token: 0x06002DA7 RID: 11687 RVA: 0x00123620 File Offset: 0x00121820
	public override void PerformActionOnSlots(Action<ItemStack> _action)
	{
		for (int i = 0; i < this.Length; i++)
		{
			_action(this.GetStackAt(i));
		}
	}

	// Token: 0x06002DA8 RID: 11688 RVA: 0x0012364B File Offset: 0x0012184B
	public override bool HasItem(ItemValue _item)
	{
		return this.GetItemCount(_item, false, -1, -1, true) > 0;
	}

	// Token: 0x06002DA9 RID: 11689 RVA: 0x0012365B File Offset: 0x0012185B
	public bool CanMoveToSlot(ItemStack _itemStack, int _slotIdx)
	{
		return _itemStack.itemValue.ItemClass == null || _itemStack.itemValue.ItemClass.CanMoveToSlot(XUiC_ItemStack.StackLocationTypes.Backpack, _slotIdx);
	}

	// Token: 0x040023AC RID: 9132
	public const byte Version = 1;

	// Token: 0x040023AD RID: 9133
	public EntityAlive entity;

	// Token: 0x040023AF RID: 9135
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] preferredItemTypes;

	// Token: 0x040023B0 RID: 9136
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isReading;

	// Token: 0x040023B5 RID: 9141
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ItemValue> activatables = new List<ItemValue>();
}
