using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001C1E RID: 7198
	public interface IUserDetailsService
	{
		// Token: 0x0600D615 RID: 54805
		void Init(IPlatform owner);

		// Token: 0x0600D616 RID: 54806
		void RequestUserDetailsUpdate(IReadOnlyList<UserDetailsRequest> requestedUsers, UserDetailsRequestCompleteHandler onComplete);
	}
}
