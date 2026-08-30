using System;
using System.Collections;
using System.Runtime.CompilerServices;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D46 RID: 3398
[global::UnityEngine.Scripting.Preserve]
public class XUiC_MessageBoxWindowGroup : XUiController
{
	// Token: 0x17000AE9 RID: 2793
	// (get) Token: 0x06006760 RID: 26464 RVA: 0x0028DBC8 File Offset: 0x0028BDC8
	// (set) Token: 0x06006761 RID: 26465 RVA: 0x0028DBD0 File Offset: 0x0028BDD0
	[XuiXmlBinding("msgTitle")]
	public string Title
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000AEA RID: 2794
	// (get) Token: 0x06006762 RID: 26466 RVA: 0x0028DBD9 File Offset: 0x0028BDD9
	// (set) Token: 0x06006763 RID: 26467 RVA: 0x0028DBE1 File Offset: 0x0028BDE1
	[XuiXmlBinding("msgText")]
	public string Text
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000AEB RID: 2795
	// (get) Token: 0x06006764 RID: 26468 RVA: 0x0028DBEA File Offset: 0x0028BDEA
	// (set) Token: 0x06006765 RID: 26469 RVA: 0x0028DBF2 File Offset: 0x0028BDF2
	[XuiXmlBinding("msgIcon")]
	public string Icon
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000AEC RID: 2796
	// (get) Token: 0x06006766 RID: 26470 RVA: 0x0028DBFB File Offset: 0x0028BDFB
	public XUiC_MessageBoxWindowGroup.MessageButton[] Buttons
	{
		get
		{
			return this.buttons;
		}
	}

	// Token: 0x06006767 RID: 26471 RVA: 0x0028DC03 File Offset: 0x0028BE03
	public override void Init()
	{
		base.Init();
		XUiC_MessageBoxWindowGroup.id = base.WindowGroup.Id;
	}

	// Token: 0x06006768 RID: 26472 RVA: 0x0028DC1B File Offset: 0x0028BE1B
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06006769 RID: 26473 RVA: 0x0028DC2C File Offset: 0x0028BE2C
	public override void OnOpen()
	{
		base.OnOpen();
		this.isClosingByAction = false;
		this.windowGroup.isEscClosable = false;
		this.xui.playerUI.CursorController.Locked = false;
		this.xui.playerUI.CursorController.SetCursorHidden(false);
	}

	// Token: 0x0600676A RID: 26474 RVA: 0x0028DC80 File Offset: 0x0028BE80
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

	// Token: 0x0600676B RID: 26475 RVA: 0x0028DD44 File Offset: 0x0028BF44
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

	// Token: 0x0600676C RID: 26476 RVA: 0x0028DD90 File Offset: 0x0028BF90
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

	// Token: 0x0600676D RID: 26477 RVA: 0x0028DE30 File Offset: 0x0028C030
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

	// Token: 0x0600676E RID: 26478 RVA: 0x0028DE5F File Offset: 0x0028C05F
	[PublicizedFrom(EAccessModifier.Private)]
	public void cancelOpenPage()
	{
		this.urlBoxData.Item1 = null;
	}

	// Token: 0x0600676F RID: 26479 RVA: 0x0028DE70 File Offset: 0x0028C070
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

	// Token: 0x06006770 RID: 26480 RVA: 0x0028DECC File Offset: 0x0028C0CC
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

	// Token: 0x06006771 RID: 26481 RVA: 0x0028DF40 File Offset: 0x0028C140
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

	// Token: 0x06006772 RID: 26482 RVA: 0x0028DFD4 File Offset: 0x0028C1D4
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

	// Token: 0x06006773 RID: 26483 RVA: 0x0028E068 File Offset: 0x0028C268
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

	// Token: 0x06006774 RID: 26484 RVA: 0x0028E1C0 File Offset: 0x0028C3C0
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

