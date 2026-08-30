using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000436 RID: 1078
	[NativeClass("BatchMaterialID")]
	[NativeHeader("Runtime/Camera/BatchRendererGroup.h")]
	[RequiredByNativeCode(Optional = true, GenerateProxy = true)]
	public struct BatchMaterialID : IEquatable<BatchMaterialID>
	{
		// Token: 0x06002448 RID: 9288 RVA: 0x0003CF0C File Offset: 0x0003B10C
		public override int GetHashCode()
		{
			return this.value.GetHashCode();
		}

		// Token: 0x06002449 RID: 9289 RVA: 0x0003CF2C File Offset: 0x0003B12C
		public override bool Equals(object obj)
		{
			bool flag = obj is BatchMaterialID;
			return flag && this.Equals((BatchMaterialID)obj);
		}

		// Token: 0x0600244A RID: 9290 RVA: 0x0003CF5C File Offset: 0x0003B15C
		public bool Equals(BatchMaterialID other)
		{
			return this.value == other.value;
		}

		// Token: 0x0600244B RID: 9291 RVA: 0x0003CF7C File Offset: 0x0003B17C
		public int CompareTo(BatchMaterialID other)
		{
			return this.value.CompareTo(other.value);
		}

		// Token: 0x0600244C RID: 9292 RVA: 0x0003CFA0 File Offset: 0x0003B1A0
		public static bool operator ==(BatchMaterialID a, BatchMaterialID b)
		{
			return a.Equals(b);
		}

		// Token: 0x0600244D RID: 9293 RVA: 0x0003CFBC File Offset: 0x0003B1BC
		public static bool operator !=(BatchMaterialID a, BatchMaterialID b)
		{
			return !a.Equals(b);
		}

		// Token: 0x04000D42 RID: 3394
		public static readonly BatchMaterialID Null = new BatchMaterialID
		{
			value = 0U
		};

		// Token: 0x04000D43 RID: 3395
		public uint value;
	}
}
