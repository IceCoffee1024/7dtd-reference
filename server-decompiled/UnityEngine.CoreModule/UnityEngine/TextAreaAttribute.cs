using System;

namespace UnityEngine
{
	// Token: 0x02000214 RID: 532
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public sealed class TextAreaAttribute : PropertyAttribute
	{
		// Token: 0x060017A8 RID: 6056 RVA: 0x000274DE File Offset: 0x000256DE
		public TextAreaAttribute()
		{
			this.minLines = 3;
			this.maxLines = 3;
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x000274F6 File Offset: 0x000256F6
		public TextAreaAttribute(int minLines, int maxLines)
		{
			this.minLines = minLines;
			this.maxLines = maxLines;
		}

		// Token: 0x0400087B RID: 2171
		public readonly int minLines;

		// Token: 0x0400087C RID: 2172
		public readonly int maxLines;
	}
}
