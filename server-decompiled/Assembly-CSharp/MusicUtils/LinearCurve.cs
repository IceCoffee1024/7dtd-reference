using System;

namespace MusicUtils
{
	// Token: 0x02001A3F RID: 6719
	public class LinearCurve : Curve
	{
		// Token: 0x0600CC65 RID: 52325 RVA: 0x004A9E9C File Offset: 0x004A809C
		public LinearCurve(float _startY, float _endY, float _startX, float _endX)
			: base(_startY, _endY, _startX, _endX)
		{
		}

		// Token: 0x0600CC66 RID: 52326 RVA: 0x004A9EA9 File Offset: 0x004A80A9
		public override float GetMixerValue(float _param)
		{
			return Utils.FastClamp(this.GetLine(_param), this.linearStart, this.linearEnd);
		}

		// Token: 0x0600CC67 RID: 52327 RVA: 0x004A9EC3 File Offset: 0x004A80C3
		[PublicizedFrom(EAccessModifier.Protected)]
		public float GetLine(float _param)
		{
			return this.rate * (_param - this.startX) + this.linearStart;
		}
	}
}
