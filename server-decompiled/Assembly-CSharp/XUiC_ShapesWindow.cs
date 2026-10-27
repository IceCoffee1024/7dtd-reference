using System;
using System.Collections.Generic;
using GUI_2;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02001047 RID: 4167
[Preserve]
public class XUiC_ShapesWindow : XUiController
{
	// Token: 0x17000F73 RID: 3955
	// (get) Token: 0x06008322 RID: 33570 RVA: 0x0031E706 File Offset: 0x0031C906
	// (set) Token: 0x06008323 RID: 33571 RVA: 0x0031E70E File Offset: 0x0031C90E
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

	// Token: 0x06008324 RID: 33572 RVA: 0x0031E748 File Offset: 0x0031C948
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

	// Token: 0x06008325 RID: 33573 RVA: 0x0031E91E File Offset: 0x0031CB1E
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleFavoritesChanged(XUiController _sender, int _mouseButton)
	{
		this.showFavorites = !this.showFavorites;
		this.favoritesBtn.Selected = this.showFavorites;
		this.UpdateAll();
	}

	// Token: 0x06008326 RID: 33574 RVA: 0x0031E946 File Offset: 0x0031CB46
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangedHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.Page = 0;
		this.UpdateShapesList();
	}

	// Token: 0x06008327 RID: 33575 RVA: 0x0031E955 File Offset: 0x0031CB55
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

	// Token: 0x06008328 RID: 33576 RVA: 0x0031E984 File Offset: 0x0031CB84
	public void UpgradeDowngradeShapes(BlockValue _targetBv)
	{
		string blockName = _targetBv.Block.GetBlockName();
		Block autoShapeHelperBlock = _targetBv.Block.GetAutoShapeHelperBlock();
		ItemValue itemValue = new BlockValue((uint)autoShapeHelperBlock.blockID).ToItemValue();
		itemValue.Meta = autoShapeHelperBlock.GetAlternateBlockIndex(blockName);
		this.ItemValue = itemValue;
		this.UpdateAll();
	}

	// Token: 0x06008329 RID: 33577 RVA: 0x0031E9DC File Offset: 0x0031CBDC
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateAltList()
	{
		this.altBlocks = this.ItemValue.ToBlockValue(false).Block.GetAltBlocks();
	}

	// Token: 0x0600832A RID: 33578 RVA: 0x0031EA08 File Offset: 0x0031CC08
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

	// Token: 0x0600832B RID: 33579 RVA: 0x0031EAE0 File Offset: 0x0031CCE0
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

	// Token: 0x0600832C RID: 33580 RVA: 0x0031ECDD File Offset: 0x0031CEDD
	public void UpdateAll()
	{
		this.updateAltList();
		this.updateCategories();
		this.UpdateShapesList();
		this.IsDirty = true;
	}

	// Token: 0x0600832D RID: 33581 RVA: 0x0031ECF8 File Offset: 0x0031CEF8
	public void RefreshItemStack()
	{
		XUiC_ItemStack stackController = this.StackController;
	}

	// Token: 0x0600832E RID: 33582 RVA: 0x0031ED04 File Offset: 0x0031CF04
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

	// Token: 0x0600832F RID: 33583 RVA: 0x0031EE50 File Offset: 0x0031D050
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

	// Token: 0x06008330 RID: 33584 RVA: 0x0031EF54 File Offset: 0x0031D154
	[PublicizedFrom(EAccessModifier.Private)]
	public void CategoryList_CategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.UpdateShapesList();
	}

	// Token: 0x06008331 RID: 33585 RVA: 0x0031EF5C File Offset: 0x0031D15C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.viewComponent.IsVisible && this.IsDirty)
		{
			base.RefreshBindings();
			this.shapeGrid.IsDirty = true;
		}
	}

	// Token: 0x06008332 RID: 33586 RVA: 0x0031EF8C File Offset: 0x0031D18C
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

	// Token: 0x04006146 RID: 24902
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label resultCount;

	// Token: 0x04006147 RID: 24903
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ShapeStackGrid shapeGrid;

	// Token: 0x04006148 RID: 24904
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04006149 RID: 24905
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x0400614A RID: 24906
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showFavorites;

	// Token: 0x0400614B RID: 24907
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button favoritesBtn;

	// Token: 0x0400614C RID: 24908
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x0400614D RID: 24909
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_CategoryList categoryList;

	// Token: 0x0400614E RID: 24910
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x0400614F RID: 24911
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController paintbrushButton;

	// Token: 0x04006150 RID: 24912
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController paintrollerButton;

	// Token: 0x04006151 RID: 24913
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblPaintEyeDropper;

	// Token: 0x04006152 RID: 24914
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblCopyBlock;

	// Token: 0x04006153 RID: 24915
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblTotal;

	// Token: 0x04006154 RID: 24916
	public ItemValue ItemValue = ItemValue.None;

	// Token: 0x04006155 RID: 24917
	[PublicizedFrom(EAccessModifier.Private)]
	public Block[] altBlocks;

	// Token: 0x04006156 RID: 24918
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ShapesFromXml.ShapeCategory> shapeCategories = new List<ShapesFromXml.ShapeCategory>();

	// Token: 0x04006157 RID: 24919
	public XUiC_ItemStack StackController;

	// Token: 0x04006158 RID: 24920
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_ShapeStackGrid.ShapeData> currentItems = new List<XUiC_ShapeStackGrid.ShapeData>();

	// Token: 0x04006159 RID: 24921
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openedBefore;
}
