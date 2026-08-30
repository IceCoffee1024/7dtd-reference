using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000A14 RID: 2580
public static class SaveDataLimitUIHelper
{
	// Token: 0x1700083F RID: 2111
	// (get) Token: 0x06004D7B RID: 19835 RVA: 0x001DB1E9 File Offset: 0x001D93E9
	public static SaveDataLimitType CurrentValue
	{
		get
		{
			return SaveDataLimitUIHelper.s_currentValue;
		}
	}

	// Token: 0x06004D7C RID: 19836 RVA: 0x001DB1F0 File Offset: 0x001D93F0
	public static XUiC_ComboBoxEnum<SaveDataLimitType> AddComboBox(XUiC_ComboBoxEnum<SaveDataLimitType> saveDataLimitComboBox)
	{
		SaveDataLimitUIHelper.AddComboBoxInternal(saveDataLimitComboBox);
		return saveDataLimitComboBox;
	}

	// Token: 0x06004D7D RID: 19837 RVA: 0x001DB1FC File Offset: 0x001D93FC
	[PublicizedFrom(EAccessModifier.Private)]
	public static SaveDataLimitType Load()
	{
		SaveDataLimitType saveDataLimitType;
		if (!EnumUtils.TryParse<SaveDataLimitType>(GamePrefs.GetString(EnumGamePrefs.SaveDataLimitType), out saveDataLimitType, true) || !saveDataLimitType.IsSupported())
		{
			return SaveDataLimitUIHelper.s_defaultValue;
		}
		return saveDataLimitType;
	}

	// Token: 0x06004D7E RID: 19838 RVA: 0x001DB22C File Offset: 0x001D942C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Save()
	{
		GamePrefs.Set(EnumGamePrefs.SaveDataLimitType, SaveDataLimitUIHelper.s_currentValue.ToStringCached<SaveDataLimitType>());
	}

	// Token: 0x06004D7F RID: 19839 RVA: 0x001DB244 File Offset: 0x001D9444
	[PublicizedFrom(EAccessModifier.Private)]
	public static void AddComboBoxInternal(XUiC_ComboBoxEnum<SaveDataLimitType> saveDataLimitComboBox)
	{
		object obj;
		if (saveDataLimitComboBox == null || SaveDataLimitUIHelper.s_saveDataLimitComboBoxes.TryGetValue(saveDataLimitComboBox, out obj))
		{
			return;
		}
		SaveDataLimitUIHelper.s_saveDataLimitComboBoxes.Add(saveDataLimitComboBox, null);
		if (PlatformOptimizations.LimitedSaveData)
		{
			saveDataLimitComboBox.Min = new SaveDataLimitType?(SaveDataLimitType.Short);
		}
		saveDataLimitComboBox.Value = SaveDataLimitUIHelper.s_currentValue;
		saveDataLimitComboBox.OnValueChanged += SaveDataLimitUIHelper.SaveDataLimitComboBox_OnValueChanged;
	}

	// Token: 0x06004D80 RID: 19840 RVA: 0x001DB2A0 File Offset: 0x001D94A0
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SetCurrentValue(SaveDataLimitType limitType)
	{
		if (!limitType.IsSupported())
		{
			Log.Error(string.Format("Can not set unsupported limit: {0}", limitType));
			return;
		}
		if (SaveDataLimitUIHelper.s_currentValue == limitType)
		{
			return;
		}
		SaveDataLimitUIHelper.s_currentValue = limitType;
		foreach (KeyValuePair<XUiC_ComboBoxEnum<SaveDataLimitType>, object> keyValuePair in ((IEnumerable<KeyValuePair<XUiC_ComboBoxEnum<SaveDataLimitType>, object>>)SaveDataLimitUIHelper.s_saveDataLimitComboBoxes))
		{
			XUiC_ComboBoxEnum<SaveDataLimitType> xuiC_ComboBoxEnum;
			object obj;
			keyValuePair.Deconstruct(out xuiC_ComboBoxEnum, out obj);
			xuiC_ComboBoxEnum.Value = SaveDataLimitUIHelper.s_currentValue;
		}
		SaveDataLimitUIHelper.Save();
		Action onValueChanged = SaveDataLimitUIHelper.OnValueChanged;
		if (onValueChanged == null)
		{
			return;
		}
		onValueChanged();
	}

	// Token: 0x06004D81 RID: 19841 RVA: 0x001DB33C File Offset: 0x001D953C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SaveDataLimitComboBox_OnValueChanged(XUiController _sender, SaveDataLimitType _oldvalue, SaveDataLimitType _newvalue)
	{
		SaveDataLimitUIHelper.SetCurrentValue(_newvalue);
	}

	// Token: 0x04003C1E RID: 15390
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ConditionalWeakTable<XUiC_ComboBoxEnum<SaveDataLimitType>, object> s_saveDataLimitComboBoxes = new ConditionalWeakTable<XUiC_ComboBoxEnum<SaveDataLimitType>, object>();

	// Token: 0x04003C1F RID: 15391
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SaveDataLimitType s_defaultValue = (PlatformOptimizations.LimitedSaveData ? SaveDataLimitType.VeryLong : SaveDataLimitType.Unlimited);

	// Token: 0x04003C20 RID: 15392
	[PublicizedFrom(EAccessModifier.Private)]
	public static SaveDataLimitType s_currentValue = SaveDataLimitUIHelper.Load();

	// Token: 0x04003C21 RID: 15393
	public static Action OnValueChanged;
}
