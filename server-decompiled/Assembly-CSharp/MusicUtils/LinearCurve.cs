using System;

namespace MusicUtils
{
	// Token: 0x02001A70 RID: 6768
	public class LinearCurve : Curve
	{
		// Token: 0x0600CD79 RID: 52601 RVA: 0x004AD998 File Offset: 0x004ABB98
		public LinearCurve(float _startY, float _endY, float _startX, float _endX)
			: base(_startY, _endY, _startX, _endX)
		{
		}

		// Token: 0x0600CD7A RID: 52602 RVA: 0x004AD9A5 File Offset: 0x004ABBA5
		public override float GetMixerValue(float _param)
		{
			return Utils.FastClamp(this.GetLine(_param), this.linearStart, this.linearEnd);
		}

		// Token: 0x0600CD7B RID: 52603 RVA: 0x004AD9BF File Offset: 0x004ABBBF
		[PublicizedFrom(EAccessModifier.Protected)]
		public float GetLine(float _param)
		{
			return this.rate * (_param - this.startX) + this.linearStart;
		}
	}
}
