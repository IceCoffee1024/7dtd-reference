using System;
using UnityEngine.Scripting;

// Token: 0x02000DAA RID: 3498
[Preserve]
public class XUiC_OptionEntryGamePrefInt : XUiC_OptionEntryGamePrefAbs
{
	// Token: 0x17000B7B RID: 2939
	// (get) Token: 0x060069F8 RID: 27128 RVA: 0x0029CEE0 File Offset: 0x0029B0E0
	// (set) Token: 0x060069F9 RID: 27129 RVA: 0x0029CEE8 File Offset: 0x0029B0E8
	public int SettingValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B7C RID: 2940
	// (get) Token: 0x060069FA RID: 27130 RVA: 0x0029CEF1 File Offset: 0x0029B0F1
	// (set) Token: 0x060069FB RID: 27131 RVA: 0x0029CF07 File Offset: 0x0029B107
	public int CurrentValue
	{
		get
		{
			XUiC_ComboBoxInt xuiC_ComboBoxInt = this.combo;
			return (int)((xuiC_ComboBoxInt != null) ? xuiC_ComboBoxInt.Value : 0L);
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.combo.Value = (long)value;
		}
	}

	// Token: 0x17000B7D RID: 2941
	// (get) Token: 0x060069FC RID: 27132 RVA: 0x0029CF16 File Offset: 0x0029B116
	// (set) Token: 0x060069FD RID: 27133 RVA: 0x0029CF24 File Offset: 0x0029B124
	public override object SelectedValue
	{
		get
		{
			return this.CurrentValue;
		}
		set
		{
			if (value is int)
			{
				int num = (int)value;
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

	// Token: 0x17000B7E RID: 2942
	// (get) Token: 0x060069FE RID: 27134 RVA: 0x0029CF9D File Offset: 0x0029B19D
	public override bool IsChanged
	{
		get
		{
			return this.CurrentValue != this.SettingValue;
		}
	}

	// Token: 0x17000B7F RID: 2943
	// (get) Token: 0x060069FF RID: 27135 RVA: 0x0029CFB0 File Offset: 0x0029B1B0
	public override bool IsDefault
	{
		get
		{
			return this.gamePref == null || this.CurrentValue == this.getPrefDefault();
		}
	}

	// Token: 0x17000B80 RID: 2944
	// (get) Token: 0x06006A00 RID: 27136 RVA: 0x00010E62 File Offset: 0x0000F062
	public override GamePrefs.EnumType expectedPrefType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return GamePrefs.EnumType.Int;
		}
	}

	// Token: 0x06006A01 RID: 27137 RVA: 0x0029CFCF File Offset: 0x0029B1CF
	[PublicizedFrom(EAccessModifier.Private)]
	public int getPrefDefault()
	{
		return (int)GamePrefs.GetDefault(this.gamePref.Value);
	}

	// Token: 0x06006A02 RID: 27138 RVA: 0x0029CFE6 File Offset: 0x0029B1E6
	[PublicizedFrom(EAccessModifier.Private)]
	public int getPrefValue()
	{
		return GamePrefs.GetInt(this.gamePref.Value);
	}

	// Token: 0x06006A03 RID: 27139 RVA: 0x0029CFF8 File Offset: 0x0029B1F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void setPrefValue(int _value)
	{
		GamePrefs.Set(this.gamePref.Value, _value);
	}

	// Token: 0x06006A04 RID: 27140 RVA: 0x0029D00C File Offset: 0x0029B20C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void initCurrentValue()
	{
		if (this.gamePref == null)
		{
			return;
		}
		this.CurrentValue = (this.SettingValue = this.getPrefValue());
		XUiC_OptionEntryAbs.DebugLog(string.Format("GOT CURRENT INT FOR {0}: {1}", this.gamePref, this.SettingValue));
	}

	// Token: 0x06006A05 RID: 27141 RVA: 0x0029D064 File Offset: 0x0029B264
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
		XUiC_OptionEntryAbs.DebugLog(string.Format("DISCARDED INT FOR {0} TO {1}", this.gamePref, this.SettingValue));
	}

	// Token: 0x06006A06 RID: 27142 RVA: 0x0029D0C4 File Offset: 0x0029B2C4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void applySelectionInternal()
	{
		this.SettingValue = this.CurrentValue;
		this.setPrefValue(this.SettingValue);
		XUiC_OptionEntryAbs.DebugLog(string.Format("SAVED INT FOR {0} TO {1}", this.gamePref, this.SettingValue));
	}

	// Token: 0x06006A07 RID: 27143 RVA: 0x0029D104 File Offset: 0x0029B304
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
		XUiC_OptionEntryAbs.DebugLog(string.Format("RESET INT FOR {0} TO {1}", this.gamePref, this.CurrentValue));
		base.invokeValueChanged();
	}

	// Token: 0x06006A08 RID: 27144 RVA: 0x0029D172 File Offset: 0x0029B372
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void immediatelyApplyCurrentSelection()
	{
		this.setPrefValue(this.CurrentValue);
	}

	// Token: 0x04005162 RID: 20834
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxInt combo;
}
