using System;
using UnityEngine;

namespace ShinyScreenSpaceRaytracedReflections
{
	// Token: 0x02001605 RID: 5637
	public class Rotate : MonoBehaviour
	{
		// Token: 0x0600B11F RID: 45343 RVA: 0x00422162 File Offset: 0x00420362
		[PublicizedFrom(EAccessModifier.Private)]
		public void Update()
		{
			base.transform.Rotate(this.axis * (Time.deltaTime * this.speed));
		}

		// Token: 0x04008515 RID: 34069
		public Vector3 axis = Vector3.up;

		// Token: 0x04008516 RID: 34070
		public float speed = 60f;
	}
}
