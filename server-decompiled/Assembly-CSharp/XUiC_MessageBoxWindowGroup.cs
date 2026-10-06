using System;
using System.Collections;
using System.Runtime.CompilerServices;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D56 RID: 3414
[global::UnityEngine.Scripting.Preserve]
public class XUiC_MessageBoxWindowGroup : XUiController
{
	// Token: 0x17000B01 RID: 2817
	// (get) Token: 0x0600675A RID: 26458 RVA: 0x00291130 File Offset: 0x0028F330
	// (set) Token: 0x0600675B RID: 26459 RVA: 0x00291138 File Offset: 0x0028F338
	[XuiXmlBinding("msgTitle")]
	public string Title
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000B02 RID: 2818
	// (get) Token: 0x0600675C RID: 26460 RVA: 0x00291141 File Offset: 0x0028F341
	// (set) Token: 0x0600675D RID: 26461 RVA: 0x00291149 File Offset: 0x0028F349
	[XuiXmlBinding("msgText")]
	public string Text
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000B03 RID: 2819
	// (get) Token: 0x0600675E RID: 26462 RVA: 0x00291152 File Offset: 0x0028F352
	// (set) Token: 0x0600675F RID: 26463 RVA: 0x0029115A File Offset: 0x0028F35A
	[XuiXmlBinding("msgIcon")]
	public string Icon
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000B04 RID: 2820
	// (get) Token: 0x06006760 RID: 26464 RVA: 0x00291163 File Offset: 0x0028F363
	public XUiC_MessageBoxWindowGroup.MessageButton[] Buttons
	{
		get
		{
			return this.buttons;
		}
	}

	// Token: 0x06006761 RID: 26465 RVA: 0x0029116B File Offset: 0x0028F36B
	public override void Init()
	{
		base.Init();
		XUiC_MessageBoxWindowGroup.id = base.WindowGroup.Id;
	}

	// Token: 0x06006762 RID: 26466 RVA: 0x00291183 File Offset: 0x0028F383
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06006763 RID: 26467 RVA: 0x00291194 File Offset: 0x0028F394
	public override void OnOpen()
	{
		base.OnOpen();
		this.isClosingByAction = false;
		this.windowGroup.isEscClosable = false;
		this.xui.playerUI.CursorController.Locked = false;
		this.xui.playerUI.CursorController.SetCursorHidden(false);
	}

	// Token: 0x06006764 RID: 26468 RVA: 0x002911E8 File Offset: 0x0028F3E8
	public override void OnClose()
	{
		base.OnClose();
		if (!this.isClosingByAction && this.pressButtonOnExternalClose >= 0 && this.pressButtonOnExternalClose < this.buttons.Length)
		{
			XUiC_MessageBoxWindowGroup.MessageButton messageButton = this.buttons[this.pressButtonOnExternalClose];
			if (!messageButton.IsUsed || !messageButton.Enabled)
			{
				return;
			}
			messageButton.Execute();
		}
		if (!this.openMainMenuOnClose && this.windowGroupEscCloseableBefore)
		{
			GUIWindow modalWindow = this.xui.playerUI.windowManager.GetModalWindow();
			if (modalWindow != null)
			{
				modalWindow.isEscClosable = true;
			}
		}
		if (GameManager.Instance.World == null && this.openMainMenuOnClose)
		{
			ThreadManager.StartCoroutine(PlatformApplicationManager.CheckRestartCoroutine(false));
			this.xui.playerUI.windowManager.Open(XUiC_MainMenu.ID, true);
		}
	}

