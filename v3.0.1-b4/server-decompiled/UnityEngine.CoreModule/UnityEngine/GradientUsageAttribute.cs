using System;

namespace UnityEngine
{
	// Token: 0x02000216 RID: 534
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public sealed class GradientUsageAttribute : PropertyAttribute
	{
		// Token: 0x060017AD RID: 6061 RVA: 0x00027644 File Offset: 0x00025844
		public GradientUsageAttribute(bool hdr)
		{
			this.hdr = hdr;
			this.colorSpace = ColorSpace.Gamma;
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x0002766A File Offset: 0x0002586A
		public GradientUsageAttribute(bool hdr, ColorSpace colorSpace)
		{
			this.hdr = hdr;
			this.colorSpace = colorSpace;
		}

		// Token: 0x04000883 RID: 2179
		public readonly bool hdr = false;

		// Token: 0x04000884 RID: 2180
		public readonly ColorSpace colorSpace = ColorSpace.Gamma;
	}
}
