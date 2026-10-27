using System;

namespace MusicUtils
{
	// Token: 0x02001A6F RID: 6767
	public abstract class Curve
	{
		// Token: 0x0600CD77 RID: 52599 RVA: 0x004AD96D File Offset: 0x004ABB6D
		public Curve(float _start, float _end, float _startX, float _endX)
		{
			this.rate = (_end - _start) / (_endX - _startX);
			this.linearStart = _start;
			this.linearEnd = _end;
			this.startX = _startX;
		}

		// Token: 0x0600CD78 RID: 52600
		public abstract float GetMixerValue(float _param);

		// Token: 0x04009BF7 RID: 39927
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly float rate;

		// Token: 0x04009BF8 RID: 39928
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly float linearStart;

		// Token: 0x04009BF9 RID: 39929
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly float linearEnd;

		// Token: 0x04009BFA RID: 39930
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly float startX;
	}
}
