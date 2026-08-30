using System;
using UnityEngine;

// Token: 0x0200005D RID: 93
[AddComponentMenu("NGUI/Examples/Window Drag Tilt")]
public class WindowDragTilt : MonoBehaviour
{
	// Token: 0x060001D4 RID: 468 RVA: 0x00010BF8 File Offset: 0x0000EDF8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.mTrans = base.transform;
		this.mLastPos = this.mTrans.position;
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x00010C18 File Offset: 0x0000EE18
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		Vector3 vector = this.mTrans.position - this.mLastPos;
		this.mLastPos = this.mTrans.position;
		this.mAngle += vector.x * this.degrees;
		this.mAngle = NGUIMath.SpringLerp(this.mAngle, 0f, 20f, Time.deltaTime);
		this.mTrans.localRotation = Quaternion.Euler(0f, 0f, -this.mAngle);
	}

	// Token: 0x0400027E RID: 638
	public int updateOrder;

	// Token: 0x0400027F RID: 639
	public float degrees = 30f;

	// Token: 0x04000280 RID: 640
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 mLastPos;

	// Token: 0x04000281 RID: 641
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform mTrans;

	// Token: 0x04000282 RID: 642
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float mAngle;
}
