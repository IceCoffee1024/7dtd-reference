using System;

namespace UnityEngine
{
	// Token: 0x02000213 RID: 531
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public sealed class MultilineAttribute : PropertyAttribute
	{
		// Token: 0x060017A6 RID: 6054 RVA: 0x000274BC File Offset: 0x000256BC
		public MultilineAttribute()
		{
			this.lines = 3;
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x000274CD File Offset: 0x000256CD
		public MultilineAttribute(int lines)
		{
			this.lines = lines;
		}

		// Token: 0x0400087A RID: 2170
		public readonly int lines;
	}
}
