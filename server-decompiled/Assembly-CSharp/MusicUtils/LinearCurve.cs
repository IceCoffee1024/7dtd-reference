using System;

namespace MusicUtils
{
	// Token: 0x02001A13 RID: 6675
	public class LinearCurve : Curve
	{
		// Token: 0x0600CB0D RID: 51981 RVA: 0x004A59D0 File Offset: 0x004A3BD0
		public LinearCurve(float _startY, float _endY, float _startX, float _endX)
			: base(_startY, _endY, _startX, _endX)
		{
		}

		// Token: 0x0600CB0E RID: 51982 RVA: 0x004A59DD File Offset: 0x004A3BDD
		public override float GetMixerValue(float _param)
		{
			return Utils.FastClamp(this.GetLine(_param), this.linearStart, this.linearEnd);
		}

		// Token: 0x0600CB0F RID: 51983 RVA: 0x004A59F7 File Offset: 0x004A3BF7
		[PublicizedFrom(EAccessModifier.Protected)]
		public float GetLine(float _param)
		{
			return this.rate * (_param - this.startX) + this.linearStart;
		}
	}
}
