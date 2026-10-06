using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001026 RID: 4134
[Preserve]
public class XUiC_RequiredItemStack : XUiC_ItemStack
{
	// Token: 0x140000DA RID: 218
	// (add) Token: 0x0600825A RID: 33370 RVA: 0x0031A6B8 File Offset: 0x003188B8
	// (remove) Token: 0x0600825B RID: 33371 RVA: 0x0031A6F0 File Offset: 0x003188F0
	public event XUiEvent_RequiredSlotFailedSwapEventHandler FailedSwap;

	// Token: 0x17000F58 RID: 3928
	// (get) Token: 0x0600825C RID: 33372 RVA: 0x0031A725 File Offset: 0x00318925
	public bool HasAllowedItemClass
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.RequiredType == XUiC_RequiredItemStack.RequiredTypes.ItemClass && this.allowedItemClasses.Count > 0 && this.allowedItemClasses[0] != null;
		}
	}

	// Token: 0x17000F59 RID: 3929
	// (get) Token: 0x0600825D RID: 33373 RVA: 0x0031A74E File Offset: 0x0031894E
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

	// Token: 0x17000F5A RID: 3930
	// (get) Token: 0x0600825E RID: 33374 RVA: 0x0031A780 File Offset: 0x00318980
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

	// Token: 0x0600825F RID: 33375 RVA: 0x0031A7E0 File Offset: 0x003189E0
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

	// Token: 0x06008260 RID: 33376 RVA: 0x0031A88B File Offset: 0x00318A8B
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

	// Token: 0x06008261 RID: 33377 RVA: 0x0031A8BC File Offset: 0x00318ABC
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

	// Token: 0x06008262 RID: 33378 RVA: 0x0031A900 File Offset: 0x00318B00
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

	// Token: 0x06008263 RID: 33379 RVA: 0x0031AA1C File Offset: 0x00318C1C
	public void ClearAllowedItemClasses()
	{
		this.IsDirty = true;
		this.allowedItemClasses.Clear();
	}

	// Token: 0x06008264 RID: 33380 RVA: 0x0031AA30 File Offset: 0x00318C30
	public void SetAllowedItemClassSingle(string _itemClassName)
	{
		this.SetAllowedItemClassSingle(ItemClass.GetItemClass(_itemClassName, false));
	}

	// Token: 0x06008265 RID: 33381 RVA: 0x0031AA3F File Offset: 0x00318C3F
	public void SetAllowedItemClassSingle(ItemClass _itemClass)
	{
		this.IsDirty = true;
		this.allowedItemClasses.Clear();
		if (_itemClass != null)
		{
			this.allowedItemClasses.Add(_itemClass);
		}
	}

	// Token: 0x06008266 RID: 33382 RVA: 0x0031AA62 File Offset: 0x00318C62
	public void SetAllowedItemClasses(ItemClass[] _itemClasses)
	{
		this.IsDirty = true;
		this.allowedItemClasses.Clear();
		this.allowedItemClasses.AddRange(_itemClasses);
	}

	// Token: 0x06008267 RID: 33383 RVA: 0x0031AA84 File Offset: 0x00318C84
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

	// Token: 0x06008269 RID: 33385 RVA: 0x0031AAF8 File Offset: 0x00318CF8
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

	// Token: 0x0600826A RID: 33386 RVA: 0x0031ABA0 File Offset: 0x00318DA0
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

	// Token: 0x0600826B RID: 33387 RVA: 0x0031ABDA File Offset: 0x00318DDA
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

	// Token: 0x040060A6 RID: 24742
	public XUiC_RequiredItemStack.RequiredTypes RequiredType;

	// Token: 0x040060A7 RID: 24743
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ItemClass> allowedItemClasses = new List<ItemClass>();

	// Token: 0x040060A8 RID: 24744
	public bool RequiredItemOnly = true;

	// Token: 0x040060A9 RID: 24745
	public bool TakeOnly;

	// Token: 0x02001027 RID: 4135
	public enum RequiredTypes
	{
		// Token: 0x040060AB RID: 24747
		ItemClass,
		// Token: 0x040060AC RID: 24748
		IsPart,
		// Token: 0x040060AD RID: 24749
		HasQuality,
		// Token: 0x040060AE RID: 24750
		HasQualityNoParts
	}
}
