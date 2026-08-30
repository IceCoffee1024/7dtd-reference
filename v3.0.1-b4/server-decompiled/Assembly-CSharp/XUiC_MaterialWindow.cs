using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000F82 RID: 3970
[Preserve]
public class XUiC_MaterialWindow : XUiController
{
	// Token: 0x17000E83 RID: 3715
	// (get) Token: 0x06007D30 RID: 32048 RVA: 0x002F80AB File Offset: 0x002F62AB
	// (set) Token: 0x06007D31 RID: 32049 RVA: 0x002F80B3 File Offset: 0x002F62B3
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

	// Token: 0x06007D32 RID: 32050 RVA: 0x002F80EC File Offset: 0x002F62EC
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

	// Token: 0x06007D33 RID: 32051 RVA: 0x002F8236 File Offset: 0x002F6436
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangedHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.Page = 0;
		this.GetMaterialData(this.txtInput.Text);
		this.materialGrid.SetMaterials(this.currentItems, this.CurrentPaintId);
	}

	// Token: 0x06007D34 RID: 32052 RVA: 0x002F8267 File Offset: 0x002F6467
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

	// Token: 0x06007D35 RID: 32053 RVA: 0x002F8294 File Offset: 0x002F6494
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

	// Token: 0x06007D36 RID: 32054 RVA: 0x002F82CC File Offset: 0x002F64CC
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

	// Token: 0x17000E84 RID: 3716
	// (get) Token: 0x06007D37 RID: 32055 RVA: 0x002F83B0 File Offset: 0x002F65B0
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

	// Token: 0x06007D38 RID: 32056 RVA: 0x002F843C File Offset: 0x002F663C
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
		if (childByType != null)
		{
			childByType.GetSlotControl(holdingItemIdx).AssembleLock = true;
		}
		this.windowGroup.Controller.GetChildByType<XUiC_WindowNonPagingHeader>().SetHeader(Localization.Get("xuiMaterials", false, null).ToUpper());
	}

	// Token: 0x06007D39 RID: 32057 RVA: 0x002F850C File Offset: 0x002F670C
	public override void OnClose()
	{
		base.OnClose();
		this.xui.ToolTipWindow.ToolTip = "";
		int holdingItemIdx = this.xui.playerUI.entityPlayer.inventory.holdingItemIdx;
		XUiController childByType = ((XUiWindowGroup)this.xui.playerUI.windowManager.GetWindow("toolbelt")).Controller.GetChildByType<XUiC_Toolbelt>();
		this.xui.DragAndDropWindow.InMenu = false;
		if (childByType != null)
		{
			(childByType as XUiC_Toolbelt).GetSlotControl(holdingItemIdx).AssembleLock = false;
		}
		if (this.xui.playerUI.windowManager.IsWindowOpen("windowpaging"))
		{
			this.xui.playerUI.windowManager.Close("windowpaging");
		}
	}

	// Token: 0x06007D3A RID: 32058 RVA: 0x002F85D8 File Offset: 0x002F67D8
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

	// Token: 0x04005CB3 RID: 23731
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label resultCount;

	// Token: 0x04005CB4 RID: 23732
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_MaterialStackGrid materialGrid;

	// Token: 0x04005CB5 RID: 23733
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x04005CB6 RID: 23734
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04005CB7 RID: 23735
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x04005CB8 RID: 23736
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x04005CB9 RID: 23737
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblPaintEyeDropper;

	// Token: 0x04005CBA RID: 23738
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblCopyBlock;

	// Token: 0x04005CBB RID: 23739
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblTotal;

	// Token: 0x04005CBC RID: 23740
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<BlockTextureData> currentItems = new List<BlockTextureData>();
}
