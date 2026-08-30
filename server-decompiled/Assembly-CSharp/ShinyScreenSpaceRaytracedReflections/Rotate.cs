using System;
using UnityEngine;

namespace ShinyScreenSpaceRaytracedReflections
{
	// Token: 0x02001620 RID: 5664
	public class Rotate : MonoBehaviour
	{
		// Token: 0x0600B21C RID: 45596 RVA: 0x004274A2 File Offset: 0x004256A2
		[PublicizedFrom(EAccessModifier.Private)]
		public void Update()
		{
			base.transform.Rotate(this.axis * (Time.deltaTime * this.speed));
		}

		// Token: 0x040085EE RID: 34286
		public Vector3 axis = Vector3.up;

		// Token: 0x040085EF RID: 34287
		public float speed = 60f;
	}
}
