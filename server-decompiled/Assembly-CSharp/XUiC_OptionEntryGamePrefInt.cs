using System;
using UnityEngine.Scripting;

// Token: 0x02000D9B RID: 3483
[Preserve]
public class XUiC_OptionEntryGamePrefInt : XUiC_OptionEntryGamePrefAbs
{
	// Token: 0x17000B60 RID: 2912
	// (get) Token: 0x06006A00 RID: 27136 RVA: 0x00299B54 File Offset: 0x00297D54
	// (set) Token: 0x06006A01 RID: 27137 RVA: 0x00299B5C File Offset: 0x00297D5C
	public int SettingValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B61 RID: 2913
	// (get) Token: 0x06006A02 RID: 27138 RVA: 0x00299B65 File Offset: 0x00297D65
	// (set) Token: 0x06006A03 RID: 27139 RVA: 0x00299B7B File Offset: 0x00297D7B
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

	// Token: 0x17000B62 RID: 2914
	// (get) Token: 0x06006A04 RID: 27140 RVA: 0x00299B8A File Offset: 0x00297D8A
	// (set) Token: 0x06006A05 RID: 27141 RVA: 0x00299B98 File Offset: 0x00297D98
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

	// Token: 0x17000B63 RID: 2915
	// (get) Token: 0x06006A06 RID: 27142 RVA: 0x00299C11 File Offset: 0x00297E11
	public override bool IsChanged
	{
		get
		{
			return this.CurrentValue != this.SettingValue;
		}
	}

	// Token: 0x17000B64 RID: 2916
	// (get) Token: 0x06006A07 RID: 27143 RVA: 0x00299C24 File Offset: 0x00297E24
	public override bool IsDefault
	{
		get
		{
			return this.gamePref == null || this.CurrentValue == this.getPrefDefault();
		}
	}

	// Token: 0x17000B65 RID: 2917
	// (get) Token: 0x06006A08 RID: 27144 RVA: 0x00010E62 File Offset: 0x0000F062
	public override GamePrefs.EnumType expectedPrefType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return GamePrefs.EnumType.Int;
		}
	}

	// Token: 0x06006A09 RID: 27145 RVA: 0x00299C43 File Offset: 0x00297E43
	[PublicizedFrom(EAccessModifier.Private)]
	public int getPrefDefault()
	{
		return (int)GamePrefs.GetDefault(this.gamePref.Value);
	}

	// Token: 0x06006A0A RID: 27146 RVA: 0x00299C5A File Offset: 0x00297E5A
	[PublicizedFrom(EAccessModifier.Private)]
	public int getPrefValue()
	{
		return GamePrefs.GetInt(this.gamePref.Value);
	}

	// Token: 0x06006A0B RID: 27147 RVA: 0x00299C6C File Offset: 0x00297E6C
	[PublicizedFrom(EAccessModifier.Private)]
	public void setPrefValue(int _value)
	{
		GamePrefs.Set(this.gamePref.Value, _value);
	}

	// Token: 0x06006A0C RID: 27148 RVA: 0x00299C80 File Offset: 0x00297E80
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

	// Token: 0x06006A0D RID: 27149 RVA: 0x00299CD8 File Offset: 0x00297ED8
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

	// Token: 0x06006A0E RID: 27150 RVA: 0x00299D38 File Offset: 0x00297F38
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void applySelectionInternal()
	{
		this.SettingValue = this.CurrentValue;
		this.setPrefValue(this.SettingValue);
		XUiC_OptionEntryAbs.DebugLog(string.Format("SAVED INT FOR {0} TO {1}", this.gamePref, this.SettingValue));
	}

	// Token: 0x06006A0F RID: 27151 RVA: 0x00299D78 File Offset: 0x00297F78
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

	// Token: 0x06006A10 RID: 27152 RVA: 0x00299DE6 File Offset: 0x00297FE6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void immediatelyApplyCurrentSelection()
	{
		this.setPrefValue(this.CurrentValue);
	}

	// Token: 0x0400512B RID: 20779
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxInt combo;
}
