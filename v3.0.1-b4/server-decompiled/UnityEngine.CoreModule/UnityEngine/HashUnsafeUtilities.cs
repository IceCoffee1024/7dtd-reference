using System;

namespace UnityEngine
{
	// Token: 0x020001DC RID: 476
	public static class HashUnsafeUtilities
	{
		// Token: 0x060014A0 RID: 5280 RVA: 0x0001D7EB File Offset: 0x0001B9EB
		public unsafe static void ComputeHash128(void* data, ulong dataSize, ulong* hash1, ulong* hash2)
		{
			SpookyHash.Hash(data, dataSize, hash1, hash2);
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x0001D7F8 File Offset: 0x0001B9F8
		public unsafe static void ComputeHash128(void* data, ulong dataSize, Hash128* hash)
		{
			ulong u64_ = hash->u64_0;
			ulong u64_2 = hash->u64_1;
			HashUnsafeUtilities.ComputeHash128(data, dataSize, &u64_, &u64_2);
			*hash = new Hash128(u64_, u64_2);
		}
	}
}
