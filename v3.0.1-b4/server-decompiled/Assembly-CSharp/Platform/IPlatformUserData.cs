using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001BC1 RID: 7105
	public interface IPlatformUserData : IPlatformUser
	{
		// Token: 0x170019EC RID: 6636
		// (get) Token: 0x0600D2FF RID: 54015
		IReadOnlyDictionary<EBlockType, IPlatformUserBlockedData> Blocked { get; }

		// Token: 0x0600D300 RID: 54016
		void MarkBlockedStateChanged();

		// Token: 0x170019ED RID: 6637
		// (get) Token: 0x0600D301 RID: 54017
		string Name { get; }

		// Token: 0x0600D302 RID: 54018
		void RequestUserDetailsUpdate();
	}
}
