using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x0200193A RID: 6458
	public class PathFinder
	{
		// Token: 0x0600C6DB RID: 50907 RVA: 0x0048F7B0 File Offset: 0x0048D9B0
		public PathFinder(PathInfo _pathInfo, bool _bDrn, bool _canClimbLadders, bool _bCanClimbWalls)
		{
			this.pathInfo = _pathInfo;
			this.canEntityDrown = _bDrn;
			this.canClimbWalls = _bCanClimbWalls;
			this.canClimbLadders = _canClimbLadders;
		}

		// Token: 0x0600C6DC RID: 50908 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Calculate(Vector3 _fromPos)
		{
		}

		// Token: 0x0600C6DD RID: 50909 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Destruct()
		{
		}

		// Token: 0x0400960D RID: 38413
		[PublicizedFrom(EAccessModifier.Protected)]
		public PathInfo pathInfo;

		// Token: 0x0400960E RID: 38414
		public bool canClimbWalls;

		// Token: 0x0400960F RID: 38415
		public bool canClimbLadders;

		// Token: 0x04009610 RID: 38416
		public bool canEntityDrown;
	}
}
