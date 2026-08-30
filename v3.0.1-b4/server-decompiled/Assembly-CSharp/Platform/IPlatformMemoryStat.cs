using System;
using System.Text;

namespace Platform
{
	// Token: 0x02001B53 RID: 6995
	public interface IPlatformMemoryStat
	{
		// Token: 0x1700199C RID: 6556
		// (get) Token: 0x0600D0F0 RID: 53488
		string Name { get; }

		// Token: 0x0600D0F1 RID: 53489
		void RenderColumn(StringBuilder builder, MemoryStatColumn column, bool delta);

		// Token: 0x0600D0F2 RID: 53490
		void UpdateLast();
	}
}
