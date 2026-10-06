using System;

namespace MusicUtils
{
	// Token: 0x02001A3E RID: 6718
	public abstract class Curve
	{
		// Token: 0x0600CC63 RID: 52323 RVA: 0x004A9E71 File Offset: 0x004A8071
		public Curve(float _start, float _end, float _startX, float _endX)
		{
			this.rate = (_end - _start) / (_endX - _startX);
			this.linearStart = _start;
			this.linearEnd = _end;
			this.startX = _startX;
		}

		// Token: 0x0600CC64 RID: 52324
		public abstract float GetMixerValue(float _param);

		// Token: 0x04009B0E RID: 39694
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly float rate;

		// Token: 0x04009B0F RID: 39695
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly float linearStart;

		// Token: 0x04009B10 RID: 39696
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly float linearEnd;

		// Token: 0x04009B11 RID: 39697
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly float startX;
	}
}
