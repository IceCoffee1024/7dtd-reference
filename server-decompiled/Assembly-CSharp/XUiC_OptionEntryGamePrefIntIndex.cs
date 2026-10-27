using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DB6 RID: 3510
[Preserve]
public class XUiC_OptionEntryGamePrefIntIndex : XUiC_OptionEntryGamePrefAbs
{
	// Token: 0x17000B87 RID: 2951
	// (get) Token: 0x06006A2F RID: 27183 RVA: 0x0029E6A8 File Offset: 0x0029C8A8
	// (set) Token: 0x06006A30 RID: 27184 RVA: 0x0029E6B0 File Offset: 0x0029C8B0
	[XuiXmlAttribute("allow_custom", false)]
	public bool AllowCustomValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B88 RID: 2952
	// (get) Token: 0x06006A31 RID: 27185 RVA: 0x0029E6B9 File Offset: 0x0029C8B9
	// (set) Token: 0x06006A32 RID: 27186 RVA: 0x0029E6C1 File Offset: 0x0029C8C1
	public int SettingValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B89 RID: 2953
	// (get) Token: 0x06006A33 RID: 27187 RVA: 0x0029E6CA File Offset: 0x0029C8CA
	// (set) Token: 0x06006A34 RID: 27188 RVA: 0x0029E6E0 File Offset: 0x0029C8E0
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

	// Token: 0x17000B8A RID: 2954
	// (get) Token: 0x06006A35 RID: 27189 RVA: 0x0029E70C File Offset: 0x0029C90C
	// (set) Token: 0x06006A36 RID: 27190 RVA: 0x0029E73C File Offset: 0x0029C93C
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

	// Token: 0x17000B8B RID: 2955
	// (get) Token: 0x06006A37 RID: 27191 RVA: 0x0029E7CA File Offset: 0x0029C9CA
	public override bool IsChanged
	{
		get
		{
			return this.CurrentValue != this.SettingValue;
		}
	}

	// Token: 0x17000B8C RID: 2956
	// (get) Token: 0x06006A38 RID: 27192 RVA: 0x0029E7DD File Offset: 0x0029C9DD
	public override bool IsDefault
	{
		get
		{
			return this.gamePref == null || this.CurrentValue == this.getPrefDefault();
		}
	}

	// Token: 0x17000B8D RID: 2957
	// (get) Token: 0x06006A39 RID: 27193 RVA: 0x00010E62 File Offset: 0x0000F062
	public override GamePrefs.EnumType expectedPrefType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return GamePrefs.EnumType.Int;
		}
	}

	// Token: 0x06006A3A RID: 27194 RVA: 0x0029E7FC File Offset: 0x0029C9FC
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

	// Token: 0x06006A3B RID: 27195 RVA: 0x0029E860 File Offset: 0x0029CA60
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

	// Token: 0x06006A3C RID: 27196 RVA: 0x0029E89C File Offset: 0x0029CA9C
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

	// Token: 0x06006A3D RID: 27197 RVA: 0x0029E8D0 File Offset: 0x0029CAD0
	[PublicizedFrom(EAccessModifier.Private)]
	public void setPrefValue(int _value)
	{
		if (this.MapListIndexToGamePref != null)
		{
			_value = this.MapListIndexToGamePref(_value);
		}
		GamePrefs.Set(this.gamePref.Value, _value);
	}

	// Token: 0x06006A3E RID: 27198 RVA: 0x0029E8FC File Offset: 0x0029CAFC
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

	// Token: 0x06006A3F RID: 27199 RVA: 0x0029E980 File Offset: 0x0029CB80
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

	// Token: 0x06006A40 RID: 27200 RVA: 0x0029E9E0 File Offset: 0x0029CBE0
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

	// Token: 0x06006A41 RID: 27201 RVA: 0x0029EA48 File Offset: 0x0029CC48
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

	// Token: 0x06006A42 RID: 27202 RVA: 0x0029EAB6 File Offset: 0x0029CCB6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void immediatelyApplyCurrentSelection()
	{
		this.setPrefValue(this.CurrentValue);
	}

	// Token: 0x04005189 RID: 20873
	public Func<int, int> MapGamePrefToListIndex;

	// Token: 0x0400518A RID: 20874
	public Func<int, int> MapListIndexToGamePref;

	// Token: 0x0400518D RID: 20877
	[PublicizedFrom(EAccessModifier.Private)]
	public int origEntriesLength;
}
