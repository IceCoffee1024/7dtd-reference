using System;

namespace Platform
{
	// Token: 0x02001BC6 RID: 7110
	public class UserDataRoamingMultiPlatform : UserDataRoamingAbs
	{
		// Token: 0x17001A2E RID: 6702
		// (get) Token: 0x0600D3B1 RID: 54193 RVA: 0x004C79EE File Offset: 0x004C5BEE
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

		// Token: 0x17001A2F RID: 6703
		// (get) Token: 0x0600D3B2 RID: 54194 RVA: 0x004C7A0E File Offset: 0x004C5C0E
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
