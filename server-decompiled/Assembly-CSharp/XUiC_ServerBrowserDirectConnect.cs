using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DE4 RID: 3556
[Preserve]
public class XUiC_ServerBrowserDirectConnect : XUiController
{
	// Token: 0x17000BFE RID: 3070
	// (get) Token: 0x06006C22 RID: 27682 RVA: 0x002A6ACB File Offset: 0x002A4CCB
	// (set) Token: 0x06006C23 RID: 27683 RVA: 0x002A6AE3 File Offset: 0x002A4CE3
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

	// Token: 0x17000BFF RID: 3071
	// (get) Token: 0x06006C24 RID: 27684 RVA: 0x002A6AF8 File Offset: 0x002A4CF8
	// (set) Token: 0x06006C25 RID: 27685 RVA: 0x002A6B1C File Offset: 0x002A4D1C
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

	// Token: 0x17000C00 RID: 3072
	// (get) Token: 0x06006C26 RID: 27686 RVA: 0x002A6B37 File Offset: 0x002A4D37
	// (set) Token: 0x06006C27 RID: 27687 RVA: 0x002A6B3F File Offset: 0x002A4D3F
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

	// Token: 0x06006C28 RID: 27688 RVA: 0x002A6B4F File Offset: 0x002A4D4F
	public override void Init()
	{
		base.Init();
		this.txtIp.SelectOnTab = this.txtPort;
		this.txtPort.SelectOnTab = this.txtIp;
	}

	// Token: 0x06006C29 RID: 27689 RVA: 0x002A6B7C File Offset: 0x002A4D7C
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

	// Token: 0x06006C2A RID: 27690 RVA: 0x002A6BF8 File Offset: 0x002A4DF8
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

	// Token: 0x06006C2B RID: 27691 RVA: 0x002A6C56 File Offset: 0x002A4E56
	public override void OnClose()
	{
		base.OnClose();
		this.saveIpPort();
		XUiC_MultiplayerPrivilegeNotification.Close();
		this.cancelled = null;
		this.confirmed = null;
	}

	// Token: 0x06006C2C RID: 27692 RVA: 0x002A6C78 File Offset: 0x002A4E78
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

	// Token: 0x06006C2D RID: 27693 RVA: 0x002A6CE8 File Offset: 0x002A4EE8
	[XuiBindEvent("OnSubmitHandler", "txtIp")]
	[XuiBindEvent("OnSubmitHandler", "txtPort")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void Txt_OnSubmitHandler(XUiController _sender, string _text)
	{
		this.BtnConnect_OnPressed(_sender, -1);
	}

	// Token: 0x06006C2E RID: 27694 RVA: 0x002A6CF4 File Offset: 0x002A4EF4
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

	// Token: 0x06006C2F RID: 27695 RVA: 0x002A6D5B File Offset: 0x002A4F5B
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

	// Token: 0x06006C30 RID: 27696 RVA: 0x002A6D8C File Offset: 0x002A4F8C
	[XuiBindEvent("OnChangeHandler", "txtIp")]
	[XuiBindEvent("OnChangeHandler", "txtPort")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void validateIpPort(XUiController _sender, string _newText, bool _changeFromCode)
	{
		int port = this.Port;
		this.ValidData = port > 0 && port < 65533 && this.Ip.Length > 0;
	}

	// Token: 0x06006C31 RID: 27697 RVA: 0x002A6DC3 File Offset: 0x002A4FC3
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

	// Token: 0x06006C32 RID: 27698 RVA: 0x002A6DE8 File Offset: 0x002A4FE8
	public static void Open(XUi _xui, Action _onConfirmed, Action _onCancelled)
	{
		XUiC_ServerBrowserDirectConnect childByType = _xui.GetChildByType<XUiC_ServerBrowserDirectConnect>();
		childByType.confirmed = _onConfirmed;
		childByType.cancelled = _onCancelled;
		_xui.playerUI.windowManager.Open(childByType.windowGroup, false);
	}

	// Token: 0x040052CD RID: 21197
	[XuiBindComponent("txtIp", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtIp;

	// Token: 0x040052CE RID: 21198
	[XuiBindComponent("txtPort", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtPort;

	// Token: 0x040052CF RID: 21199
	[XuiBindComponent("btnConnect", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConnect;

	// Token: 0x040052D0 RID: 21200
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x040052D1 RID: 21201
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex ipPortMatcher = new Regex("^(.*):(\\d+)$");

	// Token: 0x040052D2 RID: 21202
	[PublicizedFrom(EAccessModifier.Private)]
	public Action confirmed;

	// Token: 0x040052D3 RID: 21203
	[PublicizedFrom(EAccessModifier.Private)]
	public Action cancelled;

	// Token: 0x040052D4 RID: 21204
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validData;
}
