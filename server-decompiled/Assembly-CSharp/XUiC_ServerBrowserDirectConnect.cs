using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DCA RID: 3530
[Preserve]
public class XUiC_ServerBrowserDirectConnect : XUiController
{
	// Token: 0x17000BDD RID: 3037
	// (get) Token: 0x06006C05 RID: 27653 RVA: 0x002A2213 File Offset: 0x002A0413
	// (set) Token: 0x06006C06 RID: 27654 RVA: 0x002A222B File Offset: 0x002A042B
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

	// Token: 0x17000BDE RID: 3038
	// (get) Token: 0x06006C07 RID: 27655 RVA: 0x002A2240 File Offset: 0x002A0440
	// (set) Token: 0x06006C08 RID: 27656 RVA: 0x002A2264 File Offset: 0x002A0464
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

	// Token: 0x17000BDF RID: 3039
	// (get) Token: 0x06006C09 RID: 27657 RVA: 0x002A227F File Offset: 0x002A047F
	// (set) Token: 0x06006C0A RID: 27658 RVA: 0x002A2287 File Offset: 0x002A0487
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

	// Token: 0x06006C0B RID: 27659 RVA: 0x002A2297 File Offset: 0x002A0497
	public override void Init()
	{
		base.Init();
		this.txtIp.SelectOnTab = this.txtPort;
		this.txtPort.SelectOnTab = this.txtIp;
	}

	// Token: 0x06006C0C RID: 27660 RVA: 0x002A22C4 File Offset: 0x002A04C4
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

	// Token: 0x06006C0D RID: 27661 RVA: 0x002A2340 File Offset: 0x002A0540
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

	// Token: 0x06006C0E RID: 27662 RVA: 0x002A239E File Offset: 0x002A059E
	public override void OnClose()
	{
		base.OnClose();
		this.saveIpPort();
		XUiC_MultiplayerPrivilegeNotification.Close();
		this.cancelled = null;
		this.confirmed = null;
	}

	// Token: 0x06006C0F RID: 27663 RVA: 0x002A23C0 File Offset: 0x002A05C0
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

	// Token: 0x06006C10 RID: 27664 RVA: 0x002A2430 File Offset: 0x002A0630
	[XuiBindEvent("OnSubmitHandler", "txtIp")]
	[XuiBindEvent("OnSubmitHandler", "txtPort")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void Txt_OnSubmitHandler(XUiController _sender, string _text)
	{
		this.BtnConnect_OnPressed(_sender, -1);
	}

	// Token: 0x06006C11 RID: 27665 RVA: 0x002A243C File Offset: 0x002A063C
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

	// Token: 0x06006C12 RID: 27666 RVA: 0x002A24A3 File Offset: 0x002A06A3
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

	// Token: 0x06006C13 RID: 27667 RVA: 0x002A24D4 File Offset: 0x002A06D4
	[XuiBindEvent("OnChangeHandler", "txtIp")]
	[XuiBindEvent("OnChangeHandler", "txtPort")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void validateIpPort(XUiController _sender, string _newText, bool _changeFromCode)
	{
		int port = this.Port;
		this.ValidData = port > 0 && port < 65533 && this.Ip.Length > 0;
	}

	// Token: 0x06006C14 RID: 27668 RVA: 0x002A250B File Offset: 0x002A070B
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

	// Token: 0x06006C15 RID: 27669 RVA: 0x002A2530 File Offset: 0x002A0730
	public static void Open(XUi _xui, Action _onConfirmed, Action _onCancelled)
	{
		XUiC_ServerBrowserDirectConnect childByType = _xui.GetChildByType<XUiC_ServerBrowserDirectConnect>();
		childByType.confirmed = _onConfirmed;
		childByType.cancelled = _onCancelled;
		_xui.playerUI.windowManager.Open(childByType.windowGroup, false);
	}

	// Token: 0x04005271 RID: 21105
	[XuiBindComponent("txtIp", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtIp;

	// Token: 0x04005272 RID: 21106
	[XuiBindComponent("txtPort", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtPort;

	// Token: 0x04005273 RID: 21107
	[XuiBindComponent("btnConnect", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConnect;

	// Token: 0x04005274 RID: 21108
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x04005275 RID: 21109
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex ipPortMatcher = new Regex("^(.*):(\\d+)$");

	// Token: 0x04005276 RID: 21110
	[PublicizedFrom(EAccessModifier.Private)]
	public Action confirmed;

	// Token: 0x04005277 RID: 21111
	[PublicizedFrom(EAccessModifier.Private)]
	public Action cancelled;

	// Token: 0x04005278 RID: 21112
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validData;
}
