using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200020D RID: 525
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	[UsedByNativeCode]
	public class InspectorNameAttribute : PropertyAttribute
	{
		// Token: 0x0600179F RID: 6047 RVA: 0x0002743A File Offset: 0x0002563A
		public InspectorNameAttribute(string displayName)
		{
			this.displayName = displayName;
		}

		// Token: 0x04000873 RID: 2163
		public readonly string displayName;
	}
}
