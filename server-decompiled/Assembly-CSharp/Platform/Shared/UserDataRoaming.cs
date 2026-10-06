using System;

namespace Platform.Shared
{
	// Token: 0x02001CE8 RID: 7400
	public class UserDataRoaming : UserDataRoamingAbs
	{
		// Token: 0x0600DB92 RID: 56210 RVA: 0x004E72DF File Offset: 0x004E54DF
		[PublicizedFrom(EAccessModifier.Private)]
		public UserDataRoaming(SaveRoamingMode mode, UserDataStorageType storageType)
		{
			this.mode = mode;
			this.storageType = storageType;
		}

		// Token: 0x17001BBF RID: 7103
		// (get) Token: 0x0600DB93 RID: 56211 RVA: 0x004E72F5 File Offset: 0x004E54F5
		public override SaveRoamingMode SaveRoamingMode
		{
			get
			{
				return this.mode;
			}
		}

		// Token: 0x17001BC0 RID: 7104
		// (get) Token: 0x0600DB94 RID: 56212 RVA: 0x004E72FD File Offset: 0x004E54FD
		public override UserDataStorageType DefaultSaveStorage
		{
			get
			{
				return this.storageType;
			}
		}

		// Token: 0x17001BC1 RID: 7105
		// (get) Token: 0x0600DB95 RID: 56213 RVA: 0x004E7305 File Offset: 0x004E5505
		public static UserDataRoaming OptionalSaveRoaming
		{
			get
			{
				return new UserDataRoaming(SaveRoamingMode.Optional, UserDataStorageType.Roaming);
			}
		}

		// Token: 0x17001BC2 RID: 7106
		// (get) Token: 0x0600DB96 RID: 56214 RVA: 0x004E730E File Offset: 0x004E550E
		public static UserDataRoaming ForcedSaveRoaming
		{
			get
			{
				return new UserDataRoaming(SaveRoamingMode.Forced, UserDataStorageType.Roaming);
			}
		}

		// Token: 0x17001BC3 RID: 7107
		// (get) Token: 0x0600DB97 RID: 56215 RVA: 0x004E7317 File Offset: 0x004E5517
		public static UserDataRoaming NoSaveRoaming
		{
			get
			{
				return new UserDataRoaming(SaveRoamingMode.None, UserDataStorageType.DeviceLocal);
			}
		}

		// Token: 0x0400A626 RID: 42534
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveRoamingMode mode;

		// Token: 0x0400A627 RID: 42535
		[PublicizedFrom(EAccessModifier.Private)]
		public UserDataStorageType storageType;
	}
}
