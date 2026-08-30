using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Audio;
using UnityEngine;

// Token: 0x02001136 RID: 4406
public class XUiM_PlayerInventory : XUiModel, IInventory
{
	// Token: 0x140000E6 RID: 230
	// (add) Token: 0x06008B3E RID: 35646 RVA: 0x0034FCAC File Offset: 0x0034DEAC
	// (remove) Token: 0x06008B3F RID: 35647 RVA: 0x0034FCE4 File Offset: 0x0034DEE4
	public event XUiEvent_BackpackItemsChanged OnBackpackItemsChanged;

	// Token: 0x140000E7 RID: 231
	// (add) Token: 0x06008B40 RID: 35648 RVA: 0x0034FD1C File Offset: 0x0034DF1C
	// (remove) Token: 0x06008B41 RID: 35649 RVA: 0x0034FD54 File Offset: 0x0034DF54
	public event XUiEvent_ToolbeltItemsChanged OnToolbeltItemsChanged;

	// Token: 0x140000E8 RID: 232
	// (add) Token: 0x06008B42 RID: 35650 RVA: 0x0034FD8C File Offset: 0x0034DF8C
	// (remove) Token: 0x06008B43 RID: 35651 RVA: 0x0034FDC4 File Offset: 0x0034DFC4
	public event XUiEvent_CurrencyChanged OnCurrencyChanged;

	// Token: 0x17001017 RID: 4119
	// (get) Token: 0x06008B44 RID: 35652 RVA: 0x0034FDF9 File Offset: 0x0034DFF9
	public Bag Backpack
	{
		get
		{
			return this.backpack;
		}
	}

	// Token: 0x17001018 RID: 4120
	// (get) Token: 0x06008B45 RID: 35653 RVA: 0x0034FE01 File Offset: 0x0034E001
	public Inventory Toolbelt
	{
		get
		{
			return this.toolbelt;
		}
	}

	// Token: 0x17001019 RID: 4121
	// (get) Token: 0x06008B46 RID: 35654 RVA: 0x0034FE09 File Offset: 0x0034E009
	// (set) Token: 0x06008B47 RID: 35655 RVA: 0x0034FE11 File Offset: 0x0034E011
	public int CurrencyAmount
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700101A RID: 4122
	// (get) Token: 0x06008B48 RID: 35656 RVA: 0x0034FE1A File Offset: 0x0034E01A
	public int QuickSwapSlot
	{
		get
		{
			return this.toolbelt.GetBestQuickSwapSlot();
		}
	}

	// Token: 0x06008B49 RID: 35657 RVA: 0x0034FE28 File Offset: 0x0034E028
	public XUiM_PlayerInventory(XUi _xui, EntityPlayerLocal _player)
	{
		if (_player == null)
		{
			return;
		}
		this.xui = _xui;
		this.localPlayer = _player;
		this.backpack = this.localPlayer.bag;
		this.toolbelt = this.localPlayer.inventory;
		this.backpack.OnBackpackItemsChangedInternal += this.dispatchBackpackItemsChanged;
		this.toolbelt.OnToolbeltItemsChangedInternal += this.dispatchToolbeltItemsChanged;
		this.localPlayer.PlayerUI.OnUIShutdown += this.HandleUIShutdown;
		this.currencyItem = ItemClass.GetItem(TraderInfo.CurrencyItem, false);
	}

	// Token: 0x06008B4A RID: 35658 RVA: 0x0034FED0 File Offset: 0x0034E0D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void dispatchBackpackItemsChanged()
	{
		XUiEvent_BackpackItemsChanged onBackpackItemsChanged = this.OnBackpackItemsChanged;
		if (onBackpackItemsChanged != null)
		{
			onBackpackItemsChanged();
		}
		this.RefreshCurrency();
	}

	// Token: 0x06008B4B RID: 35659 RVA: 0x0034FEE9 File Offset: 0x0034E0E9
	[PublicizedFrom(EAccessModifier.Private)]
	public void dispatchToolbeltItemsChanged()
	{
		XUiEvent_ToolbeltItemsChanged onToolbeltItemsChanged = this.OnToolbeltItemsChanged;
		if (onToolbeltItemsChanged != null)
		{
			onToolbeltItemsChanged();
		}
		this.RefreshCurrency();
	}

