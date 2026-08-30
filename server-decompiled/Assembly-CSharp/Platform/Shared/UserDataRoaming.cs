using System;

namespace Platform.Shared
{
	// Token: 0x02001C8F RID: 7311
	public class UserDataRoaming : UserDataRoamingAbs
	{
		// Token: 0x0600D882 RID: 55426 RVA: 0x004DC1BB File Offset: 0x004DA3BB
		[PublicizedFrom(EAccessModifier.Private)]
		public UserDataRoaming(SaveRoamingMode mode, UserDataStorageType storageType)
		{
			this.mode = mode;
			this.storageType = storageType;
		}

		// Token: 0x17001AA7 RID: 6823
		// (get) Token: 0x0600D883 RID: 55427 RVA: 0x004DC1D1 File Offset: 0x004DA3D1
		public override SaveRoamingMode SaveRoamingMode
		{
			get
			{
				return this.mode;
			}
		}

		// Token: 0x17001AA8 RID: 6824
		// (get) Token: 0x0600D884 RID: 55428 RVA: 0x004DC1D9 File Offset: 0x004DA3D9
		public override UserDataStorageType DefaultSaveStorage
		{
			get
			{
				return this.storageType;
			}
		}

		// Token: 0x17001AA9 RID: 6825
		// (get) Token: 0x0600D885 RID: 55429 RVA: 0x004DC1E1 File Offset: 0x004DA3E1
		public static UserDataRoaming OptionalSaveRoaming
		{
			get
			{
				return new UserDataRoaming(SaveRoamingMode.Optional, UserDataStorageType.Roaming);
			}
		}

		// Token: 0x17001AAA RID: 6826
		// (get) Token: 0x0600D886 RID: 55430 RVA: 0x004DC1EA File Offset: 0x004DA3EA
		public static UserDataRoaming ForcedSaveRoaming
		{
			get
			{
				return new UserDataRoaming(SaveRoamingMode.Forced, UserDataStorageType.Roaming);
			}
		}

		// Token: 0x17001AAB RID: 6827
		// (get) Token: 0x0600D887 RID: 55431 RVA: 0x004DC1F3 File Offset: 0x004DA3F3
		public static UserDataRoaming NoSaveRoaming
		{
			get
			{
				return new UserDataRoaming(SaveRoamingMode.None, UserDataStorageType.DeviceLocal);
			}
		}

		// Token: 0x0400A41E RID: 42014
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveRoamingMode mode;

		// Token: 0x0400A41F RID: 42015
		[PublicizedFrom(EAccessModifier.Private)]
		public UserDataStorageType storageType;
	}
}
