using System;
using System.Text;

namespace Platform
{
	// Token: 0x02001BDD RID: 7133
	public interface IPlatformMemoryStat
	{
		// Token: 0x17001AFC RID: 6908
		// (get) Token: 0x0600D507 RID: 54535
		string Name { get; }

		// Token: 0x0600D508 RID: 54536
		void RenderColumn(StringBuilder builder, MemoryStatColumn column, bool delta);

		// Token: 0x0600D509 RID: 54537
		void UpdateLast();
	}
}
