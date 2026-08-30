using System;
using UnityEngine.Scripting;

// Token: 0x02000D84 RID: 3460
[Preserve]
public class XUiC_OptionEntryGamePrefBool : XUiC_OptionEntryGamePrefAbs
{
	// Token: 0x17000B3B RID: 2875
	// (get) Token: 0x06006939 RID: 26937 RVA: 0x00296428 File Offset: 0x00294628
	// (set) Token: 0x0600693A RID: 26938 RVA: 0x00296430 File Offset: 0x00294630
	[XuiXmlAttribute("invert", false)]
	public bool Invert
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B3C RID: 2876
	// (get) Token: 0x0600693B RID: 26939 RVA: 0x00296439 File Offset: 0x00294639
	// (set) Token: 0x0600693C RID: 26940 RVA: 0x00296441 File Offset: 0x00294641
	public bool SettingValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B3D RID: 2877
	// (get) Token: 0x0600693D RID: 26941 RVA: 0x0029644A File Offset: 0x0029464A
	// (set) Token: 0x0600693E RID: 26942 RVA: 0x0029645D File Offset: 0x0029465D
	public bool CurrentValue
	{
		get
		{
			XUiC_ComboBoxBool xuiC_ComboBoxBool = this.combo;
			return xuiC_ComboBoxBool != null && xuiC_ComboBoxBool.Value;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.combo.Value = value;
		}
	}

	// Token: 0x17000B3E RID: 2878
	// (get) Token: 0x0600693F RID: 26943 RVA: 0x0029646B File Offset: 0x0029466B
	// (set) Token: 0x06006940 RID: 26944 RVA: 0x00296480 File Offset: 0x00294680
	public override object SelectedValue
	{
		get
		{
			return this.CurrentValue ^ this.Invert;
		}
		set
		{
			if (value is bool)
			{
				bool flag = (bool)value;
				this.CurrentValue = flag ^ this.Invert;
				base.invokeValueChanged();
				return;
			}
			Log.Error(string.Concat(new string[]
			{
				"[XUi] ",
				base.GetType().Name,
				".SelectedValue: Given value is not a ",
				this.expectedPrefType.ToStringCached<GamePrefs.EnumType>(),
				". Hierarchy: ",
				base.GetXuiHierarchy()
			}));
		}
	}

	// Token: 0x17000B3F RID: 2879
	// (get) Token: 0x06006941 RID: 26945 RVA: 0x00296500 File Offset: 0x00294700
	public override bool IsChanged
	{
		get
		{
			return this.CurrentValue != this.SettingValue && (this.DoSaveOverride == null || this.DoSaveOverride());
		}
	}

	// Token: 0x17000B40 RID: 2880
	// (get) Token: 0x06006942 RID: 26946 RVA: 0x00296527 File Offset: 0x00294727
	public override bool IsDefault
	{
		get
		{
			return this.gamePref == null || this.CurrentValue == this.getPrefDefault();
		}
	}

	// Token: 0x17000B41 RID: 2881
	// (get) Token: 0x06006943 RID: 26947 RVA: 0x00046C7A File Offset: 0x00044E7A
	public override GamePrefs.EnumType expectedPrefType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return GamePrefs.EnumType.Bool;
		}
	}

	// Token: 0x06006944 RID: 26948 RVA: 0x00296546 File Offset: 0x00294746
	[PublicizedFrom(EAccessModifier.Private)]
	public bool getPrefDefault()
	{
		return (bool)GamePrefs.GetDefault(this.gamePref.Value) ^ this.Invert;
	}

	// Token: 0x06006945 RID: 26949 RVA: 0x00296564 File Offset: 0x00294764
	[PublicizedFrom(EAccessModifier.Private)]
	public bool getPrefValue()
	{
		return GamePrefs.GetBool(this.gamePref.Value) ^ this.Invert;
	}

	// Token: 0x06006946 RID: 26950 RVA: 0x0029657D File Offset: 0x0029477D
	[PublicizedFrom(EAccessModifier.Private)]
	public void setPrefValue(bool _value)
	{
		GamePrefs.Set(this.gamePref.Value, _value ^ this.Invert);
	}

	// Token: 0x06006947 RID: 26951 RVA: 0x00296598 File Offset: 0x00294798
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void initCurrentValue()
	{
		if (this.gamePref == null)
		{
			return;
		}
		this.CurrentValue = (this.SettingValue = this.getPrefValue());
		XUiC_OptionEntryAbs.DebugLog(string.Format("GOT CURRENT BOOL FOR {0}: {1}", this.gamePref, this.SettingValue));
	}

	// Token: 0x06006948 RID: 26952 RVA: 0x002965F0 File Offset: 0x002947F0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool applyValueOverride(bool _value)
	{
		bool flag;
		if (this.OverrideSettingValue != null && this.OverrideSettingValue(_value, out flag))
		{
			_value = flag;
		}
		return _value;
	}

	// Token: 0x06006949 RID: 26953 RVA: 0x0029661C File Offset: 0x0029481C
	public override void DiscardCurrentChange()
	{
		if (this.gamePref == null)
		{
			return;
		}
		this.CurrentValue = this.applyValueOverride(this.SettingValue);
		if (base.ApplyImmediately)
		{
			this.setPrefValue(this.CurrentValue);
		}
		XUiC_OptionEntryAbs.DebugLog(string.Format("DISCARDED BOOL FOR {0} TO {1}", this.gamePref, this.SettingValue));
	}

	// Token: 0x0600694A RID: 26954 RVA: 0x00296682 File Offset: 0x00294882
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void applySelectionInternal()
	{
		this.SettingValue = this.CurrentValue;
		this.setPrefValue(this.SettingValue);
		XUiC_OptionEntryAbs.DebugLog(string.Format("SAVED BOOL FOR {0} TO {1}", this.gamePref, this.SettingValue));
	}

	// Token: 0x0600694B RID: 26955 RVA: 0x002966C4 File Offset: 0x002948C4
	public override void ResetToDefault()
	{
		if (!base.ApplyDefaults || this.gamePref == null)
		{
			return;
		}
		this.CurrentValue = this.getPrefDefault() ^ this.Invert;
		if (base.ApplyImmediately)
		{
			this.setPrefValue(this.CurrentValue);
		}
		XUiC_OptionEntryAbs.DebugLog(string.Format("RESET BOOL FOR {0} TO {1}", this.gamePref, this.CurrentValue));
		base.invokeValueChanged();
	}

	// Token: 0x0600694C RID: 26956 RVA: 0x00296739 File Offset: 0x00294939
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void immediatelyApplyCurrentSelection()
	{
		this.setPrefValue(this.CurrentValue);
	}

	// Token: 0x04005083 RID: 20611
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxBool combo;

	// Token: 0x04005086 RID: 20614
	public XUiC_OptionEntryGamePrefBool.OverrideSettingValueDelegate OverrideSettingValue;

	// Token: 0x02000D85 RID: 3461
	// (Invoke) Token: 0x0600694F RID: 26959
	public delegate bool OverrideSettingValueDelegate(bool _settingValue, out bool _overrideValue);
}
