using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001BED RID: 7149
	public interface IUserDetailsService
	{
		// Token: 0x0600D501 RID: 54529
		void Init(IPlatform owner);

		// Token: 0x0600D502 RID: 54530
		void RequestUserDetailsUpdate(IReadOnlyList<UserDetailsRequest> requestedUsers, UserDetailsRequestCompleteHandler onComplete);
	}
}
