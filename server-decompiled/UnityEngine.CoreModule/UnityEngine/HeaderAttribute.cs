using System;

namespace UnityEngine
{
	// Token: 0x02000210 RID: 528
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
	public class HeaderAttribute : PropertyAttribute
	{
		// Token: 0x060017A3 RID: 6051 RVA: 0x00027482 File Offset: 0x00025682
		public HeaderAttribute(string header)
		{
			this.header = header;
		}

		// Token: 0x04000876 RID: 2166
		public readonly string header;
	}
}
