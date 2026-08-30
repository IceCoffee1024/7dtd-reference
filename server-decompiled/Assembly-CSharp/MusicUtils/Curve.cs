using System;

namespace MusicUtils
{
	// Token: 0x02001A12 RID: 6674
	public abstract class Curve
	{
		// Token: 0x0600CB0B RID: 51979 RVA: 0x004A59A5 File Offset: 0x004A3BA5
		public Curve(float _start, float _end, float _startX, float _endX)
		{
			this.rate = (_end - _start) / (_endX - _startX);
			this.linearStart = _start;
			this.linearEnd = _end;
			this.startX = _startX;
		}

		// Token: 0x0600CB0C RID: 51980
		public abstract float GetMixerValue(float _param);

		// Token: 0x04009A48 RID: 39496
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly float rate;

		// Token: 0x04009A49 RID: 39497
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly float linearStart;

		// Token: 0x04009A4A RID: 39498
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly float linearEnd;

		// Token: 0x04009A4B RID: 39499
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly float startX;
	}
}
