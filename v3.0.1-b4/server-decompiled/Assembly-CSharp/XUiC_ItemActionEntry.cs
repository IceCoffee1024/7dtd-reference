using System;
using Audio;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D5F RID: 3423
[global::UnityEngine.Scripting.Preserve]
public class XUiC_ItemActionEntry : XUiController
{
	// Token: 0x17000B02 RID: 2818
	// (get) Token: 0x060067CA RID: 26570 RVA: 0x00290896 File Offset: 0x0028EA96
	// (set) Token: 0x060067CB RID: 26571 RVA: 0x002908A0 File Offset: 0x0028EAA0
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

	// Token: 0x17000B03 RID: 2819
	// (get) Token: 0x060067CC RID: 26572 RVA: 0x002909D8 File Offset: 0x0028EBD8
	public XUiController Background
	{
		get
		{
			return this.background.Controller;
		}
	}

	// Token: 0x17000B04 RID: 2820
	// (get) Token: 0x060067CD RID: 26573 RVA: 0x002909E5 File Offset: 0x0028EBE5
	public XUiV_GamepadIcon GamepadIcon
	{
		get
		{
			return this.gamepadIcon;
		}
	}

	// Token: 0x060067CE RID: 26574 RVA: 0x002909F0 File Offset: 0x0028EBF0
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

	// Token: 0x060067CF RID: 26575 RVA: 0x00290AD0 File Offset: 0x0028ECD0
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

	// Token: 0x060067D0 RID: 26576 RVA: 0x00290B50 File Offset: 0x0028ED50
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

	// Token: 0x060067D1 RID: 26577 RVA: 0x00290BFC File Offset: 0x0028EDFC
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

	// Token: 0x060067D2 RID: 26578 RVA: 0x00290CAC File Offset: 0x0028EEAC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		base.inputStyleChanged(_oldStyle, _newStyle);
		this.UpdateBindingsVisibility();
	}

	// Token: 0x060067D3 RID: 26579 RVA: 0x00290CBC File Offset: 0x0028EEBC
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

	// Token: 0x060067D4 RID: 26580 RVA: 0x00290DBA File Offset: 0x0028EFBA
	public void StartTimedAction(float time)
	{
		GameManager.Instance.gameObject.AddComponent<XUiC_ItemActionEntry.TimedAction>().InitiateTimer(this.itemActionEntry, time);
	}

	// Token: 0x060067D5 RID: 26581 RVA: 0x00290DD8 File Offset: 0x0028EFD8
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

	// Token: 0x060067D6 RID: 26582 RVA: 0x00290E58 File Offset: 0x0028F058
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateBindingsVisibility()
	{
		bool flag = base.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard;
		bool flag2 = this.itemActionEntry != null && this.itemActionEntry.ShortCut != BaseItemActionEntry.GamepadShortCut.None;
		this.gamepadIcon.IsVisible = !flag && flag2;
		this.keyboardButton.IsVisible = flag && flag2;
	}

	// Token: 0x060067D7 RID: 26583 RVA: 0x0008A069 File Offset: 0x00088269
	public void MarkDirty()
	{
		this.IsDirty = true;
	}

	// Token: 0x04004F90 RID: 20368
	[PublicizedFrom(EAccessModifier.Protected)]
	public BaseItemActionEntry itemActionEntry;

	// Token: 0x04004F91 RID: 20369
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Label lblName;

	// Token: 0x04004F92 RID: 20370
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite icoIcon;

	// Token: 0x04004F93 RID: 20371
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite background;

	// Token: 0x04004F94 RID: 20372
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_GamepadIcon gamepadIcon;

	// Token: 0x04004F95 RID: 20373
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Label keyboardButton;

	// Token: 0x04004F96 RID: 20374
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 defaultBackgroundColor = Color.gray;

	// Token: 0x04004F97 RID: 20375
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 disabledFontColor = Color.gray;

	// Token: 0x04004F98 RID: 20376
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 defaultFontColor = Color.white;

	// Token: 0x04004F99 RID: 20377
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isOver;

	// Token: 0x04004F9A RID: 20378
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor statuscolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04004F9B RID: 20379
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasPressed;

	// Token: 0x02000D60 RID: 3424
	public class TimedAction : MonoBehaviour
	{
		// Token: 0x060067D9 RID: 26585 RVA: 0x00290EFA File Offset: 0x0028F0FA
		public void InitiateTimer(BaseItemActionEntry itemActionEntry, float _amount)
		{
			this.itemActionEntry = itemActionEntry;
			this.waitTime = Time.realtimeSinceStartup + _amount;
		}

		// Token: 0x060067DA RID: 26586 RVA: 0x00290F10 File Offset: 0x0028F110
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

		// Token: 0x04004F9C RID: 20380
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public BaseItemActionEntry itemActionEntry;

		// Token: 0x04004F9D RID: 20381
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public float waitTime;
	}
}
