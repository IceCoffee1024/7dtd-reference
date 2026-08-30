using System;

namespace Unity.Collections
{
	// Token: 0x020000A0 RID: 160
	public static class NativeSliceExtensions
	{
		// Token: 0x06000316 RID: 790 RVA: 0x00005E5C File Offset: 0x0000405C
		public static NativeSlice<T> Slice<T>(this NativeArray<T> thisArray) where T : struct
		{
			return new NativeSlice<T>(thisArray);
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00005E74 File Offset: 0x00004074
		public static NativeSlice<T> Slice<T>(this NativeArray<T> thisArray, int start) where T : struct
		{
			return new NativeSlice<T>(thisArray, start);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00005E90 File Offset: 0x00004090
		public static NativeSlice<T> Slice<T>(this NativeArray<T> thisArray, int start, int length) where T : struct
		{
			return new NativeSlice<T>(thisArray, start, length);
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00005EAC File Offset: 0x000040AC
		public static NativeSlice<T> Slice<T>(this NativeSlice<T> thisSlice) where T : struct
		{
			return thisSlice;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00005EC0 File Offset: 0x000040C0
		public static NativeSlice<T> Slice<T>(this NativeSlice<T> thisSlice, int start) where T : struct
		{
			return new NativeSlice<T>(thisSlice, start);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00005EDC File Offset: 0x000040DC
		public static NativeSlice<T> Slice<T>(this NativeSlice<T> thisSlice, int start, int length) where T : struct
		{
			return new NativeSlice<T>(thisSlice, start, length);
		}
	}
}
