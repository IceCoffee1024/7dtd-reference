using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000A28 RID: 2600
public static class SaveDataLimitUIHelper
{
	// Token: 0x17000865 RID: 2149
	// (get) Token: 0x06004D6E RID: 19822 RVA: 0x001DD581 File Offset: 0x001DB781
	public static SaveDataLimitType CurrentValue
	{
		get
		{
			return SaveDataLimitUIHelper.s_currentValue;
		}
	}

	// Token: 0x06004D6F RID: 19823 RVA: 0x001DD588 File Offset: 0x001DB788
	public static XUiC_ComboBoxEnum<SaveDataLimitType> AddComboBox(XUiC_ComboBoxEnum<SaveDataLimitType> saveDataLimitComboBox)
	{
		SaveDataLimitUIHelper.AddComboBoxInternal(saveDataLimitComboBox);
		return saveDataLimitComboBox;
	}

	// Token: 0x06004D70 RID: 19824 RVA: 0x001DD594 File Offset: 0x001DB794
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

	// Token: 0x06004D71 RID: 19825 RVA: 0x001DD5C4 File Offset: 0x001DB7C4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Save()
	{
		GamePrefs.Set(EnumGamePrefs.SaveDataLimitType, SaveDataLimitUIHelper.s_currentValue.ToStringCached<SaveDataLimitType>());
	}

	// Token: 0x06004D72 RID: 19826 RVA: 0x001DD5DC File Offset: 0x001DB7DC
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

	// Token: 0x06004D73 RID: 19827 RVA: 0x001DD638 File Offset: 0x001DB838
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

	// Token: 0x06004D74 RID: 19828 RVA: 0x001DD6D4 File Offset: 0x001DB8D4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SaveDataLimitComboBox_OnValueChanged(XUiController _sender, SaveDataLimitType _oldvalue, SaveDataLimitType _newvalue)
	{
		SaveDataLimitUIHelper.SetCurrentValue(_newvalue);
	}

	// Token: 0x04003C4C RID: 15436
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ConditionalWeakTable<XUiC_ComboBoxEnum<SaveDataLimitType>, object> s_saveDataLimitComboBoxes = new ConditionalWeakTable<XUiC_ComboBoxEnum<SaveDataLimitType>, object>();

	// Token: 0x04003C4D RID: 15437
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SaveDataLimitType s_defaultValue = (PlatformOptimizations.LimitedSaveData ? SaveDataLimitType.VeryLong : SaveDataLimitType.Unlimited);

	// Token: 0x04003C4E RID: 15438
	[PublicizedFrom(EAccessModifier.Private)]
	public static SaveDataLimitType s_currentValue = SaveDataLimitUIHelper.Load();

	// Token: 0x04003C4F RID: 15439
	public static Action OnValueChanged;
}
