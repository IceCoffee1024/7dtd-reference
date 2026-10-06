using System;

namespace Platform.PS5
{
	// Token: 0x02001CED RID: 7405
	public class UserDataRoamingPS5 : UserDataRoamingAbs
	{
		// Token: 0x17001BC9 RID: 7113
		// (get) Token: 0x0600DBB9 RID: 56249 RVA: 0x0002F260 File Offset: 0x0002D460
		public override SaveRoamingMode SaveRoamingMode
		{
			get
			{
				return SaveRoamingMode.Forced;
			}
		}

		// Token: 0x17001BCA RID: 7114
		// (get) Token: 0x0600DBBA RID: 56250 RVA: 0x00020119 File Offset: 0x0001E319
		public override UserDataStorageType DefaultSaveStorage
		{
			get
			{
				return UserDataStorageType.Roaming;
			}
		}
	}
}
