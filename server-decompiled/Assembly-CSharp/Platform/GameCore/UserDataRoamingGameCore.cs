using System;

namespace Platform.GameCore
{
	// Token: 0x02001C79 RID: 7289
	public class UserDataRoamingGameCore : UserDataRoamingAbs
	{
		// Token: 0x17001AD1 RID: 6865
		// (get) Token: 0x0600D82A RID: 55338 RVA: 0x0002F168 File Offset: 0x0002D368
		public override SaveRoamingMode SaveRoamingMode
		{
			get
			{
				return SaveRoamingMode.Forced;
			}
		}

		// Token: 0x17001AD2 RID: 6866
		// (get) Token: 0x0600D82B RID: 55339 RVA: 0x00020021 File Offset: 0x0001E221
		public override UserDataStorageType DefaultSaveStorage
		{
			get
			{
				return UserDataStorageType.Roaming;
			}
		}
	}
}
