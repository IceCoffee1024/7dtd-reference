using System;
using UnityEngine;

namespace PostEffects
{
	// Token: 0x02001D63 RID: 7523
	public sealed class NoiseTextureSet : ScriptableObject
	{
		// Token: 0x0600DE73 RID: 56947 RVA: 0x004FA4EA File Offset: 0x004F86EA
		public Texture2D GetTexture()
		{
			return this.GetTexture(Time.frameCount);
		}

		// Token: 0x0600DE74 RID: 56948 RVA: 0x004FA4F7 File Offset: 0x004F86F7
		public Texture2D GetTexture(int frameCount)
		{
			return this._textures[frameCount % this._textures.Length];
		}

		// Token: 0x0400A8D4 RID: 43220
		[SerializeField]
		[PublicizedFrom(EAccessModifier.Private)]
		public Texture2D[] _textures;
	}
}
