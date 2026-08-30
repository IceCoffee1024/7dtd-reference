using System;
using UnityEngine;

namespace PostEffects
{
	// Token: 0x02001D3B RID: 7483
	public sealed class NoiseTextureSet : ScriptableObject
	{
		// Token: 0x0600DCD2 RID: 56530 RVA: 0x004F3A76 File Offset: 0x004F1C76
		public Texture2D GetTexture()
		{
			return this.GetTexture(Time.frameCount);
		}

		// Token: 0x0600DCD3 RID: 56531 RVA: 0x004F3A83 File Offset: 0x004F1C83
		public Texture2D GetTexture(int frameCount)
		{
			return this._textures[frameCount % this._textures.Length];
		}

		// Token: 0x0400A79F RID: 42911
		[SerializeField]
		[PublicizedFrom(EAccessModifier.Private)]
		public Texture2D[] _textures;
	}
}
