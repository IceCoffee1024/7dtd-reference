using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DD9 RID: 3545
[Preserve]
public class XUiC_ServerBrowserDirectConnect : XUiController
{
	// Token: 0x17000BF8 RID: 3064
	// (get) Token: 0x06006BFD RID: 27645 RVA: 0x002A55A3 File Offset: 0x002A37A3
	// (set) Token: 0x06006BFE RID: 27646 RVA: 0x002A55BB File Offset: 0x002A37BB
	public string Ip
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_TextInput xuiC_TextInput = this.txtIp;
			if (xuiC_TextInput == null)
			{
				return null;
			}
			return xuiC_TextInput.Text.Trim();
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.txtIp.Text = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000BF9 RID: 3065
	// (get) Token: 0x06006BFF RID: 27647 RVA: 0x002A55D0 File Offset: 0x002A37D0
	// (set) Token: 0x06006C00 RID: 27648 RVA: 0x002A55F4 File Offset: 0x002A37F4
	public int Port
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			int num;
			if (int.TryParse(this.txtPort.Text, out num))
			{
				return num;
			}
			return -1;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.txtPort.Text = value.ToString();
			this.IsDirty = true;
		}
	}

	// Token: 0x17000BFA RID: 3066
	// (get) Token: 0x06006C01 RID: 27649 RVA: 0x002A560F File Offset: 0x002A380F
	// (set) Token: 0x06006C02 RID: 27650 RVA: 0x002A5617 File Offset: 0x002A3817
	[XuiXmlBinding("validdata")]
	public bool ValidData
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.validData;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.validData = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x06006C03 RID: 27651 RVA: 0x002A5627 File Offset: 0x002A3827
	public override void Init()
	{
		base.Init();
		this.txtIp.SelectOnTab = this.txtPort;
		this.txtPort.SelectOnTab = this.txtIp;
	}

	// Token: 0x06006C04 RID: 27652 RVA: 0x002A5654 File Offset: 0x002A3854
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent, false))
		{
			if (this.xui.playerUI.playerInput.PermanentActions.Cancel.WasReleased)
			{
				this.BtnCancel_OnPressed(this, -1);
			}
			if (this.xui.playerUI.playerInput.GUIActions.Apply.WasReleased)
			{
				this.BtnConnect_OnPressed(this, -1);
			}
		}
	}

	// Token: 0x06006C05 RID: 27653 RVA: 0x002A56D0 File Offset: 0x002A38D0
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.txtIp != null)
		{
			this.Ip = GamePrefs.GetString(EnumGamePrefs.ConnectToServerIP);
			this.Port = GamePrefs.GetInt(EnumGamePrefs.ConnectToServerPort);
			this.txtIp.SetSelected(true, false);
			this.validateIpPort(null, null, true);
		}
		XUiC_Button xuiC_Button = this.btnCancel;
		if (xuiC_Button == null)
		{
			return;
		}
		xuiC_Button.SelectCursorElement(true, false);
	}

	// Token: 0x06006C06 RID: 27654 RVA: 0x002A572E File Offset: 0x002A392E
	public override void OnClose()
	{
		base.OnClose();
		this.saveIpPort();
		XUiC_MultiplayerPrivilegeNotification.Close();
		this.cancelled = null;
		this.confirmed = null;
	}

	// Token: 0x06006C07 RID: 27655 RVA: 0x002A5750 File Offset: 0x002A3950
	[XuiBindEvent("OnClipboardHandler", "txtIp")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtIp_OnClipboardHandler(UIInput.ClipboardAction _actionType, string _oldText, int _selStart, int _selEnd, string _actionResultText)
	{
		if (_actionType != UIInput.ClipboardAction.Paste)
		{
			return;
		}
		if (_selEnd - _selStart != _oldText.Length)
		{
			return;
		}
		Match match = XUiC_ServerBrowserDirectConnect.ipPortMatcher.Match(_actionResultText);
		if (!match.Success)
		{
			return;
		}
		string value = match.Groups[1].Value;
		int num;
		if (!StringParsers.TryParseSInt32(match.Groups[2].Value, out num))
		{
			return;
		}
		this.Ip = value;
		this.Port = num;
	}

	// Token: 0x06006C08 RID: 27656 RVA: 0x002A57C0 File Offset: 0x002A39C0
	[XuiBindEvent("OnSubmitHandler", "txtIp")]
	[XuiBindEvent("OnSubmitHandler", "txtPort")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void Txt_OnSubmitHandler(XUiController _sender, string _text)
	{
		this.BtnConnect_OnPressed(_sender, -1);
	}

	// Token: 0x06006C09 RID: 27657 RVA: 0x002A57CC File Offset: 0x002A39CC
	[XuiBindEvent("OnPress", "btnConnect")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnConnect_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (!this.btnConnect.ViewComponent.Enabled)
		{
			return;
		}
		Action action = this.confirmed;
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		if (action != null)
		{
			action();
		}
		XUiC_MultiplayerPrivilegeNotification.ResolvePrivilegesWithDialog(EUserPerms.Multiplayer, delegate(bool _result)
		{
			if (!_result)
			{
				return;
			}
			string text = this.Ip;
			long num;
			if (!long.TryParse(text.Replace(".", ""), out num))
			{
				try
				{
					IPHostEntry hostEntry = Dns.GetHostEntry(text);
					if (hostEntry.AddressList.Length == 0)
					{
						Log.Out("No valid IP for server found");
						return;
					}
					text = hostEntry.AddressList[0].ToString();
					if (hostEntry.AddressList[0].AddressFamily != AddressFamily.InterNetwork)
					{
						for (int i = 1; i < hostEntry.AddressList.Length; i++)
						{
							if (hostEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
							{
								text = hostEntry.AddressList[i].ToString();
								break;
							}
						}
					}
				}
				catch (SocketException ex)
				{
					string text2 = "No such hostname: \"";
					string text3 = text;
					string text4 = "\": ";
					SocketException ex2 = ex;
					Log.Out(text2 + text3 + text4 + ((ex2 != null) ? ex2.ToString() : null));
					return;
				}
			}
			Log.Out("Connect by IP");
			GameServerInfo gameServerInfo = new GameServerInfo();
			gameServerInfo.SetValue(GameInfoString.IP, text);
			gameServerInfo.SetValue(GameInfoInt.Port, this.Port);
			GameManager.Instance.showOpenerMovieOnLoad = false;
			SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo = gameServerInfo;
			SingletonMonoBehaviour<ConnectionManager>.Instance.LastJoinSource = "Direct Connection";
			SingletonMonoBehaviour<ConnectionManager>.Instance.Connect(SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo);
		}, -1f, false, null);
	}

	// Token: 0x06006C0A RID: 27658 RVA: 0x002A5833 File Offset: 0x002A3A33
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		Action action = this.cancelled;
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06006C0B RID: 27659 RVA: 0x002A5864 File Offset: 0x002A3A64
	[XuiBindEvent("OnChangeHandler", "txtIp")]
	[XuiBindEvent("OnChangeHandler", "txtPort")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void validateIpPort(XUiController _sender, string _newText, bool _changeFromCode)
	{
		int port = this.Port;
		this.ValidData = port > 0 && port < 65533 && this.Ip.Length > 0;
	}

	// Token: 0x06006C0C RID: 27660 RVA: 0x002A589B File Offset: 0x002A3A9B
	[PublicizedFrom(EAccessModifier.Private)]
	public void saveIpPort()
	{
		if (!this.ValidData)
		{
			return;
		}
		GamePrefs.Set(EnumGamePrefs.ConnectToServerIP, this.Ip);
		GamePrefs.Set(EnumGamePrefs.ConnectToServerPort, this.Port);
	}

	// Token: 0x06006C0D RID: 27661 RVA: 0x002A58C0 File Offset: 0x002A3AC0
	public static void Open(XUi _xui, Action _onConfirmed, Action _onCancelled)
	{
		XUiC_ServerBrowserDirectConnect childByType = _xui.GetChildByType<XUiC_ServerBrowserDirectConnect>();
		childByType.confirmed = _onConfirmed;
		childByType.cancelled = _onCancelled;
		_xui.playerUI.windowManager.Open(childByType.windowGroup, false);
	}

	// Token: 0x040052A8 RID: 21160
	[XuiBindComponent("txtIp", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtIp;

	// Token: 0x040052A9 RID: 21161
	[XuiBindComponent("txtPort", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtPort;

	// Token: 0x040052AA RID: 21162
	[XuiBindComponent("btnConnect", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConnect;

	// Token: 0x040052AB RID: 21163
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x040052AC RID: 21164
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex ipPortMatcher = new Regex("^(.*):(\\d+)$");

	// Token: 0x040052AD RID: 21165
	[PublicizedFrom(EAccessModifier.Private)]
	public Action confirmed;

	// Token: 0x040052AE RID: 21166
	[PublicizedFrom(EAccessModifier.Private)]
	public Action cancelled;

	// Token: 0x040052AF RID: 21167
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validData;
}
