using System;
using InControl;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001057 RID: 4183
[global::UnityEngine.Scripting.Preserve]
public class XUiC_SignGridEntry : XUiC_SelectableEntry
{
	// Token: 0x17000F8C RID: 3980
	// (get) Token: 0x06008435 RID: 33845 RVA: 0x00327AC0 File Offset: 0x00325CC0
	public virtual Color32 BackgroundColor
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_SignGridEntry.backgroundColor;
		}
	}

	// Token: 0x17000F8D RID: 3981
	// (get) Token: 0x06008436 RID: 33846 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool IsSelectable
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x06008437 RID: 33847 RVA: 0x00327AC7 File Offset: 0x00325CC7
	public XUiC_SignGridEntry()
	{
		this.IsDirty = true;
	}

	// Token: 0x06008438 RID: 33848 RVA: 0x00327AF5 File Offset: 0x00325CF5
	public override void Init()
	{
		base.Init();
		this.highlightOverlay = base.GetChildById("highlightOverlay").ViewComponent as XUiV_Sprite;
		this.background = base.GetChildById("background").ViewComponent as XUiV_Sprite;
	}

	// Token: 0x06008439 RID: 33849 RVA: 0x00327B34 File Offset: 0x00325D34
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

	// Token: 0x0600843A RID: 33850 RVA: 0x00327B8C File Offset: 0x00325D8C
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

	// Token: 0x0600843B RID: 33851 RVA: 0x00327CC6 File Offset: 0x00325EC6
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void HandleClick()
	{
		base.IsSelected = true;
	}

	// Token: 0x0600843C RID: 33852 RVA: 0x00327CD0 File Offset: 0x00325ED0
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

	// Token: 0x0600843D RID: 33853 RVA: 0x00327D24 File Offset: 0x00325F24
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

	// Token: 0x0400622D RID: 25133
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x0400622E RID: 25134
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x0400622F RID: 25135
	public static Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x04006230 RID: 25136
	public static Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04006231 RID: 25137
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite highlightOverlay;

	// Token: 0x04006232 RID: 25138
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04006233 RID: 25139
	public Action<XUiC_SignGridEntry> OnBecameSelected;
}
