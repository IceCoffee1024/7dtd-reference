using System;
using System.Collections;
using System.Runtime.CompilerServices;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D61 RID: 3425
[global::UnityEngine.Scripting.Preserve]
public class XUiC_MessageBoxWindowGroup : XUiController
{
	// Token: 0x17000B07 RID: 2823
	// (get) Token: 0x0600677F RID: 26495 RVA: 0x00292658 File Offset: 0x00290858
	// (set) Token: 0x06006780 RID: 26496 RVA: 0x00292660 File Offset: 0x00290860
	[XuiXmlBinding("msgTitle")]
	public string Title
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000B08 RID: 2824
	// (get) Token: 0x06006781 RID: 26497 RVA: 0x00292669 File Offset: 0x00290869
	// (set) Token: 0x06006782 RID: 26498 RVA: 0x00292671 File Offset: 0x00290871
	[XuiXmlBinding("msgText")]
	public string Text
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000B09 RID: 2825
	// (get) Token: 0x06006783 RID: 26499 RVA: 0x0029267A File Offset: 0x0029087A
	// (set) Token: 0x06006784 RID: 26500 RVA: 0x00292682 File Offset: 0x00290882
	[XuiXmlBinding("msgIcon")]
	public string Icon
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000B0A RID: 2826
	// (get) Token: 0x06006785 RID: 26501 RVA: 0x0029268B File Offset: 0x0029088B
	public XUiC_MessageBoxWindowGroup.MessageButton[] Buttons
	{
		get
		{
			return this.buttons;
		}
	}

	// Token: 0x06006786 RID: 26502 RVA: 0x00292693 File Offset: 0x00290893
	public override void Init()
	{
		base.Init();
		XUiC_MessageBoxWindowGroup.id = base.WindowGroup.Id;
	}

	// Token: 0x06006787 RID: 26503 RVA: 0x002926AB File Offset: 0x002908AB
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06006788 RID: 26504 RVA: 0x002926BC File Offset: 0x002908BC
	public override void OnOpen()
	{
		base.OnOpen();
		this.isClosingByAction = false;
		this.windowGroup.isEscClosable = false;
		this.xui.playerUI.CursorController.Locked = false;
		this.xui.playerUI.CursorController.SetCursorHidden(false);
	}

	// Token: 0x06006789 RID: 26505 RVA: 0x00292710 File Offset: 0x00290910
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

	// Token: 0x0600678A RID: 26506 RVA: 0x002927D4 File Offset: 0x002909D4
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

	// Token: 0x0600678B RID: 26507 RVA: 0x00292820 File Offset: 0x00290A20
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

	// Token: 0x0600678C RID: 26508 RVA: 0x002928C0 File Offset: 0x00290AC0
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

	// Token: 0x0600678D RID: 26509 RVA: 0x002928EF File Offset: 0x00290AEF
	[PublicizedFrom(EAccessModifier.Private)]
	public void cancelOpenPage()
	{
		this.urlBoxData.Item1 = null;
	}

	// Token: 0x0600678E RID: 26510 RVA: 0x00292900 File Offset: 0x00290B00
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

	// Token: 0x0600678F RID: 26511 RVA: 0x0029295C File Offset: 0x00290B5C
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

	// Token: 0x06006790 RID: 26512 RVA: 0x002929D0 File Offset: 0x00290BD0
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

	// Token: 0x06006791 RID: 26513 RVA: 0x00292A64 File Offset: 0x00290C64
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

	// Token: 0x06006792 RID: 26514 RVA: 0x00292AF8 File Offset: 0x00290CF8
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

	// Token: 0x06006793 RID: 26515 RVA: 0x00292C50 File Offset: 0x00290E50
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

	// Token: 0x06006794 RID: 26516 RVA: 0x00292D4A File Offset: 0x00290F4A
	public static void Close(XUi _xuiInstance)
	{
		_xuiInstance.playerUI.windowManager.Close(XUiC_MessageBoxWindowGroup.id);
	}

	// Token: 0x06006795 RID: 26517 RVA: 0x00292D61 File Offset: 0x00290F61
	public static bool IsShowing(XUi _xuiInstance)
	{
		return _xuiInstance.playerUI.windowManager.IsWindowOpen(XUiC_MessageBoxWindowGroup.id);
	}

	// Token: 0x04004FFF RID: 20479
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id = "";

