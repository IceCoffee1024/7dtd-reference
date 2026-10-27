using System;

namespace Platform.Shared
{
	// Token: 0x02001D19 RID: 7449
	public class UserDataRoaming : UserDataRoamingAbs
	{
		// Token: 0x0600DCA6 RID: 56486 RVA: 0x004EAE7F File Offset: 0x004E907F
		[PublicizedFrom(EAccessModifier.Private)]
		public UserDataRoaming(SaveRoamingMode mode, UserDataStorageType storageType)
		{
			this.mode = mode;
			this.storageType = storageType;
		}

		// Token: 0x17001C07 RID: 7175
		// (get) Token: 0x0600DCA7 RID: 56487 RVA: 0x004EAE95 File Offset: 0x004E9095
		public override SaveRoamingMode SaveRoamingMode
		{
			get
			{
				return this.mode;
			}
		}

		// Token: 0x17001C08 RID: 7176
		// (get) Token: 0x0600DCA8 RID: 56488 RVA: 0x004EAE9D File Offset: 0x004E909D
		public override UserDataStorageType DefaultSaveStorage
		{
			get
			{
				return this.storageType;
			}
		}

		// Token: 0x17001C09 RID: 7177
		// (get) Token: 0x0600DCA9 RID: 56489 RVA: 0x004EAEA5 File Offset: 0x004E90A5
		public static UserDataRoaming OptionalSaveRoaming
		{
			get
			{
				return new UserDataRoaming(SaveRoamingMode.Optional, UserDataStorageType.Roaming);
			}
		}

		// Token: 0x17001C0A RID: 7178
		// (get) Token: 0x0600DCAA RID: 56490 RVA: 0x004EAEAE File Offset: 0x004E90AE
		public static UserDataRoaming ForcedSaveRoaming
		{
			get
			{
				return new UserDataRoaming(SaveRoamingMode.Forced, UserDataStorageType.Roaming);
			}
		}

		// Token: 0x17001C0B RID: 7179
		// (get) Token: 0x0600DCAB RID: 56491 RVA: 0x004EAEB7 File Offset: 0x004E90B7
		public static UserDataRoaming NoSaveRoaming
		{
			get
			{
				return new UserDataRoaming(SaveRoamingMode.None, UserDataStorageType.DeviceLocal);
			}
		}

		// Token: 0x0400A70F RID: 42767
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveRoamingMode mode;

		// Token: 0x0400A710 RID: 42768
		[PublicizedFrom(EAccessModifier.Private)]
		public UserDataStorageType storageType;
	}
}
