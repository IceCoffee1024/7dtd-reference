using System;
using System.Text;

namespace Platform
{
	// Token: 0x02001B7A RID: 7034
	public interface IPlatformMemoryStat
	{
		// Token: 0x17001A0E RID: 6670
		// (get) Token: 0x0600D27B RID: 53883
		string Name { get; }

		// Token: 0x0600D27C RID: 53884
		void RenderColumn(StringBuilder builder, MemoryStatColumn column, bool delta);

		// Token: 0x0600D27D RID: 53885
		void UpdateLast();
	}
}
