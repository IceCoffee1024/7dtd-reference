using System;
using UnityEngine;

// Token: 0x02000549 RID: 1353
public class RotateObject : MonoBehaviour
{
	// Token: 0x06002C48 RID: 11336 RVA: 0x001186CF File Offset: 0x001168CF
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		if (!this.rotateTransform)
		{
			this.rotateTransform = base.transform;
		}
	}

	// Token: 0x06002C49 RID: 11337 RVA: 0x001186EA File Offset: 0x001168EA
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		this.rotateTransform.localEulerAngles += this.RPM * (Time.deltaTime * 6f);
	}

	// Token: 0x04002204 RID: 8708
	public Transform rotateTransform;

	// Token: 0x04002205 RID: 8709
	public Vector3 RPM;
}