	// Token: 0x06008B4C RID: 35660 RVA: 0x0034FF02 File Offset: 0x0034E102
	public bool AddItemToPreferredToolbeltSlot(ItemStack _itemStack, int _slot)
	{
		return this.toolbelt.AddItemAtSlot(_itemStack, _slot);
	}

	// Token: 0x06008B4D RID: 35661 RVA: 0x0034FF11 File Offset: 0x0034E111
	public bool AddItemNoPartial(ItemStack _itemStack, bool _playCollectSound = true)
	{
		return (this.backpack.CanStack(_itemStack) || this.toolbelt.CanStack(_itemStack)) && this.AddItem(_itemStack, _playCollectSound);
	}

	// Token: 0x06008B4E RID: 35662 RVA: 0x0034FF3C File Offset: 0x0034E13C
	public bool CanSwapItems(ItemStack _removedStack, ItemStack _addedStack, int _slotNumber = -1)
	{
		List<ItemStack> allItemStacks = this.GetAllItemStacks();
		int num = _removedStack.count;
		int num2 = _addedStack.count;
		int maxCount = _removedStack.itemValue.ItemClass.MaxCount;
		int maxCount2 = _addedStack.itemValue.ItemClass.MaxCount;
		for (int i = 0; i < allItemStacks.Count - 1; i++)
		{
			if (num > 0 && allItemStacks[i].itemValue.type == _removedStack.itemValue.type && (_slotNumber == -1 || _slotNumber == i))
			{
				int count = allItemStacks[i].count;
				if (count > num)
				{
					num = 0;
				}
				else
				{
					num -= count;
					num2 -= maxCount2;
				}
			}
			else if (num2 > 0 && allItemStacks[i].itemValue.type == _addedStack.itemValue.type)
			{
				int num3 = maxCount2 - allItemStacks[i].count;
				if (num3 >= num2)
				{
					num2 = 0;
				}
				else
				{
					num2 -= num3;
				}
			}
			else if (allItemStacks[i].IsEmpty())
			{
				num2 -= maxCount2;
			}
			if (num <= 0 && num2 <= 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06008B4F RID: 35663 RVA: 0x00350054 File Offset: 0x0034E254
	public void SortStacks(int _ignoreSlots = 0, PackedBoolArray _ignoredSlots = null)
	{
		if (EffectManager.GetValue(PassiveEffects.ShuffledBackpack, null, 0f, this.localPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false) == 0f)
		{
			ItemStack[] array = StackSortUtil.CombineAndSortStacks(this.GetBackpackItemStacks(), _ignoreSlots, _ignoredSlots);
			this.backpack.SetSlots(array);
		}
	}

	// Token: 0x06008B50 RID: 35664 RVA: 0x003500AC File Offset: 0x0034E2AC
	public bool AddItem(ItemStack _itemStack, bool _playCollectSound)
	{
		if (!_itemStack.CanMoveTo(XUiC_ItemStack.StackLocationTypes.ToolBelt, -1))
		{
			return false;
		}
		bool flag = false;
		ItemStack itemStack = _itemStack.Clone();
		ItemClassArmor itemClassArmor = _itemStack.itemValue.ItemClass as ItemClassArmor;
		if (itemClassArmor != null && itemClassArmor.AutoEquip)
		{
			Equipment equipment = this.xui.playerUI.entityPlayer.equipment;
			int equipSlot = (int)itemClassArmor.EquipSlot;
			if (itemClassArmor.AllowUnEquip)
			{
				ItemValue slotItem = equipment.GetSlotItem(equipSlot);
				equipment.SetSlotItem(equipSlot, itemStack.itemValue, true);
				QuestEventManager.Current.WoreItem(itemStack.itemValue);
				itemStack.count = 0;
				itemStack = new ItemStack(slotItem, 1);
			}
			else
			{
				equipment.SetSlotItem(equipSlot, itemStack.itemValue, true);
				QuestEventManager.Current.WoreItem(itemStack.itemValue);
				flag = true;
				itemStack.count = 0;
			}
			this.xui.PlayerEquipment.RefreshEquipment();
		}
		if (!flag)
		{
			if (this.backpack.CanStackNoEmpty(itemStack))
			{
				flag = this.backpack.TryStackItem(0, itemStack).Item2;
			}
			else if (this.toolbelt.CanStackNoEmpty(itemStack))
			{
				flag = this.toolbelt.TryStackItem(0, itemStack).Item2;
			}
		}
		if (!flag)
		{
			if (this.backpack.CanStack(itemStack))
			{
				flag = this.backpack.TryStackItem(0, itemStack).Item2;
			}
			else if (this.toolbelt.CanStack(itemStack))
			{
				flag = this.toolbelt.TryStackItem(0, itemStack).Item2;
			}
		}
		if (!flag)
		{
			ItemClass itemClass = itemStack.itemValue.ItemClass;
			int num = 1;
			if (((itemClass != null) ? itemClass.Stacknumber : null) != null)
			{
				num = itemClass.MaxCount;
			}
			if (itemStack.count > num)
			{
				for (int i = itemStack.count; i > 0; i -= num)
				{
					bool flag2 = false;
					int num2 = Math.Min(i, num);
					num2 = Math.Max(0, num2);
					ItemStack itemStack2 = itemStack.Clone();
					itemStack2.count = num2;
					ItemClassArmor itemClassArmor2 = itemStack.itemValue.ItemClass as ItemClassArmor;
					if (itemClassArmor2 != null && !this.xui.PlayerEquipment.IsEquipmentTypeWorn(itemClassArmor2.EquipSlot) && this.localPlayer.equipment.ReturnItem(itemStack2, true))
					{
						flag = true;
						flag2 = true;
						itemStack.count -= itemStack2.count;
						this.xui.PlayerEquipment.RefreshEquipment();
					}
					else if (this.toolbelt.ReturnItem(itemStack2))
					{
						flag = true;
						flag2 = true;
						itemStack.count -= itemStack2.count;
					}
					else if (this.backpack.AddItem(itemStack2))
					{
						flag = true;
						flag2 = true;
						itemStack.count -= itemStack2.count;
					}
					else if (this.toolbelt.AddItem(itemStack2))
					{
						flag = true;
						flag2 = true;
						itemStack.count -= itemStack2.count;
					}
					if (!flag2)
					{
						if (_itemStack.count != itemStack.count)
						{
							XUiC_CollectedItemList collectedItemList = this.xui.CollectedItemList;
							if (collectedItemList != null)
							{
								collectedItemList.AddItemStack(new ItemStack(itemStack.itemValue, _itemStack.count - itemStack.count), false);
							}
							if (_playCollectSound)
							{
								Manager.PlayInsidePlayerHead("item_pickup", -1, 0f, false, false);
							}
							_itemStack.count = itemStack.count;
						}
						return false;
					}
				}
			}
			else
			{
				ItemClassArmor itemClassArmor3 = itemStack.itemValue.ItemClass as ItemClassArmor;
				if (itemClassArmor3 != null && !this.xui.PlayerEquipment.IsEquipmentTypeWorn(itemClassArmor3.EquipSlot) && this.localPlayer.equipment.ReturnItem(itemStack, true))
				{
					flag = true;
					itemStack = itemStack.Clone();
					itemStack.count = 0;
					this.xui.PlayerEquipment.RefreshEquipment();
				}
				else if (this.toolbelt.ReturnItem(itemStack))
				{
					flag = true;
					itemStack = itemStack.Clone();
					itemStack.count = 0;
					this.dispatchToolbeltItemsChanged();
				}
				else if (this.backpack.AddItem(itemStack))
				{
					flag = true;
					itemStack = itemStack.Clone();
					itemStack.count = 0;
					this.dispatchBackpackItemsChanged();
				}
				else if (this.toolbelt.AddItem(itemStack))
				{
					flag = true;
					itemStack = itemStack.Clone();
					itemStack.count = 0;
					this.dispatchToolbeltItemsChanged();
				}
			}
		}
		if (itemStack.count != _itemStack.count)
		{
			ItemStack itemStack3 = new ItemStack(itemStack.itemValue, _itemStack.count - itemStack.count);
			QuestEventManager.Current.ItemAdded(itemStack3);
			XUiC_CollectedItemList collectedItemList2 = this.xui.CollectedItemList;
			if (collectedItemList2 != null)
			{
				collectedItemList2.AddItemStack(itemStack3, false);
			}
			if (_playCollectSound)
			{
				Manager.PlayInsidePlayerHead("item_pickup", -1, 0f, false, false);
			}
		}
		if (itemStack.count == 0)
		{
			itemStack = ItemStack.Empty;
		}
		_itemStack.count = itemStack.count;
		return flag;
	}

	// Token: 0x06008B51 RID: 35665 RVA: 0x0035054C File Offset: 0x0034E74C
	public int CountAvailableSpaceForItem(ItemValue _itemValue, bool _limitToOneStack = true)
	{
		ItemStack itemStack = new ItemStack(_itemValue, 1);
		int maxCount = _itemValue.ItemClass.MaxCount;
		int num = 0;
		ItemStack[] slots = this.backpack.GetSlots();
		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].IsEmpty())
			{
				num += maxCount;
			}
			else if (itemStack.CanStackWith(slots[i], false))
			{
				num += maxCount - slots[i].count;
			}
			if (_limitToOneStack && num >= maxCount)
			{
				return maxCount;
			}
		}
		ItemStack[] slots2 = this.toolbelt.GetSlots();
		for (int j = 0; j < this.toolbelt.PUBLIC_SLOTS; j++)
		{
			if (slots2[j].IsEmpty())
			{
				num += maxCount;
			}
			else if (itemStack.CanStackWith(slots2[j], false))
			{
				num += maxCount - slots2[j].count;
			}
			if (_limitToOneStack && num > maxCount)
			{
				return maxCount;
			}
		}
		return num;
	}

