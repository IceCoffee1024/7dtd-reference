using System;

namespace MusicUtils
{
	// Token: 0x02001A40 RID: 6720
	public class ExponentialCurve : LinearCurve
	{
		// Token: 0x0600CC68 RID: 52328 RVA: 0x004A9EDC File Offset: 0x004A80DC
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

		// Token: 0x0600CC69 RID: 52329 RVA: 0x004A9F2D File Offset: 0x004A812D
		public override float GetMixerValue(float _param)
		{
			return Utils.FastClamp((float)Math.Pow(this.b, (double)base.GetLine(_param)), this.min, this.max);
		}

		// Token: 0x04009B12 RID: 39698
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly double b;

		// Token: 0x04009B13 RID: 39699
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly float min;

		// Token: 0x04009B14 RID: 39700
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly float max;
	}
}
