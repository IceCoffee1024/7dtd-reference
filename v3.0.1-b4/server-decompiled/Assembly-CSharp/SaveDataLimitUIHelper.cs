using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000A03 RID: 2563
public static class SaveDataLimitUIHelper
{
	// Token: 0x1700082D RID: 2093
	// (get) Token: 0x06004CF5 RID: 19701 RVA: 0x001D8751 File Offset: 0x001D6951
	public static SaveDataLimitType CurrentValue
	{
		get
		{
			return SaveDataLimitUIHelper.s_currentValue;
		}
	}

	// Token: 0x06004CF6 RID: 19702 RVA: 0x001D8758 File Offset: 0x001D6958
	public static XUiC_ComboBoxEnum<SaveDataLimitType> AddComboBox(XUiC_ComboBoxEnum<SaveDataLimitType> saveDataLimitComboBox)
	{
		SaveDataLimitUIHelper.AddComboBoxInternal(saveDataLimitComboBox);
		return saveDataLimitComboBox;
	}

	// Token: 0x06004CF7 RID: 19703 RVA: 0x001D8764 File Offset: 0x001D6964
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

	// Token: 0x06004CF8 RID: 19704 RVA: 0x001D8794 File Offset: 0x001D6994
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Save()
	{
		GamePrefs.Set(EnumGamePrefs.SaveDataLimitType, SaveDataLimitUIHelper.s_currentValue.ToStringCached<SaveDataLimitType>());
	}

	// Token: 0x06004CF9 RID: 19705 RVA: 0x001D87AC File Offset: 0x001D69AC
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

	// Token: 0x06004CFA RID: 19706 RVA: 0x001D8808 File Offset: 0x001D6A08
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

	// Token: 0x06004CFB RID: 19707 RVA: 0x001D88A4 File Offset: 0x001D6AA4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SaveDataLimitComboBox_OnValueChanged(XUiController _sender, SaveDataLimitType _oldvalue, SaveDataLimitType _newvalue)
	{
		SaveDataLimitUIHelper.SetCurrentValue(_newvalue);
	}

	// Token: 0x04003B8D RID: 15245
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ConditionalWeakTable<XUiC_ComboBoxEnum<SaveDataLimitType>, object> s_saveDataLimitComboBoxes = new ConditionalWeakTable<XUiC_ComboBoxEnum<SaveDataLimitType>, object>();

	// Token: 0x04003B8E RID: 15246
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SaveDataLimitType s_defaultValue = (PlatformOptimizations.LimitedSaveData ? SaveDataLimitType.VeryLong : SaveDataLimitType.Unlimited);

	// Token: 0x04003B8F RID: 15247
	[PublicizedFrom(EAccessModifier.Private)]
	public static SaveDataLimitType s_currentValue = SaveDataLimitUIHelper.Load();

	// Token: 0x04003B90 RID: 15248
	public static Action OnValueChanged;
}
