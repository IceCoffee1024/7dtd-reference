using System;

namespace UnityEngine
{
	// Token: 0x0200020C RID: 524
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
	public class ContextMenuItemAttribute : PropertyAttribute
	{
		// Token: 0x0600179E RID: 6046 RVA: 0x00027422 File Offset: 0x00025622
		public ContextMenuItemAttribute(string name, string function)
		{
			this.name = name;
			this.function = function;
		}

		// Token: 0x04000871 RID: 2161
		public readonly string name;

		// Token: 0x04000872 RID: 2162
		public readonly string function;
	}
}
