using System;

namespace UnityEngine
{
	// Token: 0x02000138 RID: 312
	[AttributeUsage(AttributeTargets.Method)]
	public class BeforeRenderOrderAttribute : Attribute
	{
		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060008A7 RID: 2215 RVA: 0x0000DF13 File Offset: 0x0000C113
		// (set) Token: 0x060008A8 RID: 2216 RVA: 0x0000DF1B File Offset: 0x0000C11B
		public int order { get; private set; }

		// Token: 0x060008A9 RID: 2217 RVA: 0x0000DF24 File Offset: 0x0000C124
		public BeforeRenderOrderAttribute(int order)
		{
			this.order = order;
		}
	}
}
