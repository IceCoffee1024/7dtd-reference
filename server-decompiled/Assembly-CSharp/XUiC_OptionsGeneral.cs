using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DC8 RID: 3528
[Preserve]
public class XUiC_OptionsGeneral : XUiC_OptionsDialogBase
{
	// Token: 0x06006B18 RID: 27416 RVA: 0x002A1A74 File Offset: 0x0029FC74
	[PublicizedFrom(EAccessModifier.Private)]
	public void initLanguageOptions()
	{
		if (this.optionLanguage == null || this.comboLanguage == null)
		{
			return;
		}
		this.optionLanguage.GetSettingValue = delegate
		{
			this.comboLanguage.Elements.Clear();
			bool flag = false;
			foreach (string text in Localization.KnownLanguages)
			{
				if ((flag || (flag = text.EqualsCaseInsensitive("english"))) && text.IndexOf(' ') < 0)
				{
					this.comboLanguage.Elements.Add(new XUiC_OptionsGeneral.LanguageInfo(text));
				}
			}
			this.comboLanguage.Elements.Sort();
			this.comboLanguage.Elements.Insert(0, new XUiC_OptionsGeneral.LanguageInfo(""));
			string @string = GamePrefs.GetString(EnumGamePrefs.Language);
			for (int j = 0; j < this.comboLanguage.Elements.Count; j++)
			{
				if (this.comboLanguage.Elements[j].LanguageKey.EqualsCaseInsensitive(@string))
				{
					this.comboLanguage.SelectedIndex = j;
				}
			}
		};
		this.optionLanguage.DiscardChanges = delegate
		{
			string string2 = GamePrefs.GetString(EnumGamePrefs.Language);
			for (int k = 0; k < this.comboLanguage.Elements.Count; k++)
			{
				if (this.comboLanguage.Elements[k].LanguageKey.EqualsCaseInsensitive(string2))
				{
					this.comboLanguage.SelectedIndex = k;
				}
			}
		};
		this.optionLanguage.ApplyChanges = delegate(bool _)
		{
			this.languageChanged = this.comboLanguage.Value.LanguageKey != GamePrefs.GetString(EnumGamePrefs.Language);
			this.IsDirty |= this.languageChanged;
			GamePrefs.Set(EnumGamePrefs.Language, this.comboLanguage.Value.LanguageKey);
		};
		this.optionLanguage.ResetDefaults = delegate
		{
			this.comboLanguage.SelectedIndex = 0;
		};
		this.optionLanguage.IsChangedDelegate = () => this.comboLanguage.Value.LanguageKey != GamePrefs.GetString(EnumGamePrefs.Language);
		this.optionLanguage.IsDefaultDelegate = () => this.comboLanguage.Value.LanguageKey.EqualsCaseInsensitive((string)GamePrefs.GetDefault(EnumGamePrefs.Language));
	}

