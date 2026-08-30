using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001017 RID: 4119
[Preserve]
public class XUiC_RequiredItemStack : XUiC_ItemStack
{
	// Token: 0x140000DB RID: 219
	// (add) Token: 0x0600824B RID: 33355 RVA: 0x00317688 File Offset: 0x00315888
	// (remove) Token: 0x0600824C RID: 33356 RVA: 0x003176C0 File Offset: 0x003158C0
	public event XUiEvent_RequiredSlotFailedSwapEventHandler FailedSwap;

	// Token: 0x17000F31 RID: 3889
	// (get) Token: 0x0600824D RID: 33357 RVA: 0x003176F5 File Offset: 0x003158F5
	public bool HasAllowedItemClass
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.RequiredType == XUiC_RequiredItemStack.RequiredTypes.ItemClass && this.allowedItemClasses.Count > 0 && this.allowedItemClasses[0] != null;
		}
	}

	// Token: 0x17000F32 RID: 3890
	// (get) Token: 0x0600824E RID: 33358 RVA: 0x0031771E File Offset: 0x0031591E
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

	// Token: 0x17000F33 RID: 3891
	// (get) Token: 0x0600824F RID: 33359 RVA: 0x00317750 File Offset: 0x00315950
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

	// Token: 0x06008250 RID: 33360 RVA: 0x0031779C File Offset: 0x0031599C
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

	// Token: 0x06008251 RID: 33361 RVA: 0x00317847 File Offset: 0x00315A47
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

	// Token: 0x06008252 RID: 33362 RVA: 0x00317878 File Offset: 0x00315A78
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

	// Token: 0x06008253 RID: 33363 RVA: 0x003178BC File Offset: 0x00315ABC
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

	// Token: 0x06008254 RID: 33364 RVA: 0x003179D8 File Offset: 0x00315BD8
	public void ClearAllowedItemClasses()
	{
		this.IsDirty = true;
		this.allowedItemClasses.Clear();
	}

	// Token: 0x06008255 RID: 33365 RVA: 0x003179EC File Offset: 0x00315BEC
	public void SetAllowedItemClassSingle(string _itemClassName)
	{
		this.SetAllowedItemClassSingle(ItemClass.GetItemClass(_itemClassName, false));
	}

	// Token: 0x06008256 RID: 33366 RVA: 0x003179FB File Offset: 0x00315BFB
	public void SetAllowedItemClassSingle(ItemClass _itemClass)
	{
		this.IsDirty = true;
		this.allowedItemClasses.Clear();
		if (_itemClass != null)
		{
			this.allowedItemClasses.Add(_itemClass);
		}
	}

	// Token: 0x06008257 RID: 33367 RVA: 0x00317A1E File Offset: 0x00315C1E
	public void SetAllowedItemClasses(ItemClass[] _itemClasses)
	{
		this.IsDirty = true;
		this.allowedItemClasses.Clear();
		this.allowedItemClasses.AddRange(_itemClasses);
	}

	// Token: 0x06008258 RID: 33368 RVA: 0x00317A40 File Offset: 0x00315C40
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

	// Token: 0x0600825A RID: 33370 RVA: 0x00317AB4 File Offset: 0x00315CB4
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

	// Token: 0x0600825B RID: 33371 RVA: 0x00317B5C File Offset: 0x00315D5C
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

	// Token: 0x0600825C RID: 33372 RVA: 0x00317B96 File Offset: 0x00315D96
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

	// Token: 0x0400607D RID: 24701
	public XUiC_RequiredItemStack.RequiredTypes RequiredType;

	// Token: 0x0400607E RID: 24702
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ItemClass> allowedItemClasses = new List<ItemClass>();

	// Token: 0x0400607F RID: 24703
	public bool RequiredItemOnly = true;

	// Token: 0x04006080 RID: 24704
	public bool TakeOnly;

	// Token: 0x02001018 RID: 4120
	public enum RequiredTypes
	{
		// Token: 0x04006082 RID: 24706
		ItemClass,
		// Token: 0x04006083 RID: 24707
		IsPart,
		// Token: 0x04006084 RID: 24708
		HasQuality,
		// Token: 0x04006085 RID: 24709
		HasQualityNoParts
	}
}
