using System;

namespace Platform
{
	// Token: 0x02001BE6 RID: 7142
	public static class UserDataStorageTypeExtensions
	{
		// Token: 0x0600D4E5 RID: 54501 RVA: 0x004CC388 File Offset: 0x004CA588
		public static bool UsesDataLimit(this UserDataStorageType storage)
		{
			return storage == UserDataStorageType.Roaming;
		}

		// Token: 0x0600D4E6 RID: 54502 RVA: 0x004CC38E File Offset: 0x004CA58E
		public static string LocalizedName(this UserDataStorageType storage)
		{
			return Localization.Get("xuiStorageTypeOptions" + storage.ToStringCached<UserDataStorageType>(), false, null);
		}
	}
}
