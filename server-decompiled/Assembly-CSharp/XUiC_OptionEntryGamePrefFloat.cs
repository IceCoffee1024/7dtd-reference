using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D9A RID: 3482
[Preserve]
public class XUiC_OptionEntryGamePrefFloat : XUiC_OptionEntryGamePrefAbs
{
	// Token: 0x17000B5A RID: 2906
	// (get) Token: 0x060069EE RID: 27118 RVA: 0x002998A3 File Offset: 0x00297AA3
	// (set) Token: 0x060069EF RID: 27119 RVA: 0x002998AB File Offset: 0x00297AAB
	public float SettingValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B5B RID: 2907
	// (get) Token: 0x060069F0 RID: 27120 RVA: 0x002998B4 File Offset: 0x00297AB4
	// (set) Token: 0x060069F1 RID: 27121 RVA: 0x002998D1 File Offset: 0x00297AD1
	public float CurrentValue
	{
		get
		{
			XUiC_ComboBoxFloat xuiC_ComboBoxFloat = this.combo;
			return (float)((xuiC_ComboBoxFloat != null) ? xuiC_ComboBoxFloat.Value : 0.0);
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.combo.Value = (double)value;
		}
	}

	// Token: 0x17000B5C RID: 2908
	// (get) Token: 0x060069F2 RID: 27122 RVA: 0x002998E0 File Offset: 0x00297AE0
	// (set) Token: 0x060069F3 RID: 27123 RVA: 0x002998F0 File Offset: 0x00297AF0
	public override object SelectedValue
	{
		get
		{
			return this.CurrentValue;
		}
		set
		{
			if (value is float)
			{
				float num = (float)value;
				this.CurrentValue = num;
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

	// Token: 0x17000B5D RID: 2909
	// (get) Token: 0x060069F4 RID: 27124 RVA: 0x00299969 File Offset: 0x00297B69
	public override bool IsChanged
	{
		get
		{
			return !Mathf.Approximately(this.CurrentValue, this.SettingValue);
		}
	}

	// Token: 0x17000B5E RID: 2910
	// (get) Token: 0x060069F5 RID: 27125 RVA: 0x0029997F File Offset: 0x00297B7F
	public override bool IsDefault
	{
		get
		{
			return this.gamePref == null || Mathf.Approximately(this.CurrentValue, this.getPrefDefault());
		}
	}

	// Token: 0x17000B5F RID: 2911
	// (get) Token: 0x060069F6 RID: 27126 RVA: 0x00020021 File Offset: 0x0001E221
	public override GamePrefs.EnumType expectedPrefType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return GamePrefs.EnumType.Float;
		}
	}

	// Token: 0x060069F7 RID: 27127 RVA: 0x002999A1 File Offset: 0x00297BA1
	[PublicizedFrom(EAccessModifier.Private)]
	public float getPrefDefault()
	{
		return (float)GamePrefs.GetDefault(this.gamePref.Value);
	}

	// Token: 0x060069F8 RID: 27128 RVA: 0x002999B8 File Offset: 0x00297BB8
	[PublicizedFrom(EAccessModifier.Private)]
	public float getPrefValue()
	{
		return GamePrefs.GetFloat(this.gamePref.Value);
	}

	// Token: 0x060069F9 RID: 27129 RVA: 0x002999CA File Offset: 0x00297BCA
	[PublicizedFrom(EAccessModifier.Private)]
	public void setPrefValue(float _value)
	{
		GamePrefs.Set(this.gamePref.Value, _value);
	}

	// Token: 0x060069FA RID: 27130 RVA: 0x002999E0 File Offset: 0x00297BE0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void initCurrentValue()
	{
		if (this.gamePref == null)
		{
			return;
		}
		this.CurrentValue = (this.SettingValue = this.getPrefValue());
		XUiC_OptionEntryAbs.DebugLog(string.Format("GOT CURRENT FLOAT FOR {0}: {1}", this.gamePref, this.SettingValue));
	}

	// Token: 0x060069FB RID: 27131 RVA: 0x00299A38 File Offset: 0x00297C38
	public override void DiscardCurrentChange()
	{
		if (this.gamePref == null)
		{
			return;
		}
		this.CurrentValue = this.SettingValue;
		if (base.ApplyImmediately)
		{
			this.setPrefValue(this.CurrentValue);
		}
		XUiC_OptionEntryAbs.DebugLog(string.Format("DISCARDED FLOAT FOR {0} TO {1}", this.gamePref, this.SettingValue));
	}

	// Token: 0x060069FC RID: 27132 RVA: 0x00299A98 File Offset: 0x00297C98
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void applySelectionInternal()
	{
		this.SettingValue = this.CurrentValue;
		this.setPrefValue(this.SettingValue);
		XUiC_OptionEntryAbs.DebugLog(string.Format("SAVED FLOAT FOR {0} TO {1}", this.gamePref, this.SettingValue));
	}

	// Token: 0x060069FD RID: 27133 RVA: 0x00299AD8 File Offset: 0x00297CD8
	public override void ResetToDefault()
	{
		if (!base.ApplyDefaults || this.gamePref == null)
		{
			return;
		}
		this.CurrentValue = this.getPrefDefault();
		if (base.ApplyImmediately)
		{
			this.setPrefValue(this.CurrentValue);
		}
		XUiC_OptionEntryAbs.DebugLog(string.Format("RESET FLOAT FOR {0} TO {1}", this.gamePref, this.CurrentValue));
		base.invokeValueChanged();
	}

	// Token: 0x060069FE RID: 27134 RVA: 0x00299B46 File Offset: 0x00297D46
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void immediatelyApplyCurrentSelection()
	{
		this.setPrefValue(this.CurrentValue);
	}

	// Token: 0x04005129 RID: 20777
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxFloat combo;
}
