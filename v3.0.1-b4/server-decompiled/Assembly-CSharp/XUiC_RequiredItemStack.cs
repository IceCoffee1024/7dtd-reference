using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FFE RID: 4094
[Preserve]
public class XUiC_RequiredItemStack : XUiC_ItemStack
{
	// Token: 0x140000DA RID: 218
	// (add) Token: 0x06008159 RID: 33113 RVA: 0x00312E58 File Offset: 0x00311058
	// (remove) Token: 0x0600815A RID: 33114 RVA: 0x00312E90 File Offset: 0x00311090
	public event XUiEvent_RequiredSlotFailedSwapEventHandler FailedSwap;

	// Token: 0x17000EF8 RID: 3832
	// (get) Token: 0x0600815B RID: 33115 RVA: 0x00312EC5 File Offset: 0x003110C5
	public bool HasAllowedItemClass
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.RequiredType == XUiC_RequiredItemStack.RequiredTypes.ItemClass && this.allowedItemClasses.Count > 0 && this.allowedItemClasses[0] != null;
		}
	}

	// Token: 0x17000EF9 RID: 3833
	// (get) Token: 0x0600815C RID: 33116 RVA: 0x00312EEE File Offset: 0x003110EE
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

	// Token: 0x17000EFA RID: 3834
	// (get) Token: 0x0600815D RID: 33117 RVA: 0x00312F20 File Offset: 0x00311120
	public override string ItemIconColor
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
				return "255,255,255,255";
			}
			base.GreyedOut = false;
			return "255,255,255,0";
		}
	}

	// Token: 0x0600815E RID: 33118 RVA: 0x00312F6C File Offset: 0x0031116C
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

	// Token: 0x0600815F RID: 33119 RVA: 0x00313017 File Offset: 0x00311217
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

	// Token: 0x06008160 RID: 33120 RVA: 0x00313048 File Offset: 0x00311248
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void HandleDropOne()
	{
		ItemStack currentStack = this.xui.DragAndDropWindow.CurrentStack;
		if (!this.TryStack(currentStack, 1))
		{
			return;
		}
		this.xui.DragAndDropWindow.CurrentStack = currentStack;
		base.PlayPlaceSound(null);
	}

	// Token: 0x06008161 RID: 33121 RVA: 0x0031308C File Offset: 0x0031128C
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
			int num = ((this.OverrideStackCount == -1) ? itemClassOrMissing.Stacknumber.Value : Mathf.Min(itemClassOrMissing.Stacknumber.Value, this.OverrideStackCount)) - this.itemStack.count;
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

	// Token: 0x06008162 RID: 33122 RVA: 0x003131B2 File Offset: 0x003113B2
	public void ClearAllowedItemClasses()
	{
		this.IsDirty = true;
		this.allowedItemClasses.Clear();
	}

	// Token: 0x06008163 RID: 33123 RVA: 0x003131C6 File Offset: 0x003113C6
	public void SetAllowedItemClassSingle(string _itemClassName)
	{
		this.SetAllowedItemClassSingle(ItemClass.GetItemClass(_itemClassName, false));
	}

	// Token: 0x06008164 RID: 33124 RVA: 0x003131D5 File Offset: 0x003113D5
	public void SetAllowedItemClassSingle(ItemClass _itemClass)
	{
		this.IsDirty = true;
		this.allowedItemClasses.Clear();
		if (_itemClass != null)
		{
			this.allowedItemClasses.Add(_itemClass);
		}
	}

	// Token: 0x06008165 RID: 33125 RVA: 0x003131F8 File Offset: 0x003113F8
	public void SetAllowedItemClasses(ItemClass[] _itemClasses)
	{
		this.IsDirty = true;
		this.allowedItemClasses.Clear();
		this.allowedItemClasses.AddRange(_itemClasses);
	}

	// Token: 0x06008166 RID: 33126 RVA: 0x00313218 File Offset: 0x00311418
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

	// Token: 0x06008168 RID: 33128 RVA: 0x0031328C File Offset: 0x0031148C
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

	// Token: 0x06008169 RID: 33129 RVA: 0x00313334 File Offset: 0x00311534
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

	// Token: 0x0600816A RID: 33130 RVA: 0x0031336E File Offset: 0x0031156E
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

	// Token: 0x04005FB6 RID: 24502
	public XUiC_RequiredItemStack.RequiredTypes RequiredType;

	// Token: 0x04005FB7 RID: 24503
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ItemClass> allowedItemClasses = new List<ItemClass>();

	// Token: 0x04005FB8 RID: 24504
	public bool RequiredItemOnly = true;

	// Token: 0x04005FB9 RID: 24505
	public bool TakeOnly;

	// Token: 0x02000FFF RID: 4095
	public enum RequiredTypes
	{
		// Token: 0x04005FBB RID: 24507
		ItemClass,
		// Token: 0x04005FBC RID: 24508
		IsPart,
		// Token: 0x04005FBD RID: 24509
		HasQuality,
		// Token: 0x04005FBE RID: 24510
		HasQualityNoParts
	}
}
