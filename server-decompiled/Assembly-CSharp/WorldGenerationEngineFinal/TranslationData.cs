using System;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001768 RID: 5992
	public struct TranslationData
	{
		// Token: 0x0600BA67 RID: 47719 RVA: 0x00451834 File Offset: 0x0044FA34
		public TranslationData(int _x, int _y, float _randomScaleMin = 0.5f, float _randomScaleMax = 1.5f)
		{
			this.x = _x;
			this.y = _y;
			Rand instance = Rand.Instance;
			this.scale = instance.Range(_randomScaleMin, _randomScaleMax);
			this.rotation = instance.Angle();
		}

		// Token: 0x0600BA68 RID: 47720 RVA: 0x00451870 File Offset: 0x0044FA70
		public TranslationData(int _x, int _y, float _scale, int _rotation)
		{
			this.x = _x;
			this.y = _y;
			this.scale = _scale;
			this.rotation = _rotation;
		}

		// Token: 0x04008B15 RID: 35605
		public int x;

		// Token: 0x04008B16 RID: 35606
		public int y;

		// Token: 0x04008B17 RID: 35607
		public float scale;

		// Token: 0x04008B18 RID: 35608
		public int rotation;
	}
}
