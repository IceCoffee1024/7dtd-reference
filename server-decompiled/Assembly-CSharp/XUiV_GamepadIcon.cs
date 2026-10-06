using System;
using GUI_2;
using InControl;
using Platform;

// Token: 0x0200116D RID: 4461
public class XUiV_GamepadIcon : XUiV_Sprite
{
	// Token: 0x06008D85 RID: 36229 RVA: 0x0035AED6 File Offset: 0x003590D6
	public XUiV_GamepadIcon(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x170010CE RID: 4302
	// (get) Token: 0x06008D86 RID: 36230 RVA: 0x0035AEF0 File Offset: 0x003590F0
	// (set) Token: 0x06008D87 RID: 36231 RVA: 0x0035AF18 File Offset: 0x00359118
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

	// Token: 0x170010CF RID: 4303
	// (get) Token: 0x06008D88 RID: 36232 RVA: 0x0035AF55 File Offset: 0x00359155
	// (set) Token: 0x06008D89 RID: 36233 RVA: 0x0035AF5D File Offset: 0x0035915D
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

	// Token: 0x170010D0 RID: 4304
	// (get) Token: 0x06008D8A RID: 36234 RVA: 0x0035AF76 File Offset: 0x00359176
	// (set) Token: 0x06008D8B RID: 36235 RVA: 0x0035AF7E File Offset: 0x0035917E
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

	// Token: 0x170010D1 RID: 4305
	// (get) Token: 0x06008D8C RID: 36236 RVA: 0x0035AF9C File Offset: 0x0035919C
	// (set) Token: 0x06008D8D RID: 36237 RVA: 0x0035AFA4 File Offset: 0x003591A4
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

	// Token: 0x06008D8E RID: 36238 RVA: 0x0035AFC0 File Offset: 0x003591C0
	public override void InitView()
	{
		base.UIAtlas = UIUtils.IconAtlas.Name;
		base.InitView();
		PlatformManager.NativePlatform.Input.OnLastInputStyleChanged += this.OnLastInputStyleChanged;
		this.curInput = PlatformManager.NativePlatform.Input.CurrentInputStyle;
	}

	// Token: 0x06008D8F RID: 36239 RVA: 0x0035B013 File Offset: 0x00359213
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLastInputStyleChanged(PlayerInputManager.InputStyle _style)
	{
		this.curInput = _style;
		base.SetDirty();
	}

	// Token: 0x06008D90 RID: 36240 RVA: 0x0035B022 File Offset: 0x00359222
	public override void Cleanup()
	{
		base.Cleanup();
		IPlatform nativePlatform = PlatformManager.NativePlatform;
		if (((nativePlatform != null) ? nativePlatform.Input : null) != null)
		{
			PlatformManager.NativePlatform.Input.OnLastInputStyleChanged -= this.OnLastInputStyleChanged;
		}
	}

	// Token: 0x06008D91 RID: 36241 RVA: 0x0035B058 File Offset: 0x00359258
	public override void Update(float _dt)
	{
		if (this.curInput != this.lastInputStyle)
		{
			this.lastInputStyle = this.curInput;
			base.SetDirty();
		}
		base.Update(_dt);
	}

	// Token: 0x06008D92 RID: 36242 RVA: 0x0035B084 File Offset: 0x00359284
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

	// Token: 0x06008D93 RID: 36243 RVA: 0x0035B179 File Offset: 0x00359379
	public override void SetDefaults(XUiController _parent)
	{
		base.SetDefaults(_parent);
		base.KeepSourceAspectRatio = true;
	}

	// Token: 0x0400683C RID: 26684
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInputManager.InputStyle lastInputStyle = PlayerInputManager.InputStyle.Count;

	// Token: 0x0400683D RID: 26685
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInputManager.InputStyle curInput;

	// Token: 0x0400683E RID: 26686
	[PublicizedFrom(EAccessModifier.Private)]
	public UIUtils.ButtonIcon? button;

	// Token: 0x0400683F RID: 26687
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_GamepadIcon.EActionSet actionSet = XUiV_GamepadIcon.EActionSet.Gui;

	// Token: 0x04006840 RID: 26688
	[PublicizedFrom(EAccessModifier.Private)]
	public string actionName;

	// Token: 0x04006841 RID: 26689
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerAction action;

	// Token: 0x0200116E RID: 4462
	public enum EActionSet
	{
		// Token: 0x04006843 RID: 26691
		Global,
		// Token: 0x04006844 RID: 26692
		Local,
		// Token: 0x04006845 RID: 26693
		Vehicle,
		// Token: 0x04006846 RID: 26694
		Gui,
		// Token: 0x04006847 RID: 26695
		Permanent
	}
}
