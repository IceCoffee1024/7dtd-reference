using System;
using Unity.Collections;

namespace UnityEngine
{
	// Token: 0x020001FA RID: 506
	internal struct TransformDispatchData : IDisposable
	{
		// Token: 0x06001719 RID: 5913 RVA: 0x0002660C File Offset: 0x0002480C
		public void Dispose()
		{
			this.transformedID.Dispose();
			this.parentID.Dispose();
			this.localToWorldMatrices.Dispose();
			this.positions.Dispose();
			this.rotations.Dispose();
			this.scales.Dispose();
		}

		// Token: 0x0400084D RID: 2125
		public NativeArray<int> transformedID;

		// Token: 0x0400084E RID: 2126
		public NativeArray<int> parentID;

		// Token: 0x0400084F RID: 2127
		public NativeArray<Matrix4x4> localToWorldMatrices;

		// Token: 0x04000850 RID: 2128
		public NativeArray<Vector3> positions;

		// Token: 0x04000851 RID: 2129
		public NativeArray<Quaternion> rotations;

		// Token: 0x04000852 RID: 2130
		public NativeArray<Vector3> scales;
	}
}
