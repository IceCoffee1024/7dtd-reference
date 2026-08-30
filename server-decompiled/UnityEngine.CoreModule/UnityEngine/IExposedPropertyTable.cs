using System;

namespace UnityEngine
{
	// Token: 0x0200011F RID: 287
	public interface IExposedPropertyTable
	{
		// Token: 0x06000727 RID: 1831
		void SetReferenceValue(PropertyName id, Object value);

		// Token: 0x06000728 RID: 1832
		Object GetReferenceValue(PropertyName id, out bool idValid);

		// Token: 0x06000729 RID: 1833
		void ClearReferenceValue(PropertyName id);
	}
}
