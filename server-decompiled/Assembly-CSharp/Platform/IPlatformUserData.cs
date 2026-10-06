using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001C1A RID: 7194
	public interface IPlatformUserData : IPlatformUser
	{
		// Token: 0x17001B04 RID: 6916
		// (get) Token: 0x0600D605 RID: 54789
		IReadOnlyDictionary<EBlockType, IPlatformUserBlockedData> Blocked { get; }

		// Token: 0x0600D606 RID: 54790
		void MarkBlockedStateChanged();

		// Token: 0x17001B05 RID: 6917
		// (get) Token: 0x0600D607 RID: 54791
		string Name { get; }

		// Token: 0x0600D608 RID: 54792
		void RequestUserDetailsUpdate();
	}
}