	// Token: 0x06006B19 RID: 27417 RVA: 0x002A1B1C File Offset: 0x0029FD1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void languageSavedOptions()
	{
		if (!this.languageChanged)
		{
			return;
		}
		this.languageChanged = false;
		Log.Out("Language selection changed: " + GamePrefs.GetString(EnumGamePrefs.Language));
		if ((DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX).IsCurrent())
		{
			XUiC_MessageBoxWindowGroup.ShowOkCancel(this.xui, Localization.Get("xuiConfirmRestartLanguageTitle", false, null), Localization.Get("xuiConfirmRestartLanguageText", false, null), "", delegate
			{
				Utils.RestartGame(Utils.ERestartAntiCheatMode.KeepAntiCheatMode);
			}, delegate
			{
				this.xui.playerUI.windowManager.Open(this.windowGroup, true);
			}, false, true, false);
			return;
		}
		if ((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent())
		{
			XUiC_MessageBoxWindowGroup.ShowOk(this.xui, Localization.Get("xuiLanguageChangedTitle", false, null), Localization.Get("xuiLanguageChangedText", false, null), "", delegate
			{
				this.xui.playerUI.windowManager.Open(this.windowGroup, true);
			}, false, true, false);
		}
	}

	// Token: 0x17000BBA RID: 3002
	// (get) Token: 0x06006B1A RID: 27418 RVA: 0x002A1BF1 File Offset: 0x0029FDF1
	[XuiXmlBinding("crosshairs_enabled")]
	public bool CrosshairsEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GamePrefs.GetBool(EnumGamePrefs.OptionsCrosshairEnabled);
		}
	}

	// Token: 0x06006B1B RID: 27419 RVA: 0x002A1C00 File Offset: 0x0029FE00
	[PublicizedFrom(EAccessModifier.Private)]
	public void initCrosshairOptions()
	{
		this.comboCrosshairsColor.Min = 0L;
		this.comboCrosshairsColor.Max = (long)(EntityPlayerLocal.crosshairColors.Count - 1);
		this.crosshairDrawer = this.viewComponent.UiTransform.gameObject.AddComponent<CrosshairDrawer>();
	}

	// Token: 0x06006B1C RID: 27420 RVA: 0x002A1C4D File Offset: 0x0029FE4D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnGamePrefChanged(EnumGamePrefs _pref)
	{
		if (_pref - EnumGamePrefs.OptionsCrosshairEnabled <= 3 || _pref - EnumGamePrefs.OptionsCrosshairThickness <= 1)
		{
			this.updateCrosshairElements();
		}
	}

	// Token: 0x06006B1D RID: 27421 RVA: 0x002A1C69 File Offset: 0x0029FE69
	[XuiBindEvent("OnTabChanged", "TabSelector")]
	[PublicizedFrom(EAccessModifier.Private)]
	public new void OnTabChanged(int _tabIndex, XUiC_TabSelectorTab _tab)
	{
		this.updateCrosshairElements();
	}

	// Token: 0x06006B1E RID: 27422 RVA: 0x002A1C74 File Offset: 0x0029FE74
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateRangedCrosshairDrwaing()
	{
		bool @bool = GamePrefs.GetBool(EnumGamePrefs.OptionsCrosshairEnabled);
		bool bool2 = GamePrefs.GetBool(EnumGamePrefs.OptionsCrosshairRangedEnabled);
		this.crosshairDrawer.draw = this.TabSelector.SelectedTabIndex == 1 && @bool && bool2 && !XUiC_FullScreenCollider.IsBlocked(this.viewComponent);
	}

	// Token: 0x06006B1F RID: 27423 RVA: 0x002A1CC8 File Offset: 0x0029FEC8
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateCrosshairPosition()
	{
		Vector3 vector = this.xui.playerUI.uiCamera.cachedCamera.WorldToViewportPoint(this.crosshairRangedSample.UiTransform.position);
		vector.x *= (float)Screen.width;
		vector.y *= (float)Screen.height;
		vector.y = (float)Screen.height - vector.y;
		this.crosshairDrawer.centerX = vector.x;
		this.crosshairDrawer.centerY = vector.y;
		this.updateRangedCrosshairDrwaing();
	}

	// Token: 0x06006B20 RID: 27424 RVA: 0x002A1D60 File Offset: 0x0029FF60
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateCrosshairElements()
	{
		float @float = GamePrefs.GetFloat(EnumGamePrefs.OptionsCrosshairScale);
		float float2 = GamePrefs.GetFloat(EnumGamePrefs.OptionsCrosshairThickness);
		float float3 = GamePrefs.GetFloat(EnumGamePrefs.OptionsCrosshairOpacity);
		int @int = GamePrefs.GetInt(EnumGamePrefs.OptionsCrosshairColor);
		Color color = EntityPlayerLocal.crosshairColors[@int];
		color.a = float3;
		this.crosshairSample.Color = color;
		this.crosshairSample.Size = new Vector2i((int)(@float * 100f), (int)(@float * 100f));
		this.updateRangedCrosshairDrwaing();
		if (!this.crosshairDrawer.draw)
		{
			return;
		}
		this.crosshairDrawer.thickness = float2;
		this.crosshairDrawer.length = @float * (float)Screen.height * 0.025f;
		this.crosshairDrawer.openAreaX = (this.crosshairDrawer.openAreaY = (float)Screen.height * 0.02f);
		this.crosshairDrawer.opacity = float3;
		this.crosshairDrawer.color = color;
		base.RefreshBindings();
	}

	// Token: 0x17000BBB RID: 3003
	// (get) Token: 0x06006B21 RID: 27425 RVA: 0x002A1E5A File Offset: 0x002A005A
	public static EUserPerms UserPermissions
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PermissionsManager.GetPermissions(PermissionsManager.PermissionSources.Platform | PermissionsManager.PermissionSources.LaunchPrefs | PermissionsManager.PermissionSources.DebugMask | PermissionsManager.PermissionSources.TitleStorage);
		}
	}

	// Token: 0x17000BBC RID: 3004
	// (get) Token: 0x06006B22 RID: 27426 RVA: 0x002A1E63 File Offset: 0x002A0063
	[XuiXmlBinding("has_communication_perm")]
	public bool HasCommunicationPermission
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_OptionsGeneral.UserPermissions.HasCommunication();
		}
	}

	// Token: 0x17000BBD RID: 3005
	// (get) Token: 0x06006B23 RID: 27427 RVA: 0x002A1E6F File Offset: 0x002A006F
	[XuiXmlBinding("has_crossplay_perm")]
	public bool HasCrossplayPermission
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_OptionsGeneral.UserPermissions.HasCrossplay();
		}
	}

	// Token: 0x17000BBE RID: 3006
	// (get) Token: 0x06006B24 RID: 27428 RVA: 0x002A1E7B File Offset: 0x002A007B
	[XuiXmlBinding("crossplayTooltip")]
	public string CrossplayTooltip
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PermissionsManager.GetPermissionDenyReason(EUserPerms.Crossplay, PermissionsManager.PermissionSources.Platform | PermissionsManager.PermissionSources.LaunchPrefs | PermissionsManager.PermissionSources.DebugMask | PermissionsManager.PermissionSources.TitleStorage) ?? Localization.Get("xuiOptionsGeneralCrossplayTooltip", false, null);
		}
	}

	// Token: 0x06006B25 RID: 27429 RVA: 0x002A1E98 File Offset: 0x002A0098
	[PublicizedFrom(EAccessModifier.Private)]
	public void initPermissionsBasedOptions()
	{
		if (this.optionChatComms != null)
		{
			this.optionChatComms.OverrideSettingValue = delegate(bool _, out bool _overrideValue)
			{
				_overrideValue = false;
				return !XUiC_OptionsGeneral.UserPermissions.HasCommunication();
			};
			this.optionChatComms.DoSaveOverride = () => XUiC_OptionsGeneral.UserPermissions.HasCommunication();
		}
		if (this.optionCrossplay != null)
		{
			this.optionCrossplay.OverrideSettingValue = delegate(bool _, out bool _overrideValue)
			{
				_overrideValue = false;
				return !XUiC_OptionsGeneral.UserPermissions.HasCrossplay();
			};
			this.optionCrossplay.DoSaveOverride = () => XUiC_OptionsGeneral.UserPermissions.HasCrossplay();
		}
	}

	// Token: 0x17000BBF RID: 3007
	// (get) Token: 0x06006B26 RID: 27430 RVA: 0x002A1F5D File Offset: 0x002A015D
	[XuiXmlBinding("bug_reporting")]
	public bool SupportsBugReporting
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return BacktraceUtils.BugReportFeature;
		}
	}

	// Token: 0x06006B27 RID: 27431 RVA: 0x002A1F64 File Offset: 0x002A0164
	public override void Init()
	{
		base.Init();
		XUiC_OptionsGeneral.ID = base.WindowGroup.Id;
		this.initLanguageOptions();
		this.initCrosshairOptions();
		this.initPermissionsBasedOptions();
	}

	// Token: 0x06006B28 RID: 27432 RVA: 0x002A1F8E File Offset: 0x002A018E
	[XuiBindEvent("OnPress", "btnEula")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnEula_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_EulaWindow.Open(this.xui, GameManager.HasAcceptedLatestEula());
	}

	// Token: 0x06006B29 RID: 27433 RVA: 0x002A1FA0 File Offset: 0x002A01A0
	[XuiBindEvent("OnPress", "btnBugReport")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBugReport_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_BugReportWindow.Open();
	}

	// Token: 0x06006B2A RID: 27434 RVA: 0x002A1FA7 File Offset: 0x002A01A7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void afterChangesSaved()
	{
		base.afterChangesSaved();
		this.languageSavedOptions();
	}

	// Token: 0x06006B2B RID: 27435 RVA: 0x002A1FB5 File Offset: 0x002A01B5
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.updateCrosshairPosition();
	}

	// Token: 0x06006B2C RID: 27436 RVA: 0x002A1FC4 File Offset: 0x002A01C4
	public override void OnOpen()
	{
		this.updateCrosshairElements();
		base.OnOpen();
	}

	// Token: 0x040051E2 RID: 20962
	[XuiBindComponent("OptionLanguage", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionLanguage;

	// Token: 0x040051E3 RID: 20963
	[XuiBindComponent("Language", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_OptionsGeneral.LanguageInfo> comboLanguage;

	// Token: 0x040051E4 RID: 20964
	[PublicizedFrom(EAccessModifier.Private)]
	public bool languageChanged;

	// Token: 0x040051E5 RID: 20965
	[XuiBindComponent("CrosshairsColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxInt comboCrosshairsColor;

	// Token: 0x040051E6 RID: 20966
	[XuiBindComponent("CrosshairsSample", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite crosshairSample;

	// Token: 0x040051E7 RID: 20967
	[XuiBindComponent("rangedCrosshairSample", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiView crosshairRangedSample;

	// Token: 0x040051E8 RID: 20968
	[PublicizedFrom(EAccessModifier.Private)]
	public CrosshairDrawer crosshairDrawer;

	// Token: 0x040051E9 RID: 20969
	[XuiBindComponent("OptionChatComms", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefBool optionChatComms;

	// Token: 0x040051EA RID: 20970
	[XuiBindComponent("OptionCrossplay", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefBool optionCrossplay;

	// Token: 0x040051EB RID: 20971
	public static string ID = "";

	// Token: 0x040051EC RID: 20972
	[XuiBindComponent("btnEula", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnEula;

	// Token: 0x040051ED RID: 20973
	[XuiBindComponent("btnBugReport", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBugReport;

	// Token: 0x02000DC9 RID: 3529
	public readonly struct LanguageInfo : IComparable<XUiC_OptionsGeneral.LanguageInfo>, IEquatable<XUiC_OptionsGeneral.LanguageInfo>, IComparable
	{
		// Token: 0x06006B37 RID: 27447 RVA: 0x002A2204 File Offset: 0x002A0404
		public LanguageInfo(string _languageKey)
		{
			this.LanguageKey = _languageKey;
			if (_languageKey == "")
			{
				this.NameEnglish = null;
				this.NameNative = null;
				return;
			}
			this.NameEnglish = Localization.Get("languageNameEnglish", false, _languageKey);
			this.NameNative = Localization.Get("languageNameNative", false, _languageKey);
		}

		// Token: 0x06006B38 RID: 27448 RVA: 0x002A2258 File Offset: 0x002A0458
		public override string ToString()
		{
			if (!(this.LanguageKey == ""))
			{
				return this.NameEnglish + " / " + this.NameNative;
			}
			return "-Auto-";
		}

		// Token: 0x06006B39 RID: 27449 RVA: 0x002A2288 File Offset: 0x002A0488
		public bool Equals(XUiC_OptionsGeneral.LanguageInfo _other)
		{
			return this.LanguageKey == _other.LanguageKey;
		}

		// Token: 0x06006B3A RID: 27450 RVA: 0x002A229C File Offset: 0x002A049C
		public override bool Equals(object _obj)
		{
			if (_obj is XUiC_OptionsGeneral.LanguageInfo)
			{
				XUiC_OptionsGeneral.LanguageInfo languageInfo = (XUiC_OptionsGeneral.LanguageInfo)_obj;
				return this.Equals(languageInfo);
			}
			return false;
		}

		// Token: 0x06006B3B RID: 27451 RVA: 0x002A22C1 File Offset: 0x002A04C1
		public override int GetHashCode()
		{
			if (this.LanguageKey == null)
			{
				return 0;
			}
			return this.LanguageKey.GetHashCode();
		}

		// Token: 0x06006B3C RID: 27452 RVA: 0x002A22D8 File Offset: 0x002A04D8
		public int CompareTo(XUiC_OptionsGeneral.LanguageInfo _other)
		{
			return string.Compare(this.NameEnglish, _other.NameEnglish, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06006B3D RID: 27453 RVA: 0x002A22EC File Offset: 0x002A04EC
		public int CompareTo(object _obj)
		{
			if (_obj == null)
			{
				return 1;
			}
			if (_obj is XUiC_OptionsGeneral.LanguageInfo)
			{
				XUiC_OptionsGeneral.LanguageInfo languageInfo = (XUiC_OptionsGeneral.LanguageInfo)_obj;
				return this.CompareTo(languageInfo);
			}
			throw new ArgumentException("Object must be of type LanguageInfo");
		}

		// Token: 0x040051EE RID: 20974
		public readonly string NameEnglish;

		// Token: 0x040051EF RID: 20975
		public readonly string NameNative;

		// Token: 0x040051F0 RID: 20976
		public readonly string LanguageKey;
	}
}
