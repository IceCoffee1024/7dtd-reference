using System;

namespace UnityEngine
{
	// Token: 0x02000211 RID: 529
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public sealed class RangeAttribute : PropertyAttribute
	{
		// Token: 0x060017A4 RID: 6052 RVA: 0x00027493 File Offset: 0x00025693
		public RangeAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x04000877 RID: 2167
		public readonly float min;

		// Token: 0x04000878 RID: 2168
		public readonly float max;
	}
}
