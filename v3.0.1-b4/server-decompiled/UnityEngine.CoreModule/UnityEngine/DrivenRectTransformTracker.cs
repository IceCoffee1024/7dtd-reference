using System;

namespace UnityEngine
{
	// Token: 0x0200029B RID: 667
	public struct DrivenRectTransformTracker
	{
		// Token: 0x06001C30 RID: 7216 RVA: 0x0002ED48 File Offset: 0x0002CF48
		internal static bool CanRecordModifications()
		{
			return true;
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x00002669 File Offset: 0x00000869
		public void Add(Object driver, RectTransform rectTransform, DrivenTransformProperties drivenProperties)
		{
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x0002ED5B File Offset: 0x0002CF5B
		[Obsolete("revertValues parameter is ignored. Please use Clear() instead.")]
		public void Clear(bool revertValues)
		{
			this.Clear();
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x00002669 File Offset: 0x00000869
		public void Clear()
		{
		}
	}
}
