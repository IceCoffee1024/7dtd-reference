using System;
using Unity.Burst;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001740 RID: 5952
	[BurstCompile(CompileSynchronously = true)]
	public struct PathNode
	{
		// Token: 0x0600B962 RID: 47458 RVA: 0x00449427 File Offset: 0x00447627
		public PathNode(Vector2i _position, float _travelledCost, float _totalCost, int _next)
		{
			this.position = _position;
			this.travelledCost = _travelledCost;
			this.totalCost = _totalCost;
			this.pathNext = _next;
			this.listNext = -1;
		}

		// Token: 0x0600B963 RID: 47459 RVA: 0x0044944D File Offset: 0x0044764D
		public void Set(Vector2i _position, float _travelledCost, float _totalCost, int _next)
		{
			this.position = _position;
			this.travelledCost = _travelledCost;
			this.totalCost = _totalCost;
			this.pathNext = _next;
		}

		// Token: 0x04008A41 RID: 35393
		public Vector2i position;

		// Token: 0x04008A42 RID: 35394
		public float travelledCost;

		// Token: 0x04008A43 RID: 35395
		public float totalCost;

		// Token: 0x04008A44 RID: 35396
		public int pathNext;

		// Token: 0x04008A45 RID: 35397
		public int listNext;
	}
}
