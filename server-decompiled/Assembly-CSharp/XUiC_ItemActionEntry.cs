using System;
using Audio;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D72 RID: 3442
[global::UnityEngine.Scripting.Preserve]
public class XUiC_ItemActionEntry : XUiController
{
	// Token: 0x17000B15 RID: 2837
	// (get) Token: 0x06006859 RID: 26713 RVA: 0x0029382A File Offset: 0x00291A2A
	// (set) Token: 0x0600685A RID: 26714 RVA: 0x00293834 File Offset: 0x00291A34
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

	// Token: 0x17000B16 RID: 2838
	// (get) Token: 0x0600685B RID: 26715 RVA: 0x0029396C File Offset: 0x00291B6C
	public XUiController Background
	{
		get
		{
			return this.background.Controller;
		}
	}

	// Token: 0x17000B17 RID: 2839
	// (get) Token: 0x0600685C RID: 26716 RVA: 0x00293979 File Offset: 0x00291B79
	public XUiV_GamepadIcon GamepadIcon
	{
		get
		{
			return this.gamepadIcon;
		}
	}

	// Token: 0x0600685D RID: 26717 RVA: 0x00293984 File Offset: 0x00291B84
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

	// Token: 0x0600685E RID: 26718 RVA: 0x00293A64 File Offset: 0x00291C64
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

	// Token: 0x0600685F RID: 26719 RVA: 0x00293AE4 File Offset: 0x00291CE4
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

	// Token: 0x06006860 RID: 26720 RVA: 0x00293B90 File Offset: 0x00291D90
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

	// Token: 0x06006861 RID: 26721 RVA: 0x00293C40 File Offset: 0x00291E40
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		base.inputStyleChanged(_oldStyle, _newStyle);
		this.UpdateBindingsVisibility();
	}

	// Token: 0x06006862 RID: 26722 RVA: 0x00293C50 File Offset: 0x00291E50
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

	// Token: 0x06006863 RID: 26723 RVA: 0x00293D4E File Offset: 0x00291F4E
	public void StartTimedAction(float time)
	{
		GameManager.Instance.gameObject.AddComponent<XUiC_ItemActionEntry.TimedAction>().InitiateTimer(this.itemActionEntry, time);
	}

	// Token: 0x06006864 RID: 26724 RVA: 0x00293D6C File Offset: 0x00291F6C
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

	// Token: 0x06006865 RID: 26725 RVA: 0x00293DEC File Offset: 0x00291FEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateBindingsVisibility()
	{
		bool flag = base.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard;
		bool flag2 = this.itemActionEntry != null && this.itemActionEntry.ShortCut != BaseItemActionEntry.GamepadShortCut.None;
		this.gamepadIcon.IsVisible = !flag && flag2;
		this.keyboardButton.IsVisible = flag && flag2;
	}

	// Token: 0x06006866 RID: 26726 RVA: 0x0008A715 File Offset: 0x00088915
	public void MarkDirty()
	{
		this.IsDirty = true;
	}

	// Token: 0x0400502E RID: 20526
	[PublicizedFrom(EAccessModifier.Protected)]
	public BaseItemActionEntry itemActionEntry;

	// Token: 0x0400502F RID: 20527
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Label lblName;

	// Token: 0x04005030 RID: 20528
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite icoIcon;

	// Token: 0x04005031 RID: 20529
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite background;

	// Token: 0x04005032 RID: 20530
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_GamepadIcon gamepadIcon;

	// Token: 0x04005033 RID: 20531
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Label keyboardButton;

	// Token: 0x04005034 RID: 20532
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 defaultBackgroundColor = Color.gray;

	// Token: 0x04005035 RID: 20533
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 disabledFontColor = Color.gray;

	// Token: 0x04005036 RID: 20534
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 defaultFontColor = Color.white;

	// Token: 0x04005037 RID: 20535
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isOver;

	// Token: 0x04005038 RID: 20536
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor statuscolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005039 RID: 20537
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasPressed;

	// Token: 0x02000D73 RID: 3443
	public class TimedAction : MonoBehaviour
	{
		// Token: 0x06006868 RID: 26728 RVA: 0x00293E8E File Offset: 0x0029208E
		public void InitiateTimer(BaseItemActionEntry itemActionEntry, float _amount)
		{
			this.itemActionEntry = itemActionEntry;
			this.waitTime = Time.realtimeSinceStartup + _amount;
		}

		// Token: 0x06006869 RID: 26729 RVA: 0x00293EA4 File Offset: 0x002920A4
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

		// Token: 0x0400503A RID: 20538
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public BaseItemActionEntry itemActionEntry;

		// Token: 0x0400503B RID: 20539
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public float waitTime;
	}
}
