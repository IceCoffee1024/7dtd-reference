using System;
using InControl;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001047 RID: 4167
[global::UnityEngine.Scripting.Preserve]
public class XUiC_SignGridEntry : XUiC_SelectableEntry
{
	// Token: 0x17000F81 RID: 3969
	// (get) Token: 0x060083E3 RID: 33763 RVA: 0x00325DBC File Offset: 0x00323FBC
	public virtual Color32 BackgroundColor
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_SignGridEntry.backgroundColor;
		}
	}

	// Token: 0x17000F82 RID: 3970
	// (get) Token: 0x060083E4 RID: 33764 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool IsSelectable
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x060083E5 RID: 33765 RVA: 0x00325DC3 File Offset: 0x00323FC3
	public XUiC_SignGridEntry()
	{
		this.IsDirty = true;
	}

	// Token: 0x060083E6 RID: 33766 RVA: 0x00325DF1 File Offset: 0x00323FF1
	public override void Init()
	{
		base.Init();
		this.highlightOverlay = base.GetChildById("highlightOverlay").ViewComponent as XUiV_Sprite;
		this.background = base.GetChildById("background").ViewComponent as XUiV_Sprite;
	}

	// Token: 0x060083E7 RID: 33767 RVA: 0x00325E30 File Offset: 0x00324030
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

	// Token: 0x060083E8 RID: 33768 RVA: 0x00325E88 File Offset: 0x00324088
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

	// Token: 0x060083E9 RID: 33769 RVA: 0x00325FC2 File Offset: 0x003241C2
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void HandleClick()
	{
		base.IsSelected = true;
	}

	// Token: 0x060083EA RID: 33770 RVA: 0x00325FCC File Offset: 0x003241CC
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

	// Token: 0x060083EB RID: 33771 RVA: 0x00326020 File Offset: 0x00324220
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

	// Token: 0x040061EF RID: 25071
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x040061F0 RID: 25072
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x040061F1 RID: 25073
	public static Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x040061F2 RID: 25074
	public static Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x040061F3 RID: 25075
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite highlightOverlay;

	// Token: 0x040061F4 RID: 25076
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x040061F5 RID: 25077
	public Action<XUiC_SignGridEntry> OnBecameSelected;
}
