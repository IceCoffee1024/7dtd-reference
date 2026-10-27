using System;

namespace Platform.GameCore
{
	// Token: 0x02001CDC RID: 7388
	public class UserDataRoamingGameCore : UserDataRoamingAbs
	{
		// Token: 0x17001BBF RID: 7103
		// (get) Token: 0x0600DAB7 RID: 55991 RVA: 0x0002F260 File Offset: 0x0002D460
		public override SaveRoamingMode SaveRoamingMode
		{
			get
			{
				return SaveRoamingMode.Forced;
			}
		}

		// Token: 0x17001BC0 RID: 7104
		// (get) Token: 0x0600DAB8 RID: 55992 RVA: 0x00020119 File Offset: 0x0001E319
		public override UserDataStorageType DefaultSaveStorage
		{
			get
			{
				return UserDataStorageType.Roaming;
			}
		}
	}
}
