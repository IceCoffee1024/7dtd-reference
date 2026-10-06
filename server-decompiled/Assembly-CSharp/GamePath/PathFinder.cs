using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x02001909 RID: 6409
	public class PathFinder
	{
		// Token: 0x0600C5C7 RID: 50631 RVA: 0x0048BE04 File Offset: 0x0048A004
		public PathFinder(PathInfo _pathInfo, bool _bDrn, bool _canClimbLadders, bool _bCanClimbWalls)
		{
			this.pathInfo = _pathInfo;
			this.canEntityDrown = _bDrn;
			this.canClimbWalls = _bCanClimbWalls;
			this.canClimbLadders = _canClimbLadders;
		}

		// Token: 0x0600C5C8 RID: 50632 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Calculate(Vector3 _fromPos)
		{
		}

		// Token: 0x0600C5C9 RID: 50633 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Destruct()
		{
		}

		// Token: 0x04009524 RID: 38180
		[PublicizedFrom(EAccessModifier.Protected)]
		public PathInfo pathInfo;

		// Token: 0x04009525 RID: 38181
		public bool canClimbWalls;

		// Token: 0x04009526 RID: 38182
		public bool canClimbLadders;

		// Token: 0x04009527 RID: 38183
		public bool canEntityDrown;
	}
}
