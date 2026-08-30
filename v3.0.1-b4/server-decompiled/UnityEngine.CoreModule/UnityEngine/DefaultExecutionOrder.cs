using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000233 RID: 563
	[AttributeUsage(AttributeTargets.Class)]
	[UsedByNativeCode]
	public class DefaultExecutionOrder : Attribute
	{
		// Token: 0x06001859 RID: 6233 RVA: 0x0002855D File Offset: 0x0002675D
		public DefaultExecutionOrder(int order)
		{
			this.m_Order = order;
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x0600185A RID: 6234 RVA: 0x00028570 File Offset: 0x00026770
		public int order
		{
			get
			{
				return this.m_Order;
			}
		}

		// Token: 0x040008A2 RID: 2210
		private int m_Order;
	}
}
