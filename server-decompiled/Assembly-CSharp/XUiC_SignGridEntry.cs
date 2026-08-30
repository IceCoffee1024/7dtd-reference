using System;
using InControl;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200101F RID: 4127
[global::UnityEngine.Scripting.Preserve]
public class XUiC_SignGridEntry : XUiC_SelectableEntry
{
	// Token: 0x17000F1B RID: 3867
	// (get) Token: 0x060082C8 RID: 33480 RVA: 0x0031DBAC File Offset: 0x0031BDAC
	public virtual Color32 BackgroundColor
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_SignGridEntry.backgroundColor;
		}
	}

	// Token: 0x17000F1C RID: 3868
	// (get) Token: 0x060082C9 RID: 33481 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool IsSelectable
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x060082CA RID: 33482 RVA: 0x0031DBB3 File Offset: 0x0031BDB3
	public XUiC_SignGridEntry()
	{
		this.IsDirty = true;
	}

	// Token: 0x060082CB RID: 33483 RVA: 0x0031DBE1 File Offset: 0x0031BDE1
	public override void Init()
	{
		base.Init();
		this.highlightOverlay = base.GetChildById("highlightOverlay").ViewComponent as XUiV_Sprite;
		this.background = base.GetChildById("background").ViewComponent as XUiV_Sprite;
	}

	// Token: 0x060082CC RID: 33484 RVA: 0x0031DC20 File Offset: 0x0031BE20
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

	// Token: 0x060082CD RID: 33485 RVA: 0x0031DC78 File Offset: 0x0031BE78
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

	// Token: 0x060082CE RID: 33486 RVA: 0x0031DDB2 File Offset: 0x0031BFB2
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void HandleClick()
	{
		base.IsSelected = true;
	}

	// Token: 0x060082CF RID: 33487 RVA: 0x0031DDBC File Offset: 0x0031BFBC
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

	// Token: 0x060082D0 RID: 33488 RVA: 0x0031DE10 File Offset: 0x0031C010
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

	// Token: 0x040060ED RID: 24813
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x040060EE RID: 24814
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x040060EF RID: 24815
	public static Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x040060F0 RID: 24816
	public static Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x040060F1 RID: 24817
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite highlightOverlay;

	// Token: 0x040060F2 RID: 24818
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x040060F3 RID: 24819
	public Action<XUiC_SignGridEntry> OnBecameSelected;
}
