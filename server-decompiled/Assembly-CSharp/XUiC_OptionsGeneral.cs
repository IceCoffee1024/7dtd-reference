using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D9A RID: 3482
[Preserve]
public class XUiC_OptionsGeneral : XUiC_OptionsDialogBase
{
	// Token: 0x06006A60 RID: 27232 RVA: 0x0029A05C File Offset: 0x0029825C
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

	// Token: 0x06006A61 RID: 27233 RVA: 0x0029A104 File Offset: 0x00298304
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

	// Token: 0x17000B81 RID: 2945
	// (get) Token: 0x06006A62 RID: 27234 RVA: 0x0029A1D9 File Offset: 0x002983D9
	[XuiXmlBinding("crosshairs_enabled")]
	public bool CrosshairsEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GamePrefs.GetBool(EnumGamePrefs.OptionsCrosshairEnabled);
		}
	}

	// Token: 0x06006A63 RID: 27235 RVA: 0x0029A1E8 File Offset: 0x002983E8
	[PublicizedFrom(EAccessModifier.Private)]
	public void initCrosshairOptions()
	{
		this.comboCrosshairsColor.Min = 0L;
		this.comboCrosshairsColor.Max = (long)(EntityPlayerLocal.crosshairColors.Count - 1);
		this.crosshairDrawer = this.viewComponent.UiTransform.gameObject.AddComponent<CrosshairDrawer>();
	}

	// Token: 0x06006A64 RID: 27236 RVA: 0x0029A235 File Offset: 0x00298435
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnGamePrefChanged(EnumGamePrefs _pref)
	{
		if (_pref - EnumGamePrefs.OptionsCrosshairEnabled <= 3 || _pref - EnumGamePrefs.OptionsCrosshairThickness <= 1)
		{
			this.updateCrosshairElements();
		}
	}

	// Token: 0x06006A65 RID: 27237 RVA: 0x0029A251 File Offset: 0x00298451
	[XuiBindEvent("OnTabChanged", "TabSelector")]
	[PublicizedFrom(EAccessModifier.Private)]
	public new void OnTabChanged(int _tabIndex, XUiC_TabSelectorTab _tab)
	{
		this.updateCrosshairElements();
	}

	// Token: 0x06006A66 RID: 27238 RVA: 0x0029A25C File Offset: 0x0029845C
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateRangedCrosshairDrwaing()
	{
		bool @bool = GamePrefs.GetBool(EnumGamePrefs.OptionsCrosshairEnabled);
		bool bool2 = GamePrefs.GetBool(EnumGamePrefs.OptionsCrosshairRangedEnabled);
		this.crosshairDrawer.draw = this.TabSelector.SelectedTabIndex == 1 && @bool && bool2 && !XUiC_FullScreenCollider.IsBlocked(this.viewComponent);
	}

	// Token: 0x06006A67 RID: 27239 RVA: 0x0029A2B0 File Offset: 0x002984B0
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

	// Token: 0x06006A68 RID: 27240 RVA: 0x0029A348 File Offset: 0x00298548
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

	// Token: 0x17000B82 RID: 2946
	// (get) Token: 0x06006A69 RID: 27241 RVA: 0x0029A442 File Offset: 0x00298642
	public static EUserPerms UserPermissions
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PermissionsManager.GetPermissions(PermissionsManager.PermissionSources.Platform | PermissionsManager.PermissionSources.LaunchPrefs | PermissionsManager.PermissionSources.DebugMask | PermissionsManager.PermissionSources.TitleStorage);
		}
	}

	// Token: 0x17000B83 RID: 2947
	// (get) Token: 0x06006A6A RID: 27242 RVA: 0x0029A44B File Offset: 0x0029864B
	[XuiXmlBinding("has_communication_perm")]
	public bool HasCommunicationPermission
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_OptionsGeneral.UserPermissions.HasCommunication();
		}
	}

	// Token: 0x17000B84 RID: 2948
	// (get) Token: 0x06006A6B RID: 27243 RVA: 0x0029A457 File Offset: 0x00298657
	[XuiXmlBinding("has_crossplay_perm")]
	public bool HasCrossplayPermission
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_OptionsGeneral.UserPermissions.HasCrossplay();
		}
	}

	// Token: 0x17000B85 RID: 2949
	// (get) Token: 0x06006A6C RID: 27244 RVA: 0x0029A463 File Offset: 0x00298663
	[XuiXmlBinding("crossplayTooltip")]
	public string CrossplayTooltip
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PermissionsManager.GetPermissionDenyReason(EUserPerms.Crossplay, PermissionsManager.PermissionSources.Platform | PermissionsManager.PermissionSources.LaunchPrefs | PermissionsManager.PermissionSources.DebugMask | PermissionsManager.PermissionSources.TitleStorage) ?? Localization.Get("xuiOptionsGeneralCrossplayTooltip", false, null);
		}
	}

	// Token: 0x06006A6D RID: 27245 RVA: 0x0029A480 File Offset: 0x00298680
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

	// Token: 0x17000B86 RID: 2950
	// (get) Token: 0x06006A6E RID: 27246 RVA: 0x0029A545 File Offset: 0x00298745
	[XuiXmlBinding("bug_reporting")]
	public bool SupportsBugReporting
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return BacktraceUtils.BugReportFeature;
		}
	}

	// Token: 0x06006A6F RID: 27247 RVA: 0x0029A54C File Offset: 0x0029874C
	public override void Init()
	{
		base.Init();
		XUiC_OptionsGeneral.ID = base.WindowGroup.Id;
		this.initLanguageOptions();
		this.initCrosshairOptions();
		this.initPermissionsBasedOptions();
	}

	// Token: 0x06006A70 RID: 27248 RVA: 0x0029A576 File Offset: 0x00298776
	[XuiBindEvent("OnPress", "btnEula")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnEula_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_EulaWindow.Open(this.xui, GameManager.HasAcceptedLatestEula());
	}

	// Token: 0x06006A71 RID: 27249 RVA: 0x0029A588 File Offset: 0x00298788
	[XuiBindEvent("OnPress", "btnBugReport")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBugReport_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_BugReportWindow.Open();
	}

	// Token: 0x06006A72 RID: 27250 RVA: 0x0029A58F File Offset: 0x0029878F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void afterChangesSaved()
	{
		base.afterChangesSaved();
		this.languageSavedOptions();
	}

	// Token: 0x06006A73 RID: 27251 RVA: 0x0029A59D File Offset: 0x0029879D
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.updateCrosshairPosition();
	}

	// Token: 0x06006A74 RID: 27252 RVA: 0x0029A5AC File Offset: 0x002987AC
	public override void OnOpen()
	{
		this.updateCrosshairElements();
		base.OnOpen();
	}

	// Token: 0x040050E4 RID: 20708
	[XuiBindComponent("OptionLanguage", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionLanguage;

	// Token: 0x040050E5 RID: 20709
	[XuiBindComponent("Language", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_OptionsGeneral.LanguageInfo> comboLanguage;

	// Token: 0x040050E6 RID: 20710
	[PublicizedFrom(EAccessModifier.Private)]
	public bool languageChanged;

	// Token: 0x040050E7 RID: 20711
	[XuiBindComponent("CrosshairsColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxInt comboCrosshairsColor;

	// Token: 0x040050E8 RID: 20712
	[XuiBindComponent("CrosshairsSample", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite crosshairSample;

	// Token: 0x040050E9 RID: 20713
	[XuiBindComponent("rangedCrosshairSample", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiView crosshairRangedSample;

	// Token: 0x040050EA RID: 20714
	[PublicizedFrom(EAccessModifier.Private)]
	public CrosshairDrawer crosshairDrawer;

	// Token: 0x040050EB RID: 20715
	[XuiBindComponent("OptionChatComms", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefBool optionChatComms;

	// Token: 0x040050EC RID: 20716
	[XuiBindComponent("OptionCrossplay", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefBool optionCrossplay;

	// Token: 0x040050ED RID: 20717
	public static string ID = "";

	// Token: 0x040050EE RID: 20718
	[XuiBindComponent("btnEula", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnEula;

	// Token: 0x040050EF RID: 20719
	[XuiBindComponent("btnBugReport", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBugReport;

	// Token: 0x02000D9B RID: 3483
	public readonly struct LanguageInfo : IComparable<XUiC_OptionsGeneral.LanguageInfo>, IEquatable<XUiC_OptionsGeneral.LanguageInfo>, IComparable
	{
		// Token: 0x06006A7F RID: 27263 RVA: 0x0029A7EC File Offset: 0x002989EC
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

		// Token: 0x06006A80 RID: 27264 RVA: 0x0029A840 File Offset: 0x00298A40
		public override string ToString()
		{
			if (!(this.LanguageKey == ""))
			{
				return this.NameEnglish + " / " + this.NameNative;
			}
			return "-Auto-";
		}

		// Token: 0x06006A81 RID: 27265 RVA: 0x0029A870 File Offset: 0x00298A70
		public bool Equals(XUiC_OptionsGeneral.LanguageInfo _other)
		{
			return this.LanguageKey == _other.LanguageKey;
		}

		// Token: 0x06006A82 RID: 27266 RVA: 0x0029A884 File Offset: 0x00298A84
		public override bool Equals(object _obj)
		{
			if (_obj is XUiC_OptionsGeneral.LanguageInfo)
			{
				XUiC_OptionsGeneral.LanguageInfo languageInfo = (XUiC_OptionsGeneral.LanguageInfo)_obj;
				return this.Equals(languageInfo);
			}
			return false;
		}

		// Token: 0x06006A83 RID: 27267 RVA: 0x0029A8A9 File Offset: 0x00298AA9
		public override int GetHashCode()
		{
			if (this.LanguageKey == null)
			{
				return 0;
			}
			return this.LanguageKey.GetHashCode();
		}

		// Token: 0x06006A84 RID: 27268 RVA: 0x0029A8C0 File Offset: 0x00298AC0
		public int CompareTo(XUiC_OptionsGeneral.LanguageInfo _other)
		{
			return string.Compare(this.NameEnglish, _other.NameEnglish, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06006A85 RID: 27269 RVA: 0x0029A8D4 File Offset: 0x00298AD4
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

		// Token: 0x040050F0 RID: 20720
		public readonly string NameEnglish;

		// Token: 0x040050F1 RID: 20721
		public readonly string NameNative;

		// Token: 0x040050F2 RID: 20722
		public readonly string LanguageKey;
	}
}
