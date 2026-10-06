using System;

namespace MusicUtils
{
	// Token: 0x02001A41 RID: 6721
	public class LogarithmicCurve : LinearCurve
	{
		// Token: 0x0600CC6A RID: 52330 RVA: 0x004A9F54 File Offset: 0x004A8154
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

		// Token: 0x0600CC6B RID: 52331 RVA: 0x004A9FBD File Offset: 0x004A81BD
		public override float GetMixerValue(float _param)
		{
			return Utils.FastClamp((float)Math.Log((double)Math.Max(base.GetLine(_param), 0f), this.b), this.min, this.max);
		}

		// Token: 0x04009B15 RID: 39701
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly double b;

		// Token: 0x04009B16 RID: 39702
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly float min;

		// Token: 0x04009B17 RID: 39703
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly float max;
	}
}
