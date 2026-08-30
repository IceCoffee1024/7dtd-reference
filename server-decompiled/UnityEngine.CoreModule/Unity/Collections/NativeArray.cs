using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine.Internal;

namespace Unity.Collections
{
	// Token: 0x02000098 RID: 152
	[DebuggerTypeProxy(typeof(NativeArrayDebugView<>))]
	[DebuggerDisplay("Length = {m_Length}")]
	[NativeContainerSupportsDeallocateOnJobCompletion]
	[NativeContainerSupportsMinMaxWriteRestriction]
	[NativeContainerSupportsDeferredConvertListToArray]
	[NativeContainer]
	public struct NativeArray<T> : IDisposable, IEnumerable<T>, IEnumerable, IEquatable<NativeArray<T>> where T : struct
	{
		// Token: 0x060002B3 RID: 691 RVA: 0x00004F58 File Offset: 0x00003158
		public NativeArray(int length, Allocator allocator, NativeArrayOptions options = NativeArrayOptions.ClearMemory)
		{
			NativeArray<T>.Allocate(length, allocator, out this);
			bool flag = (options & NativeArrayOptions.ClearMemory) == NativeArrayOptions.ClearMemory;
			if (flag)
			{
				UnsafeUtility.MemClear(this.m_Buffer, (long)this.Length * (long)UnsafeUtility.SizeOf<T>());
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00004F93 File Offset: 0x00003193
		public NativeArray(T[] array, Allocator allocator)
		{
			NativeArray<T>.Allocate(array.Length, allocator, out this);
			NativeArray<T>.Copy(array, this);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00004FAE File Offset: 0x000031AE
		public NativeArray(NativeArray<T> array, Allocator allocator)
		{
			NativeArray<T>.Allocate(array.Length, allocator, out this);
			NativeArray<T>.Copy(array, 0, this, 0, array.Length);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00004FD8 File Offset: 0x000031D8
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private static void CheckAllocateArguments(int length, Allocator allocator)
		{
			bool flag = allocator <= Allocator.None;
			if (flag)
			{
				throw new ArgumentException("Allocator must be Temp, TempJob or Persistent", "allocator");
			}
			bool flag2 = allocator >= Allocator.FirstUserIndex;
			if (flag2)
			{
				throw new ArgumentException("Use CollectionHelper.CreateNativeArray for custom allocator", "allocator");
			}
			bool flag3 = length < 0;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException("length", "Length must be >= 0");
			}
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00005038 File Offset: 0x00003238
		private static void Allocate(int length, Allocator allocator, out NativeArray<T> array)
		{
			long num = (long)UnsafeUtility.SizeOf<T>() * (long)length;
			array = default(NativeArray<T>);
			array.m_Buffer = UnsafeUtility.MallocTracked(num, UnsafeUtility.AlignOf<T>(), allocator, 0);
			array.m_Length = length;
			array.m_AllocatorLabel = allocator;
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x00005078 File Offset: 0x00003278
		public int Length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Length;
			}
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00005090 File Offset: 0x00003290
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[BurstDiscard]
		internal static void IsUnmanagedAndThrow()
		{
			bool flag = !UnsafeUtility.IsUnmanaged<T>();
			if (flag)
			{
				throw new InvalidOperationException(string.Format("{0} used in NativeArray<{1}> must be unmanaged (contain no managed types).", typeof(T), typeof(T)));
			}
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00002669 File Offset: 0x00000869
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CheckElementReadAccess(int index)
		{
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00002669 File Offset: 0x00000869
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CheckElementWriteAccess(int index)
		{
		}

		// Token: 0x1700007D RID: 125
		public T this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return UnsafeUtility.ReadArrayElement<T>(this.m_Buffer, index);
			}
			[WriteAccessRequired]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				UnsafeUtility.WriteArrayElement<T>(this.m_Buffer, index, value);
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060002BE RID: 702 RVA: 0x000050FF File Offset: 0x000032FF
		public bool IsCreated
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Buffer != null;
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00005110 File Offset: 0x00003310
		[WriteAccessRequired]
		public void Dispose()
		{
			bool flag = !this.IsCreated;
			if (!flag)
			{
				bool flag2 = this.m_AllocatorLabel == Allocator.Invalid;
				if (flag2)
				{
					throw new InvalidOperationException("The NativeArray can not be Disposed because it was not allocated with a valid allocator.");
				}
				bool flag3 = this.m_AllocatorLabel > Allocator.None;
				if (flag3)
				{
					UnsafeUtility.FreeTracked(this.m_Buffer, this.m_AllocatorLabel);
					this.m_AllocatorLabel = Allocator.Invalid;
				}
				this.m_Buffer = null;
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00005178 File Offset: 0x00003378
		public JobHandle Dispose(JobHandle inputDeps)
		{
			bool flag = !this.IsCreated;
			JobHandle jobHandle;
			if (flag)
			{
				jobHandle = inputDeps;
			}
			else
			{
				bool flag2 = this.m_AllocatorLabel >= Allocator.FirstUserIndex;
				if (flag2)
				{
					throw new InvalidOperationException("The NativeArray can not be Disposed because it was allocated with a custom allocator, use CollectionHelper.Dispose in com.unity.collections package.");
				}
				bool flag3 = this.m_AllocatorLabel > Allocator.None;
				if (flag3)
				{
					JobHandle jobHandle2 = new NativeArrayDisposeJob
					{
						Data = new NativeArrayDispose
						{
							m_Buffer = this.m_Buffer,
							m_AllocatorLabel = this.m_AllocatorLabel
						}
					}.Schedule(inputDeps);
					this.m_Buffer = null;
					this.m_AllocatorLabel = Allocator.Invalid;
					jobHandle = jobHandle2;
				}
				else
				{
					this.m_Buffer = null;
					jobHandle = inputDeps;
				}
			}
			return jobHandle;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00005224 File Offset: 0x00003424
		[WriteAccessRequired]
		public void CopyFrom(T[] array)
		{
			NativeArray<T>.Copy(array, this);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00005234 File Offset: 0x00003434
		[WriteAccessRequired]
		public void CopyFrom(NativeArray<T> array)
		{
			NativeArray<T>.Copy(array, this);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00005244 File Offset: 0x00003444
		public void CopyTo(T[] array)
		{
			NativeArray<T>.Copy(this, array);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x00005254 File Offset: 0x00003454
		public void CopyTo(NativeArray<T> array)
		{
			NativeArray<T>.Copy(this, array);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00005264 File Offset: 0x00003464
		public T[] ToArray()
		{
			T[] array = new T[this.Length];
			NativeArray<T>.Copy(this, array, this.Length);
			return array;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00005298 File Offset: 0x00003498
		public NativeArray<T>.Enumerator GetEnumerator()
		{
			return new NativeArray<T>.Enumerator(ref this);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x000052B0 File Offset: 0x000034B0
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new NativeArray<T>.Enumerator(ref this);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x000052D0 File Offset: 0x000034D0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x000052F0 File Offset: 0x000034F0
		public bool Equals(NativeArray<T> other)
		{
			return this.m_Buffer == other.m_Buffer && this.m_Length == other.m_Length;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00005324 File Offset: 0x00003524
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is NativeArray<T> && this.Equals((NativeArray<T>)obj);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000535C File Offset: 0x0000355C
		public override int GetHashCode()
		{
			return (this.m_Buffer * 397) ^ this.m_Length;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00005384 File Offset: 0x00003584
		public static bool operator ==(NativeArray<T> left, NativeArray<T> right)
		{
			return left.Equals(right);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x000053A0 File Offset: 0x000035A0
		public static bool operator !=(NativeArray<T> left, NativeArray<T> right)
		{
			return !left.Equals(right);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x000053BD File Offset: 0x000035BD
		public static void Copy(NativeArray<T> src, NativeArray<T> dst)
		{
			NativeArray<T>.CopySafe(src, 0, dst, 0, src.Length);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x000053D1 File Offset: 0x000035D1
		public static void Copy(NativeArray<T>.ReadOnly src, NativeArray<T> dst)
		{
			NativeArray<T>.CopySafe(src, 0, dst, 0, src.Length);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x000053E5 File Offset: 0x000035E5
		public static void Copy(T[] src, NativeArray<T> dst)
		{
			NativeArray<T>.CopySafe(src, 0, dst, 0, src.Length);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x000053F5 File Offset: 0x000035F5
		public static void Copy(NativeArray<T> src, T[] dst)
		{
			NativeArray<T>.CopySafe(src, 0, dst, 0, src.Length);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00005409 File Offset: 0x00003609
		public static void Copy(NativeArray<T>.ReadOnly src, T[] dst)
		{
			NativeArray<T>.CopySafe(src, 0, dst, 0, src.Length);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000541D File Offset: 0x0000361D
		public static void Copy(NativeArray<T> src, NativeArray<T> dst, int length)
		{
			NativeArray<T>.CopySafe(src, 0, dst, 0, length);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000542B File Offset: 0x0000362B
		public static void Copy(NativeArray<T>.ReadOnly src, NativeArray<T> dst, int length)
		{
			NativeArray<T>.CopySafe(src, 0, dst, 0, length);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00005439 File Offset: 0x00003639
		public static void Copy(T[] src, NativeArray<T> dst, int length)
		{
			NativeArray<T>.CopySafe(src, 0, dst, 0, length);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00005447 File Offset: 0x00003647
		public static void Copy(NativeArray<T> src, T[] dst, int length)
		{
			NativeArray<T>.CopySafe(src, 0, dst, 0, length);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00005455 File Offset: 0x00003655
		public static void Copy(NativeArray<T>.ReadOnly src, T[] dst, int length)
		{
			NativeArray<T>.CopySafe(src, 0, dst, 0, length);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00005463 File Offset: 0x00003663
		public static void Copy(NativeArray<T> src, int srcIndex, NativeArray<T> dst, int dstIndex, int length)
		{
			NativeArray<T>.CopySafe(src, srcIndex, dst, dstIndex, length);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00005472 File Offset: 0x00003672
		public static void Copy(NativeArray<T>.ReadOnly src, int srcIndex, NativeArray<T> dst, int dstIndex, int length)
		{
			NativeArray<T>.CopySafe(src, srcIndex, dst, dstIndex, length);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00005481 File Offset: 0x00003681
		public static void Copy(T[] src, int srcIndex, NativeArray<T> dst, int dstIndex, int length)
		{
			NativeArray<T>.CopySafe(src, srcIndex, dst, dstIndex, length);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00005490 File Offset: 0x00003690
		public static void Copy(NativeArray<T> src, int srcIndex, T[] dst, int dstIndex, int length)
		{
			NativeArray<T>.CopySafe(src, srcIndex, dst, dstIndex, length);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000549F File Offset: 0x0000369F
		public static void Copy(NativeArray<T>.ReadOnly src, int srcIndex, T[] dst, int dstIndex, int length)
		{
			NativeArray<T>.CopySafe(src, srcIndex, dst, dstIndex, length);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x000054AE File Offset: 0x000036AE
		private unsafe static void CopySafe(NativeArray<T> src, int srcIndex, NativeArray<T> dst, int dstIndex, int length)
		{
			UnsafeUtility.MemCpy((void*)((byte*)dst.m_Buffer + dstIndex * UnsafeUtility.SizeOf<T>()), (void*)((byte*)src.m_Buffer + srcIndex * UnsafeUtility.SizeOf<T>()), (long)(length * UnsafeUtility.SizeOf<T>()));
		}

		// Token: 0x060002DE RID: 734 RVA: 0x000054DC File Offset: 0x000036DC
		private unsafe static void CopySafe(NativeArray<T>.ReadOnly src, int srcIndex, NativeArray<T> dst, int dstIndex, int length)
		{
			UnsafeUtility.MemCpy((void*)((byte*)dst.m_Buffer + dstIndex * UnsafeUtility.SizeOf<T>()), (void*)((byte*)src.m_Buffer + srcIndex * UnsafeUtility.SizeOf<T>()), (long)(length * UnsafeUtility.SizeOf<T>()));
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000550C File Offset: 0x0000370C
		private unsafe static void CopySafe(T[] src, int srcIndex, NativeArray<T> dst, int dstIndex, int length)
		{
			GCHandle gchandle = GCHandle.Alloc(src, GCHandleType.Pinned);
			IntPtr intPtr = gchandle.AddrOfPinnedObject();
			UnsafeUtility.MemCpy((void*)((byte*)dst.m_Buffer + dstIndex * UnsafeUtility.SizeOf<T>()), (void*)((byte*)(void*)intPtr + srcIndex * UnsafeUtility.SizeOf<T>()), (long)(length * UnsafeUtility.SizeOf<T>()));
			gchandle.Free();
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00005560 File Offset: 0x00003760
		private unsafe static void CopySafe(NativeArray<T> src, int srcIndex, T[] dst, int dstIndex, int length)
		{
			GCHandle gchandle = GCHandle.Alloc(dst, GCHandleType.Pinned);
			IntPtr intPtr = gchandle.AddrOfPinnedObject();
			UnsafeUtility.MemCpy((void*)((byte*)(void*)intPtr + dstIndex * UnsafeUtility.SizeOf<T>()), (void*)((byte*)src.m_Buffer + srcIndex * UnsafeUtility.SizeOf<T>()), (long)(length * UnsafeUtility.SizeOf<T>()));
			gchandle.Free();
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x000055B4 File Offset: 0x000037B4
		private unsafe static void CopySafe(NativeArray<T>.ReadOnly src, int srcIndex, T[] dst, int dstIndex, int length)
		{
			GCHandle gchandle = GCHandle.Alloc(dst, GCHandleType.Pinned);
			IntPtr intPtr = gchandle.AddrOfPinnedObject();
			UnsafeUtility.MemCpy((void*)((byte*)(void*)intPtr + dstIndex * UnsafeUtility.SizeOf<T>()), (void*)((byte*)src.m_Buffer + srcIndex * UnsafeUtility.SizeOf<T>()), (long)(length * UnsafeUtility.SizeOf<T>()));
			gchandle.Free();
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00005608 File Offset: 0x00003808
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private static void CheckCopyPtr(T[] ptr)
		{
			bool flag = ptr == null;
			if (flag)
			{
				throw new ArgumentNullException("ptr");
			}
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000562C File Offset: 0x0000382C
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private static void CheckCopyLengths(int srcLength, int dstLength)
		{
			bool flag = srcLength != dstLength;
			if (flag)
			{
				throw new ArgumentException("source and destination length must be the same");
			}
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00005650 File Offset: 0x00003850
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private static void CheckCopyArguments(int srcLength, int srcIndex, int dstLength, int dstIndex, int length)
		{
			bool flag = length < 0;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("length", "length must be equal or greater than zero.");
			}
			bool flag2 = srcIndex < 0 || srcIndex > srcLength || (srcIndex == srcLength && srcLength > 0);
			if (flag2)
			{
				throw new ArgumentOutOfRangeException("srcIndex", "srcIndex is outside the range of valid indexes for the source NativeArray.");
			}
			bool flag3 = dstIndex < 0 || dstIndex > dstLength || (dstIndex == dstLength && dstLength > 0);
			if (flag3)
			{
				throw new ArgumentOutOfRangeException("dstIndex", "dstIndex is outside the range of valid indexes for the destination NativeArray.");
			}
			bool flag4 = srcIndex + length > srcLength;
			if (flag4)
			{
				throw new ArgumentException("length is greater than the number of elements from srcIndex to the end of the source NativeArray.", "length");
			}
			bool flag5 = srcIndex + length < 0;
			if (flag5)
			{
				throw new ArgumentException("srcIndex + length causes an integer overflow");
			}
			bool flag6 = dstIndex + length > dstLength;
			if (flag6)
			{
				throw new ArgumentException("length is greater than the number of elements from dstIndex to the end of the destination NativeArray.", "length");
			}
			bool flag7 = dstIndex + length < 0;
			if (flag7)
			{
				throw new ArgumentException("dstIndex + length causes an integer overflow");
			}
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00002669 File Offset: 0x00000869
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private void CheckReinterpretLoadRange<U>(int sourceIndex) where U : struct
		{
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00002669 File Offset: 0x00000869
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private void CheckReinterpretStoreRange<U>(int destIndex) where U : struct
		{
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x00005734 File Offset: 0x00003934
		public unsafe U ReinterpretLoad<U>(int sourceIndex) where U : struct
		{
			byte* ptr = (byte*)this.m_Buffer + (long)UnsafeUtility.SizeOf<T>() * (long)sourceIndex;
			return UnsafeUtility.ReadArrayElement<U>((void*)ptr, 0);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00005760 File Offset: 0x00003960
		public unsafe void ReinterpretStore<U>(int destIndex, U data) where U : struct
		{
			byte* ptr = (byte*)this.m_Buffer + (long)UnsafeUtility.SizeOf<T>() * (long)destIndex;
			UnsafeUtility.WriteArrayElement<U>((void*)ptr, 0, data);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000578C File Offset: 0x0000398C
		private NativeArray<U> InternalReinterpret<U>(int length) where U : struct
		{
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<U>(this.m_Buffer, length, this.m_AllocatorLabel);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x000057B4 File Offset: 0x000039B4
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private static void CheckReinterpretSize<U>() where U : struct
		{
			bool flag = UnsafeUtility.SizeOf<T>() != UnsafeUtility.SizeOf<U>();
			if (flag)
			{
				throw new InvalidOperationException(string.Format("Types {0} and {1} are different sizes - direct reinterpretation is not possible. If this is what you intended, use Reinterpret(<type size>)", typeof(T), typeof(U)));
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x000057FC File Offset: 0x000039FC
		public NativeArray<U> Reinterpret<U>() where U : struct
		{
			return this.InternalReinterpret<U>(this.Length);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000581C File Offset: 0x00003A1C
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private void CheckReinterpretSize<U>(long tSize, long uSize, int expectedTypeSize, long byteLen, long uLen)
		{
			bool flag = tSize != (long)expectedTypeSize;
			if (flag)
			{
				throw new InvalidOperationException(string.Format("Type {0} was expected to be {1} but is {2} bytes", typeof(T), expectedTypeSize, tSize));
			}
			bool flag2 = uLen * uSize != byteLen;
			if (flag2)
			{
				throw new InvalidOperationException(string.Format("Types {0} (array length {1}) and {2} cannot be aliased due to size constraints. The size of the types and lengths involved must line up.", typeof(T), this.Length, typeof(U)));
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000589C File Offset: 0x00003A9C
		public NativeArray<U> Reinterpret<U>(int expectedTypeSize) where U : struct
		{
			long num = (long)UnsafeUtility.SizeOf<T>();
			long num2 = (long)UnsafeUtility.SizeOf<U>();
			long num3 = (long)this.Length * num;
			long num4 = num3 / num2;
			return this.InternalReinterpret<U>((int)num4);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x000058D4 File Offset: 0x00003AD4
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private void CheckGetSubArrayArguments(int start, int length)
		{
			bool flag = start < 0;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("start", "start must be >= 0");
			}
			bool flag2 = start + length > this.Length;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException("length", string.Format("sub array range {0}-{1} is outside the range of the native array 0-{2}", start, start + length - 1, this.Length - 1));
			}
			bool flag3 = start + length < 0;
			if (flag3)
			{
				throw new ArgumentException(string.Format("sub array range {0}-{1} caused an integer overflow and is outside the range of the native array 0-{2}", start, start + length - 1, this.Length - 1));
			}
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00005978 File Offset: 0x00003B78
		public unsafe NativeArray<T> GetSubArray(int start, int length)
		{
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((void*)((byte*)this.m_Buffer + (long)UnsafeUtility.SizeOf<T>() * (long)start), length, Allocator.None);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x000059A4 File Offset: 0x00003BA4
		public NativeArray<T>.ReadOnly AsReadOnly()
		{
			return new NativeArray<T>.ReadOnly(this.m_Buffer, this.m_Length);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x000059C8 File Offset: 0x00003BC8
		[WriteAccessRequired]
		public readonly Span<T> AsSpan()
		{
			return new Span<T>(this.m_Buffer, this.m_Length);
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x000059EC File Offset: 0x00003BEC
		public readonly ReadOnlySpan<T> AsReadOnlySpan()
		{
			return new ReadOnlySpan<T>(this.m_Buffer, this.m_Length);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00005A10 File Offset: 0x00003C10
		public static implicit operator Span<T>(in NativeArray<T> source)
		{
			return source.AsSpan();
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00005A28 File Offset: 0x00003C28
		public static implicit operator ReadOnlySpan<T>(in NativeArray<T> source)
		{
			return source.AsReadOnlySpan();
		}

		// Token: 0x0400022C RID: 556
		[NativeDisableUnsafePtrRestriction]
		internal unsafe void* m_Buffer;

		// Token: 0x0400022D RID: 557
		internal int m_Length;

		// Token: 0x0400022E RID: 558
		internal Allocator m_AllocatorLabel;

		// Token: 0x02000099 RID: 153
		[ExcludeFromDocs]
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			// Token: 0x060002F5 RID: 757 RVA: 0x00005A40 File Offset: 0x00003C40
			public Enumerator(ref NativeArray<T> array)
			{
				this.m_Array = array;
				this.m_Index = -1;
				this.value = default(T);
			}

			// Token: 0x060002F6 RID: 758 RVA: 0x00002669 File Offset: 0x00000869
			public void Dispose()
			{
			}

			// Token: 0x060002F7 RID: 759 RVA: 0x00005A64 File Offset: 0x00003C64
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				this.m_Index++;
				bool flag = this.m_Index < this.m_Array.m_Length;
				bool flag2;
				if (flag)
				{
					this.value = UnsafeUtility.ReadArrayElement<T>(this.m_Array.m_Buffer, this.m_Index);
					flag2 = true;
				}
				else
				{
					this.value = default(T);
					flag2 = false;
				}
				return flag2;
			}

			// Token: 0x060002F8 RID: 760 RVA: 0x00005AC9 File Offset: 0x00003CC9
			public void Reset()
			{
				this.m_Index = -1;
			}

			// Token: 0x1700007F RID: 127
			// (get) Token: 0x060002F9 RID: 761 RVA: 0x00005AD4 File Offset: 0x00003CD4
			public T Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.value;
				}
			}

			// Token: 0x17000080 RID: 128
			// (get) Token: 0x060002FA RID: 762 RVA: 0x00005AEC File Offset: 0x00003CEC
			object IEnumerator.Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.Current;
				}
			}

			// Token: 0x0400022F RID: 559
			private NativeArray<T> m_Array;

			// Token: 0x04000230 RID: 560
			private int m_Index;

			// Token: 0x04000231 RID: 561
			private T value;
		}

		// Token: 0x0200009A RID: 154
		[NativeContainerIsReadOnly]
		[DebuggerDisplay("Length = {Length}")]
		[DebuggerTypeProxy(typeof(NativeArrayReadOnlyDebugView<>))]
		[NativeContainer]
		public struct ReadOnly : IEnumerable<T>, IEnumerable
		{
			// Token: 0x060002FB RID: 763 RVA: 0x00005B09 File Offset: 0x00003D09
			internal unsafe ReadOnly(void* buffer, int length)
			{
				this.m_Buffer = buffer;
				this.m_Length = length;
			}

			// Token: 0x17000081 RID: 129
			// (get) Token: 0x060002FC RID: 764 RVA: 0x00005B1C File Offset: 0x00003D1C
			public int Length
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Length;
				}
			}

			// Token: 0x060002FD RID: 765 RVA: 0x00005B34 File Offset: 0x00003D34
			public void CopyTo(T[] array)
			{
				NativeArray<T>.Copy(this, array);
			}

			// Token: 0x060002FE RID: 766 RVA: 0x00005B43 File Offset: 0x00003D43
			public void CopyTo(NativeArray<T> array)
			{
				NativeArray<T>.Copy(this, array);
			}

			// Token: 0x060002FF RID: 767 RVA: 0x00005B54 File Offset: 0x00003D54
			public T[] ToArray()
			{
				T[] array = new T[this.m_Length];
				NativeArray<T>.Copy(this, array, this.m_Length);
				return array;
			}

			// Token: 0x06000300 RID: 768 RVA: 0x00005B88 File Offset: 0x00003D88
			public NativeArray<U>.ReadOnly Reinterpret<U>() where U : struct
			{
				return new NativeArray<U>.ReadOnly(this.m_Buffer, this.m_Length);
			}

			// Token: 0x17000082 RID: 130
			public T this[int index]
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return UnsafeUtility.ReadArrayElement<T>(this.m_Buffer, index);
				}
			}

			// Token: 0x06000302 RID: 770 RVA: 0x00005BCC File Offset: 0x00003DCC
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void CheckElementReadAccess(int index)
			{
				bool flag = index >= this.m_Length;
				if (flag)
				{
					throw new IndexOutOfRangeException(string.Format("Index {0} is out of range (must be between 0 and {1}).", index, this.m_Length - 1));
				}
			}

			// Token: 0x17000083 RID: 131
			// (get) Token: 0x06000303 RID: 771 RVA: 0x00005C0E File Offset: 0x00003E0E
			public bool IsCreated
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Buffer != null;
				}
			}

			// Token: 0x06000304 RID: 772 RVA: 0x00005C20 File Offset: 0x00003E20
			public NativeArray<T>.ReadOnly.Enumerator GetEnumerator()
			{
				return new NativeArray<T>.ReadOnly.Enumerator(in this);
			}

			// Token: 0x06000305 RID: 773 RVA: 0x00005C38 File Offset: 0x00003E38
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x06000306 RID: 774 RVA: 0x00005C58 File Offset: 0x00003E58
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x06000307 RID: 775 RVA: 0x00005C78 File Offset: 0x00003E78
			public readonly ReadOnlySpan<T> AsReadOnlySpan()
			{
				return new ReadOnlySpan<T>(this.m_Buffer, this.m_Length);
			}

			// Token: 0x06000308 RID: 776 RVA: 0x00005C9C File Offset: 0x00003E9C
			public static implicit operator ReadOnlySpan<T>(in NativeArray<T>.ReadOnly source)
			{
				return source.AsReadOnlySpan();
			}

			// Token: 0x04000232 RID: 562
			[NativeDisableUnsafePtrRestriction]
			internal unsafe void* m_Buffer;

			// Token: 0x04000233 RID: 563
			internal int m_Length;

			// Token: 0x0200009B RID: 155
			[ExcludeFromDocs]
			public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
			{
				// Token: 0x06000309 RID: 777 RVA: 0x00005CB4 File Offset: 0x00003EB4
				public Enumerator(in NativeArray<T>.ReadOnly array)
				{
					this.m_Array = array;
					this.m_Index = -1;
					this.value = default(T);
				}

				// Token: 0x0600030A RID: 778 RVA: 0x00002669 File Offset: 0x00000869
				public void Dispose()
				{
				}

				// Token: 0x0600030B RID: 779 RVA: 0x00005CD8 File Offset: 0x00003ED8
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool MoveNext()
				{
					this.m_Index++;
					bool flag = this.m_Index < this.m_Array.m_Length;
					bool flag2;
					if (flag)
					{
						this.value = UnsafeUtility.ReadArrayElement<T>(this.m_Array.m_Buffer, this.m_Index);
						flag2 = true;
					}
					else
					{
						this.value = default(T);
						flag2 = false;
					}
					return flag2;
				}

				// Token: 0x0600030C RID: 780 RVA: 0x00005D3D File Offset: 0x00003F3D
				public void Reset()
				{
					this.m_Index = -1;
				}

				// Token: 0x17000084 RID: 132
				// (get) Token: 0x0600030D RID: 781 RVA: 0x00005D47 File Offset: 0x00003F47
				public T Current
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					get
					{
						return this.value;
					}
				}

				// Token: 0x17000085 RID: 133
				// (get) Token: 0x0600030E RID: 782 RVA: 0x00005D4F File Offset: 0x00003F4F
				object IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}

				// Token: 0x04000234 RID: 564
				private NativeArray<T>.ReadOnly m_Array;

				// Token: 0x04000235 RID: 565
				private int m_Index;

				// Token: 0x04000236 RID: 566
				private T value;
			}
		}
	}
}
