using System;
using Unity.Burst;

namespace WorldGenerationEngineFinal
{
	// Token: 0x020016EE RID: 5870
	[BurstCompile(CompileSynchronously = true)]
	public struct PathNode
	{
		// Token: 0x0600B688 RID: 46728 RVA: 0x0043F18F File Offset: 0x0043D38F
		public PathNode(Vector2i _position, float _travelledCost, float _totalCost, int _next)
		{
			this.position = _position;
			this.travelledCost = _travelledCost;
			this.totalCost = _totalCost;
			this.pathNext = _next;
			this.listNext = -1;
		}

		// Token: 0x0600B689 RID: 46729 RVA: 0x0043F1B5 File Offset: 0x0043D3B5
		public void Set(Vector2i _position, float _travelledCost, float _totalCost, int _next)
		{
			this.position = _position;
			this.travelledCost = _travelledCost;
			this.totalCost = _totalCost;
			this.pathNext = _next;
		}

		// Token: 0x04008860 RID: 34912
		public Vector2i position;

		// Token: 0x04008861 RID: 34913
		public float travelledCost;

		// Token: 0x04008862 RID: 34914
		public float totalCost;

		// Token: 0x04008863 RID: 34915
		public int pathNext;

		// Token: 0x04008864 RID: 34916
		public int listNext;
	}
}
