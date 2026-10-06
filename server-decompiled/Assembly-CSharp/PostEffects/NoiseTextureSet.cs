using System;
using UnityEngine;

namespace PostEffects
{
	// Token: 0x02001D95 RID: 7573
	public sealed class NoiseTextureSet : ScriptableObject
	{
		// Token: 0x0600DFEC RID: 57324 RVA: 0x004FF0A6 File Offset: 0x004FD2A6
		public Texture2D GetTexture()
		{
			return this.GetTexture(Time.frameCount);
		}

		// Token: 0x0600DFED RID: 57325 RVA: 0x004FF0B3 File Offset: 0x004FD2B3
		public Texture2D GetTexture(int frameCount)
		{
			return this._textures[frameCount % this._textures.Length];
		}

		// Token: 0x0400A9AB RID: 43435
		[SerializeField]
		[PublicizedFrom(EAccessModifier.Private)]
		public Texture2D[] _textures;
	}
}
