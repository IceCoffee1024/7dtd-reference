using System;
using UnityEngine;

namespace ShinyScreenSpaceRaytracedReflections
{
	// Token: 0x0200163F RID: 5695
	public class Rotate : MonoBehaviour
	{
		// Token: 0x0600B2AE RID: 45742 RVA: 0x0042B17E File Offset: 0x0042937E
		[PublicizedFrom(EAccessModifier.Private)]
		public void Update()
		{
			base.transform.Rotate(this.axis * (Time.deltaTime * this.speed));
		}

		// Token: 0x04008644 RID: 34372
		public Vector3 axis = Vector3.up;

		// Token: 0x04008645 RID: 34373
		public float speed = 60f;
	}
}
