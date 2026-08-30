using System;
using System.Text;

namespace Platform
{
	// Token: 0x02001B56 RID: 6998
	// (Invoke) Token: 0x0600D0FC RID: 53500
	public delegate void PlatformMemoryRenderDelta<in T>(StringBuilder builder, T current, T last);
}
