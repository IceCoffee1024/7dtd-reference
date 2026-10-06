using System;
using UnityEngine.Scripting;

// Token: 0x02000DA7 RID: 3495
[Preserve]
public class XUiC_OptionEntryGamePrefBool : XUiC_OptionEntryGamePrefAbs
{
	// Token: 0x17000B6E RID: 2926
	// (get) Token: 0x060069CD RID: 27085 RVA: 0x0029C908 File Offset: 0x0029AB08
	// (set) Token: 0x060069CE RID: 27086 RVA: 0x0029C910 File Offset: 0x0029AB10
	[XuiXmlAttribute("invert", false)]
	public bool Invert
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B6F RID: 2927
	// (get) Token: 0x060069CF RID: 27087 RVA: 0x0029C919 File Offset: 0x0029AB19
	// (set) Token: 0x060069D0 RID: 27088 RVA: 0x0029C921 File Offset: 0x0029AB21
	public bool SettingValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B70 RID: 2928
	// (get) Token: 0x060069D1 RID: 27089 RVA: 0x0029C92A File Offset: 0x0029AB2A
	// (set) Token: 0x060069D2 RID: 27090 RVA: 0x0029C93D File Offset: 0x0029AB3D
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

	// Token: 0x17000B71 RID: 2929
	// (get) Token: 0x060069D3 RID: 27091 RVA: 0x0029C94B File Offset: 0x0029AB4B
	// (set) Token: 0x060069D4 RID: 27092 RVA: 0x0029C960 File Offset: 0x0029AB60
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

	// Token: 0x17000B72 RID: 2930
	// (get) Token: 0x060069D5 RID: 27093 RVA: 0x0029C9E0 File Offset: 0x0029ABE0
	public override bool IsChanged
	{
		get
		{
			return this.CurrentValue != this.SettingValue && (this.DoSaveOverride == null || this.DoSaveOverride());
		}
	}

	// Token: 0x17000B73 RID: 2931
	// (get) Token: 0x060069D6 RID: 27094 RVA: 0x0029CA07 File Offset: 0x0029AC07
	public override bool IsDefault
	{
		get
		{
			return this.gamePref == null || this.CurrentValue == this.getPrefDefault();
		}
	}

	// Token: 0x17000B74 RID: 2932
	// (get) Token: 0x060069D7 RID: 27095 RVA: 0x000470B6 File Offset: 0x000452B6
	public override GamePrefs.EnumType expectedPrefType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return GamePrefs.EnumType.Bool;
		}
	}

	// Token: 0x060069D8 RID: 27096 RVA: 0x0029CA26 File Offset: 0x0029AC26
	[PublicizedFrom(EAccessModifier.Private)]
	public bool getPrefDefault()
	{
		return (bool)GamePrefs.GetDefault(this.gamePref.Value) ^ this.Invert;
	}

	// Token: 0x060069D9 RID: 27097 RVA: 0x0029CA44 File Offset: 0x0029AC44
	[PublicizedFrom(EAccessModifier.Private)]
	public bool getPrefValue()
	{
		return GamePrefs.GetBool(this.gamePref.Value) ^ this.Invert;
	}

	// Token: 0x060069DA RID: 27098 RVA: 0x0029CA5D File Offset: 0x0029AC5D
	[PublicizedFrom(EAccessModifier.Private)]
	public void setPrefValue(bool _value)
	{
		GamePrefs.Set(this.gamePref.Value, _value ^ this.Invert);
	}

	// Token: 0x060069DB RID: 27099 RVA: 0x0029CA78 File Offset: 0x0029AC78
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

	// Token: 0x060069DC RID: 27100 RVA: 0x0029CAD0 File Offset: 0x0029ACD0
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

	// Token: 0x060069DD RID: 27101 RVA: 0x0029CAFC File Offset: 0x0029ACFC
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

	// Token: 0x060069DE RID: 27102 RVA: 0x0029CB62 File Offset: 0x0029AD62
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void applySelectionInternal()
	{
		this.SettingValue = this.CurrentValue;
		this.setPrefValue(this.SettingValue);
		XUiC_OptionEntryAbs.DebugLog(string.Format("SAVED BOOL FOR {0} TO {1}", this.gamePref, this.SettingValue));
	}

	// Token: 0x060069DF RID: 27103 RVA: 0x0029CBA4 File Offset: 0x0029ADA4
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

	// Token: 0x060069E0 RID: 27104 RVA: 0x0029CC19 File Offset: 0x0029AE19
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void immediatelyApplyCurrentSelection()
	{
		this.setPrefValue(this.CurrentValue);
	}

	// Token: 0x0400515C RID: 20828
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxBool combo;

	// Token: 0x0400515F RID: 20831
	public XUiC_OptionEntryGamePrefBool.OverrideSettingValueDelegate OverrideSettingValue;

	// Token: 0x02000DA8 RID: 3496
	// (Invoke) Token: 0x060069E3 RID: 27107
	public delegate bool OverrideSettingValueDelegate(bool _settingValue, out bool _overrideValue);
}
