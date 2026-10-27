using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001C4B RID: 7243
	public interface IPlatformUserData : IPlatformUser
	{
		// Token: 0x17001B4C RID: 6988
		// (get) Token: 0x0600D719 RID: 55065
		IReadOnlyDictionary<EBlockType, IPlatformUserBlockedData> Blocked { get; }

		// Token: 0x0600D71A RID: 55066
		void MarkBlockedStateChanged();

		// Token: 0x17001B4D RID: 6989
		// (get) Token: 0x0600D71B RID: 55067
		string Name { get; }

		// Token: 0x0600D71C RID: 55068
		void RequestUserDetailsUpdate();
	}
}
