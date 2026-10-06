using System;
using System.Text;

namespace Platform
{
	// Token: 0x02001BAC RID: 7084
	public interface IPlatformMemoryStat
	{
		// Token: 0x17001AB4 RID: 6836
		// (get) Token: 0x0600D3F3 RID: 54259
		string Name { get; }

		// Token: 0x0600D3F4 RID: 54260
		void RenderColumn(StringBuilder builder, MemoryStatColumn column, bool delta);

		// Token: 0x0600D3F5 RID: 54261
		void UpdateLast();
	}
}
