using System;
using UnityEngine.Scripting;

// Token: 0x020005EB RID: 1515
[Preserve]
public class AnimationDelayData
{
	// Token: 0x0600317D RID: 12669 RVA: 0x001407D4 File Offset: 0x0013E9D4
	public static void InitStatic()
	{
		AnimationDelayData.AnimationDelay = new AnimationDelayData.AnimationDelays[100];
		for (int i = 0; i < AnimationDelayData.AnimationDelay.Length; i++)
		{
			AnimationDelayData.AnimationDelay[i] = new AnimationDelayData.AnimationDelays(0, 0f, 0f, Constants.cMinHolsterTime, Constants.cMinUnHolsterTime, false);
		}
	}

	// Token: 0x0600317E RID: 12670 RVA: 0x00140825 File Offset: 0x0013EA25
	public static void Cleanup()
	{
		AnimationDelayData.InitStatic();
	}

	// Token: 0x04002630 RID: 9776
	public static AnimationDelayData.AnimationDelays[] AnimationDelay;

	// Token: 0x020005EC RID: 1516
	public struct AnimationDelays
	{
		// Token: 0x06003180 RID: 12672 RVA: 0x0014082C File Offset: 0x0013EA2C
		public AnimationDelays(int _carry, float _rayCast, float _rayCastMoving, float _holster, float _unholster, bool _twoHanded)
		{
			this.Carry = _carry;
			this.RayCast = _rayCast;
			this.RayCastMoving = _rayCastMoving;
			this.Holster = _holster;
			this.Unholster = _unholster;
			this.TwoHanded = _twoHanded;
		}

		// Token: 0x04002631 RID: 9777
		public int Carry;

		// Token: 0x04002632 RID: 9778
		public float RayCast;

		// Token: 0x04002633 RID: 9779
		public float RayCastMoving;

		// Token: 0x04002634 RID: 9780
		public float Holster;

		// Token: 0x04002635 RID: 9781
		public float Unholster;

		// Token: 0x04002636 RID: 9782
		public bool TwoHanded;
	}
}
