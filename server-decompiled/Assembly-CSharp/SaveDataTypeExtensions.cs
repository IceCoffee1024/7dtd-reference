using System;

// Token: 0x02000A23 RID: 2595
public static class SaveDataTypeExtensions
{
	// Token: 0x06004D85 RID: 19845 RVA: 0x000CE183 File Offset: 0x000CC383
	public static bool IsRoot(this SaveDataType saveDataType)
	{
		return saveDataType == SaveDataType.User;
	}

	// Token: 0x06004D86 RID: 19846 RVA: 0x001DD083 File Offset: 0x001DB283
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

	// Token: 0x06004D87 RID: 19847 RVA: 0x001DD0C0 File Offset: 0x001DB2C0
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

	// Token: 0x06004D88 RID: 19848 RVA: 0x001DD128 File Offset: 0x001DB328
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

	// Token: 0x04003C3F RID: 15423
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SaveDataManagedPath s_rootPathUser = new SaveDataManagedPath(SaveDataType.User.GetPathRaw());

	// Token: 0x04003C40 RID: 15424
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SaveDataManagedPath s_rootPathSaves = new SaveDataManagedPath(SaveDataType.Saves.GetPathRaw());

	// Token: 0x04003C41 RID: 15425
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SaveDataManagedPath s_rootPathSavesLocal = new SaveDataManagedPath(SaveDataType.SavesLocal.GetPathRaw());

	// Token: 0x04003C42 RID: 15426
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SaveDataManagedPath s_rootPathGeneratedWorlds = new SaveDataManagedPath(SaveDataType.GeneratedWorlds.GetPathRaw());
}