	// Token: 0x06008B52 RID: 35666 RVA: 0x00350624 File Offset: 0x0034E824
	public void DropItem(ItemStack _stack)
	{
		GameManager instance = GameManager.Instance;
		if (instance)
		{
			instance.ItemDropServer(_stack, this.localPlayer.GetDropPosition(), Vector3.zero, this.localPlayer.entityId, 60f, false);
			Manager.BroadcastPlay("itemdropped");
		}
		XUiC_CollectedItemList collectedItemList = this.xui.CollectedItemList;
		if (collectedItemList == null)
		{
			return;
		}
		collectedItemList.RemoveItemStack(_stack);
	}

	// Token: 0x06008B53 RID: 35667 RVA: 0x00350688 File Offset: 0x0034E888
	public bool AddItems(ItemStack[] _itemStacks)
	{
		bool flag = true;
		for (int i = 0; i < _itemStacks.Length; i++)
		{
			flag &= this.AddItem(_itemStacks[i]);
		}
		return flag;
	}

	// Token: 0x06008B54 RID: 35668 RVA: 0x003506B4 File Offset: 0x0034E8B4
	public bool AddItemsUsingPreferenceTracker(XUiC_ItemStackGrid _srcGrid, PreferenceTracker _preferences)
	{
		if (!_preferences.AnyPreferences)
		{
			return false;
		}
		if (this.localPlayer.entityId != _preferences.PlayerID)
		{
			return false;
		}
		bool flag = false;
		XUiC_ItemStack[] itemStackControllers = _srcGrid.GetItemStackControllers();
		HashSet<int> hashSet = new HashSet<int>();
		for (int j = 0; j < itemStackControllers.Length; j++)
		{
			ValueTuple<bool, bool> valueTuple = this.TryStackItem(0, itemStackControllers[j].ItemStack);
			bool item = valueTuple.Item1;
			bool item2 = valueTuple.Item2;
			flag = flag || item;
			if (item)
			{
				if (item2)
				{
					itemStackControllers[j].ItemStack = ItemStack.Empty;
				}
				else
				{
					hashSet.Add(itemStackControllers[j].ItemStack.itemValue.type);
				}
				_srcGrid.HandleSlotChangedEvent(j, itemStackControllers[j].ItemStack);
			}
		}
		if (_preferences.toolbelt != null)
		{
			ItemStack[] slots = this.toolbelt.GetSlots();
			int k = 0;
			while (k < _preferences.toolbelt.Length && k < slots.Length)
			{
				if (slots[k].IsEmpty())
				{
					int type2 = _preferences.toolbelt[k].itemValue.type;
					XUiC_ItemStack xuiC_ItemStack;
					if (hashSet.Contains(type2))
					{
						xuiC_ItemStack = itemStackControllers.FirstOrDefault<XUiC_ItemStack>((XUiC_ItemStack _stack) => _stack.ItemStack.itemValue.type == type2);
					}
					else
					{
						xuiC_ItemStack = itemStackControllers.FirstOrDefault<XUiC_ItemStack>((XUiC_ItemStack _stack) => _stack.ItemStack.Equals(_preferences.toolbelt[k]));
					}
					if (((xuiC_ItemStack != null) ? xuiC_ItemStack.ItemStack : null) != null && !xuiC_ItemStack.ItemStack.IsEmpty())
					{
						this.toolbelt.SetItem(k, xuiC_ItemStack.ItemStack);
						xuiC_ItemStack.ItemStack = ItemStack.Empty;
						_srcGrid.HandleSlotChangedEvent(xuiC_ItemStack.SlotNumber, ItemStack.Empty);
						flag = true;
					}
				}
				int num = k;
				k = num + 1;
			}
		}
		if (_preferences.equipment != null)
		{
			int num2 = Utils.FastMin(this.localPlayer.equipment.GetSlotCount(), _preferences.equipment.Length);
			int num;
			int l;
			for (l = 0; l < num2; l = num + 1)
			{
				if (this.localPlayer.equipment.GetSlotItem(l) == null)
				{
					XUiC_ItemStack xuiC_ItemStack2 = itemStackControllers.FirstOrDefault<XUiC_ItemStack>((XUiC_ItemStack _stack) => _stack.ItemStack.itemValue.Equals(_preferences.equipment[l]));
					if (((xuiC_ItemStack2 != null) ? xuiC_ItemStack2.ItemStack : null) != null && !xuiC_ItemStack2.ItemStack.IsEmpty())
					{
						this.localPlayer.equipment.SetSlotItem(l, xuiC_ItemStack2.ItemStack.itemValue, true);
						xuiC_ItemStack2.ItemStack.count--;
						if (xuiC_ItemStack2.ItemStack.IsEmpty())
						{
							xuiC_ItemStack2.ItemStack = ItemStack.Empty;
							_srcGrid.HandleSlotChangedEvent(xuiC_ItemStack2.SlotNumber, ItemStack.Empty);
						}
						flag = true;
					}
				}
				num = l;
			}
		}
		this.xui.PlayerEquipment.RefreshEquipment();
		if (_preferences.bag != null)
		{
			ItemStack[] slots2 = this.backpack.GetSlots();
			int i = 0;
			while (i < _preferences.bag.Length && i < slots2.Length)
			{
				if (slots2[i].IsEmpty())
				{
					int type = _preferences.bag[i].itemValue.type;
					XUiC_ItemStack xuiC_ItemStack3;
					if (hashSet.Contains(type))
					{
						xuiC_ItemStack3 = itemStackControllers.FirstOrDefault<XUiC_ItemStack>((XUiC_ItemStack _stack) => _stack.ItemStack.itemValue.type == type);
					}
					else
					{
						xuiC_ItemStack3 = itemStackControllers.FirstOrDefault<XUiC_ItemStack>((XUiC_ItemStack _stack) => _stack.ItemStack.Equals(_preferences.bag[i]));
					}
					if (((xuiC_ItemStack3 != null) ? xuiC_ItemStack3.ItemStack : null) != null && !xuiC_ItemStack3.ItemStack.IsEmpty())
					{
						this.backpack.SetSlot(i, xuiC_ItemStack3.ItemStack, true);
						xuiC_ItemStack3.ItemStack = ItemStack.Empty;
						_srcGrid.HandleSlotChangedEvent(xuiC_ItemStack3.SlotNumber, ItemStack.Empty);
						flag = true;
					}
				}
				int num = i;
				i = num + 1;
			}
		}
		return flag;
	}