	// Token: 0x06006775 RID: 26485 RVA: 0x0028E2BA File Offset: 0x0028C4BA
	public static void Close(XUi _xuiInstance)
	{
		_xuiInstance.playerUI.windowManager.Close(XUiC_MessageBoxWindowGroup.id);
	}

	// Token: 0x06006776 RID: 26486 RVA: 0x0028E2D1 File Offset: 0x0028C4D1
	public static bool IsShowing(XUi _xuiInstance)
	{
		return _xuiInstance.playerUI.windowManager.IsWindowOpen(XUiC_MessageBoxWindowGroup.id);
	}

	// Token: 0x04004F9B RID: 20379
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id = "";

	// Token: 0x04004F9F RID: 20383
	[XuiBindComponent("buttons.", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_MessageBoxWindowGroup.MessageButton[] buttons;

	// Token: 0x04004FA0 RID: 20384
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_FullScreenCollider fullScreenCollider;

	// Token: 0x04004FA1 RID: 20385
	[PublicizedFrom(EAccessModifier.Private)]
	public int pressButtonOnOutsideClick = -1;

	// Token: 0x04004FA2 RID: 20386
	[PublicizedFrom(EAccessModifier.Private)]
	public int pressButtonOnExternalClose = -1;

	// Token: 0x04004FA3 RID: 20387
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isClosingByAction;

	// Token: 0x04004FA4 RID: 20388
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openMainMenuOnClose;

	// Token: 0x04004FA5 RID: 20389
	[PublicizedFrom(EAccessModifier.Private)]
	public bool windowGroupEscCloseableBefore;

	// Token: 0x04004FA6 RID: 20390
	[TupleElementNames(new string[] { "prevModalWindow", "url", "browserOpenMethod" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<GUIWindow, string, Func<string, bool>> urlBoxData;

	// Token: 0x02000D47 RID: 3399
	[global::UnityEngine.Scripting.Preserve]
	public class MessageButton : XUiController
	{
		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x06006779 RID: 26489 RVA: 0x0028E32B File Offset: 0x0028C52B
		[XuiXmlBinding("msg_btn_used")]
		public bool IsUsed
		{
			get
			{
				return !string.IsNullOrEmpty(this.Caption);
			}
		}

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x0600677A RID: 26490 RVA: 0x0028E33B File Offset: 0x0028C53B
		// (set) Token: 0x0600677B RID: 26491 RVA: 0x0028E34C File Offset: 0x0028C54C
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

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x0600677C RID: 26492 RVA: 0x0028E36B File Offset: 0x0028C56B
		[XuiXmlBinding("msg_btn_caption")]
		public string CaptionWithSuffix
		{
			get
			{
				return this.Caption + (this.HoldToConfirm ? (" (" + Localization.Get("xuiHoldButton", false, null) + ")") : string.Empty);
			}
		}

		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x0600677D RID: 26493 RVA: 0x0028E3A2 File Offset: 0x0028C5A2
		// (set) Token: 0x0600677E RID: 26494 RVA: 0x0028E3AA File Offset: 0x0028C5AA
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

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x0600677F RID: 26495 RVA: 0x0028E3C4 File Offset: 0x0028C5C4
		[XuiXmlBinding("msg_btn_action")]
		public string HotkeyAction
		{
			get
			{
				PlayerAction playerAction = this.hotkey;
				return ((playerAction != null) ? playerAction.Name : null) ?? "";
			}
		}

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06006780 RID: 26496 RVA: 0x0028E3E1 File Offset: 0x0028C5E1
		[XuiXmlBinding("msg_btn_hashotkey")]
		public bool HasHotkey
		{
			get
			{
				return this.hotkey != null;
			}
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06006781 RID: 26497 RVA: 0x0028E3EC File Offset: 0x0028C5EC
		// (set) Token: 0x06006782 RID: 26498 RVA: 0x0028E3F4 File Offset: 0x0028C5F4
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

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06006783 RID: 26499 RVA: 0x0028E3FD File Offset: 0x0028C5FD
		// (set) Token: 0x06006784 RID: 26500 RVA: 0x0028E405 File Offset: 0x0028C605
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

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06006785 RID: 26501 RVA: 0x0028E41F File Offset: 0x0028C61F
		// (set) Token: 0x06006786 RID: 26502 RVA: 0x0028E427 File Offset: 0x0028C627
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

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x06006787 RID: 26503 RVA: 0x0028E446 File Offset: 0x0028C646
		// (set) Token: 0x06006788 RID: 26504 RVA: 0x0028E44E File Offset: 0x0028C64E
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

		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x06006789 RID: 26505 RVA: 0x0028E46D File Offset: 0x0028C66D
		public bool HoldToConfirm
		{
			get
			{
				return this.HoldTime > 0f;
			}
		}

		// Token: 0x0600678A RID: 26506 RVA: 0x0028E47C File Offset: 0x0028C67C
		public void Clear()
		{
			this.Caption = "";
			this.Hotkey = null;
			this.Callback = null;
			this.Enabled = false;
			this.EnabledDelay = 0f;
			this.HoldTime = 0f;
		}

		// Token: 0x0600678B RID: 26507 RVA: 0x0028E4B4 File Offset: 0x0028C6B4
		public void DefaultConfirm(string _captionKey, Action _callback, bool _enabled = true, float _enabledDelay = 0f, float _holdTime = 0f, bool _holdTimeOnKbdMouse = false)
		{
			this.Set(_captionKey, this.xui.playerUI.playerInput.GUIActions.Submit, _callback, _enabled, _enabledDelay, _holdTime, _holdTimeOnKbdMouse);
		}

		// Token: 0x0600678C RID: 26508 RVA: 0x0028E4DF File Offset: 0x0028C6DF
		public void DefaultCancel(string _captionKey, Action _callback, bool _enabled = true, float _enabledDelay = 0f, float _holdTime = 0f, bool _holdTimeOnKbdMouse = false)
		{
			this.Set(_captionKey, this.xui.playerUI.playerInput.GUIActions.Cancel, _callback, _enabled, _enabledDelay, _holdTime, _holdTimeOnKbdMouse);
		}

		// Token: 0x0600678D RID: 26509 RVA: 0x0028E50C File Offset: 0x0028C70C
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

		// Token: 0x0600678E RID: 26510 RVA: 0x0028E56F File Offset: 0x0028C76F
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

		// Token: 0x0600678F RID: 26511 RVA: 0x0028E588 File Offset: 0x0028C788
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

		// Token: 0x06006790 RID: 26512 RVA: 0x0028E628 File Offset: 0x0028C828
		public void Execute()
		{
			ThreadManager.StartCoroutine(this.executeCallbackLater(this.Callback));
		}

		// Token: 0x06006791 RID: 26513 RVA: 0x0028E63C File Offset: 0x0028C83C
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

		// Token: 0x04004FA7 RID: 20391
		[XuiBindParent(true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_MessageBoxWindowGroup owner;

		// Token: 0x04004FA8 RID: 20392
		[XuiBindComponent(true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_Button button;

		// Token: 0x04004FA9 RID: 20393
		[PublicizedFrom(EAccessModifier.Private)]
		public string caption;

		// Token: 0x04004FAA RID: 20394
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerAction hotkey;

		// Token: 0x04004FAB RID: 20395
		[PublicizedFrom(EAccessModifier.Private)]
		public Action callback;

		// Token: 0x04004FAC RID: 20396
		[PublicizedFrom(EAccessModifier.Private)]
		public bool enabled;

		// Token: 0x04004FAD RID: 20397
		[PublicizedFrom(EAccessModifier.Private)]
		public float enabledDelay;

		// Token: 0x04004FAE RID: 20398
		[PublicizedFrom(EAccessModifier.Private)]
		public float holdTime;

		// Token: 0x04004FAF RID: 20399
		[PublicizedFrom(EAccessModifier.Private)]
		public bool wasPressedThisFrame;
	}
}
