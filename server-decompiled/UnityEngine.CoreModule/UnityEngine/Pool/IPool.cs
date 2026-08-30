using System;

namespace UnityEngine.Pool
{
	// Token: 0x020003C4 RID: 964
	internal interface IPool
	{
		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x060020F3 RID: 8435
		int CountInactive { get; }

		// Token: 0x060020F4 RID: 8436
		void Clear();
	}
}
