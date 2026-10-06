using System;
using System.Collections.Generic;
using System.Text;
using GUI_2;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000F22 RID: 3874
[Preserve]
public class XUiC_Creative2Window : XUiController
{
	// Token: 0x17000E49 RID: 3657
	// (get) Token: 0x06007911 RID: 30993 RVA: 0x002DE04A File Offset: 0x002DC24A
	// (set) Token: 0x06007912 RID: 30994 RVA: 0x002DE052 File Offset: 0x002DC252
	public int Page
	{
		get
		{
			return this.page;
		}
		set
		{
			if (this.page != value)
			{
				this.page = value;
				this.creativeGrid.Page = this.page;
				XUiC_Paging xuiC_Paging = this.pager;
				if (xuiC_Paging == null)
				{
					return;
				}
				xuiC_Paging.SetPage(this.page);
			}
		}
	}

	// Token: 0x06007913 RID: 30995 RVA: 0x002DE08C File Offset: 0x002DC28C
	public override void Init()
	{
		base.Init();
		this.pager = base.GetChildByType<XUiC_Paging>();
		if (this.pager != null)
		{
			this.pager.OnPageChanged += delegate
			{
				this.Page = this.pager.CurrentPageNumber;
			};
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].OnScroll += this.HandleOnScroll;
		}
		base.OnScroll += this.HandleOnScroll;
		XUiController childById = base.GetChildById("simplepickup");
		if (childById != null)
		{
			this.simpleClickButton = childById.ViewComponent as XUiV_Button;
			if (this.simpleClickButton != null)
			{
				childById.OnPress += this.SimpleClickButton_OnPress;
			}
		}
		this.allowDevBlocks = !(DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent() || !Submission.Enabled;
		if (this.allowDevBlocks)
		{
			XUiController childById2 = base.GetChildById("devblocks");
			if (childById2 != null)
			{
				this.devBlockButton = childById2.ViewComponent as XUiV_Button;
				if (this.devBlockButton != null)
				{
					childById2.OnPress += this.DevBlockButton_OnPress;
				}
			}
		}
		XUiController childById3 = base.GetChildById("favorites");
		if (childById3 != null)
		{
			this.favorites = childById3.ViewComponent as XUiV_Button;
			if (this.favorites != null)
			{
				childById3.OnPress += this.HandleFavoritesChanged;
			}
		}
		XUiController childById4 = base.GetChildById("hideshapes");
		if (childById4 != null)
		{
			this.hideshapes = childById4.ViewComponent as XUiV_Button;
			if (this.hideshapes != null)
			{
				childById4.OnPress += this.Hideshapes_Changed;
			}
		}
		this.creativeGrid = base.Parent.GetChildByType<XUiC_Creative2StackGrid>();
		XUiC_ItemStack[] childrenByType = this.creativeGrid.GetChildrenByType<XUiC_ItemStack>(null);
		for (int j = 0; j < childrenByType.Length; j++)
		{
			childrenByType[j].OnScroll += this.HandleOnScroll;
		}
		this.txtInput = this.windowGroup.Controller.GetChildById("searchInput") as XUiC_TextInput;
		if (this.txtInput != null)
		{
			this.txtInput.OnChangeHandler += this.TxtInput_OnChange;
			this.txtInput.Text = "";
		}
		this.setupMainCategories((XUiC_CategoryList)base.GetChildById("categories"));
		List<XUiC_CategoryList> list = new List<XUiC_CategoryList>();
		base.GetChildrenByType<XUiC_CategoryList>(list);
		foreach (XUiC_CategoryList xuiC_CategoryList in list)
		{
			if (xuiC_CategoryList.ViewComponent.ID.StartsWith("sub", StringComparison.OrdinalIgnoreCase))
			{
				xuiC_CategoryList.CategoryChanged += this.SubCategory_CategoryChanged;
				xuiC_CategoryList.OnVisiblity += delegate(XUiController _sender, bool _visibleSelf, bool _visibleInScene)
				{
					if (this.isOpening)
					{
						return;
					}
					if (!_visibleInScene)
					{
						XUiC_CategoryEntry xuiC_CategoryEntry = this.currentSubCategory;
						if (((xuiC_CategoryEntry != null) ? xuiC_CategoryEntry.CategoryList : null) != _sender)
						{
							return;
						}
					}
					XUiC_CategoryList xuiC_CategoryList2 = (XUiC_CategoryList)_sender;
					if (xuiC_CategoryList2 == null)
					{
						return;
					}
					xuiC_CategoryList2.HandleCategoryChanged();
				};
			}
		}
		this.subCategoryListShapes = (XUiC_CategoryList)base.GetChildById("subCategoriesShapes");
		this.currentCreativeMode = (GameManager.Instance.IsEditMode() ? EnumCreativeMode.Dev : EnumCreativeMode.Player);
	}

