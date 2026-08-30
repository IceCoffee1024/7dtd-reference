using System;
using System.Collections.Generic;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x02000DCE RID: 3534
[Preserve]
public class XUiC_ServerInfo : XUiController
{
	// Token: 0x17000BE8 RID: 3048
	// (get) Token: 0x06006C2C RID: 27692 RVA: 0x002A2794 File Offset: 0x002A0994
	// (set) Token: 0x06006C2D RID: 27693 RVA: 0x002A27A5 File Offset: 0x002A09A5
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

	// Token: 0x17000BE9 RID: 3049
	// (get) Token: 0x06006C2E RID: 27694 RVA: 0x002A27B5 File Offset: 0x002A09B5
	// (set) Token: 0x06006C2F RID: 27695 RVA: 0x002A27BD File Offset: 0x002A09BD
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

	// Token: 0x17000BEA RID: 3050
	// (get) Token: 0x06006C30 RID: 27696 RVA: 0x002A27CD File Offset: 0x002A09CD
	// (set) Token: 0x06006C31 RID: 27697 RVA: 0x002A27DE File Offset: 0x002A09DE
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

	// Token: 0x17000BEB RID: 3051
	// (get) Token: 0x06006C32 RID: 27698 RVA: 0x002A27EE File Offset: 0x002A09EE
	// (set) Token: 0x06006C33 RID: 27699 RVA: 0x002A27FF File Offset: 0x002A09FF
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

	// Token: 0x17000BEC RID: 3052
	// (get) Token: 0x06006C34 RID: 27700 RVA: 0x002A2810 File Offset: 0x002A0A10
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

	// Token: 0x17000BED RID: 3053
	// (get) Token: 0x06006C35 RID: 27701 RVA: 0x002A286C File Offset: 0x002A0A6C
	[XuiXmlBinding("showip")]
	public bool ShowIp
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.displayMode == XUiC_ServerInfo.DisplayMode.Dedicated;
		}
	}

	// Token: 0x06006C36 RID: 27702 RVA: 0x002A2878 File Offset: 0x002A0A78
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

	// Token: 0x06006C37 RID: 27703 RVA: 0x0028AD37 File Offset: 0x00288F37
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06006C38 RID: 27704 RVA: 0x002A295B File Offset: 0x002A0B5B
	public void InitializeForListFilter(XUiC_ServersList.EnumServerLists _mode)
	{
		if (_mode == XUiC_ServersList.EnumServerLists.Peer || _mode == XUiC_ServersList.EnumServerLists.Friends || _mode == XUiC_ServersList.EnumServerLists.History)
		{
			this.setDisplayMode(XUiC_ServerInfo.DisplayMode.Peer);
			return;
		}
		this.setDisplayMode(XUiC_ServerInfo.DisplayMode.Dedicated);
	}

	// Token: 0x06006C39 RID: 27705 RVA: 0x002A2978 File Offset: 0x002A0B78
	[PublicizedFrom(EAccessModifier.Private)]
	public void setDisplayMode(XUiC_ServerInfo.DisplayMode _mode)
	{
		if (this.displayMode != _mode)
		{
			this.displayMode = _mode;
			this.IsDirty = true;
		}
	}

	// Token: 0x06006C3A RID: 27706 RVA: 0x002A2994 File Offset: 0x002A0B94
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

	// Token: 0x06006C3B RID: 27707 RVA: 0x002A2AC0 File Offset: 0x002A0CC0
	[XuiBindEvent("OnHover", "lblServerUrl")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void UrlController_OnHover(XUiController _sender, bool _isOver)
	{
		this.ServerUrlHovered = _isOver;
	}

	// Token: 0x06006C3C RID: 27708 RVA: 0x002A2ACC File Offset: 0x002A0CCC
	[XuiBindEvent("OnPress", "lblServerUrl")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void UrlController_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_MessageBoxWindowGroup.ShowUrlConfirmationDialog(this.xui, this.unfilteredUrlText, false, null, null, null, "", this.ServerUrl, false);
	}

	// Token: 0x04005255 RID: 21077
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_ServerBrowserGamePrefInfo> infoFields = new List<XUiC_ServerBrowserGamePrefInfo>();

	// Token: 0x04005256 RID: 21078
	[XuiBindComponent("ServerWebsiteURL", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController lblServerUrl;

	// Token: 0x04005257 RID: 21079
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerInfo.DisplayMode displayMode;

	// Token: 0x04005258 RID: 21080
	[PublicizedFrom(EAccessModifier.Private)]
	public string unfilteredUrlText;

	// Token: 0x04005259 RID: 21081
	[PublicizedFrom(EAccessModifier.Private)]
	public string serverUrl;

	// Token: 0x0400525A RID: 21082
	[PublicizedFrom(EAccessModifier.Private)]
	public string serverDescription;

	// Token: 0x0400525B RID: 21083
	[PublicizedFrom(EAccessModifier.Private)]
	public bool serverUrlHovered;

	// Token: 0x0400525C RID: 21084
	[PublicizedFrom(EAccessModifier.Private)]
	public string serverSandboxCode;

	// Token: 0x02000DCF RID: 3535
	[PublicizedFrom(EAccessModifier.Private)]
	public enum DisplayMode
	{
		// Token: 0x0400525E RID: 21086
		Dedicated,
		// Token: 0x0400525F RID: 21087
		Peer
	}
}
