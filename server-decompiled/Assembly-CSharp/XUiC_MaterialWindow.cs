using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FBA RID: 4026
[Preserve]
public class XUiC_MaterialWindow : XUiController
{
	// Token: 0x17000EEE RID: 3822
	// (get) Token: 0x06007E81 RID: 32385 RVA: 0x003013F2 File Offset: 0x002FF5F2
	// (set) Token: 0x06007E82 RID: 32386 RVA: 0x003013FA File Offset: 0x002FF5FA
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

	// Token: 0x06007E83 RID: 32387 RVA: 0x00301434 File Offset: 0x002FF634
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

	// Token: 0x06007E84 RID: 32388 RVA: 0x0030157E File Offset: 0x002FF77E
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangedHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.Page = 0;
		this.GetMaterialData(this.txtInput.Text);
		this.materialGrid.SetMaterials(this.currentItems, this.CurrentPaintId);
	}

	// Token: 0x06007E85 RID: 32389 RVA: 0x003015AF File Offset: 0x002FF7AF
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

	// Token: 0x06007E86 RID: 32390 RVA: 0x003015DC File Offset: 0x002FF7DC
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

	// Token: 0x06007E87 RID: 32391 RVA: 0x00301614 File Offset: 0x002FF814
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

	// Token: 0x17000EEF RID: 3823
	// (get) Token: 0x06007E88 RID: 32392 RVA: 0x003016F8 File Offset: 0x002FF8F8
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

	// Token: 0x06007E89 RID: 32393 RVA: 0x00301788 File Offset: 0x002FF988
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

	// Token: 0x06007E8A RID: 32394 RVA: 0x00301860 File Offset: 0x002FFA60
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

	// Token: 0x06007E8B RID: 32395 RVA: 0x00301930 File Offset: 0x002FFB30
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

	// Token: 0x04005DDE RID: 24030
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label resultCount;

	// Token: 0x04005DDF RID: 24031
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_MaterialStackGrid materialGrid;

	// Token: 0x04005DE0 RID: 24032
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x04005DE1 RID: 24033
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04005DE2 RID: 24034
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x04005DE3 RID: 24035
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x04005DE4 RID: 24036
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblPaintEyeDropper;

	// Token: 0x04005DE5 RID: 24037
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblCopyBlock;

	// Token: 0x04005DE6 RID: 24038
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblTotal;

	// Token: 0x04005DE7 RID: 24039
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<BlockTextureData> currentItems = new List<BlockTextureData>();
}
