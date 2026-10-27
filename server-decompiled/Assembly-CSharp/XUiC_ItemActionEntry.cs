using System;
using Audio;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D8D RID: 3469
[global::UnityEngine.Scripting.Preserve]
public class XUiC_ItemActionEntry : XUiController
{
	// Token: 0x17000B3B RID: 2875
	// (get) Token: 0x06006883 RID: 26755 RVA: 0x0029829E File Offset: 0x0029649E
	// (set) Token: 0x06006884 RID: 26756 RVA: 0x002982A8 File Offset: 0x002964A8
	public BaseItemActionEntry ItemActionEntry
	{
		get
		{
			return this.itemActionEntry;
		}
		set
		{
			if (this.itemActionEntry != null)
			{
				this.itemActionEntry.ParentItem = null;
			}
			this.itemActionEntry = value;
			this.background.Enabled = value != null;
			if (this.itemActionEntry != null)
			{
				PlayerAction playerAction;
				switch (this.itemActionEntry.ShortCut)
				{
				case BaseItemActionEntry.GamepadShortCut.DPadUp:
					playerAction = this.xui.playerUI.playerInput.GUIActions.DPad_Up;
					break;
				case BaseItemActionEntry.GamepadShortCut.DPadLeft:
					playerAction = this.xui.playerUI.playerInput.GUIActions.DPad_Left;
					break;
				case BaseItemActionEntry.GamepadShortCut.DPadRight:
					playerAction = this.xui.playerUI.playerInput.GUIActions.DPad_Right;
					break;
				case BaseItemActionEntry.GamepadShortCut.DPadDown:
					playerAction = this.xui.playerUI.playerInput.GUIActions.DPad_Down;
					break;
				default:
					playerAction = null;
					break;
				}
				PlayerAction playerAction2 = playerAction;
				this.gamepadIcon.Action = playerAction2;
				this.keyboardButton.Text = playerAction2.GetBindingString(false, PlayerInputManager.InputStyle.Undefined, XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.KeyboardWithAngleBrackets, false, null);
				this.itemActionEntry.ParentItem = this;
				this.itemActionEntry.RefreshEnabled();
			}
			this.background.IsNavigatable = this.itemActionEntry != null;
			this.UpdateBindingsVisibility();
			base.RefreshBindings();
		}
	}

	// Token: 0x17000B3C RID: 2876
	// (get) Token: 0x06006885 RID: 26757 RVA: 0x002983E0 File Offset: 0x002965E0
	public XUiController Background
	{
		get
		{
			return this.background.Controller;
		}
	}

	// Token: 0x17000B3D RID: 2877
	// (get) Token: 0x06006886 RID: 26758 RVA: 0x002983ED File Offset: 0x002965ED
	public XUiV_GamepadIcon GamepadIcon
	{
		get
		{
			return this.gamepadIcon;
		}
	}

	// Token: 0x06006887 RID: 26759 RVA: 0x002983F8 File Offset: 0x002965F8
	public override void Init()
	{
		base.Init();
		this.lblName = base.GetChildById("name").ViewComponent as XUiV_Label;
		this.icoIcon = base.GetChildById("icon").ViewComponent as XUiV_Sprite;
		this.background = base.GetChildById("background").ViewComponent as XUiV_Sprite;
		this.gamepadIcon = base.GetChildById("gamepadIcon").ViewComponent as XUiV_GamepadIcon;
		this.keyboardButton = base.GetChildById("keyboardButton").ViewComponent as XUiV_Label;
		this.background.Controller.OnPress += this.OnPressAction;
		this.background.Controller.OnHover += this.OnHover;
		this.IsDirty = true;
		base.registerForInputStyleChanges();
	}

	// Token: 0x06006888 RID: 26760 RVA: 0x002984D8 File Offset: 0x002966D8
	[PublicizedFrom(EAccessModifier.Private)]
	public new void OnHover(XUiController _sender, bool _isOver)
	{
		XUiV_Sprite xuiV_Sprite = (XUiV_Sprite)_sender.ViewComponent;
		this.isOver = _isOver;
		if (this.itemActionEntry == null)
		{
			xuiV_Sprite.Color = this.defaultBackgroundColor;
			xuiV_Sprite.SpriteName = "menu_empty";
			return;
		}
		if (xuiV_Sprite != null)
		{
			if (_isOver)
			{
				xuiV_Sprite.Color = Color.white;
				xuiV_Sprite.SpriteName = "ui_game_select_row";
				return;
			}
			xuiV_Sprite.Color = this.defaultBackgroundColor;
			xuiV_Sprite.SpriteName = "menu_empty";
		}
	}

	// Token: 0x06006889 RID: 26761 RVA: 0x00298558 File Offset: 0x00296758
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPressAction(XUiController _sender, int _mouseButton)
	{
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		if (this.itemActionEntry != null)
		{
			if (this.itemActionEntry.Enabled)
			{
				Manager.PlayInsidePlayerHead(this.itemActionEntry.SoundName, -1, 0f, false, false);
				this.itemActionEntry.OnActivated();
			}
			else
			{
				Manager.PlayInsidePlayerHead(this.itemActionEntry.DisabledSound, -1, 0f, false, false);
				this.itemActionEntry.OnDisabledActivate();
			}
			this.background.Color = this.defaultBackgroundColor;
			this.background.SpriteName = "menu_empty";
			this.wasPressed = true;
		}
	}

