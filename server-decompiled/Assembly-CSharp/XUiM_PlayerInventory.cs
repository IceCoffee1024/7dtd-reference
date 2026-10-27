using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Audio;
using UnityEngine;

// Token: 0x0200115E RID: 4446
public class XUiM_PlayerInventory : XUiModel, IInventory
{
	// Token: 0x140000E5 RID: 229
	// (add) Token: 0x06008BFD RID: 35837 RVA: 0x00356174 File Offset: 0x00354374
	// (remove) Token: 0x06008BFE RID: 35838 RVA: 0x003561AC File Offset: 0x003543AC
	public event XUiEvent_BackpackItemsChanged OnBackpackItemsChanged;

	// Token: 0x140000E6 RID: 230
	// (add) Token: 0x06008BFF RID: 35839 RVA: 0x003561E4 File Offset: 0x003543E4
	// (remove) Token: 0x06008C00 RID: 35840 RVA: 0x0035621C File Offset: 0x0035441C
	public event XUiEvent_ToolbeltItemsChanged OnToolbeltItemsChanged;

	// Token: 0x140000E7 RID: 231
	// (add) Token: 0x06008C01 RID: 35841 RVA: 0x00356254 File Offset: 0x00354454
	// (remove) Token: 0x06008C02 RID: 35842 RVA: 0x0035628C File Offset: 0x0035448C
	public event XUiEvent_CurrencyChanged OnCurrencyChanged;

	// Token: 0x17001057 RID: 4183
	// (get) Token: 0x06008C03 RID: 35843 RVA: 0x003562C1 File Offset: 0x003544C1
	public Bag Backpack
	{
		get
		{
			return this.backpack;
		}
	}

	// Token: 0x17001058 RID: 4184
	// (get) Token: 0x06008C04 RID: 35844 RVA: 0x003562C9 File Offset: 0x003544C9
	public Inventory Toolbelt
	{
		get
		{
			return this.toolbelt;
		}
	}

	// Token: 0x17001059 RID: 4185
	// (get) Token: 0x06008C05 RID: 35845 RVA: 0x003562D1 File Offset: 0x003544D1
	// (set) Token: 0x06008C06 RID: 35846 RVA: 0x003562D9 File Offset: 0x003544D9
	public int CurrencyAmount
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700105A RID: 4186
	// (get) Token: 0x06008C07 RID: 35847 RVA: 0x003562E2 File Offset: 0x003544E2
	public int QuickSwapSlot
	{
		get
		{
			return this.toolbelt.GetBestQuickSwapSlot();
		}
	}

	// Token: 0x06008C08 RID: 35848 RVA: 0x003562F0 File Offset: 0x003544F0
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

	// Token: 0x06008C09 RID: 35849 RVA: 0x00356398 File Offset: 0x00354598
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

	// Token: 0x06008C0A RID: 35850 RVA: 0x003563B1 File Offset: 0x003545B1
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

	// Token: 0x06008C0B RID: 35851 RVA: 0x003563CA File Offset: 0x003545CA
	public bool AddItemToPreferredToolbeltSlot(ItemStack _itemStack, int _slot)
	{
		return this.toolbelt.AddItemAtSlot(_itemStack, _slot);
	}

	// Token: 0x06008C0C RID: 35852 RVA: 0x003563D9 File Offset: 0x003545D9
	public bool AddItemNoPartial(ItemStack _itemStack, bool _playCollectSound = true)
	{
		return (this.backpack.CanStack(_itemStack) || this.toolbelt.CanStack(_itemStack)) && this.AddItem(_itemStack, _playCollectSound);
	}

