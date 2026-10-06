using System;

namespace Platform.GameCore
{
	// Token: 0x02001CAB RID: 7339
	public class UserDataRoamingGameCore : UserDataRoamingAbs
	{
		// Token: 0x17001B77 RID: 7031
		// (get) Token: 0x0600D9A3 RID: 55715 RVA: 0x0002F260 File Offset: 0x0002D460
		public override SaveRoamingMode SaveRoamingMode
		{
			get
			{
				return SaveRoamingMode.Forced;
			}
		}

		// Token: 0x17001B78 RID: 7032
		// (get) Token: 0x0600D9A4 RID: 55716 RVA: 0x00020119 File Offset: 0x0001E319
		public override UserDataStorageType DefaultSaveStorage
		{
			get
			{
				return UserDataStorageType.Roaming;
			}
		}
	}
}
