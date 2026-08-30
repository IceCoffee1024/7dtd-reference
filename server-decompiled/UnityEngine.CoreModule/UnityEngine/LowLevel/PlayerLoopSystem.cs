using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.LowLevel
{
	// Token: 0x0200032B RID: 811
	[MovedFrom("UnityEngine.Experimental.LowLevel")]
	public struct PlayerLoopSystem
	{
		// Token: 0x060020CC RID: 8396 RVA: 0x00036572 File Offset: 0x00034772
		public override string ToString()
		{
			return this.type.Name;
		}

		// Token: 0x04000AD8 RID: 2776
		public Type type;

		// Token: 0x04000AD9 RID: 2777
		public PlayerLoopSystem[] subSystemList;

		// Token: 0x04000ADA RID: 2778
		public PlayerLoopSystem.UpdateFunction updateDelegate;

		// Token: 0x04000ADB RID: 2779
		public IntPtr updateFunction;

		// Token: 0x04000ADC RID: 2780
		public IntPtr loopConditionFunction;

		// Token: 0x0200032C RID: 812
		// (Invoke) Token: 0x060020CE RID: 8398
		public delegate void UpdateFunction();
	}
}
