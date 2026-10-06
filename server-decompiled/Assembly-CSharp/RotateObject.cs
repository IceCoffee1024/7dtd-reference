using System;
using UnityEngine;

// Token: 0x02000554 RID: 1364
public class RotateObject : MonoBehaviour
{
	// Token: 0x06002C68 RID: 11368 RVA: 0x0011A513 File Offset: 0x00118713
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		if (!this.rotateTransform)
		{
			this.rotateTransform = base.transform;
		}
	}

	// Token: 0x06002C69 RID: 11369 RVA: 0x0011A52E File Offset: 0x0011872E
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		this.rotateTransform.localEulerAngles += this.RPM * (Time.deltaTime * 6f);
	}

	// Token: 0x0400223E RID: 8766
	public Transform rotateTransform;

	// Token: 0x0400223F RID: 8767
	public Vector3 RPM;
}
