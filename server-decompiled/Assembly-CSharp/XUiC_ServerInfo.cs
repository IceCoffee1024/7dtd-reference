using System;
using System.Collections.Generic;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x02000DFD RID: 3581
[Preserve]
public class XUiC_ServerInfo : XUiController
{
	// Token: 0x17000C22 RID: 3106
	// (get) Token: 0x06006CE8 RID: 27880 RVA: 0x002AA4E0 File Offset: 0x002A86E0
	// (set) Token: 0x06006CE9 RID: 27881 RVA: 0x002AA4F1 File Offset: 0x002A86F1
	[XuiXmlBinding("serverurl")]
	public string ServerUrl
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.serverUrl ?? "";
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.serverUrl = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000C23 RID: 3107
	// (get) Token: 0x06006CEA RID: 27882 RVA: 0x002AA501 File Offset: 0x002A8701
	// (set) Token: 0x06006CEB RID: 27883 RVA: 0x002AA509 File Offset: 0x002A8709
	[XuiXmlBinding("serverurlhovered")]
	public bool ServerUrlHovered
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.serverUrlHovered;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.serverUrlHovered = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000C24 RID: 3108
	// (get) Token: 0x06006CEC RID: 27884 RVA: 0x002AA519 File Offset: 0x002A8719
	// (set) Token: 0x06006CED RID: 27885 RVA: 0x002AA52A File Offset: 0x002A872A
	[XuiXmlBinding("serverdescription")]
	public string ServerDescription
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.serverDescription ?? "";
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.serverDescription = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000C25 RID: 3109
	// (get) Token: 0x06006CEE RID: 27886 RVA: 0x002AA53A File Offset: 0x002A873A
	// (set) Token: 0x06006CEF RID: 27887 RVA: 0x002AA54B File Offset: 0x002A874B
	[XuiXmlBinding("serversandboxcode")]
	public string ServerSandboxCode
	{
		get
		{
			return this.serverSandboxCode ?? "";
		}
		set
		{
			this.serverSandboxCode = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000C26 RID: 3110
	// (get) Token: 0x06006CF0 RID: 27888 RVA: 0x002AA55C File Offset: 0x002A875C
	[XuiXmlBinding("sandboxpreset")]
	public string SandboxPreset
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			SandboxOptionPreset presetByCode = SandboxOptionManager.Current.GetPresetByCode(this.ServerSandboxCode);
			if (presetByCode == null)
			{
				return Localization.Get("sandboxPresetGroupUser", false, null);
			}
			if (presetByCode != null && !presetByCode.IsUserPreset && !presetByCode.IsCustomPreset && !presetByCode.IsModded)
			{
				return presetByCode.DisplayName;
			}
			return Localization.Get("sandboxPresetGroupUser", false, null);
		}
	}

