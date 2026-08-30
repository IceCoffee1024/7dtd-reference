using System;
using UnityEngine;

// Token: 0x02000058 RID: 88
[AddComponentMenu("NGUI/Examples/Spin")]
public class Spin : MonoBehaviour
{
	// Token: 0x060001C3 RID: 451 RVA: 0x000107F1 File Offset: 0x0000E9F1
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.mTrans = base.transform;
		this.mRb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x0001080B File Offset: 0x0000EA0B
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.mRb == null)
		{
			this.ApplyDelta(this.ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime);
		}
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x00010835 File Offset: 0x0000EA35
	[PublicizedFrom(EAccessModifier.Private)]
	public void FixedUpdate()
	{
		if (this.mRb != null)
		{
			this.ApplyDelta(Time.deltaTime);
		}
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x00010850 File Offset: 0x0000EA50
	public void ApplyDelta(float delta)
	{
		delta *= 360f;
		Quaternion quaternion = Quaternion.Euler(this.rotationsPerSecond * delta);
		if (this.mRb == null)
		{
			this.mTrans.rotation = this.mTrans.rotation * quaternion;
			return;
		}
		this.mRb.MoveRotation(this.mRb.rotation * quaternion);
	}

	// Token: 0x0400026F RID: 623
	public Vector3 rotationsPerSecond = new Vector3(0f, 0.1f, 0f);

	// Token: 0x04000270 RID: 624
	public bool ignoreTimeScale;

	// Token: 0x04000271 RID: 625
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Rigidbody mRb;

	// Token: 0x04000272 RID: 626
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform mTrans;
}
