using System;
using System.Collections.Generic;
using GUI_2;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02001037 RID: 4151
[Preserve]
public class XUiC_ShapesWindow : XUiController
{
	// Token: 0x17000F68 RID: 3944
	// (get) Token: 0x060082D0 RID: 33488 RVA: 0x0031CA02 File Offset: 0x0031AC02
	// (set) Token: 0x060082D1 RID: 33489 RVA: 0x0031CA0A File Offset: 0x0031AC0A
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
				this.shapeGrid.Page = this.page;
				XUiC_Paging xuiC_Paging = this.pager;
				if (xuiC_Paging == null)
				{
					return;
				}
				xuiC_Paging.SetPage(this.page);
			}
		}
	}

	// Token: 0x060082D2 RID: 33490 RVA: 0x0031CA44 File Offset: 0x0031AC44
	public override void Init()
	{
		base.Init();
		this.resultCount = (XUiV_Label)base.GetChildById("resultCount").ViewComponent;
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
		this.shapeGrid = base.Parent.GetChildByType<XUiC_ShapeStackGrid>();
		XUiController[] childrenByType = this.shapeGrid.GetChildrenByType<XUiC_ShapeStack>(null);
		XUiController[] array = childrenByType;
		this.shapeGrid.Owner = this;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].OnScroll += this.HandleOnScroll;
		}
		this.length = array.Length;
		this.txtInput = (XUiC_TextInput)this.windowGroup.Controller.GetChildById("searchInput");
		if (this.txtInput != null)
		{
			this.txtInput.OnChangeHandler += this.HandleOnChangedHandler;
			this.txtInput.Text = "";
		}
		XUiController childById = base.GetChildById("favorites");
		if (childById != null)
		{
			this.favoritesBtn = childById.ViewComponent as XUiV_Button;
			if (this.favoritesBtn != null)
			{
				childById.OnPress += this.HandleFavoritesChanged;
			}
		}
		this.lblTotal = Localization.Get("lblTotalItems", false, null);
		this.categoryList = (XUiC_CategoryList)base.GetChildById("categories");
		if (this.categoryList != null)
		{
			this.categoryList.AllowUnselect = true;
			this.categoryList.CategoryChanged += this.CategoryList_CategoryChanged;
		}
	}

	// Token: 0x060082D3 RID: 33491 RVA: 0x0031CC1A File Offset: 0x0031AE1A
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleFavoritesChanged(XUiController _sender, int _mouseButton)
	{
		this.showFavorites = !this.showFavorites;
		this.favoritesBtn.Selected = this.showFavorites;
		this.UpdateAll();
	}

	// Token: 0x060082D4 RID: 33492 RVA: 0x0031CC42 File Offset: 0x0031AE42
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangedHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.Page = 0;
		this.UpdateShapesList();
	}

	// Token: 0x060082D5 RID: 33493 RVA: 0x0031CC51 File Offset: 0x0031AE51
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

	// Token: 0x060082D6 RID: 33494 RVA: 0x0031CC80 File Offset: 0x0031AE80
	public void UpgradeDowngradeShapes(BlockValue _targetBv)
	{
		string blockName = _targetBv.Block.GetBlockName();
		Block autoShapeHelperBlock = _targetBv.Block.GetAutoShapeHelperBlock();
		ItemValue itemValue = new BlockValue((uint)autoShapeHelperBlock.blockID).ToItemValue();
		itemValue.Meta = autoShapeHelperBlock.GetAlternateBlockIndex(blockName);
		this.ItemValue = itemValue;
		this.UpdateAll();
	}

	// Token: 0x060082D7 RID: 33495 RVA: 0x0031CCD8 File Offset: 0x0031AED8
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateAltList()
	{
		this.altBlocks = this.ItemValue.ToBlockValue(false).Block.GetAltBlocks();
	}

	// Token: 0x060082D8 RID: 33496 RVA: 0x0031CD04 File Offset: 0x0031AF04
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateCategories()
	{
		XUiC_CategoryEntry currentCategory = this.categoryList.CurrentCategory;
		string text = ((currentCategory != null) ? currentCategory.CategoryName : null);
		Block.GetShapeCategories(this.altBlocks, this.shapeCategories);
		for (int i = 0; i < this.categoryList.Children.Count; i++)
		{
			if (i < this.shapeCategories.Count)
			{
				this.categoryList.SetCategoryEntry(i, this.shapeCategories[i].Name, this.shapeCategories[i].Icon, this.shapeCategories[i].LocalizedName);
			}
			else
			{
				this.categoryList.SetCategoryEmpty(i);
			}
		}
		if (text != null)
		{
			this.categoryList.SetCategory(text);
			return;
		}
		if (!this.openedBefore)
		{
			this.categoryList.SetCategoryToFirst();
			this.openedBefore = true;
		}
	}

	// Token: 0x060082D9 RID: 33497 RVA: 0x0031CDDC File Offset: 0x0031AFDC
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateShapesList()
	{
		List<string> favoriteShapes = this.xui.playerUI.entityPlayer.favoriteShapes;
		this.currentItems.Clear();
		this.length = this.shapeGrid.Length;
		string text = this.txtInput.Text;
		XUiC_CategoryEntry currentCategory = this.categoryList.CurrentCategory;
		string text2 = ((currentCategory != null) ? currentCategory.CategoryName : null);
		ShapesFromXml.ShapeCategory shapeCategory = null;
		if (!string.IsNullOrEmpty(text2))
		{
			shapeCategory = ShapesFromXml.shapeCategories[text2];
		}
		for (int i = 0; i < this.altBlocks.Length; i++)
		{
			Block block = this.altBlocks[i];
			string blockName = block.GetBlockName();
			string localizedBlockName = block.GetLocalizedBlockName();
			if ((!this.showFavorites || favoriteShapes.Contains(XUiC_ShapeStack.GetFavoritesEntryName(block))) && (string.IsNullOrEmpty(text) || blockName.ContainsCaseInsensitive(text) || localizedBlockName.ContainsCaseInsensitive(text)) && (shapeCategory == null || block.ShapeCategories.Contains(shapeCategory)) && !block.Properties.GetString("ShapeMenu").EqualsCaseInsensitive("false"))
			{
				this.currentItems.Add(new XUiC_ShapeStackGrid.ShapeData
				{
					Block = block,
					Index = i
				});
			}
		}
		this.currentItems.Sort((XUiC_ShapeStackGrid.ShapeData _shapeA, XUiC_ShapeStackGrid.ShapeData _shapeB) => StringComparer.Ordinal.Compare(_shapeA.Block.SortOrder, _shapeB.Block.SortOrder));
		XUiC_Paging xuiC_Paging = this.pager;
		if (xuiC_Paging != null)
		{
			xuiC_Paging.SetLastPageByElementsAndPageLength(this.currentItems.Count, this.length);
		}
		this.shapeGrid.SetShapes(this.currentItems, this.ItemValue.Meta);
		int num = this.currentItems.FindIndex((XUiC_ShapeStackGrid.ShapeData _data) => _data.Index == this.ItemValue.Meta);
		if (num < 0)
		{
			this.Page = 0;
		}
		else
		{
			this.Page = num / this.length;
		}
		this.resultCount.Text = string.Format(this.lblTotal, this.currentItems.Count.ToString());
	}

	// Token: 0x060082DA RID: 33498 RVA: 0x0031CFD9 File Offset: 0x0031B1D9
	public void UpdateAll()
	{
		this.updateAltList();
		this.updateCategories();
		this.UpdateShapesList();
		this.IsDirty = true;
	}

	// Token: 0x060082DB RID: 33499 RVA: 0x0031CFF4 File Offset: 0x0031B1F4
	public void RefreshItemStack()
	{
		XUiC_ItemStack stackController = this.StackController;
	}

	// Token: 0x060082DC RID: 33500 RVA: 0x0031D000 File Offset: 0x0031B200
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuCategory);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.LeftTrigger, "igcoCategoryLeft", XUiC_GamepadCalloutWindow.CalloutType.MenuCategory);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightTrigger, "igcoCategoryRight", XUiC_GamepadCalloutWindow.CalloutType.MenuCategory);
		this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuCategory, 0f);
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonNorth, "igcoToggleFavorite", XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts);
		this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts, 0f);
		int holdingItemIdx = this.xui.playerUI.entityPlayer.inventory.holdingItemIdx;
		this.xui.DragAndDropWindow.InMenu = true;
		XUiC_Toolbelt childByType = ((XUiWindowGroup)this.xui.playerUI.windowManager.GetWindow("toolbelt")).Controller.GetChildByType<XUiC_Toolbelt>();
		XUiC_ItemStack xuiC_ItemStack;
		if (childByType != null && childByType.TryGetSlotControl(holdingItemIdx, out xuiC_ItemStack))
		{
			this.StackController = xuiC_ItemStack;
			this.StackController.AssembleLock = true;
		}
		this.windowGroup.Controller.GetChildByType<XUiC_WindowNonPagingHeader>().SetHeader(Localization.Get("xuiShapes", false, null).ToUpper());
		this.UpdateAll();
	}

	// Token: 0x060082DD RID: 33501 RVA: 0x0031D14C File Offset: 0x0031B34C
	public override void OnClose()
	{
		base.OnClose();
		this.xui.ToolTipWindow.ToolTip = "";
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuCategory);
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts);
		bool childByType = ((XUiWindowGroup)this.xui.playerUI.windowManager.GetWindow("toolbelt")).Controller.GetChildByType<XUiC_Toolbelt>() != null;
		this.xui.DragAndDropWindow.InMenu = false;
		if (childByType)
		{
			this.StackController.AssembleLock = false;
			this.StackController.ItemStack = new ItemStack(this.ItemValue, this.StackController.ItemStack.count);
			this.ItemValue = ItemValue.None;
			this.StackController.ForceRefreshItemStack();
		}
		if (this.xui.playerUI.windowManager.IsWindowOpen("windowpaging"))
		{
			this.xui.playerUI.windowManager.Close("windowpaging");
		}
	}

	// Token: 0x060082DE RID: 33502 RVA: 0x0031D250 File Offset: 0x0031B450
	[PublicizedFrom(EAccessModifier.Private)]
	public void CategoryList_CategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.UpdateShapesList();
	}

	// Token: 0x060082DF RID: 33503 RVA: 0x0031D258 File Offset: 0x0031B458
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.viewComponent.IsVisible && this.IsDirty)
		{
			base.RefreshBindings();
			this.shapeGrid.IsDirty = true;
		}
	}

	// Token: 0x060082E0 RID: 33504 RVA: 0x0031D288 File Offset: 0x0031B488
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "keyboardonly")
		{
			if (PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard)
			{
				_value = "true";
			}
			else
			{
				_value = "false";
			}
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x04006108 RID: 24840
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label resultCount;

	// Token: 0x04006109 RID: 24841
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ShapeStackGrid shapeGrid;

	// Token: 0x0400610A RID: 24842
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x0400610B RID: 24843
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x0400610C RID: 24844
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showFavorites;

	// Token: 0x0400610D RID: 24845
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button favoritesBtn;

	// Token: 0x0400610E RID: 24846
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x0400610F RID: 24847
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_CategoryList categoryList;

	// Token: 0x04006110 RID: 24848
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x04006111 RID: 24849
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController paintbrushButton;

	// Token: 0x04006112 RID: 24850
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController paintrollerButton;

	// Token: 0x04006113 RID: 24851
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblPaintEyeDropper;

	// Token: 0x04006114 RID: 24852
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblCopyBlock;

	// Token: 0x04006115 RID: 24853
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblTotal;

	// Token: 0x04006116 RID: 24854
	public ItemValue ItemValue = ItemValue.None;

	// Token: 0x04006117 RID: 24855
	[PublicizedFrom(EAccessModifier.Private)]
	public Block[] altBlocks;

	// Token: 0x04006118 RID: 24856
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ShapesFromXml.ShapeCategory> shapeCategories = new List<ShapesFromXml.ShapeCategory>();

	// Token: 0x04006119 RID: 24857
	public XUiC_ItemStack StackController;

	// Token: 0x0400611A RID: 24858
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_ShapeStackGrid.ShapeData> currentItems = new List<XUiC_ShapeStackGrid.ShapeData>();

	// Token: 0x0400611B RID: 24859
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openedBefore;
}
