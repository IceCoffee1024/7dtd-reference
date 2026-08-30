using System;

namespace MusicUtils
{
	// Token: 0x020019ED RID: 6637
	public class ExponentialCurve : LinearCurve
	{
		// Token: 0x0600C987 RID: 51591 RVA: 0x0049F61C File Offset: 0x0049D81C
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

		// Token: 0x0600C988 RID: 51592 RVA: 0x0049F66D File Offset: 0x0049D86D
		public override float GetMixerValue(float _param)
		{
			return Utils.FastClamp((float)Math.Pow(this.b, (double)base.GetLine(_param)), this.min, this.max);
		}

		// Token: 0x0400991B RID: 39195
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly double b;

		// Token: 0x0400991C RID: 39196
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly float min;

		// Token: 0x0400991D RID: 39197
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly float max;
	}
}
