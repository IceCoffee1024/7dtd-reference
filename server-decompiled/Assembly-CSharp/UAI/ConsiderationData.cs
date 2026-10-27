using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017E6 RID: 6118
	[Preserve]
	public class ConsiderationData
	{
		// Token: 0x0600BCFF RID: 48383 RVA: 0x0045F1B3 File Offset: 0x0045D3B3
		public ConsiderationData()
		{
			this.EntityTargets = new List<Entity>();
			this.WaypointTargets = new List<Vector3>();
		}

		// Token: 0x04008D81 RID: 36225
		public List<Entity> EntityTargets;

		// Token: 0x04008D82 RID: 36226
		public List<Vector3> WaypointTargets;
	}
}
