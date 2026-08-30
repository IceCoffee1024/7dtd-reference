using System;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200016A RID: 362
	[NativeClass("GfxBufferID")]
	[NativeHeader("Runtime/GfxDevice/GfxDeviceTypes.h")]
	public readonly struct GraphicsBufferHandle : IEquatable<GraphicsBufferHandle>
	{
		// Token: 0x06000F13 RID: 3859 RVA: 0x000151E8 File Offset: 0x000133E8
		public override int GetHashCode()
		{
			return this.value.GetHashCode();
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x00015208 File Offset: 0x00013408
		public override bool Equals(object obj)
		{
			bool flag = obj is GraphicsBufferHandle;
			return flag && this.Equals((GraphicsBufferHandle)obj);
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x00015238 File Offset: 0x00013438
		public bool Equals(GraphicsBufferHandle other)
		{
			return this.value == other.value;
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x00015258 File Offset: 0x00013458
		public int CompareTo(GraphicsBufferHandle other)
		{
			return this.value.CompareTo(other.value);
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x0001527C File Offset: 0x0001347C
		public static bool operator ==(GraphicsBufferHandle a, GraphicsBufferHandle b)
		{
			return a.Equals(b);
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x00015298 File Offset: 0x00013498
		public static bool operator !=(GraphicsBufferHandle a, GraphicsBufferHandle b)
		{
			return !a.Equals(b);
		}

		// Token: 0x0400047B RID: 1147
		public readonly uint value;
	}
}
