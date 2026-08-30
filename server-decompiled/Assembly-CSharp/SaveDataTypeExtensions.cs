using System;

// Token: 0x02000A19 RID: 2585
public static class SaveDataTypeExtensions
{
	// Token: 0x06004DB6 RID: 19894 RVA: 0x000CE0CF File Offset: 0x000CC2CF
	public static bool IsRoot(this SaveDataType saveDataType)
	{
		return saveDataType == SaveDataType.User;
	}

	// Token: 0x06004DB7 RID: 19895 RVA: 0x001DBDD3 File Offset: 0x001D9FD3
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

	// Token: 0x06004DB8 RID: 19896 RVA: 0x001DBE10 File Offset: 0x001DA010
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

	// Token: 0x06004DB9 RID: 19897 RVA: 0x001DBE78 File Offset: 0x001DA078
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

	// Token: 0x04003C32 RID: 15410
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SaveDataManagedPath s_rootPathUser = new SaveDataManagedPath(SaveDataType.User.GetPathRaw());

	// Token: 0x04003C33 RID: 15411
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SaveDataManagedPath s_rootPathSaves = new SaveDataManagedPath(SaveDataType.Saves.GetPathRaw());

	// Token: 0x04003C34 RID: 15412
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SaveDataManagedPath s_rootPathSavesLocal = new SaveDataManagedPath(SaveDataType.SavesLocal.GetPathRaw());

	// Token: 0x04003C35 RID: 15413
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SaveDataManagedPath s_rootPathGeneratedWorlds = new SaveDataManagedPath(SaveDataType.GeneratedWorlds.GetPathRaw());
}
