using System;
using UnityEngine;

namespace PostEffects
{
	// Token: 0x02001DC6 RID: 7622
	public sealed class NoiseTextureSet : ScriptableObject
	{
		// Token: 0x0600E100 RID: 57600 RVA: 0x00502C3E File Offset: 0x00500E3E
		public Texture2D GetTexture()
		{
			return this.GetTexture(Time.frameCount);
		}

		// Token: 0x0600E101 RID: 57601 RVA: 0x00502C4B File Offset: 0x00500E4B
		public Texture2D GetTexture(int frameCount)
		{
			return this._textures[frameCount % this._textures.Length];
		}

		// Token: 0x0400AA94 RID: 43668
		[SerializeField]
		[PublicizedFrom(EAccessModifier.Private)]
		public Texture2D[] _textures;
	}
}
