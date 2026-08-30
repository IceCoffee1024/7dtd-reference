using System;
using UnityEngine;

// Token: 0x02000050 RID: 80
[AddComponentMenu("NGUI/Examples/Lag Rotation")]
public class LagRotation : MonoBehaviour
{
	// Token: 0x060001AD RID: 429 RVA: 0x000101FF File Offset: 0x0000E3FF
	public void OnRepositionEnd()
	{
		this.Interpolate(1000f);
	}

	// Token: 0x060001AE RID: 430 RVA: 0x0001020C File Offset: 0x0000E40C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Interpolate(float delta)
	{
		if (this.mTrans != null)
		{
			Transform parent = this.mTrans.parent;
			if (parent != null)
			{
				this.mAbsolute = Quaternion.Slerp(this.mAbsolute, parent.rotation * this.mRelative, delta * this.speed);
				this.mTrans.rotation = this.mAbsolute;
			}
		}
	}

	// Token: 0x060001AF RID: 431 RVA: 0x00010277 File Offset: 0x0000E477
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.mTrans = base.transform;
		this.mRelative = this.mTrans.localRotation;
		this.mAbsolute = this.mTrans.rotation;
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x000102A7 File Offset: 0x0000E4A7
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		this.Interpolate(this.ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime);
	}

	// Token: 0x04000259 RID: 601
	public float speed = 10f;

	// Token: 0x0400025A RID: 602
	public bool ignoreTimeScale;

	// Token: 0x0400025B RID: 603
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform mTrans;

	// Token: 0x0400025C RID: 604
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Quaternion mRelative;

	// Token: 0x0400025D RID: 605
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Quaternion mAbsolute;
}
