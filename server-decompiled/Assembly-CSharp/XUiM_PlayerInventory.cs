using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Audio;
using UnityEngine;

// Token: 0x0200114E RID: 4430
public class XUiM_PlayerInventory : XUiModel, IInventory
{
	// Token: 0x140000E5 RID: 229
	// (add) Token: 0x06008BA9 RID: 35753 RVA: 0x003540C4 File Offset: 0x003522C4
	// (remove) Token: 0x06008BAA RID: 35754 RVA: 0x003540FC File Offset: 0x003522FC
	public event XUiEvent_BackpackItemsChanged OnBackpackItemsChanged;

	// Token: 0x140000E6 RID: 230
	// (add) Token: 0x06008BAB RID: 35755 RVA: 0x00354134 File Offset: 0x00352334
	// (remove) Token: 0x06008BAC RID: 35756 RVA: 0x0035416C File Offset: 0x0035236C
	public event XUiEvent_ToolbeltItemsChanged OnToolbeltItemsChanged;

	// Token: 0x140000E7 RID: 231
	// (add) Token: 0x06008BAD RID: 35757 RVA: 0x003541A4 File Offset: 0x003523A4
	// (remove) Token: 0x06008BAE RID: 35758 RVA: 0x003541DC File Offset: 0x003523DC
	public event XUiEvent_CurrencyChanged OnCurrencyChanged;

	// Token: 0x1700104C RID: 4172
	// (get) Token: 0x06008BAF RID: 35759 RVA: 0x00354211 File Offset: 0x00352411
	public Bag Backpack
	{
		get
		{
			return this.backpack;
		}
	}

	// Token: 0x1700104D RID: 4173
	// (get) Token: 0x06008BB0 RID: 35760 RVA: 0x00354219 File Offset: 0x00352419
	public Inventory Toolbelt
	{
		get
		{
			return this.toolbelt;
		}
	}

	// Token: 0x1700104E RID: 4174
	// (get) Token: 0x06008BB1 RID: 35761 RVA: 0x00354221 File Offset: 0x00352421
	// (set) Token: 0x06008BB2 RID: 35762 RVA: 0x00354229 File Offset: 0x00352429
	public int CurrencyAmount
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700104F RID: 4175
	// (get) Token: 0x06008BB3 RID: 35763 RVA: 0x00354232 File Offset: 0x00352432
	public int QuickSwapSlot
	{
		get
		{
			return this.toolbelt.GetBestQuickSwapSlot();
		}
	}

	// Token: 0x06008BB4 RID: 35764 RVA: 0x00354240 File Offset: 0x00352440
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

	// Token: 0x06008BB5 RID: 35765 RVA: 0x003542E8 File Offset: 0x003524E8
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

	// Token: 0x06008BB6 RID: 35766 RVA: 0x00354301 File Offset: 0x00352501
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

	// Token: 0x06008BB7 RID: 35767 RVA: 0x0035431A File Offset: 0x0035251A
	public bool AddItemToPreferredToolbeltSlot(ItemStack _itemStack, int _slot)
	{
		return this.toolbelt.AddItemAtSlot(_itemStack, _slot);
	}

	// Token: 0x06008BB8 RID: 35768 RVA: 0x00354329 File Offset: 0x00352529
	public bool AddItemNoPartial(ItemStack _itemStack, bool _playCollectSound = true)
	{
		return (this.backpack.CanStack(_itemStack) || this.toolbelt.CanStack(_itemStack)) && this.AddItem(_itemStack, _playCollectSound);
	}

	// Token: 0x06008BB9 RID: 35769 RVA: 0x00354354 File Offset: 0x00352554
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

	// Token: 0x06008BBA RID: 35770 RVA: 0x0035446C File Offset: 0x0035266C
	public void SortStacks()
	{
		if (EffectManager.GetValue(PassiveEffects.ShuffledBackpack, null, 0f, this.localPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false) == 0f)
		{
			this.backpack.ItemGrid.Sort();
		}
	}

