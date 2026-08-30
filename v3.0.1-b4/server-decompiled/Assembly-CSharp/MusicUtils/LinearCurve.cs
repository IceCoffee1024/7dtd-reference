using System;

namespace MusicUtils
{
	// Token: 0x020019EC RID: 6636
	public class LinearCurve : Curve
	{
		// Token: 0x0600C984 RID: 51588 RVA: 0x0049F5DC File Offset: 0x0049D7DC
		public LinearCurve(float _startY, float _endY, float _startX, float _endX)
			: base(_startY, _endY, _startX, _endX)
		{
		}

		// Token: 0x0600C985 RID: 51589 RVA: 0x0049F5E9 File Offset: 0x0049D7E9
		public override float GetMixerValue(float _param)
		{
			return Utils.FastClamp(this.GetLine(_param), this.linearStart, this.linearEnd);
		}

		// Token: 0x0600C986 RID: 51590 RVA: 0x0049F603 File Offset: 0x0049D803
		[PublicizedFrom(EAccessModifier.Protected)]
		public float GetLine(float _param)
		{
			return this.rate * (_param - this.startX) + this.linearStart;
		}
	}
}
