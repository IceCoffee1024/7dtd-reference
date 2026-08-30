using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;

namespace UnityEngine.Jobs
{
	// Token: 0x020002C4 RID: 708
	[NativeType(Header = "Runtime/Transform/ScriptBindings/TransformAccess.bindings.h", CodegenOptions = CodegenOptions.Custom)]
	public struct TransformAccessArray : IDisposable
	{
		// Token: 0x06001E43 RID: 7747 RVA: 0x00031DAE File Offset: 0x0002FFAE
		public TransformAccessArray(Transform[] transforms, int desiredJobCount = -1)
		{
			TransformAccessArray.Allocate(transforms.Length, desiredJobCount, out this);
			TransformAccessArray.SetTransforms(this.m_TransformArray, transforms);
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x00031DC9 File Offset: 0x0002FFC9
		public TransformAccessArray(int capacity, int desiredJobCount = -1)
		{
			TransformAccessArray.Allocate(capacity, desiredJobCount, out this);
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x00031DD5 File Offset: 0x0002FFD5
		public static void Allocate(int capacity, int desiredJobCount, out TransformAccessArray array)
		{
			array.m_TransformArray = TransformAccessArray.Create(capacity, desiredJobCount);
			UnsafeUtility.LeakRecord(array.m_TransformArray, LeakCategory.TransformAccessArray, 0);
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06001E46 RID: 7750 RVA: 0x00031DF4 File Offset: 0x0002FFF4
		public bool isCreated
		{
			get
			{
				return this.m_TransformArray != IntPtr.Zero;
			}
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x00031E16 File Offset: 0x00030016
		public void Dispose()
		{
			UnsafeUtility.LeakErase(this.m_TransformArray, LeakCategory.TransformAccessArray);
			TransformAccessArray.DestroyTransformAccessArray(this.m_TransformArray);
			this.m_TransformArray = IntPtr.Zero;
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x00031E40 File Offset: 0x00030040
		internal IntPtr GetTransformAccessArrayForSchedule()
		{
			return this.m_TransformArray;
		}

		// Token: 0x170005F9 RID: 1529
		public Transform this[int index]
		{
			get
			{
				return TransformAccessArray.GetTransform(this.m_TransformArray, index);
			}
			set
			{
				TransformAccessArray.SetTransform(this.m_TransformArray, index, value);
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06001E4B RID: 7755 RVA: 0x00031E88 File Offset: 0x00030088
		// (set) Token: 0x06001E4C RID: 7756 RVA: 0x00031EA5 File Offset: 0x000300A5
		public int capacity
		{
			get
			{
				return TransformAccessArray.GetCapacity(this.m_TransformArray);
			}
			set
			{
				TransformAccessArray.SetCapacity(this.m_TransformArray, value);
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001E4D RID: 7757 RVA: 0x00031EB8 File Offset: 0x000300B8
		public int length
		{
			get
			{
				return TransformAccessArray.GetLength(this.m_TransformArray);
			}
		}

		// Token: 0x06001E4E RID: 7758 RVA: 0x00031ED5 File Offset: 0x000300D5
		public void Add(Transform transform)
		{
			TransformAccessArray.Add(this.m_TransformArray, transform);
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x00031EE5 File Offset: 0x000300E5
		public void Add(int instanceId)
		{
			TransformAccessArray.AddInstanceId(this.m_TransformArray, instanceId);
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x00031EF5 File Offset: 0x000300F5
		public void RemoveAtSwapBack(int index)
		{
			TransformAccessArray.RemoveAtSwapBack(this.m_TransformArray, index);
		}

		// Token: 0x06001E51 RID: 7761 RVA: 0x00031F05 File Offset: 0x00030105
		public void SetTransforms(Transform[] transforms)
		{
			TransformAccessArray.SetTransforms(this.m_TransformArray, transforms);
		}

		// Token: 0x06001E52 RID: 7762
		[NativeMethod(Name = "TransformAccessArrayBindings::Create", IsFreeFunction = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Create(int capacity, int desiredJobCount);

		// Token: 0x06001E53 RID: 7763
		[NativeMethod(Name = "DestroyTransformAccessArray", IsFreeFunction = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DestroyTransformAccessArray(IntPtr transformArray);

		// Token: 0x06001E54 RID: 7764
		[NativeMethod(Name = "TransformAccessArrayBindings::SetTransforms", IsFreeFunction = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetTransforms(IntPtr transformArrayIntPtr, Transform[] transforms);

		// Token: 0x06001E55 RID: 7765
		[NativeMethod(Name = "TransformAccessArrayBindings::AddTransform", IsFreeFunction = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Add(IntPtr transformArrayIntPtr, Transform transform);

		// Token: 0x06001E56 RID: 7766
		[NativeMethod(Name = "TransformAccessArrayBindings::AddTransformInstanceId", IsFreeFunction = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddInstanceId(IntPtr transformArrayIntPtr, int instanceId);

		// Token: 0x06001E57 RID: 7767
		[NativeMethod(Name = "TransformAccessArrayBindings::RemoveAtSwapBack", IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemoveAtSwapBack(IntPtr transformArrayIntPtr, int index);

		// Token: 0x06001E58 RID: 7768
		[NativeMethod(Name = "TransformAccessArrayBindings::GetSortedTransformAccess", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr GetSortedTransformAccess(IntPtr transformArrayIntPtr);

		// Token: 0x06001E59 RID: 7769
		[NativeMethod(Name = "TransformAccessArrayBindings::GetSortedToUserIndex", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr GetSortedToUserIndex(IntPtr transformArrayIntPtr);

		// Token: 0x06001E5A RID: 7770
		[NativeMethod(Name = "TransformAccessArrayBindings::GetLength", IsFreeFunction = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetLength(IntPtr transformArrayIntPtr);

		// Token: 0x06001E5B RID: 7771
		[NativeMethod(Name = "TransformAccessArrayBindings::GetCapacity", IsFreeFunction = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetCapacity(IntPtr transformArrayIntPtr);

		// Token: 0x06001E5C RID: 7772
		[NativeMethod(Name = "TransformAccessArrayBindings::SetCapacity", IsFreeFunction = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetCapacity(IntPtr transformArrayIntPtr, int capacity);

		// Token: 0x06001E5D RID: 7773
		[NativeMethod(Name = "TransformAccessArrayBindings::GetTransform", IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Transform GetTransform(IntPtr transformArrayIntPtr, int index);

		// Token: 0x06001E5E RID: 7774
		[NativeMethod(Name = "TransformAccessArrayBindings::SetTransform", IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetTransform(IntPtr transformArrayIntPtr, int index, Transform transform);

		// Token: 0x040009FB RID: 2555
		private IntPtr m_TransformArray;
	}
}
