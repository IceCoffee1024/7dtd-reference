using System;

namespace Platform
{
	// Token: 0x02001C17 RID: 7191
	public static class UserDataStorageTypeExtensions
	{
		// Token: 0x0600D5F9 RID: 54777 RVA: 0x004CFEB0 File Offset: 0x004CE0B0
		public static bool UsesDataLimit(this UserDataStorageType storage)
		{
			return storage == UserDataStorageType.Roaming;
		}

		// Token: 0x0600D5FA RID: 54778 RVA: 0x004CFEB6 File Offset: 0x004CE0B6
		public static string LocalizedName(this UserDataStorageType storage)
		{
			return Localization.Get("xuiStorageTypeOptions" + storage.ToStringCached<UserDataStorageType>(), false, null);
		}
	}
}
