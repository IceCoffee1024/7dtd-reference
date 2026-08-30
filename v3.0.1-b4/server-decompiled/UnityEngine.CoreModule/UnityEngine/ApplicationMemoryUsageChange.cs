using System;

namespace UnityEngine
{
	// Token: 0x020000EE RID: 238
	public struct ApplicationMemoryUsageChange
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x00007E77 File Offset: 0x00006077
		// (set) Token: 0x060004CD RID: 1229 RVA: 0x00007E7F File Offset: 0x0000607F
		public ApplicationMemoryUsage memoryUsage { readonly get; private set; }

		// Token: 0x060004CE RID: 1230 RVA: 0x00007E88 File Offset: 0x00006088
		public ApplicationMemoryUsageChange(ApplicationMemoryUsage usage)
		{
			this.memoryUsage = usage;
		}
	}
}