	// Token: 0x06006765 RID: 26469 RVA: 0x002912AC File Offset: 0x0028F4AC
	[XuiBindEvent("OnPress", "fullScreenCollider")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void FullScreenCollider_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.pressButtonOnOutsideClick < 0 || this.pressButtonOnOutsideClick >= this.buttons.Length)
		{
			return;
		}
		XUiC_MessageBoxWindowGroup.MessageButton messageButton = this.buttons[this.pressButtonOnOutsideClick];
		if (!messageButton.IsUsed || !messageButton.Enabled)
		{
			return;
		}
		messageButton.Execute();
	}

	// Token: 0x06006766 RID: 26470 RVA: 0x002912F8 File Offset: 0x0028F4F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void showMessage(string _title, string _text, string _icon = "", bool _openMainMenuOnClose = true, bool _modal = true, int _buttonOnOutsideClick = -1, int _buttonOnExternalClose = -1)
	{
		this.Text = _text;
		this.Title = _title;
		this.Icon = _icon ?? "";
		this.pressButtonOnOutsideClick = _buttonOnOutsideClick;
		this.pressButtonOnExternalClose = _buttonOnExternalClose;
		this.openMainMenuOnClose = _openMainMenuOnClose;
		GUIWindow modalWindow = this.xui.playerUI.windowManager.GetModalWindow();
		if (modalWindow != null)
		{
			this.windowGroupEscCloseableBefore = modalWindow.isEscClosable;
			modalWindow.isEscClosable = false;
		}
		if (this.windowGroup.isShowing)
		{
			this.OnOpen();
			return;
		}
		this.xui.playerUI.windowManager.Open(base.WindowGroup, _modal);
	}

	// Token: 0x06006767 RID: 26471 RVA: 0x00291398 File Offset: 0x0028F598
	[PublicizedFrom(EAccessModifier.Private)]
	public void openPage()
	{
		this.urlBoxData.Item1 = null;
		Func<string, bool> item = this.urlBoxData.Item3;
		if (item == null)
		{
			return;
		}
		item(this.urlBoxData.Item2);
	}

	// Token: 0x06006768 RID: 26472 RVA: 0x002913C7 File Offset: 0x0028F5C7
	[PublicizedFrom(EAccessModifier.Private)]
	public void cancelOpenPage()
	{
		this.urlBoxData.Item1 = null;
	}

	// Token: 0x06006769 RID: 26473 RVA: 0x002913D8 File Offset: 0x0028F5D8
	public static void ShowCustom(XUi _xuiInstance, string _title, string _text, string _icon, Action<XUiC_MessageBoxWindowGroup> _setupCallback, bool _openMainMenuOnClose = true, bool _modal = true, int _buttonOnOutsideClick = -1, int _buttonOnExternalClose = -1)
	{
		XUiC_MessageBoxWindowGroup xuiC_MessageBoxWindowGroup = (XUiC_MessageBoxWindowGroup)_xuiInstance.FindWindowGroupByName(XUiC_MessageBoxWindowGroup.id);
		for (int i = 0; i < xuiC_MessageBoxWindowGroup.buttons.Length; i++)
		{
			xuiC_MessageBoxWindowGroup.buttons[i].Clear();
		}
		if (_setupCallback != null)
		{
			_setupCallback(xuiC_MessageBoxWindowGroup);
		}
		xuiC_MessageBoxWindowGroup.showMessage(_title, _text, _icon, _openMainMenuOnClose, _modal, _buttonOnOutsideClick, _buttonOnExternalClose);
	}

	// Token: 0x0600676A RID: 26474 RVA: 0x00291434 File Offset: 0x0028F634
	public static void ShowOk(XUi _xuiInstance, string _title, string _text, string _icon = "", Action _onOk = null, bool _openMainMenuOnClose = true, bool _modal = true, bool _confirmOnOutsideClick = false)
	{
		XUiC_MessageBoxWindowGroup xuiC_MessageBoxWindowGroup = (XUiC_MessageBoxWindowGroup)_xuiInstance.FindWindowGroupByName(XUiC_MessageBoxWindowGroup.id);
		for (int i = 0; i < xuiC_MessageBoxWindowGroup.buttons.Length; i++)
		{
			xuiC_MessageBoxWindowGroup.buttons[i].Clear();
		}
		xuiC_MessageBoxWindowGroup.buttons[0].DefaultConfirm("xuiOk", _onOk, true, 0f, 0f, false);
		xuiC_MessageBoxWindowGroup.showMessage(_title, _text, _icon, _openMainMenuOnClose, _modal, _confirmOnOutsideClick ? 0 : (-1), 0);
	}

	// Token: 0x0600676B RID: 26475 RVA: 0x002914A8 File Offset: 0x0028F6A8
	public static void ShowOkCancel(XUi _xuiInstance, string _title, string _text, string _icon = "", Action _onOk = null, Action _onCancel = null, bool _openMainMenuOnClose = true, bool _modal = true, bool _cancelOnOutsideClick = false)
	{
		XUiC_MessageBoxWindowGroup xuiC_MessageBoxWindowGroup = (XUiC_MessageBoxWindowGroup)_xuiInstance.FindWindowGroupByName(XUiC_MessageBoxWindowGroup.id);
		for (int i = 0; i < xuiC_MessageBoxWindowGroup.buttons.Length; i++)
		{
			xuiC_MessageBoxWindowGroup.buttons[i].Clear();
		}
		xuiC_MessageBoxWindowGroup.buttons[0].DefaultConfirm("xuiOk", _onOk, true, 0f, 0f, false);
		xuiC_MessageBoxWindowGroup.buttons[2].DefaultCancel("xuiCancel", _onCancel, true, 0f, 0f, false);
		xuiC_MessageBoxWindowGroup.showMessage(_title, _text, _icon, _openMainMenuOnClose, _modal, _cancelOnOutsideClick ? 2 : (-1), 2);
	}

	// Token: 0x0600676C RID: 26476 RVA: 0x0029153C File Offset: 0x0028F73C
	public static void ShowConfirmCancel(XUi _xuiInstance, string _title, string _text, string _icon = "", Action _onConfirm = null, Action _onCancel = null, float _confirmHoldTime = 0f, bool _openMainMenuOnClose = true, bool _modal = true, bool _cancelOnOutsideClick = false)
	{
		XUiC_MessageBoxWindowGroup xuiC_MessageBoxWindowGroup = (XUiC_MessageBoxWindowGroup)_xuiInstance.FindWindowGroupByName(XUiC_MessageBoxWindowGroup.id);
		for (int i = 0; i < xuiC_MessageBoxWindowGroup.buttons.Length; i++)
		{
			xuiC_MessageBoxWindowGroup.buttons[i].Clear();
		}
		xuiC_MessageBoxWindowGroup.buttons[0].DefaultConfirm("btnConfirm", _onConfirm, true, 0f, _confirmHoldTime, false);
		xuiC_MessageBoxWindowGroup.buttons[2].DefaultCancel("xuiCancel", _onCancel, true, 0f, 0f, false);
		xuiC_MessageBoxWindowGroup.showMessage(_title, _text, _icon, _openMainMenuOnClose, _modal, _cancelOnOutsideClick ? 2 : (-1), 2);
	}

	// Token: 0x0600676D RID: 26477 RVA: 0x002915D0 File Offset: 0x0028F7D0
	public static void ShowUrlConfirmationDialog(XUi _xuiInstance, string _url, bool _modal = false, Func<string, bool> _browserOpenMethod = null, string _title = null, string _text = null, string _icon = "", string _displayUrl = null, bool _cancelOnOutsideClick = false)
	{
		XUiC_MessageBoxWindowGroup xuiC_MessageBoxWindowGroup = (XUiC_MessageBoxWindowGroup)_xuiInstance.FindWindowGroupByName(XUiC_MessageBoxWindowGroup.id);
		if (string.IsNullOrEmpty(_url))
		{
			return;
		}
		if (!Utils.IsValidWebUrl(ref _url))
		{
			return;
		}
		if (_displayUrl == null)
		{
			_displayUrl = _url;
		}
		xuiC_MessageBoxWindowGroup.urlBoxData.Item2 = _url;
		xuiC_MessageBoxWindowGroup.urlBoxData.Item3 = _browserOpenMethod ?? ((PlatformManager.NativePlatform.Utils != null) ? new Func<string, bool>(PlatformManager.NativePlatform.Utils.OpenBrowser) : null);
		for (int i = 0; i < xuiC_MessageBoxWindowGroup.buttons.Length; i++)
		{
			xuiC_MessageBoxWindowGroup.buttons[i].Clear();
		}
		xuiC_MessageBoxWindowGroup.buttons[0].DefaultConfirm("lblContextActionOpen", new Action(xuiC_MessageBoxWindowGroup.openPage), true, 0f, 0f, false);
		xuiC_MessageBoxWindowGroup.buttons[2].DefaultCancel("xuiCancel", new Action(xuiC_MessageBoxWindowGroup.cancelOpenPage), true, 0f, 0f, false);
		if (_title == null)
		{
			_title = Localization.Get("xuiOpenUrlConfirmationTitle", false, null);
		}
		if (_text == null)
		{
			_text = Localization.Get("xuiOpenUrlConfirmationText", false, null);
		}
		_text = string.Format(_text, _displayUrl);
		bool flag = false;
		if (xuiC_MessageBoxWindowGroup.windowGroup.isShowing)
		{
			_modal = xuiC_MessageBoxWindowGroup.windowGroup.isModal;
			flag = xuiC_MessageBoxWindowGroup.openMainMenuOnClose;
		}
		xuiC_MessageBoxWindowGroup.showMessage(_title, _text, _icon, flag, _modal, _cancelOnOutsideClick ? 2 : (-1), 2);
	}

	// Token: 0x0600676E RID: 26478 RVA: 0x00291728 File Offset: 0x0028F928
	public static void ShowNetworkError(XUi _xuiInstance, NetworkConnectionError _error, bool _confirmOnOutsideClick = false)
	{
		string text = _error.ToStringCached<NetworkConnectionError>();
		switch (_error)
		{
		case NetworkConnectionError.InternalDirectConnectFailed:
		case NetworkConnectionError.EmptyConnectTarget:
		case NetworkConnectionError.IncorrectParameters:
		case NetworkConnectionError.AlreadyConnectedToAnotherServer:
			break;
		case NetworkConnectionError.CreateSocketOrThreadFailure:
			text = string.Format(Localization.Get("mmLblErrorSocketFailure", false, null), SingletonMonoBehaviour<ConnectionManager>.Instance.GetRequiredPortsString());
			goto IL_00D1;
		default:
			switch (_error)
			{
			case NetworkConnectionError.ConnectionFailed:
			case NetworkConnectionError.AlreadyConnectedToServer:
			case (NetworkConnectionError)17:
			case NetworkConnectionError.TooManyConnectedPlayers:
			case (NetworkConnectionError)19:
			case (NetworkConnectionError)20:
			case NetworkConnectionError.RSAPublicKeyMismatch:
			case NetworkConnectionError.ConnectionBanned:
				break;
			case NetworkConnectionError.InvalidPassword:
				text = Localization.Get("mmLblErrorWrongPassword", false, null);
				goto IL_00D1;
			default:
				switch (_error)
				{
				case NetworkConnectionError.RestartRequired:
					text = string.Format(Localization.Get("app_restartRequired", false, null), Array.Empty<object>());
					goto IL_00D1;
				}
				break;
			}
			break;
		}
		text = string.Format(Localization.Get("mmLblErrorUnknown", false, null), text);
		IL_00D1:
		XUiC_MessageBoxWindowGroup.ShowOk(_xuiInstance, Localization.Get("mmLblErrorServerInit", false, null), text, "", null, true, true, _confirmOnOutsideClick);
	}

	// Token: 0x0600676F RID: 26479 RVA: 0x00291822 File Offset: 0x0028FA22
	public static void Close(XUi _xuiInstance)
	{
		_xuiInstance.playerUI.windowManager.Close(XUiC_MessageBoxWindowGroup.id);
	}

	// Token: 0x06006770 RID: 26480 RVA: 0x00291839 File Offset: 0x0028FA39
	public static bool IsShowing(XUi _xuiInstance)
	{
		return _xuiInstance.playerUI.windowManager.IsWindowOpen(XUiC_MessageBoxWindowGroup.id);
	}

	// Token: 0x04004FDA RID: 20442
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id = "";

	// Token: 0x04004FDE RID: 20446
	[XuiBindComponent("buttons.", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_MessageBoxWindowGroup.MessageButton[] buttons;

	// Token: 0x04004FDF RID: 20447
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_FullScreenCollider fullScreenCollider;

	// Token: 0x04004FE0 RID: 20448
	[PublicizedFrom(EAccessModifier.Private)]
	public int pressButtonOnOutsideClick = -1;

	// Token: 0x04004FE1 RID: 20449
	[PublicizedFrom(EAccessModifier.Private)]
	public int pressButtonOnExternalClose = -1;

	// Token: 0x04004FE2 RID: 20450
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isClosingByAction;

	// Token: 0x04004FE3 RID: 20451
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openMainMenuOnClose;

	// Token: 0x04004FE4 RID: 20452
	[PublicizedFrom(EAccessModifier.Private)]
	public bool windowGroupEscCloseableBefore;

	// Token: 0x04004FE5 RID: 20453
	[TupleElementNames(new string[] { "prevModalWindow", "url", "browserOpenMethod" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<GUIWindow, string, Func<string, bool>> urlBoxData;

	// Token: 0x02000D57 RID: 3415
	[global::UnityEngine.Scripting.Preserve]
	public class MessageButton : XUiController
	{
		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x06006773 RID: 26483 RVA: 0x00291893 File Offset: 0x0028FA93
		[XuiXmlBinding("msg_btn_used")]
		public bool IsUsed
		{
			get
			{
				return !string.IsNullOrEmpty(this.Caption);
			}
		}

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x06006774 RID: 26484 RVA: 0x002918A3 File Offset: 0x0028FAA3
		// (set) Token: 0x06006775 RID: 26485 RVA: 0x002918B4 File Offset: 0x0028FAB4
		[XuiXmlBinding("msg_btn_caption_raw")]
		public string Caption
		{
			get
			{
				return this.caption ?? "";
			}
			set
			{
				if (this.caption == value)
				{
					return;
				}
				this.caption = value;
				this.IsDirty = true;
			}
		}

		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06006776 RID: 26486 RVA: 0x002918D3 File Offset: 0x0028FAD3
		[XuiXmlBinding("msg_btn_caption")]
		public string CaptionWithSuffix
		{
			get
			{
				return this.Caption + (this.HoldToConfirm ? (" (" + Localization.Get("xuiHoldButton", false, null) + ")") : string.Empty);
			}
		}

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x06006777 RID: 26487 RVA: 0x0029190A File Offset: 0x0028FB0A
		// (set) Token: 0x06006778 RID: 26488 RVA: 0x00291912 File Offset: 0x0028FB12
		public PlayerAction Hotkey
		{
			get
			{
				return this.hotkey;
			}
			set
			{
				if (this.hotkey == value)
				{
					return;
				}
				this.hotkey = value;
				this.IsDirty = true;
			}
		}

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x06006779 RID: 26489 RVA: 0x0029192C File Offset: 0x0028FB2C
		[XuiXmlBinding("msg_btn_action")]
		public string HotkeyAction
		{
			get
			{
				PlayerAction playerAction = this.hotkey;
				return ((playerAction != null) ? playerAction.Name : null) ?? "";
			}
		}

		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x0600677A RID: 26490 RVA: 0x00291949 File Offset: 0x0028FB49
		[XuiXmlBinding("msg_btn_hashotkey")]
		public bool HasHotkey
		{
			get
			{
				return this.hotkey != null;
			}
		}

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x0600677B RID: 26491 RVA: 0x00291954 File Offset: 0x0028FB54
		// (set) Token: 0x0600677C RID: 26492 RVA: 0x0029195C File Offset: 0x0028FB5C
		public Action Callback
		{
			get
			{
				return this.callback;
			}
			set
			{
				this.callback = value;
			}
		}

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x0600677D RID: 26493 RVA: 0x00291965 File Offset: 0x0028FB65
		// (set) Token: 0x0600677E RID: 26494 RVA: 0x0029196D File Offset: 0x0028FB6D
		[XuiXmlBinding("msg_btn_enabled")]
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				if (this.enabled == value)
				{
					return;
				}
				this.enabled = value;
				this.IsDirty = true;
			}
		}

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x0600677F RID: 26495 RVA: 0x00291987 File Offset: 0x0028FB87
		// (set) Token: 0x06006780 RID: 26496 RVA: 0x0029198F File Offset: 0x0028FB8F
		[XuiXmlBinding("msg_btn_enableddelay")]
		public float EnabledDelay
		{
			get
			{
				return this.enabledDelay;
			}
			set
			{
				if (Mathf.Approximately(this.enabledDelay, value))
				{
					return;
				}
				this.enabledDelay = value;
				this.IsDirty = true;
			}
		}

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x06006781 RID: 26497 RVA: 0x002919AE File Offset: 0x0028FBAE
		// (set) Token: 0x06006782 RID: 26498 RVA: 0x002919B6 File Offset: 0x0028FBB6
		[XuiXmlBinding("msg_btn_hold_time")]
		public float HoldTime
		{
			get
			{
				return this.holdTime;
			}
			set
			{
				if (Mathf.Approximately(this.holdTime, value))
				{
					return;
				}
				this.holdTime = value;
				this.IsDirty = true;
			}
		}

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x06006783 RID: 26499 RVA: 0x002919D5 File Offset: 0x0028FBD5
		public bool HoldToConfirm
		{
			get
			{
				return this.HoldTime > 0f;
			}
		}

		// Token: 0x06006784 RID: 26500 RVA: 0x002919E4 File Offset: 0x0028FBE4
		public void Clear()
		{
			this.Caption = "";
			this.Hotkey = null;
			this.Callback = null;
			this.Enabled = false;
			this.EnabledDelay = 0f;
			this.HoldTime = 0f;
		}

		// Token: 0x06006785 RID: 26501 RVA: 0x00291A1C File Offset: 0x0028FC1C
		public void DefaultConfirm(string _captionKey, Action _callback, bool _enabled = true, float _enabledDelay = 0f, float _holdTime = 0f, bool _holdTimeOnKbdMouse = false)
		{
			this.Set(_captionKey, this.xui.playerUI.playerInput.GUIActions.Submit, _callback, _enabled, _enabledDelay, _holdTime, _holdTimeOnKbdMouse);
		}

		// Token: 0x06006786 RID: 26502 RVA: 0x00291A47 File Offset: 0x0028FC47
		public void DefaultCancel(string _captionKey, Action _callback, bool _enabled = true, float _enabledDelay = 0f, float _holdTime = 0f, bool _holdTimeOnKbdMouse = false)
		{
			this.Set(_captionKey, this.xui.playerUI.playerInput.GUIActions.Cancel, _callback, _enabled, _enabledDelay, _holdTime, _holdTimeOnKbdMouse);
		}

		// Token: 0x06006787 RID: 26503 RVA: 0x00291A74 File Offset: 0x0028FC74
		public void Set(string _captionKey, PlayerAction _hotkey, Action _callback, bool _enabled = true, float _enabledDelay = 0f, float _holdTime = 0f, bool _holdTimeOnKbdMouse = false)
		{
			this.Caption = Localization.Get(_captionKey, false, null);
			this.Hotkey = _hotkey;
			this.Callback = _callback;
			this.Enabled = _enabled;
			this.EnabledDelay = _enabledDelay;
			if (!_holdTimeOnKbdMouse && PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard)
			{
				this.HoldTime = 0f;
				return;
			}
			this.HoldTime = _holdTime;
		}

		// Token: 0x06006788 RID: 26504 RVA: 0x00291AD7 File Offset: 0x0028FCD7
		[XuiBindEvent("OnPress", "button")]
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnButtonPressed(XUiController _sender, int _mouseButton)
		{
			if (this.wasPressedThisFrame)
			{
				return;
			}
			this.wasPressedThisFrame = true;
			this.Execute();
		}

		// Token: 0x06006789 RID: 26505 RVA: 0x00291AF0 File Offset: 0x0028FCF0
		public override void Update(float _dt)
		{
			base.Update(_dt);
			if (this.EnabledDelay > 0f)
			{
				this.EnabledDelay -= _dt;
				if (this.EnabledDelay <= 0f)
				{
					this.Enabled = true;
				}
			}
			base.handleDirtyUpdateDefault();
			if (this.hotkey == null || !this.Enabled)
			{
				return;
			}
			if (this.hotkey.WasPressed)
			{
				this.button.MouseUpDown(true);
			}
			if (this.hotkey.WasReleased)
			{
				this.button.MouseUpDown(false);
				this.button.Pressed(-1);
			}
			this.wasPressedThisFrame = false;
		}

		// Token: 0x0600678A RID: 26506 RVA: 0x00291B90 File Offset: 0x0028FD90
		public void Execute()
		{
			ThreadManager.StartCoroutine(this.executeCallbackLater(this.Callback));
		}

		// Token: 0x0600678B RID: 26507 RVA: 0x00291BA4 File Offset: 0x0028FDA4
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator executeCallbackLater(Action _callback)
		{
			yield return null;
			this.owner.isClosingByAction = true;
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
			if (_callback != null)
			{
				_callback();
			}
			yield break;
		}

		// Token: 0x04004FE6 RID: 20454
		[XuiBindParent(true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_MessageBoxWindowGroup owner;

		// Token: 0x04004FE7 RID: 20455
		[XuiBindComponent(true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_Button button;

		// Token: 0x04004FE8 RID: 20456
		[PublicizedFrom(EAccessModifier.Private)]
		public string caption;

		// Token: 0x04004FE9 RID: 20457
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerAction hotkey;

		// Token: 0x04004FEA RID: 20458
		[PublicizedFrom(EAccessModifier.Private)]
		public Action callback;

		// Token: 0x04004FEB RID: 20459
		[PublicizedFrom(EAccessModifier.Private)]
		public bool enabled;

		// Token: 0x04004FEC RID: 20460
		[PublicizedFrom(EAccessModifier.Private)]
		public float enabledDelay;

		// Token: 0x04004FED RID: 20461
		[PublicizedFrom(EAccessModifier.Private)]
		public float holdTime;

		// Token: 0x04004FEE RID: 20462
		[PublicizedFrom(EAccessModifier.Private)]
		public bool wasPressedThisFrame;
	}
}
