using System;

namespace MusicUtils
{
	// Token: 0x020019EE RID: 6638
	public class LogarithmicCurve : LinearCurve
	{
		// Token: 0x0600C989 RID: 51593 RVA: 0x0049F694 File Offset: 0x0049D894
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

		// Token: 0x0600C98A RID: 51594 RVA: 0x0049F6FD File Offset: 0x0049D8FD
		public override float GetMixerValue(float _param)
		{
			return Utils.FastClamp((float)Math.Log((double)Math.Max(base.GetLine(_param), 0f), this.b), this.min, this.max);
		}

		// Token: 0x0400991E RID: 39198
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly double b;

		// Token: 0x0400991F RID: 39199
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly float min;

		// Token: 0x04009920 RID: 39200
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly float max;
	}
}