	// Token: 0x06008C0D RID: 35853 RVA: 0x00356404 File Offset: 0x00354604
	public bool CanSwapItems(ItemStack _removedStack, ItemStack _addedStack, int _slotNumber = -1)
	{
		List<ItemStack> allItemStacks = this.GetAllItemStacks();
		int num = _removedStack.count;
		int num2 = _addedStack.count;
		int maxCount = _removedStack.itemValue.ItemClass.MaxCount;
		int maxCount2 = _addedStack.itemValue.ItemClass.MaxCount;
		for (int i = 0; i < allItemStacks.Count; i++)
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

	// Token: 0x06008C0E RID: 35854 RVA: 0x00356518 File Offset: 0x00354718
	public void SortStacks()
	{
		if (EffectManager.GetValue(PassiveEffects.ShuffledBackpack, null, 0f, this.localPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false) == 0f)
		{
			this.backpack.ItemGrid.Sort();
		}
	}

	// Token: 0x06008C0F RID: 35855 RVA: 0x00356564 File Offset: 0x00354764
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

	// Token: 0x06008C10 RID: 35856 RVA: 0x00356A00 File Offset: 0x00354C00
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

	// Token: 0x06008C11 RID: 35857 RVA: 0x00356AD4 File Offset: 0x00354CD4
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

	// Token: 0x06008C12 RID: 35858 RVA: 0x00356B38 File Offset: 0x00354D38
	public bool AddItems(ItemStack[] _itemStacks)
	{
		bool flag = true;
		for (int i = 0; i < _itemStacks.Length; i++)
		{
			flag &= this.AddItem(_itemStacks[i]);
		}
		return flag;
	}

	// Token: 0x06008C13 RID: 35859 RVA: 0x00356B64 File Offset: 0x00354D64
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

	// Token: 0x06008C14 RID: 35860 RVA: 0x00357023 File Offset: 0x00355223
	public bool AddItemToBackpack(ItemStack _itemStack)
	{
		this.backpack.TryStackItem(0, _itemStack);
		return _itemStack.count > 0 && this.backpack.AddItem(_itemStack);
	}

	// Token: 0x06008C15 RID: 35861 RVA: 0x0035704D File Offset: 0x0035524D
	public bool AddItemToToolbelt(ItemStack _itemStack)
	{
		this.toolbelt.TryStackItem(0, _itemStack);
		return _itemStack.count > 0 && this.toolbelt.AddItem(_itemStack);
	}

	// Token: 0x06008C16 RID: 35862 RVA: 0x00357077 File Offset: 0x00355277
	public bool HasItem(ItemStack _itemStack)
	{
		return this.HasItems(new ItemStack[] { _itemStack }, 1);
	}

	// Token: 0x06008C17 RID: 35863 RVA: 0x0035708C File Offset: 0x0035528C
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

	// Token: 0x06008C18 RID: 35864 RVA: 0x003570FF File Offset: 0x003552FF
	public void RemoveItem(ItemStack _itemStack)
	{
		this.RemoveItems(new ItemStack[] { _itemStack }, 1, null);
	}

	// Token: 0x06008C19 RID: 35865 RVA: 0x00357114 File Offset: 0x00355314
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

	// Token: 0x06008C1A RID: 35866 RVA: 0x00357194 File Offset: 0x00355394
	public int GetItemCount(ItemValue _itemValue)
	{
		return 0 + this.backpack.GetItemCount(_itemValue, -1, -1, true) + this.toolbelt.GetItemCount(_itemValue, false, -1, -1, true);
	}

	// Token: 0x06008C1B RID: 35867 RVA: 0x003571B8 File Offset: 0x003553B8
	public int GetItemCountWithMods(ItemValue _itemValue)
	{
		return 0 + this.backpack.GetItemCount(_itemValue, -1, -1, false) + this.toolbelt.GetItemCount(_itemValue, false, -1, -1, false);
	}

	// Token: 0x06008C1C RID: 35868 RVA: 0x003571DC File Offset: 0x003553DC
	public int GetItemCount(int _itemId)
	{
		ItemValue itemValue = new ItemValue(_itemId, false);
		return 0 + this.backpack.GetItemCount(itemValue, -1, -1, true) + this.toolbelt.GetItemCount(itemValue, false, -1, -1, true);
	}

	// Token: 0x06008C1D RID: 35869 RVA: 0x00357213 File Offset: 0x00355413
	public List<ItemStack> GetAllItemStacks()
	{
		List<ItemStack> list = new List<ItemStack>();
		list.AddRange(this.GetBackpackItemStacks());
		list.AddRange(this.GetToolbeltItemStacks());
		return list;
	}

	// Token: 0x06008C1E RID: 35870 RVA: 0x00357232 File Offset: 0x00355432
	public ItemStack[] GetBackpackItemStacks()
	{
		return this.backpack.ItemGrid.CloneItems();
	}

	// Token: 0x06008C1F RID: 35871 RVA: 0x00357244 File Offset: 0x00355444
	public ItemStack[] GetToolbeltItemStacks()
	{
		return this.toolbelt.ItemGrid.CloneItems();
	}

	// Token: 0x06008C20 RID: 35872 RVA: 0x00357256 File Offset: 0x00355456
	public void SetBackpackItemStacks(ItemStack[] _itemStacks)
	{
		this.backpack.SetSlots(_itemStacks, false);
		this.dispatchBackpackItemsChanged();
	}

	// Token: 0x06008C21 RID: 35873 RVA: 0x0035726B File Offset: 0x0035546B
	public void SetToolbeltItemStacks(ItemStack[] _itemStacks)
	{
		this.toolbelt.SetSlots(_itemStacks);
		this.dispatchToolbeltItemsChanged();
	}

	// Token: 0x06008C22 RID: 35874 RVA: 0x00357280 File Offset: 0x00355480
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

	// Token: 0x06008C23 RID: 35875 RVA: 0x003572BC File Offset: 0x003554BC
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

	// Token: 0x06008C24 RID: 35876 RVA: 0x0035731E File Offset: 0x0035551E
	public bool AddItem(ItemStack _itemStack)
	{
		return this.AddItem(_itemStack, true);
	}

	// Token: 0x06008C25 RID: 35877 RVA: 0x00357328 File Offset: 0x00355528
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

	// Token: 0x06008C26 RID: 35878 RVA: 0x003573C9 File Offset: 0x003555C9
	public bool HasItem(ItemValue _item)
	{
		return this.GetItemCount(_item) > 0;
	}

	// Token: 0x06008C27 RID: 35879 RVA: 0x003573D8 File Offset: 0x003555D8
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

	// Token: 0x0400679D RID: 26525
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUi xui;

	// Token: 0x0400679E RID: 26526
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly EntityPlayerLocal localPlayer;

	// Token: 0x0400679F RID: 26527
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Bag backpack;

	// Token: 0x040067A0 RID: 26528
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Inventory toolbelt;

	// Token: 0x040067A1 RID: 26529
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ItemValue currencyItem;
}
