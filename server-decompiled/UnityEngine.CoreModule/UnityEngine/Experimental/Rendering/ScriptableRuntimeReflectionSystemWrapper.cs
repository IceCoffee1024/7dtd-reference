using System;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x020004D7 RID: 1239
	[RequiredByNativeCode]
	internal class ScriptableRuntimeReflectionSystemWrapper
	{
		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06002B32 RID: 11058 RVA: 0x00049233 File Offset: 0x00047433
		// (set) Token: 0x06002B33 RID: 11059 RVA: 0x0004923B File Offset: 0x0004743B
		internal IScriptableRuntimeReflectionSystem implementation { get; set; }

		// Token: 0x06002B34 RID: 11060 RVA: 0x00049244 File Offset: 0x00047444
		[RequiredByNativeCode]
		private void Internal_ScriptableRuntimeReflectionSystemWrapper_TickRealtimeProbes(out bool result)
		{
			result = this.implementation != null && this.implementation.TickRealtimeProbes();
		}
	}
}
