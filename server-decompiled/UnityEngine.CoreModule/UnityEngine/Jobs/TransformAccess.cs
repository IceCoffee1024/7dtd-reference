using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Jobs
{
	// Token: 0x020002C3 RID: 707
	[NativeHeader("Runtime/Transform/ScriptBindings/TransformAccess.bindings.h")]
	public struct TransformAccess
	{
		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06001E1E RID: 7710 RVA: 0x00031C40 File Offset: 0x0002FE40
		// (set) Token: 0x06001E1F RID: 7711 RVA: 0x00031C5C File Offset: 0x0002FE5C
		public Vector3 position
		{
			get
			{
				Vector3 vector;
				TransformAccess.GetPosition(ref this, out vector);
				return vector;
			}
			set
			{
				TransformAccess.SetPosition(ref this, ref value);
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06001E20 RID: 7712 RVA: 0x00031C68 File Offset: 0x0002FE68
		// (set) Token: 0x06001E21 RID: 7713 RVA: 0x00031C84 File Offset: 0x0002FE84
		public Quaternion rotation
		{
			get
			{
				Quaternion quaternion;
				TransformAccess.GetRotation(ref this, out quaternion);
				return quaternion;
			}
			set
			{
				TransformAccess.SetRotation(ref this, ref value);
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06001E22 RID: 7714 RVA: 0x00031C90 File Offset: 0x0002FE90
		// (set) Token: 0x06001E23 RID: 7715 RVA: 0x00031CAC File Offset: 0x0002FEAC
		public Vector3 localPosition
		{
			get
			{
				Vector3 vector;
				TransformAccess.GetLocalPosition(ref this, out vector);
				return vector;
			}
			set
			{
				TransformAccess.SetLocalPosition(ref this, ref value);
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001E24 RID: 7716 RVA: 0x00031CB8 File Offset: 0x0002FEB8
		// (set) Token: 0x06001E25 RID: 7717 RVA: 0x00031CD4 File Offset: 0x0002FED4
		public Quaternion localRotation
		{
			get
			{
				Quaternion quaternion;
				TransformAccess.GetLocalRotation(ref this, out quaternion);
				return quaternion;
			}
			set
			{
				TransformAccess.SetLocalRotation(ref this, ref value);
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001E26 RID: 7718 RVA: 0x00031CE0 File Offset: 0x0002FEE0
		// (set) Token: 0x06001E27 RID: 7719 RVA: 0x00031CFC File Offset: 0x0002FEFC
		public Vector3 localScale
		{
			get
			{
				Vector3 vector;
				TransformAccess.GetLocalScale(ref this, out vector);
				return vector;
			}
			set
			{
				TransformAccess.SetLocalScale(ref this, ref value);
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06001E28 RID: 7720 RVA: 0x00031D08 File Offset: 0x0002FF08
		public Matrix4x4 localToWorldMatrix
		{
			get
			{
				Matrix4x4 matrix4x;
				TransformAccess.GetLocalToWorldMatrix(ref this, out matrix4x);
				return matrix4x;
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06001E29 RID: 7721 RVA: 0x00031D24 File Offset: 0x0002FF24
		public Matrix4x4 worldToLocalMatrix
		{
			get
			{
				Matrix4x4 matrix4x;
				TransformAccess.GetWorldToLocalMatrix(ref this, out matrix4x);
				return matrix4x;
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06001E2A RID: 7722 RVA: 0x00031D40 File Offset: 0x0002FF40
		public bool isValid
		{
			get
			{
				return this.hierarchy != IntPtr.Zero;
			}
		}

		// Token: 0x06001E2B RID: 7723 RVA: 0x00031D52 File Offset: 0x0002FF52
		public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
		{
			TransformAccess.SetPositionAndRotation_Internal(ref this, ref position, ref rotation);
		}

		// Token: 0x06001E2C RID: 7724 RVA: 0x00031D60 File Offset: 0x0002FF60
		public void SetLocalPositionAndRotation(Vector3 localPosition, Quaternion localRotation)
		{
			TransformAccess.SetLocalPositionAndRotation_Internal(ref this, ref localPosition, ref localRotation);
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x00031D6E File Offset: 0x0002FF6E
		public void GetPositionAndRotation(out Vector3 position, out Quaternion rotation)
		{
			TransformAccess.GetPositionAndRotation_Internal(ref this, out position, out rotation);
		}

		// Token: 0x06001E2E RID: 7726 RVA: 0x00031D7A File Offset: 0x0002FF7A
		public void GetLocalPositionAndRotation(out Vector3 localPosition, out Quaternion localRotation)
		{
			TransformAccess.GetLocalPositionAndRotation_Internal(ref this, out localPosition, out localRotation);
		}

		// Token: 0x06001E2F RID: 7727
		[NativeMethod(Name = "TransformAccessBindings::SetPositionAndRotation", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPositionAndRotation_Internal(ref TransformAccess access, ref Vector3 position, ref Quaternion rotation);

		// Token: 0x06001E30 RID: 7728
		[NativeMethod(Name = "TransformAccessBindings::SetLocalPositionAndRotation", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetLocalPositionAndRotation_Internal(ref TransformAccess access, ref Vector3 localPosition, ref Quaternion localRotation);

		// Token: 0x06001E31 RID: 7729
		[NativeMethod(Name = "TransformAccessBindings::GetPositionAndRotation", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetPositionAndRotation_Internal(ref TransformAccess access, out Vector3 position, out Quaternion rotation);

		// Token: 0x06001E32 RID: 7730
		[NativeMethod(Name = "TransformAccessBindings::GetLocalPositionAndRotation", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetLocalPositionAndRotation_Internal(ref TransformAccess access, out Vector3 localPosition, out Quaternion localRotation);

		// Token: 0x06001E33 RID: 7731
		[NativeMethod(Name = "TransformAccessBindings::GetPosition", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetPosition(ref TransformAccess access, out Vector3 p);

		// Token: 0x06001E34 RID: 7732
		[NativeMethod(Name = "TransformAccessBindings::SetPosition", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPosition(ref TransformAccess access, ref Vector3 p);

		// Token: 0x06001E35 RID: 7733
		[NativeMethod(Name = "TransformAccessBindings::GetRotation", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetRotation(ref TransformAccess access, out Quaternion r);

		// Token: 0x06001E36 RID: 7734
		[NativeMethod(Name = "TransformAccessBindings::SetRotation", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetRotation(ref TransformAccess access, ref Quaternion r);

		// Token: 0x06001E37 RID: 7735
		[NativeMethod(Name = "TransformAccessBindings::GetLocalPosition", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetLocalPosition(ref TransformAccess access, out Vector3 p);

		// Token: 0x06001E38 RID: 7736
		[NativeMethod(Name = "TransformAccessBindings::SetLocalPosition", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetLocalPosition(ref TransformAccess access, ref Vector3 p);

		// Token: 0x06001E39 RID: 7737
		[NativeMethod(Name = "TransformAccessBindings::GetLocalRotation", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetLocalRotation(ref TransformAccess access, out Quaternion r);

		// Token: 0x06001E3A RID: 7738
		[NativeMethod(Name = "TransformAccessBindings::SetLocalRotation", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetLocalRotation(ref TransformAccess access, ref Quaternion r);

		// Token: 0x06001E3B RID: 7739
		[NativeMethod(Name = "TransformAccessBindings::GetLocalScale", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetLocalScale(ref TransformAccess access, out Vector3 r);

		// Token: 0x06001E3C RID: 7740
		[NativeMethod(Name = "TransformAccessBindings::SetLocalScale", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetLocalScale(ref TransformAccess access, ref Vector3 r);

		// Token: 0x06001E3D RID: 7741
		[NativeMethod(Name = "TransformAccessBindings::GetLocalToWorldMatrix", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetLocalToWorldMatrix(ref TransformAccess access, out Matrix4x4 m);

		// Token: 0x06001E3E RID: 7742
		[NativeMethod(Name = "TransformAccessBindings::GetWorldToLocalMatrix", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetWorldToLocalMatrix(ref TransformAccess access, out Matrix4x4 m);

		// Token: 0x06001E3F RID: 7743 RVA: 0x00031D88 File Offset: 0x0002FF88
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void CheckHierarchyValid()
		{
			bool flag = !this.isValid;
			if (flag)
			{
				throw new NullReferenceException("The TransformAccess is not valid and points to an invalid hierarchy");
			}
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x00002669 File Offset: 0x00000869
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void MarkReadWrite()
		{
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x00002669 File Offset: 0x00000869
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void MarkReadOnly()
		{
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x00002669 File Offset: 0x00000869
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CheckWriteAccess()
		{
		}

		// Token: 0x040009F9 RID: 2553
		private IntPtr hierarchy;

		// Token: 0x040009FA RID: 2554
		private int index;
	}
}
