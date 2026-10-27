using System;

namespace Platform
{
	// Token: 0x02001C29 RID: 7209
	public class UserDataRoamingMultiPlatform : UserDataRoamingAbs
	{
		// Token: 0x17001B1C RID: 6940
		// (get) Token: 0x0600D63D RID: 54845 RVA: 0x004CFFFE File Offset: 0x004CE1FE
		public override SaveRoamingMode SaveRoamingMode
		{
			get
			{
				if (GameManager.IsDedicatedServer)
				{
					return SaveRoamingMode.None;
				}
				IUserDataRoaming userDataRoaming = PlatformManager.NativePlatform.UserDataRoaming;
				if (userDataRoaming == null)
				{
					return SaveRoamingMode.None;
				}
				return userDataRoaming.SaveRoamingMode;
			}
		}

		// Token: 0x17001B1D RID: 6941
		// (get) Token: 0x0600D63E RID: 54846 RVA: 0x004D001E File Offset: 0x004CE21E
		public override UserDataStorageType DefaultSaveStorage
		{
			get
			{
				if (GameManager.IsDedicatedServer)
				{
					return UserDataStorageType.DeviceLocal;
				}
				IUserDataRoaming userDataRoaming = PlatformManager.NativePlatform.UserDataRoaming;
				if (userDataRoaming == null)
				{
					return UserDataStorageType.DeviceLocal;
				}
				return userDataRoaming.DefaultSaveStorage;
			}
		}
	}
}
