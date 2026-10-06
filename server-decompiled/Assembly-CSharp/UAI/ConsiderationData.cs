using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017B5 RID: 6069
	[Preserve]
	public class ConsiderationData
	{
		// Token: 0x0600BBEB RID: 48107 RVA: 0x0045B807 File Offset: 0x00459A07
		public ConsiderationData()
		{
			this.EntityTargets = new List<Entity>();
			this.WaypointTargets = new List<Vector3>();
		}

		// Token: 0x04008C98 RID: 35992
		public List<Entity> EntityTargets;

		// Token: 0x04008C99 RID: 35993
		public List<Vector3> WaypointTargets;
	}
}
