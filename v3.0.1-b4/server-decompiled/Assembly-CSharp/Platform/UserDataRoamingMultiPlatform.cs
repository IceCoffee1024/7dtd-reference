using System;

namespace Platform
{
	// Token: 0x02001B9F RID: 7071
	public class UserDataRoamingMultiPlatform : UserDataRoamingAbs
	{
		// Token: 0x170019BC RID: 6588
		// (get) Token: 0x0600D223 RID: 53795 RVA: 0x004C15FA File Offset: 0x004BF7FA
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

		// Token: 0x170019BD RID: 6589
		// (get) Token: 0x0600D224 RID: 53796 RVA: 0x004C161A File Offset: 0x004BF81A
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
