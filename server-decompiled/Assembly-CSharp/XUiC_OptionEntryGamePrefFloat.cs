using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DB4 RID: 3508
[Preserve]
public class XUiC_OptionEntryGamePrefFloat : XUiC_OptionEntryGamePrefAbs
{
	// Token: 0x17000B7B RID: 2939
	// (get) Token: 0x06006A0B RID: 27147 RVA: 0x0029E157 File Offset: 0x0029C357
	// (set) Token: 0x06006A0C RID: 27148 RVA: 0x0029E15F File Offset: 0x0029C35F
	public float SettingValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B7C RID: 2940
	// (get) Token: 0x06006A0D RID: 27149 RVA: 0x0029E168 File Offset: 0x0029C368
	// (set) Token: 0x06006A0E RID: 27150 RVA: 0x0029E185 File Offset: 0x0029C385
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

	// Token: 0x17000B7D RID: 2941
	// (get) Token: 0x06006A0F RID: 27151 RVA: 0x0029E194 File Offset: 0x0029C394
	// (set) Token: 0x06006A10 RID: 27152 RVA: 0x0029E1A4 File Offset: 0x0029C3A4
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

	// Token: 0x17000B7E RID: 2942
	// (get) Token: 0x06006A11 RID: 27153 RVA: 0x0029E21D File Offset: 0x0029C41D
	public override bool IsChanged
	{
		get
		{
			return !Mathf.Approximately(this.CurrentValue, this.SettingValue);
		}
	}

	// Token: 0x17000B7F RID: 2943
	// (get) Token: 0x06006A12 RID: 27154 RVA: 0x0029E233 File Offset: 0x0029C433
	public override bool IsDefault
	{
		get
		{
			return this.gamePref == null || Mathf.Approximately(this.CurrentValue, this.getPrefDefault());
		}
	}

	// Token: 0x17000B80 RID: 2944
	// (get) Token: 0x06006A13 RID: 27155 RVA: 0x00020119 File Offset: 0x0001E319
	public override GamePrefs.EnumType expectedPrefType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return GamePrefs.EnumType.Float;
		}
	}

	// Token: 0x06006A14 RID: 27156 RVA: 0x0029E255 File Offset: 0x0029C455
	[PublicizedFrom(EAccessModifier.Private)]
	public float getPrefDefault()
	{
		return (float)GamePrefs.GetDefault(this.gamePref.Value);
	}

	// Token: 0x06006A15 RID: 27157 RVA: 0x0029E26C File Offset: 0x0029C46C
	[PublicizedFrom(EAccessModifier.Private)]
	public float getPrefValue()
	{
		return GamePrefs.GetFloat(this.gamePref.Value);
	}

	// Token: 0x06006A16 RID: 27158 RVA: 0x0029E27E File Offset: 0x0029C47E
	[PublicizedFrom(EAccessModifier.Private)]
	public void setPrefValue(float _value)
	{
		GamePrefs.Set(this.gamePref.Value, _value);
	}

	// Token: 0x06006A17 RID: 27159 RVA: 0x0029E294 File Offset: 0x0029C494
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

	// Token: 0x06006A18 RID: 27160 RVA: 0x0029E2EC File Offset: 0x0029C4EC
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

	// Token: 0x06006A19 RID: 27161 RVA: 0x0029E34C File Offset: 0x0029C54C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void applySelectionInternal()
	{
		this.SettingValue = this.CurrentValue;
		this.setPrefValue(this.SettingValue);
		XUiC_OptionEntryAbs.DebugLog(string.Format("SAVED FLOAT FOR {0} TO {1}", this.gamePref, this.SettingValue));
	}

	// Token: 0x06006A1A RID: 27162 RVA: 0x0029E38C File Offset: 0x0029C58C
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

	// Token: 0x06006A1B RID: 27163 RVA: 0x0029E3FA File Offset: 0x0029C5FA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void immediatelyApplyCurrentSelection()
	{
		this.setPrefValue(this.CurrentValue);
	}

	// Token: 0x04005185 RID: 20869
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxFloat combo;
}
