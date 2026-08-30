using System;
using System.Text;

namespace Platform
{
	// Token: 0x02001B7D RID: 7037
	// (Invoke) Token: 0x0600D287 RID: 53895
	public delegate void PlatformMemoryRenderDelta<in T>(StringBuilder builder, T current, T last);
}
