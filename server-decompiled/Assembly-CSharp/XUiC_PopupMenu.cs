using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000D64 RID: 3428
[Preserve]
public class XUiC_PopupMenu : XUiController
{
	// Token: 0x17000B18 RID: 2840
	// (get) Token: 0x060067B8 RID: 26552 RVA: 0x00293178 File Offset: 0x00291378
	// (set) Token: 0x060067B9 RID: 26553 RVA: 0x00293180 File Offset: 0x00291380
	[XuiXmlAttribute("menu_min_width", false)]
	public int MenuMinWidth
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = 100;

	// Token: 0x17000B19 RID: 2841
	// (get) Token: 0x060067BA RID: 26554 RVA: 0x00293189 File Offset: 0x00291389
	// (set) Token: 0x060067BB RID: 26555 RVA: 0x00293191 File Offset: 0x00291391
	[XuiXmlAttribute("element_padding_over_label", false)]
	public int MaxWidthPadding
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = 60;

	// Token: 0x17000B1A RID: 2842
	// (get) Token: 0x060067BC RID: 26556 RVA: 0x0029319A File Offset: 0x0029139A
	// (set) Token: 0x060067BD RID: 26557 RVA: 0x002931A2 File Offset: 0x002913A2
	[XuiXmlAttribute("slider_min_width", false)]
	public int SliderMinWidth { get; set; } = 250;

	// Token: 0x17000B1B RID: 2843
	// (get) Token: 0x060067BE RID: 26558 RVA: 0x002931AB File Offset: 0x002913AB
	[XuiXmlBinding("grid_width")]
	public int GridSizeX
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.gridSize.x;
		}
	}

	// Token: 0x17000B1C RID: 2844
	// (get) Token: 0x060067BF RID: 26559 RVA: 0x002931B8 File Offset: 0x002913B8
	[XuiXmlBinding("grid_height")]
	public int GridSizeY
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.gridSize.y;
		}
	}

	// Token: 0x17000B1D RID: 2845
	// (get) Token: 0x060067C0 RID: 26560 RVA: 0x002931C5 File Offset: 0x002913C5
	[XuiXmlBinding("title")]
	public string Title
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.title ?? "";
		}
	}

	// Token: 0x060067C1 RID: 26561 RVA: 0x002931D6 File Offset: 0x002913D6
	public override void Init()
	{
		base.Init();
		this.xui.PopupMenuWindow = this;
	}

	// Token: 0x060067C2 RID: 26562 RVA: 0x002931EA File Offset: 0x002913EA
	public override void Cleanup()
	{
		base.Cleanup();
		this.xui.PopupMenuWindow = null;
	}

	// Token: 0x060067C3 RID: 26563 RVA: 0x002931FE File Offset: 0x002913FE
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

	// Token: 0x060067C4 RID: 26564 RVA: 0x00293234 File Offset: 0x00291434
	public XUiC_PopupMenu AddItem(XUiC_PopupMenuItem.Entry _newMenuItems)
	{
		this.menuItems.Add(_newMenuItems);
		this.IsDirty = true;
		return this;
	}

	// Token: 0x060067C5 RID: 26565 RVA: 0x0029324C File Offset: 0x0029144C
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

	// Token: 0x060067C6 RID: 26566 RVA: 0x0029328A File Offset: 0x0029148A
	public XUiC_PopupMenu Show()
	{
		this.xui.playerUI.windowManager.Open(this.windowGroup, false);
		this.justOpened = true;
		return this;
	}

	// Token: 0x060067C7 RID: 26567 RVA: 0x002932B0 File Offset: 0x002914B0
	public void Close()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.clearItems();
	}

	// Token: 0x060067C8 RID: 26568 RVA: 0x002932D4 File Offset: 0x002914D4
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

	// Token: 0x060067C9 RID: 26569 RVA: 0x002934A4 File Offset: 0x002916A4
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

	// Token: 0x04005018 RID: 20504
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_PopupMenuItem.Entry> menuItems = new List<XUiC_PopupMenuItem.Entry>();

	// Token: 0x04005019 RID: 20505
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i xuiPosition;

	// Token: 0x0400501A RID: 20506
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i offset;

	// Token: 0x0400501B RID: 20507
	public bool IsOver;

	// Token: 0x0400501C RID: 20508
	[XuiBindComponent("list", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Grid gridView;

	// Token: 0x0400501D RID: 20509
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_PopupMenuItem[] popupMenuItems;

	// Token: 0x04005021 RID: 20513
	[PublicizedFrom(EAccessModifier.Private)]
	public string title;

	// Token: 0x04005022 RID: 20514
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i gridSize = Vector2i.one;

	// Token: 0x04005023 RID: 20515
	[PublicizedFrom(EAccessModifier.Private)]
	public bool justOpened;
}
