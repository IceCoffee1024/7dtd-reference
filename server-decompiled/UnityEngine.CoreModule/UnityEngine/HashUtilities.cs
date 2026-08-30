using System;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine
{
	// Token: 0x020001DB RID: 475
	public static class HashUtilities
	{
		// Token: 0x0600149B RID: 5275 RVA: 0x0001D6A4 File Offset: 0x0001B8A4
		public unsafe static void AppendHash(ref Hash128 inHash, ref Hash128 outHash)
		{
			fixed (Hash128* ptr = &outHash)
			{
				Hash128* ptr2 = ptr;
				fixed (Hash128* ptr3 = &inHash)
				{
					Hash128* ptr4 = ptr3;
					HashUnsafeUtilities.ComputeHash128((void*)ptr4, (ulong)((long)sizeof(Hash128)), ptr2);
				}
			}
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x0001D6D8 File Offset: 0x0001B8D8
		public unsafe static void QuantisedMatrixHash(ref Matrix4x4 value, ref Hash128 hash)
		{
			fixed (Hash128* ptr = &hash)
			{
				Hash128* ptr2 = ptr;
				int* ptr3 = stackalloc int[(UIntPtr)64];
				for (int i = 0; i < 16; i++)
				{
					ptr3[i] = (int)(value[i] * 1000f + 0.5f);
				}
				HashUnsafeUtilities.ComputeHash128((void*)ptr3, 64UL, ptr2);
			}
		}

		// Token: 0x0600149D RID: 5277 RVA: 0x0001D734 File Offset: 0x0001B934
		public unsafe static void QuantisedVectorHash(ref Vector3 value, ref Hash128 hash)
		{
			fixed (Hash128* ptr = &hash)
			{
				Hash128* ptr2 = ptr;
				int* ptr3 = stackalloc int[(UIntPtr)12];
				for (int i = 0; i < 3; i++)
				{
					ptr3[i] = (int)(value[i] * 1000f + 0.5f);
				}
				HashUnsafeUtilities.ComputeHash128((void*)ptr3, 12UL, ptr2);
			}
		}

		// Token: 0x0600149E RID: 5278 RVA: 0x0001D78C File Offset: 0x0001B98C
		public unsafe static void ComputeHash128<T>(ref T value, ref Hash128 hash) where T : struct
		{
			void* ptr = UnsafeUtility.AddressOf<T>(ref value);
			ulong num = (ulong)((long)UnsafeUtility.SizeOf<T>());
			Hash128* ptr2 = (Hash128*)UnsafeUtility.AddressOf<Hash128>(ref hash);
			HashUnsafeUtilities.ComputeHash128(ptr, num, ptr2);
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x0001D7B8 File Offset: 0x0001B9B8
		public unsafe static void ComputeHash128(byte[] value, ref Hash128 hash)
		{
			fixed (byte* ptr = &value[0])
			{
				byte* ptr2 = ptr;
				ulong num = (ulong)((long)value.Length);
				Hash128* ptr3 = (Hash128*)UnsafeUtility.AddressOf<Hash128>(ref hash);
				HashUnsafeUtilities.ComputeHash128((void*)ptr2, num, ptr3);
			}
		}
	}
}
