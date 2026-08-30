using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Playables
{
	// Token: 0x020004D2 RID: 1234
	[RequiredByNativeCode]
	[NativeHeader("Runtime/Export/Director/TexturePlayableOutput.bindings.h")]
	[StaticAccessor("TexturePlayableOutputBindings", StaticAccessorType.DoubleColon)]
	[NativeHeader("Runtime/Graphics/RenderTexture.h")]
	[NativeHeader("Runtime/Graphics/Director/TexturePlayableOutput.h")]
	public struct TexturePlayableOutput : IPlayableOutput
	{
		// Token: 0x06002B16 RID: 11030 RVA: 0x00048F8C File Offset: 0x0004718C
		public static TexturePlayableOutput Create(PlayableGraph graph, string name, RenderTexture target)
		{
			PlayableOutputHandle playableOutputHandle;
			bool flag = !TexturePlayableGraphExtensions.InternalCreateTextureOutput(ref graph, name, out playableOutputHandle);
			TexturePlayableOutput texturePlayableOutput;
			if (flag)
			{
				texturePlayableOutput = TexturePlayableOutput.Null;
			}
			else
			{
				TexturePlayableOutput texturePlayableOutput2 = new TexturePlayableOutput(playableOutputHandle);
				texturePlayableOutput2.SetTarget(target);
				texturePlayableOutput = texturePlayableOutput2;
			}
			return texturePlayableOutput;
		}

		// Token: 0x06002B17 RID: 11031 RVA: 0x00048FCC File Offset: 0x000471CC
		internal TexturePlayableOutput(PlayableOutputHandle handle)
		{
			bool flag = handle.IsValid();
			if (flag)
			{
				bool flag2 = !handle.IsPlayableOutputOfType<TexturePlayableOutput>();
				if (flag2)
				{
					throw new InvalidCastException("Can't set handle: the playable is not an TexturePlayableOutput.");
				}
			}
			this.m_Handle = handle;
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06002B18 RID: 11032 RVA: 0x00049008 File Offset: 0x00047208
		public static TexturePlayableOutput Null
		{
			get
			{
				return new TexturePlayableOutput(PlayableOutputHandle.Null);
			}
		}

		// Token: 0x06002B19 RID: 11033 RVA: 0x00049024 File Offset: 0x00047224
		public PlayableOutputHandle GetHandle()
		{
			return this.m_Handle;
		}

		// Token: 0x06002B1A RID: 11034 RVA: 0x0004903C File Offset: 0x0004723C
		public static implicit operator PlayableOutput(TexturePlayableOutput output)
		{
			return new PlayableOutput(output.GetHandle());
		}

		// Token: 0x06002B1B RID: 11035 RVA: 0x0004905C File Offset: 0x0004725C
		public static explicit operator TexturePlayableOutput(PlayableOutput output)
		{
			return new TexturePlayableOutput(output.GetHandle());
		}

		// Token: 0x06002B1C RID: 11036 RVA: 0x0004907C File Offset: 0x0004727C
		public RenderTexture GetTarget()
		{
			return TexturePlayableOutput.InternalGetTarget(ref this.m_Handle);
		}

		// Token: 0x06002B1D RID: 11037 RVA: 0x00049099 File Offset: 0x00047299
		public void SetTarget(RenderTexture value)
		{
			TexturePlayableOutput.InternalSetTarget(ref this.m_Handle, value);
		}

		// Token: 0x06002B1E RID: 11038
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RenderTexture InternalGetTarget(ref PlayableOutputHandle output);

		// Token: 0x06002B1F RID: 11039
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalSetTarget(ref PlayableOutputHandle output, RenderTexture target);

		// Token: 0x0400102B RID: 4139
		private PlayableOutputHandle m_Handle;
	}
}
