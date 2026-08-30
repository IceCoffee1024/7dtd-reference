using System;

namespace Platform.Shared
{
	// Token: 0x02001CB6 RID: 7350
	public class UserDataRoaming : UserDataRoamingAbs
	{
		// Token: 0x0600DA18 RID: 55832 RVA: 0x004E2797 File Offset: 0x004E0997
		[PublicizedFrom(EAccessModifier.Private)]
		public UserDataRoaming(SaveRoamingMode mode, UserDataStorageType storageType)
		{
			this.mode = mode;
			this.storageType = storageType;
		}

		// Token: 0x17001B19 RID: 6937
		// (get) Token: 0x0600DA19 RID: 55833 RVA: 0x004E27AD File Offset: 0x004E09AD
		public override SaveRoamingMode SaveRoamingMode
		{
			get
			{
				return this.mode;
			}
		}

		// Token: 0x17001B1A RID: 6938
		// (get) Token: 0x0600DA1A RID: 55834 RVA: 0x004E27B5 File Offset: 0x004E09B5
		public override UserDataStorageType DefaultSaveStorage
		{
			get
			{
				return this.storageType;
			}
		}

		// Token: 0x17001B1B RID: 6939
		// (get) Token: 0x0600DA1B RID: 55835 RVA: 0x004E27BD File Offset: 0x004E09BD
		public static UserDataRoaming OptionalSaveRoaming
		{
			get
			{
				return new UserDataRoaming(SaveRoamingMode.Optional, UserDataStorageType.Roaming);
			}
		}

		// Token: 0x17001B1C RID: 6940
		// (get) Token: 0x0600DA1C RID: 55836 RVA: 0x004E27C6 File Offset: 0x004E09C6
		public static UserDataRoaming ForcedSaveRoaming
		{
			get
			{
				return new UserDataRoaming(SaveRoamingMode.Forced, UserDataStorageType.Roaming);
			}
		}

		// Token: 0x17001B1D RID: 6941
		// (get) Token: 0x0600DA1D RID: 55837 RVA: 0x004E27CF File Offset: 0x004E09CF
		public static UserDataRoaming NoSaveRoaming
		{
			get
			{
				return new UserDataRoaming(SaveRoamingMode.None, UserDataStorageType.DeviceLocal);
			}
		}

		// Token: 0x0400A551 RID: 42321
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveRoamingMode mode;

		// Token: 0x0400A552 RID: 42322
		[PublicizedFrom(EAccessModifier.Private)]
		public UserDataStorageType storageType;
	}
}
