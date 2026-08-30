using System;
using Unity.Collections;

namespace UnityEngine
{
	// Token: 0x020001F9 RID: 505
	internal struct TypeDispatchData : IDisposable
	{
		// Token: 0x06001718 RID: 5912 RVA: 0x000265E9 File Offset: 0x000247E9
		public void Dispose()
		{
			this.changed = null;
			this.changedID.Dispose();
			this.destroyedID.Dispose();
		}

		// Token: 0x0400084A RID: 2122
		public Object[] changed;

		// Token: 0x0400084B RID: 2123
		public NativeArray<int> changedID;

		// Token: 0x0400084C RID: 2124
		public NativeArray<int> destroyedID;
	}
}