	// Token: 0x06008BBB RID: 35771 RVA: 0x003544B8 File Offset: 0x003526B8
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
				equipment.SetSlotItem(equipSlot, itemStack.itemValue);
				QuestEventManager.Current.WoreItem(itemStack.itemValue);
				itemStack.count = 0;
				itemStack = new ItemStack(slotItem, 1);
			}
			else
			{
				equipment.SetSlotItem(equipSlot, itemStack.itemValue);
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
					if (itemClassArmor2 != null && !this.xui.PlayerEquipment.IsEquipmentTypeWorn(itemClassArmor2.EquipSlot) && this.localPlayer.equipment.ReturnItem(itemStack2))
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
				if (itemClassArmor3 != null && !this.xui.PlayerEquipment.IsEquipmentTypeWorn(itemClassArmor3.EquipSlot) && this.localPlayer.equipment.ReturnItem(itemStack))
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

	// Token: 0x06008BBC RID: 35772 RVA: 0x00354954 File Offset: 0x00352B54
	public int CountAvailableSpaceForItem(ItemValue _itemValue, bool _limitToOneStack = true)
	{
		ItemStack itemStack = new ItemStack(_itemValue, 1);
		int maxCount = _itemValue.ItemClass.MaxCount;
		int num = 0;
		for (int i = 0; i < this.backpack.SlotCount; i++)
		{
			ItemStack itemStack2 = this.backpack.ItemGrid[i];
			if (itemStack2.IsEmpty())
			{
				num += maxCount;
			}
			else if (itemStack.CanStackWith(itemStack2, false))
			{
				num += maxCount - itemStack2.count;
			}
			if (_limitToOneStack && num >= maxCount)
			{
				return maxCount;
			}
		}
		for (int j = 0; j < this.toolbelt.Length; j++)
		{
			ItemStack item = this.toolbelt.GetItem(j);
			if (item.IsEmpty())
			{
				num += maxCount;
			}
			else if (itemStack.CanStackWith(item, false))
			{
				num += maxCount - item.count;
			}
			if (_limitToOneStack && num > maxCount)
			{
				return maxCount;
			}
		}
		return num;
	}

	// Token: 0x06008BBD RID: 35773 RVA: 0x00354A28 File Offset: 0x00352C28
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

	// Token: 0x06008BBE RID: 35774 RVA: 0x00354A8C File Offset: 0x00352C8C
	public bool AddItems(ItemStack[] _itemStacks)
	{
		bool flag = true;
		for (int i = 0; i < _itemStacks.Length; i++)
		{
			flag &= this.AddItem(_itemStacks[i]);
		}
		return flag;
	}

	// Token: 0x06008BBF RID: 35775 RVA: 0x00354AB8 File Offset: 0x00352CB8
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
		if (_preferences.equipment != null)
		{
			int num = Utils.FastMin(this.localPlayer.equipment.GetSlotCount(), _preferences.equipment.Length);
			int num2;
			int k;
			for (k = 0; k < num; k = num2 + 1)
			{
				if (this.localPlayer.equipment.GetSlotItem(k).IsEmpty())
				{
					XUiC_ItemStack xuiC_ItemStack = itemStackControllers.FirstOrDefault<XUiC_ItemStack>((XUiC_ItemStack _stack) => _stack.ItemStack.itemValue.Equals(_preferences.equipment[k]));
					if (((xuiC_ItemStack != null) ? xuiC_ItemStack.ItemStack : null) != null && !xuiC_ItemStack.ItemStack.IsEmpty())
					{
						this.localPlayer.equipment.SetSlotItem(k, xuiC_ItemStack.ItemStack.itemValue);
						ItemStack itemStack = xuiC_ItemStack.ItemStack;
						num2 = itemStack.count;
						itemStack.count = num2 - 1;
						if (xuiC_ItemStack.ItemStack.IsEmpty())
						{
							xuiC_ItemStack.ItemStack = ItemStack.Empty;
							_srcGrid.HandleSlotChangedEvent(xuiC_ItemStack.SlotNumber, ItemStack.Empty);
						}
						flag = true;
					}
				}
				num2 = k;
			}
		}
		this.xui.PlayerEquipment.RefreshEquipment();
		this.localPlayer.UpdateBagpackSize();
		if (_preferences.toolbelt != null)
		{
			int l = 0;
			while (l < _preferences.toolbelt.Length && l < this.toolbelt.Length)
			{
				if (this.toolbelt.GetItem(l).IsEmpty())
				{
					int type2 = _preferences.toolbelt[l].itemValue.type;
					XUiC_ItemStack xuiC_ItemStack2;
					if (hashSet.Contains(type2))
					{
						xuiC_ItemStack2 = itemStackControllers.FirstOrDefault<XUiC_ItemStack>((XUiC_ItemStack _stack) => _stack.ItemStack.itemValue.type == type2);
					}
					else
					{
						xuiC_ItemStack2 = itemStackControllers.FirstOrDefault<XUiC_ItemStack>((XUiC_ItemStack _stack) => _stack.ItemStack.Equals(_preferences.toolbelt[l]));
					}
					if (((xuiC_ItemStack2 != null) ? xuiC_ItemStack2.ItemStack : null) != null && !xuiC_ItemStack2.ItemStack.IsEmpty())
					{
						this.toolbelt.SetItem(l, xuiC_ItemStack2.ItemStack);
						xuiC_ItemStack2.ItemStack = ItemStack.Empty;
						_srcGrid.HandleSlotChangedEvent(xuiC_ItemStack2.SlotNumber, ItemStack.Empty);
						flag = true;
					}
				}
				int num2 = l;
				l = num2 + 1;
			}
		}
		if (_preferences.bag != null)
		{
			int i = 0;
			while (i < _preferences.bag.Length && i < this.backpack.SlotCount)
			{
				if (this.backpack.ItemGrid[i].IsEmpty())
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
						this.backpack.SetSlot(i, xuiC_ItemStack3.ItemStack);
						xuiC_ItemStack3.ItemStack = ItemStack.Empty;
						_srcGrid.HandleSlotChangedEvent(xuiC_ItemStack3.SlotNumber, ItemStack.Empty);
						flag = true;
					}
				}
				int num2 = i;
				i = num2 + 1;
			}
		}
		return flag;
	}

	// Token: 0x06008BC0 RID: 35776 RVA: 0x00354F77 File Offset: 0x00353177
	public bool AddItemToBackpack(ItemStack _itemStack)
	{
		this.backpack.TryStackItem(0, _itemStack);
		return _itemStack.count > 0 && this.backpack.AddItem(_itemStack);
	}

	// Token: 0x06008BC1 RID: 35777 RVA: 0x00354FA1 File Offset: 0x003531A1
	public bool AddItemToToolbelt(ItemStack _itemStack)
	{
		this.toolbelt.TryStackItem(0, _itemStack);
		return _itemStack.count > 0 && this.toolbelt.AddItem(_itemStack);
	}

	// Token: 0x06008BC2 RID: 35778 RVA: 0x00354FCB File Offset: 0x003531CB
	public bool HasItem(ItemStack _itemStack)
	{
		return this.HasItems(new ItemStack[] { _itemStack }, 1);
	}

	// Token: 0x06008BC3 RID: 35779 RVA: 0x00354FE0 File Offset: 0x003531E0
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

	// Token: 0x06008BC4 RID: 35780 RVA: 0x00355053 File Offset: 0x00353253
	public void RemoveItem(ItemStack _itemStack)
	{
		this.RemoveItems(new ItemStack[] { _itemStack }, 1, null);
	}

	// Token: 0x06008BC5 RID: 35781 RVA: 0x00355068 File Offset: 0x00353268
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

	// Token: 0x06008BC6 RID: 35782 RVA: 0x003550E8 File Offset: 0x003532E8
	public int GetItemCount(ItemValue _itemValue)
	{
		return 0 + this.backpack.GetItemCount(_itemValue, -1, -1, true) + this.toolbelt.GetItemCount(_itemValue, false, -1, -1, true);
	}

	// Token: 0x06008BC7 RID: 35783 RVA: 0x0035510C File Offset: 0x0035330C
	public int GetItemCountWithMods(ItemValue _itemValue)
	{
		return 0 + this.backpack.GetItemCount(_itemValue, -1, -1, false) + this.toolbelt.GetItemCount(_itemValue, false, -1, -1, false);
	}

	// Token: 0x06008BC8 RID: 35784 RVA: 0x00355130 File Offset: 0x00353330
	public int GetItemCount(int _itemId)
	{
		ItemValue itemValue = new ItemValue(_itemId, false);
		return 0 + this.backpack.GetItemCount(itemValue, -1, -1, true) + this.toolbelt.GetItemCount(itemValue, false, -1, -1, true);
	}

	// Token: 0x06008BC9 RID: 35785 RVA: 0x00355167 File Offset: 0x00353367
	public List<ItemStack> GetAllItemStacks()
	{
		List<ItemStack> list = new List<ItemStack>();
		list.AddRange(this.GetBackpackItemStacks());
		list.AddRange(this.GetToolbeltItemStacks());
		return list;
	}

	// Token: 0x06008BCA RID: 35786 RVA: 0x00355186 File Offset: 0x00353386
	public ItemStack[] GetBackpackItemStacks()
	{
		return this.backpack.ItemGrid.CloneItems();
	}

	// Token: 0x06008BCB RID: 35787 RVA: 0x00355198 File Offset: 0x00353398
	public ItemStack[] GetToolbeltItemStacks()
	{
		return this.toolbelt.ItemGrid.CloneItems();
	}

	// Token: 0x06008BCC RID: 35788 RVA: 0x003551AA File Offset: 0x003533AA
	public void SetBackpackItemStacks(ItemStack[] _itemStacks)
	{
		this.backpack.SetSlots(_itemStacks, false);
		this.dispatchBackpackItemsChanged();
	}

	// Token: 0x06008BCD RID: 35789 RVA: 0x003551BF File Offset: 0x003533BF
	public void SetToolbeltItemStacks(ItemStack[] _itemStacks)
	{
		this.toolbelt.SetSlots(_itemStacks);
		this.dispatchToolbeltItemsChanged();
	}

	// Token: 0x06008BCE RID: 35790 RVA: 0x003551D4 File Offset: 0x003533D4
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

	// Token: 0x06008BCF RID: 35791 RVA: 0x00355210 File Offset: 0x00353410
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

	// Token: 0x06008BD0 RID: 35792 RVA: 0x00355272 File Offset: 0x00353472
	public bool AddItem(ItemStack _itemStack)
	{
		return this.AddItem(_itemStack, true);
	}

	// Token: 0x06008BD1 RID: 35793 RVA: 0x0035527C File Offset: 0x0035347C
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

	// Token: 0x06008BD2 RID: 35794 RVA: 0x0035531D File Offset: 0x0035351D
	public bool HasItem(ItemValue _item)
	{
		return this.GetItemCount(_item) > 0;
	}

	// Token: 0x06008BD3 RID: 35795 RVA: 0x0035532C File Offset: 0x0035352C
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

	// Token: 0x0400675D RID: 26461
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUi xui;

	// Token: 0x0400675E RID: 26462
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly EntityPlayerLocal localPlayer;

	// Token: 0x0400675F RID: 26463
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Bag backpack;

	// Token: 0x04006760 RID: 26464
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Inventory toolbelt;

	// Token: 0x04006761 RID: 26465
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ItemValue currencyItem;
}