	// Token: 0x04005003 RID: 20483
	[XuiBindComponent("buttons.", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_MessageBoxWindowGroup.MessageButton[] buttons;

	// Token: 0x04005004 RID: 20484
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_FullScreenCollider fullScreenCollider;

	// Token: 0x04005005 RID: 20485
	[PublicizedFrom(EAccessModifier.Private)]
	public int pressButtonOnOutsideClick = -1;

	// Token: 0x04005006 RID: 20486
	[PublicizedFrom(EAccessModifier.Private)]
	public int pressButtonOnExternalClose = -1;

	// Token: 0x04005007 RID: 20487
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isClosingByAction;

	// Token: 0x04005008 RID: 20488
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openMainMenuOnClose;

	// Token: 0x04005009 RID: 20489
	[PublicizedFrom(EAccessModifier.Private)]
	public bool windowGroupEscCloseableBefore;

	// Token: 0x0400500A RID: 20490
	[TupleElementNames(new string[] { "prevModalWindow", "url", "browserOpenMethod" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<GUIWindow, string, Func<string, bool>> urlBoxData;

	// Token: 0x02000D62 RID: 3426
	[global::UnityEngine.Scripting.Preserve]
	public class MessageButton : XUiController
	{
		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x06006798 RID: 26520 RVA: 0x00292DBB File Offset: 0x00290FBB
		[XuiXmlBinding("msg_btn_used")]
		public bool IsUsed
		{
			get
			{
				return !string.IsNullOrEmpty(this.Caption);
			}
		}

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x06006799 RID: 26521 RVA: 0x00292DCB File Offset: 0x00290FCB
		// (set) Token: 0x0600679A RID: 26522 RVA: 0x00292DDC File Offset: 0x00290FDC
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

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x0600679B RID: 26523 RVA: 0x00292DFB File Offset: 0x00290FFB
		[XuiXmlBinding("msg_btn_caption")]
		public string CaptionWithSuffix
		{
			get
			{
				return this.Caption + (this.HoldToConfirm ? (" (" + Localization.Get("xuiHoldButton", false, null) + ")") : string.Empty);
			}
		}

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x0600679C RID: 26524 RVA: 0x00292E32 File Offset: 0x00291032
		// (set) Token: 0x0600679D RID: 26525 RVA: 0x00292E3A File Offset: 0x0029103A
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

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x0600679E RID: 26526 RVA: 0x00292E54 File Offset: 0x00291054
		[XuiXmlBinding("msg_btn_action")]
		public string HotkeyAction
		{
			get
			{
				PlayerAction playerAction = this.hotkey;
				return ((playerAction != null) ? playerAction.Name : null) ?? "";
			}
		}

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x0600679F RID: 26527 RVA: 0x00292E71 File Offset: 0x00291071
		[XuiXmlBinding("msg_btn_hashotkey")]
		public bool HasHotkey
		{
			get
			{
				return this.hotkey != null;
			}
		}

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x060067A0 RID: 26528 RVA: 0x00292E7C File Offset: 0x0029107C
		// (set) Token: 0x060067A1 RID: 26529 RVA: 0x00292E84 File Offset: 0x00291084
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

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x060067A2 RID: 26530 RVA: 0x00292E8D File Offset: 0x0029108D
		// (set) Token: 0x060067A3 RID: 26531 RVA: 0x00292E95 File Offset: 0x00291095
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

		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x060067A4 RID: 26532 RVA: 0x00292EAF File Offset: 0x002910AF
		// (set) Token: 0x060067A5 RID: 26533 RVA: 0x00292EB7 File Offset: 0x002910B7
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

		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x060067A6 RID: 26534 RVA: 0x00292ED6 File Offset: 0x002910D6
		// (set) Token: 0x060067A7 RID: 26535 RVA: 0x00292EDE File Offset: 0x002910DE
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

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x060067A8 RID: 26536 RVA: 0x00292EFD File Offset: 0x002910FD
		public bool HoldToConfirm
		{
			get
			{
				return this.HoldTime > 0f;
			}
		}

		// Token: 0x060067A9 RID: 26537 RVA: 0x00292F0C File Offset: 0x0029110C
		public void Clear()
		{
			this.Caption = "";
			this.Hotkey = null;
			this.Callback = null;
			this.Enabled = false;
			this.EnabledDelay = 0f;
			this.HoldTime = 0f;
		}

		// Token: 0x060067AA RID: 26538 RVA: 0x00292F44 File Offset: 0x00291144
		public void DefaultConfirm(string _captionKey, Action _callback, bool _enabled = true, float _enabledDelay = 0f, float _holdTime = 0f, bool _holdTimeOnKbdMouse = false)
		{
			this.Set(_captionKey, this.xui.playerUI.playerInput.GUIActions.Submit, _callback, _enabled, _enabledDelay, _holdTime, _holdTimeOnKbdMouse);
		}

		// Token: 0x060067AB RID: 26539 RVA: 0x00292F6F File Offset: 0x0029116F
		public void DefaultCancel(string _captionKey, Action _callback, bool _enabled = true, float _enabledDelay = 0f, float _holdTime = 0f, bool _holdTimeOnKbdMouse = false)
		{
			this.Set(_captionKey, this.xui.playerUI.playerInput.GUIActions.Cancel, _callback, _enabled, _enabledDelay, _holdTime, _holdTimeOnKbdMouse);
		}

		// Token: 0x060067AC RID: 26540 RVA: 0x00292F9C File Offset: 0x0029119C
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

		// Token: 0x060067AD RID: 26541 RVA: 0x00292FFF File Offset: 0x002911FF
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

		// Token: 0x060067AE RID: 26542 RVA: 0x00293018 File Offset: 0x00291218
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

		// Token: 0x060067AF RID: 26543 RVA: 0x002930B8 File Offset: 0x002912B8
		public void Execute()
		{
			ThreadManager.StartCoroutine(this.executeCallbackLater(this.Callback));
		}

		// Token: 0x060067B0 RID: 26544 RVA: 0x002930CC File Offset: 0x002912CC
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

		// Token: 0x0400500B RID: 20491
		[XuiBindParent(true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_MessageBoxWindowGroup owner;

		// Token: 0x0400500C RID: 20492
		[XuiBindComponent(true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_Button button;

		// Token: 0x0400500D RID: 20493
		[PublicizedFrom(EAccessModifier.Private)]
		public string caption;

		// Token: 0x0400500E RID: 20494
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerAction hotkey;

		// Token: 0x0400500F RID: 20495
		[PublicizedFrom(EAccessModifier.Private)]
		public Action callback;

		// Token: 0x04005010 RID: 20496
		[PublicizedFrom(EAccessModifier.Private)]
		public bool enabled;

		// Token: 0x04005011 RID: 20497
		[PublicizedFrom(EAccessModifier.Private)]
		public float enabledDelay;

		// Token: 0x04005012 RID: 20498
		[PublicizedFrom(EAccessModifier.Private)]
		public float holdTime;

		// Token: 0x04005013 RID: 20499
		[PublicizedFrom(EAccessModifier.Private)]
		public bool wasPressedThisFrame;
	}
}
