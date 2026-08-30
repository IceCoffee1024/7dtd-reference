using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000D49 RID: 3401
[Preserve]
public class XUiC_PopupMenu : XUiController
{
	// Token: 0x17000AFA RID: 2810
	// (get) Token: 0x06006799 RID: 26521 RVA: 0x0028E6E8 File Offset: 0x0028C8E8
	// (set) Token: 0x0600679A RID: 26522 RVA: 0x0028E6F0 File Offset: 0x0028C8F0
	[XuiXmlAttribute("menu_min_width", false)]
	public int MenuMinWidth
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = 100;

	// Token: 0x17000AFB RID: 2811
	// (get) Token: 0x0600679B RID: 26523 RVA: 0x0028E6F9 File Offset: 0x0028C8F9
	// (set) Token: 0x0600679C RID: 26524 RVA: 0x0028E701 File Offset: 0x0028C901
	[XuiXmlAttribute("element_padding_over_label", false)]
	public int MaxWidthPadding
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = 60;

	// Token: 0x17000AFC RID: 2812
	// (get) Token: 0x0600679D RID: 26525 RVA: 0x0028E70A File Offset: 0x0028C90A
	// (set) Token: 0x0600679E RID: 26526 RVA: 0x0028E712 File Offset: 0x0028C912
	[XuiXmlAttribute("slider_min_width", false)]
	public int SliderMinWidth { get; set; } = 250;

	// Token: 0x17000AFD RID: 2813
	// (get) Token: 0x0600679F RID: 26527 RVA: 0x0028E71B File Offset: 0x0028C91B
	[XuiXmlBinding("grid_width")]
	public int GridSizeX
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.gridSize.x;
		}
	}

	// Token: 0x17000AFE RID: 2814
	// (get) Token: 0x060067A0 RID: 26528 RVA: 0x0028E728 File Offset: 0x0028C928
	[XuiXmlBinding("grid_height")]
	public int GridSizeY
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.gridSize.y;
		}
	}

	// Token: 0x17000AFF RID: 2815
	// (get) Token: 0x060067A1 RID: 26529 RVA: 0x0028E735 File Offset: 0x0028C935
	[XuiXmlBinding("title")]
	public string Title
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.title ?? "";
		}
	}

	// Token: 0x060067A2 RID: 26530 RVA: 0x0028E746 File Offset: 0x0028C946
	public override void Init()
	{
		base.Init();
		this.xui.PopupMenuWindow = this;
	}

	// Token: 0x060067A3 RID: 26531 RVA: 0x0028E75A File Offset: 0x0028C95A
	public override void Cleanup()
	{
		base.Cleanup();
		this.xui.PopupMenuWindow = null;
	}

	// Token: 0x060067A4 RID: 26532 RVA: 0x0028E76E File Offset: 0x0028C96E
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

	// Token: 0x060067A5 RID: 26533 RVA: 0x0028E7A4 File Offset: 0x0028C9A4
	public XUiC_PopupMenu AddItem(XUiC_PopupMenuItem.Entry _newMenuItems)
	{
		this.menuItems.Add(_newMenuItems);
		this.IsDirty = true;
		return this;
	}

	// Token: 0x060067A6 RID: 26534 RVA: 0x0028E7BC File Offset: 0x0028C9BC
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

	// Token: 0x060067A7 RID: 26535 RVA: 0x0028E7FA File Offset: 0x0028C9FA
	public XUiC_PopupMenu Show()
	{
		this.xui.playerUI.windowManager.Open(this.windowGroup, false);
		this.justOpened = true;
		return this;
	}

	// Token: 0x060067A8 RID: 26536 RVA: 0x0028E820 File Offset: 0x0028CA20
	public void Close()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.clearItems();
	}

	// Token: 0x060067A9 RID: 26537 RVA: 0x0028E844 File Offset: 0x0028CA44
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

	// Token: 0x060067AA RID: 26538 RVA: 0x0028EA14 File Offset: 0x0028CC14
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

	// Token: 0x04004FB4 RID: 20404
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_PopupMenuItem.Entry> menuItems = new List<XUiC_PopupMenuItem.Entry>();

	// Token: 0x04004FB5 RID: 20405
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i xuiPosition;

	// Token: 0x04004FB6 RID: 20406
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i offset;

	// Token: 0x04004FB7 RID: 20407
	public bool IsOver;

	// Token: 0x04004FB8 RID: 20408
	[XuiBindComponent("list", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Grid gridView;

	// Token: 0x04004FB9 RID: 20409
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_PopupMenuItem[] popupMenuItems;

	// Token: 0x04004FBD RID: 20413
	[PublicizedFrom(EAccessModifier.Private)]
	public string title;

	// Token: 0x04004FBE RID: 20414
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i gridSize = Vector2i.one;

	// Token: 0x04004FBF RID: 20415
	[PublicizedFrom(EAccessModifier.Private)]
	public bool justOpened;
}
