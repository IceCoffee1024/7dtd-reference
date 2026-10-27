using System;

namespace MusicUtils
{
	// Token: 0x02001A71 RID: 6769
	public class ExponentialCurve : LinearCurve
	{
		// Token: 0x0600CD7C RID: 52604 RVA: 0x004AD9D8 File Offset: 0x004ABBD8
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

		// Token: 0x0600CD7D RID: 52605 RVA: 0x004ADA29 File Offset: 0x004ABC29
		public override float GetMixerValue(float _param)
		{
			return Utils.FastClamp((float)Math.Pow(this.b, (double)base.GetLine(_param)), this.min, this.max);
		}

		// Token: 0x04009BFB RID: 39931
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly double b;

		// Token: 0x04009BFC RID: 39932
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly float min;

		// Token: 0x04009BFD RID: 39933
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly float max;
	}
}
