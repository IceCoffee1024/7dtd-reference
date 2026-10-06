using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DAB RID: 3499
[Preserve]
public class XUiC_OptionEntryGamePrefIntIndex : XUiC_OptionEntryGamePrefAbs
{
	// Token: 0x17000B81 RID: 2945
	// (get) Token: 0x06006A0A RID: 27146 RVA: 0x0029D180 File Offset: 0x0029B380
	// (set) Token: 0x06006A0B RID: 27147 RVA: 0x0029D188 File Offset: 0x0029B388
	[XuiXmlAttribute("allow_custom", false)]
	public bool AllowCustomValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B82 RID: 2946
	// (get) Token: 0x06006A0C RID: 27148 RVA: 0x0029D191 File Offset: 0x0029B391
	// (set) Token: 0x06006A0D RID: 27149 RVA: 0x0029D199 File Offset: 0x0029B399
	public int SettingValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B83 RID: 2947
	// (get) Token: 0x06006A0E RID: 27150 RVA: 0x0029D1A2 File Offset: 0x0029B3A2
	// (set) Token: 0x06006A0F RID: 27151 RVA: 0x0029D1B8 File Offset: 0x0029B3B8
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

	// Token: 0x17000B84 RID: 2948
	// (get) Token: 0x06006A10 RID: 27152 RVA: 0x0029D1E4 File Offset: 0x0029B3E4
	// (set) Token: 0x06006A11 RID: 27153 RVA: 0x0029D214 File Offset: 0x0029B414
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

	// Token: 0x17000B85 RID: 2949
	// (get) Token: 0x06006A12 RID: 27154 RVA: 0x0029D2A2 File Offset: 0x0029B4A2
	public override bool IsChanged
	{
		get
		{
			return this.CurrentValue != this.SettingValue;
		}
	}

	// Token: 0x17000B86 RID: 2950
	// (get) Token: 0x06006A13 RID: 27155 RVA: 0x0029D2B5 File Offset: 0x0029B4B5
	public override bool IsDefault
	{
		get
		{
			return this.gamePref == null || this.CurrentValue == this.getPrefDefault();
		}
	}

	// Token: 0x17000B87 RID: 2951
	// (get) Token: 0x06006A14 RID: 27156 RVA: 0x00010E62 File Offset: 0x0000F062
	public override GamePrefs.EnumType expectedPrefType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return GamePrefs.EnumType.Int;
		}
	}

	// Token: 0x06006A15 RID: 27157 RVA: 0x0029D2D4 File Offset: 0x0029B4D4
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

	// Token: 0x06006A16 RID: 27158 RVA: 0x0029D338 File Offset: 0x0029B538
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

	// Token: 0x06006A17 RID: 27159 RVA: 0x0029D374 File Offset: 0x0029B574
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

	// Token: 0x06006A18 RID: 27160 RVA: 0x0029D3A8 File Offset: 0x0029B5A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void setPrefValue(int _value)
	{
		if (this.MapListIndexToGamePref != null)
		{
			_value = this.MapListIndexToGamePref(_value);
		}
		GamePrefs.Set(this.gamePref.Value, _value);
	}

	// Token: 0x06006A19 RID: 27161 RVA: 0x0029D3D4 File Offset: 0x0029B5D4
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

	// Token: 0x06006A1A RID: 27162 RVA: 0x0029D458 File Offset: 0x0029B658
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

	// Token: 0x06006A1B RID: 27163 RVA: 0x0029D4B8 File Offset: 0x0029B6B8
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

	// Token: 0x06006A1C RID: 27164 RVA: 0x0029D520 File Offset: 0x0029B720
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

	// Token: 0x06006A1D RID: 27165 RVA: 0x0029D58E File Offset: 0x0029B78E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void immediatelyApplyCurrentSelection()
	{
		this.setPrefValue(this.CurrentValue);
	}

	// Token: 0x04005164 RID: 20836
	public Func<int, int> MapGamePrefToListIndex;

	// Token: 0x04005165 RID: 20837
	public Func<int, int> MapListIndexToGamePref;

	// Token: 0x04005168 RID: 20840
	[PublicizedFrom(EAccessModifier.Private)]
	public int origEntriesLength;
}
