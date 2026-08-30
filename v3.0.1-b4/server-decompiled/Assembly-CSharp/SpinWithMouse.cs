using System;
using UnityEngine;

// Token: 0x02000059 RID: 89
[AddComponentMenu("NGUI/Examples/Spin With Mouse")]
public class SpinWithMouse : MonoBehaviour
{
	// Token: 0x060001C8 RID: 456 RVA: 0x000108E1 File Offset: 0x0000EAE1
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.mTrans = base.transform;
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x000108F0 File Offset: 0x0000EAF0
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDrag(Vector2 delta)
	{
		UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
		if (this.target != null)
		{
			this.target.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * this.speed, 0f) * this.target.localRotation;
			return;
		}
		this.mTrans.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * this.speed, 0f) * this.mTrans.localRotation;
	}

	// Token: 0x04000273 RID: 627
	public Transform target;

	// Token: 0x04000274 RID: 628
	public float speed = 1f;

	// Token: 0x04000275 RID: 629
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform mTrans;
}
