using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DAE RID: 3502
[Preserve]
public class XUiC_OptionsGeneral : XUiC_OptionsDialogBase
{
	// Token: 0x06006AFB RID: 27387 RVA: 0x0029D1BC File Offset: 0x0029B3BC
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

	// Token: 0x06006AFC RID: 27388 RVA: 0x0029D264 File Offset: 0x0029B464
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

	// Token: 0x17000B99 RID: 2969
	// (get) Token: 0x06006AFD RID: 27389 RVA: 0x0029D339 File Offset: 0x0029B539
	[XuiXmlBinding("crosshairs_enabled")]
	public bool CrosshairsEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GamePrefs.GetBool(EnumGamePrefs.OptionsCrosshairEnabled);
		}
	}

	// Token: 0x06006AFE RID: 27390 RVA: 0x0029D348 File Offset: 0x0029B548
	[PublicizedFrom(EAccessModifier.Private)]
	public void initCrosshairOptions()
	{
		this.comboCrosshairsColor.Min = 0L;
		this.comboCrosshairsColor.Max = (long)(EntityPlayerLocal.crosshairColors.Count - 1);
		this.crosshairDrawer = this.viewComponent.UiTransform.gameObject.AddComponent<CrosshairDrawer>();
	}

	// Token: 0x06006AFF RID: 27391 RVA: 0x0029D395 File Offset: 0x0029B595
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnGamePrefChanged(EnumGamePrefs _pref)
	{
		if (_pref - EnumGamePrefs.OptionsCrosshairEnabled <= 3 || _pref - EnumGamePrefs.OptionsCrosshairThickness <= 1)
		{
			this.updateCrosshairElements();
		}
	}

	// Token: 0x06006B00 RID: 27392 RVA: 0x0029D3B1 File Offset: 0x0029B5B1
	[XuiBindEvent("OnTabChanged", "TabSelector")]
	[PublicizedFrom(EAccessModifier.Private)]
	public new void OnTabChanged(int _tabIndex, XUiC_TabSelectorTab _tab)
	{
		this.updateCrosshairElements();
	}

	// Token: 0x06006B01 RID: 27393 RVA: 0x0029D3BC File Offset: 0x0029B5BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateRangedCrosshairDrwaing()
	{
		bool @bool = GamePrefs.GetBool(EnumGamePrefs.OptionsCrosshairEnabled);
		bool bool2 = GamePrefs.GetBool(EnumGamePrefs.OptionsCrosshairRangedEnabled);
		this.crosshairDrawer.draw = this.TabSelector.SelectedTabIndex == 1 && @bool && bool2 && !XUiC_FullScreenCollider.IsBlocked(this.viewComponent);
	}

	// Token: 0x06006B02 RID: 27394 RVA: 0x0029D410 File Offset: 0x0029B610
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

	// Token: 0x06006B03 RID: 27395 RVA: 0x0029D4A8 File Offset: 0x0029B6A8
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

	// Token: 0x17000B9A RID: 2970
	// (get) Token: 0x06006B04 RID: 27396 RVA: 0x0029D5A2 File Offset: 0x0029B7A2
	public static EUserPerms UserPermissions
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PermissionsManager.GetPermissions(PermissionsManager.PermissionSources.Platform | PermissionsManager.PermissionSources.LaunchPrefs | PermissionsManager.PermissionSources.DebugMask | PermissionsManager.PermissionSources.TitleStorage);
		}
	}

	// Token: 0x17000B9B RID: 2971
	// (get) Token: 0x06006B05 RID: 27397 RVA: 0x0029D5AB File Offset: 0x0029B7AB
	[XuiXmlBinding("has_communication_perm")]
	public bool HasCommunicationPermission
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_OptionsGeneral.UserPermissions.HasCommunication();
		}
	}

	// Token: 0x17000B9C RID: 2972
	// (get) Token: 0x06006B06 RID: 27398 RVA: 0x0029D5B7 File Offset: 0x0029B7B7
	[XuiXmlBinding("has_crossplay_perm")]
	public bool HasCrossplayPermission
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_OptionsGeneral.UserPermissions.HasCrossplay();
		}
	}

	// Token: 0x17000B9D RID: 2973
	// (get) Token: 0x06006B07 RID: 27399 RVA: 0x0029D5C3 File Offset: 0x0029B7C3
	[XuiXmlBinding("crossplayTooltip")]
	public string CrossplayTooltip
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PermissionsManager.GetPermissionDenyReason(EUserPerms.Crossplay, PermissionsManager.PermissionSources.Platform | PermissionsManager.PermissionSources.LaunchPrefs | PermissionsManager.PermissionSources.DebugMask | PermissionsManager.PermissionSources.TitleStorage) ?? Localization.Get("xuiOptionsGeneralCrossplayTooltip", false, null);
		}
	}

	// Token: 0x06006B08 RID: 27400 RVA: 0x0029D5E0 File Offset: 0x0029B7E0
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

	// Token: 0x17000B9E RID: 2974
	// (get) Token: 0x06006B09 RID: 27401 RVA: 0x0029D6A5 File Offset: 0x0029B8A5
	[XuiXmlBinding("bug_reporting")]
	public bool SupportsBugReporting
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return BacktraceUtils.BugReportFeature;
		}
	}

	// Token: 0x06006B0A RID: 27402 RVA: 0x0029D6AC File Offset: 0x0029B8AC
	public override void Init()
	{
		base.Init();
		XUiC_OptionsGeneral.ID = base.WindowGroup.Id;
		this.initLanguageOptions();
		this.initCrosshairOptions();
		this.initPermissionsBasedOptions();
	}

	// Token: 0x06006B0B RID: 27403 RVA: 0x0029D6D6 File Offset: 0x0029B8D6
	[XuiBindEvent("OnPress", "btnEula")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnEula_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_EulaWindow.Open(this.xui, GameManager.HasAcceptedLatestEula());
	}

	// Token: 0x06006B0C RID: 27404 RVA: 0x0029D6E8 File Offset: 0x0029B8E8
	[XuiBindEvent("OnPress", "btnBugReport")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBugReport_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_BugReportWindow.Open();
	}

	// Token: 0x06006B0D RID: 27405 RVA: 0x0029D6EF File Offset: 0x0029B8EF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void afterChangesSaved()
	{
		base.afterChangesSaved();
		this.languageSavedOptions();
	}

	// Token: 0x06006B0E RID: 27406 RVA: 0x0029D6FD File Offset: 0x0029B8FD
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.updateCrosshairPosition();
	}

	// Token: 0x06006B0F RID: 27407 RVA: 0x0029D70C File Offset: 0x0029B90C
	public override void OnOpen()
	{
		this.updateCrosshairElements();
		base.OnOpen();
	}

	// Token: 0x04005186 RID: 20870
	[XuiBindComponent("OptionLanguage", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionLanguage;

	// Token: 0x04005187 RID: 20871
	[XuiBindComponent("Language", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_OptionsGeneral.LanguageInfo> comboLanguage;

	// Token: 0x04005188 RID: 20872
	[PublicizedFrom(EAccessModifier.Private)]
	public bool languageChanged;

	// Token: 0x04005189 RID: 20873
	[XuiBindComponent("CrosshairsColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxInt comboCrosshairsColor;

	// Token: 0x0400518A RID: 20874
	[XuiBindComponent("CrosshairsSample", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite crosshairSample;

	// Token: 0x0400518B RID: 20875
	[XuiBindComponent("rangedCrosshairSample", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiView crosshairRangedSample;

	// Token: 0x0400518C RID: 20876
	[PublicizedFrom(EAccessModifier.Private)]
	public CrosshairDrawer crosshairDrawer;

	// Token: 0x0400518D RID: 20877
	[XuiBindComponent("OptionChatComms", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefBool optionChatComms;

	// Token: 0x0400518E RID: 20878
	[XuiBindComponent("OptionCrossplay", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefBool optionCrossplay;

	// Token: 0x0400518F RID: 20879
	public static string ID = "";

	// Token: 0x04005190 RID: 20880
	[XuiBindComponent("btnEula", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnEula;

	// Token: 0x04005191 RID: 20881
	[XuiBindComponent("btnBugReport", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBugReport;

	// Token: 0x02000DAF RID: 3503
	public readonly struct LanguageInfo : IComparable<XUiC_OptionsGeneral.LanguageInfo>, IEquatable<XUiC_OptionsGeneral.LanguageInfo>, IComparable
	{
		// Token: 0x06006B1A RID: 27418 RVA: 0x0029D94C File Offset: 0x0029BB4C
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

		// Token: 0x06006B1B RID: 27419 RVA: 0x0029D9A0 File Offset: 0x0029BBA0
		public override string ToString()
		{
			if (!(this.LanguageKey == ""))
			{
				return this.NameEnglish + " / " + this.NameNative;
			}
			return "-Auto-";
		}

		// Token: 0x06006B1C RID: 27420 RVA: 0x0029D9D0 File Offset: 0x0029BBD0
		public bool Equals(XUiC_OptionsGeneral.LanguageInfo _other)
		{
			return this.LanguageKey == _other.LanguageKey;
		}

		// Token: 0x06006B1D RID: 27421 RVA: 0x0029D9E4 File Offset: 0x0029BBE4
		public override bool Equals(object _obj)
		{
			if (_obj is XUiC_OptionsGeneral.LanguageInfo)
			{
				XUiC_OptionsGeneral.LanguageInfo languageInfo = (XUiC_OptionsGeneral.LanguageInfo)_obj;
				return this.Equals(languageInfo);
			}
			return false;
		}

		// Token: 0x06006B1E RID: 27422 RVA: 0x0029DA09 File Offset: 0x0029BC09
		public override int GetHashCode()
		{
			if (this.LanguageKey == null)
			{
				return 0;
			}
			return this.LanguageKey.GetHashCode();
		}

		// Token: 0x06006B1F RID: 27423 RVA: 0x0029DA20 File Offset: 0x0029BC20
		public int CompareTo(XUiC_OptionsGeneral.LanguageInfo _other)
		{
			return string.Compare(this.NameEnglish, _other.NameEnglish, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06006B20 RID: 27424 RVA: 0x0029DA34 File Offset: 0x0029BC34
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

		// Token: 0x04005192 RID: 20882
		public readonly string NameEnglish;

		// Token: 0x04005193 RID: 20883
		public readonly string NameNative;

		// Token: 0x04005194 RID: 20884
		public readonly string LanguageKey;
	}
}
