using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000435 RID: 1077
	[RequiredByNativeCode(Optional = true, GenerateProxy = true)]
	[NativeClass("BatchID")]
	[NativeHeader("Runtime/Camera/BatchRendererGroup.h")]
	public struct BatchID : IEquatable<BatchID>
	{
		// Token: 0x06002441 RID: 9281 RVA: 0x0003CE18 File Offset: 0x0003B018
		public override int GetHashCode()
		{
			return this.value.GetHashCode();
		}

		// Token: 0x06002442 RID: 9282 RVA: 0x0003CE38 File Offset: 0x0003B038
		public override bool Equals(object obj)
		{
			bool flag = obj is BatchID;
			return flag && this.Equals((BatchID)obj);
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x0003CE68 File Offset: 0x0003B068
		public bool Equals(BatchID other)
		{
			return this.value == other.value;
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x0003CE88 File Offset: 0x0003B088
		public int CompareTo(BatchID other)
		{
			return this.value.CompareTo(other.value);
		}

		// Token: 0x06002445 RID: 9285 RVA: 0x0003CEAC File Offset: 0x0003B0AC
		public static bool operator ==(BatchID a, BatchID b)
		{
			return a.Equals(b);
		}

		// Token: 0x06002446 RID: 9286 RVA: 0x0003CEC8 File Offset: 0x0003B0C8
		public static bool operator !=(BatchID a, BatchID b)
		{
			return !a.Equals(b);
		}

		// Token: 0x04000D40 RID: 3392
		public static readonly BatchID Null = new BatchID
		{
			value = 0U
		};

		// Token: 0x04000D41 RID: 3393
		public uint value;
	}
}
