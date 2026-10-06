using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FAA RID: 4010
[Preserve]
public class XUiC_MaterialWindow : XUiController
{
	// Token: 0x17000EE3 RID: 3811
	// (get) Token: 0x06007E32 RID: 32306 RVA: 0x002FF8D6 File Offset: 0x002FDAD6
	// (set) Token: 0x06007E33 RID: 32307 RVA: 0x002FF8DE File Offset: 0x002FDADE
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
				this.materialGrid.Page = this.page;
				XUiC_Paging xuiC_Paging = this.pager;
				if (xuiC_Paging == null)
				{
					return;
				}
				xuiC_Paging.SetPage(this.page);
			}
		}
	}

	// Token: 0x06007E34 RID: 32308 RVA: 0x002FF918 File Offset: 0x002FDB18
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
		this.materialGrid = base.Parent.GetChildByType<XUiC_MaterialStackGrid>();
		XUiController[] childrenByType = this.materialGrid.GetChildrenByType<XUiC_MaterialStack>(null);
		XUiController[] array = childrenByType;
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
		this.lblTotal = Localization.Get("lblTotalItems", false, null);
	}

	// Token: 0x06007E35 RID: 32309 RVA: 0x002FFA62 File Offset: 0x002FDC62
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangedHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.Page = 0;
		this.GetMaterialData(this.txtInput.Text);
		this.materialGrid.SetMaterials(this.currentItems, this.CurrentPaintId);
	}

	// Token: 0x06007E36 RID: 32310 RVA: 0x002FFA93 File Offset: 0x002FDC93
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

	// Token: 0x06007E37 RID: 32311 RVA: 0x002FFAC0 File Offset: 0x002FDCC0
	[PublicizedFrom(EAccessModifier.Private)]
	public void GetMaterialData(string _name)
	{
		if (_name == null)
		{
			_name = "";
		}
		this.currentItems.Clear();
		this.length = this.materialGrid.Length;
		this.Page = 0;
		this.FilterByName(_name);
	}

	// Token: 0x06007E38 RID: 32312 RVA: 0x002FFAF8 File Offset: 0x002FDCF8
	public void FilterByName(string _name)
	{
		bool flag = GameStats.GetBool(EnumGameStats.IsCreativeMenuEnabled) && GamePrefs.GetBool(EnumGamePrefs.CreativeMenuEnabled);
		this.currentItems.Clear();
		for (int i = 0; i < BlockTextureData.list.Length; i++)
		{
			BlockTextureData blockTextureData = BlockTextureData.list[i];
			if (blockTextureData != null && (!blockTextureData.Hidden || flag))
			{
				if (_name != "")
				{
					string name = blockTextureData.Name;
					if (_name == "" || name.ContainsCaseInsensitive(_name))
					{
						this.currentItems.Add(blockTextureData);
					}
				}
				else
				{
					this.currentItems.Add(blockTextureData);
				}
			}
		}
		XUiC_Paging xuiC_Paging = this.pager;
		if (xuiC_Paging != null)
		{
			xuiC_Paging.SetLastPageByElementsAndPageLength(this.currentItems.Count, this.length);
		}
		this.resultCount.Text = string.Format(this.lblTotal, this.currentItems.Count.ToString());
	}

	// Token: 0x17000EE4 RID: 3812
	// (get) Token: 0x06007E39 RID: 32313 RVA: 0x002FFBDC File Offset: 0x002FDDDC
	public int CurrentPaintId
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.xui.playerUI.entityPlayer.inventory.holdingItem is ItemClassBlock)
			{
				return (int)(this.xui.playerUI.entityPlayer.inventory.holdingItemItemValue.TextureFullArray[0] & 255L);
			}
			return ((ItemActionTextureBlock.ItemActionTextureBlockData)this.xui.playerUI.entityPlayer.inventory.holdingItemData.actionData[1]).idx;
		}
	}

	// Token: 0x06007E3A RID: 32314 RVA: 0x002FFC6C File Offset: 0x002FDE6C
	public override void OnOpen()
	{
		base.OnOpen();
		this.GetMaterialData(this.txtInput.Text);
		this.IsDirty = true;
		int currentPaintId = this.CurrentPaintId;
		this.materialGrid.SetMaterials(this.currentItems, currentPaintId);
		int holdingItemIdx = this.xui.playerUI.entityPlayer.inventory.holdingItemIdx;
		XUiC_Toolbelt childByType = ((XUiWindowGroup)this.xui.playerUI.windowManager.GetWindow("toolbelt")).Controller.GetChildByType<XUiC_Toolbelt>();
		this.xui.DragAndDropWindow.InMenu = true;
		XUiC_ItemStack xuiC_ItemStack;
		if (childByType != null && childByType.TryGetSlotControl(holdingItemIdx, out xuiC_ItemStack))
		{
			xuiC_ItemStack.AssembleLock = true;
		}
		this.windowGroup.Controller.GetChildByType<XUiC_WindowNonPagingHeader>().SetHeader(Localization.Get("xuiMaterials", false, null).ToUpper());
	}

	// Token: 0x06007E3B RID: 32315 RVA: 0x002FFD44 File Offset: 0x002FDF44
	public override void OnClose()
	{
		base.OnClose();
		this.xui.ToolTipWindow.ToolTip = "";
		int holdingItemIdx = this.xui.playerUI.entityPlayer.inventory.holdingItemIdx;
		this.xui.DragAndDropWindow.InMenu = false;
		XUiWindowGroup xuiWindowGroup = (XUiWindowGroup)this.xui.playerUI.windowManager.GetWindow("toolbelt");
		XUiC_Toolbelt xuiC_Toolbelt = ((xuiWindowGroup != null) ? xuiWindowGroup.Controller.GetChildByType<XUiC_Toolbelt>() : null);
		XUiC_ItemStack xuiC_ItemStack;
		if (xuiC_Toolbelt != null && xuiC_Toolbelt.TryGetSlotControl(holdingItemIdx, out xuiC_ItemStack))
		{
			xuiC_ItemStack.AssembleLock = false;
		}
		if (this.xui.playerUI.windowManager.IsWindowOpen("windowpaging"))
		{
			this.xui.playerUI.windowManager.Close("windowpaging");
		}
	}

	// Token: 0x06007E3C RID: 32316 RVA: 0x002FFE14 File Offset: 0x002FE014
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.viewComponent.IsVisible)
		{
			if (null != this.xui.playerUI && this.xui.playerUI.playerInput != null && this.xui.playerUI.playerInput.GUIActions != null)
			{
				PlayerActionsGUI guiactions = this.xui.playerUI.playerInput.GUIActions;
			}
			if (this.IsDirty)
			{
				base.RefreshBindings();
				this.materialGrid.IsDirty = true;
			}
		}
	}

	// Token: 0x04005DA3 RID: 23971
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label resultCount;

	// Token: 0x04005DA4 RID: 23972
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_MaterialStackGrid materialGrid;

	// Token: 0x04005DA5 RID: 23973
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x04005DA6 RID: 23974
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04005DA7 RID: 23975
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x04005DA8 RID: 23976
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x04005DA9 RID: 23977
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblPaintEyeDropper;

	// Token: 0x04005DAA RID: 23978
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblCopyBlock;

	// Token: 0x04005DAB RID: 23979
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblTotal;

	// Token: 0x04005DAC RID: 23980
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<BlockTextureData> currentItems = new List<BlockTextureData>();
}
