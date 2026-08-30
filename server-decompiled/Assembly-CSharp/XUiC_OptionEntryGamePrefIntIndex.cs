using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D88 RID: 3464
[Preserve]
public class XUiC_OptionEntryGamePrefIntIndex : XUiC_OptionEntryGamePrefAbs
{
	// Token: 0x17000B4E RID: 2894
	// (get) Token: 0x06006976 RID: 26998 RVA: 0x00296CA0 File Offset: 0x00294EA0
	// (set) Token: 0x06006977 RID: 26999 RVA: 0x00296CA8 File Offset: 0x00294EA8
	[XuiXmlAttribute("allow_custom", false)]
	public bool AllowCustomValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B4F RID: 2895
	// (get) Token: 0x06006978 RID: 27000 RVA: 0x00296CB1 File Offset: 0x00294EB1
	// (set) Token: 0x06006979 RID: 27001 RVA: 0x00296CB9 File Offset: 0x00294EB9
	public int SettingValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B50 RID: 2896
	// (get) Token: 0x0600697A RID: 27002 RVA: 0x00296CC2 File Offset: 0x00294EC2
	// (set) Token: 0x0600697B RID: 27003 RVA: 0x00296CD8 File Offset: 0x00294ED8
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

	// Token: 0x17000B51 RID: 2897
	// (get) Token: 0x0600697C RID: 27004 RVA: 0x00296D04 File Offset: 0x00294F04
	// (set) Token: 0x0600697D RID: 27005 RVA: 0x00296D34 File Offset: 0x00294F34
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

	// Token: 0x17000B52 RID: 2898
	// (get) Token: 0x0600697E RID: 27006 RVA: 0x00296DC2 File Offset: 0x00294FC2
	public override bool IsChanged
	{
		get
		{
			return this.CurrentValue != this.SettingValue;
		}
	}

	// Token: 0x17000B53 RID: 2899
	// (get) Token: 0x0600697F RID: 27007 RVA: 0x00296DD5 File Offset: 0x00294FD5
	public override bool IsDefault
	{
		get
		{
			return this.gamePref == null || this.CurrentValue == this.getPrefDefault();
		}
	}

	// Token: 0x17000B54 RID: 2900
	// (get) Token: 0x06006980 RID: 27008 RVA: 0x00010E62 File Offset: 0x0000F062
	public override GamePrefs.EnumType expectedPrefType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return GamePrefs.EnumType.Int;
		}
	}

	// Token: 0x06006981 RID: 27009 RVA: 0x00296DF4 File Offset: 0x00294FF4
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

	// Token: 0x06006982 RID: 27010 RVA: 0x00296E58 File Offset: 0x00295058
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

	// Token: 0x06006983 RID: 27011 RVA: 0x00296E94 File Offset: 0x00295094
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

	// Token: 0x06006984 RID: 27012 RVA: 0x00296EC8 File Offset: 0x002950C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void setPrefValue(int _value)
	{
		if (this.MapListIndexToGamePref != null)
		{
			_value = this.MapListIndexToGamePref(_value);
		}
		GamePrefs.Set(this.gamePref.Value, _value);
	}

	// Token: 0x06006985 RID: 27013 RVA: 0x00296EF4 File Offset: 0x002950F4
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

	// Token: 0x06006986 RID: 27014 RVA: 0x00296F78 File Offset: 0x00295178
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

	// Token: 0x06006987 RID: 27015 RVA: 0x00296FD8 File Offset: 0x002951D8
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

	// Token: 0x06006988 RID: 27016 RVA: 0x00297040 File Offset: 0x00295240
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

	// Token: 0x06006989 RID: 27017 RVA: 0x002970AE File Offset: 0x002952AE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void immediatelyApplyCurrentSelection()
	{
		this.setPrefValue(this.CurrentValue);
	}

	// Token: 0x0400508B RID: 20619
	public Func<int, int> MapGamePrefToListIndex;

	// Token: 0x0400508C RID: 20620
	public Func<int, int> MapListIndexToGamePref;

	// Token: 0x0400508F RID: 20623
	[PublicizedFrom(EAccessModifier.Private)]
	public int origEntriesLength;
}
