using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DA9 RID: 3497
[Preserve]
public class XUiC_OptionEntryGamePrefFloat : XUiC_OptionEntryGamePrefAbs
{
	// Token: 0x17000B75 RID: 2933
	// (get) Token: 0x060069E6 RID: 27110 RVA: 0x0029CC2F File Offset: 0x0029AE2F
	// (set) Token: 0x060069E7 RID: 27111 RVA: 0x0029CC37 File Offset: 0x0029AE37
	public float SettingValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B76 RID: 2934
	// (get) Token: 0x060069E8 RID: 27112 RVA: 0x0029CC40 File Offset: 0x0029AE40
	// (set) Token: 0x060069E9 RID: 27113 RVA: 0x0029CC5D File Offset: 0x0029AE5D
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

	// Token: 0x17000B77 RID: 2935
	// (get) Token: 0x060069EA RID: 27114 RVA: 0x0029CC6C File Offset: 0x0029AE6C
	// (set) Token: 0x060069EB RID: 27115 RVA: 0x0029CC7C File Offset: 0x0029AE7C
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

	// Token: 0x17000B78 RID: 2936
	// (get) Token: 0x060069EC RID: 27116 RVA: 0x0029CCF5 File Offset: 0x0029AEF5
	public override bool IsChanged
	{
		get
		{
			return !Mathf.Approximately(this.CurrentValue, this.SettingValue);
		}
	}

	// Token: 0x17000B79 RID: 2937
	// (get) Token: 0x060069ED RID: 27117 RVA: 0x0029CD0B File Offset: 0x0029AF0B
	public override bool IsDefault
	{
		get
		{
			return this.gamePref == null || Mathf.Approximately(this.CurrentValue, this.getPrefDefault());
		}
	}

	// Token: 0x17000B7A RID: 2938
	// (get) Token: 0x060069EE RID: 27118 RVA: 0x00020119 File Offset: 0x0001E319
	public override GamePrefs.EnumType expectedPrefType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return GamePrefs.EnumType.Float;
		}
	}

	// Token: 0x060069EF RID: 27119 RVA: 0x0029CD2D File Offset: 0x0029AF2D
	[PublicizedFrom(EAccessModifier.Private)]
	public float getPrefDefault()
	{
		return (float)GamePrefs.GetDefault(this.gamePref.Value);
	}

	// Token: 0x060069F0 RID: 27120 RVA: 0x0029CD44 File Offset: 0x0029AF44
	[PublicizedFrom(EAccessModifier.Private)]
	public float getPrefValue()
	{
		return GamePrefs.GetFloat(this.gamePref.Value);
	}

	// Token: 0x060069F1 RID: 27121 RVA: 0x0029CD56 File Offset: 0x0029AF56
	[PublicizedFrom(EAccessModifier.Private)]
	public void setPrefValue(float _value)
	{
		GamePrefs.Set(this.gamePref.Value, _value);
	}

	// Token: 0x060069F2 RID: 27122 RVA: 0x0029CD6C File Offset: 0x0029AF6C
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

	// Token: 0x060069F3 RID: 27123 RVA: 0x0029CDC4 File Offset: 0x0029AFC4
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

	// Token: 0x060069F4 RID: 27124 RVA: 0x0029CE24 File Offset: 0x0029B024
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void applySelectionInternal()
	{
		this.SettingValue = this.CurrentValue;
		this.setPrefValue(this.SettingValue);
		XUiC_OptionEntryAbs.DebugLog(string.Format("SAVED FLOAT FOR {0} TO {1}", this.gamePref, this.SettingValue));
	}

	// Token: 0x060069F5 RID: 27125 RVA: 0x0029CE64 File Offset: 0x0029B064
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

	// Token: 0x060069F6 RID: 27126 RVA: 0x0029CED2 File Offset: 0x0029B0D2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void immediatelyApplyCurrentSelection()
	{
		this.setPrefValue(this.CurrentValue);
	}

	// Token: 0x04005160 RID: 20832
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxFloat combo;
}
