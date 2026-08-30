using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018DD RID: 6365
	public class PathFinder
	{
		// Token: 0x0600C46F RID: 50287 RVA: 0x004878BC File Offset: 0x00485ABC
		public PathFinder(PathInfo _pathInfo, bool _bDrn, bool _canClimbLadders, bool _bCanClimbWalls)
		{
			this.pathInfo = _pathInfo;
			this.canEntityDrown = _bDrn;
			this.canClimbWalls = _bCanClimbWalls;
			this.canClimbLadders = _canClimbLadders;
		}

		// Token: 0x0600C470 RID: 50288 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Calculate(Vector3 _fromPos)
		{
		}

		// Token: 0x0600C471 RID: 50289 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Destruct()
		{
		}

		// Token: 0x0400945E RID: 37982
		[PublicizedFrom(EAccessModifier.Protected)]
		public PathInfo pathInfo;

		// Token: 0x0400945F RID: 37983
		public bool canClimbWalls;

		// Token: 0x04009460 RID: 37984
		public bool canClimbLadders;

		// Token: 0x04009461 RID: 37985
		public bool canEntityDrown;
	}
}
