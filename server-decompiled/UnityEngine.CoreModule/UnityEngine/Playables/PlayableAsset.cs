using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	// Token: 0x0200049D RID: 1181
	[AssetFileNameExtension("playable", new string[] { })]
	[RequiredByNativeCode]
	[Serializable]
	public abstract class PlayableAsset : ScriptableObject, IPlayableAsset
	{
		// Token: 0x06002879 RID: 10361
		public abstract Playable CreatePlayable(PlayableGraph graph, GameObject owner);

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x0600287A RID: 10362 RVA: 0x000454CC File Offset: 0x000436CC
		public virtual double duration
		{
			get
			{
				return PlayableBinding.DefaultDuration;
			}
		}

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x0600287B RID: 10363 RVA: 0x000454E4 File Offset: 0x000436E4
		public virtual IEnumerable<PlayableBinding> outputs
		{
			get
			{
				return PlayableBinding.None;
			}
		}

		// Token: 0x0600287C RID: 10364 RVA: 0x000454FC File Offset: 0x000436FC
		[RequiredByNativeCode]
		internal unsafe static void Internal_CreatePlayable(PlayableAsset asset, PlayableGraph graph, GameObject go, IntPtr ptr)
		{
			bool flag = asset == null;
			Playable playable;
			if (flag)
			{
				playable = Playable.Null;
			}
			else
			{
				playable = asset.CreatePlayable(graph, go);
			}
			Playable* ptr2 = (Playable*)ptr.ToPointer();
			*ptr2 = playable;
		}

		// Token: 0x0600287D RID: 10365 RVA: 0x00045538 File Offset: 0x00043738
		[RequiredByNativeCode]
		internal unsafe static void Internal_GetPlayableAssetDuration(PlayableAsset asset, IntPtr ptrToDouble)
		{
			double duration = asset.duration;
			double* ptr = (double*)ptrToDouble.ToPointer();
			*ptr = duration;
		}
	}
}
