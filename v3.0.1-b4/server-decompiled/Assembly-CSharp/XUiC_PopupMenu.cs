using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000D37 RID: 3383
[Preserve]
public class XUiC_PopupMenu : XUiController
{
	// Token: 0x17000AE7 RID: 2791
	// (get) Token: 0x0600670D RID: 26381 RVA: 0x0028B804 File Offset: 0x00289A04
	// (set) Token: 0x0600670E RID: 26382 RVA: 0x0028B80C File Offset: 0x00289A0C
	[XuiXmlAttribute("menu_min_width", false)]
	public int MenuMinWidth
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = 100;

	// Token: 0x17000AE8 RID: 2792
	// (get) Token: 0x0600670F RID: 26383 RVA: 0x0028B815 File Offset: 0x00289A15
	// (set) Token: 0x06006710 RID: 26384 RVA: 0x0028B81D File Offset: 0x00289A1D
	[XuiXmlAttribute("element_padding_over_label", false)]
	public int MaxWidthPadding
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = 60;

	// Token: 0x17000AE9 RID: 2793
	// (get) Token: 0x06006711 RID: 26385 RVA: 0x0028B826 File Offset: 0x00289A26
	// (set) Token: 0x06006712 RID: 26386 RVA: 0x0028B82E File Offset: 0x00289A2E
	[XuiXmlAttribute("slider_min_width", false)]
	public int SliderMinWidth { get; set; } = 250;

	// Token: 0x17000AEA RID: 2794
	// (get) Token: 0x06006713 RID: 26387 RVA: 0x0028B837 File Offset: 0x00289A37
	[XuiXmlBinding("grid_width")]
	public int GridSizeX
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.gridSize.x;
		}
	}

	// Token: 0x17000AEB RID: 2795
	// (get) Token: 0x06006714 RID: 26388 RVA: 0x0028B844 File Offset: 0x00289A44
	[XuiXmlBinding("grid_height")]
	public int GridSizeY
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.gridSize.y;
		}
	}

	// Token: 0x17000AEC RID: 2796
	// (get) Token: 0x06006715 RID: 26389 RVA: 0x0028B851 File Offset: 0x00289A51
	[XuiXmlBinding("title")]
	public string Title
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.title ?? "";
		}
	}

	// Token: 0x06006716 RID: 26390 RVA: 0x0028B862 File Offset: 0x00289A62
	public override void Init()
	{
		base.Init();
		this.xui.PopupMenuWindow = this;
	}

	// Token: 0x06006717 RID: 26391 RVA: 0x0028B876 File Offset: 0x00289A76
	public override void Cleanup()
	{
		base.Cleanup();
		this.xui.PopupMenuWindow = null;
	}

	// Token: 0x06006718 RID: 26392 RVA: 0x0028B88A File Offset: 0x00289A8A
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

	// Token: 0x06006719 RID: 26393 RVA: 0x0028B8C0 File Offset: 0x00289AC0
	public XUiC_PopupMenu AddItem(XUiC_PopupMenuItem.Entry _newMenuItems)
	{
		this.menuItems.Add(_newMenuItems);
		this.IsDirty = true;
		return this;
	}

	// Token: 0x0600671A RID: 26394 RVA: 0x0028B8D8 File Offset: 0x00289AD8
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

	// Token: 0x0600671B RID: 26395 RVA: 0x0028B916 File Offset: 0x00289B16
	public XUiC_PopupMenu Show()
	{
		this.xui.playerUI.windowManager.Open(this.windowGroup, false);
		this.justOpened = true;
		return this;
	}

	// Token: 0x0600671C RID: 26396 RVA: 0x0028B93C File Offset: 0x00289B3C
	public void Close()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.clearItems();
	}

	// Token: 0x0600671D RID: 26397 RVA: 0x0028B960 File Offset: 0x00289B60
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

	// Token: 0x0600671E RID: 26398 RVA: 0x0028BB30 File Offset: 0x00289D30
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

	// Token: 0x04004F16 RID: 20246
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_PopupMenuItem.Entry> menuItems = new List<XUiC_PopupMenuItem.Entry>();

	// Token: 0x04004F17 RID: 20247
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i xuiPosition;

	// Token: 0x04004F18 RID: 20248
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i offset;

	// Token: 0x04004F19 RID: 20249
	public bool IsOver;

	// Token: 0x04004F1A RID: 20250
	[XuiBindComponent("list", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Grid gridView;

	// Token: 0x04004F1B RID: 20251
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_PopupMenuItem[] popupMenuItems;

	// Token: 0x04004F1F RID: 20255
	[PublicizedFrom(EAccessModifier.Private)]
	public string title;

	// Token: 0x04004F20 RID: 20256
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i gridSize = Vector2i.one;

	// Token: 0x04004F21 RID: 20257
	[PublicizedFrom(EAccessModifier.Private)]
	public bool justOpened;
}
