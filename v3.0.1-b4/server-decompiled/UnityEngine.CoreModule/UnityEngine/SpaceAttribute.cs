using System;

namespace UnityEngine
{
	// Token: 0x0200020F RID: 527
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
	public class SpaceAttribute : PropertyAttribute
	{
		// Token: 0x060017A1 RID: 6049 RVA: 0x0002745C File Offset: 0x0002565C
		public SpaceAttribute()
		{
			this.height = 8f;
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x00027471 File Offset: 0x00025671
		public SpaceAttribute(float height)
		{
			this.height = height;
		}

		// Token: 0x04000875 RID: 2165
		public readonly float height;
	}
}
