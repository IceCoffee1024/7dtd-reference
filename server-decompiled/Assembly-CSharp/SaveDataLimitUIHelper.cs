using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000A1E RID: 2590
public static class SaveDataLimitUIHelper
{
	// Token: 0x1700085F RID: 2143
	// (get) Token: 0x06004D4A RID: 19786 RVA: 0x001DC499 File Offset: 0x001DA699
	public static SaveDataLimitType CurrentValue
	{
		get
		{
			return SaveDataLimitUIHelper.s_currentValue;
		}
	}

	// Token: 0x06004D4B RID: 19787 RVA: 0x001DC4A0 File Offset: 0x001DA6A0
	public static XUiC_ComboBoxEnum<SaveDataLimitType> AddComboBox(XUiC_ComboBoxEnum<SaveDataLimitType> saveDataLimitComboBox)
	{
		SaveDataLimitUIHelper.AddComboBoxInternal(saveDataLimitComboBox);
		return saveDataLimitComboBox;
	}

	// Token: 0x06004D4C RID: 19788 RVA: 0x001DC4AC File Offset: 0x001DA6AC
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

	// Token: 0x06004D4D RID: 19789 RVA: 0x001DC4DC File Offset: 0x001DA6DC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Save()
	{
		GamePrefs.Set(EnumGamePrefs.SaveDataLimitType, SaveDataLimitUIHelper.s_currentValue.ToStringCached<SaveDataLimitType>());
	}

	// Token: 0x06004D4E RID: 19790 RVA: 0x001DC4F4 File Offset: 0x001DA6F4
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

	// Token: 0x06004D4F RID: 19791 RVA: 0x001DC550 File Offset: 0x001DA750
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

	// Token: 0x06004D50 RID: 19792 RVA: 0x001DC5EC File Offset: 0x001DA7EC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SaveDataLimitComboBox_OnValueChanged(XUiController _sender, SaveDataLimitType _oldvalue, SaveDataLimitType _newvalue)
	{
		SaveDataLimitUIHelper.SetCurrentValue(_newvalue);
	}

	// Token: 0x04003C2B RID: 15403
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ConditionalWeakTable<XUiC_ComboBoxEnum<SaveDataLimitType>, object> s_saveDataLimitComboBoxes = new ConditionalWeakTable<XUiC_ComboBoxEnum<SaveDataLimitType>, object>();

	// Token: 0x04003C2C RID: 15404
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SaveDataLimitType s_defaultValue = (PlatformOptimizations.LimitedSaveData ? SaveDataLimitType.VeryLong : SaveDataLimitType.Unlimited);

	// Token: 0x04003C2D RID: 15405
	[PublicizedFrom(EAccessModifier.Private)]
	public static SaveDataLimitType s_currentValue = SaveDataLimitUIHelper.Load();

	// Token: 0x04003C2E RID: 15406
	public static Action OnValueChanged;
}
