using System;
using Unity.Burst;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001771 RID: 6001
	[BurstCompile(CompileSynchronously = true)]
	public struct PathNode
	{
		// Token: 0x0600BA76 RID: 47734 RVA: 0x0044CDD3 File Offset: 0x0044AFD3
		public PathNode(Vector2i _position, float _travelledCost, float _totalCost, int _next)
		{
			this.position = _position;
			this.travelledCost = _travelledCost;
			this.totalCost = _totalCost;
			this.pathNext = _next;
			this.listNext = -1;
		}

		// Token: 0x0600BA77 RID: 47735 RVA: 0x0044CDF9 File Offset: 0x0044AFF9
		public void Set(Vector2i _position, float _travelledCost, float _totalCost, int _next)
		{
			this.position = _position;
			this.travelledCost = _travelledCost;
			this.totalCost = _totalCost;
			this.pathNext = _next;
		}

		// Token: 0x04008B2A RID: 35626
		public Vector2i position;

		// Token: 0x04008B2B RID: 35627
		public float travelledCost;

		// Token: 0x04008B2C RID: 35628
		public float totalCost;

		// Token: 0x04008B2D RID: 35629
		public int pathNext;

		// Token: 0x04008B2E RID: 35630
		public int listNext;
	}
}
