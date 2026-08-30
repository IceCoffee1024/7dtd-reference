using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200020B RID: 523
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	[UsedByNativeCode]
	public abstract class PropertyAttribute : Attribute
	{
		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x0600179B RID: 6043 RVA: 0x00027411 File Offset: 0x00025611
		// (set) Token: 0x0600179C RID: 6044 RVA: 0x00027419 File Offset: 0x00025619
		public int order { get; set; }
	}
}
