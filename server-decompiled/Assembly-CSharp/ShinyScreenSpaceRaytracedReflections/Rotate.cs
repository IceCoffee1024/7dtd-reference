using System;
using UnityEngine;

namespace ShinyScreenSpaceRaytracedReflections
{
	// Token: 0x02001651 RID: 5713
	public class Rotate : MonoBehaviour
	{
		// Token: 0x0600B306 RID: 45830 RVA: 0x0042D582 File Offset: 0x0042B782
		[PublicizedFrom(EAccessModifier.Private)]
		public void Update()
		{
			base.transform.Rotate(this.axis * (Time.deltaTime * this.speed));
		}

		// Token: 0x04008685 RID: 34437
		public Vector3 axis = Vector3.up;

		// Token: 0x04008686 RID: 34438
		public float speed = 60f;
	}
}
