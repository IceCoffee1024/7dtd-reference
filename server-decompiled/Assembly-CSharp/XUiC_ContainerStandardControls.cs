using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

// Token: 0x02000F1D RID: 3869
[Preserve]
public class XUiC_ContainerStandardControls : XUiController
{
	// Token: 0x17000E3E RID: 3646
	// (get) Token: 0x060078A4 RID: 30884 RVA: 0x002DC863 File Offset: 0x002DAA63
	// (set) Token: 0x060078A5 RID: 30885 RVA: 0x002DC876 File Offset: 0x002DAA76
	public PackedBoolArray LockedSlots
	{
		get
		{
			Func<PackedBoolArray> getLockedSlotsFromStorage = this.GetLockedSlotsFromStorage;
			if (getLockedSlotsFromStorage == null)
			{
				return null;
			}
			return getLockedSlotsFromStorage();
		}
		set
		{
			Action<PackedBoolArray> setLockedSlotsToStorage = this.SetLockedSlotsToStorage;
			if (setLockedSlotsToStorage == null)
			{
				return;
			}
			setLockedSlotsToStorage(value);
		}
	}

	// Token: 0x17000E3F RID: 3647
	// (get) Token: 0x060078A6 RID: 30886 RVA: 0x002DC889 File Offset: 0x002DAA89
	// (set) Token: 0x060078A7 RID: 30887 RVA: 0x002DC894 File Offset: 0x002DAA94
	public bool UserLockMode
	{
		get
		{
			return this.userLockMode;
		}
		set
		{
			if (value == this.userLockMode)
			{
				return;
			}
			this.userLockMode = value;
			if (this.btnToggleLockMode != null)
			{
				this.btnToggleLockMode.IsSelected = value;
			}
			base.WindowGroup.isEscClosable = !this.userLockMode;
			this.xui.playerUI.windowManager.GetModalWindow().isEscClosable = !this.userLockMode;
			this.IsDirty = true;
			Action<bool> lockModeToggled = this.LockModeToggled;
			if (lockModeToggled == null)
			{
				return;
			}
			lockModeToggled(this.userLockMode);
		}
	}

	// Token: 0x17000E40 RID: 3648
	// (get) Token: 0x060078A8 RID: 30888 RVA: 0x002DC91A File Offset: 0x002DAB1A
	[XuiXmlBinding("filtering_active")]
	public bool FilteringActive
	{
		get
		{
			return this.FilterCategories || this.FilterTextSelected;
		}
	}

