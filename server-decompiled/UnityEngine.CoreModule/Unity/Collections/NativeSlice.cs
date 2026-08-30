using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Internal;

namespace Unity.Collections
{
	// Token: 0x020000A1 RID: 161
	[NativeContainer]
	[DebuggerDisplay("Length = {Length}")]
	[DebuggerTypeProxy(typeof(NativeSliceDebugView<>))]
	[NativeContainerSupportsMinMaxWriteRestriction]
	public struct NativeSlice<T> : IEnumerable<T>, IEnumerable, IEquatable<NativeSlice<T>> where T : struct
	{
		// Token: 0x0600031C RID: 796 RVA: 0x00005EF6 File Offset: 0x000040F6
		public NativeSlice(NativeSlice<T> slice, int start)
		{
			this = new NativeSlice<T>(slice, start, slice.Length - start);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00005F0B File Offset: 0x0000410B
		public NativeSlice(NativeSlice<T> slice, int start, int length)
		{
			this.m_Stride = slice.m_Stride;
			this.m_Buffer = slice.m_Buffer + this.m_Stride * start;
			this.m_Length = length;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00005F36 File Offset: 0x00004136
		public NativeSlice(NativeArray<T> array)
		{
			this = new NativeSlice<T>(array, 0, array.Length);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00005F49 File Offset: 0x00004149
		public NativeSlice(NativeArray<T> array, int start)
		{
			this = new NativeSlice<T>(array, start, array.Length - start);
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00005F60 File Offset: 0x00004160
		public static implicit operator NativeSlice<T>(NativeArray<T> array)
		{
			return new NativeSlice<T>(array);
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00005F78 File Offset: 0x00004178
		public unsafe NativeSlice(NativeArray<T> array, int start, int length)
		{
			this.m_Stride = UnsafeUtility.SizeOf<T>();
			byte* ptr = (byte*)array.m_Buffer + this.m_Stride * start;
			this.m_Buffer = ptr;
			this.m_Length = length;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00005FB0 File Offset: 0x000041B0
		public NativeSlice<U> SliceConvert<U>() where U : struct
		{
			int num = UnsafeUtility.SizeOf<U>();
			NativeSlice<U> nativeSlice;
			nativeSlice.m_Buffer = this.m_Buffer;
			nativeSlice.m_Stride = num;
			nativeSlice.m_Length = this.m_Length * this.m_Stride / num;
			return nativeSlice;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00005FF4 File Offset: 0x000041F4
		public NativeSlice<U> SliceWithStride<U>(int offset) where U : struct
		{
			NativeSlice<U> nativeSlice;
			nativeSlice.m_Buffer = this.m_Buffer + offset;
			nativeSlice.m_Stride = this.m_Stride;
			nativeSlice.m_Length = this.m_Length;
			return nativeSlice;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00006030 File Offset: 0x00004230
		public NativeSlice<U> SliceWithStride<U>() where U : struct
		{
			return this.SliceWithStride<U>(0);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00002669 File Offset: 0x00000869
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private void CheckReadIndex(int index)
		{
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00002669 File Offset: 0x00000869
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private void CheckWriteIndex(int index)
		{
		}

		// Token: 0x17000088 RID: 136
		public unsafe T this[int index]
		{
			get
			{
				return UnsafeUtility.ReadArrayElementWithStride<T>((void*)this.m_Buffer, index, this.m_Stride);
			}
			[WriteAccessRequired]
			set
			{
				UnsafeUtility.WriteArrayElementWithStride<T>((void*)this.m_Buffer, index, this.m_Stride, value);
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00006087 File Offset: 0x00004287
		[WriteAccessRequired]
		public void CopyFrom(NativeSlice<T> slice)
		{
			UnsafeUtility.MemCpyStride(this.GetUnsafePtr<T>(), this.Stride, slice.GetUnsafeReadOnlyPtr<T>(), slice.Stride, UnsafeUtility.SizeOf<T>(), this.m_Length);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x000060BC File Offset: 0x000042BC
		[WriteAccessRequired]
		public unsafe void CopyFrom(T[] array)
		{
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			IntPtr intPtr = gchandle.AddrOfPinnedObject();
			int num = UnsafeUtility.SizeOf<T>();
			UnsafeUtility.MemCpyStride(this.GetUnsafePtr<T>(), this.Stride, (void*)intPtr, num, num, this.m_Length);
			gchandle.Free();
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00006110 File Offset: 0x00004310
		public void CopyTo(NativeArray<T> array)
		{
			int num = UnsafeUtility.SizeOf<T>();
			UnsafeUtility.MemCpyStride(array.GetUnsafePtr<T>(), num, this.GetUnsafeReadOnlyPtr<T>(), this.Stride, num, this.m_Length);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000614C File Offset: 0x0000434C
		public unsafe void CopyTo(T[] array)
		{
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			IntPtr intPtr = gchandle.AddrOfPinnedObject();
			int num = UnsafeUtility.SizeOf<T>();
			UnsafeUtility.MemCpyStride((void*)intPtr, num, this.GetUnsafeReadOnlyPtr<T>(), this.Stride, num, this.m_Length);
			gchandle.Free();
		}

		// Token: 0x0600032D RID: 813 RVA: 0x000061A0 File Offset: 0x000043A0
		public T[] ToArray()
		{
			T[] array = new T[this.Length];
			this.CopyTo(array);
			return array;
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600032E RID: 814 RVA: 0x000061C7 File Offset: 0x000043C7
		public int Stride
		{
			get
			{
				return this.m_Stride;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600032F RID: 815 RVA: 0x000061D0 File Offset: 0x000043D0
		public int Length
		{
			get
			{
				return this.m_Length;
			}
		}

		// Token: 0x06000330 RID: 816 RVA: 0x000061E8 File Offset: 0x000043E8
		public NativeSlice<T>.Enumerator GetEnumerator()
		{
			return new NativeSlice<T>.Enumerator(ref this);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00006200 File Offset: 0x00004400
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new NativeSlice<T>.Enumerator(ref this);
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00006220 File Offset: 0x00004420
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00006240 File Offset: 0x00004440
		public bool Equals(NativeSlice<T> other)
		{
			return this.m_Buffer == other.m_Buffer && this.m_Stride == other.m_Stride && this.m_Length == other.m_Length;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00006280 File Offset: 0x00004480
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is NativeSlice<T> && this.Equals((NativeSlice<T>)obj);
		}

		// Token: 0x06000335 RID: 821 RVA: 0x000062B8 File Offset: 0x000044B8
		public override int GetHashCode()
		{
			int num = this.m_Buffer;
			num = (num * 397) ^ this.m_Stride;
			return (num * 397) ^ this.m_Length;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x000062F4 File Offset: 0x000044F4
		public static bool operator ==(NativeSlice<T> left, NativeSlice<T> right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x00006310 File Offset: 0x00004510
		public static bool operator !=(NativeSlice<T> left, NativeSlice<T> right)
		{
			return !left.Equals(right);
		}

		// Token: 0x0400023C RID: 572
		[NativeDisableUnsafePtrRestriction]
		internal unsafe byte* m_Buffer;

		// Token: 0x0400023D RID: 573
		internal int m_Stride;

		// Token: 0x0400023E RID: 574
		internal int m_Length;

		// Token: 0x020000A2 RID: 162
		[ExcludeFromDocs]
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			// Token: 0x06000338 RID: 824 RVA: 0x0000632D File Offset: 0x0000452D
			public Enumerator(ref NativeSlice<T> array)
			{
				this.m_Array = array;
				this.m_Index = -1;
			}

			// Token: 0x06000339 RID: 825 RVA: 0x00002669 File Offset: 0x00000869
			public void Dispose()
			{
			}

			// Token: 0x0600033A RID: 826 RVA: 0x00006344 File Offset: 0x00004544
			public bool MoveNext()
			{
				this.m_Index++;
				return this.m_Index < this.m_Array.Length;
			}

			// Token: 0x0600033B RID: 827 RVA: 0x00006377 File Offset: 0x00004577
			public void Reset()
			{
				this.m_Index = -1;
			}

			// Token: 0x1700008B RID: 139
			// (get) Token: 0x0600033C RID: 828 RVA: 0x00006381 File Offset: 0x00004581
			public T Current
			{
				get
				{
					return this.m_Array[this.m_Index];
				}
			}

			// Token: 0x1700008C RID: 140
			// (get) Token: 0x0600033D RID: 829 RVA: 0x00006394 File Offset: 0x00004594
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x0400023F RID: 575
			private NativeSlice<T> m_Array;

			// Token: 0x04000240 RID: 576
			private int m_Index;
		}
	}
}
