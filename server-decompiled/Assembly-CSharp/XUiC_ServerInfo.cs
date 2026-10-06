using System;
using System.Collections.Generic;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x02000DF2 RID: 3570
[Preserve]
public class XUiC_ServerInfo : XUiController
{
	// Token: 0x17000C1C RID: 3100
	// (get) Token: 0x06006CC3 RID: 27843 RVA: 0x002A8FB8 File Offset: 0x002A71B8
	// (set) Token: 0x06006CC4 RID: 27844 RVA: 0x002A8FC9 File Offset: 0x002A71C9
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

	// Token: 0x17000C1D RID: 3101
	// (get) Token: 0x06006CC5 RID: 27845 RVA: 0x002A8FD9 File Offset: 0x002A71D9
	// (set) Token: 0x06006CC6 RID: 27846 RVA: 0x002A8FE1 File Offset: 0x002A71E1
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

	// Token: 0x17000C1E RID: 3102
	// (get) Token: 0x06006CC7 RID: 27847 RVA: 0x002A8FF1 File Offset: 0x002A71F1
	// (set) Token: 0x06006CC8 RID: 27848 RVA: 0x002A9002 File Offset: 0x002A7202
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

	// Token: 0x17000C1F RID: 3103
	// (get) Token: 0x06006CC9 RID: 27849 RVA: 0x002A9012 File Offset: 0x002A7212
	// (set) Token: 0x06006CCA RID: 27850 RVA: 0x002A9023 File Offset: 0x002A7223
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

	// Token: 0x17000C20 RID: 3104
	// (get) Token: 0x06006CCB RID: 27851 RVA: 0x002A9034 File Offset: 0x002A7234
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

	// Token: 0x17000C21 RID: 3105
	// (get) Token: 0x06006CCC RID: 27852 RVA: 0x002A9090 File Offset: 0x002A7290
	[XuiXmlBinding("showip")]
	public bool ShowIp
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.displayMode == XUiC_ServerInfo.DisplayMode.Dedicated;
		}
	}

	// Token: 0x06006CCD RID: 27853 RVA: 0x002A909C File Offset: 0x002A729C
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

	// Token: 0x06006CCE RID: 27854 RVA: 0x00291183 File Offset: 0x0028F383
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06006CCF RID: 27855 RVA: 0x002A917F File Offset: 0x002A737F
	public void InitializeForListFilter(XUiC_ServersList.EnumServerLists _mode)
	{
		if (_mode == XUiC_ServersList.EnumServerLists.Peer || _mode == XUiC_ServersList.EnumServerLists.Friends || _mode == XUiC_ServersList.EnumServerLists.History)
		{
			this.setDisplayMode(XUiC_ServerInfo.DisplayMode.Peer);
			return;
		}
		this.setDisplayMode(XUiC_ServerInfo.DisplayMode.Dedicated);
	}

	// Token: 0x06006CD0 RID: 27856 RVA: 0x002A919C File Offset: 0x002A739C
	[PublicizedFrom(EAccessModifier.Private)]
	public void setDisplayMode(XUiC_ServerInfo.DisplayMode _mode)
	{
		if (this.displayMode != _mode)
		{
			this.displayMode = _mode;
			this.IsDirty = true;
		}
	}

	// Token: 0x06006CD1 RID: 27857 RVA: 0x002A91B8 File Offset: 0x002A73B8
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

	// Token: 0x06006CD2 RID: 27858 RVA: 0x002A92E4 File Offset: 0x002A74E4
	[XuiBindEvent("OnHover", "lblServerUrl")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void UrlController_OnHover(XUiController _sender, bool _isOver)
	{
		this.ServerUrlHovered = _isOver;
	}

	// Token: 0x06006CD3 RID: 27859 RVA: 0x002A92F0 File Offset: 0x002A74F0
	[XuiBindEvent("OnPress", "lblServerUrl")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void UrlController_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_MessageBoxWindowGroup.ShowUrlConfirmationDialog(this.xui, this.unfilteredUrlText, false, null, null, null, "", this.ServerUrl, false);
	}

	// Token: 0x0400532F RID: 21295
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_ServerBrowserGamePrefInfo> infoFields = new List<XUiC_ServerBrowserGamePrefInfo>();

	// Token: 0x04005330 RID: 21296
	[XuiBindComponent("ServerWebsiteURL", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController lblServerUrl;

	// Token: 0x04005331 RID: 21297
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerInfo.DisplayMode displayMode;

	// Token: 0x04005332 RID: 21298
	[PublicizedFrom(EAccessModifier.Private)]
	public string unfilteredUrlText;

	// Token: 0x04005333 RID: 21299
	[PublicizedFrom(EAccessModifier.Private)]
	public string serverUrl;

	// Token: 0x04005334 RID: 21300
	[PublicizedFrom(EAccessModifier.Private)]
	public string serverDescription;

	// Token: 0x04005335 RID: 21301
	[PublicizedFrom(EAccessModifier.Private)]
	public bool serverUrlHovered;

	// Token: 0x04005336 RID: 21302
	[PublicizedFrom(EAccessModifier.Private)]
	public string serverSandboxCode;

	// Token: 0x02000DF3 RID: 3571
	[PublicizedFrom(EAccessModifier.Private)]
	public enum DisplayMode
	{
		// Token: 0x04005338 RID: 21304
		Dedicated,
		// Token: 0x04005339 RID: 21305
		Peer
	}
}
