using System;
using System.Collections;
using System.Runtime.CompilerServices;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D34 RID: 3380
[global::UnityEngine.Scripting.Preserve]
public class XUiC_MessageBoxWindowGroup : XUiController
{
	// Token: 0x17000AD6 RID: 2774
	// (get) Token: 0x060066D4 RID: 26324 RVA: 0x0028ACE4 File Offset: 0x00288EE4
	// (set) Token: 0x060066D5 RID: 26325 RVA: 0x0028ACEC File Offset: 0x00288EEC
	[XuiXmlBinding("msgTitle")]
	public string Title
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000AD7 RID: 2775
	// (get) Token: 0x060066D6 RID: 26326 RVA: 0x0028ACF5 File Offset: 0x00288EF5
	// (set) Token: 0x060066D7 RID: 26327 RVA: 0x0028ACFD File Offset: 0x00288EFD
	[XuiXmlBinding("msgText")]
	public string Text
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000AD8 RID: 2776
	// (get) Token: 0x060066D8 RID: 26328 RVA: 0x0028AD06 File Offset: 0x00288F06
	// (set) Token: 0x060066D9 RID: 26329 RVA: 0x0028AD0E File Offset: 0x00288F0E
	[XuiXmlBinding("msgIcon")]
	public string Icon
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000AD9 RID: 2777
	// (get) Token: 0x060066DA RID: 26330 RVA: 0x0028AD17 File Offset: 0x00288F17
	public XUiC_MessageBoxWindowGroup.MessageButton[] Buttons
	{
		get
		{
			return this.buttons;
		}
	}

	// Token: 0x060066DB RID: 26331 RVA: 0x0028AD1F File Offset: 0x00288F1F
	public override void Init()
	{
		base.Init();
		XUiC_MessageBoxWindowGroup.id = base.WindowGroup.Id;
	}

	// Token: 0x060066DC RID: 26332 RVA: 0x0028AD37 File Offset: 0x00288F37
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x060066DD RID: 26333 RVA: 0x0028AD48 File Offset: 0x00288F48
	public override void OnOpen()
	{
		base.OnOpen();
		this.isClosingByAction = false;
		this.windowGroup.isEscClosable = false;
		this.xui.playerUI.CursorController.Locked = false;
		this.xui.playerUI.CursorController.SetCursorHidden(false);
	}

	// Token: 0x060066DE RID: 26334 RVA: 0x0028AD9C File Offset: 0x00288F9C
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

	// Token: 0x060066DF RID: 26335 RVA: 0x0028AE60 File Offset: 0x00289060
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

	// Token: 0x060066E0 RID: 26336 RVA: 0x0028AEAC File Offset: 0x002890AC
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

	// Token: 0x060066E1 RID: 26337 RVA: 0x0028AF4C File Offset: 0x0028914C
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

	// Token: 0x060066E2 RID: 26338 RVA: 0x0028AF7B File Offset: 0x0028917B
	[PublicizedFrom(EAccessModifier.Private)]
	public void cancelOpenPage()
	{
		this.urlBoxData.Item1 = null;
	}

	// Token: 0x060066E3 RID: 26339 RVA: 0x0028AF8C File Offset: 0x0028918C
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

	// Token: 0x060066E4 RID: 26340 RVA: 0x0028AFE8 File Offset: 0x002891E8
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

	// Token: 0x060066E5 RID: 26341 RVA: 0x0028B05C File Offset: 0x0028925C
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

	// Token: 0x060066E6 RID: 26342 RVA: 0x0028B0F0 File Offset: 0x002892F0
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

	// Token: 0x060066E7 RID: 26343 RVA: 0x0028B184 File Offset: 0x00289384
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

	// Token: 0x060066E8 RID: 26344 RVA: 0x0028B2DC File Offset: 0x002894DC
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

