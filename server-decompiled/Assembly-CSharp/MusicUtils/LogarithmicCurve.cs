using System;

namespace MusicUtils
{
	// Token: 0x02001A15 RID: 6677
	public class LogarithmicCurve : LinearCurve
	{
		// Token: 0x0600CB12 RID: 51986 RVA: 0x004A5A88 File Offset: 0x004A3C88
		public LogarithmicCurve(double _base, double _scale, float _start, float _end, float _startX, float _endX)
			: base((float)Math.Pow(_base, (double)_start / _scale), (float)Math.Pow(_base, (double)_end / _scale), _startX, _endX)
		{
			this.b = Math.Pow(_base, 1.0 / _scale);
			if (_start < _end)
			{
				this.min = _start;
				this.max = _end;
				return;
			}
			this.min = _end;
			this.max = _start;
		}

		// Token: 0x0600CB13 RID: 51987 RVA: 0x004A5AF1 File Offset: 0x004A3CF1
		public override float GetMixerValue(float _param)
		{
			return Utils.FastClamp((float)Math.Log((double)Math.Max(base.GetLine(_param), 0f), this.b), this.min, this.max);
		}

		// Token: 0x04009A4F RID: 39503
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly double b;

		// Token: 0x04009A50 RID: 39504
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly float min;

		// Token: 0x04009A51 RID: 39505
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly float max;
	}
}
