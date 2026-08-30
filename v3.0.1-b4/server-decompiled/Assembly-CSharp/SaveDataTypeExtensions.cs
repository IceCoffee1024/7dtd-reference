using System;

// Token: 0x02000A08 RID: 2568
public static class SaveDataTypeExtensions
{
	// Token: 0x06004D30 RID: 19760 RVA: 0x000CD933 File Offset: 0x000CBB33
	public static bool IsRoot(this SaveDataType saveDataType)
	{
		return saveDataType == SaveDataType.User;
	}

	// Token: 0x06004D31 RID: 19761 RVA: 0x001D933B File Offset: 0x001D753B
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

	// Token: 0x06004D32 RID: 19762 RVA: 0x001D9378 File Offset: 0x001D7578
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

	// Token: 0x06004D33 RID: 19763 RVA: 0x001D93E0 File Offset: 0x001D75E0
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

	// Token: 0x04003BA1 RID: 15265
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SaveDataManagedPath s_rootPathUser = new SaveDataManagedPath(SaveDataType.User.GetPathRaw());

	// Token: 0x04003BA2 RID: 15266
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SaveDataManagedPath s_rootPathSaves = new SaveDataManagedPath(SaveDataType.Saves.GetPathRaw());

	// Token: 0x04003BA3 RID: 15267
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SaveDataManagedPath s_rootPathSavesLocal = new SaveDataManagedPath(SaveDataType.SavesLocal.GetPathRaw());

	// Token: 0x04003BA4 RID: 15268
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SaveDataManagedPath s_rootPathGeneratedWorlds = new SaveDataManagedPath(SaveDataType.GeneratedWorlds.GetPathRaw());
}
