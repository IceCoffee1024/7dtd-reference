using System;
using GUI_2;
using InControl;
using Platform;

// Token: 0x02001155 RID: 4437
public class XUiV_GamepadIcon : XUiV_Sprite
{
	// Token: 0x06008D1A RID: 36122 RVA: 0x00356AB6 File Offset: 0x00354CB6
	public XUiV_GamepadIcon(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x17001099 RID: 4249
	// (get) Token: 0x06008D1B RID: 36123 RVA: 0x00356AD0 File Offset: 0x00354CD0
	// (set) Token: 0x06008D1C RID: 36124 RVA: 0x00356AF8 File Offset: 0x00354CF8
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

	// Token: 0x1700109A RID: 4250
	// (get) Token: 0x06008D1D RID: 36125 RVA: 0x00356B35 File Offset: 0x00354D35
	// (set) Token: 0x06008D1E RID: 36126 RVA: 0x00356B3D File Offset: 0x00354D3D
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

	// Token: 0x1700109B RID: 4251
	// (get) Token: 0x06008D1F RID: 36127 RVA: 0x00356B56 File Offset: 0x00354D56
	// (set) Token: 0x06008D20 RID: 36128 RVA: 0x00356B5E File Offset: 0x00354D5E
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

	// Token: 0x1700109C RID: 4252
	// (get) Token: 0x06008D21 RID: 36129 RVA: 0x00356B7C File Offset: 0x00354D7C
	// (set) Token: 0x06008D22 RID: 36130 RVA: 0x00356B84 File Offset: 0x00354D84
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

	// Token: 0x06008D23 RID: 36131 RVA: 0x00356BA0 File Offset: 0x00354DA0
	public override void InitView()
	{
		base.UIAtlas = UIUtils.IconAtlas.Name;
		base.InitView();
		PlatformManager.NativePlatform.Input.OnLastInputStyleChanged += this.OnLastInputStyleChanged;
		this.curInput = PlatformManager.NativePlatform.Input.CurrentInputStyle;
	}

	// Token: 0x06008D24 RID: 36132 RVA: 0x00356BF3 File Offset: 0x00354DF3
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLastInputStyleChanged(PlayerInputManager.InputStyle _style)
	{
		this.curInput = _style;
		base.SetDirty();
	}

	// Token: 0x06008D25 RID: 36133 RVA: 0x00356C02 File Offset: 0x00354E02
	public override void Cleanup()
	{
		base.Cleanup();
		IPlatform nativePlatform = PlatformManager.NativePlatform;
		if (((nativePlatform != null) ? nativePlatform.Input : null) != null)
		{
			PlatformManager.NativePlatform.Input.OnLastInputStyleChanged -= this.OnLastInputStyleChanged;
		}
	}

	// Token: 0x06008D26 RID: 36134 RVA: 0x00356C38 File Offset: 0x00354E38
	public override void Update(float _dt)
	{
		if (this.curInput != this.lastInputStyle)
		{
			this.lastInputStyle = this.curInput;
			base.SetDirty();
		}
		base.Update(_dt);
	}

	// Token: 0x06008D27 RID: 36135 RVA: 0x00356C64 File Offset: 0x00354E64
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

	// Token: 0x06008D28 RID: 36136 RVA: 0x00356D59 File Offset: 0x00354F59
	public override void SetDefaults(XUiController _parent)
	{
		base.SetDefaults(_parent);
		base.KeepSourceAspectRatio = true;
	}

	// Token: 0x040067E7 RID: 26599
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInputManager.InputStyle lastInputStyle = PlayerInputManager.InputStyle.Count;

	// Token: 0x040067E8 RID: 26600
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInputManager.InputStyle curInput;

	// Token: 0x040067E9 RID: 26601
	[PublicizedFrom(EAccessModifier.Private)]
	public UIUtils.ButtonIcon? button;

	// Token: 0x040067EA RID: 26602
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_GamepadIcon.EActionSet actionSet = XUiV_GamepadIcon.EActionSet.Gui;

	// Token: 0x040067EB RID: 26603
	[PublicizedFrom(EAccessModifier.Private)]
	public string actionName;

	// Token: 0x040067EC RID: 26604
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerAction action;

	// Token: 0x02001156 RID: 4438
	public enum EActionSet
	{
		// Token: 0x040067EE RID: 26606
		Global,
		// Token: 0x040067EF RID: 26607
		Local,
		// Token: 0x040067F0 RID: 26608
		Vehicle,
		// Token: 0x040067F1 RID: 26609
		Gui,
		// Token: 0x040067F2 RID: 26610
		Permanent
	}
}
