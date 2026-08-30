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
	// Token: 0x0200016B RID: 363
	[NativeHeader("Runtime/Export/Graphics/GraphicsBuffer.bindings.h")]
	[UsedByNativeCode]
	[NativeHeader("Runtime/Shaders/GraphicsBuffer.h")]
	public sealed class GraphicsBuffer : IDisposable
	{
		// Token: 0x06000F19 RID: 3865 RVA: 0x000152B8 File Offset: 0x000134B8
		~GraphicsBuffer()
		{
			this.Dispose(false);
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x000152EC File Offset: 0x000134EC
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x00015300 File Offset: 0x00013500
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				GraphicsBuffer.DestroyBuffer(this);
			}
			else
			{
				bool flag = this.m_Ptr != IntPtr.Zero;
				if (flag)
				{
					Debug.LogWarning("GarbageCollector disposing of GraphicsBuffer. Please use GraphicsBuffer.Release() or .Dispose() to manually release the buffer.");
				}
			}
			this.m_Ptr = IntPtr.Zero;
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x0001534C File Offset: 0x0001354C
		private static bool RequiresCompute(GraphicsBuffer.Target target)
		{
			GraphicsBuffer.Target target2 = GraphicsBuffer.Target.Structured | GraphicsBuffer.Target.Raw | GraphicsBuffer.Target.Append | GraphicsBuffer.Target.Counter | GraphicsBuffer.Target.IndirectArguments;
			return (target & target2) > (GraphicsBuffer.Target)0;
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x0001536C File Offset: 0x0001356C
		private static bool IsVertexIndexOrCopyOnly(GraphicsBuffer.Target target)
		{
			GraphicsBuffer.Target target2 = GraphicsBuffer.Target.Vertex | GraphicsBuffer.Target.Index | GraphicsBuffer.Target.CopySource | GraphicsBuffer.Target.CopyDestination;
			return (target & target2) == target;
		}

		// Token: 0x06000F1E RID: 3870
		[FreeFunction("GraphicsBuffer_Bindings::InitBuffer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr InitBuffer(GraphicsBuffer.Target target, GraphicsBuffer.UsageFlags usageFlags, int count, int stride);

		// Token: 0x06000F1F RID: 3871
		[FreeFunction("GraphicsBuffer_Bindings::DestroyBuffer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DestroyBuffer(GraphicsBuffer buf);

		// Token: 0x06000F20 RID: 3872 RVA: 0x00015388 File Offset: 0x00013588
		public GraphicsBuffer(GraphicsBuffer.Target target, int count, int stride)
		{
			GraphicsBuffer.UsageFlags usageFlags = (((target & (GraphicsBuffer.Target.Vertex | GraphicsBuffer.Target.Index)) == target) ? GraphicsBuffer.UsageFlags.LockBufferForWrite : GraphicsBuffer.UsageFlags.None);
			this.InternalInitialization(target, usageFlags, count, stride);
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x000153B7 File Offset: 0x000135B7
		public GraphicsBuffer(GraphicsBuffer.Target target, GraphicsBuffer.UsageFlags usageFlags, int count, int stride)
		{
			this.InternalInitialization(target, usageFlags, count, stride);
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x000153D0 File Offset: 0x000135D0
		private void InternalInitialization(GraphicsBuffer.Target target, GraphicsBuffer.UsageFlags usageFlags, int count, int stride)
		{
			bool flag = GraphicsBuffer.RequiresCompute(target) && !SystemInfo.supportsComputeShaders;
			if (flag)
			{
				throw new ArgumentException("Attempting to create a graphics buffer that requires compute shader support, but compute shaders are not supported on this platform. Target: " + target.ToString());
			}
			bool flag2 = count <= 0;
			if (flag2)
			{
				throw new ArgumentException("Attempting to create a zero length graphics buffer", "count");
			}
			bool flag3 = stride <= 0;
			if (flag3)
			{
				throw new ArgumentException("Attempting to create a graphics buffer with a negative or null stride", "stride");
			}
			bool flag4 = (target & GraphicsBuffer.Target.Index) != (GraphicsBuffer.Target)0 && stride != 2 && stride != 4;
			if (flag4)
			{
				throw new ArgumentException("Attempting to create an index buffer with an invalid stride: " + stride.ToString(), "stride");
			}
			bool flag5 = !GraphicsBuffer.IsVertexIndexOrCopyOnly(target) && stride % 4 != 0;
			if (flag5)
			{
				throw new ArgumentException("Stride must be a multiple of 4 unless the buffer is only used as a vertex buffer and/or index buffer ", "stride");
			}
			long num = (long)count * (long)stride;
			long maxGraphicsBufferSize = SystemInfo.maxGraphicsBufferSize;
			bool flag6 = num > maxGraphicsBufferSize;
			if (flag6)
			{
				throw new ArgumentException(string.Format("The total size of the graphics buffer ({0} bytes) exceeds the maximum buffer size. Maximum supported buffer size: {1} bytes.", num, maxGraphicsBufferSize));
			}
			bool flag7 = (usageFlags & GraphicsBuffer.UsageFlags.LockBufferForWrite) != GraphicsBuffer.UsageFlags.None && (target & GraphicsBuffer.Target.CopyDestination) > (GraphicsBuffer.Target)0;
			if (flag7)
			{
				throw new ArgumentException("Attempting to create a LockBufferForWrite capable buffer that can be copied into. LockBufferForWrite buffers are read-only on the GPU.");
			}
			this.m_Ptr = GraphicsBuffer.InitBuffer(target, usageFlags, count, stride);
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x00015513 File Offset: 0x00013713
		public void Release()
		{
			this.Dispose();
		}

		// Token: 0x06000F24 RID: 3876
		[FreeFunction("GraphicsBuffer_Bindings::IsValidBuffer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsValidBuffer(GraphicsBuffer buf);

		// Token: 0x06000F25 RID: 3877 RVA: 0x00015520 File Offset: 0x00013720
		public bool IsValid()
		{
			return this.m_Ptr != IntPtr.Zero && GraphicsBuffer.IsValidBuffer(this);
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000F26 RID: 3878
		public extern int count
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000F27 RID: 3879
		public extern int stride
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000F28 RID: 3880
		public extern GraphicsBuffer.Target target
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000F29 RID: 3881
		[FreeFunction(Name = "GraphicsBuffer_Bindings::GetUsageFlags", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern GraphicsBuffer.UsageFlags GetUsageFlags();

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000F2A RID: 3882 RVA: 0x00015550 File Offset: 0x00013750
		public GraphicsBuffer.UsageFlags usageFlags
		{
			get
			{
				return this.GetUsageFlags();
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000F2B RID: 3883 RVA: 0x00015568 File Offset: 0x00013768
		public GraphicsBufferHandle bufferHandle
		{
			get
			{
				GraphicsBufferHandle graphicsBufferHandle;
				this.get_bufferHandle_Injected(out graphicsBufferHandle);
				return graphicsBufferHandle;
			}
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x00015580 File Offset: 0x00013780
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
				throw new ArgumentException(string.Format("Array passed to GraphicsBuffer.SetData(array) must be blittable.\n{0}", UnsafeUtility.GetReasonForArrayNonBlittable(data)));
			}
			this.InternalSetData(data, 0, 0, data.Length, UnsafeUtility.SizeOf(data.GetType().GetElementType()));
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x000155E8 File Offset: 0x000137E8
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
				throw new ArgumentException(string.Format("List<{0}> passed to GraphicsBuffer.SetData(List<>) must be blittable.\n{1}", typeof(T), UnsafeUtility.GetReasonForGenericListNonBlittable<T>()));
			}
			this.InternalSetData(NoAllocHelpers.ExtractArrayFromList(data), 0, 0, NoAllocHelpers.SafeLength<T>(data), Marshal.SizeOf(typeof(T)));
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x00015659 File Offset: 0x00013859
		[SecuritySafeCritical]
		public void SetData<T>(NativeArray<T> data) where T : struct
		{
			this.InternalSetNativeData((IntPtr)data.GetUnsafeReadOnlyPtr<T>(), 0, 0, data.Length, UnsafeUtility.SizeOf<T>());
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x0001567C File Offset: 0x0001387C
		[SecuritySafeCritical]
		public void SetData(Array data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count)
		{
			bool flag = data == null;
			if (flag)
			{
				throw new ArgumentNullException("data");
			}
			bool flag2 = !UnsafeUtility.IsArrayBlittable(data);
			if (flag2)
			{
				throw new ArgumentException(string.Format("Array passed to GraphicsBuffer.SetData(array) must be blittable.\n{0}", UnsafeUtility.GetReasonForArrayNonBlittable(data)));
			}
			bool flag3 = managedBufferStartIndex < 0 || graphicsBufferStartIndex < 0 || count < 0 || managedBufferStartIndex + count > data.Length;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad indices/count arguments (managedBufferStartIndex:{0} graphicsBufferStartIndex:{1} count:{2})", managedBufferStartIndex, graphicsBufferStartIndex, count));
			}
			this.InternalSetData(data, managedBufferStartIndex, graphicsBufferStartIndex, count, Marshal.SizeOf(data.GetType().GetElementType()));
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x00015720 File Offset: 0x00013920
		[SecuritySafeCritical]
		public void SetData<T>(List<T> data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where T : struct
		{
			bool flag = data == null;
			if (flag)
			{
				throw new ArgumentNullException("data");
			}
			bool flag2 = !UnsafeUtility.IsGenericListBlittable<T>();
			if (flag2)
			{
				throw new ArgumentException(string.Format("List<{0}> passed to GraphicsBuffer.SetData(List<>) must be blittable.\n{1}", typeof(T), UnsafeUtility.GetReasonForGenericListNonBlittable<T>()));
			}
			bool flag3 = managedBufferStartIndex < 0 || graphicsBufferStartIndex < 0 || count < 0 || managedBufferStartIndex + count > data.Count;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad indices/count arguments (managedBufferStartIndex:{0} graphicsBufferStartIndex:{1} count:{2})", managedBufferStartIndex, graphicsBufferStartIndex, count));
			}
			this.InternalSetData(NoAllocHelpers.ExtractArrayFromList(data), managedBufferStartIndex, graphicsBufferStartIndex, count, Marshal.SizeOf(typeof(T)));
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x000157D0 File Offset: 0x000139D0
		[SecuritySafeCritical]
		public void SetData<T>(NativeArray<T> data, int nativeBufferStartIndex, int graphicsBufferStartIndex, int count) where T : struct
		{
			bool flag = nativeBufferStartIndex < 0 || graphicsBufferStartIndex < 0 || count < 0 || nativeBufferStartIndex + count > data.Length;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad indices/count arguments (nativeBufferStartIndex:{0} graphicsBufferStartIndex:{1} count:{2})", nativeBufferStartIndex, graphicsBufferStartIndex, count));
			}
			this.InternalSetNativeData((IntPtr)data.GetUnsafeReadOnlyPtr<T>(), nativeBufferStartIndex, graphicsBufferStartIndex, count, UnsafeUtility.SizeOf<T>());
		}

		// Token: 0x06000F32 RID: 3890
		[FreeFunction(Name = "GraphicsBuffer_Bindings::InternalSetNativeData", HasExplicitThis = true, ThrowsException = true)]
		[SecurityCritical]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InternalSetNativeData(IntPtr data, int nativeBufferStartIndex, int graphicsBufferStartIndex, int count, int elemSize);

		// Token: 0x06000F33 RID: 3891
		[SecurityCritical]
		[FreeFunction(Name = "GraphicsBuffer_Bindings::InternalSetData", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InternalSetData(Array data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count, int elemSize);

		// Token: 0x06000F34 RID: 3892 RVA: 0x00015840 File Offset: 0x00013A40
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
				throw new ArgumentException(string.Format("Array passed to GraphicsBuffer.GetData(array) must be blittable.\n{0}", UnsafeUtility.GetReasonForArrayNonBlittable(data)));
			}
			this.InternalGetData(data, 0, 0, data.Length, Marshal.SizeOf(data.GetType().GetElementType()));
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x000158A8 File Offset: 0x00013AA8
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
				throw new ArgumentException(string.Format("Array passed to GraphicsBuffer.GetData(array) must be blittable.\n{0}", UnsafeUtility.GetReasonForArrayNonBlittable(data)));
			}
			bool flag3 = managedBufferStartIndex < 0 || computeBufferStartIndex < 0 || count < 0 || managedBufferStartIndex + count > data.Length;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad indices/count argument (managedBufferStartIndex:{0} computeBufferStartIndex:{1} count:{2})", managedBufferStartIndex, computeBufferStartIndex, count));
			}
			this.InternalGetData(data, managedBufferStartIndex, computeBufferStartIndex, count, Marshal.SizeOf(data.GetType().GetElementType()));
		}

		// Token: 0x06000F36 RID: 3894
		[SecurityCritical]
		[FreeFunction(Name = "GraphicsBuffer_Bindings::InternalGetData", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InternalGetData(Array data, int managedBufferStartIndex, int computeBufferStartIndex, int count, int elemSize);

		// Token: 0x06000F37 RID: 3895
		[FreeFunction(Name = "GraphicsBuffer_Bindings::InternalGetNativeBufferPtr", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern IntPtr GetNativeBufferPtr();

		// Token: 0x06000F38 RID: 3896
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* BeginBufferWrite(int offset = 0, int size = 0);

		// Token: 0x06000F39 RID: 3897 RVA: 0x0001594C File Offset: 0x00013B4C
		public unsafe NativeArray<T> LockBufferForWrite<T>(int bufferStartIndex, int count) where T : struct
		{
			bool flag = !this.IsValid();
			if (flag)
			{
				throw new InvalidOperationException("LockBufferForWrite requires a valid GraphicsBuffer");
			}
			bool flag2 = (this.usageFlags & GraphicsBuffer.UsageFlags.LockBufferForWrite) == GraphicsBuffer.UsageFlags.None;
			if (flag2)
			{
				throw new InvalidOperationException("GraphicsBuffer must be created with usage mode UsageFlage.LockBufferForWrite to use LockBufferForWrite");
			}
			int num = UnsafeUtility.SizeOf<T>();
			bool flag3 = bufferStartIndex < 0 || count < 0 || (bufferStartIndex + count) * num > this.count * this.stride;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad indices/count arguments (bufferStartIndex:{0} count:{1} elementSize:{2}, this.count:{3}, this.stride{4})", new object[] { bufferStartIndex, count, num, this.count, this.stride }));
			}
			void* ptr = this.BeginBufferWrite(bufferStartIndex * num, count * num);
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>(ptr, count, Allocator.Invalid);
		}

		// Token: 0x06000F3A RID: 3898
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void EndBufferWrite(int bytesWritten = 0);

		// Token: 0x06000F3B RID: 3899 RVA: 0x00015A28 File Offset: 0x00013C28
		public void UnlockBufferAfterWrite<T>(int countWritten) where T : struct
		{
			bool flag = countWritten < 0;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad indices/count arguments (countWritten:{0})", countWritten));
			}
			int num = UnsafeUtility.SizeOf<T>();
			this.EndBufferWrite(countWritten * num);
		}

		// Token: 0x17000305 RID: 773
		// (set) Token: 0x06000F3C RID: 3900 RVA: 0x00015A64 File Offset: 0x00013C64
		public string name
		{
			set
			{
				this.SetName(value);
			}
		}

		// Token: 0x06000F3D RID: 3901
		[FreeFunction(Name = "GraphicsBuffer_Bindings::SetName", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetName(string name);

		// Token: 0x06000F3E RID: 3902
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetCounterValue(uint counterValue);

		// Token: 0x06000F3F RID: 3903
		[FreeFunction(Name = "GraphicsBuffer_Bindings::CopyCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CopyCountCC(ComputeBuffer src, ComputeBuffer dst, int dstOffsetBytes);

		// Token: 0x06000F40 RID: 3904
		[FreeFunction(Name = "GraphicsBuffer_Bindings::CopyCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CopyCountGC(GraphicsBuffer src, ComputeBuffer dst, int dstOffsetBytes);

		// Token: 0x06000F41 RID: 3905
		[FreeFunction(Name = "GraphicsBuffer_Bindings::CopyCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CopyCountCG(ComputeBuffer src, GraphicsBuffer dst, int dstOffsetBytes);

		// Token: 0x06000F42 RID: 3906
		[FreeFunction(Name = "GraphicsBuffer_Bindings::CopyCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CopyCountGG(GraphicsBuffer src, GraphicsBuffer dst, int dstOffsetBytes);

		// Token: 0x06000F43 RID: 3907 RVA: 0x00015A6E File Offset: 0x00013C6E
		public static void CopyCount(ComputeBuffer src, ComputeBuffer dst, int dstOffsetBytes)
		{
			GraphicsBuffer.CopyCountCC(src, dst, dstOffsetBytes);
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x00015A7A File Offset: 0x00013C7A
		public static void CopyCount(GraphicsBuffer src, ComputeBuffer dst, int dstOffsetBytes)
		{
			GraphicsBuffer.CopyCountGC(src, dst, dstOffsetBytes);
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x00015A86 File Offset: 0x00013C86
		public static void CopyCount(ComputeBuffer src, GraphicsBuffer dst, int dstOffsetBytes)
		{
			GraphicsBuffer.CopyCountCG(src, dst, dstOffsetBytes);
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x00015A92 File Offset: 0x00013C92
		public static void CopyCount(GraphicsBuffer src, GraphicsBuffer dst, int dstOffsetBytes)
		{
			GraphicsBuffer.CopyCountGG(src, dst, dstOffsetBytes);
		}

		// Token: 0x06000F47 RID: 3911
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_bufferHandle_Injected(out GraphicsBufferHandle ret);

		// Token: 0x0400047C RID: 1148
		internal IntPtr m_Ptr;

		// Token: 0x0200016C RID: 364
		[Flags]
		public enum Target
		{
			// Token: 0x0400047E RID: 1150
			Vertex = 1,
			// Token: 0x0400047F RID: 1151
			Index = 2,
			// Token: 0x04000480 RID: 1152
			CopySource = 4,
			// Token: 0x04000481 RID: 1153
			CopyDestination = 8,
			// Token: 0x04000482 RID: 1154
			Structured = 16,
			// Token: 0x04000483 RID: 1155
			Raw = 32,
			// Token: 0x04000484 RID: 1156
			Append = 64,
			// Token: 0x04000485 RID: 1157
			Counter = 128,
			// Token: 0x04000486 RID: 1158
			IndirectArguments = 256,
			// Token: 0x04000487 RID: 1159
			Constant = 512
		}

		// Token: 0x0200016D RID: 365
		[Flags]
		public enum UsageFlags
		{
			// Token: 0x04000489 RID: 1161
			None = 0,
			// Token: 0x0400048A RID: 1162
			LockBufferForWrite = 1
		}

		// Token: 0x0200016E RID: 366
		public struct IndirectDrawArgs
		{
			// Token: 0x17000306 RID: 774
			// (get) Token: 0x06000F48 RID: 3912 RVA: 0x00015A9E File Offset: 0x00013C9E
			// (set) Token: 0x06000F49 RID: 3913 RVA: 0x00015AA6 File Offset: 0x00013CA6
			public uint vertexCountPerInstance { readonly get; set; }

			// Token: 0x17000307 RID: 775
			// (get) Token: 0x06000F4A RID: 3914 RVA: 0x00015AAF File Offset: 0x00013CAF
			// (set) Token: 0x06000F4B RID: 3915 RVA: 0x00015AB7 File Offset: 0x00013CB7
			public uint instanceCount { readonly get; set; }

			// Token: 0x17000308 RID: 776
			// (get) Token: 0x06000F4C RID: 3916 RVA: 0x00015AC0 File Offset: 0x00013CC0
			// (set) Token: 0x06000F4D RID: 3917 RVA: 0x00015AC8 File Offset: 0x00013CC8
			public uint startVertex { readonly get; set; }

			// Token: 0x17000309 RID: 777
			// (get) Token: 0x06000F4E RID: 3918 RVA: 0x00015AD1 File Offset: 0x00013CD1
			// (set) Token: 0x06000F4F RID: 3919 RVA: 0x00015AD9 File Offset: 0x00013CD9
			public uint startInstance { readonly get; set; }

			// Token: 0x0400048B RID: 1163
			public const int size = 16;
		}

		// Token: 0x0200016F RID: 367
		public struct IndirectDrawIndexedArgs
		{
			// Token: 0x1700030A RID: 778
			// (get) Token: 0x06000F50 RID: 3920 RVA: 0x00015AE2 File Offset: 0x00013CE2
			// (set) Token: 0x06000F51 RID: 3921 RVA: 0x00015AEA File Offset: 0x00013CEA
			public uint indexCountPerInstance { readonly get; set; }

			// Token: 0x1700030B RID: 779
			// (get) Token: 0x06000F52 RID: 3922 RVA: 0x00015AF3 File Offset: 0x00013CF3
			// (set) Token: 0x06000F53 RID: 3923 RVA: 0x00015AFB File Offset: 0x00013CFB
			public uint instanceCount { readonly get; set; }

			// Token: 0x1700030C RID: 780
			// (get) Token: 0x06000F54 RID: 3924 RVA: 0x00015B04 File Offset: 0x00013D04
			// (set) Token: 0x06000F55 RID: 3925 RVA: 0x00015B0C File Offset: 0x00013D0C
			public uint startIndex { readonly get; set; }

			// Token: 0x1700030D RID: 781
			// (get) Token: 0x06000F56 RID: 3926 RVA: 0x00015B15 File Offset: 0x00013D15
			// (set) Token: 0x06000F57 RID: 3927 RVA: 0x00015B1D File Offset: 0x00013D1D
			public uint baseVertexIndex { readonly get; set; }

			// Token: 0x1700030E RID: 782
			// (get) Token: 0x06000F58 RID: 3928 RVA: 0x00015B26 File Offset: 0x00013D26
			// (set) Token: 0x06000F59 RID: 3929 RVA: 0x00015B2E File Offset: 0x00013D2E
			public uint startInstance { readonly get; set; }

			// Token: 0x04000490 RID: 1168
			public const int size = 20;
		}
	}
}
