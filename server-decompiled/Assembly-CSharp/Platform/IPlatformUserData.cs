using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001BE8 RID: 7144
	public interface IPlatformUserData : IPlatformUser
	{
		// Token: 0x17001A5E RID: 6750
		// (get) Token: 0x0600D48D RID: 54413
		IReadOnlyDictionary<EBlockType, IPlatformUserBlockedData> Blocked { get; }

		// Token: 0x0600D48E RID: 54414
		void MarkBlockedStateChanged();

		// Token: 0x17001A5F RID: 6751
		// (get) Token: 0x0600D48F RID: 54415
		string Name { get; }

		// Token: 0x0600D490 RID: 54416
		void RequestUserDetailsUpdate();
	}
}
