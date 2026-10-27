using System;
using GUI_2;
using InControl;
using Platform;

// Token: 0x0200117D RID: 4477
public class XUiV_GamepadIcon : XUiV_Sprite
{
	// Token: 0x06008DD9 RID: 36313 RVA: 0x0035CF82 File Offset: 0x0035B182
	public XUiV_GamepadIcon(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x170010D9 RID: 4313
	// (get) Token: 0x06008DDA RID: 36314 RVA: 0x0035CF9C File Offset: 0x0035B19C
	// (set) Token: 0x06008DDB RID: 36315 RVA: 0x0035CFC4 File Offset: 0x0035B1C4
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

	// Token: 0x170010DA RID: 4314
	// (get) Token: 0x06008DDC RID: 36316 RVA: 0x0035D001 File Offset: 0x0035B201
	// (set) Token: 0x06008DDD RID: 36317 RVA: 0x0035D009 File Offset: 0x0035B209
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

	// Token: 0x170010DB RID: 4315
	// (get) Token: 0x06008DDE RID: 36318 RVA: 0x0035D022 File Offset: 0x0035B222
	// (set) Token: 0x06008DDF RID: 36319 RVA: 0x0035D02A File Offset: 0x0035B22A
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

	// Token: 0x170010DC RID: 4316
	// (get) Token: 0x06008DE0 RID: 36320 RVA: 0x0035D048 File Offset: 0x0035B248
	// (set) Token: 0x06008DE1 RID: 36321 RVA: 0x0035D050 File Offset: 0x0035B250
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

	// Token: 0x06008DE2 RID: 36322 RVA: 0x0035D06C File Offset: 0x0035B26C
	public override void InitView()
	{
		base.UIAtlas = UIUtils.IconAtlas.Name;
		base.InitView();
		PlatformManager.NativePlatform.Input.OnLastInputStyleChanged += this.OnLastInputStyleChanged;
		this.curInput = PlatformManager.NativePlatform.Input.CurrentInputStyle;
	}

	// Token: 0x06008DE3 RID: 36323 RVA: 0x0035D0BF File Offset: 0x0035B2BF
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLastInputStyleChanged(PlayerInputManager.InputStyle _style)
	{
		this.curInput = _style;
		base.SetDirty();
	}

	// Token: 0x06008DE4 RID: 36324 RVA: 0x0035D0CE File Offset: 0x0035B2CE
	public override void Cleanup()
	{
		base.Cleanup();
		IPlatform nativePlatform = PlatformManager.NativePlatform;
		if (((nativePlatform != null) ? nativePlatform.Input : null) != null)
		{
			PlatformManager.NativePlatform.Input.OnLastInputStyleChanged -= this.OnLastInputStyleChanged;
		}
	}

	// Token: 0x06008DE5 RID: 36325 RVA: 0x0035D104 File Offset: 0x0035B304
	public override void Update(float _dt)
	{
		if (this.curInput != this.lastInputStyle)
		{
			this.lastInputStyle = this.curInput;
			base.SetDirty();
		}
		base.Update(_dt);
	}

	// Token: 0x06008DE6 RID: 36326 RVA: 0x0035D130 File Offset: 0x0035B330
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

	// Token: 0x06008DE7 RID: 36327 RVA: 0x0035D225 File Offset: 0x0035B425
	public override void SetDefaults(XUiController _parent)
	{
		base.SetDefaults(_parent);
		base.KeepSourceAspectRatio = true;
	}

	// Token: 0x0400687C RID: 26748
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInputManager.InputStyle lastInputStyle = PlayerInputManager.InputStyle.Count;

	// Token: 0x0400687D RID: 26749
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInputManager.InputStyle curInput;

	// Token: 0x0400687E RID: 26750
	[PublicizedFrom(EAccessModifier.Private)]
	public UIUtils.ButtonIcon? button;

	// Token: 0x0400687F RID: 26751
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_GamepadIcon.EActionSet actionSet = XUiV_GamepadIcon.EActionSet.Gui;

	// Token: 0x04006880 RID: 26752
	[PublicizedFrom(EAccessModifier.Private)]
	public string actionName;

	// Token: 0x04006881 RID: 26753
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerAction action;

	// Token: 0x0200117E RID: 4478
	public enum EActionSet
	{
		// Token: 0x04006883 RID: 26755
		Global,
		// Token: 0x04006884 RID: 26756
		Local,
		// Token: 0x04006885 RID: 26757
		Vehicle,
		// Token: 0x04006886 RID: 26758
		Gui,
		// Token: 0x04006887 RID: 26759
		Permanent
	}
}
