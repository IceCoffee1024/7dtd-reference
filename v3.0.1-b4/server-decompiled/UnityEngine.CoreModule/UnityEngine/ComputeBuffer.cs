using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200027B RID: 635
	[NativeHeader("Runtime/Export/Graphics/GraphicsBuffer.bindings.h")]
	[NativeHeader("Runtime/Shaders/GraphicsBuffer.h")]
	[NativeClass("GraphicsBuffer")]
	[UsedByNativeCode]
	public sealed class ComputeBuffer : IDisposable
	{
		// Token: 0x06001A37 RID: 6711 RVA: 0x0002C2FC File Offset: 0x0002A4FC
		~ComputeBuffer()
		{
			this.Dispose(false);
		}

		// Token: 0x06001A38 RID: 6712 RVA: 0x0002C330 File Offset: 0x0002A530
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001A39 RID: 6713 RVA: 0x0002C344 File Offset: 0x0002A544
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				ComputeBuffer.DestroyBuffer(this);
			}
			else
			{
				bool flag = this.m_Ptr != IntPtr.Zero;
				if (flag)
				{
					Debug.LogWarning("GarbageCollector disposing of ComputeBuffer. Please use ComputeBuffer.Release() or .Dispose() to manually release the buffer.");
				}
			}
			this.m_Ptr = IntPtr.Zero;
		}

		// Token: 0x06001A3A RID: 6714
		[FreeFunction("GraphicsBuffer_Bindings::InitComputeBuffer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr InitBuffer(int count, int stride, ComputeBufferType type, ComputeBufferMode usage);

		// Token: 0x06001A3B RID: 6715
		[FreeFunction("GraphicsBuffer_Bindings::DestroyComputeBuffer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DestroyBuffer(ComputeBuffer buf);

		// Token: 0x06001A3C RID: 6716 RVA: 0x0002C38E File Offset: 0x0002A58E
		public ComputeBuffer(int count, int stride)
			: this(count, stride, ComputeBufferType.Default, ComputeBufferMode.Immutable, 3)
		{
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x0002C39D File Offset: 0x0002A59D
		public ComputeBuffer(int count, int stride, ComputeBufferType type)
			: this(count, stride, type, ComputeBufferMode.Immutable, 3)
		{
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x0002C3AC File Offset: 0x0002A5AC
		public ComputeBuffer(int count, int stride, ComputeBufferType type, ComputeBufferMode usage)
			: this(count, stride, type, usage, 3)
		{
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x0002C3BC File Offset: 0x0002A5BC
		private ComputeBuffer(int count, int stride, ComputeBufferType type, ComputeBufferMode usage, int stackDepth)
		{
			bool flag = count <= 0;
			if (flag)
			{
				throw new ArgumentException("Attempting to create a zero length compute buffer", "count");
			}
			bool flag2 = stride <= 0;
			if (flag2)
			{
				throw new ArgumentException("Attempting to create a compute buffer with a negative or null stride", "stride");
			}
			long num = (long)count * (long)stride;
			long maxGraphicsBufferSize = SystemInfo.maxGraphicsBufferSize;
			bool flag3 = num > maxGraphicsBufferSize;
			if (flag3)
			{
				throw new ArgumentException(string.Format("The total size of the compute buffer ({0} bytes) exceeds the maximum buffer size. Maximum supported buffer size: {1} bytes.", num, maxGraphicsBufferSize));
			}
			this.m_Ptr = ComputeBuffer.InitBuffer(count, stride, type, usage);
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x0002C44C File Offset: 0x0002A64C
		public void Release()
		{
			this.Dispose();
		}

		// Token: 0x06001A41 RID: 6721
		[FreeFunction("GraphicsBuffer_Bindings::IsValidBuffer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsValidBuffer(ComputeBuffer buf);

		// Token: 0x06001A42 RID: 6722 RVA: 0x0002C458 File Offset: 0x0002A658
		public bool IsValid()
		{
			return this.m_Ptr != IntPtr.Zero && ComputeBuffer.IsValidBuffer(this);
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06001A43 RID: 6723
		public extern int count
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06001A44 RID: 6724
		public extern int stride
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06001A45 RID: 6725
		private extern ComputeBufferMode usage
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x0002C488 File Offset: 0x0002A688
		[SecuritySafeCritical]
		public void SetData(Array data)
		{
			bool flag = data == null;
			if (flag)
			{
				throw new ArgumentNullException("data");
			}
			bool flag2 = !UnsafeUtility.IsArrayBlittable(data);
			if (flag2)
			{
				throw new ArgumentException(string.Format("Array passed to ComputeBuffer.SetData(array) must be blittable.\n{0}", UnsafeUtility.GetReasonForArrayNonBlittable(data)));
			}
			this.InternalSetData(data, 0, 0, data.Length, UnsafeUtility.SizeOf(data.GetType().GetElementType()));
		}

		// Token: 0x06001A47 RID: 6727 RVA: 0x0002C4F0 File Offset: 0x0002A6F0
		[SecuritySafeCritical]
		public void SetData<T>(List<T> data) where T : struct
		{
			bool flag = data == null;
			if (flag)
			{
				throw new ArgumentNullException("data");
			}
			bool flag2 = !UnsafeUtility.IsGenericListBlittable<T>();
			if (flag2)
			{
				throw new ArgumentException(string.Format("List<{0}> passed to ComputeBuffer.SetData(List<>) must be blittable.\n{1}", typeof(T), UnsafeUtility.GetReasonForGenericListNonBlittable<T>()));
			}
			this.InternalSetData(NoAllocHelpers.ExtractArrayFromList(data), 0, 0, NoAllocHelpers.SafeLength<T>(data), Marshal.SizeOf(typeof(T)));
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x0002C561 File Offset: 0x0002A761
		[SecuritySafeCritical]
		public void SetData<T>(NativeArray<T> data) where T : struct
		{
			this.InternalSetNativeData((IntPtr)data.GetUnsafeReadOnlyPtr<T>(), 0, 0, data.Length, UnsafeUtility.SizeOf<T>());
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x0002C584 File Offset: 0x0002A784
		[SecuritySafeCritical]
		public void SetData(Array data, int managedBufferStartIndex, int computeBufferStartIndex, int count)
		{
			bool flag = data == null;
			if (flag)
			{
				throw new ArgumentNullException("data");
			}
			bool flag2 = !UnsafeUtility.IsArrayBlittable(data);
			if (flag2)
			{
				throw new ArgumentException(string.Format("Array passed to ComputeBuffer.SetData(array) must be blittable.\n{0}", UnsafeUtility.GetReasonForArrayNonBlittable(data)));
			}
			bool flag3 = managedBufferStartIndex < 0 || computeBufferStartIndex < 0 || count < 0 || managedBufferStartIndex + count > data.Length;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad indices/count arguments (managedBufferStartIndex:{0} computeBufferStartIndex:{1} count:{2})", managedBufferStartIndex, computeBufferStartIndex, count));
			}
			this.InternalSetData(data, managedBufferStartIndex, computeBufferStartIndex, count, Marshal.SizeOf(data.GetType().GetElementType()));
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x0002C628 File Offset: 0x0002A828
		[SecuritySafeCritical]
		public void SetData<T>(List<T> data, int managedBufferStartIndex, int computeBufferStartIndex, int count) where T : struct
		{
			bool flag = data == null;
			if (flag)
			{
				throw new ArgumentNullException("data");
			}
			bool flag2 = !UnsafeUtility.IsGenericListBlittable<T>();
			if (flag2)
			{
				throw new ArgumentException(string.Format("List<{0}> passed to ComputeBuffer.SetData(List<>) must be blittable.\n{1}", typeof(T), UnsafeUtility.GetReasonForGenericListNonBlittable<T>()));
			}
			bool flag3 = managedBufferStartIndex < 0 || computeBufferStartIndex < 0 || count < 0 || managedBufferStartIndex + count > data.Count;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad indices/count arguments (managedBufferStartIndex:{0} computeBufferStartIndex:{1} count:{2})", managedBufferStartIndex, computeBufferStartIndex, count));
			}
			this.InternalSetData(NoAllocHelpers.ExtractArrayFromList(data), managedBufferStartIndex, computeBufferStartIndex, count, Marshal.SizeOf(typeof(T)));
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x0002C6D8 File Offset: 0x0002A8D8
		[SecuritySafeCritical]
		public void SetData<T>(NativeArray<T> data, int nativeBufferStartIndex, int computeBufferStartIndex, int count) where T : struct
		{
			bool flag = nativeBufferStartIndex < 0 || computeBufferStartIndex < 0 || count < 0 || nativeBufferStartIndex + count > data.Length;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad indices/count arguments (nativeBufferStartIndex:{0} computeBufferStartIndex:{1} count:{2})", nativeBufferStartIndex, computeBufferStartIndex, count));
			}
			this.InternalSetNativeData((IntPtr)data.GetUnsafeReadOnlyPtr<T>(), nativeBufferStartIndex, computeBufferStartIndex, count, UnsafeUtility.SizeOf<T>());
		}

		// Token: 0x06001A4C RID: 6732
		[FreeFunction(Name = "GraphicsBuffer_Bindings::InternalSetNativeData", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InternalSetNativeData(IntPtr data, int nativeBufferStartIndex, int computeBufferStartIndex, int count, int elemSize);

		// Token: 0x06001A4D RID: 6733
		[FreeFunction(Name = "GraphicsBuffer_Bindings::InternalSetData", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InternalSetData(Array data, int managedBufferStartIndex, int computeBufferStartIndex, int count, int elemSize);

		// Token: 0x06001A4E RID: 6734 RVA: 0x0002C748 File Offset: 0x0002A948
		[SecurityCritical]
		public void GetData(Array data)
		{
			bool flag = data == null;
			if (flag)
			{
				throw new ArgumentNullException("data");
			}
			bool flag2 = !UnsafeUtility.IsArrayBlittable(data);
			if (flag2)
			{
				throw new ArgumentException(string.Format("Array passed to ComputeBuffer.GetData(array) must be blittable.\n{0}", UnsafeUtility.GetReasonForArrayNonBlittable(data)));
			}
			this.InternalGetData(data, 0, 0, data.Length, Marshal.SizeOf(data.GetType().GetElementType()));
		}

		// Token: 0x06001A4F RID: 6735 RVA: 0x0002C7B0 File Offset: 0x0002A9B0
		[SecurityCritical]
		public void GetData(Array data, int managedBufferStartIndex, int computeBufferStartIndex, int count)
		{
			bool flag = data == null;
			if (flag)
			{
				throw new ArgumentNullException("data");
			}
			bool flag2 = !UnsafeUtility.IsArrayBlittable(data);
			if (flag2)
			{
				throw new ArgumentException(string.Format("Array passed to ComputeBuffer.GetData(array) must be blittable.\n{0}", UnsafeUtility.GetReasonForArrayNonBlittable(data)));
			}
			bool flag3 = managedBufferStartIndex < 0 || computeBufferStartIndex < 0 || count < 0 || managedBufferStartIndex + count > data.Length;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad indices/count argument (managedBufferStartIndex:{0} computeBufferStartIndex:{1} count:{2})", managedBufferStartIndex, computeBufferStartIndex, count));
			}
			this.InternalGetData(data, managedBufferStartIndex, computeBufferStartIndex, count, Marshal.SizeOf(data.GetType().GetElementType()));
		}

		// Token: 0x06001A50 RID: 6736
		[FreeFunction(Name = "GraphicsBuffer_Bindings::InternalGetData", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InternalGetData(Array data, int managedBufferStartIndex, int computeBufferStartIndex, int count, int elemSize);

		// Token: 0x06001A51 RID: 6737
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* BeginBufferWrite(int offset = 0, int size = 0);

		// Token: 0x06001A52 RID: 6738 RVA: 0x0002C854 File Offset: 0x0002AA54
		public unsafe NativeArray<T> BeginWrite<T>(int computeBufferStartIndex, int count) where T : struct
		{
			bool flag = !this.IsValid();
			if (flag)
			{
				throw new InvalidOperationException("BeginWrite requires a valid ComputeBuffer");
			}
			bool flag2 = this.usage != ComputeBufferMode.SubUpdates;
			if (flag2)
			{
				throw new ArgumentException("ComputeBuffer must be created with usage mode ComputeBufferMode.SubUpdates to be able to be mapped with BeginWrite");
			}
			int num = UnsafeUtility.SizeOf<T>();
			bool flag3 = computeBufferStartIndex < 0 || count < 0 || (computeBufferStartIndex + count) * num > this.count * this.stride;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad indices/count arguments (computeBufferStartIndex:{0} count:{1} elementSize:{2}, this.count:{3}, this.stride{4})", new object[] { computeBufferStartIndex, count, num, this.count, this.stride }));
			}
			void* ptr = this.BeginBufferWrite(computeBufferStartIndex * num, count * num);
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>(ptr, count, Allocator.Invalid);
		}

		// Token: 0x06001A53 RID: 6739
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void EndBufferWrite(int bytesWritten = 0);

		// Token: 0x06001A54 RID: 6740 RVA: 0x0002C930 File Offset: 0x0002AB30
		public void EndWrite<T>(int countWritten) where T : struct
		{
			bool flag = countWritten < 0;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad indices/count arguments (countWritten:{0})", countWritten));
			}
			int num = UnsafeUtility.SizeOf<T>();
			this.EndBufferWrite(countWritten * num);
		}

		// Token: 0x170004EB RID: 1259
		// (set) Token: 0x06001A55 RID: 6741 RVA: 0x0002C96C File Offset: 0x0002AB6C
		public string name
		{
			set
			{
				this.SetName(value);
			}
		}

		// Token: 0x06001A56 RID: 6742
		[FreeFunction(Name = "GraphicsBuffer_Bindings::SetName", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetName(string name);

		// Token: 0x06001A57 RID: 6743
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetCounterValue(uint counterValue);

		// Token: 0x06001A58 RID: 6744
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CopyCount(ComputeBuffer src, ComputeBuffer dst, int dstOffsetBytes);

		// Token: 0x06001A59 RID: 6745
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern IntPtr GetNativeBufferPtr();

		// Token: 0x04000918 RID: 2328
		internal IntPtr m_Ptr;
	}
}
