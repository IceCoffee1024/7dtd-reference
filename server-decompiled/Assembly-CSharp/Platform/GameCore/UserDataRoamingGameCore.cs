using System;

namespace Platform.GameCore
{
	// Token: 0x02001C52 RID: 7250
	public class UserDataRoamingGameCore : UserDataRoamingAbs
	{
		// Token: 0x17001A5F RID: 6751
		// (get) Token: 0x0600D69A RID: 54938 RVA: 0x0002EEAC File Offset: 0x0002D0AC
		public override SaveRoamingMode SaveRoamingMode
		{
			get
			{
				return SaveRoamingMode.Forced;
			}
		}

		// Token: 0x17001A60 RID: 6752
		// (get) Token: 0x0600D69B RID: 54939 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override UserDataStorageType DefaultSaveStorage
		{
			get
			{
				return UserDataStorageType.Roaming;
			}
		}
	}
}
