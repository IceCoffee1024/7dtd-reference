using System;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	// Token: 0x02000497 RID: 1175
	public interface IPlayableBehaviour
	{
		// Token: 0x06002863 RID: 10339
		[RequiredByNativeCode]
		void OnGraphStart(Playable playable);

		// Token: 0x06002864 RID: 10340
		[RequiredByNativeCode]
		void OnGraphStop(Playable playable);

		// Token: 0x06002865 RID: 10341
		[RequiredByNativeCode]
		void OnPlayableCreate(Playable playable);

		// Token: 0x06002866 RID: 10342
		[RequiredByNativeCode]
		void OnPlayableDestroy(Playable playable);

		// Token: 0x06002867 RID: 10343
		[RequiredByNativeCode]
		void OnBehaviourPlay(Playable playable, FrameData info);

		// Token: 0x06002868 RID: 10344
		[RequiredByNativeCode]
		void OnBehaviourPause(Playable playable, FrameData info);

		// Token: 0x06002869 RID: 10345
		[RequiredByNativeCode]
		void PrepareFrame(Playable playable, FrameData info);

		// Token: 0x0600286A RID: 10346
		[RequiredByNativeCode]
		void ProcessFrame(Playable playable, FrameData info, object playerData);
	}
}
