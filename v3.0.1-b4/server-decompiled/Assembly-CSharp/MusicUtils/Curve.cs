using System;

namespace MusicUtils
{
	// Token: 0x020019EB RID: 6635
	public abstract class Curve
	{
		// Token: 0x0600C982 RID: 51586 RVA: 0x0049F5B1 File Offset: 0x0049D7B1
		public Curve(float _start, float _end, float _startX, float _endX)
		{
			this.rate = (_end - _start) / (_endX - _startX);
			this.linearStart = _start;
			this.linearEnd = _end;
			this.startX = _startX;
		}

		// Token: 0x0600C983 RID: 51587
		public abstract float GetMixerValue(float _param);

		// Token: 0x04009917 RID: 39191
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly float rate;

		// Token: 0x04009918 RID: 39192
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly float linearStart;

		// Token: 0x04009919 RID: 39193
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly float linearEnd;

		// Token: 0x0400991A RID: 39194
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly float startX;
	}
}
