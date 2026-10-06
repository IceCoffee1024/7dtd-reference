using System;
using System.Text;

namespace Platform
{
	// Token: 0x02001BAF RID: 7087
	// (Invoke) Token: 0x0600D3FF RID: 54271
	public delegate void PlatformMemoryRenderDelta<in T>(StringBuilder builder, T current, T last);
}
