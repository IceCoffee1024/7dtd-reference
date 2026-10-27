using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001C1F RID: 7199
	public interface IUserIdentifierMappingService
	{
		// Token: 0x0600D617 RID: 54807
		bool CanQuery(PlatformUserIdentifierAbs _id);

		// Token: 0x0600D618 RID: 54808
		void QueryMappedAccountDetails(PlatformUserIdentifierAbs _id, EPlatformIdentifier _platform, MappedAccountQueryCallback _callback);

		// Token: 0x0600D619 RID: 54809
		void QueryMappedAccountsDetails(IReadOnlyList<MappedAccountRequest> _requests, MappedAccountsQueryCallback _callback);

		// Token: 0x0600D61A RID: 54810
		bool CanReverseQuery(EPlatformIdentifier _platform, string _platformId);

		// Token: 0x0600D61B RID: 54811
		void ReverseQueryMappedAccountDetails(EPlatformIdentifier _platform, string _platformId, MappedAccountReverseQueryCallback _callback);

		// Token: 0x0600D61C RID: 54812
		void ReverseQueryMappedAccountsDetails(IReadOnlyList<MappedAccountReverseRequest> _requests, MappedAccountsReverseQueryCallback _callback);
	}
}
