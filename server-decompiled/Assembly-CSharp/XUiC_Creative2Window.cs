using System;
using System.Collections.Generic;
using System.Text;
using GUI_2;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000F31 RID: 3889
[Preserve]
public class XUiC_Creative2Window : XUiController
{
	// Token: 0x17000E53 RID: 3667
	// (get) Token: 0x06007954 RID: 31060 RVA: 0x002DF93A File Offset: 0x002DDB3A
	// (set) Token: 0x06007955 RID: 31061 RVA: 0x002DF942 File Offset: 0x002DDB42
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

	// Token: 0x06007956 RID: 31062 RVA: 0x002DF97C File Offset: 0x002DDB7C
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

	// Token: 0x06007957 RID: 31063 RVA: 0x002DFC74 File Offset: 0x002DDE74
	[PublicizedFrom(EAccessModifier.Private)]
	public void Hideshapes_Changed(XUiController _sender, int _mouseButton)
	{
		this.hideShapes = !this.hideShapes;
		this.hideshapes.Selected = this.hideShapes;
		this.RefreshList = true;
	}

	// Token: 0x06007958 RID: 31064 RVA: 0x002DFC9D File Offset: 0x002DDE9D
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleFavoritesChanged(XUiController _sender, int _mouseButton)
	{
		this.showFavorites = !this.showFavorites;
		this.favorites.Selected = this.showFavorites;
		this.RefreshList = true;
	}

	// Token: 0x06007959 RID: 31065 RVA: 0x002DFCC8 File Offset: 0x002DDEC8
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

	// Token: 0x0600795A RID: 31066 RVA: 0x002DFD3D File Offset: 0x002DDF3D
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

	// Token: 0x0600795B RID: 31067 RVA: 0x002DFD7A File Offset: 0x002DDF7A
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtInput_OnChange(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.RefreshList = true;
	}

