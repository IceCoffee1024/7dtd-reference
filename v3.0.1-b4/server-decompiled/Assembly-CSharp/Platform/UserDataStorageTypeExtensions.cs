using System;

namespace Platform
{
	// Token: 0x02001B8D RID: 7053
	public static class UserDataStorageTypeExtensions
	{
		// Token: 0x0600D1E1 RID: 53729 RVA: 0x004C14AC File Offset: 0x004BF6AC
		public static bool UsesDataLimit(this UserDataStorageType storage)
		{
			return storage == UserDataStorageType.Roaming;
		}

		// Token: 0x0600D1E2 RID: 53730 RVA: 0x004C14B2 File Offset: 0x004BF6B2
		public static string LocalizedName(this UserDataStorageType storage)
		{
			return Localization.Get("xuiStorageTypeOptions" + storage.ToStringCached<UserDataStorageType>(), false, null);
		}
	}
}
