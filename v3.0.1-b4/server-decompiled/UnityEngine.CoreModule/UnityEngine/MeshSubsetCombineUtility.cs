using System;
using System.Collections.Generic;

namespace UnityEngine
{
	// Token: 0x02000285 RID: 645
	internal class MeshSubsetCombineUtility
	{
		// Token: 0x02000286 RID: 646
		public struct MeshInstance
		{
			// Token: 0x04000934 RID: 2356
			public int meshInstanceID;

			// Token: 0x04000935 RID: 2357
			public int rendererInstanceID;

			// Token: 0x04000936 RID: 2358
			public int additionalVertexStreamsMeshInstanceID;

			// Token: 0x04000937 RID: 2359
			public int enlightenVertexStreamMeshInstanceID;

			// Token: 0x04000938 RID: 2360
			public Matrix4x4 transform;

			// Token: 0x04000939 RID: 2361
			public Vector4 lightmapScaleOffset;

			// Token: 0x0400093A RID: 2362
			public Vector4 realtimeLightmapScaleOffset;
		}

		// Token: 0x02000287 RID: 647
		public struct SubMeshInstance
		{
			// Token: 0x0400093B RID: 2363
			public int meshInstanceID;

			// Token: 0x0400093C RID: 2364
			public int vertexOffset;

			// Token: 0x0400093D RID: 2365
			public int gameObjectInstanceID;

			// Token: 0x0400093E RID: 2366
			public int subMeshIndex;

			// Token: 0x0400093F RID: 2367
			public Matrix4x4 transform;
		}

		// Token: 0x02000288 RID: 648
		public struct MeshContainer
		{
			// Token: 0x04000940 RID: 2368
			public GameObject gameObject;

			// Token: 0x04000941 RID: 2369
			public MeshSubsetCombineUtility.MeshInstance instance;

			// Token: 0x04000942 RID: 2370
			public List<MeshSubsetCombineUtility.SubMeshInstance> subMeshInstances;
		}
	}
}
