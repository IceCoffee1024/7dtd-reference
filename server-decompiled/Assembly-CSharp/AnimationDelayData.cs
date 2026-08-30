using System;
using UnityEngine.Scripting;

// Token: 0x020005E5 RID: 1509
[Preserve]
public class AnimationDelayData
{
	// Token: 0x06003108 RID: 12552 RVA: 0x0013ECC8 File Offset: 0x0013CEC8
	public static void InitStatic()
	{
		AnimationDelayData.AnimationDelay = new AnimationDelayData.AnimationDelays[100];
		for (int i = 0; i < AnimationDelayData.AnimationDelay.Length; i++)
		{
			AnimationDelayData.AnimationDelay[i] = new AnimationDelayData.AnimationDelays(0, 0f, 0f, Constants.cMinHolsterTime, Constants.cMinUnHolsterTime, false);
		}
	}

	// Token: 0x06003109 RID: 12553 RVA: 0x0013ED19 File Offset: 0x0013CF19
	public static void Cleanup()
	{
		AnimationDelayData.InitStatic();
	}

	// Token: 0x04002602 RID: 9730
	public static AnimationDelayData.AnimationDelays[] AnimationDelay;

	// Token: 0x020005E6 RID: 1510
	public struct AnimationDelays
	{
		// Token: 0x0600310B RID: 12555 RVA: 0x0013ED20 File Offset: 0x0013CF20
		public AnimationDelays(int _carry, float _rayCast, float _rayCastMoving, float _holster, float _unholster, bool _twoHanded)
		{
			this.Carry = _carry;
			this.RayCast = _rayCast;
			this.RayCastMoving = _rayCastMoving;
			this.Holster = _holster;
			this.Unholster = _unholster;
			this.TwoHanded = _twoHanded;
		}

		// Token: 0x04002603 RID: 9731
		public int Carry;

		// Token: 0x04002604 RID: 9732
		public float RayCast;

		// Token: 0x04002605 RID: 9733
		public float RayCastMoving;

		// Token: 0x04002606 RID: 9734
		public float Holster;

		// Token: 0x04002607 RID: 9735
		public float Unholster;

		// Token: 0x04002608 RID: 9736
		public bool TwoHanded;
	}
}
