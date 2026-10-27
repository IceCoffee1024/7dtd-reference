using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001036 RID: 4150
[Preserve]
public class XUiC_RequiredItemStack : XUiC_ItemStack
{
	// Token: 0x140000DA RID: 218
	// (add) Token: 0x060082AC RID: 33452 RVA: 0x0031C3BC File Offset: 0x0031A5BC
	// (remove) Token: 0x060082AD RID: 33453 RVA: 0x0031C3F4 File Offset: 0x0031A5F4
	public event XUiEvent_RequiredSlotFailedSwapEventHandler FailedSwap;

	// Token: 0x17000F63 RID: 3939
	// (get) Token: 0x060082AE RID: 33454 RVA: 0x0031C429 File Offset: 0x0031A629
	public bool HasAllowedItemClass
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.RequiredType == XUiC_RequiredItemStack.RequiredTypes.ItemClass && this.allowedItemClasses.Count > 0 && this.allowedItemClasses[0] != null;
		}
	}

	// Token: 0x17000F64 RID: 3940
	// (get) Token: 0x060082AF RID: 33455 RVA: 0x0031C452 File Offset: 0x0031A652
	public override string ItemIcon
	{
		get
		{
			if (this.HasAllowedItemClass && this.itemStack.IsEmpty())
			{
				return this.allowedItemClasses[0].GetIconName();
			}
			return base.ItemIcon;
		}
	}

	// Token: 0x17000F65 RID: 3941
	// (get) Token: 0x060082B0 RID: 33456 RVA: 0x0031C484 File Offset: 0x0031A684
	public override Color ItemIconColor
	{
		get
		{
			if (base.itemClass != null)
			{
				base.GreyedOut = false;
				return base.ItemIconColor;
			}
			if (this.HasAllowedItemClass && !base.StackLock)
			{
				base.GreyedOut = true;
				return Color.white;
			}
			base.GreyedOut = false;
			return new Color(1f, 1f, 1f, 0f);
		}
	}

	// Token: 0x060082B1 RID: 33457 RVA: 0x0031C4E4 File Offset: 0x0031A6E4
	public bool ItemAllowed(ItemStack _stack)
	{
		bool flag;
		switch (this.RequiredType)
		{
		case XUiC_RequiredItemStack.RequiredTypes.ItemClass:
			flag = !this.HasAllowedItemClass || !this.RequiredItemOnly || this.allowedItemClasses.IndexOf(_stack.itemValue.ItemClass) >= 0;
			break;
		case XUiC_RequiredItemStack.RequiredTypes.IsPart:
			flag = _stack.itemValue.ItemClass.PartParentId != null;
			break;
		case XUiC_RequiredItemStack.RequiredTypes.HasQuality:
			flag = _stack.itemValue.HasQuality;
			break;
		case XUiC_RequiredItemStack.RequiredTypes.HasQualityNoParts:
			flag = _stack.itemValue.HasQuality && !_stack.itemValue.ItemClass.HasSubItems;
			break;
		default:
			flag = true;
			break;
		}
		return flag;
	}

	// Token: 0x060082B2 RID: 33458 RVA: 0x0031C58F File Offset: 0x0031A78F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool CanSwap(ItemStack _stack)
	{
		if (this.TakeOnly && !_stack.IsEmpty())
		{
			return false;
		}
		bool flag = this.ItemAllowed(_stack);
		if (!flag)
		{
			XUiEvent_RequiredSlotFailedSwapEventHandler failedSwap = this.FailedSwap;
			if (failedSwap == null)
			{
				return flag;
			}
			failedSwap(_stack);
		}
		return flag;
	}

	// Token: 0x060082B3 RID: 33459 RVA: 0x0031C5C0 File Offset: 0x0031A7C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void HandleDropOne()
	{
		ItemStack currentStack = this.xui.DragAndDropWindow.CurrentStack;
		if (!this.TryStack(currentStack, 1))
		{
			return;
		}
		this.xui.DragAndDropWindow.SetCurrentStack(currentStack, true);
		base.PlayPlaceSound(null);
	}

	// Token: 0x060082B4 RID: 33460 RVA: 0x0031C604 File Offset: 0x0031A804
	public bool TryStack(ItemStack _stack, int _countToPlace = -1)
	{
		if (_stack.IsEmpty())
		{
			return false;
		}
		if (_countToPlace < 0)
		{
			_countToPlace = _stack.count;
		}
		if (_countToPlace > _stack.count)
		{
			_countToPlace = _stack.count;
		}
		if (this.itemStack.IsEmpty())
		{
			if (!this.ItemAllowed(_stack))
			{
				return false;
			}
			ItemStack itemStack = _stack.Clone();
			itemStack.count = _countToPlace;
			_stack.count -= _countToPlace;
			base.ItemStack = itemStack;
			base.HandleSlotChangeEvent();
			return true;
		}
		else
		{
			if (_stack.itemValue.type != this.itemStack.itemValue.type)
			{
				return false;
			}
			ItemClass itemClassOrMissing = this.itemStack.itemValue.ItemClassOrMissing;
			int num = ((this.OverrideStackCount == -1) ? itemClassOrMissing.MaxCount : Mathf.Min(itemClassOrMissing.MaxCount, this.OverrideStackCount)) - this.itemStack.count;
			if (num <= 0)
			{
				return false;
			}
			if (num < _countToPlace)
			{
				_countToPlace = num;
			}
			ItemStack itemStack2 = this.itemStack.Clone();
			itemStack2.count += _countToPlace;
			_stack.count -= _countToPlace;
			base.ItemStack = itemStack2;
			base.HandleSlotChangeEvent();
			return true;
		}
	}

	// Token: 0x060082B5 RID: 33461 RVA: 0x0031C720 File Offset: 0x0031A920
	public void ClearAllowedItemClasses()
	{
		this.IsDirty = true;
		this.allowedItemClasses.Clear();
	}

	// Token: 0x060082B6 RID: 33462 RVA: 0x0031C734 File Offset: 0x0031A934
	public void SetAllowedItemClassSingle(string _itemClassName)
	{
		this.SetAllowedItemClassSingle(ItemClass.GetItemClass(_itemClassName, false));
	}

	// Token: 0x060082B7 RID: 33463 RVA: 0x0031C743 File Offset: 0x0031A943
	public void SetAllowedItemClassSingle(ItemClass _itemClass)
	{
		this.IsDirty = true;
		this.allowedItemClasses.Clear();
		if (_itemClass != null)
		{
			this.allowedItemClasses.Add(_itemClass);
		}
	}

	// Token: 0x060082B8 RID: 33464 RVA: 0x0031C766 File Offset: 0x0031A966
	public void SetAllowedItemClasses(ItemClass[] _itemClasses)
	{
		this.IsDirty = true;
		this.allowedItemClasses.Clear();
		this.allowedItemClasses.AddRange(_itemClasses);
	}

	// Token: 0x060082B9 RID: 33465 RVA: 0x0031C788 File Offset: 0x0031A988
	public static void ParseItemClassesFromString(IList<ItemClass> _targetList, string _itemClassesString)
	{
		XUiC_RequiredItemStack.<>c__DisplayClass22_0 CS$<>8__locals1;
		CS$<>8__locals1._targetList = _targetList;
		if (string.IsNullOrEmpty(_itemClassesString))
		{
			return;
		}
		foreach (string text in _itemClassesString.Split('+', StringSplitOptions.RemoveEmptyEntries))
		{
			if (text.StartsWith("tags(", StringComparison.Ordinal))
			{
				XUiC_RequiredItemStack.<ParseItemClassesFromString>g__ParseTagsBasedEntry|22_0(text, ref CS$<>8__locals1);
			}
			else
			{
				XUiC_RequiredItemStack.<ParseItemClassesFromString>g__AddItemClassByName|22_1(text, ref CS$<>8__locals1);
			}
		}
	}

	// Token: 0x060082BB RID: 33467 RVA: 0x0031C7FC File Offset: 0x0031A9FC
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <ParseItemClassesFromString>g__ParseTagsBasedEntry|22_0(string _text, ref XUiC_RequiredItemStack.<>c__DisplayClass22_0 A_1)
	{
		if (_text[_text.Length - 1] != ')')
		{
			Log.Warning("ItemClasses: Malformed tags() entry: '" + _text + "'. From: " + StackTraceUtility.ExtractStackTrace());
			return;
		}
		foreach (ItemClass itemClass in ItemClass.GetItemsWithTag(FastTags<TagGroup.Global>.Parse(_text.Substring("tags(".Length, _text.Length - 1 - "tags(".Length))))
		{
			XUiC_RequiredItemStack.<ParseItemClassesFromString>g__AddItemClass|22_2(itemClass, ref A_1);
		}
	}

	// Token: 0x060082BC RID: 33468 RVA: 0x0031C8A4 File Offset: 0x0031AAA4
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <ParseItemClassesFromString>g__AddItemClassByName|22_1(string _itemClassName, ref XUiC_RequiredItemStack.<>c__DisplayClass22_0 A_1)
	{
		ItemClass itemClass = ItemClass.GetItemClass(_itemClassName, false);
		if (itemClass == null)
		{
			Log.Warning("ItemClasses: ItemClass '" + _itemClassName + "' not found. From: " + StackTraceUtility.ExtractStackTrace());
			return;
		}
		XUiC_RequiredItemStack.<ParseItemClassesFromString>g__AddItemClass|22_2(itemClass, ref A_1);
	}

	// Token: 0x060082BD RID: 33469 RVA: 0x0031C8DE File Offset: 0x0031AADE
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <ParseItemClassesFromString>g__AddItemClass|22_2(ItemClass _itemClass, ref XUiC_RequiredItemStack.<>c__DisplayClass22_0 A_1)
	{
		if (A_1._targetList.IndexOf(_itemClass) >= 0)
		{
			return;
		}
		A_1._targetList.Add(_itemClass);
	}

	// Token: 0x040060E4 RID: 24804
	public XUiC_RequiredItemStack.RequiredTypes RequiredType;

	// Token: 0x040060E5 RID: 24805
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ItemClass> allowedItemClasses = new List<ItemClass>();

	// Token: 0x040060E6 RID: 24806
	public bool RequiredItemOnly = true;

	// Token: 0x040060E7 RID: 24807
	public bool TakeOnly;

	// Token: 0x02001037 RID: 4151
	public enum RequiredTypes
	{
		// Token: 0x040060E9 RID: 24809
		ItemClass,
		// Token: 0x040060EA RID: 24810
		IsPart,
		// Token: 0x040060EB RID: 24811
		HasQuality,
		// Token: 0x040060EC RID: 24812
		HasQualityNoParts
	}
}
