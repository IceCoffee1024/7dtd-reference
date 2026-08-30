using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	// Token: 0x0200049B RID: 1179
	[RequiredByNativeCode]
	public struct Playable : IPlayable, IEquatable<Playable>
	{
		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x0600286E RID: 10350 RVA: 0x000453F0 File Offset: 0x000435F0
		public static Playable Null
		{
			get
			{
				return Playable.m_NullPlayable;
			}
		}

		// Token: 0x0600286F RID: 10351 RVA: 0x00045408 File Offset: 0x00043608
		public static Playable Create(PlayableGraph graph, int inputCount = 0)
		{
			Playable playable = new Playable(graph.CreatePlayableHandle());
			playable.SetInputCount(inputCount);
			return playable;
		}

		// Token: 0x06002870 RID: 10352 RVA: 0x00045431 File Offset: 0x00043631
		[VisibleToOtherModules]
		internal Playable(PlayableHandle handle)
		{
			this.m_Handle = handle;
		}

		// Token: 0x06002871 RID: 10353 RVA: 0x0004543C File Offset: 0x0004363C
		public PlayableHandle GetHandle()
		{
			return this.m_Handle;
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x00045454 File Offset: 0x00043654
		public bool IsPlayableOfType<T>() where T : struct, IPlayable
		{
			return this.GetHandle().IsPlayableOfType<T>();
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x00045474 File Offset: 0x00043674
		public Type GetPlayableType()
		{
			return this.GetHandle().GetPlayableType();
		}

		// Token: 0x06002874 RID: 10356 RVA: 0x00045494 File Offset: 0x00043694
		public bool Equals(Playable other)
		{
			return this.GetHandle() == other.GetHandle();
		}

		// Token: 0x04000F69 RID: 3945
		private PlayableHandle m_Handle;

		// Token: 0x04000F6A RID: 3946
		private static readonly Playable m_NullPlayable = new Playable(PlayableHandle.Null);
	}
}
