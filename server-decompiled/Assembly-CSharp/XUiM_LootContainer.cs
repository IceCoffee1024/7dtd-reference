using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02001151 RID: 4433
public class XUiM_LootContainer : XUiModel
{
	// Token: 0x06008B95 RID: 35733 RVA: 0x00354D6C File Offset: 0x00352F6C
	public static bool AddItem(ItemStack _itemStack, XUi _xui)
	{
		if (!_itemStack.CanMoveTo(XUiC_ItemStack.StackLocationTypes.LootContainer, -1))
		{
			return false;
		}
		if (_xui.LootContainer == null)
		{
			return false;
		}
		_xui.LootContainer.TryStackItem(0, _itemStack);
		return _itemStack.count > 0 && _xui.LootContainer.AddItem(_itemStack);
	}

	// Token: 0x06008B96 RID: 35734 RVA: 0x00354DAC File Offset: 0x00352FAC
	[return: TupleElementNames(new string[] { "_allMoved", "_anyMoved" })]
	public static ValueTuple<bool, bool> StashItems(XUiController _srcWindow, XUiC_ItemStackGrid _srcGrid, IInventory _dstInventory, int _ignoreSlots, PackedBoolArray _ignoredSlots, XUiM_LootContainer.EItemMoveKind _moveKind, bool _startBottomRight)
	{
		if (_srcGrid == null || _dstInventory == null)
		{
			return new ValueTuple<bool, bool>(false, false);
		}
		float unscaledTime = Time.unscaledTime;
		if (_moveKind == XUiM_LootContainer.EItemMoveKind.FillOnlyFirstCreateSecond && unscaledTime - XUiM_LootContainer.lastStashTime < 2f)
		{
			_moveKind = XUiM_LootContainer.EItemMoveKind.FillAndCreate;
		}
		bool flag = true;
		bool flag2 = false;
		PreferenceTracker preferenceTracker = null;
		XUiC_LootWindow xuiC_LootWindow = _srcWindow as XUiC_LootWindow;
		if (xuiC_LootWindow != null)
		{
			preferenceTracker = xuiC_LootWindow.GetPreferenceTrackerFromTileEntity();
		}
		else
		{
			XUiC_BagContainer xuiC_BagContainer = _srcWindow as XUiC_BagContainer;
			if (xuiC_BagContainer != null)
			{
				Bag bag = xuiC_BagContainer.Bag;
				preferenceTracker = ((bag != null) ? bag.preferences : null);
			}
		}
		if (preferenceTracker != null && preferenceTracker.AnyPreferences)
		{
			XUiM_PlayerInventory xuiM_PlayerInventory = _dstInventory as XUiM_PlayerInventory;
			if (xuiM_PlayerInventory != null)
			{
				flag2 = xuiM_PlayerInventory.AddItemsUsingPreferenceTracker(_srcGrid, preferenceTracker);
			}
		}
		XUiC_ItemStack[] itemStackControllers = _srcGrid.GetItemStackControllers();
		int num = (_startBottomRight ? (itemStackControllers.Length - 1) : 0);
		while (_startBottomRight ? (num >= 0) : (num < itemStackControllers.Length))
		{
			if (!StackSortUtil.IsIgnoredSlot(_ignoreSlots, _ignoredSlots, num))
			{
				XUiC_ItemStack xuiC_ItemStack = itemStackControllers[num];
				if (!xuiC_ItemStack.StackLock)
				{
					ItemStack itemStack = xuiC_ItemStack.ItemStack;
					if (!xuiC_ItemStack.ItemStack.IsEmpty())
					{
						int count = itemStack.count;
						_dstInventory.TryStackItem(0, itemStack);
						if (itemStack.count > 0 && (_moveKind == XUiM_LootContainer.EItemMoveKind.All || (_moveKind == XUiM_LootContainer.EItemMoveKind.FillAndCreate && _dstInventory.HasItem(itemStack.itemValue))) && _dstInventory.AddItem(itemStack))
						{
							itemStack = ItemStack.Empty;
						}
						if (itemStack.count == 0)
						{
							itemStack = ItemStack.Empty;
						}
						else
						{
							flag = false;
						}
						if (count != itemStack.count)
						{
							xuiC_ItemStack.ForceSetItemStack(itemStack);
							flag2 = true;
						}
					}
				}
			}
			num = (_startBottomRight ? (num - 1) : (num + 1));
		}
		XUiM_LootContainer.lastStashTime = unscaledTime;
		return new ValueTuple<bool, bool>(flag, flag2);
	}

	// Token: 0x0400678D RID: 26509
	[PublicizedFrom(EAccessModifier.Private)]
	public static float lastStashTime;

	// Token: 0x0400678E RID: 26510
	[PublicizedFrom(EAccessModifier.Private)]
	public const float SecondClickMaxDelaySec = 2f;

	// Token: 0x02001152 RID: 4434
	public enum EItemMoveKind
	{
		// Token: 0x04006790 RID: 26512
		All,
		// Token: 0x04006791 RID: 26513
		FillOnly,
		// Token: 0x04006792 RID: 26514
		FillAndCreate,
		// Token: 0x04006793 RID: 26515
		FillOnlyFirstCreateSecond
	}
}
