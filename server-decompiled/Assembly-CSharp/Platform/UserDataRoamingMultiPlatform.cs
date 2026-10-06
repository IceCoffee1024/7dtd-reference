using System;

namespace Platform
{
	// Token: 0x02001BF8 RID: 7160
	public class UserDataRoamingMultiPlatform : UserDataRoamingAbs
	{
		// Token: 0x17001AD4 RID: 6868
		// (get) Token: 0x0600D529 RID: 54569 RVA: 0x004CC4D6 File Offset: 0x004CA6D6
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

		// Token: 0x17001AD5 RID: 6869
		// (get) Token: 0x0600D52A RID: 54570 RVA: 0x004CC4F6 File Offset: 0x004CA6F6
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
