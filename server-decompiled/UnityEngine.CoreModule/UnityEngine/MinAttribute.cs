using System;

namespace UnityEngine
{
	// Token: 0x02000212 RID: 530
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public sealed class MinAttribute : PropertyAttribute
	{
		// Token: 0x060017A5 RID: 6053 RVA: 0x000274AB File Offset: 0x000256AB
		public MinAttribute(float min)
		{
			this.min = min;
		}

		// Token: 0x04000879 RID: 2169
		public readonly float min;
	}
}
