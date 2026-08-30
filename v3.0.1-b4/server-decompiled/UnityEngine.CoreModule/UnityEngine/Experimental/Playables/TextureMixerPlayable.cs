using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Playables
{
	// Token: 0x020004CF RID: 1231
	[NativeHeader("Runtime/Director/Core/HPlayable.h")]
	[StaticAccessor("TextureMixerPlayableBindings", StaticAccessorType.DoubleColon)]
	[NativeHeader("Runtime/Export/Director/TextureMixerPlayable.bindings.h")]
	[NativeHeader("Runtime/Graphics/Director/TextureMixerPlayable.h")]
	[RequiredByNativeCode]
	public struct TextureMixerPlayable : IPlayable, IEquatable<TextureMixerPlayable>
	{
		// Token: 0x06002B0B RID: 11019 RVA: 0x00048E30 File Offset: 0x00047030
		public static TextureMixerPlayable Create(PlayableGraph graph)
		{
			PlayableHandle playableHandle = TextureMixerPlayable.CreateHandle(graph);
			return new TextureMixerPlayable(playableHandle);
		}

		// Token: 0x06002B0C RID: 11020 RVA: 0x00048E50 File Offset: 0x00047050
		private static PlayableHandle CreateHandle(PlayableGraph graph)
		{
			PlayableHandle @null = PlayableHandle.Null;
			bool flag = !TextureMixerPlayable.CreateTextureMixerPlayableInternal(ref graph, ref @null);
			PlayableHandle playableHandle;
			if (flag)
			{
				playableHandle = PlayableHandle.Null;
			}
			else
			{
				playableHandle = @null;
			}
			return playableHandle;
		}

		// Token: 0x06002B0D RID: 11021 RVA: 0x00048E84 File Offset: 0x00047084
		internal TextureMixerPlayable(PlayableHandle handle)
		{
			bool flag = handle.IsValid();
			if (flag)
			{
				bool flag2 = !handle.IsPlayableOfType<TextureMixerPlayable>();
				if (flag2)
				{
					throw new InvalidCastException("Can't set handle: the playable is not an TextureMixerPlayable.");
				}
			}
			this.m_Handle = handle;
		}

		// Token: 0x06002B0E RID: 11022 RVA: 0x00048EC0 File Offset: 0x000470C0
		public PlayableHandle GetHandle()
		{
			return this.m_Handle;
		}

		// Token: 0x06002B0F RID: 11023 RVA: 0x00048ED8 File Offset: 0x000470D8
		public static implicit operator Playable(TextureMixerPlayable playable)
		{
			return new Playable(playable.GetHandle());
		}

		// Token: 0x06002B10 RID: 11024 RVA: 0x00048EF8 File Offset: 0x000470F8
		public static explicit operator TextureMixerPlayable(Playable playable)
		{
			return new TextureMixerPlayable(playable.GetHandle());
		}

		// Token: 0x06002B11 RID: 11025 RVA: 0x00048F18 File Offset: 0x00047118
		public bool Equals(TextureMixerPlayable other)
		{
			return this.GetHandle() == other.GetHandle();
		}

		// Token: 0x06002B12 RID: 11026
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CreateTextureMixerPlayableInternal(ref PlayableGraph graph, ref PlayableHandle handle);

		// Token: 0x0400102A RID: 4138
		private PlayableHandle m_Handle;
	}
}
