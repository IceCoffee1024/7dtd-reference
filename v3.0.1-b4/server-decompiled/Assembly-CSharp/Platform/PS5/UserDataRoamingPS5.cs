using System;

namespace Platform.PS5
{
	// Token: 0x02001C94 RID: 7316
	public class UserDataRoamingPS5 : UserDataRoamingAbs
	{
		// Token: 0x17001AB1 RID: 6833
		// (get) Token: 0x0600D8A7 RID: 55463 RVA: 0x0002EEAC File Offset: 0x0002D0AC
		public override SaveRoamingMode SaveRoamingMode
		{
			get
			{
				return SaveRoamingMode.Forced;
			}
		}

		// Token: 0x17001AB2 RID: 6834
		// (get) Token: 0x0600D8A8 RID: 55464 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override UserDataStorageType DefaultSaveStorage
		{
			get
			{
				return UserDataStorageType.Roaming;
			}
		}
	}
}
