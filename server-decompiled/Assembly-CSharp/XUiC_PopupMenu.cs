using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000D59 RID: 3417
[Preserve]
public class XUiC_PopupMenu : XUiController
{
	// Token: 0x17000B12 RID: 2834
	// (get) Token: 0x06006793 RID: 26515 RVA: 0x00291C50 File Offset: 0x0028FE50
	// (set) Token: 0x06006794 RID: 26516 RVA: 0x00291C58 File Offset: 0x0028FE58
	[XuiXmlAttribute("menu_min_width", false)]
	public int MenuMinWidth
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = 100;

	// Token: 0x17000B13 RID: 2835
	// (get) Token: 0x06006795 RID: 26517 RVA: 0x00291C61 File Offset: 0x0028FE61
	// (set) Token: 0x06006796 RID: 26518 RVA: 0x00291C69 File Offset: 0x0028FE69
	[XuiXmlAttribute("element_padding_over_label", false)]
	public int MaxWidthPadding
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = 60;

	// Token: 0x17000B14 RID: 2836
	// (get) Token: 0x06006797 RID: 26519 RVA: 0x00291C72 File Offset: 0x0028FE72
	// (set) Token: 0x06006798 RID: 26520 RVA: 0x00291C7A File Offset: 0x0028FE7A
	[XuiXmlAttribute("slider_min_width", false)]
	public int SliderMinWidth { get; set; } = 250;