	// Token: 0x0600795C RID: 31068 RVA: 0x002DFD83 File Offset: 0x002DDF83
	[PublicizedFrom(EAccessModifier.Private)]
	public void DevBlockButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.showDevBlocks = !this.showDevBlocks;
		this.devBlockButton.Selected = this.showDevBlocks;
		this.RefreshList = true;
	}

	// Token: 0x0600795D RID: 31069 RVA: 0x002DFDAC File Offset: 0x002DDFAC
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

	// Token: 0x0600795E RID: 31070 RVA: 0x002DFE02 File Offset: 0x002DE002
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

	// Token: 0x0600795F RID: 31071 RVA: 0x002DFE30 File Offset: 0x002DE030
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

	// Token: 0x06007960 RID: 31072 RVA: 0x002DFED0 File Offset: 0x002DE0D0
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

	// Token: 0x06007961 RID: 31073 RVA: 0x002DFFD6 File Offset: 0x002DE1D6
	public void RefreshView()
	{
		this.IsDirty = true;
		this.creativeGrid.IsDirty = true;
	}

	// Token: 0x06007962 RID: 31074 RVA: 0x002DFFEC File Offset: 0x002DE1EC
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

	// Token: 0x06007963 RID: 31075 RVA: 0x002E0093 File Offset: 0x002DE293
	public override void OnClose()
	{
		base.OnClose();
		this.xui.ToolTipWindow.ToolTip = "";
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuCategory);
	}

	// Token: 0x06007964 RID: 31076 RVA: 0x002E00C1 File Offset: 0x002DE2C1
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

	// Token: 0x06007965 RID: 31077 RVA: 0x002E00F4 File Offset: 0x002DE2F4
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

	// Token: 0x06007966 RID: 31078 RVA: 0x002E01C8 File Offset: 0x002DE3C8
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

	// Token: 0x06007967 RID: 31079 RVA: 0x002E03EC File Offset: 0x002DE5EC
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

	// Token: 0x06007968 RID: 31080 RVA: 0x002E04F0 File Offset: 0x002DE6F0
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

	// Token: 0x06007969 RID: 31081 RVA: 0x002E053A File Offset: 0x002DE73A
	[PublicizedFrom(EAccessModifier.Private)]
	public bool categoryIsOtherShapes(XUiC_CategoryEntry _categoryEntry)
	{
		return "autoshapes".EqualsCaseInsensitive(this.categoryFilterType(_categoryEntry)) && "$other".EqualsCaseInsensitive(this.categoryFilterText(_categoryEntry));
	}

	// Token: 0x0600796A RID: 31082 RVA: 0x002E0564 File Offset: 0x002DE764
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

	// Token: 0x0600796B RID: 31083 RVA: 0x002E0590 File Offset: 0x002DE790
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

	// Token: 0x0600796C RID: 31084 RVA: 0x002E05BC File Offset: 0x002DE7BC
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

	// Token: 0x040059F5 RID: 23029
	[PublicizedFrom(EAccessModifier.Private)]
	public const string FilterTypeAutoshapes = "autoshapes";

	// Token: 0x040059F6 RID: 23030
	[PublicizedFrom(EAccessModifier.Private)]
	public const string FilterTextAutoshapesOther = "$other";

	// Token: 0x040059F7 RID: 23031
	[PublicizedFrom(EAccessModifier.Private)]
	public const string FilterTypeItemgroups = "itemgroups";

	// Token: 0x040059F8 RID: 23032
	[PublicizedFrom(EAccessModifier.Private)]
	public const string FilterTypeBlocktags = "blockfiltertags";

	// Token: 0x040059F9 RID: 23033
	[PublicizedFrom(EAccessModifier.Private)]
	public const string FilterTypeShapecategory = "shapecategory";

	// Token: 0x040059FA RID: 23034
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button simpleClickButton;

	// Token: 0x040059FB RID: 23035
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button devBlockButton;

	// Token: 0x040059FC RID: 23036
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button favorites;

	// Token: 0x040059FD RID: 23037
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button hideshapes;

	// Token: 0x040059FE RID: 23038
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Creative2StackGrid creativeGrid;

	// Token: 0x040059FF RID: 23039
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x04005A00 RID: 23040
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_CategoryList subCategoryListShapes;

	// Token: 0x04005A01 RID: 23041
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x04005A02 RID: 23042
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_CategoryEntry currentMainCategory;

	// Token: 0x04005A03 RID: 23043
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_CategoryEntry currentSubCategory;

	// Token: 0x04005A04 RID: 23044
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOpening;

	// Token: 0x04005A05 RID: 23045
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04005A06 RID: 23046
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x04005A07 RID: 23047
	[PublicizedFrom(EAccessModifier.Private)]
	public bool simpleClick;

	// Token: 0x04005A08 RID: 23048
	[PublicizedFrom(EAccessModifier.Private)]
	public bool allowDevBlocks;

	// Token: 0x04005A09 RID: 23049
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showDevBlocks;

	// Token: 0x04005A0A RID: 23050
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showFavorites;

	// Token: 0x04005A0B RID: 23051
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hideShapes;

	// Token: 0x04005A0C RID: 23052
	[PublicizedFrom(EAccessModifier.Private)]
	public bool RefreshList;

	// Token: 0x04005A0D RID: 23053
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly StringBuilder headerNameBuilder = new StringBuilder();

	// Token: 0x04005A0E RID: 23054
	[PublicizedFrom(EAccessModifier.Private)]
	public string filterCrumbs;

	// Token: 0x04005A0F RID: 23055
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ItemClass.FilterItem[] currentFilterExcludeFuncs = new ItemClass.FilterItem[3];

	// Token: 0x04005A10 RID: 23056
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ItemClass> filteredItems = new List<ItemClass>();

	// Token: 0x04005A11 RID: 23057
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ItemStack> itemStacks = new List<ItemStack>();

	// Token: 0x04005A12 RID: 23058
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumCreativeMode currentCreativeMode = EnumCreativeMode.Player;

	// Token: 0x04005A13 RID: 23059
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<Block> otherShapeBlocks = new List<Block>();
}