	// Token: 0x0600688A RID: 26762 RVA: 0x00298604 File Offset: 0x00296804
	public override void Update(float _dt)
	{
		if (this.isOver && !this.background.UiTransformIsHovered)
		{
			this.background.Color = this.defaultBackgroundColor;
			this.background.SpriteName = "menu_empty";
			this.isOver = false;
		}
		if (this.isOver && this.wasPressed && this.itemActionEntry != null)
		{
			this.background.Color = Color.white;
			this.background.SpriteName = "ui_game_select_row";
			this.wasPressed = false;
		}
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
		base.RefreshBindings();
		base.Update(_dt);
	}

	// Token: 0x0600688B RID: 26763 RVA: 0x002986B4 File Offset: 0x002968B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		base.inputStyleChanged(_oldStyle, _newStyle);
		this.UpdateBindingsVisibility();
	}

	// Token: 0x0600688C RID: 26764 RVA: 0x002986C4 File Offset: 0x002968C4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		if (bindingName == "actionicon")
		{
			value = ((this.itemActionEntry != null) ? this.itemActionEntry.IconName : "");
			return true;
		}
		if (bindingName == "actionname")
		{
			value = ((this.itemActionEntry != null) ? this.itemActionEntry.ActionName : "");
			return true;
		}
		if (bindingName == "statuscolor")
		{
			value = "255,255,255,255";
			if (this.itemActionEntry != null)
			{
				Color32 color = (this.itemActionEntry.Enabled ? this.defaultFontColor : this.disabledFontColor);
				value = this.statuscolorFormatter.Format(color);
			}
			return true;
		}
		if (!(bindingName == "inspectheld"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		value = ((this.itemActionEntry != null && this.xui.playerUI.playerInput.GUIActions.Inspect.IsPressed) ? "true" : "false");
		return true;
	}

	// Token: 0x0600688D RID: 26765 RVA: 0x002987C2 File Offset: 0x002969C2
	public void StartTimedAction(float time)
	{
		GameManager.Instance.gameObject.AddComponent<XUiC_ItemActionEntry.TimedAction>().InitiateTimer(this.itemActionEntry, time);
	}

	// Token: 0x0600688E RID: 26766 RVA: 0x002987E0 File Offset: 0x002969E0
	public override bool ParseAttribute(string name, string value)
	{
		bool flag = base.ParseAttribute(name, value);
		if (!flag)
		{
			if (!(name == "default_font_color"))
			{
				if (!(name == "disabled_font_color"))
				{
					if (!(name == "default_background_color"))
					{
						return false;
					}
					this.defaultBackgroundColor = StringParsers.ParseColor32(value);
				}
				else
				{
					this.disabledFontColor = StringParsers.ParseColor32(value);
				}
			}
			else
			{
				this.defaultFontColor = StringParsers.ParseColor32(value);
			}
			return true;
		}
		return flag;
	}

	// Token: 0x0600688F RID: 26767 RVA: 0x00298860 File Offset: 0x00296A60
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateBindingsVisibility()
	{
		bool flag = base.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard;
		bool flag2 = this.itemActionEntry != null && this.itemActionEntry.ShortCut != BaseItemActionEntry.GamepadShortCut.None;
		this.gamepadIcon.IsVisible = !flag && flag2;
		this.keyboardButton.IsVisible = flag && flag2;
	}

	// Token: 0x06006890 RID: 26768 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	public void MarkDirty()
	{
		this.IsDirty = true;
	}

	// Token: 0x0400508E RID: 20622
	[PublicizedFrom(EAccessModifier.Protected)]
	public BaseItemActionEntry itemActionEntry;

	// Token: 0x0400508F RID: 20623
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Label lblName;

	// Token: 0x04005090 RID: 20624
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite icoIcon;

	// Token: 0x04005091 RID: 20625
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite background;

	// Token: 0x04005092 RID: 20626
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_GamepadIcon gamepadIcon;

	// Token: 0x04005093 RID: 20627
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Label keyboardButton;

	// Token: 0x04005094 RID: 20628
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 defaultBackgroundColor = Color.gray;

	// Token: 0x04005095 RID: 20629
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 disabledFontColor = Color.gray;

	// Token: 0x04005096 RID: 20630
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 defaultFontColor = Color.white;

	// Token: 0x04005097 RID: 20631
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isOver;

	// Token: 0x04005098 RID: 20632
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor statuscolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005099 RID: 20633
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasPressed;

	// Token: 0x02000D8E RID: 3470
	public class TimedAction : MonoBehaviour
	{
		// Token: 0x06006892 RID: 26770 RVA: 0x00298902 File Offset: 0x00296B02
		public void InitiateTimer(BaseItemActionEntry itemActionEntry, float _amount)
		{
			this.itemActionEntry = itemActionEntry;
			this.waitTime = Time.realtimeSinceStartup + _amount;
		}

		// Token: 0x06006893 RID: 26771 RVA: 0x00298918 File Offset: 0x00296B18
		[PublicizedFrom(EAccessModifier.Private)]
		public void Update()
		{
			if (this.waitTime != 0f && Time.realtimeSinceStartup >= this.waitTime)
			{
				this.waitTime = 0f;
				this.itemActionEntry.OnTimerCompleted();
				global::UnityEngine.Object.Destroy(this);
			}
		}

		// Token: 0x0400509A RID: 20634
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public BaseItemActionEntry itemActionEntry;

		// Token: 0x0400509B RID: 20635
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public float waitTime;
	}
}
