using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000437 RID: 1079
	[NativeClass("BatchMeshID")]
	[RequiredByNativeCode(Optional = true, GenerateProxy = true)]
	[NativeHeader("Runtime/Camera/BatchRendererGroup.h")]
	public struct BatchMeshID : IEquatable<BatchMeshID>
	{
		// Token: 0x0600244F RID: 9295 RVA: 0x0003D000 File Offset: 0x0003B200
		public override int GetHashCode()
		{
			return this.value.GetHashCode();
		}

		// Token: 0x06002450 RID: 9296 RVA: 0x0003D020 File Offset: 0x0003B220
		public override bool Equals(object obj)
		{
			bool flag = obj is BatchMeshID;
			return flag && this.Equals((BatchMeshID)obj);
		}

		// Token: 0x06002451 RID: 9297 RVA: 0x0003D050 File Offset: 0x0003B250
		public bool Equals(BatchMeshID other)
		{
			return this.value == other.value;
		}

		// Token: 0x06002452 RID: 9298 RVA: 0x0003D070 File Offset: 0x0003B270
		public int CompareTo(BatchMeshID other)
		{
			return this.value.CompareTo(other.value);
		}

		// Token: 0x06002453 RID: 9299 RVA: 0x0003D094 File Offset: 0x0003B294
		public static bool operator ==(BatchMeshID a, BatchMeshID b)
		{
			return a.Equals(b);
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x0003D0B0 File Offset: 0x0003B2B0
		public static bool operator !=(BatchMeshID a, BatchMeshID b)
		{
			return !a.Equals(b);
		}

		// Token: 0x04000D44 RID: 3396
		public static readonly BatchMeshID Null = new BatchMeshID
		{
			value = 0U
		};

		// Token: 0x04000D45 RID: 3397
		public uint value;
	}
}
