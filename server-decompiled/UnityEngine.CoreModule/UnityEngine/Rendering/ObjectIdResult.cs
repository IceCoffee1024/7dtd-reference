using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200045F RID: 1119
	public class ObjectIdResult
	{
		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06002594 RID: 9620 RVA: 0x00040129 File Offset: 0x0003E329
		public Object[] idToObjectMapping { get; }

		// Token: 0x06002595 RID: 9621 RVA: 0x00040131 File Offset: 0x0003E331
		internal ObjectIdResult(Object[] idToObjectMapping)
		{
			this.idToObjectMapping = idToObjectMapping;
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x00040144 File Offset: 0x0003E344
		public static int DecodeIdFromColor(Color color)
		{
			return (int)(color.r * 255f) + ((int)(color.g * 255f) << 8) + ((int)(color.b * 255f) << 16) + ((int)(color.a * 255f) << 24);
		}
	}
}
