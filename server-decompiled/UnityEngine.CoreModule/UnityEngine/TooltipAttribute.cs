using System;

namespace UnityEngine
{
	// Token: 0x0200020E RID: 526
	[AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
	public class TooltipAttribute : PropertyAttribute
	{
		// Token: 0x060017A0 RID: 6048 RVA: 0x0002744B File Offset: 0x0002564B
		public TooltipAttribute(string tooltip)
		{
			this.tooltip = tooltip;
		}

		// Token: 0x04000874 RID: 2164
		public readonly string tooltip;
	}
}
