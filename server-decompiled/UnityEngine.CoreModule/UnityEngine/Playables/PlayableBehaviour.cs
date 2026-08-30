using System;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	// Token: 0x0200049E RID: 1182
	[RequiredByNativeCode]
	[Serializable]
	public abstract class PlayableBehaviour : IPlayableBehaviour, ICloneable
	{
		// Token: 0x0600287F RID: 10367 RVA: 0x00009E2F File Offset: 0x0000802F
		public PlayableBehaviour()
		{
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x00002669 File Offset: 0x00000869
		public virtual void OnGraphStart(Playable playable)
		{
		}

		// Token: 0x06002881 RID: 10369 RVA: 0x00002669 File Offset: 0x00000869
		public virtual void OnGraphStop(Playable playable)
		{
		}

		// Token: 0x06002882 RID: 10370 RVA: 0x00002669 File Offset: 0x00000869
		public virtual void OnPlayableCreate(Playable playable)
		{
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x00002669 File Offset: 0x00000869
		public virtual void OnPlayableDestroy(Playable playable)
		{
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x00002669 File Offset: 0x00000869
		[Obsolete("OnBehaviourDelay is obsolete; use a custom ScriptPlayable to implement this feature", false)]
		public virtual void OnBehaviourDelay(Playable playable, FrameData info)
		{
		}

		// Token: 0x06002885 RID: 10373 RVA: 0x00002669 File Offset: 0x00000869
		public virtual void OnBehaviourPlay(Playable playable, FrameData info)
		{
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x00002669 File Offset: 0x00000869
		public virtual void OnBehaviourPause(Playable playable, FrameData info)
		{
		}

		// Token: 0x06002887 RID: 10375 RVA: 0x00002669 File Offset: 0x00000869
		public virtual void PrepareData(Playable playable, FrameData info)
		{
		}

		// Token: 0x06002888 RID: 10376 RVA: 0x00002669 File Offset: 0x00000869
		public virtual void PrepareFrame(Playable playable, FrameData info)
		{
		}

		// Token: 0x06002889 RID: 10377 RVA: 0x00002669 File Offset: 0x00000869
		public virtual void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
		}

		// Token: 0x0600288A RID: 10378 RVA: 0x0004555C File Offset: 0x0004375C
		public virtual object Clone()
		{
			return base.MemberwiseClone();
		}
	}
}
