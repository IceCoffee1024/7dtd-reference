using System;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001799 RID: 6041
	public struct TranslationData
	{
		// Token: 0x0600BB7B RID: 47995 RVA: 0x004551E0 File Offset: 0x004533E0
		public TranslationData(int _x, int _y, float _randomScaleMin = 0.5f, float _randomScaleMax = 1.5f)
		{
			this.x = _x;
			this.y = _y;
			Rand instance = Rand.Instance;
			this.scale = instance.Range(_randomScaleMin, _randomScaleMax);
			this.rotation = instance.Angle();
		}

		// Token: 0x0600BB7C RID: 47996 RVA: 0x0045521C File Offset: 0x0045341C
		public TranslationData(int _x, int _y, float _scale, int _rotation)
		{
			this.x = _x;
			this.y = _y;
			this.scale = _scale;
			this.rotation = _rotation;
		}

		// Token: 0x04008BFE RID: 35838
		public int x;

		// Token: 0x04008BFF RID: 35839
		public int y;

		// Token: 0x04008C00 RID: 35840
		public float scale;

		// Token: 0x04008C01 RID: 35841
		public int rotation;
	}
}
