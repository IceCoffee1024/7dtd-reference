using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018B7 RID: 6327
	public class PathFinder
	{
		// Token: 0x0600C2E8 RID: 49896 RVA: 0x00481790 File Offset: 0x0047F990
		public PathFinder(PathInfo _pathInfo, bool _bDrn, bool _canClimbLadders, bool _bCanClimbWalls)
		{
			this.pathInfo = _pathInfo;
			this.canEntityDrown = _bDrn;
			this.canClimbWalls = _bCanClimbWalls;
			this.canClimbLadders = _canClimbLadders;
		}

		// Token: 0x0600C2E9 RID: 49897 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Calculate(Vector3 _fromPos)
		{
		}

		// Token: 0x0600C2EA RID: 49898 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Destruct()
		{
		}

		// Token: 0x04009335 RID: 37685
		[PublicizedFrom(EAccessModifier.Protected)]
		public PathInfo pathInfo;

		// Token: 0x04009336 RID: 37686
		public bool canClimbWalls;

		// Token: 0x04009337 RID: 37687
		public bool canClimbLadders;

		// Token: 0x04009338 RID: 37688
		public bool canEntityDrown;
	}
}
