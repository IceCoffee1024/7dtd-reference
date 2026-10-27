using System;
using UnityEngine.Scripting;

// Token: 0x02000DB2 RID: 3506
[Preserve]
public class XUiC_OptionEntryGamePrefBool : XUiC_OptionEntryGamePrefAbs
{
	// Token: 0x17000B74 RID: 2932
	// (get) Token: 0x060069F2 RID: 27122 RVA: 0x0029DE30 File Offset: 0x0029C030
	// (set) Token: 0x060069F3 RID: 27123 RVA: 0x0029DE38 File Offset: 0x0029C038
	[XuiXmlAttribute("invert", false)]
	public bool Invert
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B75 RID: 2933
	// (get) Token: 0x060069F4 RID: 27124 RVA: 0x0029DE41 File Offset: 0x0029C041
	// (set) Token: 0x060069F5 RID: 27125 RVA: 0x0029DE49 File Offset: 0x0029C049
	public bool SettingValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B76 RID: 2934
	// (get) Token: 0x060069F6 RID: 27126 RVA: 0x0029DE52 File Offset: 0x0029C052
	// (set) Token: 0x060069F7 RID: 27127 RVA: 0x0029DE65 File Offset: 0x0029C065
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

	// Token: 0x17000B77 RID: 2935
	// (get) Token: 0x060069F8 RID: 27128 RVA: 0x0029DE73 File Offset: 0x0029C073
	// (set) Token: 0x060069F9 RID: 27129 RVA: 0x0029DE88 File Offset: 0x0029C088
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

	// Token: 0x17000B78 RID: 2936
	// (get) Token: 0x060069FA RID: 27130 RVA: 0x0029DF08 File Offset: 0x0029C108
	public override bool IsChanged
	{
		get
		{
			return this.CurrentValue != this.SettingValue && (this.DoSaveOverride == null || this.DoSaveOverride());
		}
	}

	// Token: 0x17000B79 RID: 2937
	// (get) Token: 0x060069FB RID: 27131 RVA: 0x0029DF2F File Offset: 0x0029C12F
	public override bool IsDefault
	{
		get
		{
			return this.gamePref == null || this.CurrentValue == this.getPrefDefault();
		}
	}

	// Token: 0x17000B7A RID: 2938
	// (get) Token: 0x060069FC RID: 27132 RVA: 0x00047076 File Offset: 0x00045276
	public override GamePrefs.EnumType expectedPrefType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return GamePrefs.EnumType.Bool;
		}
	}

	// Token: 0x060069FD RID: 27133 RVA: 0x0029DF4E File Offset: 0x0029C14E
	[PublicizedFrom(EAccessModifier.Private)]
	public bool getPrefDefault()
	{
		return (bool)GamePrefs.GetDefault(this.gamePref.Value) ^ this.Invert;
	}

	// Token: 0x060069FE RID: 27134 RVA: 0x0029DF6C File Offset: 0x0029C16C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool getPrefValue()
	{
		return GamePrefs.GetBool(this.gamePref.Value) ^ this.Invert;
	}

	// Token: 0x060069FF RID: 27135 RVA: 0x0029DF85 File Offset: 0x0029C185
	[PublicizedFrom(EAccessModifier.Private)]
	public void setPrefValue(bool _value)
	{
		GamePrefs.Set(this.gamePref.Value, _value ^ this.Invert);
	}

	// Token: 0x06006A00 RID: 27136 RVA: 0x0029DFA0 File Offset: 0x0029C1A0
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

	// Token: 0x06006A01 RID: 27137 RVA: 0x0029DFF8 File Offset: 0x0029C1F8
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

	// Token: 0x06006A02 RID: 27138 RVA: 0x0029E024 File Offset: 0x0029C224
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

	// Token: 0x06006A03 RID: 27139 RVA: 0x0029E08A File Offset: 0x0029C28A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void applySelectionInternal()
	{
		this.SettingValue = this.CurrentValue;
		this.setPrefValue(this.SettingValue);
		XUiC_OptionEntryAbs.DebugLog(string.Format("SAVED BOOL FOR {0} TO {1}", this.gamePref, this.SettingValue));
	}

	// Token: 0x06006A04 RID: 27140 RVA: 0x0029E0CC File Offset: 0x0029C2CC
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

	// Token: 0x06006A05 RID: 27141 RVA: 0x0029E141 File Offset: 0x0029C341
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void immediatelyApplyCurrentSelection()
	{
		this.setPrefValue(this.CurrentValue);
	}

	// Token: 0x04005181 RID: 20865
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxBool combo;

	// Token: 0x04005184 RID: 20868
	public XUiC_OptionEntryGamePrefBool.OverrideSettingValueDelegate OverrideSettingValue;

	// Token: 0x02000DB3 RID: 3507
	// (Invoke) Token: 0x06006A08 RID: 27144
	public delegate bool OverrideSettingValueDelegate(bool _settingValue, out bool _overrideValue);
}
