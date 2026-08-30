using System;
using UnityEngine;

// Token: 0x0200054E RID: 1358
public class RotateObject : MonoBehaviour
{
	// Token: 0x06002C72 RID: 11378 RVA: 0x00119743 File Offset: 0x00117943
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		if (!this.rotateTransform)
		{
			this.rotateTransform = base.transform;
		}
	}

	// Token: 0x06002C73 RID: 11379 RVA: 0x0011975E File Offset: 0x0011795E
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		this.rotateTransform.localEulerAngles += this.RPM * (Time.deltaTime * 6f);
	}

	// Token: 0x04002236 RID: 8758
	public Transform rotateTransform;

	// Token: 0x04002237 RID: 8759
	public Vector3 RPM;
}
