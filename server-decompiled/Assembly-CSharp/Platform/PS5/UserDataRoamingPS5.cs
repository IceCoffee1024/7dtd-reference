using System;

namespace Platform.PS5
{
	// Token: 0x02001CBB RID: 7355
	public class UserDataRoamingPS5 : UserDataRoamingAbs
	{
		// Token: 0x17001B23 RID: 6947
		// (get) Token: 0x0600DA3F RID: 55871 RVA: 0x0002F168 File Offset: 0x0002D368
		public override SaveRoamingMode SaveRoamingMode
		{
			get
			{
				return SaveRoamingMode.Forced;
			}
		}

		// Token: 0x17001B24 RID: 6948
		// (get) Token: 0x0600DA40 RID: 55872 RVA: 0x00020021 File Offset: 0x0001E221
		public override UserDataStorageType DefaultSaveStorage
		{
			get
			{
				return UserDataStorageType.Roaming;
			}
		}
	}
}
