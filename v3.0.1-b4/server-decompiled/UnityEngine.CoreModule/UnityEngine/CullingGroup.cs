using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000111 RID: 273
	[NativeHeader("Runtime/Export/Camera/CullingGroup.bindings.h")]
	[StructLayout(LayoutKind.Sequential)]
	public class CullingGroup : IDisposable
	{
		// Token: 0x06000655 RID: 1621 RVA: 0x0000912F File Offset: 0x0000732F
		public CullingGroup()
		{
			this.m_Ptr = CullingGroup.Init(this);
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0000914C File Offset: 0x0000734C
		protected override void Finalize()
		{
			try
			{
				bool flag = this.m_Ptr != IntPtr.Zero;
				if (flag)
				{
					this.FinalizerFailure();
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x06000657 RID: 1623
		[FreeFunction("CullingGroup_Bindings::Dispose", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void DisposeInternal();

		// Token: 0x06000658 RID: 1624 RVA: 0x00009194 File Offset: 0x00007394
		public void Dispose()
		{
			this.DisposeInternal();
			this.m_Ptr = IntPtr.Zero;
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x000091AC File Offset: 0x000073AC
		// (set) Token: 0x0600065A RID: 1626 RVA: 0x000091C4 File Offset: 0x000073C4
		public CullingGroup.StateChanged onStateChanged
		{
			get
			{
				return this.m_OnStateChanged;
			}
			set
			{
				this.m_OnStateChanged = value;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x0600065B RID: 1627
		// (set) Token: 0x0600065C RID: 1628
		public extern bool enabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x0600065D RID: 1629
		// (set) Token: 0x0600065E RID: 1630
		public extern Camera targetCamera
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x0600065F RID: 1631
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetBoundingSpheres([Unmarshalled] BoundingSphere[] array);

		// Token: 0x06000660 RID: 1632
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetBoundingSphereCount(int count);

		// Token: 0x06000661 RID: 1633
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void EraseSwapBack(int index);

		// Token: 0x06000662 RID: 1634 RVA: 0x000091CE File Offset: 0x000073CE
		public static void EraseSwapBack<T>(int index, T[] myArray, ref int size)
		{
			size--;
			myArray[index] = myArray[size];
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x000091E8 File Offset: 0x000073E8
		public int QueryIndices(bool visible, int[] result, int firstIndex)
		{
			return this.QueryIndices(visible, -1, CullingQueryOptions.IgnoreDistance, result, firstIndex);
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00009208 File Offset: 0x00007408
		public int QueryIndices(int distanceIndex, int[] result, int firstIndex)
		{
			return this.QueryIndices(false, distanceIndex, CullingQueryOptions.IgnoreVisibility, result, firstIndex);
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00009228 File Offset: 0x00007428
		public int QueryIndices(bool visible, int distanceIndex, int[] result, int firstIndex)
		{
			return this.QueryIndices(visible, distanceIndex, CullingQueryOptions.Normal, result, firstIndex);
		}

		// Token: 0x06000666 RID: 1638
		[NativeThrows]
		[FreeFunction("CullingGroup_Bindings::QueryIndices", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int QueryIndices(bool visible, int distanceIndex, CullingQueryOptions options, [Unmarshalled] int[] result, int firstIndex);

		// Token: 0x06000667 RID: 1639
		[NativeThrows]
		[FreeFunction("CullingGroup_Bindings::IsVisible", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsVisible(int index);

		// Token: 0x06000668 RID: 1640
		[NativeThrows]
		[FreeFunction("CullingGroup_Bindings::GetDistance", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetDistance(int index);

		// Token: 0x06000669 RID: 1641
		[FreeFunction("CullingGroup_Bindings::SetBoundingDistances", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetBoundingDistances([Unmarshalled] float[] distances);

		// Token: 0x0600066A RID: 1642 RVA: 0x00009246 File Offset: 0x00007446
		[FreeFunction("CullingGroup_Bindings::SetDistanceReferencePoint", HasExplicitThis = true)]
		private void SetDistanceReferencePoint_InternalVector3(Vector3 point)
		{
			this.SetDistanceReferencePoint_InternalVector3_Injected(ref point);
		}

		// Token: 0x0600066B RID: 1643
		[NativeMethod("SetDistanceReferenceTransform")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetDistanceReferencePoint_InternalTransform(Transform transform);

		// Token: 0x0600066C RID: 1644 RVA: 0x00009250 File Offset: 0x00007450
		public void SetDistanceReferencePoint(Vector3 point)
		{
			this.SetDistanceReferencePoint_InternalVector3(point);
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0000925B File Offset: 0x0000745B
		public void SetDistanceReferencePoint(Transform transform)
		{
			this.SetDistanceReferencePoint_InternalTransform(transform);
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00009268 File Offset: 0x00007468
		[RequiredByNativeCode]
		[SecuritySafeCritical]
		private unsafe static void SendEvents(CullingGroup cullingGroup, IntPtr eventsPtr, int count)
		{
			CullingGroupEvent* ptr = (CullingGroupEvent*)eventsPtr.ToPointer();
			bool flag = cullingGroup.m_OnStateChanged == null;
			if (!flag)
			{
				for (int i = 0; i < count; i++)
				{
					cullingGroup.m_OnStateChanged(ptr[i]);
				}
			}
		}

		// Token: 0x0600066F RID: 1647
		[FreeFunction("CullingGroup_Bindings::Init")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Init(object scripting);

		// Token: 0x06000670 RID: 1648
		[FreeFunction("CullingGroup_Bindings::FinalizerFailure", HasExplicitThis = true, IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void FinalizerFailure();

		// Token: 0x06000671 RID: 1649
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetDistanceReferencePoint_InternalVector3_Injected(ref Vector3 point);

		// Token: 0x04000392 RID: 914
		internal IntPtr m_Ptr;

		// Token: 0x04000393 RID: 915
		private CullingGroup.StateChanged m_OnStateChanged = null;

		// Token: 0x02000112 RID: 274
		// (Invoke) Token: 0x06000673 RID: 1651
		public delegate void StateChanged(CullingGroupEvent sphere);
	}
}
