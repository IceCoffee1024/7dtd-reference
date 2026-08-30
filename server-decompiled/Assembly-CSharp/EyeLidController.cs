using System;
using UnityEngine;

// Token: 0x02000A39 RID: 2617
public class EyeLidController : MonoBehaviour
{
	// Token: 0x06004F52 RID: 20306 RVA: 0x001E132C File Offset: 0x001DF52C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.entityAlive = base.GetComponentInParent<EntityAlive>();
		this.random = GameRandomManager.Instance.CreateGameRandom();
		this.nextBlinkTime = Time.time + this.random.RandomRange(1f, 5f);
		if (this.autoRecordEyeBoneTransforms)
		{
			this.leftTopLocalPosition = this.leftTopTransform.localPosition;
			this.leftBottomLocalPosition = this.leftBottomTransform.localPosition;
			this.leftTopRotation = this.leftTopTransform.localRotation;
			this.leftBottomRotation = this.leftBottomTransform.localRotation;
			this.rightTopLocalPosition = this.rightTopTransform.localPosition;
			this.rightBottomLocalPosition = this.rightBottomTransform.localPosition;
			this.rightTopRotation = this.rightTopTransform.localRotation;
			this.rightBottomRotation = this.rightBottomTransform.localRotation;
		}
	}

	// Token: 0x06004F53 RID: 20307 RVA: 0x001E140C File Offset: 0x001DF60C
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		if (this.debug || (this.entityAlive != null && this.entityAlive.IsDead()))
		{
			this.blinkProgress = 1f;
		}
		else
		{
			if (Time.time > this.nextBlinkTime)
			{
				this.nextBlinkTime = Time.time + this.random.RandomRange(1f, 5f);
				this.blinkState = EyeLidController.BlinkState.Closing;
			}
			switch (this.blinkState)
			{
			case EyeLidController.BlinkState.Closing:
				this.blinkProgress += 20f * Time.deltaTime;
				if (this.blinkProgress >= 1f)
				{
					this.blinkProgress = 1f;
					this.blinkState = EyeLidController.BlinkState.Opening;
				}
				break;
			case EyeLidController.BlinkState.Opening:
				this.blinkProgress -= 10f * Time.deltaTime;
				if (this.blinkProgress <= 0f)
				{
					this.blinkProgress = 0f;
					this.blinkState = EyeLidController.BlinkState.Open;
				}
				break;
			}
		}
		this.leftTopTransform.localPosition = this.leftTopLocalPosition + this.topOffset * this.blinkProgress;
		this.leftTopTransform.localRotation = Quaternion.Euler(this.topRotation * this.blinkProgress) * this.leftTopRotation;
		this.leftBottomTransform.localPosition = this.leftBottomLocalPosition;
		this.leftBottomTransform.localRotation = Quaternion.Euler(this.bottomRotation * this.blinkProgress) * this.leftBottomRotation;
		this.rightTopTransform.localPosition = this.rightTopLocalPosition + this.topOffset * this.blinkProgress;
		this.rightTopTransform.localRotation = Quaternion.Euler(this.topRotation * this.blinkProgress) * this.rightTopRotation;
		this.rightBottomTransform.localPosition = this.rightBottomLocalPosition;
		this.rightBottomTransform.localRotation = Quaternion.Euler(this.bottomRotation * this.blinkProgress) * this.rightBottomRotation;
	}

	// Token: 0x04003CC6 RID: 15558
	public Transform leftTopTransform;

	// Token: 0x04003CC7 RID: 15559
	public Transform leftBottomTransform;

	// Token: 0x04003CC8 RID: 15560
	public Transform rightTopTransform;

	// Token: 0x04003CC9 RID: 15561
	public Transform rightBottomTransform;

	// Token: 0x04003CCA RID: 15562
	public Vector3 leftTopLocalPosition;

	// Token: 0x04003CCB RID: 15563
	public Vector3 leftBottomLocalPosition;

	// Token: 0x04003CCC RID: 15564
	public Quaternion leftTopRotation;

	// Token: 0x04003CCD RID: 15565
	public Quaternion leftBottomRotation;

	// Token: 0x04003CCE RID: 15566
	public Vector3 rightTopLocalPosition;

	// Token: 0x04003CCF RID: 15567
	public Vector3 rightBottomLocalPosition;

	// Token: 0x04003CD0 RID: 15568
	public Quaternion rightTopRotation;

	// Token: 0x04003CD1 RID: 15569
	public Quaternion rightBottomRotation;

	// Token: 0x04003CD2 RID: 15570
	public bool autoRecordEyeBoneTransforms;

	// Token: 0x04003CD3 RID: 15571
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float nextBlinkTime;

	// Token: 0x04003CD4 RID: 15572
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float blinkProgress;

	// Token: 0x04003CD5 RID: 15573
	public bool debug;

	// Token: 0x04003CD6 RID: 15574
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityAlive entityAlive;

	// Token: 0x04003CD7 RID: 15575
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameRandom random;

	// Token: 0x04003CD8 RID: 15576
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 topOffset = new Vector3(0f, 0f, 0.007f);

	// Token: 0x04003CD9 RID: 15577
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 topRotation = new Vector3(40f, 0f, 0f);

	// Token: 0x04003CDA RID: 15578
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 bottomRotation = new Vector3(-10f, 0f, -10f);

	// Token: 0x04003CDB RID: 15579
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EyeLidController.BlinkState blinkState;

	// Token: 0x02000A3A RID: 2618
	[PublicizedFrom(EAccessModifier.Private)]
	public enum BlinkState
	{
		// Token: 0x04003CDD RID: 15581
		Open,
		// Token: 0x04003CDE RID: 15582
		Closing,
		// Token: 0x04003CDF RID: 15583
		Opening
	}
}
