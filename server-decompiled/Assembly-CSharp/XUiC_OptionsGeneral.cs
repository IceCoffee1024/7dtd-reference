using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DBD RID: 3517
[Preserve]
public class XUiC_OptionsGeneral : XUiC_OptionsDialogBase
{
	// Token: 0x06006AF3 RID: 27379 RVA: 0x002A054C File Offset: 0x0029E74C
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

	// Token: 0x06006AF4 RID: 27380 RVA: 0x002A05F4 File Offset: 0x0029E7F4
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

	// Token: 0x17000BB4 RID: 2996
	// (get) Token: 0x06006AF5 RID: 27381 RVA: 0x002A06C9 File Offset: 0x0029E8C9
	[XuiXmlBinding("crosshairs_enabled")]
	public bool CrosshairsEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GamePrefs.GetBool(EnumGamePrefs.OptionsCrosshairEnabled);
		}
	}

	// Token: 0x06006AF6 RID: 27382 RVA: 0x002A06D8 File Offset: 0x0029E8D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void initCrosshairOptions()
	{
		this.comboCrosshairsColor.Min = 0L;
		this.comboCrosshairsColor.Max = (long)(EntityPlayerLocal.crosshairColors.Count - 1);
		this.crosshairDrawer = this.viewComponent.UiTransform.gameObject.AddComponent<CrosshairDrawer>();
	}

	// Token: 0x06006AF7 RID: 27383 RVA: 0x002A0725 File Offset: 0x0029E925
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnGamePrefChanged(EnumGamePrefs _pref)
	{
		if (_pref - EnumGamePrefs.OptionsCrosshairEnabled <= 3 || _pref - EnumGamePrefs.OptionsCrosshairThickness <= 1)
		{
			this.updateCrosshairElements();
		}
	}

	// Token: 0x06006AF8 RID: 27384 RVA: 0x002A0741 File Offset: 0x0029E941
	[XuiBindEvent("OnTabChanged", "TabSelector")]
	[PublicizedFrom(EAccessModifier.Private)]
	public new void OnTabChanged(int _tabIndex, XUiC_TabSelectorTab _tab)
	{
		this.updateCrosshairElements();
	}

	// Token: 0x06006AF9 RID: 27385 RVA: 0x002A074C File Offset: 0x0029E94C
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateRangedCrosshairDrwaing()
	{
		bool @bool = GamePrefs.GetBool(EnumGamePrefs.OptionsCrosshairEnabled);
		bool bool2 = GamePrefs.GetBool(EnumGamePrefs.OptionsCrosshairRangedEnabled);
		this.crosshairDrawer.draw = this.TabSelector.SelectedTabIndex == 1 && @bool && bool2 && !XUiC_FullScreenCollider.IsBlocked(this.viewComponent);
	}

	// Token: 0x06006AFA RID: 27386 RVA: 0x002A07A0 File Offset: 0x0029E9A0
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

	// Token: 0x06006AFB RID: 27387 RVA: 0x002A0838 File Offset: 0x0029EA38
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

	// Token: 0x17000BB5 RID: 2997
	// (get) Token: 0x06006AFC RID: 27388 RVA: 0x002A0932 File Offset: 0x0029EB32
	public static EUserPerms UserPermissions
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PermissionsManager.GetPermissions(PermissionsManager.PermissionSources.Platform | PermissionsManager.PermissionSources.LaunchPrefs | PermissionsManager.PermissionSources.DebugMask | PermissionsManager.PermissionSources.TitleStorage);
		}
	}

	// Token: 0x17000BB6 RID: 2998
	// (get) Token: 0x06006AFD RID: 27389 RVA: 0x002A093B File Offset: 0x0029EB3B
	[XuiXmlBinding("has_communication_perm")]
	public bool HasCommunicationPermission
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_OptionsGeneral.UserPermissions.HasCommunication();
		}
	}

	// Token: 0x17000BB7 RID: 2999
	// (get) Token: 0x06006AFE RID: 27390 RVA: 0x002A0947 File Offset: 0x0029EB47
	[XuiXmlBinding("has_crossplay_perm")]
	public bool HasCrossplayPermission
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_OptionsGeneral.UserPermissions.HasCrossplay();
		}
	}

	// Token: 0x17000BB8 RID: 3000
	// (get) Token: 0x06006AFF RID: 27391 RVA: 0x002A0953 File Offset: 0x0029EB53
	[XuiXmlBinding("crossplayTooltip")]
	public string CrossplayTooltip
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PermissionsManager.GetPermissionDenyReason(EUserPerms.Crossplay, PermissionsManager.PermissionSources.Platform | PermissionsManager.PermissionSources.LaunchPrefs | PermissionsManager.PermissionSources.DebugMask | PermissionsManager.PermissionSources.TitleStorage) ?? Localization.Get("xuiOptionsGeneralCrossplayTooltip", false, null);
		}
	}

	// Token: 0x06006B00 RID: 27392 RVA: 0x002A0970 File Offset: 0x0029EB70
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

	// Token: 0x17000BB9 RID: 3001
	// (get) Token: 0x06006B01 RID: 27393 RVA: 0x002A0A35 File Offset: 0x0029EC35
	[XuiXmlBinding("bug_reporting")]
	public bool SupportsBugReporting
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return BacktraceUtils.BugReportFeature;
		}
	}

	// Token: 0x06006B02 RID: 27394 RVA: 0x002A0A3C File Offset: 0x0029EC3C
	public override void Init()
	{
		base.Init();
		XUiC_OptionsGeneral.ID = base.WindowGroup.Id;
		this.initLanguageOptions();
		this.initCrosshairOptions();
		this.initPermissionsBasedOptions();
	}

	// Token: 0x06006B03 RID: 27395 RVA: 0x002A0A66 File Offset: 0x0029EC66
	[XuiBindEvent("OnPress", "btnEula")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnEula_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_EulaWindow.Open(this.xui, GameManager.HasAcceptedLatestEula());
	}

	// Token: 0x06006B04 RID: 27396 RVA: 0x002A0A78 File Offset: 0x0029EC78
	[XuiBindEvent("OnPress", "btnBugReport")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBugReport_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_BugReportWindow.Open();
	}

	// Token: 0x06006B05 RID: 27397 RVA: 0x002A0A7F File Offset: 0x0029EC7F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void afterChangesSaved()
	{
		base.afterChangesSaved();
		this.languageSavedOptions();
	}

	// Token: 0x06006B06 RID: 27398 RVA: 0x002A0A8D File Offset: 0x0029EC8D
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.updateCrosshairPosition();
	}

	// Token: 0x06006B07 RID: 27399 RVA: 0x002A0A9C File Offset: 0x0029EC9C
	public override void OnOpen()
	{
		this.updateCrosshairElements();
		base.OnOpen();
	}

	// Token: 0x040051BD RID: 20925
	[XuiBindComponent("OptionLanguage", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionLanguage;

	// Token: 0x040051BE RID: 20926
	[XuiBindComponent("Language", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_OptionsGeneral.LanguageInfo> comboLanguage;

	// Token: 0x040051BF RID: 20927
	[PublicizedFrom(EAccessModifier.Private)]
	public bool languageChanged;

	// Token: 0x040051C0 RID: 20928
	[XuiBindComponent("CrosshairsColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxInt comboCrosshairsColor;

	// Token: 0x040051C1 RID: 20929
	[XuiBindComponent("CrosshairsSample", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite crosshairSample;

	// Token: 0x040051C2 RID: 20930
	[XuiBindComponent("rangedCrosshairSample", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiView crosshairRangedSample;

	// Token: 0x040051C3 RID: 20931
	[PublicizedFrom(EAccessModifier.Private)]
	public CrosshairDrawer crosshairDrawer;

	// Token: 0x040051C4 RID: 20932
	[XuiBindComponent("OptionChatComms", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefBool optionChatComms;

	// Token: 0x040051C5 RID: 20933
	[XuiBindComponent("OptionCrossplay", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefBool optionCrossplay;

	// Token: 0x040051C6 RID: 20934
	public static string ID = "";

	// Token: 0x040051C7 RID: 20935
	[XuiBindComponent("btnEula", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnEula;

	// Token: 0x040051C8 RID: 20936
	[XuiBindComponent("btnBugReport", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBugReport;

	// Token: 0x02000DBE RID: 3518
	public readonly struct LanguageInfo : IComparable<XUiC_OptionsGeneral.LanguageInfo>, IEquatable<XUiC_OptionsGeneral.LanguageInfo>, IComparable
	{
		// Token: 0x06006B12 RID: 27410 RVA: 0x002A0CDC File Offset: 0x0029EEDC
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

		// Token: 0x06006B13 RID: 27411 RVA: 0x002A0D30 File Offset: 0x0029EF30
		public override string ToString()
		{
			if (!(this.LanguageKey == ""))
			{
				return this.NameEnglish + " / " + this.NameNative;
			}
			return "-Auto-";
		}

		// Token: 0x06006B14 RID: 27412 RVA: 0x002A0D60 File Offset: 0x0029EF60
		public bool Equals(XUiC_OptionsGeneral.LanguageInfo _other)
		{
			return this.LanguageKey == _other.LanguageKey;
		}

		// Token: 0x06006B15 RID: 27413 RVA: 0x002A0D74 File Offset: 0x0029EF74
		public override bool Equals(object _obj)
		{
			if (_obj is XUiC_OptionsGeneral.LanguageInfo)
			{
				XUiC_OptionsGeneral.LanguageInfo languageInfo = (XUiC_OptionsGeneral.LanguageInfo)_obj;
				return this.Equals(languageInfo);
			}
			return false;
		}

		// Token: 0x06006B16 RID: 27414 RVA: 0x002A0D99 File Offset: 0x0029EF99
		public override int GetHashCode()
		{
			if (this.LanguageKey == null)
			{
				return 0;
			}
			return this.LanguageKey.GetHashCode();
		}

		// Token: 0x06006B17 RID: 27415 RVA: 0x002A0DB0 File Offset: 0x0029EFB0
		public int CompareTo(XUiC_OptionsGeneral.LanguageInfo _other)
		{
			return string.Compare(this.NameEnglish, _other.NameEnglish, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06006B18 RID: 27416 RVA: 0x002A0DC4 File Offset: 0x0029EFC4
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

		// Token: 0x040051C9 RID: 20937
		public readonly string NameEnglish;

		// Token: 0x040051CA RID: 20938
		public readonly string NameNative;

		// Token: 0x040051CB RID: 20939
		public readonly string LanguageKey;
	}
}
