using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x02001789 RID: 6025
	[Preserve]
	public class ConsiderationData
	{
		// Token: 0x0600BA94 RID: 47764 RVA: 0x004572E7 File Offset: 0x004554E7
		public ConsiderationData()
		{
			this.EntityTargets = new List<Entity>();
			this.WaypointTargets = new List<Vector3>();
		}

		// Token: 0x04008BD3 RID: 35795
		public List<Entity> EntityTargets;

		// Token: 0x04008BD4 RID: 35796
		public List<Vector3> WaypointTargets;
	}
}
