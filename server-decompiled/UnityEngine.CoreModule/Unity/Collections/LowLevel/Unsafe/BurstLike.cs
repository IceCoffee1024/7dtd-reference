using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.LowLevel;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x020000A4 RID: 164
	[NativeHeader("Runtime/Export/BurstLike/BurstLike.bindings.h")]
	[StaticAccessor("BurstLike", StaticAccessorType.DoubleColon)]
	internal static class BurstLike
	{
		// Token: 0x06000340 RID: 832
		[BurstAuthorizedExternalMethod]
		[ThreadSafe(ThrowsException = false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int NativeFunctionCall_Int_IntPtr_IntPtr(IntPtr function, IntPtr p0, IntPtr p1, out int error);

		// Token: 0x020000A5 RID: 165
		internal readonly struct SharedStatic<[IsUnmanaged] T> where T : struct, ValueType
		{
			// Token: 0x06000341 RID: 833 RVA: 0x000063D1 File Offset: 0x000045D1
			private unsafe SharedStatic(void* buffer)
			{
				this._buffer = buffer;
			}

			// Token: 0x1700008E RID: 142
			// (get) Token: 0x06000342 RID: 834 RVA: 0x000063DA File Offset: 0x000045DA
			public ref T Data
			{
				get
				{
					return UnsafeUtility.AsRef<T>(this._buffer);
				}
			}

			// Token: 0x1700008F RID: 143
			// (get) Token: 0x06000343 RID: 835 RVA: 0x000063E7 File Offset: 0x000045E7
			public unsafe void* UnsafeDataPointer
			{
				get
				{
					return this._buffer;
				}
			}

			// Token: 0x06000344 RID: 836 RVA: 0x000063EF File Offset: 0x000045EF
			public static BurstLike.SharedStatic<T> GetOrCreate<TContext>(uint alignment = 0U)
			{
				return new BurstLike.SharedStatic<T>(BurstLike.SharedStatic.GetOrCreateSharedStaticInternal(BurstRuntime.GetHashCode64<TContext>(), 0L, (uint)UnsafeUtility.SizeOf<T>(), alignment));
			}

			// Token: 0x06000345 RID: 837 RVA: 0x00006408 File Offset: 0x00004608
			public static BurstLike.SharedStatic<T> GetOrCreate<TContext, TSubContext>(uint alignment = 0U)
			{
				return new BurstLike.SharedStatic<T>(BurstLike.SharedStatic.GetOrCreateSharedStaticInternal(BurstRuntime.GetHashCode64<TContext>(), BurstRuntime.GetHashCode64<TSubContext>(), (uint)UnsafeUtility.SizeOf<T>(), alignment));
			}

			// Token: 0x04000242 RID: 578
			private unsafe readonly void* _buffer;
		}

		// Token: 0x020000A6 RID: 166
		internal static class SharedStatic
		{
			// Token: 0x06000346 RID: 838 RVA: 0x00006424 File Offset: 0x00004624
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			private static void CheckSizeOf(uint sizeOf)
			{
				bool flag = sizeOf == 0U;
				if (flag)
				{
					throw new ArgumentException("sizeOf must be > 0", "sizeOf");
				}
			}

			// Token: 0x06000347 RID: 839 RVA: 0x0000644C File Offset: 0x0000464C
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			private unsafe static void CheckResult(void* result)
			{
				bool flag = result == null;
				if (flag)
				{
					throw new InvalidOperationException("Unable to create a SharedStatic for this key. This is most likely due to the size of the struct inside of the SharedStatic having changed or the same key being reused for differently sized values. To fix this the editor needs to be restarted.");
				}
			}

			// Token: 0x06000348 RID: 840 RVA: 0x00006470 File Offset: 0x00004670
			[RequiredMember]
			public unsafe static void* GetOrCreateSharedStaticInternal(long getHashCode64, long getSubHashCode64, uint sizeOf, uint alignment)
			{
				Hash128 hash = new Hash128((ulong)getHashCode64, (ulong)getSubHashCode64);
				return BurstCompilerService.GetOrCreateSharedMemory(ref hash, sizeOf, (alignment == 0U) ? 4U : alignment);
			}
		}
	}
}
