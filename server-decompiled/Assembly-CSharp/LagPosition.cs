using System;
using UnityEngine;

// Token: 0x0200004F RID: 79
public class LagPosition : MonoBehaviour
{
	// Token: 0x060001A5 RID: 421 RVA: 0x00010076 File Offset: 0x0000E276
	public void OnRepositionEnd()
	{
		this.Interpolate(1000f);
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x00010084 File Offset: 0x0000E284
	[PublicizedFrom(EAccessModifier.Private)]
	public void Interpolate(float delta)
	{
		Transform parent = this.mTrans.parent;
		if (parent != null)
		{
			Vector3 vector = parent.position + parent.rotation * this.mRelative;
			this.mAbsolute.x = Mathf.Lerp(this.mAbsolute.x, vector.x, Mathf.Clamp01(delta * this.speed.x));
			this.mAbsolute.y = Mathf.Lerp(this.mAbsolute.y, vector.y, Mathf.Clamp01(delta * this.speed.y));
			this.mAbsolute.z = Mathf.Lerp(this.mAbsolute.z, vector.z, Mathf.Clamp01(delta * this.speed.z));
			this.mTrans.position = this.mAbsolute;
		}
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x00010170 File Offset: 0x0000E370
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.mTrans = base.transform;
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x0001017E File Offset: 0x0000E37E
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		if (this.mStarted)
		{
			this.ResetPosition();
		}
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x0001018E File Offset: 0x0000E38E
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.mStarted = true;
		this.ResetPosition();
	}

	// Token: 0x060001AA RID: 426 RVA: 0x0001019D File Offset: 0x0000E39D
	public void ResetPosition()
	{
		this.mAbsolute = this.mTrans.position;
		this.mRelative = this.mTrans.localPosition;
	}

	// Token: 0x060001AB RID: 427 RVA: 0x000101C1 File Offset: 0x0000E3C1
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		this.Interpolate(this.ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime);
	}

	// Token: 0x04000253 RID: 595
	public Vector3 speed = new Vector3(10f, 10f, 10f);

	// Token: 0x04000254 RID: 596
	public bool ignoreTimeScale;

	// Token: 0x04000255 RID: 597
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform mTrans;

	// Token: 0x04000256 RID: 598
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 mRelative;

	// Token: 0x04000257 RID: 599
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 mAbsolute;

	// Token: 0x04000258 RID: 600
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool mStarted;
}
