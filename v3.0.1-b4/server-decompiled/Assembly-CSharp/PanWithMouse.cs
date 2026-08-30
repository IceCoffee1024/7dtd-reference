using System;
using UnityEngine;

// Token: 0x02000054 RID: 84
[AddComponentMenu("NGUI/Examples/Pan With Mouse")]
public class PanWithMouse : MonoBehaviour
{
	// Token: 0x060001B9 RID: 441 RVA: 0x000103C3 File Offset: 0x0000E5C3
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.mTrans = base.transform;
		this.mStart = this.mTrans.localRotation;
	}

	// Token: 0x060001BA RID: 442 RVA: 0x000103E4 File Offset: 0x0000E5E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		float deltaTime = RealTime.deltaTime;
		Vector3 vector = UICamera.lastEventPosition;
		float num = (float)Screen.width * 0.5f;
		float num2 = (float)Screen.height * 0.5f;
		if (this.range < 0.1f)
		{
			this.range = 0.1f;
		}
		float num3 = Mathf.Clamp((vector.x - num) / num / this.range, -1f, 1f);
		float num4 = Mathf.Clamp((vector.y - num2) / num2 / this.range, -1f, 1f);
		this.mRot = Vector2.Lerp(this.mRot, new Vector2(num3, num4), deltaTime * 5f);
		this.mTrans.localRotation = this.mStart * Quaternion.Euler(-this.mRot.y * this.degrees.y, this.mRot.x * this.degrees.x, 0f);
	}

	// Token: 0x04000263 RID: 611
	public Vector2 degrees = new Vector2(5f, 3f);

	// Token: 0x04000264 RID: 612
	public float range = 1f;

	// Token: 0x04000265 RID: 613
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform mTrans;

	// Token: 0x04000266 RID: 614
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Quaternion mStart;

	// Token: 0x04000267 RID: 615
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 mRot = Vector2.zero;
}