	// Token: 0x06007914 RID: 30996 RVA: 0x002DE384 File Offset: 0x002DC584
	[PublicizedFrom(EAccessModifier.Private)]
	public void Hideshapes_Changed(XUiController _sender, int _mouseButton)
	{
		this.hideShapes = !this.hideShapes;
		this.hideshapes.Selected = this.hideShapes;
		this.RefreshList = true;
	}

	// Token: 0x06007915 RID: 30997 RVA: 0x002DE3AD File Offset: 0x002DC5AD
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleFavoritesChanged(XUiController _sender, int _mouseButton)
	{
		this.showFavorites = !this.showFavorites;
		this.favorites.Selected = this.showFavorites;
		this.RefreshList = true;
	}

	// Token: 0x06007916 RID: 30998 RVA: 0x002DE3D8 File Offset: 0x002DC5D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void CategoryList_CategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.currentFilterExcludeFuncs[0] = null;
		this.currentFilterExcludeFuncs[1] = null;
		this.currentMainCategory = _categoryEntry;
		this.RefreshList = true;
		this.IsDirty = true;
		if (_categoryEntry == null)
		{
			return;
		}
		this.currentFilterExcludeFuncs[0] = this.buildFilter(_categoryEntry);
		Block block = this.categoryIsBlockShapes(_categoryEntry);
		if (block != null)
		{
			this.setupSubcategoriesForShapes(block);
			return;
		}
		if (this.categoryIsOtherShapes(_categoryEntry))
		{
			this.setupSubcategoriesForShapes(this.otherShapeBlocks[0]);
		}
	}

	// Token: 0x06007917 RID: 30999 RVA: 0x002DE44D File Offset: 0x002DC64D
	[PublicizedFrom(EAccessModifier.Private)]
	public void SubCategory_CategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.currentFilterExcludeFuncs[1] = null;
		this.currentSubCategory = _categoryEntry;
		this.IsDirty = true;
		this.RefreshList = true;
		if (this.currentMainCategory == null)
		{
			return;
		}
		this.currentFilterExcludeFuncs[1] = this.buildFilter(this.currentSubCategory);
	}

	// Token: 0x06007918 RID: 31000 RVA: 0x002DE48A File Offset: 0x002DC68A
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtInput_OnChange(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.RefreshList = true;
	}

	// Token: 0x06007919 RID: 31001 RVA: 0x002DE493 File Offset: 0x002DC693
	[PublicizedFrom(EAccessModifier.Private)]
	public void DevBlockButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.showDevBlocks = !this.showDevBlocks;
		this.devBlockButton.Selected = this.showDevBlocks;
		this.RefreshList = true;
	}

	// Token: 0x0600791A RID: 31002 RVA: 0x002DE4BC File Offset: 0x002DC6BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void SimpleClickButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.simpleClick = !this.simpleClick;
		this.simpleClickButton.Selected = this.simpleClick;
		XUiC_ItemStack[] childrenByType = this.creativeGrid.GetChildrenByType<XUiC_ItemStack>(null);
		for (int i = 0; i < childrenByType.Length; i++)
		{
			childrenByType[i].SimpleClick = this.simpleClick;
		}
	}

	// Token: 0x0600791B RID: 31003 RVA: 0x002DE512 File Offset: 0x002DC712
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnScroll(XUiController _sender, float _delta)
	{
		if (_delta > 0f)
		{
			XUiC_Paging xuiC_Paging = this.pager;
			if (xuiC_Paging == null)
			{
				return;
			}
			xuiC_Paging.PageDown();
			return;
		}
		else
		{
			XUiC_Paging xuiC_Paging2 = this.pager;
			if (xuiC_Paging2 == null)
			{
				return;
			}
			xuiC_Paging2.PageUp();
			return;
		}
	}

	// Token: 0x0600791C RID: 31004 RVA: 0x002DE540 File Offset: 0x002DC740
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateHeader()
	{
		if (this.currentMainCategory == null)
		{
			this.filterCrumbs = Localization.Get("lblAll", false, null);
			return;
		}
		this.headerNameBuilder.Clear();
		this.headerNameBuilder.Append(this.currentMainCategory.CategoryDisplayName);
		if (this.currentSubCategory == null)
		{
			this.filterCrumbs = this.headerNameBuilder.ToString();
			return;
		}
		this.headerNameBuilder.Append(" | ");
		this.headerNameBuilder.Append(this.currentSubCategory.CategoryDisplayName);
		this.filterCrumbs = this.headerNameBuilder.ToString();
	}

	// Token: 0x0600791D RID: 31005 RVA: 0x002DE5E0 File Offset: 0x002DC7E0
	public void Refresh()
	{
		this.updateHeader();
		XUiC_TextInput xuiC_TextInput = this.txtInput;
		string text = ((xuiC_TextInput != null) ? xuiC_TextInput.Text : null);
		this.currentFilterExcludeFuncs[2] = null;
		if (this.hideShapes && this.currentMainCategory == null)
		{
			this.currentFilterExcludeFuncs[2] = (ItemClass _class, Block _block) => _block != null && _block.GetAutoShapeType() > EAutoShapeType.None;
		}
		this.filteredItems.Clear();
		ItemClass.GetItemsAndBlocks(this.filteredItems, -1, -1, this.currentFilterExcludeFuncs, text, this.showDevBlocks, this.currentCreativeMode, this.showFavorites, true, this.xui);
		this.length = this.creativeGrid.Length;
		this.Page = 0;
		this.IsDirty = true;
		ItemClass.CreateItemStacks(this.filteredItems, this.itemStacks, text);
		this.creativeGrid.SetSlots(this.itemStacks.ToArray());
		XUiC_Paging xuiC_Paging = this.pager;
		if (xuiC_Paging == null)
		{
			return;
		}
		xuiC_Paging.SetLastPageByElementsAndPageLength(this.itemStacks.Count, this.length);
	}

	// Token: 0x0600791E RID: 31006 RVA: 0x002DE6E6 File Offset: 0x002DC8E6
	public void RefreshView()
	{
		this.IsDirty = true;
		this.creativeGrid.IsDirty = true;
	}

	// Token: 0x0600791F RID: 31007 RVA: 0x002DE6FC File Offset: 0x002DC8FC
	public override void OnOpen()
	{
		this.isOpening = true;
		base.OnOpen();
		this.isOpening = false;
		this.Refresh();
		if (!GameStats.GetBool(EnumGameStats.IsCreativeMenuEnabled) && !GamePrefs.GetBool(EnumGamePrefs.CreativeMenuEnabled))
		{
			this.xui.playerUI.windowManager.CloseAllOpenModalWindows(null, false);
		}
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuCategory);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.LeftTrigger, "igcoCategoryLeft", XUiC_GamepadCalloutWindow.CalloutType.MenuCategory);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightTrigger, "igcoCategoryRight", XUiC_GamepadCalloutWindow.CalloutType.MenuCategory);
		this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuCategory, 0f);
	}

	// Token: 0x06007920 RID: 31008 RVA: 0x002DE7A3 File Offset: 0x002DC9A3
	public override void OnClose()
	{
		base.OnClose();
		this.xui.ToolTipWindow.ToolTip = "";
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuCategory);
	}

	// Token: 0x06007921 RID: 31009 RVA: 0x002DE7D1 File Offset: 0x002DC9D1
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
		if (this.RefreshList)
		{
			this.RefreshList = false;
			this.Refresh();
		}
	}

	// Token: 0x06007922 RID: 31010 RVA: 0x002DE804 File Offset: 0x002DCA04
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "filter_crumb")
		{
			_value = this.filterCrumbs ?? "";
			return true;
		}
		if (_bindingName == "result_count")
		{
			_value = this.itemStacks.Count.ToString();
			return true;
		}
		if (_bindingName == "main_category_is_shapes")
		{
			_value = "autoshapes".EqualsCaseInsensitive(this.categoryFilterType(this.currentMainCategory)).ToString();
			return true;
		}
		if (_bindingName == "main_category_name")
		{
			XUiC_CategoryEntry xuiC_CategoryEntry = this.currentMainCategory;
			_value = ((xuiC_CategoryEntry != null) ? xuiC_CategoryEntry.CategoryName : null) ?? "";
			return true;
		}
		if (!(_bindingName == "allow_dev"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		_value = this.allowDevBlocks.ToString();
		return true;
	}

	// Token: 0x06007923 RID: 31011 RVA: 0x002DE8D8 File Offset: 0x002DCAD8
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass.FilterItem buildFilter(XUiC_CategoryEntry _categoryEntry)
	{
		string text = this.categoryFilterType(_categoryEntry);
		if (text == null)
		{
			if (_categoryEntry != null)
			{
				Log.Warning("[XUi] CreativeMenu: No filtertype on sub category button '" + _categoryEntry.CategoryName + "'");
			}
			return null;
		}
		string filtertext = this.categoryFilterText(_categoryEntry);
		if (text.EqualsCaseInsensitive("autoshapes"))
		{
			if (filtertext == null)
			{
				return (ItemClass _class, Block _block) => _block == null || _block.GetAutoShapeType() == EAutoShapeType.None;
			}
			if (filtertext.EqualsCaseInsensitive("$other"))
			{
				return delegate(ItemClass _class, Block _block)
				{
					if (_block == null || _block.GetAutoShapeType() == EAutoShapeType.None)
					{
						return true;
					}
					for (int i = 0; i < this.otherShapeBlocks.Count; i++)
					{
						Block block = this.otherShapeBlocks[i];
						if (_block == block || _block.GetAutoShapeHelperBlock() == block)
						{
							return false;
						}
					}
					return true;
				};
			}
			string text2 = filtertext + ":" + ShapesFromXml.VariantHelperName;
			Block shapesHelper = Block.GetBlockByName(text2, false);
			if (shapesHelper == null)
			{
				Log.Warning("[XUi] CreativeMenu: Filtering on autoshapes with baseblock '" + text2 + "' failed: Block not found!");
				return null;
			}
			return (ItemClass _class, Block _block) => _block == null || _block.GetAutoShapeType() == EAutoShapeType.None || (_block != shapesHelper && _block.GetAutoShapeHelperBlock() != shapesHelper);
		}
		else if (text.EqualsCaseInsensitive("shapecategory"))
		{
			if (filtertext == null)
			{
				return null;
			}
			ShapesFromXml.ShapeCategory shapeCategory;
			if (!ShapesFromXml.shapeCategories.TryGetValue(filtertext, out shapeCategory))
			{
				Log.Warning("[XUi] CreativeMenu: Filtering on autoshapes with category '" + filtertext + "' failed: Unknown shape category!");
				return null;
			}
			return (ItemClass _class, Block _block) => ((_block != null) ? _block.ShapeCategories : null) == null || !_block.ShapeCategories.Contains(shapeCategory);
		}
		else if (text.EqualsCaseInsensitive("itemgroups"))
		{
			if (string.IsNullOrEmpty(filtertext))
			{
				return (ItemClass _class, Block _block) => _block != null;
			}
			return delegate(ItemClass _class, Block _block)
			{
				if (_block != null)
				{
					return true;
				}
				for (int j = 0; j < _class.Groups.Length; j++)
				{
					if (_class.Groups[j] == filtertext)
					{
						return false;
					}
				}
				return true;
			};
		}
		else
		{
			if (!text.EqualsCaseInsensitive("blockfiltertags"))
			{
				Log.Warning(string.Concat(new string[] { "[XUi] CreativeMenu: Unknown filtertype '", text, "' on sub category button '", _categoryEntry.CategoryName, "'" }));
				return null;
			}
			if (string.IsNullOrEmpty(filtertext))
			{
				return (ItemClass _class, Block _block) => _block == null;
			}
			return delegate(ItemClass _class, Block _block)
			{
				if (((_block != null) ? _block.FilterTags : null) != null)
				{
					for (int k = 0; k < _block.FilterTags.Length; k++)
					{
						if (_block.FilterTags[k] == filtertext)
						{
							return false;
						}
					}
				}
				return true;
			};
		}
	}

	// Token: 0x06007924 RID: 31012 RVA: 0x002DEAFC File Offset: 0x002DCCFC
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupMainCategories(XUiC_CategoryList _categoryList)
	{
		if (_categoryList == null)
		{
			return;
		}
		List<Block> individualButtonShapes = new List<Block>();
		for (int i = 0; i < _categoryList.MaxCategories; i++)
		{
			XUiC_CategoryEntry categoryByIndex = _categoryList.GetCategoryByIndex(i);
			Block block = this.categoryIsBlockShapes(categoryByIndex);
			if (block != null)
			{
				categoryByIndex.CategoryName = block.GetBlockName();
				categoryByIndex.CategoryDisplayName = block.blockMaterial.GetLocalizedMaterialName();
				individualButtonShapes.Add(block);
			}
		}
		List<ItemClass> list = new List<ItemClass>();
		ItemClass.GetItemsAndBlocks(list, -1, Block.ItemsStartHere, new ItemClass.FilterItem[]
		{
			(ItemClass _class, Block _block) => _block == null || _block.GetAutoShapeType() != EAutoShapeType.Helper || individualButtonShapes.Contains(_block)
		}, null, true, EnumCreativeMode.Player, false, false, null);
		foreach (ItemClass itemClass in list)
		{
			this.otherShapeBlocks.Add(itemClass.GetBlock());
		}
		_categoryList.CategoryChanged += this.CategoryList_CategoryChanged;
	}

	// Token: 0x06007925 RID: 31013 RVA: 0x002DEC00 File Offset: 0x002DCE00
	[PublicizedFrom(EAccessModifier.Private)]
	public Block categoryIsBlockShapes(XUiC_CategoryEntry _categoryEntry)
	{
		if (!"autoshapes".EqualsCaseInsensitive(this.categoryFilterType(_categoryEntry)))
		{
			return null;
		}
		string text = this.categoryFilterText(_categoryEntry);
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		return Block.GetBlockByName(text + ":" + ShapesFromXml.VariantHelperName, false);
	}

	// Token: 0x06007926 RID: 31014 RVA: 0x002DEC4A File Offset: 0x002DCE4A
	[PublicizedFrom(EAccessModifier.Private)]
	public bool categoryIsOtherShapes(XUiC_CategoryEntry _categoryEntry)
	{
		return "autoshapes".EqualsCaseInsensitive(this.categoryFilterType(_categoryEntry)) && "$other".EqualsCaseInsensitive(this.categoryFilterText(_categoryEntry));
	}

	// Token: 0x06007927 RID: 31015 RVA: 0x002DEC74 File Offset: 0x002DCE74
	[PublicizedFrom(EAccessModifier.Private)]
	public string categoryFilterType(XUiC_CategoryEntry _categoryEntry)
	{
		if (_categoryEntry == null)
		{
			return null;
		}
		object obj;
		_categoryEntry.CustomAttributes.TryGetValue("filtertype", out obj);
		return obj.ToString();
	}

	// Token: 0x06007928 RID: 31016 RVA: 0x002DECA0 File Offset: 0x002DCEA0
	[PublicizedFrom(EAccessModifier.Private)]
	public string categoryFilterText(XUiC_CategoryEntry _categoryEntry)
	{
		if (_categoryEntry == null)
		{
			return null;
		}
		object obj;
		_categoryEntry.CustomAttributes.TryGetValue("filtertext", out obj);
		return obj.ToString();
	}

	// Token: 0x06007929 RID: 31017 RVA: 0x002DECCC File Offset: 0x002DCECC
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupSubcategoriesForShapes(Block _shapesHelper)
	{
		if (this.subCategoryListShapes == null)
		{
			return;
		}
		this.subCategoryListShapes.CategoryChanged -= this.SubCategory_CategoryChanged;
		XUiC_CategoryEntry currentCategory = this.subCategoryListShapes.CurrentCategory;
		string text = ((currentCategory != null) ? currentCategory.CategoryName : null);
		IEnumerable<Block> altBlocks = _shapesHelper.GetAltBlocks();
		List<ShapesFromXml.ShapeCategory> list = new List<ShapesFromXml.ShapeCategory>();
		Block.GetShapeCategories(altBlocks, list);
		for (int i = 0; i < this.subCategoryListShapes.Children.Count; i++)
		{
			if (i < list.Count)
			{
				this.subCategoryListShapes.SetCategoryEntry(i, list[i].Name, list[i].Icon, list[i].LocalizedName);
				this.subCategoryListShapes.GetCategoryByIndex(i).CustomAttributes["filtertype"] = "shapecategory";
				this.subCategoryListShapes.GetCategoryByIndex(i).CustomAttributes["filtertext"] = list[i].Name;
			}
			else
			{
				this.subCategoryListShapes.SetCategoryEmpty(i);
			}
		}
		this.subCategoryListShapes.CategoryChanged += this.SubCategory_CategoryChanged;
		this.subCategoryListShapes.SetCategory(text);
	}

	// Token: 0x040059C2 RID: 22978
	[PublicizedFrom(EAccessModifier.Private)]
	public const string FilterTypeAutoshapes = "autoshapes";

	// Token: 0x040059C3 RID: 22979
	[PublicizedFrom(EAccessModifier.Private)]
	public const string FilterTextAutoshapesOther = "$other";

	// Token: 0x040059C4 RID: 22980
	[PublicizedFrom(EAccessModifier.Private)]
	public const string FilterTypeItemgroups = "itemgroups";

	// Token: 0x040059C5 RID: 22981
	[PublicizedFrom(EAccessModifier.Private)]
	public const string FilterTypeBlocktags = "blockfiltertags";

	// Token: 0x040059C6 RID: 22982
	[PublicizedFrom(EAccessModifier.Private)]
	public const string FilterTypeShapecategory = "shapecategory";

	// Token: 0x040059C7 RID: 22983
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button simpleClickButton;

	// Token: 0x040059C8 RID: 22984
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button devBlockButton;

	// Token: 0x040059C9 RID: 22985
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button favorites;

	// Token: 0x040059CA RID: 22986
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button hideshapes;

	// Token: 0x040059CB RID: 22987
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Creative2StackGrid creativeGrid;

	// Token: 0x040059CC RID: 22988
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x040059CD RID: 22989
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_CategoryList subCategoryListShapes;

	// Token: 0x040059CE RID: 22990
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x040059CF RID: 22991
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_CategoryEntry currentMainCategory;

	// Token: 0x040059D0 RID: 22992
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_CategoryEntry currentSubCategory;

	// Token: 0x040059D1 RID: 22993
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOpening;

	// Token: 0x040059D2 RID: 22994
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x040059D3 RID: 22995
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x040059D4 RID: 22996
	[PublicizedFrom(EAccessModifier.Private)]
	public bool simpleClick;

	// Token: 0x040059D5 RID: 22997
	[PublicizedFrom(EAccessModifier.Private)]
	public bool allowDevBlocks;

	// Token: 0x040059D6 RID: 22998
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showDevBlocks;

	// Token: 0x040059D7 RID: 22999
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showFavorites;

	// Token: 0x040059D8 RID: 23000
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hideShapes;

	// Token: 0x040059D9 RID: 23001
	[PublicizedFrom(EAccessModifier.Private)]
	public bool RefreshList;

	// Token: 0x040059DA RID: 23002
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly StringBuilder headerNameBuilder = new StringBuilder();

	// Token: 0x040059DB RID: 23003
	[PublicizedFrom(EAccessModifier.Private)]
	public string filterCrumbs;

	// Token: 0x040059DC RID: 23004
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ItemClass.FilterItem[] currentFilterExcludeFuncs = new ItemClass.FilterItem[3];

	// Token: 0x040059DD RID: 23005
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ItemClass> filteredItems = new List<ItemClass>();

	// Token: 0x040059DE RID: 23006
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ItemStack> itemStacks = new List<ItemStack>();

	// Token: 0x040059DF RID: 23007
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumCreativeMode currentCreativeMode = EnumCreativeMode.Player;

	// Token: 0x040059E0 RID: 23008
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<Block> otherShapeBlocks = new List<Block>();
}