	// Token: 0x17000C27 RID: 3111
	// (get) Token: 0x06006CF1 RID: 27889 RVA: 0x002AA5B8 File Offset: 0x002A87B8
	[XuiXmlBinding("showip")]
	public bool ShowIp
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.displayMode == XUiC_ServerInfo.DisplayMode.Dedicated;
		}
	}

	// Token: 0x06006CF2 RID: 27890 RVA: 0x002AA5C4 File Offset: 0x002A87C4
	public override void Init()
	{
		base.Init();
		foreach (XUiC_ServerBrowserGamePrefInfo xuiC_ServerBrowserGamePrefInfo in base.GetChildrenByType<XUiC_ServerBrowserGamePrefInfo>(null))
		{
			this.infoFields.Add(xuiC_ServerBrowserGamePrefInfo);
			if (xuiC_ServerBrowserGamePrefInfo.ValueType == GamePrefs.EnumType.Int)
			{
				GameInfoInt gameInfoInt = xuiC_ServerBrowserGamePrefInfo.GameInfoInt;
				if (gameInfoInt != GameInfoInt.CurrentServerTime)
				{
					if (gameInfoInt == GameInfoInt.AirDropFrequency)
					{
						xuiC_ServerBrowserGamePrefInfo.CustomIntValueFormatter = (GameServerInfo _, int _value) => string.Format(Localization.Get("goAirDropValue" + ((_value == 1) ? "" : "s"), false, null), _value);
					}
				}
				else
				{
					xuiC_ServerBrowserGamePrefInfo.CustomIntValueFormatter = (GameServerInfo _, int _worldTime) => ValueDisplayFormatters.WorldTime((ulong)((long)_worldTime), Localization.Get("xuiDayTimeLong", false, null));
				}
			}
			else if (xuiC_ServerBrowserGamePrefInfo.ValueType == GamePrefs.EnumType.String && xuiC_ServerBrowserGamePrefInfo.GameInfoString == GameInfoString.ServerVersion)
			{
				xuiC_ServerBrowserGamePrefInfo.CustomStringValueFormatter = delegate(GameServerInfo _info, string _s)
				{
					if (_info.Version.Build != 0)
					{
						return (_info.IsCompatibleVersion ? "" : "[ff0000]") + _info.Version.LongString;
					}
					return "[ff0000]" + _s;
				};
			}
		}
	}

	// Token: 0x06006CF3 RID: 27891 RVA: 0x002926AB File Offset: 0x002908AB
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06006CF4 RID: 27892 RVA: 0x002AA6A7 File Offset: 0x002A88A7
	public void InitializeForListFilter(XUiC_ServersList.EnumServerLists _mode)
	{
		if (_mode == XUiC_ServersList.EnumServerLists.Peer || _mode == XUiC_ServersList.EnumServerLists.Friends || _mode == XUiC_ServersList.EnumServerLists.History)
		{
			this.setDisplayMode(XUiC_ServerInfo.DisplayMode.Peer);
			return;
		}
		this.setDisplayMode(XUiC_ServerInfo.DisplayMode.Dedicated);
	}

	// Token: 0x06006CF5 RID: 27893 RVA: 0x002AA6C4 File Offset: 0x002A88C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void setDisplayMode(XUiC_ServerInfo.DisplayMode _mode)
	{
		if (this.displayMode != _mode)
		{
			this.displayMode = _mode;
			this.IsDirty = true;
		}
	}

	// Token: 0x06006CF6 RID: 27894 RVA: 0x002AA6E0 File Offset: 0x002A88E0
	public void SetServerInfo(GameServerInfo _gameInfo)
	{
		foreach (XUiC_ServerBrowserGamePrefInfo xuiC_ServerBrowserGamePrefInfo in this.infoFields)
		{
			xuiC_ServerBrowserGamePrefInfo.SetCurrentValue(_gameInfo);
		}
		this.ServerDescription = "";
		this.ServerUrl = "";
		this.unfilteredUrlText = "";
		string text = ((_gameInfo == null) ? "" : GeneratedTextManager.GetDisplayTextImmediately(_gameInfo.ServerDisplayName, false, GeneratedTextManager.TextFilteringMode.Filter, GeneratedTextManager.BbCodeSupportMode.NotSupported));
		GameStats.Set(EnumGameStats.SandboxPreset, text);
		this.ServerSandboxCode = ((_gameInfo == null) ? "" : _gameInfo.GetValue(GameInfoString.SandboxCode));
		if (_gameInfo == null)
		{
			return;
		}
		AuthoredText serverDisplayName = _gameInfo.ServerDescription;
		if (_gameInfo.IsDedicated || _gameInfo.IsLAN)
		{
			this.setDisplayMode(XUiC_ServerInfo.DisplayMode.Dedicated);
		}
		else
		{
			this.setDisplayMode(XUiC_ServerInfo.DisplayMode.Peer);
			if (string.IsNullOrEmpty(serverDisplayName.Text))
			{
				serverDisplayName = _gameInfo.ServerDisplayName;
			}
		}
		this.unfilteredUrlText = _gameInfo.ServerURL.Text;
		GeneratedTextManager.GetDisplayText(serverDisplayName, delegate(string _desc)
		{
			this.ServerDescription = _desc;
		}, true, false, GeneratedTextManager.TextFilteringMode.FilterWithSafeString, GeneratedTextManager.BbCodeSupportMode.Supported);
		GeneratedTextManager.GetDisplayText(_gameInfo.ServerURL, delegate(string _url)
		{
			this.ServerUrl = _url;
		}, true, false, GeneratedTextManager.TextFilteringMode.FilterWithSafeString, GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes);
	}

	// Token: 0x06006CF7 RID: 27895 RVA: 0x002AA80C File Offset: 0x002A8A0C
	[XuiBindEvent("OnHover", "lblServerUrl")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void UrlController_OnHover(XUiController _sender, bool _isOver)
	{
		this.ServerUrlHovered = _isOver;
	}

	// Token: 0x06006CF8 RID: 27896 RVA: 0x002AA818 File Offset: 0x002A8A18
	[XuiBindEvent("OnPress", "lblServerUrl")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void UrlController_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_MessageBoxWindowGroup.ShowUrlConfirmationDialog(this.xui, this.unfilteredUrlText, false, null, null, null, "", this.ServerUrl, false);
	}

	// Token: 0x04005354 RID: 21332
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_ServerBrowserGamePrefInfo> infoFields = new List<XUiC_ServerBrowserGamePrefInfo>();

	// Token: 0x04005355 RID: 21333
	[XuiBindComponent("ServerWebsiteURL", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController lblServerUrl;

	// Token: 0x04005356 RID: 21334
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerInfo.DisplayMode displayMode;

	// Token: 0x04005357 RID: 21335
	[PublicizedFrom(EAccessModifier.Private)]
	public string unfilteredUrlText;

	// Token: 0x04005358 RID: 21336
	[PublicizedFrom(EAccessModifier.Private)]
	public string serverUrl;

	// Token: 0x04005359 RID: 21337
	[PublicizedFrom(EAccessModifier.Private)]
	public string serverDescription;

	// Token: 0x0400535A RID: 21338
	[PublicizedFrom(EAccessModifier.Private)]
	public bool serverUrlHovered;

	// Token: 0x0400535B RID: 21339
	[PublicizedFrom(EAccessModifier.Private)]
	public string serverSandboxCode;

	// Token: 0x02000DFE RID: 3582
	[PublicizedFrom(EAccessModifier.Private)]
	public enum DisplayMode
	{
		// Token: 0x0400535D RID: 21341
		Dedicated,
		// Token: 0x0400535E RID: 21342
		Peer
	}
}
