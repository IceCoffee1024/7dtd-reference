using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D9C RID: 3484
[Preserve]
public class XUiC_OptionEntryGamePrefIntIndex : XUiC_OptionEntryGamePrefAbs
{
	// Token: 0x17000B66 RID: 2918
	// (get) Token: 0x06006A12 RID: 27154 RVA: 0x00299DF4 File Offset: 0x00297FF4
	// (set) Token: 0x06006A13 RID: 27155 RVA: 0x00299DFC File Offset: 0x00297FFC
	[XuiXmlAttribute("allow_custom", false)]
	public bool AllowCustomValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B67 RID: 2919
	// (get) Token: 0x06006A14 RID: 27156 RVA: 0x00299E05 File Offset: 0x00298005
	// (set) Token: 0x06006A15 RID: 27157 RVA: 0x00299E0D File Offset: 0x0029800D
	public int SettingValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B68 RID: 2920
	// (get) Token: 0x06006A16 RID: 27158 RVA: 0x00299E16 File Offset: 0x00298016
	// (set) Token: 0x06006A17 RID: 27159 RVA: 0x00299E2C File Offset: 0x0029802C
	public int CurrentValue
	{
		get
		{
			XUiC_ComboBoxBase comboGeneric = this.comboGeneric;
			return (int)((comboGeneric != null) ? comboGeneric.ValueGeneric : 0L);
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.comboGeneric.ValueGeneric = (long)Mathf.Clamp(value, (int)this.comboGeneric.ValueMinGeneric, (int)this.comboGeneric.ValueMaxGeneric);
		}
	}

	// Token: 0x17000B69 RID: 2921
	// (get) Token: 0x06006A18 RID: 27160 RVA: 0x00299E58 File Offset: 0x00298058
	// (set) Token: 0x06006A19 RID: 27161 RVA: 0x00299E88 File Offset: 0x00298088
	public override object SelectedValue
	{
		get
		{
			int num = this.CurrentValue;
			if (this.MapListIndexToGamePref != null)
			{
				num = this.MapListIndexToGamePref(num);
			}
			return num;
		}
		set
		{
			if (value is int)
			{
				int num = (int)value;
				if (this.MapGamePrefToListIndex != null)
				{
					num = this.MapGamePrefToListIndex(num);
				}
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

	// Token: 0x17000B6A RID: 2922
	// (get) Token: 0x06006A1A RID: 27162 RVA: 0x00299F16 File Offset: 0x00298116
	public override bool IsChanged
	{
		get
		{
			return this.CurrentValue != this.SettingValue;
		}
	}

	// Token: 0x17000B6B RID: 2923
	// (get) Token: 0x06006A1B RID: 27163 RVA: 0x00299F29 File Offset: 0x00298129
	public override bool IsDefault
	{
		get
		{
			return this.gamePref == null || this.CurrentValue == this.getPrefDefault();
		}
	}

	// Token: 0x17000B6C RID: 2924
	// (get) Token: 0x06006A1C RID: 27164 RVA: 0x00010E62 File Offset: 0x0000F062
	public override GamePrefs.EnumType expectedPrefType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return GamePrefs.EnumType.Int;
		}
	}

	// Token: 0x06006A1D RID: 27165 RVA: 0x00299F48 File Offset: 0x00298148
	public override void Init()
	{
		base.Init();
		if (this.AllowCustomValue)
		{
			XUiC_ComboBoxList<string> xuiC_ComboBoxList = this.comboGeneric as XUiC_ComboBoxList<string>;
			if (xuiC_ComboBoxList != null)
			{
				this.origEntriesLength = (int)(this.comboGeneric.ValueMaxGeneric + 1L);
				this.comboGeneric.ValueMaxGeneric = (long)(this.origEntriesLength - 1);
				xuiC_ComboBoxList.Elements.Add("Custom");
			}
		}
	}

	// Token: 0x06006A1E RID: 27166 RVA: 0x00299FAC File Offset: 0x002981AC
	[PublicizedFrom(EAccessModifier.Private)]
	public int getPrefDefault()
	{
		int num = (int)GamePrefs.GetDefault(this.gamePref.Value);
		if (this.MapGamePrefToListIndex != null)
		{
			num = this.MapGamePrefToListIndex(num);
		}
		return num;
	}

	// Token: 0x06006A1F RID: 27167 RVA: 0x00299FE8 File Offset: 0x002981E8
	[PublicizedFrom(EAccessModifier.Private)]
	public int getPrefValue()
	{
		int num = GamePrefs.GetInt(this.gamePref.Value);
		if (this.MapGamePrefToListIndex != null)
		{
			num = this.MapGamePrefToListIndex(num);
		}
		return num;
	}

	// Token: 0x06006A20 RID: 27168 RVA: 0x0029A01C File Offset: 0x0029821C
	[PublicizedFrom(EAccessModifier.Private)]
	public void setPrefValue(int _value)
	{
		if (this.MapListIndexToGamePref != null)
		{
			_value = this.MapListIndexToGamePref(_value);
		}
		GamePrefs.Set(this.gamePref.Value, _value);
	}

	// Token: 0x06006A21 RID: 27169 RVA: 0x0029A048 File Offset: 0x00298248
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void initCurrentValue()
	{
		if (this.gamePref == null)
		{
			return;
		}
		int prefValue = this.getPrefValue();
		if (this.AllowCustomValue && prefValue >= this.origEntriesLength)
		{
			this.comboGeneric.ValueMaxGeneric = (long)this.origEntriesLength;
			prefValue = this.origEntriesLength;
		}
		this.CurrentValue = (this.SettingValue = prefValue);
		XUiC_OptionEntryAbs.DebugLog(string.Format("GOT CURRENT INT FOR {0}: {1}", this.gamePref, this.SettingValue));
	}

	// Token: 0x06006A22 RID: 27170 RVA: 0x0029A0CC File Offset: 0x002982CC
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

	// Token: 0x06006A23 RID: 27171 RVA: 0x0029A12C File Offset: 0x0029832C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void applySelectionInternal()
	{
		if (!this.AllowCustomValue || this.comboGeneric.ValueGeneric < (long)this.origEntriesLength)
		{
			this.SettingValue = this.CurrentValue;
			this.setPrefValue(this.SettingValue);
		}
		XUiC_OptionEntryAbs.DebugLog(string.Format("SAVED INT FOR {0} TO {1}", this.gamePref, this.SettingValue));
	}

	// Token: 0x06006A24 RID: 27172 RVA: 0x0029A194 File Offset: 0x00298394
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

	// Token: 0x06006A25 RID: 27173 RVA: 0x0029A202 File Offset: 0x00298402
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void immediatelyApplyCurrentSelection()
	{
		this.setPrefValue(this.CurrentValue);
	}

	// Token: 0x0400512D RID: 20781
	public Func<int, int> MapGamePrefToListIndex;

	// Token: 0x0400512E RID: 20782
	public Func<int, int> MapListIndexToGamePref;

	// Token: 0x04005131 RID: 20785
	[PublicizedFrom(EAccessModifier.Private)]
	public int origEntriesLength;
}
