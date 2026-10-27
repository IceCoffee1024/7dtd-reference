using System;
using System.Text;

namespace Platform
{
	// Token: 0x02001BE0 RID: 7136
	// (Invoke) Token: 0x0600D513 RID: 54547
	public delegate void PlatformMemoryRenderDelta<in T>(StringBuilder builder, T current, T last);
}
