using System;

namespace UnityEngine.Playables
{
	// Token: 0x020004A6 RID: 1190
	public enum PlayState
	{
		// Token: 0x04000F81 RID: 3969
		Paused,
		// Token: 0x04000F82 RID: 3970
		Playing,
		// Token: 0x04000F83 RID: 3971
		[Obsolete("Delayed is obsolete; use a custom ScriptPlayable to implement this feature", false)]
		Delayed
	}
}
