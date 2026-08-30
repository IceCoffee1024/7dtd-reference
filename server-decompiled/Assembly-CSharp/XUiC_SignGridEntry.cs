using System;
using InControl;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001038 RID: 4152
[global::UnityEngine.Scripting.Preserve]
public class XUiC_SignGridEntry : XUiC_SelectableEntry
{
	// Token: 0x17000F54 RID: 3924
	// (get) Token: 0x060083BA RID: 33722 RVA: 0x003223D4 File Offset: 0x003205D4
	public virtual Color32 BackgroundColor
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_SignGridEntry.backgroundColor;
		}
	}

	// Token: 0x17000F55 RID: 3925
	// (get) Token: 0x060083BB RID: 33723 RVA: 0x00020021 File Offset: 0x0001E221
	public virtual bool IsSelectable
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x060083BC RID: 33724 RVA: 0x003223DB File Offset: 0x003205DB
	public XUiC_SignGridEntry()
	{
		this.IsDirty = true;
	}

	// Token: 0x060083BD RID: 33725 RVA: 0x00322409 File Offset: 0x00320609
	public override void Init()
	{
		base.Init();
		this.highlightOverlay = base.GetChildById("highlightOverlay").ViewComponent as XUiV_Sprite;
		this.background = base.GetChildById("background").ViewComponent as XUiV_Sprite;
	}

	// Token: 0x060083BE RID: 33726 RVA: 0x00322448 File Offset: 0x00320648
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SelectedChanged(bool isSelected)
	{
		base.SelectedChanged(isSelected);
		if (!isSelected)
		{
			this.background.Color = this.BackgroundColor;
			return;
		}
		this.background.Color = this.selectColor;
		Action<XUiC_SignGridEntry> onBecameSelected = this.OnBecameSelected;
		if (onBecameSelected == null)
		{
			return;
		}
		onBecameSelected(this);
	}

	// Token: 0x060083BF RID: 33727 RVA: 0x003224A0 File Offset: 0x003206A0
	public override void Update(float _dt)
	{
		base.Update(_dt);
		PlayerActionsGUI guiactions = this.xui.playerUI.playerInput.GUIActions;
		if (base.WindowGroup.isShowing)
		{
			CursorControllerAbs cursorController = this.xui.playerUI.CursorController;
			cursorController.GetScreenPosition();
			bool mouseButtonUp = cursorController.GetMouseButtonUp(UICamera.MouseButton.LeftButton);
			cursorController.GetMouseButtonDown(UICamera.MouseButton.LeftButton);
			cursorController.GetMouseButton(UICamera.MouseButton.LeftButton);
			cursorController.GetMouseButtonUp(UICamera.MouseButton.RightButton);
			cursorController.GetMouseButtonDown(UICamera.MouseButton.RightButton);
			cursorController.GetMouseButton(UICamera.MouseButton.RightButton);
			if (this.isOver && base.ViewComponent.UiTransformIsHovered && base.ViewComponent.EventOnPress)
			{
				if (guiactions.LastInputType == BindingSourceType.DeviceBindingSource)
				{
					if (guiactions.Submit.WasReleased && this.IsSelectable)
					{
						this.HandleClick();
					}
				}
				else if (mouseButtonUp && this.IsSelectable)
				{
					this.HandleClick();
				}
			}
			else
			{
				if (this.highlightOverlay != null)
				{
					this.highlightOverlay.Color = this.BackgroundColor;
				}
				if (!base.IsSelected)
				{
					this.background.Color = this.BackgroundColor;
				}
				if (this.isOver)
				{
					this.isOver = false;
				}
			}
		}
		if (this.IsDirty)
		{
			this.IsDirty = false;
		}
	}

	// Token: 0x060083C0 RID: 33728 RVA: 0x003225DA File Offset: 0x003207DA
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void HandleClick()
	{
		base.IsSelected = true;
	}

	// Token: 0x060083C1 RID: 33729 RVA: 0x003225E4 File Offset: 0x003207E4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		this.isOver = _isOver;
		if (!base.IsSelected)
		{
			if (_isOver)
			{
				this.background.Color = XUiC_SignGridEntry.highlightColor;
			}
			else
			{
				this.background.Color = this.BackgroundColor;
			}
		}
		base.OnHovered(_isOver);
	}

	// Token: 0x060083C2 RID: 33730 RVA: 0x00322638 File Offset: 0x00320838
	public override bool ParseAttribute(string name, string value)
	{
		bool flag = base.ParseAttribute(name, value);
		if (!flag)
		{
			if (!(name == "select_color"))
			{
				if (!(name == "background_color"))
				{
					if (!(name == "highlight_color"))
					{
						return false;
					}
					XUiC_SignGridEntry.highlightColor = StringParsers.ParseColor32(value);
				}
				else
				{
					XUiC_SignGridEntry.backgroundColor = StringParsers.ParseColor32(value);
				}
			}
			else
			{
				this.selectColor = StringParsers.ParseColor32(value);
			}
			return true;
		}
		return flag;
	}

	// Token: 0x040061B4 RID: 25012
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x040061B5 RID: 25013
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x040061B6 RID: 25014
	public static Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x040061B7 RID: 25015
	public static Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x040061B8 RID: 25016
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite highlightOverlay;

	// Token: 0x040061B9 RID: 25017
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x040061BA RID: 25018
	public Action<XUiC_SignGridEntry> OnBecameSelected;
}
