using System;

namespace Platform.PS5
{
	// Token: 0x02001D1E RID: 7454
	public class UserDataRoamingPS5 : UserDataRoamingAbs
	{
		// Token: 0x17001C11 RID: 7185
		// (get) Token: 0x0600DCCD RID: 56525 RVA: 0x0002F260 File Offset: 0x0002D460
		public override SaveRoamingMode SaveRoamingMode
		{
			get
			{
				return SaveRoamingMode.Forced;
			}
		}

		// Token: 0x17001C12 RID: 7186
		// (get) Token: 0x0600DCCE RID: 56526 RVA: 0x00020119 File Offset: 0x0001E319
		public override UserDataStorageType DefaultSaveStorage
		{
			get
			{
				return UserDataStorageType.Roaming;
			}
		}
	}
}
