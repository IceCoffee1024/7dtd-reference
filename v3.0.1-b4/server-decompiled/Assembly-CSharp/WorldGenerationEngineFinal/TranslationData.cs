using System;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001716 RID: 5910
	public struct TranslationData
	{
		// Token: 0x0600B78D RID: 46989 RVA: 0x0044759C File Offset: 0x0044579C
		public TranslationData(int _x, int _y, float _randomScaleMin = 0.5f, float _randomScaleMax = 1.5f)
		{
			this.x = _x;
			this.y = _y;
			Rand instance = Rand.Instance;
			this.scale = instance.Range(_randomScaleMin, _randomScaleMax);
			this.rotation = instance.Angle();
		}

		// Token: 0x0600B78E RID: 46990 RVA: 0x004475D8 File Offset: 0x004457D8
		public TranslationData(int _x, int _y, float _scale, int _rotation)
		{
			this.x = _x;
			this.y = _y;
			this.scale = _scale;
			this.rotation = _rotation;
		}

		// Token: 0x04008934 RID: 35124
		public int x;

		// Token: 0x04008935 RID: 35125
		public int y;

		// Token: 0x04008936 RID: 35126
		public float scale;

		// Token: 0x04008937 RID: 35127
		public int rotation;
	}
}
