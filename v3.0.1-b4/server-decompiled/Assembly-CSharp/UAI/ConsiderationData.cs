using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x02001763 RID: 5987
	[Preserve]
	public class ConsiderationData
	{
		// Token: 0x0600B911 RID: 47377 RVA: 0x0045156F File Offset: 0x0044F76F
		public ConsiderationData()
		{
			this.EntityTargets = new List<Entity>();
			this.WaypointTargets = new List<Vector3>();
		}

		// Token: 0x04008AB7 RID: 35511
		public List<Entity> EntityTargets;

		// Token: 0x04008AB8 RID: 35512
		public List<Vector3> WaypointTargets;
	}
}
