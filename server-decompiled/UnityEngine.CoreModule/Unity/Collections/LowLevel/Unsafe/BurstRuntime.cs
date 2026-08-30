using System;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x020000A7 RID: 167
	internal static class BurstRuntime
	{
		// Token: 0x06000349 RID: 841 RVA: 0x0000649C File Offset: 0x0000469C
		public static long GetHashCode64<T>()
		{
			return BurstRuntime.HashCode64<T>.Value;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x000064B4 File Offset: 0x000046B4
		internal static long HashStringWithFNV1A64(string text)
		{
			ulong num = 14695981039346656037UL;
			foreach (char c in text)
			{
				num = 1099511628211UL * (num ^ (ulong)((byte)(c & 'ÿ')));
				num = 1099511628211UL * (num ^ (ulong)((byte)(c >> 8)));
			}
			return (long)num;
		}

		// Token: 0x020000A8 RID: 168
		private struct HashCode64<T>
		{
			// Token: 0x04000243 RID: 579
			public static readonly long Value = BurstRuntime.HashStringWithFNV1A64(typeof(T).AssemblyQualifiedName);
		}
	}
}
