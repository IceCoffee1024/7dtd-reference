using System;

// Token: 0x02000A2D RID: 2605
public static class SaveDataTypeExtensions
{
	// Token: 0x06004DA9 RID: 19881 RVA: 0x000CE52F File Offset: 0x000CC72F
	public static bool IsRoot(this SaveDataType saveDataType)
	{
		return saveDataType == SaveDataType.User;
	}

	// Token: 0x06004DAA RID: 19882 RVA: 0x001DE16B File Offset: 0x001DC36B
	public static int GetSlotPathDepth(this SaveDataType saveDataType)
	{
		switch (saveDataType)
		{
		case SaveDataType.User:
			return 0;
		case SaveDataType.Saves:
			return 2;
		case SaveDataType.SavesLocal:
			return 1;
		case SaveDataType.GeneratedWorlds:
			return 1;
		default:
			Log.Error(string.Format("{0}.{1} does not have a slot path length, defaulting to '0'.", "SaveDataType", saveDataType));
			return 0;
		}
	}

	// Token: 0x06004DAB RID: 19883 RVA: 0x001DE1A8 File Offset: 0x001DC3A8
	public static string GetPathRaw(this SaveDataType saveDataType)
	{
		string text;
		switch (saveDataType)
		{
		case SaveDataType.User:
			text = string.Empty;
			break;
		case SaveDataType.Saves:
			text = "Saves";
			break;
		case SaveDataType.SavesLocal:
			text = "SavesLocal";
			break;
		case SaveDataType.GeneratedWorlds:
			text = "GeneratedWorlds";
			break;
		default:
			throw new ArgumentOutOfRangeException("saveDataType", saveDataType, string.Format("No path specified for {0}.", saveDataType));
		}
		return text;
	}

	// Token: 0x06004DAC RID: 19884 RVA: 0x001DE210 File Offset: 0x001DC410
	public static SaveDataManagedPath GetPath(this SaveDataType saveDataType)
	{
		SaveDataManagedPath saveDataManagedPath;
		switch (saveDataType)
		{
		case SaveDataType.User:
			saveDataManagedPath = SaveDataTypeExtensions.s_rootPathUser;
			break;
		case SaveDataType.Saves:
			saveDataManagedPath = SaveDataTypeExtensions.s_rootPathSaves;
			break;
		case SaveDataType.SavesLocal:
			saveDataManagedPath = SaveDataTypeExtensions.s_rootPathSavesLocal;
			break;
		case SaveDataType.GeneratedWorlds:
			saveDataManagedPath = SaveDataTypeExtensions.s_rootPathGeneratedWorlds;
			break;
		default:
			throw new ArgumentOutOfRangeException("saveDataType", saveDataType, string.Format("No relative path specified for {0}.", saveDataType));
		}
		return saveDataManagedPath;
	}

	// Token: 0x04003C60 RID: 15456
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SaveDataManagedPath s_rootPathUser = new SaveDataManagedPath(SaveDataType.User.GetPathRaw());

	// Token: 0x04003C61 RID: 15457
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SaveDataManagedPath s_rootPathSaves = new SaveDataManagedPath(SaveDataType.Saves.GetPathRaw());

	// Token: 0x04003C62 RID: 15458
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SaveDataManagedPath s_rootPathSavesLocal = new SaveDataManagedPath(SaveDataType.SavesLocal.GetPathRaw());

	// Token: 0x04003C63 RID: 15459
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SaveDataManagedPath s_rootPathGeneratedWorlds = new SaveDataManagedPath(SaveDataType.GeneratedWorlds.GetPathRaw());
}
