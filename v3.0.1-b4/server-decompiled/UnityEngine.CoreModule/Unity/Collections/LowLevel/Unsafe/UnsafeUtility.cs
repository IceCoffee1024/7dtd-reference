using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.Burst;
using UnityEngine.Bindings;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x020000B7 RID: 183
	[StaticAccessor("UnsafeUtility", StaticAccessorType.DoubleColon)]
	[NativeHeader("Runtime/Export/Unsafe/UnsafeUtility.bindings.h")]
	public static class UnsafeUtility
	{
		// Token: 0x06000360 RID: 864
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetFieldOffsetInStruct(FieldInfo field);

		// Token: 0x06000361 RID: 865
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetFieldOffsetInClass(FieldInfo field);

		// Token: 0x06000362 RID: 866 RVA: 0x0000667C File Offset: 0x0000487C
		public static int GetFieldOffset(FieldInfo field)
		{
			bool isValueType = field.DeclaringType.IsValueType;
			int num;
			if (isValueType)
			{
				num = UnsafeUtility.GetFieldOffsetInStruct(field);
			}
			else
			{
				bool isClass = field.DeclaringType.IsClass;
				if (isClass)
				{
					num = UnsafeUtility.GetFieldOffsetInClass(field);
				}
				else
				{
					num = -1;
				}
			}
			return num;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x000066C0 File Offset: 0x000048C0
		public unsafe static void* PinGCObjectAndGetAddress(object target, out ulong gcHandle)
		{
			return UnsafeUtility.PinSystemObjectAndGetAddress(target, out gcHandle);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x000066DC File Offset: 0x000048DC
		public unsafe static void* PinGCArrayAndGetDataAddress(Array target, out ulong gcHandle)
		{
			return UnsafeUtility.PinSystemArrayAndGetAddress(target, out gcHandle);
		}

		// Token: 0x06000365 RID: 869
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void* PinSystemArrayAndGetAddress(object target, out ulong gcHandle);

		// Token: 0x06000366 RID: 870
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void* PinSystemObjectAndGetAddress(object target, out ulong gcHandle);

		// Token: 0x06000367 RID: 871
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ReleaseGCObject(ulong gcHandle);

		// Token: 0x06000368 RID: 872
		[ThreadSafe(ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern void CopyObjectAddressToPtr(object target, void* dstPtr);

		// Token: 0x06000369 RID: 873 RVA: 0x000066F8 File Offset: 0x000048F8
		public static bool IsBlittable<T>() where T : struct
		{
			return UnsafeUtility.IsBlittable(typeof(T));
		}

		// Token: 0x0600036A RID: 874
		[ThreadSafe(ThrowsException = false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int CheckForLeaks();

		// Token: 0x0600036B RID: 875
		[ThreadSafe(ThrowsException = false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int ForgiveLeaks();

		// Token: 0x0600036C RID: 876
		[ThreadSafe(ThrowsException = false)]
		[BurstAuthorizedExternalMethod]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern NativeLeakDetectionMode GetLeakDetectionMode();

		// Token: 0x0600036D RID: 877
		[BurstAuthorizedExternalMethod]
		[ThreadSafe(ThrowsException = false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetLeakDetectionMode(NativeLeakDetectionMode value);

		// Token: 0x0600036E RID: 878
		[ThreadSafe(ThrowsException = false)]
		[BurstAuthorizedExternalMethod]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int LeakRecord(IntPtr handle, LeakCategory category, int callstacksToSkip);

		// Token: 0x0600036F RID: 879
		[BurstAuthorizedExternalMethod]
		[ThreadSafe(ThrowsException = false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int LeakErase(IntPtr handle, LeakCategory category);

		// Token: 0x06000370 RID: 880
		[ThreadSafe(ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern void* MallocTracked(long size, int alignment, Allocator allocator, int callstacksToSkip);

		// Token: 0x06000371 RID: 881
		[ThreadSafe(ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern void FreeTracked(void* memory, Allocator allocator);

		// Token: 0x06000372 RID: 882
		[ThreadSafe(ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern void* Malloc(long size, int alignment, Allocator allocator);

		// Token: 0x06000373 RID: 883
		[ThreadSafe(ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern void Free(void* memory, Allocator allocator);

		// Token: 0x06000374 RID: 884 RVA: 0x0000671C File Offset: 0x0000491C
		public static bool IsValidAllocator(Allocator allocator)
		{
			return allocator > Allocator.None;
		}

		// Token: 0x06000375 RID: 885
		[ThreadSafe(ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern void MemCpy(void* destination, void* source, long size);

		// Token: 0x06000376 RID: 886
		[ThreadSafe(ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern void MemCpyReplicate(void* destination, void* source, int size, int count);

		// Token: 0x06000377 RID: 887
		[ThreadSafe(ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern void MemCpyStride(void* destination, int destinationStride, void* source, int sourceStride, int elementSize, int count);

		// Token: 0x06000378 RID: 888
		[ThreadSafe(ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern void MemMove(void* destination, void* source, long size);

		// Token: 0x06000379 RID: 889
		[ThreadSafe(ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern void MemSet(void* destination, byte value, long size);

		// Token: 0x0600037A RID: 890 RVA: 0x00006732 File Offset: 0x00004932
		public unsafe static void MemClear(void* destination, long size)
		{
			UnsafeUtility.MemSet(destination, 0, size);
		}

		// Token: 0x0600037B RID: 891
		[ThreadSafe(ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern int MemCmp(void* ptr1, void* ptr2, long size);

		// Token: 0x0600037C RID: 892
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int SizeOf(Type type);

		// Token: 0x0600037D RID: 893
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsBlittable(Type type);

		// Token: 0x0600037E RID: 894
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsUnmanaged(Type type);

		// Token: 0x0600037F RID: 895
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsValidNativeContainerElementType(Type type);

		// Token: 0x06000380 RID: 896
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetScriptingTypeFlags(Type type);

		// Token: 0x06000381 RID: 897
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void LogError(string msg, string filename, int linenumber);

		// Token: 0x06000382 RID: 898 RVA: 0x00006740 File Offset: 0x00004940
		private static bool IsBlittableValueType(Type t)
		{
			return t.IsValueType && UnsafeUtility.IsBlittable(t);
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00006764 File Offset: 0x00004964
		private static string GetReasonForTypeNonBlittableImpl(Type t, string name)
		{
			bool flag = !t.IsValueType;
			string text;
			if (flag)
			{
				text = string.Format("{0} is not blittable because it is not of value type ({1})\n", name, t);
			}
			else
			{
				bool isPrimitive = t.IsPrimitive;
				if (isPrimitive)
				{
					text = string.Format("{0} is not blittable ({1})\n", name, t);
				}
				else
				{
					string text2 = "";
					foreach (FieldInfo fieldInfo in t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
					{
						bool flag2 = !UnsafeUtility.IsBlittableValueType(fieldInfo.FieldType);
						if (flag2)
						{
							text2 += UnsafeUtility.GetReasonForTypeNonBlittableImpl(fieldInfo.FieldType, string.Format("{0}.{1}", name, fieldInfo.Name));
						}
					}
					text = text2;
				}
			}
			return text;
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00006818 File Offset: 0x00004A18
		internal static bool IsArrayBlittable(Array arr)
		{
			return UnsafeUtility.IsBlittableValueType(arr.GetType().GetElementType());
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000683C File Offset: 0x00004A3C
		internal static bool IsGenericListBlittable<T>() where T : struct
		{
			return UnsafeUtility.IsBlittable<T>();
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00006854 File Offset: 0x00004A54
		internal static string GetReasonForArrayNonBlittable(Array arr)
		{
			Type elementType = arr.GetType().GetElementType();
			return UnsafeUtility.GetReasonForTypeNonBlittableImpl(elementType, elementType.Name);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00006880 File Offset: 0x00004A80
		internal static string GetReasonForGenericListNonBlittable<T>() where T : struct
		{
			Type typeFromHandle = typeof(T);
			return UnsafeUtility.GetReasonForTypeNonBlittableImpl(typeFromHandle, typeFromHandle.Name);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x000068AC File Offset: 0x00004AAC
		internal static string GetReasonForTypeNonBlittable(Type t)
		{
			return UnsafeUtility.GetReasonForTypeNonBlittableImpl(t, t.Name);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x000068CC File Offset: 0x00004ACC
		internal static string GetReasonForValueTypeNonBlittable<T>() where T : struct
		{
			Type typeFromHandle = typeof(T);
			return UnsafeUtility.GetReasonForTypeNonBlittableImpl(typeFromHandle, typeFromHandle.Name);
		}

		// Token: 0x0600038A RID: 906 RVA: 0x000068F8 File Offset: 0x00004AF8
		public static bool IsUnmanaged<T>()
		{
			return (UnsafeUtility.TypeFlagsCache<T>.flags & 1) == 0;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00006914 File Offset: 0x00004B14
		public static bool IsNativeContainerType<T>()
		{
			return (UnsafeUtility.TypeFlagsCache<T>.flags & 2) != 0;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00006930 File Offset: 0x00004B30
		public static bool IsValidNativeContainerElementType<T>()
		{
			return UnsafeUtility.TypeFlagsCache<T>.flags == 0;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000694C File Offset: 0x00004B4C
		public static int AlignOf<T>() where T : struct
		{
			return UnsafeUtility.SizeOf<UnsafeUtility.AlignOfHelper<T>>() - UnsafeUtility.SizeOf<T>();
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00006969 File Offset: 0x00004B69
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void CopyPtrToStructure<T>(void* ptr, out T output) where T : struct
		{
			UnsafeUtility.InternalCopyPtrToStructure<T>(ptr, out output);
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00006974 File Offset: 0x00004B74
		private unsafe static void InternalCopyPtrToStructure<T>(void* ptr, out T output) where T : struct
		{
			output = *(T*)ptr;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00006982 File Offset: 0x00004B82
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void CopyStructureToPtr<T>(ref T input, void* ptr) where T : struct
		{
			UnsafeUtility.InternalCopyStructureToPtr<T>(ref input, ptr);
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00006974 File Offset: 0x00004B74
		private unsafe static void InternalCopyStructureToPtr<T>(ref T input, void* ptr) where T : struct
		{
			*(T*)ptr = input;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000698D File Offset: 0x00004B8D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static T ReadArrayElement<T>(void* source, int index)
		{
			return *(T*)((byte*)source + (long)index * (long)sizeof(T));
		}

		// Token: 0x06000393 RID: 915 RVA: 0x000069A1 File Offset: 0x00004BA1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static T ReadArrayElementWithStride<T>(void* source, int index, int stride)
		{
			return *(T*)((byte*)source + (long)index * (long)stride);
		}

		// Token: 0x06000394 RID: 916 RVA: 0x000069B0 File Offset: 0x00004BB0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteArrayElement<T>(void* destination, int index, T value)
		{
			*(T*)((byte*)destination + (long)index * (long)sizeof(T)) = value;
		}

		// Token: 0x06000395 RID: 917 RVA: 0x000069C5 File Offset: 0x00004BC5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteArrayElementWithStride<T>(void* destination, int index, int stride, T value)
		{
			*(T*)((byte*)destination + (long)index * (long)stride) = value;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x000069D5 File Offset: 0x00004BD5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void* AddressOf<T>(ref T output) where T : struct
		{
			return (void*)(&output);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x000069D8 File Offset: 0x00004BD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int SizeOf<T>() where T : struct
		{
			return sizeof(T);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x000069D5 File Offset: 0x00004BD5
		public static ref T As<U, T>(ref U from)
		{
			return ref from;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x000069D5 File Offset: 0x00004BD5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static ref T AsRef<T>(void* ptr) where T : struct
		{
			return ref *(T*)ptr;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x000069E0 File Offset: 0x00004BE0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static ref T ArrayElementAsRef<T>(void* ptr, int index) where T : struct
		{
			return ref *(T*)((byte*)ptr + (long)index * (long)sizeof(T));
		}

		// Token: 0x0600039B RID: 923 RVA: 0x000069F0 File Offset: 0x00004BF0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EnumToInt<T>(T enumValue) where T : struct, IConvertible
		{
			int num = 0;
			UnsafeUtility.InternalEnumToInt<T>(ref enumValue, ref num);
			return num;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00006A0F File Offset: 0x00004C0F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void InternalEnumToInt<T>(ref T enumValue, ref int intValue)
		{
			intValue = enumValue;
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00006A15 File Offset: 0x00004C15
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool EnumEquals<T>(T lhs, T rhs) where T : struct, IConvertible
		{
			return lhs == rhs;
		}

		// Token: 0x04000244 RID: 580
		private const int kIsManaged = 1;

		// Token: 0x04000245 RID: 581
		private const int kIsNativeContainer = 2;

		// Token: 0x020000B8 RID: 184
		internal struct TypeFlagsCache<T>
		{
			// Token: 0x0600039E RID: 926 RVA: 0x00006A1D File Offset: 0x00004C1D
			static TypeFlagsCache()
			{
				UnsafeUtility.TypeFlagsCache<T>.Init(ref UnsafeUtility.TypeFlagsCache<T>.flags);
			}

			// Token: 0x0600039F RID: 927 RVA: 0x00006A2B File Offset: 0x00004C2B
			[BurstDiscard]
			private static void Init(ref int flags)
			{
				flags = UnsafeUtility.GetScriptingTypeFlags(typeof(T));
			}

			// Token: 0x04000246 RID: 582
			internal static readonly int flags;
		}

		// Token: 0x020000B9 RID: 185
		private struct AlignOfHelper<T> where T : struct
		{
			// Token: 0x04000247 RID: 583
			public byte dummy;

			// Token: 0x04000248 RID: 584
			public T data;
		}
	}
}
