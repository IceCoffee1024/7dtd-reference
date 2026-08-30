using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.LowLevel
{
	// Token: 0x0200032A RID: 810
	[NativeType(Header = "Runtime/Misc/PlayerLoop.h")]
	[RequiredByNativeCode]
	[MovedFrom("UnityEngine.Experimental.LowLevel")]
	internal struct PlayerLoopSystemInternal
	{
		// Token: 0x04000AD3 RID: 2771
		public Type type;

		// Token: 0x04000AD4 RID: 2772
		public PlayerLoopSystem.UpdateFunction updateDelegate;

		// Token: 0x04000AD5 RID: 2773
		public IntPtr updateFunction;

		// Token: 0x04000AD6 RID: 2774
		public IntPtr loopConditionFunction;

		// Token: 0x04000AD7 RID: 2775
		public int numSubSystems;
	}
}
