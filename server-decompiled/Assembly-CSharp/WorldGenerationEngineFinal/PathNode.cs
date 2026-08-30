using System;
using Unity.Burst;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001714 RID: 5908
	[BurstCompile(CompileSynchronously = true)]
	public struct PathNode
	{
		// Token: 0x0600B80B RID: 47115 RVA: 0x00444F07 File Offset: 0x00443107
		public PathNode(Vector2i _position, float _travelledCost, float _totalCost, int _next)
		{
			this.position = _position;
			this.travelledCost = _travelledCost;
			this.totalCost = _totalCost;
			this.pathNext = _next;
			this.listNext = -1;
		}

		// Token: 0x0600B80C RID: 47116 RVA: 0x00444F2D File Offset: 0x0044312D
		public void Set(Vector2i _position, float _travelledCost, float _totalCost, int _next)
		{
			this.position = _position;
			this.travelledCost = _travelledCost;
			this.totalCost = _totalCost;
			this.pathNext = _next;
		}

		// Token: 0x0400897C RID: 35196
		public Vector2i position;

		// Token: 0x0400897D RID: 35197
		public float travelledCost;

		// Token: 0x0400897E RID: 35198
		public float totalCost;

		// Token: 0x0400897F RID: 35199
		public int pathNext;

		// Token: 0x04008980 RID: 35200
		public int listNext;
	}
}
