using System;
using UnityEngine;

// Token: 0x02000558 RID: 1368
public class RotateObject : MonoBehaviour
{
	// Token: 0x06002C70 RID: 11376 RVA: 0x0011AA9F File Offset: 0x00118C9F
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		if (!this.rotateTransform)
		{
			this.rotateTransform = base.transform;
		}
	}

	// Token: 0x06002C71 RID: 11377 RVA: 0x0011AABA File Offset: 0x00118CBA
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		this.rotateTransform.localEulerAngles += this.RPM * (Time.deltaTime * 6f);
	}

	// Token: 0x04002255 RID: 8789
	public Transform rotateTransform;

	// Token: 0x04002256 RID: 8790
	public Vector3 RPM;
}
