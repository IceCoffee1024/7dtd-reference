using System;

namespace UnityEngine.Networking
{
	// Token: 0x020018E9 RID: 6377
	public enum NetworkError
	{
		// Token: 0x04009493 RID: 38035
		Ok,
		// Token: 0x04009494 RID: 38036
		WrongHost,
		// Token: 0x04009495 RID: 38037
		WrongConnection,
		// Token: 0x04009496 RID: 38038
		WrongChannel,
		// Token: 0x04009497 RID: 38039
		NoResources,
		// Token: 0x04009498 RID: 38040
		BadMessage,
		// Token: 0x04009499 RID: 38041
		Timeout,
		// Token: 0x0400949A RID: 38042
		MessageToLong,
		// Token: 0x0400949B RID: 38043
		WrongOperation,
		// Token: 0x0400949C RID: 38044
		VersionMismatch,
		// Token: 0x0400949D RID: 38045
		CRCMismatch,
		// Token: 0x0400949E RID: 38046
		DNSFailure,
		// Token: 0x0400949F RID: 38047
		UsageError
	}
}
