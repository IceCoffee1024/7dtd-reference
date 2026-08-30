using System;
using UnityEngine.Scripting;

// Token: 0x02000D98 RID: 3480
[Preserve]
public class XUiC_OptionEntryGamePrefBool : XUiC_OptionEntryGamePrefAbs
{
	// Token: 0x17000B53 RID: 2899
	// (get) Token: 0x060069D5 RID: 27093 RVA: 0x0029957C File Offset: 0x0029777C
	// (set) Token: 0x060069D6 RID: 27094 RVA: 0x00299584 File Offset: 0x00297784
	[XuiXmlAttribute("invert", false)]
	public bool Invert
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B54 RID: 2900
	// (get) Token: 0x060069D7 RID: 27095 RVA: 0x0029958D File Offset: 0x0029778D
	// (set) Token: 0x060069D8 RID: 27096 RVA: 0x00299595 File Offset: 0x00297795
	public bool SettingValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B55 RID: 2901
	// (get) Token: 0x060069D9 RID: 27097 RVA: 0x0029959E File Offset: 0x0029779E
	// (set) Token: 0x060069DA RID: 27098 RVA: 0x002995B1 File Offset: 0x002977B1
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

	// Token: 0x17000B56 RID: 2902
	// (get) Token: 0x060069DB RID: 27099 RVA: 0x002995BF File Offset: 0x002977BF
	// (set) Token: 0x060069DC RID: 27100 RVA: 0x002995D4 File Offset: 0x002977D4
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

	// Token: 0x17000B57 RID: 2903
	// (get) Token: 0x060069DD RID: 27101 RVA: 0x00299654 File Offset: 0x00297854
	public override bool IsChanged
	{
		get
		{
			return this.CurrentValue != this.SettingValue && (this.DoSaveOverride == null || this.DoSaveOverride());
		}
	}

	// Token: 0x17000B58 RID: 2904
	// (get) Token: 0x060069DE RID: 27102 RVA: 0x0029967B File Offset: 0x0029787B
	public override bool IsDefault
	{
		get
		{
			return this.gamePref == null || this.CurrentValue == this.getPrefDefault();
		}
	}

	// Token: 0x17000B59 RID: 2905
	// (get) Token: 0x060069DF RID: 27103 RVA: 0x00046EDA File Offset: 0x000450DA
	public override GamePrefs.EnumType expectedPrefType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return GamePrefs.EnumType.Bool;
		}
	}

	// Token: 0x060069E0 RID: 27104 RVA: 0x0029969A File Offset: 0x0029789A
	[PublicizedFrom(EAccessModifier.Private)]
	public bool getPrefDefault()
	{
		return (bool)GamePrefs.GetDefault(this.gamePref.Value) ^ this.Invert;
	}

	// Token: 0x060069E1 RID: 27105 RVA: 0x002996B8 File Offset: 0x002978B8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool getPrefValue()
	{
		return GamePrefs.GetBool(this.gamePref.Value) ^ this.Invert;
	}

	// Token: 0x060069E2 RID: 27106 RVA: 0x002996D1 File Offset: 0x002978D1
	[PublicizedFrom(EAccessModifier.Private)]
	public void setPrefValue(bool _value)
	{
		GamePrefs.Set(this.gamePref.Value, _value ^ this.Invert);
	}

	// Token: 0x060069E3 RID: 27107 RVA: 0x002996EC File Offset: 0x002978EC
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

	// Token: 0x060069E4 RID: 27108 RVA: 0x00299744 File Offset: 0x00297944
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

	// Token: 0x060069E5 RID: 27109 RVA: 0x00299770 File Offset: 0x00297970
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

	// Token: 0x060069E6 RID: 27110 RVA: 0x002997D6 File Offset: 0x002979D6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void applySelectionInternal()
	{
		this.SettingValue = this.CurrentValue;
		this.setPrefValue(this.SettingValue);
		XUiC_OptionEntryAbs.DebugLog(string.Format("SAVED BOOL FOR {0} TO {1}", this.gamePref, this.SettingValue));
	}

	// Token: 0x060069E7 RID: 27111 RVA: 0x00299818 File Offset: 0x00297A18
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

	// Token: 0x060069E8 RID: 27112 RVA: 0x0029988D File Offset: 0x00297A8D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void immediatelyApplyCurrentSelection()
	{
		this.setPrefValue(this.CurrentValue);
	}

	// Token: 0x04005125 RID: 20773
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxBool combo;

	// Token: 0x04005128 RID: 20776
	public XUiC_OptionEntryGamePrefBool.OverrideSettingValueDelegate OverrideSettingValue;

	// Token: 0x02000D99 RID: 3481
	// (Invoke) Token: 0x060069EB RID: 27115
	public delegate bool OverrideSettingValueDelegate(bool _settingValue, out bool _overrideValue);
}