	// Token: 0x06008B55 RID: 35669 RVA: 0x00350B58 File Offset: 0x0034ED58
	public bool AddItemToBackpack(ItemStack _itemStack)
	{
		this.backpack.TryStackItem(0, _itemStack);
		return _itemStack.count > 0 && this.backpack.AddItem(_itemStack);
	}

	// Token: 0x06008B56 RID: 35670 RVA: 0x00350B82 File Offset: 0x0034ED82
	public bool AddItemToToolbelt(ItemStack _itemStack)
	{
		this.toolbelt.TryStackItem(0, _itemStack);
		return _itemStack.count > 0 && this.toolbelt.AddItem(_itemStack);
	}

	// Token: 0x06008B57 RID: 35671 RVA: 0x00350BAC File Offset: 0x0034EDAC
	public bool HasItem(ItemStack _itemStack)
	{
		return this.HasItems(new ItemStack[] { _itemStack }, 1);
	}

	// Token: 0x06008B58 RID: 35672 RVA: 0x00350BC0 File Offset: 0x0034EDC0
	public bool HasItems(IList<ItemStack> _itemStacks, int _multiplier = 1)
	{
		for (int i = 0; i < _itemStacks.Count; i++)
		{
			int num = _itemStacks[i].count * _multiplier;
			num -= this.backpack.GetItemCount(_itemStacks[i].itemValue, -1, -1, true);
			if (num > 0)
			{
				num -= this.toolbelt.GetItemCount(_itemStacks[i].itemValue, false, -1, -1, true);
			}
			if (num > 0)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06008B59 RID: 35673 RVA: 0x00350C33 File Offset: 0x0034EE33
	public void RemoveItem(ItemStack _itemStack)
	{
		this.RemoveItems(new ItemStack[] { _itemStack }, 1, null);
	}

	// Token: 0x06008B5A RID: 35674 RVA: 0x00350C48 File Offset: 0x0034EE48
	public void RemoveItems(IList<ItemStack> _itemStacks, int _multiplier = 1, IList<ItemStack> _removedItems = null)
	{
		if (!this.HasItems(_itemStacks, 1))
		{
			return;
		}
		for (int i = 0; i < _itemStacks.Count; i++)
		{
			int num = _itemStacks[i].count * _multiplier;
			num -= this.backpack.DecItem(_itemStacks[i].itemValue, num, true, _removedItems);
			if (num > 0)
			{
				this.toolbelt.DecItem(_itemStacks[i].itemValue, num, true, _removedItems);
			}
		}
		this.dispatchBackpackItemsChanged();
		this.dispatchToolbeltItemsChanged();
	}

	// Token: 0x06008B5B RID: 35675 RVA: 0x00350CC8 File Offset: 0x0034EEC8
	public int GetItemCount(ItemValue _itemValue)
	{
		return 0 + this.backpack.GetItemCount(_itemValue, -1, -1, true) + this.toolbelt.GetItemCount(_itemValue, false, -1, -1, true);
	}

	// Token: 0x06008B5C RID: 35676 RVA: 0x00350CEC File Offset: 0x0034EEEC
	public int GetItemCountWithMods(ItemValue _itemValue)
	{
		return 0 + this.backpack.GetItemCount(_itemValue, -1, -1, false) + this.toolbelt.GetItemCount(_itemValue, false, -1, -1, false);
	}

	// Token: 0x06008B5D RID: 35677 RVA: 0x00350D10 File Offset: 0x0034EF10
	public int GetItemCount(int _itemId)
	{
		ItemValue itemValue = new ItemValue(_itemId, false);
		return 0 + this.backpack.GetItemCount(itemValue, -1, -1, true) + this.toolbelt.GetItemCount(itemValue, false, -1, -1, true);
	}

	// Token: 0x06008B5E RID: 35678 RVA: 0x00350D47 File Offset: 0x0034EF47
	public List<ItemStack> GetAllItemStacks()
	{
		List<ItemStack> list = new List<ItemStack>();
		list.AddRange(this.GetBackpackItemStacks());
		list.AddRange(this.GetToolbeltItemStacks());
		return list;
	}

	// Token: 0x06008B5F RID: 35679 RVA: 0x00350D66 File Offset: 0x0034EF66
	public ItemStack[] GetBackpackItemStacks()
	{
		return this.backpack.GetSlots();
	}

	// Token: 0x06008B60 RID: 35680 RVA: 0x00350D73 File Offset: 0x0034EF73
	public ItemStack[] GetToolbeltItemStacks()
	{
		return this.toolbelt.GetSlots();
	}

	// Token: 0x06008B61 RID: 35681 RVA: 0x00350D80 File Offset: 0x0034EF80
	public void SetBackpackItemStacks(ItemStack[] _itemStacks)
	{
		this.backpack.SetSlots(_itemStacks);
		this.dispatchBackpackItemsChanged();
	}

	// Token: 0x06008B62 RID: 35682 RVA: 0x00350D94 File Offset: 0x0034EF94
	public void SetToolbeltItemStacks(ItemStack[] _itemStacks)
	{
		this.toolbelt.SetSlots(_itemStacks, false);
		this.dispatchToolbeltItemsChanged();
	}

	// Token: 0x06008B63 RID: 35683 RVA: 0x00350DAC File Offset: 0x0034EFAC
	public void RefreshCurrency()
	{
		int itemCount = this.GetItemCount(this.currencyItem);
		if (itemCount != this.CurrencyAmount)
		{
			this.CurrencyAmount = itemCount;
			XUiEvent_CurrencyChanged onCurrencyChanged = this.OnCurrencyChanged;
			if (onCurrencyChanged == null)
			{
				return;
			}
			onCurrencyChanged();
		}
	}

	// Token: 0x06008B64 RID: 35684 RVA: 0x00350DE8 File Offset: 0x0034EFE8
	public void HandleUIShutdown()
	{
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(this.localPlayer);
		if (uiforPlayer != null)
		{
			uiforPlayer.OnUIShutdown -= this.HandleUIShutdown;
		}
		this.backpack.OnBackpackItemsChangedInternal -= this.dispatchBackpackItemsChanged;
		this.toolbelt.OnToolbeltItemsChangedInternal -= this.dispatchToolbeltItemsChanged;
	}

	// Token: 0x06008B65 RID: 35685 RVA: 0x00350E4A File Offset: 0x0034F04A
	public bool AddItem(ItemStack _itemStack)
	{
		return this.AddItem(_itemStack, true);
	}

	// Token: 0x06008B66 RID: 35686 RVA: 0x00350E54 File Offset: 0x0034F054
	[return: TupleElementNames(new string[] { "anyMoved", "allMoved" })]
	public ValueTuple<bool, bool> TryStackItem(int _startIndex, ItemStack _itemStack)
	{
		int count = _itemStack.count;
		bool flag = this.toolbelt.TryStackItem(_startIndex, _itemStack).Item2;
		if (!flag)
		{
			flag = this.backpack.TryStackItem(_startIndex, _itemStack).Item2;
		}
		if (count != _itemStack.count)
		{
			ItemStack itemStack = new ItemStack(_itemStack.itemValue, count - _itemStack.count);
			QuestEventManager.Current.ItemAdded(itemStack);
			XUiC_CollectedItemList collectedItemList = this.xui.CollectedItemList;
			if (collectedItemList != null)
			{
				collectedItemList.AddItemStack(itemStack, false);
			}
			Manager.PlayInsidePlayerHead("item_pickup", -1, 0f, false, false);
		}
		return new ValueTuple<bool, bool>(count != _itemStack.count, flag);
	}

	// Token: 0x06008B67 RID: 35687 RVA: 0x00350EF5 File Offset: 0x0034F0F5
	public bool HasItem(ItemValue _item)
	{
		return this.GetItemCount(_item) > 0;
	}

	// Token: 0x06008B68 RID: 35688 RVA: 0x00350F04 File Offset: 0x0034F104
	public static bool TryStackItem(int _startIndex, ItemStack _itemStack, ItemStack[] _items)
	{
		for (int i = _startIndex; i < _items.Length; i++)
		{
			int count = _itemStack.count;
			if (_itemStack.itemValue.type == _items[i].itemValue.type && _items[i].CanStackPartly(ref count))
			{
				_items[i].count += count;
				_itemStack.count -= count;
				if (_itemStack.count == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x04006708 RID: 26376
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUi xui;

	// Token: 0x04006709 RID: 26377
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly EntityPlayerLocal localPlayer;

	// Token: 0x0400670A RID: 26378
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Bag backpack;

	// Token: 0x0400670B RID: 26379
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Inventory toolbelt;

	// Token: 0x0400670C RID: 26380
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ItemValue currencyItem;
}
