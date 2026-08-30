using System;

namespace WorldGenerationEngineFinal
{
	// Token: 0x0200173C RID: 5948
	public struct TranslationData
	{
		// Token: 0x0600B910 RID: 47376 RVA: 0x0044D314 File Offset: 0x0044B514
		public TranslationData(int _x, int _y, float _randomScaleMin = 0.5f, float _randomScaleMax = 1.5f)
		{
			this.x = _x;
			this.y = _y;
			Rand instance = Rand.Instance;
			this.scale = instance.Range(_randomScaleMin, _randomScaleMax);
			this.rotation = instance.Angle();
		}

		// Token: 0x0600B911 RID: 47377 RVA: 0x0044D350 File Offset: 0x0044B550
		public TranslationData(int _x, int _y, float _scale, int _rotation)
		{
			this.x = _x;
			this.y = _y;
			this.scale = _scale;
			this.rotation = _rotation;
		}

		// Token: 0x04008A50 RID: 35408
		public int x;

		// Token: 0x04008A51 RID: 35409
		public int y;

		// Token: 0x04008A52 RID: 35410
		public float scale;

		// Token: 0x04008A53 RID: 35411
		public int rotation;
	}
}
