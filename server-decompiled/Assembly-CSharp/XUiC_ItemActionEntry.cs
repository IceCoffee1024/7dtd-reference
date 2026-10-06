using System;
using Audio;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D82 RID: 3458
[global::UnityEngine.Scripting.Preserve]
public class XUiC_ItemActionEntry : XUiController
{
	// Token: 0x17000B35 RID: 2869
	// (get) Token: 0x0600685E RID: 26718 RVA: 0x00296D76 File Offset: 0x00294F76
	// (set) Token: 0x0600685F RID: 26719 RVA: 0x00296D80 File Offset: 0x00294F80
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

	// Token: 0x17000B36 RID: 2870
	// (get) Token: 0x06006860 RID: 26720 RVA: 0x00296EB8 File Offset: 0x002950B8
	public XUiController Background
	{
		get
		{
			return this.background.Controller;
		}
	}

	// Token: 0x17000B37 RID: 2871
	// (get) Token: 0x06006861 RID: 26721 RVA: 0x00296EC5 File Offset: 0x002950C5
	public XUiV_GamepadIcon GamepadIcon
	{
		get
		{
			return this.gamepadIcon;
		}
	}

	// Token: 0x06006862 RID: 26722 RVA: 0x00296ED0 File Offset: 0x002950D0
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

	// Token: 0x06006863 RID: 26723 RVA: 0x00296FB0 File Offset: 0x002951B0
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

	// Token: 0x06006864 RID: 26724 RVA: 0x00297030 File Offset: 0x00295230
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

	// Token: 0x06006865 RID: 26725 RVA: 0x002970DC File Offset: 0x002952DC
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

	// Token: 0x06006866 RID: 26726 RVA: 0x0029718C File Offset: 0x0029538C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		base.inputStyleChanged(_oldStyle, _newStyle);
		this.UpdateBindingsVisibility();
	}

	// Token: 0x06006867 RID: 26727 RVA: 0x0029719C File Offset: 0x0029539C
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

	// Token: 0x06006868 RID: 26728 RVA: 0x0029729A File Offset: 0x0029549A
	public void StartTimedAction(float time)
	{
		GameManager.Instance.gameObject.AddComponent<XUiC_ItemActionEntry.TimedAction>().InitiateTimer(this.itemActionEntry, time);
	}

	// Token: 0x06006869 RID: 26729 RVA: 0x002972B8 File Offset: 0x002954B8
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

	// Token: 0x0600686A RID: 26730 RVA: 0x00297338 File Offset: 0x00295538
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateBindingsVisibility()
	{
		bool flag = base.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard;
		bool flag2 = this.itemActionEntry != null && this.itemActionEntry.ShortCut != BaseItemActionEntry.GamepadShortCut.None;
		this.gamepadIcon.IsVisible = !flag && flag2;
		this.keyboardButton.IsVisible = flag && flag2;
	}

	// Token: 0x0600686B RID: 26731 RVA: 0x0008A995 File Offset: 0x00088B95
	public void MarkDirty()
	{
		this.IsDirty = true;
	}

	// Token: 0x04005069 RID: 20585
	[PublicizedFrom(EAccessModifier.Protected)]
	public BaseItemActionEntry itemActionEntry;

	// Token: 0x0400506A RID: 20586
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Label lblName;

	// Token: 0x0400506B RID: 20587
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite icoIcon;

	// Token: 0x0400506C RID: 20588
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite background;

	// Token: 0x0400506D RID: 20589
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_GamepadIcon gamepadIcon;

	// Token: 0x0400506E RID: 20590
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Label keyboardButton;

	// Token: 0x0400506F RID: 20591
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 defaultBackgroundColor = Color.gray;

	// Token: 0x04005070 RID: 20592
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 disabledFontColor = Color.gray;

	// Token: 0x04005071 RID: 20593
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 defaultFontColor = Color.white;

	// Token: 0x04005072 RID: 20594
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isOver;

	// Token: 0x04005073 RID: 20595
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor statuscolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005074 RID: 20596
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasPressed;

	// Token: 0x02000D83 RID: 3459
	public class TimedAction : MonoBehaviour
	{
		// Token: 0x0600686D RID: 26733 RVA: 0x002973DA File Offset: 0x002955DA
		public void InitiateTimer(BaseItemActionEntry itemActionEntry, float _amount)
		{
			this.itemActionEntry = itemActionEntry;
			this.waitTime = Time.realtimeSinceStartup + _amount;
		}

		// Token: 0x0600686E RID: 26734 RVA: 0x002973F0 File Offset: 0x002955F0
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

		// Token: 0x04005075 RID: 20597
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public BaseItemActionEntry itemActionEntry;

		// Token: 0x04005076 RID: 20598
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public float waitTime;
	}
}