	// Token: 0x060066E9 RID: 26345 RVA: 0x0028B3D6 File Offset: 0x002895D6
	public static void Close(XUi _xuiInstance)
	{
		_xuiInstance.playerUI.windowManager.Close(XUiC_MessageBoxWindowGroup.id);
	}

	// Token: 0x060066EA RID: 26346 RVA: 0x0028B3ED File Offset: 0x002895ED
	public static bool IsShowing(XUi _xuiInstance)
	{
		return _xuiInstance.playerUI.windowManager.IsWindowOpen(XUiC_MessageBoxWindowGroup.id);
	}

	// Token: 0x04004EFD RID: 20221
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id = "";

	// Token: 0x04004F01 RID: 20225
	[XuiBindComponent("buttons.", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_MessageBoxWindowGroup.MessageButton[] buttons;

	// Token: 0x04004F02 RID: 20226
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_FullScreenCollider fullScreenCollider;

	// Token: 0x04004F03 RID: 20227
	[PublicizedFrom(EAccessModifier.Private)]
	public int pressButtonOnOutsideClick = -1;

	// Token: 0x04004F04 RID: 20228
	[PublicizedFrom(EAccessModifier.Private)]
	public int pressButtonOnExternalClose = -1;

	// Token: 0x04004F05 RID: 20229
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isClosingByAction;

	// Token: 0x04004F06 RID: 20230
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openMainMenuOnClose;

	// Token: 0x04004F07 RID: 20231
	[PublicizedFrom(EAccessModifier.Private)]
	public bool windowGroupEscCloseableBefore;

	// Token: 0x04004F08 RID: 20232
	[TupleElementNames(new string[] { "prevModalWindow", "url", "browserOpenMethod" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<GUIWindow, string, Func<string, bool>> urlBoxData;

	// Token: 0x02000D35 RID: 3381
	[global::UnityEngine.Scripting.Preserve]
	public class MessageButton : XUiController
	{
		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x060066ED RID: 26349 RVA: 0x0028B447 File Offset: 0x00289647
		[XuiXmlBinding("msg_btn_used")]
		public bool IsUsed
		{
			get
			{
				return !string.IsNullOrEmpty(this.Caption);
			}
		}

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x060066EE RID: 26350 RVA: 0x0028B457 File Offset: 0x00289657
		// (set) Token: 0x060066EF RID: 26351 RVA: 0x0028B468 File Offset: 0x00289668
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

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x060066F0 RID: 26352 RVA: 0x0028B487 File Offset: 0x00289687
		[XuiXmlBinding("msg_btn_caption")]
		public string CaptionWithSuffix
		{
			get
			{
				return this.Caption + (this.HoldToConfirm ? (" (" + Localization.Get("xuiHoldButton", false, null) + ")") : string.Empty);
			}
		}

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x060066F1 RID: 26353 RVA: 0x0028B4BE File Offset: 0x002896BE
		// (set) Token: 0x060066F2 RID: 26354 RVA: 0x0028B4C6 File Offset: 0x002896C6
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

		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x060066F3 RID: 26355 RVA: 0x0028B4E0 File Offset: 0x002896E0
		[XuiXmlBinding("msg_btn_action")]
		public string HotkeyAction
		{
			get
			{
				PlayerAction playerAction = this.hotkey;
				return ((playerAction != null) ? playerAction.Name : null) ?? "";
			}
		}

		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x060066F4 RID: 26356 RVA: 0x0028B4FD File Offset: 0x002896FD
		[XuiXmlBinding("msg_btn_hashotkey")]
		public bool HasHotkey
		{
			get
			{
				return this.hotkey != null;
			}
		}

		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x060066F5 RID: 26357 RVA: 0x0028B508 File Offset: 0x00289708
		// (set) Token: 0x060066F6 RID: 26358 RVA: 0x0028B510 File Offset: 0x00289710
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

		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x060066F7 RID: 26359 RVA: 0x0028B519 File Offset: 0x00289719
		// (set) Token: 0x060066F8 RID: 26360 RVA: 0x0028B521 File Offset: 0x00289721
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

		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x060066F9 RID: 26361 RVA: 0x0028B53B File Offset: 0x0028973B
		// (set) Token: 0x060066FA RID: 26362 RVA: 0x0028B543 File Offset: 0x00289743
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

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x060066FB RID: 26363 RVA: 0x0028B562 File Offset: 0x00289762
		// (set) Token: 0x060066FC RID: 26364 RVA: 0x0028B56A File Offset: 0x0028976A
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

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x060066FD RID: 26365 RVA: 0x0028B589 File Offset: 0x00289789
		public bool HoldToConfirm
		{
			get
			{
				return this.HoldTime > 0f;
			}
		}

		// Token: 0x060066FE RID: 26366 RVA: 0x0028B598 File Offset: 0x00289798
		public void Clear()
		{
			this.Caption = "";
			this.Hotkey = null;
			this.Callback = null;
			this.Enabled = false;
			this.EnabledDelay = 0f;
			this.HoldTime = 0f;
		}

		// Token: 0x060066FF RID: 26367 RVA: 0x0028B5D0 File Offset: 0x002897D0
		public void DefaultConfirm(string _captionKey, Action _callback, bool _enabled = true, float _enabledDelay = 0f, float _holdTime = 0f, bool _holdTimeOnKbdMouse = false)
		{
			this.Set(_captionKey, this.xui.playerUI.playerInput.GUIActions.Submit, _callback, _enabled, _enabledDelay, _holdTime, _holdTimeOnKbdMouse);
		}

		// Token: 0x06006700 RID: 26368 RVA: 0x0028B5FB File Offset: 0x002897FB
		public void DefaultCancel(string _captionKey, Action _callback, bool _enabled = true, float _enabledDelay = 0f, float _holdTime = 0f, bool _holdTimeOnKbdMouse = false)
		{
			this.Set(_captionKey, this.xui.playerUI.playerInput.GUIActions.Cancel, _callback, _enabled, _enabledDelay, _holdTime, _holdTimeOnKbdMouse);
		}

		// Token: 0x06006701 RID: 26369 RVA: 0x0028B628 File Offset: 0x00289828
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

		// Token: 0x06006702 RID: 26370 RVA: 0x0028B68B File Offset: 0x0028988B
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

		// Token: 0x06006703 RID: 26371 RVA: 0x0028B6A4 File Offset: 0x002898A4
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

		// Token: 0x06006704 RID: 26372 RVA: 0x0028B744 File Offset: 0x00289944
		public void Execute()
		{
			ThreadManager.StartCoroutine(this.executeCallbackLater(this.Callback));
		}

		// Token: 0x06006705 RID: 26373 RVA: 0x0028B758 File Offset: 0x00289958
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

		// Token: 0x04004F09 RID: 20233
		[XuiBindParent(true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_MessageBoxWindowGroup owner;

		// Token: 0x04004F0A RID: 20234
		[XuiBindComponent(true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_Button button;

		// Token: 0x04004F0B RID: 20235
		[PublicizedFrom(EAccessModifier.Private)]
		public string caption;

		// Token: 0x04004F0C RID: 20236
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerAction hotkey;

		// Token: 0x04004F0D RID: 20237
		[PublicizedFrom(EAccessModifier.Private)]
		public Action callback;

		// Token: 0x04004F0E RID: 20238
		[PublicizedFrom(EAccessModifier.Private)]
		public bool enabled;

		// Token: 0x04004F0F RID: 20239
		[PublicizedFrom(EAccessModifier.Private)]
		public float enabledDelay;

		// Token: 0x04004F10 RID: 20240
		[PublicizedFrom(EAccessModifier.Private)]
		public float holdTime;

		// Token: 0x04004F11 RID: 20241
		[PublicizedFrom(EAccessModifier.Private)]
		public bool wasPressedThisFrame;
	}
}
