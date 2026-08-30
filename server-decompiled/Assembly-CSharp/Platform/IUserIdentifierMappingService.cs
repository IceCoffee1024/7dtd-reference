using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001BBC RID: 7100
	public interface IUserIdentifierMappingService
	{
		// Token: 0x0600D38B RID: 54155
		bool CanQuery(PlatformUserIdentifierAbs _id);

		// Token: 0x0600D38C RID: 54156
		void QueryMappedAccountDetails(PlatformUserIdentifierAbs _id, EPlatformIdentifier _platform, MappedAccountQueryCallback _callback);

		// Token: 0x0600D38D RID: 54157
		void QueryMappedAccountsDetails(IReadOnlyList<MappedAccountRequest> _requests, MappedAccountsQueryCallback _callback);

		// Token: 0x0600D38E RID: 54158
		bool CanReverseQuery(EPlatformIdentifier _platform, string _platformId);

		// Token: 0x0600D38F RID: 54159
		void ReverseQueryMappedAccountDetails(EPlatformIdentifier _platform, string _platformId, MappedAccountReverseQueryCallback _callback);

		// Token: 0x0600D390 RID: 54160
		void ReverseQueryMappedAccountsDetails(IReadOnlyList<MappedAccountReverseRequest> _requests, MappedAccountsReverseQueryCallback _callback);
	}
}
