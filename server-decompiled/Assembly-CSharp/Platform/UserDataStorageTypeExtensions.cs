using System;

namespace Platform
{
	// Token: 0x02001BB4 RID: 7092
	public static class UserDataStorageTypeExtensions
	{
		// Token: 0x0600D36D RID: 54125 RVA: 0x004C78A0 File Offset: 0x004C5AA0
		public static bool UsesDataLimit(this UserDataStorageType storage)
		{
			return storage == UserDataStorageType.Roaming;
		}

		// Token: 0x0600D36E RID: 54126 RVA: 0x004C78A6 File Offset: 0x004C5AA6
		public static string LocalizedName(this UserDataStorageType storage)
		{
			return Localization.Get("xuiStorageTypeOptions" + storage.ToStringCached<UserDataStorageType>(), false, null);
		}
	}
}
