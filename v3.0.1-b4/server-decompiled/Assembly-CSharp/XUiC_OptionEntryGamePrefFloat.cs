using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D86 RID: 3462
[Preserve]
public class XUiC_OptionEntryGamePrefFloat : XUiC_OptionEntryGamePrefAbs
{
	// Token: 0x17000B42 RID: 2882
	// (get) Token: 0x06006952 RID: 26962 RVA: 0x0029674F File Offset: 0x0029494F
	// (set) Token: 0x06006953 RID: 26963 RVA: 0x00296757 File Offset: 0x00294957
	public float SettingValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B43 RID: 2883
	// (get) Token: 0x06006954 RID: 26964 RVA: 0x00296760 File Offset: 0x00294960
	// (set) Token: 0x06006955 RID: 26965 RVA: 0x0029677D File Offset: 0x0029497D
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

	// Token: 0x17000B44 RID: 2884
	// (get) Token: 0x06006956 RID: 26966 RVA: 0x0029678C File Offset: 0x0029498C
	// (set) Token: 0x06006957 RID: 26967 RVA: 0x0029679C File Offset: 0x0029499C
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

	// Token: 0x17000B45 RID: 2885
	// (get) Token: 0x06006958 RID: 26968 RVA: 0x00296815 File Offset: 0x00294A15
	public override bool IsChanged
	{
		get
		{
			return !Mathf.Approximately(this.CurrentValue, this.SettingValue);
		}
	}

	// Token: 0x17000B46 RID: 2886
	// (get) Token: 0x06006959 RID: 26969 RVA: 0x0029682B File Offset: 0x00294A2B
	public override bool IsDefault
	{
		get
		{
			return this.gamePref == null || Mathf.Approximately(this.CurrentValue, this.getPrefDefault());
		}
	}

	// Token: 0x17000B47 RID: 2887
	// (get) Token: 0x0600695A RID: 26970 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override GamePrefs.EnumType expectedPrefType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return GamePrefs.EnumType.Float;
		}
	}

	// Token: 0x0600695B RID: 26971 RVA: 0x0029684D File Offset: 0x00294A4D
	[PublicizedFrom(EAccessModifier.Private)]
	public float getPrefDefault()
	{
		return (float)GamePrefs.GetDefault(this.gamePref.Value);
	}

	// Token: 0x0600695C RID: 26972 RVA: 0x00296864 File Offset: 0x00294A64
	[PublicizedFrom(EAccessModifier.Private)]
	public float getPrefValue()
	{
		return GamePrefs.GetFloat(this.gamePref.Value);
	}

	// Token: 0x0600695D RID: 26973 RVA: 0x00296876 File Offset: 0x00294A76
	[PublicizedFrom(EAccessModifier.Private)]
	public void setPrefValue(float _value)
	{
		GamePrefs.Set(this.gamePref.Value, _value);
	}

	// Token: 0x0600695E RID: 26974 RVA: 0x0029688C File Offset: 0x00294A8C
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

	// Token: 0x0600695F RID: 26975 RVA: 0x002968E4 File Offset: 0x00294AE4
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

	// Token: 0x06006960 RID: 26976 RVA: 0x00296944 File Offset: 0x00294B44
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void applySelectionInternal()
	{
		this.SettingValue = this.CurrentValue;
		this.setPrefValue(this.SettingValue);
		XUiC_OptionEntryAbs.DebugLog(string.Format("SAVED FLOAT FOR {0} TO {1}", this.gamePref, this.SettingValue));
	}

	// Token: 0x06006961 RID: 26977 RVA: 0x00296984 File Offset: 0x00294B84
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

	// Token: 0x06006962 RID: 26978 RVA: 0x002969F2 File Offset: 0x00294BF2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void immediatelyApplyCurrentSelection()
	{
		this.setPrefValue(this.CurrentValue);
	}

	// Token: 0x04005087 RID: 20615
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxFloat combo;
}
