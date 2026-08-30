using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DB6 RID: 3510
[Preserve]
public class XUiC_ServerBrowserDirectConnect : XUiController
{
	// Token: 0x17000BC5 RID: 3013
	// (get) Token: 0x06006B6A RID: 27498 RVA: 0x0029F0A3 File Offset: 0x0029D2A3
	// (set) Token: 0x06006B6B RID: 27499 RVA: 0x0029F0BB File Offset: 0x0029D2BB
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

	// Token: 0x17000BC6 RID: 3014
	// (get) Token: 0x06006B6C RID: 27500 RVA: 0x0029F0D0 File Offset: 0x0029D2D0
	// (set) Token: 0x06006B6D RID: 27501 RVA: 0x0029F0F4 File Offset: 0x0029D2F4
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

	// Token: 0x17000BC7 RID: 3015
	// (get) Token: 0x06006B6E RID: 27502 RVA: 0x0029F10F File Offset: 0x0029D30F
	// (set) Token: 0x06006B6F RID: 27503 RVA: 0x0029F117 File Offset: 0x0029D317
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

	// Token: 0x06006B70 RID: 27504 RVA: 0x0029F127 File Offset: 0x0029D327
	public override void Init()
	{
		base.Init();
		this.txtIp.SelectOnTab = this.txtPort;
		this.txtPort.SelectOnTab = this.txtIp;
	}

	// Token: 0x06006B71 RID: 27505 RVA: 0x0029F154 File Offset: 0x0029D354
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

	// Token: 0x06006B72 RID: 27506 RVA: 0x0029F1D0 File Offset: 0x0029D3D0
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

	// Token: 0x06006B73 RID: 27507 RVA: 0x0029F22E File Offset: 0x0029D42E
	public override void OnClose()
	{
		base.OnClose();
		this.saveIpPort();
		XUiC_MultiplayerPrivilegeNotification.Close();
		this.cancelled = null;
		this.confirmed = null;
	}

	// Token: 0x06006B74 RID: 27508 RVA: 0x0029F250 File Offset: 0x0029D450
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

	// Token: 0x06006B75 RID: 27509 RVA: 0x0029F2C0 File Offset: 0x0029D4C0
	[XuiBindEvent("OnSubmitHandler", "txtIp")]
	[XuiBindEvent("OnSubmitHandler", "txtPort")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void Txt_OnSubmitHandler(XUiController _sender, string _text)
	{
		this.BtnConnect_OnPressed(_sender, -1);
	}

	// Token: 0x06006B76 RID: 27510 RVA: 0x0029F2CC File Offset: 0x0029D4CC
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
			SingletonMonoBehaviour<ConnectionManager>.Instance.Connect(SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo);
		}, -1f, false, null);
	}

	// Token: 0x06006B77 RID: 27511 RVA: 0x0029F333 File Offset: 0x0029D533
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

	// Token: 0x06006B78 RID: 27512 RVA: 0x0029F364 File Offset: 0x0029D564
	[XuiBindEvent("OnChangeHandler", "txtIp")]
	[XuiBindEvent("OnChangeHandler", "txtPort")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void validateIpPort(XUiController _sender, string _newText, bool _changeFromCode)
	{
		int port = this.Port;
		this.ValidData = port > 0 && port < 65533 && this.Ip.Length > 0;
	}

	// Token: 0x06006B79 RID: 27513 RVA: 0x0029F39B File Offset: 0x0029D59B
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

	// Token: 0x06006B7A RID: 27514 RVA: 0x0029F3C0 File Offset: 0x0029D5C0
	public static void Open(XUi _xui, Action _onConfirmed, Action _onCancelled)
	{
		XUiC_ServerBrowserDirectConnect childByType = _xui.GetChildByType<XUiC_ServerBrowserDirectConnect>();
		childByType.confirmed = _onConfirmed;
		childByType.cancelled = _onCancelled;
		_xui.playerUI.windowManager.Open(childByType.windowGroup, false);
	}

	// Token: 0x040051CF RID: 20943
	[XuiBindComponent("txtIp", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtIp;

	// Token: 0x040051D0 RID: 20944
	[XuiBindComponent("txtPort", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtPort;

	// Token: 0x040051D1 RID: 20945
	[XuiBindComponent("btnConnect", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConnect;

	// Token: 0x040051D2 RID: 20946
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x040051D3 RID: 20947
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex ipPortMatcher = new Regex("^(.*):(\\d+)$");

	// Token: 0x040051D4 RID: 20948
	[PublicizedFrom(EAccessModifier.Private)]
	public Action confirmed;

	// Token: 0x040051D5 RID: 20949
	[PublicizedFrom(EAccessModifier.Private)]
	public Action cancelled;

	// Token: 0x040051D6 RID: 20950
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validData;
}
