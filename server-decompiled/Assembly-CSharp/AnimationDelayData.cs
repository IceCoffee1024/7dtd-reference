using System;
using UnityEngine.Scripting;

// Token: 0x020005F2 RID: 1522
[Preserve]
public class AnimationDelayData
{
	// Token: 0x0600318B RID: 12683 RVA: 0x001413C8 File Offset: 0x0013F5C8
	public static void InitStatic()
	{
		AnimationDelayData.AnimationDelay = new AnimationDelayData.AnimationDelays[100];
		for (int i = 0; i < AnimationDelayData.AnimationDelay.Length; i++)
		{
			AnimationDelayData.AnimationDelay[i] = new AnimationDelayData.AnimationDelays(0, 0f, 0f, Constants.cMinHolsterTime, Constants.cMinUnHolsterTime, false);
		}
	}

	// Token: 0x0600318C RID: 12684 RVA: 0x00141419 File Offset: 0x0013F619
	public static void Cleanup()
	{
		AnimationDelayData.InitStatic();
	}

	// Token: 0x0400264B RID: 9803
	public static AnimationDelayData.AnimationDelays[] AnimationDelay;

	// Token: 0x020005F3 RID: 1523
	public struct AnimationDelays
	{
		// Token: 0x0600318E RID: 12686 RVA: 0x00141420 File Offset: 0x0013F620
		public AnimationDelays(int _carry, float _rayCast, float _rayCastMoving, float _holster, float _unholster, bool _twoHanded)
		{
			this.Carry = _carry;
			this.RayCast = _rayCast;
			this.RayCastMoving = _rayCastMoving;
			this.Holster = _holster;
			this.Unholster = _unholster;
			this.TwoHanded = _twoHanded;
		}

		// Token: 0x0400264C RID: 9804
		public int Carry;

		// Token: 0x0400264D RID: 9805
		public float RayCast;

		// Token: 0x0400264E RID: 9806
		public float RayCastMoving;

		// Token: 0x0400264F RID: 9807
		public float Holster;

		// Token: 0x04002650 RID: 9808
		public float Unholster;

		// Token: 0x04002651 RID: 9809
		public bool TwoHanded;
	}
}