	// Token: 0x17000B15 RID: 2837
	// (get) Token: 0x06006799 RID: 26521 RVA: 0x00291C83 File Offset: 0x0028FE83
	[XuiXmlBinding("grid_width")]
	public int GridSizeX
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.gridSize.x;
		}
	}

	// Token: 0x17000B16 RID: 2838
	// (get) Token: 0x0600679A RID: 26522 RVA: 0x00291C90 File Offset: 0x0028FE90
	[XuiXmlBinding("grid_height")]
	public int GridSizeY
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.gridSize.y;
		}
	}

	// Token: 0x17000B17 RID: 2839
	// (get) Token: 0x0600679B RID: 26523 RVA: 0x00291C9D File Offset: 0x0028FE9D
	[XuiXmlBinding("title")]
	public string Title
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.title ?? "";
		}
	}

	// Token: 0x0600679C RID: 26524 RVA: 0x00291CAE File Offset: 0x0028FEAE
	public override void Init()
	{
		base.Init();
		this.xui.PopupMenuWindow = this;
	}

	// Token: 0x0600679D RID: 26525 RVA: 0x00291CC2 File Offset: 0x0028FEC2
	public override void Cleanup()
	{
		base.Cleanup();
		this.xui.PopupMenuWindow = null;
	}

	// Token: 0x0600679E RID: 26526 RVA: 0x00291CD6 File Offset: 0x0028FED6
	public XUiC_PopupMenu Setup(Vector2i _offsetPosition, string _title = "")
	{
		this.clearItems();
		this.IsOver = false;
		this.xuiPosition = this.xui.GetMouseXUiPosition();
		this.offset = _offsetPosition;
		this.title = _title;
		this.IsDirty = true;
		return this;
	}

	// Token: 0x0600679F RID: 26527 RVA: 0x00291D0C File Offset: 0x0028FF0C
	public XUiC_PopupMenu AddItem(XUiC_PopupMenuItem.Entry _newMenuItems)
	{
		this.menuItems.Add(_newMenuItems);
		this.IsDirty = true;
		return this;
	}

	// Token: 0x060067A0 RID: 26528 RVA: 0x00291D24 File Offset: 0x0028FF24
	[PublicizedFrom(EAccessModifier.Private)]
	public void clearItems()
	{
		this.menuItems.Clear();
		XUiC_PopupMenuItem[] array = this.popupMenuItems;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetEntry(null);
		}
		this.IsDirty = true;
	}

	// Token: 0x060067A1 RID: 26529 RVA: 0x00291D62 File Offset: 0x0028FF62
	public XUiC_PopupMenu Show()
	{
		this.xui.playerUI.windowManager.Open(this.windowGroup, false);
		this.justOpened = true;
		return this;
	}

	// Token: 0x060067A2 RID: 26530 RVA: 0x00291D88 File Offset: 0x0028FF88
	public void Close()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.clearItems();
	}

	// Token: 0x060067A3 RID: 26531 RVA: 0x00291DAC File Offset: 0x0028FFAC
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.menuItems.Count > 0 && !this.justOpened)
		{
			if (PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard)
			{
				if ((this.xui.playerUI.CursorController.GetMouseButtonUp(UICamera.MouseButton.LeftButton) | this.xui.playerUI.playerInput.GUIActions.RightStick.WasReleased) && !this.IsOver)
				{
					this.Close();
				}
			}
			else if (this.xui.playerUI.playerInput.GUIActions.Cancel.WasReleased)
			{
				this.Close();
			}
		}
		if (this.justOpened)
		{
			this.justOpened = false;
			if (this.menuItems.Count == 0)
			{
				this.Close();
				return;
			}
			int num = this.MenuMinWidth - this.MaxWidthPadding;
			int num2 = this.menuItems.Count - 1;
			for (int i = 0; i < this.popupMenuItems.Length; i++)
			{
				int num3 = this.popupMenuItems[i].SetEntry((i < this.menuItems.Count) ? this.menuItems[i] : null);
				if (num3 > num)
				{
					num = num3;
				}
				if (i < num2 && i < this.menuItems.Count && this.menuItems[i].IsEnabled)
				{
					num2 = i;
				}
			}
			num += this.MaxWidthPadding;
			this.gridSize = new Vector2i(num, this.menuItems.Count * this.gridView.CellHeight);
			this.limitPositionToScreenBounds();
			this.xui.playerUI.CursorController.SetNavigationTargetLater(this.popupMenuItems[num2].ViewComponent);
			this.IsDirty = true;
		}
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
	}

	// Token: 0x060067A4 RID: 26532 RVA: 0x00291F7C File Offset: 0x0029017C
	[PublicizedFrom(EAccessModifier.Private)]
	public void limitPositionToScreenBounds()
	{
		Vector2i vector2i = this.gridSize;
		vector2i.y = -vector2i.y;
		Vector2i vector2i2 = this.xui.GetXUiScreenSize() / 2;
		Vector2i vector2i3 = new Vector2i((int)((double)vector2i2.x * 0.97), (int)((double)vector2i2.y * 0.97));
		Vector2i vector2i4 = this.xuiPosition + this.offset;
		Vector2i vector2i5 = vector2i4;
		Vector2i vector2i6 = vector2i5 + vector2i;
		if (vector2i6.x >= vector2i3.x)
		{
			vector2i4.x = vector2i3.x - vector2i.x;
		}
		else if (vector2i5.x <= -vector2i3.x)
		{
			vector2i4.x = -vector2i3.x;
		}
		if (vector2i6.y <= -vector2i3.y)
		{
			vector2i4.y = -vector2i3.y - vector2i.y;
		}
		else if (vector2i5.y >= vector2i3.y)
		{
			vector2i4.y = vector2i3.y;
		}
		base.ViewComponent.Position = vector2i4;
		base.ViewComponent.TryUpdatePosition();
	}

	// Token: 0x04004FF3 RID: 20467
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_PopupMenuItem.Entry> menuItems = new List<XUiC_PopupMenuItem.Entry>();

	// Token: 0x04004FF4 RID: 20468
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i xuiPosition;

	// Token: 0x04004FF5 RID: 20469
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i offset;

	// Token: 0x04004FF6 RID: 20470
	public bool IsOver;

	// Token: 0x04004FF7 RID: 20471
	[XuiBindComponent("list", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Grid gridView;

	// Token: 0x04004FF8 RID: 20472
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_PopupMenuItem[] popupMenuItems;

	// Token: 0x04004FFC RID: 20476
	[PublicizedFrom(EAccessModifier.Private)]
	public string title;

	// Token: 0x04004FFD RID: 20477
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i gridSize = Vector2i.one;

	// Token: 0x04004FFE RID: 20478
	[PublicizedFrom(EAccessModifier.Private)]
	public bool justOpened;
}
