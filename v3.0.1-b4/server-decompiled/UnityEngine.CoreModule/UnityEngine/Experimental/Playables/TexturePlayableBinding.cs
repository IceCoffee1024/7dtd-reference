using System;
using UnityEngine.Playables;

namespace UnityEngine.Experimental.Playables
{
	// Token: 0x020004D0 RID: 1232
	public static class TexturePlayableBinding
	{
		// Token: 0x06002B13 RID: 11027 RVA: 0x00048F3C File Offset: 0x0004713C
		public static PlayableBinding Create(string name, Object key)
		{
			return PlayableBinding.CreateInternal(name, key, typeof(RenderTexture), new PlayableBinding.CreateOutputMethod(TexturePlayableBinding.CreateTextureOutput));
		}

		// Token: 0x06002B14 RID: 11028 RVA: 0x00048F6C File Offset: 0x0004716C
		private static PlayableOutput CreateTextureOutput(PlayableGraph graph, string name)
		{
			return TexturePlayableOutput.Create(graph, name, null);
		}
	}
}
