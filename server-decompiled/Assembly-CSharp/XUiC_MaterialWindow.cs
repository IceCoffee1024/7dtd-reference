using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000F9B RID: 3995
[Preserve]
public class XUiC_MaterialWindow : XUiController
{
	// Token: 0x17000EBC RID: 3772
	// (get) Token: 0x06007E22 RID: 32290 RVA: 0x002FC87F File Offset: 0x002FAA7F
	// (set) Token: 0x06007E23 RID: 32291 RVA: 0x002FC887 File Offset: 0x002FAA87
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

	// Token: 0x06007E24 RID: 32292 RVA: 0x002FC8C0 File Offset: 0x002FAAC0
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

	// Token: 0x06007E25 RID: 32293 RVA: 0x002FCA0A File Offset: 0x002FAC0A
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangedHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.Page = 0;
		this.GetMaterialData(this.txtInput.Text);
		this.materialGrid.SetMaterials(this.currentItems, this.CurrentPaintId);
	}

	// Token: 0x06007E26 RID: 32294 RVA: 0x002FCA3B File Offset: 0x002FAC3B
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

	// Token: 0x06007E27 RID: 32295 RVA: 0x002FCA68 File Offset: 0x002FAC68
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

	// Token: 0x06007E28 RID: 32296 RVA: 0x002FCAA0 File Offset: 0x002FACA0
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

	// Token: 0x17000EBD RID: 3773
	// (get) Token: 0x06007E29 RID: 32297 RVA: 0x002FCB84 File Offset: 0x002FAD84
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

	// Token: 0x06007E2A RID: 32298 RVA: 0x002FCC10 File Offset: 0x002FAE10
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

	// Token: 0x06007E2B RID: 32299 RVA: 0x002FCCE0 File Offset: 0x002FAEE0
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

	// Token: 0x06007E2C RID: 32300 RVA: 0x002FCDAC File Offset: 0x002FAFAC
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

	// Token: 0x04005D7A RID: 23930
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label resultCount;

	// Token: 0x04005D7B RID: 23931
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_MaterialStackGrid materialGrid;

	// Token: 0x04005D7C RID: 23932
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x04005D7D RID: 23933
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04005D7E RID: 23934
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x04005D7F RID: 23935
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x04005D80 RID: 23936
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblPaintEyeDropper;

	// Token: 0x04005D81 RID: 23937
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblCopyBlock;

	// Token: 0x04005D82 RID: 23938
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblTotal;

	// Token: 0x04005D83 RID: 23939
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<BlockTextureData> currentItems = new List<BlockTextureData>();
}
