using System;
using UnityEngine;

// Token: 0x02000A54 RID: 2644
public class EyeLidController : MonoBehaviour
{
	// Token: 0x06004FA7 RID: 20391 RVA: 0x001E5074 File Offset: 0x001E3274
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

	// Token: 0x06004FA8 RID: 20392 RVA: 0x001E5154 File Offset: 0x001E3354
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

	// Token: 0x04003D64 RID: 15716
	public Transform leftTopTransform;

	// Token: 0x04003D65 RID: 15717
	public Transform leftBottomTransform;

	// Token: 0x04003D66 RID: 15718
	public Transform rightTopTransform;

	// Token: 0x04003D67 RID: 15719
	public Transform rightBottomTransform;

	// Token: 0x04003D68 RID: 15720
	public Vector3 leftTopLocalPosition;

	// Token: 0x04003D69 RID: 15721
	public Vector3 leftBottomLocalPosition;

	// Token: 0x04003D6A RID: 15722
	public Quaternion leftTopRotation;

	// Token: 0x04003D6B RID: 15723
	public Quaternion leftBottomRotation;

	// Token: 0x04003D6C RID: 15724
	public Vector3 rightTopLocalPosition;

	// Token: 0x04003D6D RID: 15725
	public Vector3 rightBottomLocalPosition;

	// Token: 0x04003D6E RID: 15726
	public Quaternion rightTopRotation;

	// Token: 0x04003D6F RID: 15727
	public Quaternion rightBottomRotation;

	// Token: 0x04003D70 RID: 15728
	public bool autoRecordEyeBoneTransforms;

	// Token: 0x04003D71 RID: 15729
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float nextBlinkTime;

	// Token: 0x04003D72 RID: 15730
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float blinkProgress;

	// Token: 0x04003D73 RID: 15731
	public bool debug;

	// Token: 0x04003D74 RID: 15732
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityAlive entityAlive;

	// Token: 0x04003D75 RID: 15733
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameRandom random;

	// Token: 0x04003D76 RID: 15734
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 topOffset = new Vector3(0f, 0f, 0.007f);

	// Token: 0x04003D77 RID: 15735
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 topRotation = new Vector3(40f, 0f, 0f);

	// Token: 0x04003D78 RID: 15736
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 bottomRotation = new Vector3(-10f, 0f, -10f);

	// Token: 0x04003D79 RID: 15737
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EyeLidController.BlinkState blinkState;

	// Token: 0x02000A55 RID: 2645
	[PublicizedFrom(EAccessModifier.Private)]
	public enum BlinkState
	{
		// Token: 0x04003D7B RID: 15739
		Open,
		// Token: 0x04003D7C RID: 15740
		Closing,
		// Token: 0x04003D7D RID: 15741
		Opening
	}
}
