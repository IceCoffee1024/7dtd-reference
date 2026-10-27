using System;

namespace MusicUtils
{
	// Token: 0x02001A72 RID: 6770
	public class LogarithmicCurve : LinearCurve
	{
		// Token: 0x0600CD7E RID: 52606 RVA: 0x004ADA50 File Offset: 0x004ABC50
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

		// Token: 0x0600CD7F RID: 52607 RVA: 0x004ADAB9 File Offset: 0x004ABCB9
		public override float GetMixerValue(float _param)
		{
			return Utils.FastClamp((float)Math.Log((double)Math.Max(base.GetLine(_param), 0f), this.b), this.min, this.max);
		}

		// Token: 0x04009BFE RID: 39934
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly double b;

		// Token: 0x04009BFF RID: 39935
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly float min;

		// Token: 0x04009C00 RID: 39936
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly float max;
	}
}
