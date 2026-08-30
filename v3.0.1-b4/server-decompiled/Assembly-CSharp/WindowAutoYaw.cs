using System;
using UnityEngine;

// Token: 0x0200005C RID: 92
[AddComponentMenu("NGUI/Examples/Window Auto-Yaw")]
public class WindowAutoYaw : MonoBehaviour
{
	// Token: 0x060001D0 RID: 464 RVA: 0x00010B3A File Offset: 0x0000ED3A
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.mTrans.localRotation = Quaternion.identity;
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x00010B4C File Offset: 0x0000ED4C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.mTrans = base.transform;
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x00010B80 File Offset: 0x0000ED80
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.uiCamera != null)
		{
			Vector3 vector = this.uiCamera.WorldToViewportPoint(this.mTrans.position);
			this.mTrans.localRotation = Quaternion.Euler(0f, (vector.x * 2f - 1f) * this.yawAmount, 0f);
		}
	}

	// Token: 0x0400027A RID: 634
	public int updateOrder;

	// Token: 0x0400027B RID: 635
	public Camera uiCamera;

	// Token: 0x0400027C RID: 636
	public float yawAmount = 20f;

	// Token: 0x0400027D RID: 637
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform mTrans;
}
