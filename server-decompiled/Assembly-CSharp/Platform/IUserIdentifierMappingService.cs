using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001B95 RID: 7061
	public interface IUserIdentifierMappingService
	{
		// Token: 0x0600D1FF RID: 53759
		bool CanQuery(PlatformUserIdentifierAbs _id);

		// Token: 0x0600D200 RID: 53760
		void QueryMappedAccountDetails(PlatformUserIdentifierAbs _id, EPlatformIdentifier _platform, MappedAccountQueryCallback _callback);

		// Token: 0x0600D201 RID: 53761
		void QueryMappedAccountsDetails(IReadOnlyList<MappedAccountRequest> _requests, MappedAccountsQueryCallback _callback);

		// Token: 0x0600D202 RID: 53762
		bool CanReverseQuery(EPlatformIdentifier _platform, string _platformId);

		// Token: 0x0600D203 RID: 53763
		void ReverseQueryMappedAccountDetails(EPlatformIdentifier _platform, string _platformId, MappedAccountReverseQueryCallback _callback);

		// Token: 0x0600D204 RID: 53764
		void ReverseQueryMappedAccountsDetails(IReadOnlyList<MappedAccountReverseRequest> _requests, MappedAccountsReverseQueryCallback _callback);
	}
}
