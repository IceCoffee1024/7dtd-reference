using System;
using GUI_2;
using InControl;
using Platform;

// Token: 0x0200113C RID: 4412
public class XUiV_GamepadIcon : XUiV_Sprite
{
	// Token: 0x06008C28 RID: 35880 RVA: 0x00352282 File Offset: 0x00350482
	public XUiV_GamepadIcon(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x17001060 RID: 4192
	// (get) Token: 0x06008C29 RID: 35881 RVA: 0x0035229C File Offset: 0x0035049C
	// (set) Token: 0x06008C2A RID: 35882 RVA: 0x003522C4 File Offset: 0x003504C4
	[XuiXmlAttribute("button", false)]
	public UIUtils.ButtonIcon Button
	{
		get
		{
			UIUtils.ButtonIcon? buttonIcon = this.button;
			if (buttonIcon == null)
			{
				return UIUtils.ButtonIcon.None;
			}
			return buttonIcon.GetValueOrDefault();
		}
		set
		{
			UIUtils.ButtonIcon? buttonIcon = this.button;
			if ((buttonIcon.GetValueOrDefault() == value) & (buttonIcon != null))
			{
				return;
			}
			this.button = new UIUtils.ButtonIcon?(value);
			base.SetDirty();
		}
	}

	// Token: 0x17001061 RID: 4193
	// (get) Token: 0x06008C2B RID: 35883 RVA: 0x00352301 File Offset: 0x00350501
	// (set) Token: 0x06008C2C RID: 35884 RVA: 0x00352309 File Offset: 0x00350509
	[XuiXmlAttribute("actionset", false)]
	public XUiV_GamepadIcon.EActionSet ActionSet
	{
		get
		{
			return this.actionSet;
		}
		set
		{
			if (this.actionSet == value)
			{
				return;
			}
			this.actionSet = value;
			base.SetDirty();
		}
	}

	// Token: 0x17001062 RID: 4194
	// (get) Token: 0x06008C2D RID: 35885 RVA: 0x00352322 File Offset: 0x00350522
	// (set) Token: 0x06008C2E RID: 35886 RVA: 0x0035232A File Offset: 0x0035052A
	[XuiXmlAttribute("actionname", false)]
	public string ActionName
	{
		get
		{
			return this.actionName;
		}
		set
		{
			if (this.actionName == value)
			{
				return;
			}
			this.actionName = value;
			base.SetDirty();
		}
	}

	// Token: 0x17001063 RID: 4195
	// (get) Token: 0x06008C2F RID: 35887 RVA: 0x00352348 File Offset: 0x00350548
	// (set) Token: 0x06008C30 RID: 35888 RVA: 0x00352350 File Offset: 0x00350550
	[XuiXmlAttribute("action", false)]
	public PlayerAction Action
	{
		get
		{
			return this.action;
		}
		set
		{
			if (this.action == value)
			{
				return;
			}
			this.action = value;
			base.SetDirty();
		}
	}

	// Token: 0x06008C31 RID: 35889 RVA: 0x0035236C File Offset: 0x0035056C
	public override void InitView()
	{
		base.UIAtlas = UIUtils.IconAtlas.Name;
		base.InitView();
		PlatformManager.NativePlatform.Input.OnLastInputStyleChanged += this.OnLastInputStyleChanged;
		this.curInput = PlatformManager.NativePlatform.Input.CurrentInputStyle;
	}

	// Token: 0x06008C32 RID: 35890 RVA: 0x003523BF File Offset: 0x003505BF
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLastInputStyleChanged(PlayerInputManager.InputStyle _style)
	{
		this.curInput = _style;
	}

	// Token: 0x06008C33 RID: 35891 RVA: 0x003523C8 File Offset: 0x003505C8
	public override void Cleanup()
	{
		base.Cleanup();
		IPlatform nativePlatform = PlatformManager.NativePlatform;
		if (((nativePlatform != null) ? nativePlatform.Input : null) != null)
		{
			PlatformManager.NativePlatform.Input.OnLastInputStyleChanged -= this.OnLastInputStyleChanged;
		}
	}

	// Token: 0x06008C34 RID: 35892 RVA: 0x003523FE File Offset: 0x003505FE
	public override void Update(float _dt)
	{
		if (this.curInput != this.lastInputStyle)
		{
			this.lastInputStyle = this.curInput;
			base.SetDirty();
		}
		base.Update(_dt);
	}

	// Token: 0x06008C35 RID: 35893 RVA: 0x00352428 File Offset: 0x00350628
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		base.UIAtlas = ((this.curInput == PlayerInputManager.InputStyle.Keyboard) ? "" : UIUtils.IconAtlas.Name);
		PlayerAction playerAction = null;
		if (!string.IsNullOrEmpty(this.ActionName))
		{
			PlayerActionSet playerActionSet;
			switch (this.ActionSet)
			{
			case XUiV_GamepadIcon.EActionSet.Global:
				playerActionSet = PlayerActionsGlobal.Instance;
				break;
			case XUiV_GamepadIcon.EActionSet.Local:
				playerActionSet = LocalPlayerUI.primaryUI.playerInput;
				break;
			case XUiV_GamepadIcon.EActionSet.Vehicle:
				playerActionSet = LocalPlayerUI.primaryUI.playerInput.VehicleActions;
				break;
			case XUiV_GamepadIcon.EActionSet.Gui:
				playerActionSet = LocalPlayerUI.primaryUI.playerInput.GUIActions;
				break;
			case XUiV_GamepadIcon.EActionSet.Permanent:
				playerActionSet = LocalPlayerUI.primaryUI.playerInput.PermanentActions;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			playerAction = playerActionSet.GetPlayerActionByName(this.ActionName);
		}
		else if (this.Action != null)
		{
			playerAction = this.Action;
		}
		UIUtils.ButtonIcon buttonIcon = ((playerAction != null) ? UIUtils.GetButtonIconForAction(playerAction) : this.Button);
		base.SpriteName = UIUtils.GetSpriteName(buttonIcon);
		base.updateData();
	}

	// Token: 0x06008C36 RID: 35894 RVA: 0x0035251D File Offset: 0x0035071D
	public override void SetDefaults(XUiController _parent)
	{
		base.SetDefaults(_parent);
		base.KeepSourceAspectRatio = true;
	}

	// Token: 0x0400671D RID: 26397
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInputManager.InputStyle lastInputStyle = PlayerInputManager.InputStyle.Count;

	// Token: 0x0400671E RID: 26398
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInputManager.InputStyle curInput;

	// Token: 0x0400671F RID: 26399
	[PublicizedFrom(EAccessModifier.Private)]
	public UIUtils.ButtonIcon? button;

	// Token: 0x04006720 RID: 26400
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_GamepadIcon.EActionSet actionSet = XUiV_GamepadIcon.EActionSet.Gui;

	// Token: 0x04006721 RID: 26401
	[PublicizedFrom(EAccessModifier.Private)]
	public string actionName;

	// Token: 0x04006722 RID: 26402
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerAction action;

	// Token: 0x0200113D RID: 4413
	public enum EActionSet
	{
		// Token: 0x04006724 RID: 26404
		Global,
		// Token: 0x04006725 RID: 26405
		Local,
		// Token: 0x04006726 RID: 26406
		Vehicle,
		// Token: 0x04006727 RID: 26407
		Gui,
		// Token: 0x04006728 RID: 26408
		Permanent
	}
}
