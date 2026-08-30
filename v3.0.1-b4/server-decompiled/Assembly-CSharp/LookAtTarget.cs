using System;
using UnityEngine;

// Token: 0x02000052 RID: 82
[AddComponentMenu("NGUI/Examples/Look At Target")]
public class LookAtTarget : MonoBehaviour
{
	// Token: 0x060001B4 RID: 436 RVA: 0x000102F0 File Offset: 0x0000E4F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.mTrans = base.transform;
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x00010300 File Offset: 0x0000E500
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		if (this.target != null)
		{
			Vector3 vector = this.target.position - this.mTrans.position;
			if (vector.magnitude > 0.001f)
			{
				Quaternion quaternion = Quaternion.LookRotation(vector);
				this.mTrans.rotation = Quaternion.Slerp(this.mTrans.rotation, quaternion, Mathf.Clamp01(this.speed * Time.deltaTime));
			}
		}
	}

	// Token: 0x0400025F RID: 607
	public int level;

	// Token: 0x04000260 RID: 608
	public Transform target;

	// Token: 0x04000261 RID: 609
	public float speed = 8f;

	// Token: 0x04000262 RID: 610
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform mTrans;
}
