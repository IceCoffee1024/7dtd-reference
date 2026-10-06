using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001BEE RID: 7150
	public interface IUserIdentifierMappingService
	{
		// Token: 0x0600D503 RID: 54531
		bool CanQuery(PlatformUserIdentifierAbs _id);

		// Token: 0x0600D504 RID: 54532
		void QueryMappedAccountDetails(PlatformUserIdentifierAbs _id, EPlatformIdentifier _platform, MappedAccountQueryCallback _callback);

		// Token: 0x0600D505 RID: 54533
		void QueryMappedAccountsDetails(IReadOnlyList<MappedAccountRequest> _requests, MappedAccountsQueryCallback _callback);

		// Token: 0x0600D506 RID: 54534
		bool CanReverseQuery(EPlatformIdentifier _platform, string _platformId);

		// Token: 0x0600D507 RID: 54535
		void ReverseQueryMappedAccountDetails(EPlatformIdentifier _platform, string _platformId, MappedAccountReverseQueryCallback _callback);

		// Token: 0x0600D508 RID: 54536
		void ReverseQueryMappedAccountsDetails(IReadOnlyList<MappedAccountReverseRequest> _requests, MappedAccountsReverseQueryCallback _callback);
	}
}
