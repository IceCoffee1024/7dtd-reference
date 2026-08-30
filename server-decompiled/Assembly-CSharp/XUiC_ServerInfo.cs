using System;
using System.Collections.Generic;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x02000DE3 RID: 3555
[Preserve]
public class XUiC_ServerInfo : XUiController
{
	// Token: 0x17000C01 RID: 3073
	// (get) Token: 0x06006CCB RID: 27851 RVA: 0x002A5C28 File Offset: 0x002A3E28
	// (set) Token: 0x06006CCC RID: 27852 RVA: 0x002A5C39 File Offset: 0x002A3E39
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

	// Token: 0x17000C02 RID: 3074
	// (get) Token: 0x06006CCD RID: 27853 RVA: 0x002A5C49 File Offset: 0x002A3E49
	// (set) Token: 0x06006CCE RID: 27854 RVA: 0x002A5C51 File Offset: 0x002A3E51
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

	// Token: 0x17000C03 RID: 3075
	// (get) Token: 0x06006CCF RID: 27855 RVA: 0x002A5C61 File Offset: 0x002A3E61
	// (set) Token: 0x06006CD0 RID: 27856 RVA: 0x002A5C72 File Offset: 0x002A3E72
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

	// Token: 0x17000C04 RID: 3076
	// (get) Token: 0x06006CD1 RID: 27857 RVA: 0x002A5C82 File Offset: 0x002A3E82
	// (set) Token: 0x06006CD2 RID: 27858 RVA: 0x002A5C93 File Offset: 0x002A3E93
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

	// Token: 0x17000C05 RID: 3077
	// (get) Token: 0x06006CD3 RID: 27859 RVA: 0x002A5CA4 File Offset: 0x002A3EA4
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

	// Token: 0x17000C06 RID: 3078
	// (get) Token: 0x06006CD4 RID: 27860 RVA: 0x002A5D00 File Offset: 0x002A3F00
	[XuiXmlBinding("showip")]
	public bool ShowIp
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.displayMode == XUiC_ServerInfo.DisplayMode.Dedicated;
		}
	}

	// Token: 0x06006CD5 RID: 27861 RVA: 0x002A5D0C File Offset: 0x002A3F0C
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

	// Token: 0x06006CD6 RID: 27862 RVA: 0x0028DC1B File Offset: 0x0028BE1B
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06006CD7 RID: 27863 RVA: 0x002A5DEF File Offset: 0x002A3FEF
	public void InitializeForListFilter(XUiC_ServersList.EnumServerLists _mode)
	{
		if (_mode == XUiC_ServersList.EnumServerLists.Peer || _mode == XUiC_ServersList.EnumServerLists.Friends || _mode == XUiC_ServersList.EnumServerLists.History)
		{
			this.setDisplayMode(XUiC_ServerInfo.DisplayMode.Peer);
			return;
		}
		this.setDisplayMode(XUiC_ServerInfo.DisplayMode.Dedicated);
	}

	// Token: 0x06006CD8 RID: 27864 RVA: 0x002A5E0C File Offset: 0x002A400C
	[PublicizedFrom(EAccessModifier.Private)]
	public void setDisplayMode(XUiC_ServerInfo.DisplayMode _mode)
	{
		if (this.displayMode != _mode)
		{
			this.displayMode = _mode;
			this.IsDirty = true;
		}
	}

	// Token: 0x06006CD9 RID: 27865 RVA: 0x002A5E28 File Offset: 0x002A4028
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

	// Token: 0x06006CDA RID: 27866 RVA: 0x002A5F54 File Offset: 0x002A4154
	[XuiBindEvent("OnHover", "lblServerUrl")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void UrlController_OnHover(XUiController _sender, bool _isOver)
	{
		this.ServerUrlHovered = _isOver;
	}

	// Token: 0x06006CDB RID: 27867 RVA: 0x002A5F60 File Offset: 0x002A4160
	[XuiBindEvent("OnPress", "lblServerUrl")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void UrlController_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_MessageBoxWindowGroup.ShowUrlConfirmationDialog(this.xui, this.unfilteredUrlText, false, null, null, null, "", this.ServerUrl, false);
	}

	// Token: 0x040052F8 RID: 21240
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_ServerBrowserGamePrefInfo> infoFields = new List<XUiC_ServerBrowserGamePrefInfo>();

	// Token: 0x040052F9 RID: 21241
	[XuiBindComponent("ServerWebsiteURL", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController lblServerUrl;

	// Token: 0x040052FA RID: 21242
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerInfo.DisplayMode displayMode;

	// Token: 0x040052FB RID: 21243
	[PublicizedFrom(EAccessModifier.Private)]
	public string unfilteredUrlText;

	// Token: 0x040052FC RID: 21244
	[PublicizedFrom(EAccessModifier.Private)]
	public string serverUrl;

	// Token: 0x040052FD RID: 21245
	[PublicizedFrom(EAccessModifier.Private)]
	public string serverDescription;

	// Token: 0x040052FE RID: 21246
	[PublicizedFrom(EAccessModifier.Private)]
	public bool serverUrlHovered;

	// Token: 0x040052FF RID: 21247
	[PublicizedFrom(EAccessModifier.Private)]
	public string serverSandboxCode;

	// Token: 0x02000DE4 RID: 3556
	[PublicizedFrom(EAccessModifier.Private)]
	public enum DisplayMode
	{
		// Token: 0x04005301 RID: 21249
		Dedicated,
		// Token: 0x04005302 RID: 21250
		Peer
	}
}
