using System;

namespace MusicUtils
{
	// Token: 0x02001A14 RID: 6676
	public class ExponentialCurve : LinearCurve
	{
		// Token: 0x0600CB10 RID: 51984 RVA: 0x004A5A10 File Offset: 0x004A3C10
		public ExponentialCurve(double _base, float _start, float _end, float _startX, float _endX)
			: base((float)Math.Log((double)_start, _base), (float)Math.Log((double)_end, _base), _startX, _endX)
		{
			this.b = _base;
			if (_start < _end)
			{
				this.min = _start;
				this.max = _end;
				return;
			}
			this.min = _end;
			this.max = _start;
		}

		// Token: 0x0600CB11 RID: 51985 RVA: 0x004A5A61 File Offset: 0x004A3C61
		public override float GetMixerValue(float _param)
		{
			return Utils.FastClamp((float)Math.Pow(this.b, (double)base.GetLine(_param)), this.min, this.max);
		}

		// Token: 0x04009A4C RID: 39500
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly double b;

		// Token: 0x04009A4D RID: 39501
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly float min;

		// Token: 0x04009A4E RID: 39502
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly float max;
	}
}
