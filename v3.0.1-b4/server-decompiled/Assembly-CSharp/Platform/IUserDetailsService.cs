using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001B94 RID: 7060
	public interface IUserDetailsService
	{
		// Token: 0x0600D1FD RID: 53757
		void Init(IPlatform owner);

		// Token: 0x0600D1FE RID: 53758
		void RequestUserDetailsUpdate(IReadOnlyList<UserDetailsRequest> requestedUsers, UserDetailsRequestCompleteHandler onComplete);
	}
}
