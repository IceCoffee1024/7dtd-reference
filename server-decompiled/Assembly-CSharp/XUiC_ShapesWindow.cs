using System;
using System.Collections.Generic;
using GUI_2;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200100F RID: 4111
[Preserve]
public class XUiC_ShapesWindow : XUiController
{
	// Token: 0x17000F08 RID: 3848
	// (get) Token: 0x060081CF RID: 33231 RVA: 0x00315196 File Offset: 0x00313396
	// (set) Token: 0x060081D0 RID: 33232 RVA: 0x0031519E File Offset: 0x0031339E
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

	// Token: 0x060081D1 RID: 33233 RVA: 0x003151D8 File Offset: 0x003133D8
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

	// Token: 0x060081D2 RID: 33234 RVA: 0x003153AE File Offset: 0x003135AE
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleFavoritesChanged(XUiController _sender, int _mouseButton)
	{
		this.showFavorites = !this.showFavorites;
		this.favoritesBtn.Selected = this.showFavorites;
		this.UpdateAll();
	}

	// Token: 0x060081D3 RID: 33235 RVA: 0x003153D6 File Offset: 0x003135D6
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangedHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.Page = 0;
		this.UpdateShapesList();
	}

	// Token: 0x060081D4 RID: 33236 RVA: 0x003153E5 File Offset: 0x003135E5
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

	// Token: 0x060081D5 RID: 33237 RVA: 0x00315414 File Offset: 0x00313614
	public void UpgradeDowngradeShapes(BlockValue _targetBv)
	{
		string blockName = _targetBv.Block.GetBlockName();
		Block autoShapeHelperBlock = _targetBv.Block.GetAutoShapeHelperBlock();
		ItemValue itemValue = new BlockValue((uint)autoShapeHelperBlock.blockID).ToItemValue();
		itemValue.Meta = autoShapeHelperBlock.GetAlternateBlockIndex(blockName);
		this.ItemValue = itemValue;
		this.UpdateAll();
	}

	// Token: 0x060081D6 RID: 33238 RVA: 0x0031546C File Offset: 0x0031366C
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateAltList()
	{
		this.altBlocks = this.ItemValue.ToBlockValue(false).Block.GetAltBlocks();
	}

	// Token: 0x060081D7 RID: 33239 RVA: 0x00315498 File Offset: 0x00313698
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

	// Token: 0x060081D8 RID: 33240 RVA: 0x00315570 File Offset: 0x00313770
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

	// Token: 0x060081D9 RID: 33241 RVA: 0x0031576D File Offset: 0x0031396D
	public void UpdateAll()
	{
		this.updateAltList();
		this.updateCategories();
		this.UpdateShapesList();
		this.IsDirty = true;
	}

	// Token: 0x060081DA RID: 33242 RVA: 0x00315788 File Offset: 0x00313988
	public void RefreshItemStack()
	{
		XUiC_ItemStack stackController = this.StackController;
	}

	// Token: 0x060081DB RID: 33243 RVA: 0x00315794 File Offset: 0x00313994
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
		XUiC_Toolbelt childByType = ((XUiWindowGroup)this.xui.playerUI.windowManager.GetWindow("toolbelt")).Controller.GetChildByType<XUiC_Toolbelt>();
		this.xui.DragAndDropWindow.InMenu = true;
		if (childByType != null)
		{
			this.StackController = childByType.GetSlotControl(holdingItemIdx);
			this.StackController.AssembleLock = true;
		}
		this.windowGroup.Controller.GetChildByType<XUiC_WindowNonPagingHeader>().SetHeader(Localization.Get("xuiShapes", false, null).ToUpper());
		this.UpdateAll();
	}

	// Token: 0x060081DC RID: 33244 RVA: 0x003158DC File Offset: 0x00313ADC
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

	// Token: 0x060081DD RID: 33245 RVA: 0x003159E0 File Offset: 0x00313BE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void CategoryList_CategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.UpdateShapesList();
	}

	// Token: 0x060081DE RID: 33246 RVA: 0x003159E8 File Offset: 0x00313BE8
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.viewComponent.IsVisible && this.IsDirty)
		{
			base.RefreshBindings();
			this.shapeGrid.IsDirty = true;
		}
	}

	// Token: 0x060081DF RID: 33247 RVA: 0x00315A18 File Offset: 0x00313C18
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

	// Token: 0x04006018 RID: 24600
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label resultCount;

	// Token: 0x04006019 RID: 24601
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ShapeStackGrid shapeGrid;

	// Token: 0x0400601A RID: 24602
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x0400601B RID: 24603
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x0400601C RID: 24604
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showFavorites;

	// Token: 0x0400601D RID: 24605
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button favoritesBtn;

	// Token: 0x0400601E RID: 24606
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x0400601F RID: 24607
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_CategoryList categoryList;

	// Token: 0x04006020 RID: 24608
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x04006021 RID: 24609
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController paintbrushButton;

	// Token: 0x04006022 RID: 24610
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController paintrollerButton;

	// Token: 0x04006023 RID: 24611
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblPaintEyeDropper;

	// Token: 0x04006024 RID: 24612
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblCopyBlock;

	// Token: 0x04006025 RID: 24613
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblTotal;

	// Token: 0x04006026 RID: 24614
	public ItemValue ItemValue = ItemValue.None;

	// Token: 0x04006027 RID: 24615
	[PublicizedFrom(EAccessModifier.Private)]
	public Block[] altBlocks;

	// Token: 0x04006028 RID: 24616
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ShapesFromXml.ShapeCategory> shapeCategories = new List<ShapesFromXml.ShapeCategory>();

	// Token: 0x04006029 RID: 24617
	public XUiC_ItemStack StackController;

	// Token: 0x0400602A RID: 24618
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_ShapeStackGrid.ShapeData> currentItems = new List<XUiC_ShapeStackGrid.ShapeData>();

	// Token: 0x0400602B RID: 24619
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openedBefore;
}
