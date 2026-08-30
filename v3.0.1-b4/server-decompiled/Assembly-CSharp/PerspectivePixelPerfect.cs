using System;
using UnityEngine;

// Token: 0x02000055 RID: 85
public class PerspectivePixelPerfect : MonoBehaviour
{
	// Token: 0x060001BC RID: 444 RVA: 0x00010518 File Offset: 0x0000E718
	[ContextMenu("Execute")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		Transform transform = base.transform;
		Camera main = Camera.main;
		float nearClipPlane = main.nearClipPlane;
		float farClipPlane = main.farClipPlane;
		float num = Mathf.Lerp(nearClipPlane, farClipPlane, this.bias);
		float fieldOfView = main.fieldOfView;
		float num2 = Mathf.Tan(0.017453292f * fieldOfView * 0.5f) * num;
		transform.localPosition = new Vector3(0f, 0f, num);
		transform.localScale = new Vector3(num2, num2, 1f);
	}

	// Token: 0x04000268 RID: 616
	[Tooltip("Bias is a value above 0 that determines how far offset the object will be from the near clip, in percent (near to far clip)")]
	public float bias = 0.001f;
}
