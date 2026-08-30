using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001BBB RID: 7099
	public interface IUserDetailsService
	{
		// Token: 0x0600D389 RID: 54153
		void Init(IPlatform owner);

		// Token: 0x0600D38A RID: 54154
		void RequestUserDetailsUpdate(IReadOnlyList<UserDetailsRequest> requestedUsers, UserDetailsRequestCompleteHandler onComplete);
	}
}
