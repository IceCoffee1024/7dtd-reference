using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020002A1 RID: 673
	[NativeHeader("Configuration/UnityConfigure.h")]
	[NativeHeader("Runtime/Transform/Transform.h")]
	[NativeHeader("Runtime/Transform/ScriptBindings/TransformScriptBindings.h")]
	[RequiredByNativeCode]
	public class Transform : Component, IEnumerable
	{
		// Token: 0x06001C63 RID: 7267 RVA: 0x0002F38E File Offset: 0x0002D58E
		protected Transform()
		{
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001C64 RID: 7268 RVA: 0x0002F398 File Offset: 0x0002D598
		// (set) Token: 0x06001C65 RID: 7269 RVA: 0x0002F3AE File Offset: 0x0002D5AE
		public Vector3 position
		{
			get
			{
				Vector3 vector;
				this.get_position_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_position_Injected(ref value);
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001C66 RID: 7270 RVA: 0x0002F3B8 File Offset: 0x0002D5B8
		// (set) Token: 0x06001C67 RID: 7271 RVA: 0x0002F3CE File Offset: 0x0002D5CE
		public Vector3 localPosition
		{
			get
			{
				Vector3 vector;
				this.get_localPosition_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_localPosition_Injected(ref value);
			}
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x0002F3D8 File Offset: 0x0002D5D8
		internal Vector3 GetLocalEulerAngles(RotationOrder order)
		{
			Vector3 vector;
			this.GetLocalEulerAngles_Injected(order, out vector);
			return vector;
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x0002F3EF File Offset: 0x0002D5EF
		internal void SetLocalEulerAngles(Vector3 euler, RotationOrder order)
		{
			this.SetLocalEulerAngles_Injected(ref euler, order);
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x0002F3FA File Offset: 0x0002D5FA
		[NativeConditional("UNITY_EDITOR")]
		internal void SetLocalEulerHint(Vector3 euler)
		{
			this.SetLocalEulerHint_Injected(ref euler);
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06001C6B RID: 7275 RVA: 0x0002F404 File Offset: 0x0002D604
		// (set) Token: 0x06001C6C RID: 7276 RVA: 0x0002F424 File Offset: 0x0002D624
		public Vector3 eulerAngles
		{
			get
			{
				return this.rotation.eulerAngles;
			}
			set
			{
				this.rotation = Quaternion.Euler(value);
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06001C6D RID: 7277 RVA: 0x0002F434 File Offset: 0x0002D634
		// (set) Token: 0x06001C6E RID: 7278 RVA: 0x0002F454 File Offset: 0x0002D654
		public Vector3 localEulerAngles
		{
			get
			{
				return this.localRotation.eulerAngles;
			}
			set
			{
				this.localRotation = Quaternion.Euler(value);
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06001C6F RID: 7279 RVA: 0x0002F464 File Offset: 0x0002D664
		// (set) Token: 0x06001C70 RID: 7280 RVA: 0x0002F486 File Offset: 0x0002D686
		public Vector3 right
		{
			get
			{
				return this.rotation * Vector3.right;
			}
			set
			{
				this.rotation = Quaternion.FromToRotation(Vector3.right, value);
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06001C71 RID: 7281 RVA: 0x0002F49C File Offset: 0x0002D69C
		// (set) Token: 0x06001C72 RID: 7282 RVA: 0x0002F4BE File Offset: 0x0002D6BE
		public Vector3 up
		{
			get
			{
				return this.rotation * Vector3.up;
			}
			set
			{
				this.rotation = Quaternion.FromToRotation(Vector3.up, value);
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06001C73 RID: 7283 RVA: 0x0002F4D4 File Offset: 0x0002D6D4
		// (set) Token: 0x06001C74 RID: 7284 RVA: 0x0002F4F6 File Offset: 0x0002D6F6
		public Vector3 forward
		{
			get
			{
				return this.rotation * Vector3.forward;
			}
			set
			{
				this.rotation = Quaternion.LookRotation(value);
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001C75 RID: 7285 RVA: 0x0002F508 File Offset: 0x0002D708
		// (set) Token: 0x06001C76 RID: 7286 RVA: 0x0002F51E File Offset: 0x0002D71E
		public Quaternion rotation
		{
			get
			{
				Quaternion quaternion;
				this.get_rotation_Injected(out quaternion);
				return quaternion;
			}
			set
			{
				this.set_rotation_Injected(ref value);
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001C77 RID: 7287 RVA: 0x0002F528 File Offset: 0x0002D728
		// (set) Token: 0x06001C78 RID: 7288 RVA: 0x0002F53E File Offset: 0x0002D73E
		public Quaternion localRotation
		{
			get
			{
				Quaternion quaternion;
				this.get_localRotation_Injected(out quaternion);
				return quaternion;
			}
			set
			{
				this.set_localRotation_Injected(ref value);
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06001C79 RID: 7289 RVA: 0x0002F548 File Offset: 0x0002D748
		// (set) Token: 0x06001C7A RID: 7290 RVA: 0x0002F560 File Offset: 0x0002D760
		[NativeConditional("UNITY_EDITOR")]
		internal RotationOrder rotationOrder
		{
			get
			{
				return (RotationOrder)this.GetRotationOrderInternal();
			}
			set
			{
				this.SetRotationOrderInternal(value);
			}
		}

		// Token: 0x06001C7B RID: 7291
		[NativeConditional("UNITY_EDITOR")]
		[NativeMethod("GetRotationOrder")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetRotationOrderInternal();

		// Token: 0x06001C7C RID: 7292
		[NativeMethod("SetRotationOrder")]
		[NativeConditional("UNITY_EDITOR")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetRotationOrderInternal(RotationOrder rotationOrder);

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06001C7D RID: 7293 RVA: 0x0002F56C File Offset: 0x0002D76C
		// (set) Token: 0x06001C7E RID: 7294 RVA: 0x0002F582 File Offset: 0x0002D782
		public Vector3 localScale
		{
			get
			{
				Vector3 vector;
				this.get_localScale_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_localScale_Injected(ref value);
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06001C7F RID: 7295 RVA: 0x0002F58C File Offset: 0x0002D78C
		// (set) Token: 0x06001C80 RID: 7296 RVA: 0x0002F5A4 File Offset: 0x0002D7A4
		public Transform parent
		{
			get
			{
				return this.parentInternal;
			}
			set
			{
				bool flag = this is RectTransform;
				if (flag)
				{
					Debug.LogWarning("Parent of RectTransform is being set with parent property. Consider using the SetParent method instead, with the worldPositionStays argument set to false. This will retain local orientation and scale rather than world orientation and scale, which can prevent common UI scaling issues.", this);
				}
				this.parentInternal = value;
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06001C81 RID: 7297 RVA: 0x0002F5D4 File Offset: 0x0002D7D4
		// (set) Token: 0x06001C82 RID: 7298 RVA: 0x0002F5EC File Offset: 0x0002D7EC
		internal Transform parentInternal
		{
			get
			{
				return this.GetParent();
			}
			set
			{
				this.SetParent(value);
			}
		}

		// Token: 0x06001C83 RID: 7299
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Transform GetParent();

		// Token: 0x06001C84 RID: 7300 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public void SetParent(Transform p)
		{
			this.SetParent(p, true);
		}

		// Token: 0x06001C85 RID: 7301
		[FreeFunction("SetParent", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetParent(Transform parent, bool worldPositionStays);

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06001C86 RID: 7302 RVA: 0x0002F604 File Offset: 0x0002D804
		public Matrix4x4 worldToLocalMatrix
		{
			get
			{
				Matrix4x4 matrix4x;
				this.get_worldToLocalMatrix_Injected(out matrix4x);
				return matrix4x;
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001C87 RID: 7303 RVA: 0x0002F61C File Offset: 0x0002D81C
		public Matrix4x4 localToWorldMatrix
		{
			get
			{
				Matrix4x4 matrix4x;
				this.get_localToWorldMatrix_Injected(out matrix4x);
				return matrix4x;
			}
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x0002F632 File Offset: 0x0002D832
		public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
		{
			this.SetPositionAndRotation_Injected(ref position, ref rotation);
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x0002F63E File Offset: 0x0002D83E
		public void SetLocalPositionAndRotation(Vector3 localPosition, Quaternion localRotation)
		{
			this.SetLocalPositionAndRotation_Injected(ref localPosition, ref localRotation);
		}

		// Token: 0x06001C8A RID: 7306
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GetPositionAndRotation(out Vector3 position, out Quaternion rotation);

		// Token: 0x06001C8B RID: 7307
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GetLocalPositionAndRotation(out Vector3 localPosition, out Quaternion localRotation);

		// Token: 0x06001C8C RID: 7308 RVA: 0x0002F64C File Offset: 0x0002D84C
		public void Translate(Vector3 translation, [DefaultValue("Space.Self")] Space relativeTo)
		{
			bool flag = relativeTo == Space.World;
			if (flag)
			{
				this.position += translation;
			}
			else
			{
				this.position += this.TransformDirection(translation);
			}
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x0002F690 File Offset: 0x0002D890
		public void Translate(Vector3 translation)
		{
			this.Translate(translation, Space.Self);
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x0002F69C File Offset: 0x0002D89C
		public void Translate(float x, float y, float z, [DefaultValue("Space.Self")] Space relativeTo)
		{
			this.Translate(new Vector3(x, y, z), relativeTo);
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x0002F6B0 File Offset: 0x0002D8B0
		public void Translate(float x, float y, float z)
		{
			this.Translate(new Vector3(x, y, z), Space.Self);
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x0002F6C4 File Offset: 0x0002D8C4
		public void Translate(Vector3 translation, Transform relativeTo)
		{
			bool flag = relativeTo;
			if (flag)
			{
				this.position += relativeTo.TransformDirection(translation);
			}
			else
			{
				this.position += translation;
			}
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x0002F70A File Offset: 0x0002D90A
		public void Translate(float x, float y, float z, Transform relativeTo)
		{
			this.Translate(new Vector3(x, y, z), relativeTo);
		}

		// Token: 0x06001C92 RID: 7314 RVA: 0x0002F720 File Offset: 0x0002D920
		public void Rotate(Vector3 eulers, [DefaultValue("Space.Self")] Space relativeTo)
		{
			Quaternion quaternion = Quaternion.Euler(eulers.x, eulers.y, eulers.z);
			bool flag = relativeTo == Space.Self;
			if (flag)
			{
				this.localRotation *= quaternion;
			}
			else
			{
				this.rotation *= Quaternion.Inverse(this.rotation) * quaternion * this.rotation;
			}
		}

		// Token: 0x06001C93 RID: 7315 RVA: 0x0002F793 File Offset: 0x0002D993
		public void Rotate(Vector3 eulers)
		{
			this.Rotate(eulers, Space.Self);
		}

		// Token: 0x06001C94 RID: 7316 RVA: 0x0002F79F File Offset: 0x0002D99F
		public void Rotate(float xAngle, float yAngle, float zAngle, [DefaultValue("Space.Self")] Space relativeTo)
		{
			this.Rotate(new Vector3(xAngle, yAngle, zAngle), relativeTo);
		}

		// Token: 0x06001C95 RID: 7317 RVA: 0x0002F7B3 File Offset: 0x0002D9B3
		public void Rotate(float xAngle, float yAngle, float zAngle)
		{
			this.Rotate(new Vector3(xAngle, yAngle, zAngle), Space.Self);
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x0002F7C6 File Offset: 0x0002D9C6
		[NativeMethod("RotateAround")]
		internal void RotateAroundInternal(Vector3 axis, float angle)
		{
			this.RotateAroundInternal_Injected(ref axis, angle);
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x0002F7D4 File Offset: 0x0002D9D4
		public void Rotate(Vector3 axis, float angle, [DefaultValue("Space.Self")] Space relativeTo)
		{
			bool flag = relativeTo == Space.Self;
			if (flag)
			{
				this.RotateAroundInternal(base.transform.TransformDirection(axis), angle * 0.017453292f);
			}
			else
			{
				this.RotateAroundInternal(axis, angle * 0.017453292f);
			}
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x0002F815 File Offset: 0x0002DA15
		public void Rotate(Vector3 axis, float angle)
		{
			this.Rotate(axis, angle, Space.Self);
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x0002F824 File Offset: 0x0002DA24
		public void RotateAround(Vector3 point, Vector3 axis, float angle)
		{
			Vector3 vector = this.position;
			Quaternion quaternion = Quaternion.AngleAxis(angle, axis);
			Vector3 vector2 = vector - point;
			vector2 = quaternion * vector2;
			vector = point + vector2;
			this.position = vector;
			this.RotateAroundInternal(axis, angle * 0.017453292f);
		}

		// Token: 0x06001C9A RID: 7322 RVA: 0x0002F870 File Offset: 0x0002DA70
		public void LookAt(Transform target, [DefaultValue("Vector3.up")] Vector3 worldUp)
		{
			bool flag = target;
			if (flag)
			{
				this.LookAt(target.position, worldUp);
			}
		}

		// Token: 0x06001C9B RID: 7323 RVA: 0x0002F898 File Offset: 0x0002DA98
		public void LookAt(Transform target)
		{
			bool flag = target;
			if (flag)
			{
				this.LookAt(target.position, Vector3.up);
			}
		}

		// Token: 0x06001C9C RID: 7324 RVA: 0x0002F8C2 File Offset: 0x0002DAC2
		public void LookAt(Vector3 worldPosition, [DefaultValue("Vector3.up")] Vector3 worldUp)
		{
			this.Internal_LookAt(worldPosition, worldUp);
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x0002F8CE File Offset: 0x0002DACE
		public void LookAt(Vector3 worldPosition)
		{
			this.Internal_LookAt(worldPosition, Vector3.up);
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x0002F8DE File Offset: 0x0002DADE
		[FreeFunction("Internal_LookAt", HasExplicitThis = true)]
		private void Internal_LookAt(Vector3 worldPosition, Vector3 worldUp)
		{
			this.Internal_LookAt_Injected(ref worldPosition, ref worldUp);
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x0002F8EC File Offset: 0x0002DAEC
		public Vector3 TransformDirection(Vector3 direction)
		{
			Vector3 vector;
			this.TransformDirection_Injected(ref direction, out vector);
			return vector;
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x0002F904 File Offset: 0x0002DB04
		public Vector3 TransformDirection(float x, float y, float z)
		{
			return this.TransformDirection(new Vector3(x, y, z));
		}

		// Token: 0x06001CA1 RID: 7329
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe extern void TransformDirections([Span("count", true)] Vector3* directions, int count, [Span("transformedCount", false)] Vector3* transformedDirections, int transformedCount);

		// Token: 0x06001CA2 RID: 7330 RVA: 0x0002F924 File Offset: 0x0002DB24
		public unsafe void TransformDirections(ReadOnlySpan<Vector3> directions, Span<Vector3> transformedDirections)
		{
			bool flag = directions.Length != transformedDirections.Length;
			if (flag)
			{
				throw new InvalidOperationException("Both spans passed to Transform.TransformDirections() must be the same length");
			}
			fixed (Vector3* pinnableReference = directions.GetPinnableReference())
			{
				Vector3* ptr = pinnableReference;
				fixed (Vector3* pinnableReference2 = transformedDirections.GetPinnableReference())
				{
					Vector3* ptr2 = pinnableReference2;
					this.TransformDirections(ptr, directions.Length, ptr2, transformedDirections.Length);
				}
			}
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x0002F98E File Offset: 0x0002DB8E
		public void TransformDirections(Span<Vector3> directions)
		{
			this.TransformDirections(directions, directions);
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x0002F9A0 File Offset: 0x0002DBA0
		public Vector3 InverseTransformDirection(Vector3 direction)
		{
			Vector3 vector;
			this.InverseTransformDirection_Injected(ref direction, out vector);
			return vector;
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x0002F9B8 File Offset: 0x0002DBB8
		public Vector3 InverseTransformDirection(float x, float y, float z)
		{
			return this.InverseTransformDirection(new Vector3(x, y, z));
		}

		// Token: 0x06001CA6 RID: 7334
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe extern void InverseTransformDirections([Span("count", true)] Vector3* directions, int count, [Span("transformedCount", false)] Vector3* transformedDirections, int transformedCount);

		// Token: 0x06001CA7 RID: 7335 RVA: 0x0002F9D8 File Offset: 0x0002DBD8
		public unsafe void InverseTransformDirections(ReadOnlySpan<Vector3> directions, Span<Vector3> transformedDirections)
		{
			bool flag = directions.Length != transformedDirections.Length;
			if (flag)
			{
				throw new InvalidOperationException("Both spans passed to Transform.InverseTransformDirections() must be the same length");
			}
			fixed (Vector3* pinnableReference = directions.GetPinnableReference())
			{
				Vector3* ptr = pinnableReference;
				fixed (Vector3* pinnableReference2 = transformedDirections.GetPinnableReference())
				{
					Vector3* ptr2 = pinnableReference2;
					this.InverseTransformDirections(ptr, directions.Length, ptr2, transformedDirections.Length);
				}
			}
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x0002FA42 File Offset: 0x0002DC42
		public void InverseTransformDirections(Span<Vector3> directions)
		{
			this.InverseTransformDirections(directions, directions);
		}

		// Token: 0x06001CA9 RID: 7337 RVA: 0x0002FA54 File Offset: 0x0002DC54
		public Vector3 TransformVector(Vector3 vector)
		{
			Vector3 vector2;
			this.TransformVector_Injected(ref vector, out vector2);
			return vector2;
		}

		// Token: 0x06001CAA RID: 7338 RVA: 0x0002FA6C File Offset: 0x0002DC6C
		public Vector3 TransformVector(float x, float y, float z)
		{
			return this.TransformVector(new Vector3(x, y, z));
		}

		// Token: 0x06001CAB RID: 7339
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe extern void TransformVectors([Span("count", true)] Vector3* vectors, int count, [Span("transformedCount", false)] Vector3* transformedVectors, int transformedCount);

		// Token: 0x06001CAC RID: 7340 RVA: 0x0002FA8C File Offset: 0x0002DC8C
		public unsafe void TransformVectors(ReadOnlySpan<Vector3> vectors, Span<Vector3> transformedVectors)
		{
			bool flag = vectors.Length != transformedVectors.Length;
			if (flag)
			{
				throw new InvalidOperationException("Both spans passed to Transform.TransformVectors() must be the same length");
			}
			fixed (Vector3* pinnableReference = vectors.GetPinnableReference())
			{
				Vector3* ptr = pinnableReference;
				fixed (Vector3* pinnableReference2 = transformedVectors.GetPinnableReference())
				{
					Vector3* ptr2 = pinnableReference2;
					this.TransformVectors(ptr, vectors.Length, ptr2, transformedVectors.Length);
				}
			}
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x0002FAF6 File Offset: 0x0002DCF6
		public void TransformVectors(Span<Vector3> vectors)
		{
			this.TransformVectors(vectors, vectors);
		}

		// Token: 0x06001CAE RID: 7342 RVA: 0x0002FB08 File Offset: 0x0002DD08
		public Vector3 InverseTransformVector(Vector3 vector)
		{
			Vector3 vector2;
			this.InverseTransformVector_Injected(ref vector, out vector2);
			return vector2;
		}

		// Token: 0x06001CAF RID: 7343 RVA: 0x0002FB20 File Offset: 0x0002DD20
		public Vector3 InverseTransformVector(float x, float y, float z)
		{
			return this.InverseTransformVector(new Vector3(x, y, z));
		}

		// Token: 0x06001CB0 RID: 7344
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe extern void InverseTransformVectors([Span("count", true)] Vector3* vectors, int count, [Span("transformedCount", false)] Vector3* transformedVectors, int transformedCount);

		// Token: 0x06001CB1 RID: 7345 RVA: 0x0002FB40 File Offset: 0x0002DD40
		public unsafe void InverseTransformVectors(ReadOnlySpan<Vector3> vectors, Span<Vector3> transformedVectors)
		{
			bool flag = vectors.Length != transformedVectors.Length;
			if (flag)
			{
				throw new InvalidOperationException("Both spans passed to Transform.InverseTransformVectors() must be the same length");
			}
			fixed (Vector3* pinnableReference = vectors.GetPinnableReference())
			{
				Vector3* ptr = pinnableReference;
				fixed (Vector3* pinnableReference2 = transformedVectors.GetPinnableReference())
				{
					Vector3* ptr2 = pinnableReference2;
					this.InverseTransformVectors(ptr, vectors.Length, ptr2, transformedVectors.Length);
				}
			}
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x0002FBAA File Offset: 0x0002DDAA
		public void InverseTransformVectors(Span<Vector3> vectors)
		{
			this.InverseTransformVectors(vectors, vectors);
		}

		// Token: 0x06001CB3 RID: 7347 RVA: 0x0002FBBC File Offset: 0x0002DDBC
		public Vector3 TransformPoint(Vector3 position)
		{
			Vector3 vector;
			this.TransformPoint_Injected(ref position, out vector);
			return vector;
		}

		// Token: 0x06001CB4 RID: 7348 RVA: 0x0002FBD4 File Offset: 0x0002DDD4
		public Vector3 TransformPoint(float x, float y, float z)
		{
			return this.TransformPoint(new Vector3(x, y, z));
		}

		// Token: 0x06001CB5 RID: 7349
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe extern void TransformPoints([Span("count", true)] Vector3* positions, int count, [Span("transformedCount", false)] Vector3* transformedPositions, int transformedCount);

		// Token: 0x06001CB6 RID: 7350 RVA: 0x0002FBF4 File Offset: 0x0002DDF4
		public unsafe void TransformPoints(ReadOnlySpan<Vector3> positions, Span<Vector3> transformedPositions)
		{
			bool flag = positions.Length != transformedPositions.Length;
			if (flag)
			{
				throw new InvalidOperationException("Both spans passed to Transform.TransformPoints() must be the same length");
			}
			fixed (Vector3* pinnableReference = positions.GetPinnableReference())
			{
				Vector3* ptr = pinnableReference;
				fixed (Vector3* pinnableReference2 = transformedPositions.GetPinnableReference())
				{
					Vector3* ptr2 = pinnableReference2;
					this.TransformPoints(ptr, positions.Length, ptr2, transformedPositions.Length);
				}
			}
		}

		// Token: 0x06001CB7 RID: 7351 RVA: 0x0002FC5E File Offset: 0x0002DE5E
		public void TransformPoints(Span<Vector3> positions)
		{
			this.TransformPoints(positions, positions);
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x0002FC70 File Offset: 0x0002DE70
		public Vector3 InverseTransformPoint(Vector3 position)
		{
			Vector3 vector;
			this.InverseTransformPoint_Injected(ref position, out vector);
			return vector;
		}

		// Token: 0x06001CB9 RID: 7353 RVA: 0x0002FC88 File Offset: 0x0002DE88
		public Vector3 InverseTransformPoint(float x, float y, float z)
		{
			return this.InverseTransformPoint(new Vector3(x, y, z));
		}

		// Token: 0x06001CBA RID: 7354
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe extern void InverseTransformPoints([Span("count", true)] Vector3* positions, int count, [Span("transformedCount", false)] Vector3* transformedPositions, int transformedCount);

		// Token: 0x06001CBB RID: 7355 RVA: 0x0002FCA8 File Offset: 0x0002DEA8
		public unsafe void InverseTransformPoints(ReadOnlySpan<Vector3> positions, Span<Vector3> transformedPositions)
		{
			bool flag = positions.Length != transformedPositions.Length;
			if (flag)
			{
				throw new InvalidOperationException("Both spans passed to Transform.InverseTransformPoints() must be the same length");
			}
			fixed (Vector3* pinnableReference = positions.GetPinnableReference())
			{
				Vector3* ptr = pinnableReference;
				fixed (Vector3* pinnableReference2 = transformedPositions.GetPinnableReference())
				{
					Vector3* ptr2 = pinnableReference2;
					this.InverseTransformPoints(ptr, positions.Length, ptr2, transformedPositions.Length);
				}
			}
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x0002FD12 File Offset: 0x0002DF12
		public void InverseTransformPoints(Span<Vector3> positions)
		{
			this.InverseTransformPoints(positions, positions);
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06001CBD RID: 7357 RVA: 0x0002FD24 File Offset: 0x0002DF24
		public Transform root
		{
			get
			{
				return this.GetRoot();
			}
		}

		// Token: 0x06001CBE RID: 7358
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Transform GetRoot();

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001CBF RID: 7359
		public extern int childCount
		{
			[NativeMethod("GetChildrenCount")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06001CC0 RID: 7360
		[FreeFunction("DetachChildren", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void DetachChildren();

		// Token: 0x06001CC1 RID: 7361
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetAsFirstSibling();

		// Token: 0x06001CC2 RID: 7362
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetAsLastSibling();

		// Token: 0x06001CC3 RID: 7363
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetSiblingIndex(int index);

		// Token: 0x06001CC4 RID: 7364
		[NativeMethod("MoveAfterSiblingInternal")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void MoveAfterSibling(Transform transform, bool notifyEditorAndMarkDirty);

		// Token: 0x06001CC5 RID: 7365
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetSiblingIndex();

		// Token: 0x06001CC6 RID: 7366
		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Transform FindRelativeTransformWithPath([NotNull("NullExceptionObject")] Transform transform, string path, [DefaultValue("false")] bool isActiveOnly);

		// Token: 0x06001CC7 RID: 7367 RVA: 0x0002FD3C File Offset: 0x0002DF3C
		public Transform Find(string n)
		{
			bool flag = n == null;
			if (flag)
			{
				throw new ArgumentNullException("Name cannot be null");
			}
			return Transform.FindRelativeTransformWithPath(this, n, false);
		}

		// Token: 0x06001CC8 RID: 7368
		[NativeConditional("UNITY_EDITOR")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SendTransformChangedScale();

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001CC9 RID: 7369 RVA: 0x0002FD6C File Offset: 0x0002DF6C
		public Vector3 lossyScale
		{
			[NativeMethod("GetWorldScaleLossy")]
			get
			{
				Vector3 vector;
				this.get_lossyScale_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x06001CCA RID: 7370
		[FreeFunction("Internal_IsChildOrSameTransform", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsChildOf([NotNull("ArgumentNullException")] Transform parent);

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001CCB RID: 7371
		// (set) Token: 0x06001CCC RID: 7372
		[NativeProperty("HasChangedDeprecated")]
		public extern bool hasChanged
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x0002FD84 File Offset: 0x0002DF84
		[Obsolete("FindChild has been deprecated. Use Find instead (UnityUpgradable) -> Find([mscorlib] System.String)", false)]
		public Transform FindChild(string n)
		{
			return this.Find(n);
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x0002FDA0 File Offset: 0x0002DFA0
		public IEnumerator GetEnumerator()
		{
			return new Transform.Enumerator(this);
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x0002FDB8 File Offset: 0x0002DFB8
		[Obsolete("warning use Transform.Rotate instead.")]
		public void RotateAround(Vector3 axis, float angle)
		{
			this.RotateAround_Injected(ref axis, angle);
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x0002FDC3 File Offset: 0x0002DFC3
		[Obsolete("warning use Transform.Rotate instead.")]
		public void RotateAroundLocal(Vector3 axis, float angle)
		{
			this.RotateAroundLocal_Injected(ref axis, angle);
		}

		// Token: 0x06001CD1 RID: 7377
		[FreeFunction("GetChild", HasExplicitThis = true)]
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Transform GetChild(int index);

		// Token: 0x06001CD2 RID: 7378
		[Obsolete("warning use Transform.childCount instead (UnityUpgradable) -> Transform.childCount", false)]
		[NativeMethod("GetChildrenCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetChildCount();

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001CD3 RID: 7379 RVA: 0x0002FDD0 File Offset: 0x0002DFD0
		// (set) Token: 0x06001CD4 RID: 7380 RVA: 0x0002FDE8 File Offset: 0x0002DFE8
		public int hierarchyCapacity
		{
			get
			{
				return this.internal_getHierarchyCapacity();
			}
			set
			{
				this.internal_setHierarchyCapacity(value);
			}
		}

		// Token: 0x06001CD5 RID: 7381
		[FreeFunction("GetHierarchyCapacity", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int internal_getHierarchyCapacity();

		// Token: 0x06001CD6 RID: 7382
		[FreeFunction("SetHierarchyCapacity", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void internal_setHierarchyCapacity(int value);

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001CD7 RID: 7383 RVA: 0x0002FDF4 File Offset: 0x0002DFF4
		public int hierarchyCount
		{
			get
			{
				return this.internal_getHierarchyCount();
			}
		}

		// Token: 0x06001CD8 RID: 7384
		[FreeFunction("GetHierarchyCount", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int internal_getHierarchyCount();

		// Token: 0x06001CD9 RID: 7385
		[FreeFunction("IsNonUniformScaleTransform", HasExplicitThis = true)]
		[NativeConditional("UNITY_EDITOR")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool IsNonUniformScaleTransform();

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001CDA RID: 7386 RVA: 0x0002FE0C File Offset: 0x0002E00C
		// (set) Token: 0x06001CDB RID: 7387 RVA: 0x0002FE14 File Offset: 0x0002E014
		[NativeConditional("UNITY_EDITOR")]
		internal bool constrainProportionsScale
		{
			get
			{
				return this.IsConstrainProportionsScale();
			}
			set
			{
				this.SetConstrainProportionsScale(value);
			}
		}

		// Token: 0x06001CDC RID: 7388
		[NativeConditional("UNITY_EDITOR")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetConstrainProportionsScale(bool isLinked);

		// Token: 0x06001CDD RID: 7389
		[NativeConditional("UNITY_EDITOR")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool IsConstrainProportionsScale();

		// Token: 0x06001CDE RID: 7390
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_position_Injected(out Vector3 ret);

		// Token: 0x06001CDF RID: 7391
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_position_Injected(ref Vector3 value);

		// Token: 0x06001CE0 RID: 7392
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_localPosition_Injected(out Vector3 ret);

		// Token: 0x06001CE1 RID: 7393
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_localPosition_Injected(ref Vector3 value);

		// Token: 0x06001CE2 RID: 7394
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetLocalEulerAngles_Injected(RotationOrder order, out Vector3 ret);

		// Token: 0x06001CE3 RID: 7395
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetLocalEulerAngles_Injected(ref Vector3 euler, RotationOrder order);

		// Token: 0x06001CE4 RID: 7396
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetLocalEulerHint_Injected(ref Vector3 euler);

		// Token: 0x06001CE5 RID: 7397
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_rotation_Injected(out Quaternion ret);

		// Token: 0x06001CE6 RID: 7398
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_rotation_Injected(ref Quaternion value);

		// Token: 0x06001CE7 RID: 7399
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_localRotation_Injected(out Quaternion ret);

		// Token: 0x06001CE8 RID: 7400
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_localRotation_Injected(ref Quaternion value);

		// Token: 0x06001CE9 RID: 7401
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_localScale_Injected(out Vector3 ret);

		// Token: 0x06001CEA RID: 7402
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_localScale_Injected(ref Vector3 value);

		// Token: 0x06001CEB RID: 7403
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_worldToLocalMatrix_Injected(out Matrix4x4 ret);

		// Token: 0x06001CEC RID: 7404
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_localToWorldMatrix_Injected(out Matrix4x4 ret);

		// Token: 0x06001CED RID: 7405
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetPositionAndRotation_Injected(ref Vector3 position, ref Quaternion rotation);

		// Token: 0x06001CEE RID: 7406
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetLocalPositionAndRotation_Injected(ref Vector3 localPosition, ref Quaternion localRotation);

		// Token: 0x06001CEF RID: 7407
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void RotateAroundInternal_Injected(ref Vector3 axis, float angle);

		// Token: 0x06001CF0 RID: 7408
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_LookAt_Injected(ref Vector3 worldPosition, ref Vector3 worldUp);

		// Token: 0x06001CF1 RID: 7409
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void TransformDirection_Injected(ref Vector3 direction, out Vector3 ret);

		// Token: 0x06001CF2 RID: 7410
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InverseTransformDirection_Injected(ref Vector3 direction, out Vector3 ret);

		// Token: 0x06001CF3 RID: 7411
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void TransformVector_Injected(ref Vector3 vector, out Vector3 ret);

		// Token: 0x06001CF4 RID: 7412
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InverseTransformVector_Injected(ref Vector3 vector, out Vector3 ret);

		// Token: 0x06001CF5 RID: 7413
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void TransformPoint_Injected(ref Vector3 position, out Vector3 ret);

		// Token: 0x06001CF6 RID: 7414
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InverseTransformPoint_Injected(ref Vector3 position, out Vector3 ret);

		// Token: 0x06001CF7 RID: 7415
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_lossyScale_Injected(out Vector3 ret);

		// Token: 0x06001CF8 RID: 7416
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void RotateAround_Injected(ref Vector3 axis, float angle);

		// Token: 0x06001CF9 RID: 7417
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void RotateAroundLocal_Injected(ref Vector3 axis, float angle);

		// Token: 0x020002A2 RID: 674
		private class Enumerator : IEnumerator
		{
			// Token: 0x06001CFA RID: 7418 RVA: 0x0002FE1E File Offset: 0x0002E01E
			internal Enumerator(Transform outer)
			{
				this.outer = outer;
			}

			// Token: 0x170005B2 RID: 1458
			// (get) Token: 0x06001CFB RID: 7419 RVA: 0x0002FE38 File Offset: 0x0002E038
			public object Current
			{
				get
				{
					return this.outer.GetChild(this.currentIndex);
				}
			}

			// Token: 0x06001CFC RID: 7420 RVA: 0x0002FE5C File Offset: 0x0002E05C
			public bool MoveNext()
			{
				int childCount = this.outer.childCount;
				int num = this.currentIndex + 1;
				this.currentIndex = num;
				return num < childCount;
			}

			// Token: 0x06001CFD RID: 7421 RVA: 0x0002FE8E File Offset: 0x0002E08E
			public void Reset()
			{
				this.currentIndex = -1;
			}

			// Token: 0x040009A2 RID: 2466
			private Transform outer;

			// Token: 0x040009A3 RID: 2467
			private int currentIndex = -1;
		}
	}
}
