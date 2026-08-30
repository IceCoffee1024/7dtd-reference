using System;
using UnityEngine.Scripting;

// Token: 0x02000D87 RID: 3463
[Preserve]
public class XUiC_OptionEntryGamePrefInt : XUiC_OptionEntryGamePrefAbs
{
	// Token: 0x17000B48 RID: 2888
	// (get) Token: 0x06006964 RID: 26980 RVA: 0x00296A00 File Offset: 0x00294C00
	// (set) Token: 0x06006965 RID: 26981 RVA: 0x00296A08 File Offset: 0x00294C08
	public int SettingValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B49 RID: 2889
	// (get) Token: 0x06006966 RID: 26982 RVA: 0x00296A11 File Offset: 0x00294C11
	// (set) Token: 0x06006967 RID: 26983 RVA: 0x00296A27 File Offset: 0x00294C27
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

	// Token: 0x17000B4A RID: 2890
	// (get) Token: 0x06006968 RID: 26984 RVA: 0x00296A36 File Offset: 0x00294C36
	// (set) Token: 0x06006969 RID: 26985 RVA: 0x00296A44 File Offset: 0x00294C44
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

	// Token: 0x17000B4B RID: 2891
	// (get) Token: 0x0600696A RID: 26986 RVA: 0x00296ABD File Offset: 0x00294CBD
	public override bool IsChanged
	{
		get
		{
			return this.CurrentValue != this.SettingValue;
		}
	}

	// Token: 0x17000B4C RID: 2892
	// (get) Token: 0x0600696B RID: 26987 RVA: 0x00296AD0 File Offset: 0x00294CD0
	public override bool IsDefault
	{
		get
		{
			return this.gamePref == null || this.CurrentValue == this.getPrefDefault();
		}
	}

	// Token: 0x17000B4D RID: 2893
	// (get) Token: 0x0600696C RID: 26988 RVA: 0x00010E62 File Offset: 0x0000F062
	public override GamePrefs.EnumType expectedPrefType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return GamePrefs.EnumType.Int;
		}
	}

	// Token: 0x0600696D RID: 26989 RVA: 0x00296AEF File Offset: 0x00294CEF
	[PublicizedFrom(EAccessModifier.Private)]
	public int getPrefDefault()
	{
		return (int)GamePrefs.GetDefault(this.gamePref.Value);
	}

	// Token: 0x0600696E RID: 26990 RVA: 0x00296B06 File Offset: 0x00294D06
	[PublicizedFrom(EAccessModifier.Private)]
	public int getPrefValue()
	{
		return GamePrefs.GetInt(this.gamePref.Value);
	}

	// Token: 0x0600696F RID: 26991 RVA: 0x00296B18 File Offset: 0x00294D18
	[PublicizedFrom(EAccessModifier.Private)]
	public void setPrefValue(int _value)
	{
		GamePrefs.Set(this.gamePref.Value, _value);
	}

	// Token: 0x06006970 RID: 26992 RVA: 0x00296B2C File Offset: 0x00294D2C
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

	// Token: 0x06006971 RID: 26993 RVA: 0x00296B84 File Offset: 0x00294D84
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

	// Token: 0x06006972 RID: 26994 RVA: 0x00296BE4 File Offset: 0x00294DE4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void applySelectionInternal()
	{
		this.SettingValue = this.CurrentValue;
		this.setPrefValue(this.SettingValue);
		XUiC_OptionEntryAbs.DebugLog(string.Format("SAVED INT FOR {0} TO {1}", this.gamePref, this.SettingValue));
	}

	// Token: 0x06006973 RID: 26995 RVA: 0x00296C24 File Offset: 0x00294E24
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

	// Token: 0x06006974 RID: 26996 RVA: 0x00296C92 File Offset: 0x00294E92
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void immediatelyApplyCurrentSelection()
	{
		this.setPrefValue(this.CurrentValue);
	}

	// Token: 0x04005089 RID: 20617
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxInt combo;
}
