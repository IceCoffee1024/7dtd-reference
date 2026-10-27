using System;
using UnityEngine;

// Token: 0x02000A5E RID: 2654
public class EyeLidController : MonoBehaviour
{
	// Token: 0x06004FCB RID: 20427 RVA: 0x001E615C File Offset: 0x001E435C
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

	// Token: 0x06004FCC RID: 20428 RVA: 0x001E623C File Offset: 0x001E443C
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

	// Token: 0x04003D85 RID: 15749
	public Transform leftTopTransform;

	// Token: 0x04003D86 RID: 15750
	public Transform leftBottomTransform;

	// Token: 0x04003D87 RID: 15751
	public Transform rightTopTransform;

	// Token: 0x04003D88 RID: 15752
	public Transform rightBottomTransform;

	// Token: 0x04003D89 RID: 15753
	public Vector3 leftTopLocalPosition;

	// Token: 0x04003D8A RID: 15754
	public Vector3 leftBottomLocalPosition;

	// Token: 0x04003D8B RID: 15755
	public Quaternion leftTopRotation;

	// Token: 0x04003D8C RID: 15756
	public Quaternion leftBottomRotation;

	// Token: 0x04003D8D RID: 15757
	public Vector3 rightTopLocalPosition;

	// Token: 0x04003D8E RID: 15758
	public Vector3 rightBottomLocalPosition;

	// Token: 0x04003D8F RID: 15759
	public Quaternion rightTopRotation;

	// Token: 0x04003D90 RID: 15760
	public Quaternion rightBottomRotation;

	// Token: 0x04003D91 RID: 15761
	public bool autoRecordEyeBoneTransforms;

	// Token: 0x04003D92 RID: 15762
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float nextBlinkTime;

	// Token: 0x04003D93 RID: 15763
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float blinkProgress;

	// Token: 0x04003D94 RID: 15764
	public bool debug;

	// Token: 0x04003D95 RID: 15765
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityAlive entityAlive;

	// Token: 0x04003D96 RID: 15766
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameRandom random;

	// Token: 0x04003D97 RID: 15767
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 topOffset = new Vector3(0f, 0f, 0.007f);

	// Token: 0x04003D98 RID: 15768
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 topRotation = new Vector3(40f, 0f, 0f);

	// Token: 0x04003D99 RID: 15769
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 bottomRotation = new Vector3(-10f, 0f, -10f);

	// Token: 0x04003D9A RID: 15770
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EyeLidController.BlinkState blinkState;

	// Token: 0x02000A5F RID: 2655
	[PublicizedFrom(EAccessModifier.Private)]
	public enum BlinkState
	{
		// Token: 0x04003D9C RID: 15772
		Open,
		// Token: 0x04003D9D RID: 15773
		Closing,
		// Token: 0x04003D9E RID: 15774
		Opening
	}
}
