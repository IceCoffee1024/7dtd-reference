using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	// Token: 0x020004A8 RID: 1192
	[RequiredByNativeCode]
	public struct PlayableOutput : IPlayableOutput, IEquatable<PlayableOutput>
	{
		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06002977 RID: 10615 RVA: 0x000464E8 File Offset: 0x000446E8
		public static PlayableOutput Null
		{
			get
			{
				return PlayableOutput.m_NullPlayableOutput;
			}
		}

		// Token: 0x06002978 RID: 10616 RVA: 0x000464FF File Offset: 0x000446FF
		[VisibleToOtherModules]
		internal PlayableOutput(PlayableOutputHandle handle)
		{
			this.m_Handle = handle;
		}

		// Token: 0x06002979 RID: 10617 RVA: 0x0004650C File Offset: 0x0004470C
		public PlayableOutputHandle GetHandle()
		{
			return this.m_Handle;
		}

		// Token: 0x0600297A RID: 10618 RVA: 0x00046524 File Offset: 0x00044724
		public bool IsPlayableOutputOfType<T>() where T : struct, IPlayableOutput
		{
			return this.GetHandle().IsPlayableOutputOfType<T>();
		}

		// Token: 0x0600297B RID: 10619 RVA: 0x00046544 File Offset: 0x00044744
		public Type GetPlayableOutputType()
		{
			return this.GetHandle().GetPlayableOutputType();
		}

		// Token: 0x0600297C RID: 10620 RVA: 0x00046564 File Offset: 0x00044764
		public bool Equals(PlayableOutput other)
		{
			return this.GetHandle() == other.GetHandle();
		}

		// Token: 0x04000F87 RID: 3975
		private PlayableOutputHandle m_Handle;

		// Token: 0x04000F88 RID: 3976
		private static readonly PlayableOutput m_NullPlayableOutput = new PlayableOutput(PlayableOutputHandle.Null);
	}
}
