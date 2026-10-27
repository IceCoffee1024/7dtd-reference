using System;
using UnityEngine.Scripting;

// Token: 0x02000DB5 RID: 3509
[Preserve]
public class XUiC_OptionEntryGamePrefInt : XUiC_OptionEntryGamePrefAbs
{
	// Token: 0x17000B81 RID: 2945
	// (get) Token: 0x06006A1D RID: 27165 RVA: 0x0029E408 File Offset: 0x0029C608
	// (set) Token: 0x06006A1E RID: 27166 RVA: 0x0029E410 File Offset: 0x0029C610
	public int SettingValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B82 RID: 2946
	// (get) Token: 0x06006A1F RID: 27167 RVA: 0x0029E419 File Offset: 0x0029C619
	// (set) Token: 0x06006A20 RID: 27168 RVA: 0x0029E42F File Offset: 0x0029C62F
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

	// Token: 0x17000B83 RID: 2947
	// (get) Token: 0x06006A21 RID: 27169 RVA: 0x0029E43E File Offset: 0x0029C63E
	// (set) Token: 0x06006A22 RID: 27170 RVA: 0x0029E44C File Offset: 0x0029C64C
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

	// Token: 0x17000B84 RID: 2948
	// (get) Token: 0x06006A23 RID: 27171 RVA: 0x0029E4C5 File Offset: 0x0029C6C5
	public override bool IsChanged
	{
		get
		{
			return this.CurrentValue != this.SettingValue;
		}
	}

	// Token: 0x17000B85 RID: 2949
	// (get) Token: 0x06006A24 RID: 27172 RVA: 0x0029E4D8 File Offset: 0x0029C6D8
	public override bool IsDefault
	{
		get
		{
			return this.gamePref == null || this.CurrentValue == this.getPrefDefault();
		}
	}

	// Token: 0x17000B86 RID: 2950
	// (get) Token: 0x06006A25 RID: 27173 RVA: 0x00010E62 File Offset: 0x0000F062
	public override GamePrefs.EnumType expectedPrefType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return GamePrefs.EnumType.Int;
		}
	}

	// Token: 0x06006A26 RID: 27174 RVA: 0x0029E4F7 File Offset: 0x0029C6F7
	[PublicizedFrom(EAccessModifier.Private)]
	public int getPrefDefault()
	{
		return (int)GamePrefs.GetDefault(this.gamePref.Value);
	}

	// Token: 0x06006A27 RID: 27175 RVA: 0x0029E50E File Offset: 0x0029C70E
	[PublicizedFrom(EAccessModifier.Private)]
	public int getPrefValue()
	{
		return GamePrefs.GetInt(this.gamePref.Value);
	}

	// Token: 0x06006A28 RID: 27176 RVA: 0x0029E520 File Offset: 0x0029C720
	[PublicizedFrom(EAccessModifier.Private)]
	public void setPrefValue(int _value)
	{
		GamePrefs.Set(this.gamePref.Value, _value);
	}

	// Token: 0x06006A29 RID: 27177 RVA: 0x0029E534 File Offset: 0x0029C734
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

	// Token: 0x06006A2A RID: 27178 RVA: 0x0029E58C File Offset: 0x0029C78C
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

	// Token: 0x06006A2B RID: 27179 RVA: 0x0029E5EC File Offset: 0x0029C7EC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void applySelectionInternal()
	{
		this.SettingValue = this.CurrentValue;
		this.setPrefValue(this.SettingValue);
		XUiC_OptionEntryAbs.DebugLog(string.Format("SAVED INT FOR {0} TO {1}", this.gamePref, this.SettingValue));
	}

	// Token: 0x06006A2C RID: 27180 RVA: 0x0029E62C File Offset: 0x0029C82C
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

	// Token: 0x06006A2D RID: 27181 RVA: 0x0029E69A File Offset: 0x0029C89A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void immediatelyApplyCurrentSelection()
	{
		this.setPrefValue(this.CurrentValue);
	}

	// Token: 0x04005187 RID: 20871
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxInt combo;
}