	// Token: 0x17000E41 RID: 3649
	// (get) Token: 0x060078A9 RID: 30889 RVA: 0x002DC92C File Offset: 0x002DAB2C
	// (set) Token: 0x060078AA RID: 30890 RVA: 0x002DC950 File Offset: 0x002DAB50
	[XuiXmlBinding("filter_categories")]
	public bool FilterCategories
	{
		get
		{
			if (this.filterCategories)
			{
				XUiC_ButtonSelectable xuiC_ButtonSelectable = this.btnFilterCategory;
				return xuiC_ButtonSelectable == null || xuiC_ButtonSelectable.ViewComponent.IsVisible;
			}
			return false;
		}
		set
		{
			if (this.filterCategories == value)
			{
				return;
			}
			if (value)
			{
				this.UserLockMode = false;
				this.FilterTextSelected = false;
			}
			this.filterCategories = value;
			if (this.btnFilterText != null)
			{
				this.btnFilterCategory.IsSelected = value;
			}
			this.triggerFilterChanged = true;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000E42 RID: 3650
	// (get) Token: 0x060078AB RID: 30891 RVA: 0x002DC9A1 File Offset: 0x002DABA1
	// (set) Token: 0x060078AC RID: 30892 RVA: 0x002DC9C4 File Offset: 0x002DABC4
	[XuiXmlBinding("filter_text_selected")]
	public bool FilterTextSelected
	{
		get
		{
			if (this.filterTextSelected)
			{
				XUiC_ButtonSelectable xuiC_ButtonSelectable = this.btnFilterText;
				return xuiC_ButtonSelectable == null || xuiC_ButtonSelectable.ViewComponent.IsVisible;
			}
			return false;
		}
		set
		{
			if (this.filterTextSelected == value)
			{
				return;
			}
			if (value)
			{
				this.UserLockMode = false;
				this.FilterCategories = false;
			}
			this.filterTextSelected = value;
			if (this.btnFilterText != null)
			{
				this.btnFilterText.IsSelected = value;
			}
			this.triggerFilterChanged = true;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000E43 RID: 3651
	// (get) Token: 0x060078AD RID: 30893 RVA: 0x002DCA15 File Offset: 0x002DAC15
	// (set) Token: 0x060078AE RID: 30894 RVA: 0x002DCA28 File Offset: 0x002DAC28
	[XuiXmlBinding("filter_text")]
	public string FilterText
	{
		get
		{
			XUiC_TextInput xuiC_TextInput = this.txtFilterText;
			if (xuiC_TextInput == null)
			{
				return null;
			}
			return xuiC_TextInput.Text;
		}
		set
		{
			if (string.Equals(this.txtFilterText.Text, value, StringComparison.Ordinal))
			{
				return;
			}
			if (this.txtFilterText != null)
			{
				this.txtFilterText.Text = value;
			}
			this.triggerFilterChanged = true;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000E44 RID: 3652
	// (get) Token: 0x060078AF RID: 30895 RVA: 0x002DCA61 File Offset: 0x002DAC61
	public string FilterCategory
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_CategoryList xuiC_CategoryList = this.categoryFiltersArea;
			if (xuiC_CategoryList == null)
			{
				return null;
			}
			XUiC_CategoryEntry currentCategory = xuiC_CategoryList.CurrentCategory;
			if (currentCategory == null)
			{
				return null;
			}
			return currentCategory.CategoryName;
		}
	}

	// Token: 0x060078B0 RID: 30896 RVA: 0x002DCA80 File Offset: 0x002DAC80
	public override void Init()
	{
		base.Init();
		XUiController xuiController = base.GetChildById("btnSort");
		if (xuiController != null)
		{
			xuiController.OnPress += delegate(XUiController _, int _)
			{
				this.Sort();
			};
		}
		xuiController = base.GetChildById("btnMoveAll");
		if (xuiController != null)
		{
			xuiController.OnPress += delegate(XUiController _, int _)
			{
				this.MoveAll();
			};
		}
		xuiController = base.GetChildById("btnMoveFillAndSmart");
		if (xuiController != null)
		{
			xuiController.OnPress += delegate(XUiController _, int _)
			{
				this.MoveFillAndSmart();
			};
		}
		xuiController = base.GetChildById("btnMoveFillStacks");
		if (xuiController != null)
		{
			xuiController.OnPress += delegate(XUiController _, int _)
			{
				this.MoveFillStacks();
			};
		}
		xuiController = base.GetChildById("btnMoveSmart");
		if (xuiController != null)
		{
			xuiController.OnPress += delegate(XUiController _, int _)
			{
				this.MoveSmart();
			};
		}
		XUiC_CategoryList xuiC_CategoryList = this.categoryFiltersArea;
		if (xuiC_CategoryList != null)
		{
			xuiC_CategoryList.SetupCategoriesDefinedForTraders();
		}
		XUiC_CategoryList xuiC_CategoryList2 = this.categoryFiltersArea;
		if (xuiC_CategoryList2 == null)
		{
			return;
		}
		xuiC_CategoryList2.SetCategoryToFirst();
	}

	// Token: 0x060078B1 RID: 30897 RVA: 0x002DCB5A File Offset: 0x002DAD5A
	public override void OnOpen()
	{
		base.OnOpen();
		Action<PackedBoolArray> applyLockedSlotStates = this.ApplyLockedSlotStates;
		if (applyLockedSlotStates != null)
		{
			applyLockedSlotStates(this.LockedSlots);
		}
		this.triggerFilterChanged = true;
	}

	// Token: 0x060078B2 RID: 30898 RVA: 0x002DCB80 File Offset: 0x002DAD80
	public override void OnClose()
	{
		base.OnClose();
		this.UserLockMode = false;
	}

	// Token: 0x060078B3 RID: 30899 RVA: 0x002DCB8F File Offset: 0x002DAD8F
	public void Sort()
	{
		Action<XUiC_ContainerStandardControls> updateLockedSlotStates = this.UpdateLockedSlotStates;
		if (updateLockedSlotStates != null)
		{
			updateLockedSlotStates(this);
		}
		Action<PackedBoolArray> sortPressed = this.SortPressed;
		if (sortPressed == null)
		{
			return;
		}
		sortPressed(this.LockedSlots);
	}

	// Token: 0x060078B4 RID: 30900 RVA: 0x002DCBBC File Offset: 0x002DADBC
	public void MoveAll()
	{
		Action<XUiC_ContainerStandardControls> updateLockedSlotStates = this.UpdateLockedSlotStates;
		if (updateLockedSlotStates != null)
		{
			updateLockedSlotStates(this);
		}
		XUiController xuiController;
		XUiC_ItemStackGrid xuiC_ItemStackGrid;
		IInventory inventory;
		if (this.MoveAllowed(out xuiController, out xuiC_ItemStackGrid, out inventory))
		{
			ValueTuple<bool, bool> valueTuple = XUiM_LootContainer.StashItems(xuiController, xuiC_ItemStackGrid, inventory, 0, this.LockedSlots, XUiM_LootContainer.EItemMoveKind.All, this.MoveStartBottomRight);
			bool item = valueTuple.Item1;
			bool item2 = valueTuple.Item2;
			Action<bool, bool> moveAllDone = this.MoveAllDone;
			if (moveAllDone == null)
			{
				return;
			}
			moveAllDone(item, item2);
		}
	}

	// Token: 0x060078B5 RID: 30901 RVA: 0x002DCC28 File Offset: 0x002DAE28
	public void MoveFillAndSmart()
	{
		Action<XUiC_ContainerStandardControls> updateLockedSlotStates = this.UpdateLockedSlotStates;
		if (updateLockedSlotStates != null)
		{
			updateLockedSlotStates(this);
		}
		XUiController xuiController;
		XUiC_ItemStackGrid xuiC_ItemStackGrid;
		IInventory inventory;
		if (this.MoveAllowed(out xuiController, out xuiC_ItemStackGrid, out inventory))
		{
			XUiM_LootContainer.StashItems(xuiController, xuiC_ItemStackGrid, inventory, 0, this.LockedSlots, XUiM_LootContainer.EItemMoveKind.FillOnlyFirstCreateSecond, this.MoveStartBottomRight);
		}
	}

	// Token: 0x060078B6 RID: 30902 RVA: 0x002DCC74 File Offset: 0x002DAE74
	public void MoveFillStacks()
	{
		Action<XUiC_ContainerStandardControls> updateLockedSlotStates = this.UpdateLockedSlotStates;
		if (updateLockedSlotStates != null)
		{
			updateLockedSlotStates(this);
		}
		XUiController xuiController;
		XUiC_ItemStackGrid xuiC_ItemStackGrid;
		IInventory inventory;
		if (this.MoveAllowed(out xuiController, out xuiC_ItemStackGrid, out inventory))
		{
			XUiM_LootContainer.StashItems(xuiController, xuiC_ItemStackGrid, inventory, 0, this.LockedSlots, XUiM_LootContainer.EItemMoveKind.FillOnly, this.MoveStartBottomRight);
		}
	}

	// Token: 0x060078B7 RID: 30903 RVA: 0x002DCCC0 File Offset: 0x002DAEC0
	public void MoveSmart()
	{
		Action<XUiC_ContainerStandardControls> updateLockedSlotStates = this.UpdateLockedSlotStates;
		if (updateLockedSlotStates != null)
		{
			updateLockedSlotStates(this);
		}
		XUiController xuiController;
		XUiC_ItemStackGrid xuiC_ItemStackGrid;
		IInventory inventory;
		if (this.MoveAllowed(out xuiController, out xuiC_ItemStackGrid, out inventory))
		{
			XUiM_LootContainer.StashItems(xuiController, xuiC_ItemStackGrid, inventory, 0, this.LockedSlots, XUiM_LootContainer.EItemMoveKind.FillAndCreate, this.MoveStartBottomRight);
		}
	}

	// Token: 0x060078B8 RID: 30904 RVA: 0x002DCD09 File Offset: 0x002DAF09
	[XuiBindEvent("OnButtonSelected", "btnToggleLockMode")]
	public void btnToggleLockMode_Selected(XUiC_ButtonSelectable _sender, bool _isSelected)
	{
		this.UserLockMode = _isSelected;
	}

	// Token: 0x060078B9 RID: 30905 RVA: 0x002DCD12 File Offset: 0x002DAF12
	[XuiBindEvent("OnButtonSelected", "btnFilterCategory")]
	public void btnFilterCategory_Selected(XUiC_ButtonSelectable _sender, bool _isSelected)
	{
		this.FilterCategories = _isSelected;
	}

	// Token: 0x060078BA RID: 30906 RVA: 0x002DCD1B File Offset: 0x002DAF1B
	[XuiBindEvent("OnButtonSelected", "btnFilterText")]
	public void btnFilterText_Selected(XUiC_ButtonSelectable _sender, bool _isSelected)
	{
		this.FilterTextSelected = _isSelected;
	}

	// Token: 0x060078BB RID: 30907 RVA: 0x002DCD24 File Offset: 0x002DAF24
	[XuiBindEvent("OnChangeHandler", "txtFilterText")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void searchInputChanged(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.triggerFilterChanged = true;
	}

	// Token: 0x060078BC RID: 30908 RVA: 0x002DCD24 File Offset: 0x002DAF24
	[XuiBindEvent("CategoryChanged", "categoryFiltersArea")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void filterCategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.triggerFilterChanged = true;
	}

	// Token: 0x060078BD RID: 30909 RVA: 0x002DCD30 File Offset: 0x002DAF30
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ContainerStandardControls.MatchesFiltersDelegate GetActiveFilters()
	{
		if (!this.FilteringActive)
		{
			return null;
		}
		XUiC_ContainerStandardControls.MatchesFiltersDelegate matchesFiltersDelegate = null;
		string filterText = this.FilterText;
		if (this.FilterTextSelected && !string.IsNullOrEmpty(filterText))
		{
			matchesFiltersDelegate = XUiC_ContainerStandardControls.<GetActiveFilters>g__CombineDelegates|56_2(matchesFiltersDelegate, delegate(ItemStack stack)
			{
				if (stack.IsEmpty())
				{
					return false;
				}
				ItemClass itemClass = stack.itemValue.ItemClass;
				return itemClass.GetItemName().Contains(filterText, StringComparison.CurrentCultureIgnoreCase) || itemClass.GetLocalizedItemName().Contains(filterText, StringComparison.CurrentCultureIgnoreCase);
			});
		}
		string itemGroupName = this.FilterCategory;
		if (this.FilterCategories && !string.IsNullOrEmpty(itemGroupName))
		{
			matchesFiltersDelegate = XUiC_ContainerStandardControls.<GetActiveFilters>g__CombineDelegates|56_2(matchesFiltersDelegate, delegate(ItemStack stack)
			{
				if (stack.IsEmpty())
				{
					return false;
				}
				ItemClass itemClass2 = stack.itemValue.ItemClass;
				string[] array = itemClass2.Groups;
				if (itemClass2.IsBlock())
				{
					array = Block.list[stack.itemValue.type].GroupNames;
				}
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null && array[i].EqualsCaseInsensitive(itemGroupName))
					{
						return true;
					}
				}
				return false;
			});
		}
		return matchesFiltersDelegate;
	}

	// Token: 0x060078BE RID: 30910 RVA: 0x002DCDB8 File Offset: 0x002DAFB8
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "move_start_bottom_left")
		{
			this.MoveStartBottomRight = StringParsers.ParseBool(_value, 0, -1, true);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x060078BF RID: 30911 RVA: 0x002DCDE0 File Offset: 0x002DAFE0
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.UserLockMode)
		{
			PlayerActionsLocal playerInput = this.xui.playerUI.playerInput;
			if (playerInput.GUIActions.Cancel.WasPressed || playerInput.PermanentActions.Cancel.WasPressed)
			{
				this.UserLockMode = false;
			}
		}
		base.handleDirtyUpdateDefault();
		if (this.triggerFilterChanged)
		{
			this.triggerFilterChanged = false;
			XUiC_ContainerStandardControls.SearchFiltersChangedDelegate searchFiltersChanged = this.SearchFiltersChanged;
			if (searchFiltersChanged == null)
			{
				return;
			}
			searchFiltersChanged(this.GetActiveFilters());
		}
	}

	// Token: 0x060078C6 RID: 30918 RVA: 0x002DCE8C File Offset: 0x002DB08C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static XUiC_ContainerStandardControls.MatchesFiltersDelegate <GetActiveFilters>g__CombineDelegates|56_2(XUiC_ContainerStandardControls.MatchesFiltersDelegate _old, XUiC_ContainerStandardControls.MatchesFiltersDelegate _new)
	{
		if (_old == null)
		{
			return _new;
		}
		return (ItemStack stack) => _old(stack) && _new(stack);
	}

	// Token: 0x0400597D RID: 22909
	public Func<PackedBoolArray> GetLockedSlotsFromStorage;

	// Token: 0x0400597E RID: 22910
	public Action<PackedBoolArray> SetLockedSlotsToStorage;

	// Token: 0x0400597F RID: 22911
	public Action<PackedBoolArray> ApplyLockedSlotStates;

	// Token: 0x04005980 RID: 22912
	public Action<XUiC_ContainerStandardControls> UpdateLockedSlotStates;

	// Token: 0x04005981 RID: 22913
	public Action<PackedBoolArray> SortPressed;

	// Token: 0x04005982 RID: 22914
	public Action<bool> LockModeToggled;

	// Token: 0x04005983 RID: 22915
	public XUiC_ContainerStandardControls.MoveAllowedDelegate MoveAllowed;

	// Token: 0x04005984 RID: 22916
	public Action<bool, bool> MoveAllDone;

	// Token: 0x04005985 RID: 22917
	public bool MoveStartBottomRight;

	// Token: 0x04005986 RID: 22918
	public XUiC_ContainerStandardControls.SearchFiltersChangedDelegate SearchFiltersChanged;

	// Token: 0x04005987 RID: 22919
	[XuiBindComponent("btnToggleLockMode", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnToggleLockMode;

	// Token: 0x04005988 RID: 22920
	[XuiBindComponent("btnFilterCategory", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnFilterCategory;

	// Token: 0x04005989 RID: 22921
	[XuiBindComponent("categoryFiltersArea", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_CategoryList categoryFiltersArea;

	// Token: 0x0400598A RID: 22922
	[XuiBindComponent("btnFilterText", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable btnFilterText;

	// Token: 0x0400598B RID: 22923
	[XuiBindComponent("txtFilterText", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtFilterText;

	// Token: 0x0400598C RID: 22924
	[PublicizedFrom(EAccessModifier.Private)]
	public bool userLockMode;

	// Token: 0x0400598D RID: 22925
	[PublicizedFrom(EAccessModifier.Private)]
	public bool filterCategories;

	// Token: 0x0400598E RID: 22926
	[PublicizedFrom(EAccessModifier.Private)]
	public bool filterTextSelected;

	// Token: 0x0400598F RID: 22927
	[PublicizedFrom(EAccessModifier.Private)]
	public bool filteringActive;

	// Token: 0x04005990 RID: 22928
	[PublicizedFrom(EAccessModifier.Private)]
	public bool triggerFilterChanged;

	// Token: 0x04005991 RID: 22929
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenColor lockModeButtonColorTweener;

	// Token: 0x02000F1E RID: 3870
	// (Invoke) Token: 0x060078C8 RID: 30920
	public delegate bool MatchesFiltersDelegate(ItemStack _stack);

	// Token: 0x02000F1F RID: 3871
	// (Invoke) Token: 0x060078CC RID: 30924
	public delegate void SearchFiltersChangedDelegate(XUiC_ContainerStandardControls.MatchesFiltersDelegate _newFiltersDelegate);

	// Token: 0x02000F20 RID: 3872
	// (Invoke) Token: 0x060078D0 RID: 30928
	public delegate bool MoveAllowedDelegate(out XUiController _parentWindow, out XUiC_ItemStackGrid _sourceGrid, out IInventory _destinationInventory);
}
