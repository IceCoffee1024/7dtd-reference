using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x0200044D RID: 1101
	[UsedByNativeCode]
	public struct CameraProperties : IEquatable<CameraProperties>
	{
		// Token: 0x060024E0 RID: 9440 RVA: 0x0003E000 File Offset: 0x0003C200
		public unsafe Plane GetShadowCullingPlane(int index)
		{
			bool flag = index < 0 || index >= 6;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("{0} was {1}, but must be at least 0 and less than {2}", "index", index, 6));
			}
			fixed (byte* ptr = &this.m_ShadowCullPlanes.FixedElementField)
			{
				byte* ptr2 = ptr;
				Plane* ptr3 = (Plane*)ptr2;
				return ptr3[index];
			}
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x0003E06C File Offset: 0x0003C26C
		public unsafe void SetShadowCullingPlane(int index, Plane plane)
		{
			bool flag = index < 0 || index >= 6;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("{0} was {1}, but must be at least 0 and less than {2}", "index", index, 6));
			}
			fixed (byte* ptr = &this.m_ShadowCullPlanes.FixedElementField)
			{
				byte* ptr2 = ptr;
				Plane* ptr3 = (Plane*)ptr2;
				ptr3[index] = plane;
			}
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x0003E0D8 File Offset: 0x0003C2D8
		public unsafe Plane GetCameraCullingPlane(int index)
		{
			bool flag = index < 0 || index >= 6;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("{0} was {1}, but must be at least 0 and less than {2}", "index", index, 6));
			}
			fixed (byte* ptr = &this.m_CameraCullPlanes.FixedElementField)
			{
				byte* ptr2 = ptr;
				Plane* ptr3 = (Plane*)ptr2;
				return ptr3[index];
			}
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x0003E144 File Offset: 0x0003C344
		public unsafe void SetCameraCullingPlane(int index, Plane plane)
		{
			bool flag = index < 0 || index >= 6;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("{0} was {1}, but must be at least 0 and less than {2}", "index", index, 6));
			}
			fixed (byte* ptr = &this.m_CameraCullPlanes.FixedElementField)
			{
				byte* ptr2 = ptr;
				Plane* ptr3 = (Plane*)ptr2;
				ptr3[index] = plane;
			}
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x0003E1B0 File Offset: 0x0003C3B0
		public unsafe bool Equals(CameraProperties other)
		{
			for (int i = 0; i < 6; i++)
			{
				bool flag = !this.GetShadowCullingPlane(i).Equals(other.GetShadowCullingPlane(i));
				if (flag)
				{
					return false;
				}
			}
			for (int j = 0; j < 6; j++)
			{
				bool flag2 = !this.GetCameraCullingPlane(j).Equals(other.GetCameraCullingPlane(j));
				if (flag2)
				{
					return false;
				}
			}
			fixed (float* ptr = &this.layerCullDistances.FixedElementField)
			{
				float* ptr2 = ptr;
				for (int k = 0; k < 32; k++)
				{
					bool flag3 = ptr2[k] != *((ref other.layerCullDistances.FixedElementField) + (IntPtr)k * 4);
					if (flag3)
					{
						return false;
					}
				}
			}
			return this.screenRect.Equals(other.screenRect) && this.viewDir.Equals(other.viewDir) && this.projectionNear.Equals(other.projectionNear) && this.projectionFar.Equals(other.projectionFar) && this.cameraNear.Equals(other.cameraNear) && this.cameraFar.Equals(other.cameraFar) && this.cameraAspect.Equals(other.cameraAspect) && this.cameraToWorld.Equals(other.cameraToWorld) && this.actualWorldToClip.Equals(other.actualWorldToClip) && this.cameraClipToWorld.Equals(other.cameraClipToWorld) && this.cameraWorldToClip.Equals(other.cameraWorldToClip) && this.implicitProjection.Equals(other.implicitProjection) && this.stereoWorldToClipLeft.Equals(other.stereoWorldToClipLeft) && this.stereoWorldToClipRight.Equals(other.stereoWorldToClipRight) && this.worldToCamera.Equals(other.worldToCamera) && this.up.Equals(other.up) && this.right.Equals(other.right) && this.transformDirection.Equals(other.transformDirection) && this.cameraEuler.Equals(other.cameraEuler) && this.velocity.Equals(other.velocity) && this.farPlaneWorldSpaceLength.Equals(other.farPlaneWorldSpaceLength) && this.rendererCount == other.rendererCount && this.baseFarDistance.Equals(other.baseFarDistance) && this.shadowCullCenter.Equals(other.shadowCullCenter) && this.layerCullSpherical == other.layerCullSpherical && this.coreCameraValues.Equals(other.coreCameraValues) && this.cameraType == other.cameraType && this.projectionIsOblique == other.projectionIsOblique && this.isImplicitProjectionMatrix == other.isImplicitProjectionMatrix;
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x0003E504 File Offset: 0x0003C704
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is CameraProperties && this.Equals((CameraProperties)obj);
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x0003E53C File Offset: 0x0003C73C
		public unsafe override int GetHashCode()
		{
			int num = this.screenRect.GetHashCode();
			num = (num * 397) ^ this.viewDir.GetHashCode();
			num = (num * 397) ^ this.projectionNear.GetHashCode();
			num = (num * 397) ^ this.projectionFar.GetHashCode();
			num = (num * 397) ^ this.cameraNear.GetHashCode();
			num = (num * 397) ^ this.cameraFar.GetHashCode();
			num = (num * 397) ^ this.cameraAspect.GetHashCode();
			num = (num * 397) ^ this.cameraToWorld.GetHashCode();
			num = (num * 397) ^ this.actualWorldToClip.GetHashCode();
			num = (num * 397) ^ this.cameraClipToWorld.GetHashCode();
			num = (num * 397) ^ this.cameraWorldToClip.GetHashCode();
			num = (num * 397) ^ this.implicitProjection.GetHashCode();
			num = (num * 397) ^ this.stereoWorldToClipLeft.GetHashCode();
			num = (num * 397) ^ this.stereoWorldToClipRight.GetHashCode();
			num = (num * 397) ^ this.worldToCamera.GetHashCode();
			num = (num * 397) ^ this.up.GetHashCode();
			num = (num * 397) ^ this.right.GetHashCode();
			num = (num * 397) ^ this.transformDirection.GetHashCode();
			num = (num * 397) ^ this.cameraEuler.GetHashCode();
			num = (num * 397) ^ this.velocity.GetHashCode();
			num = (num * 397) ^ this.farPlaneWorldSpaceLength.GetHashCode();
			num = (num * 397) ^ (int)this.rendererCount;
			for (int i = 0; i < 6; i++)
			{
				num = (num * 397) ^ this.GetShadowCullingPlane(i).GetHashCode();
			}
			for (int j = 0; j < 6; j++)
			{
				num = (num * 397) ^ this.GetCameraCullingPlane(j).GetHashCode();
			}
			num = (num * 397) ^ this.baseFarDistance.GetHashCode();
			num = (num * 397) ^ this.shadowCullCenter.GetHashCode();
			fixed (float* ptr = &this.layerCullDistances.FixedElementField)
			{
				float* ptr2 = ptr;
				for (int k = 0; k < 32; k++)
				{
					num = (num * 397) ^ ptr2[k].GetHashCode();
				}
			}
			num = (num * 397) ^ this.layerCullSpherical;
			num = (num * 397) ^ this.coreCameraValues.GetHashCode();
			num = (num * 397) ^ (int)this.cameraType;
			num = (num * 397) ^ this.projectionIsOblique;
			return (num * 397) ^ this.isImplicitProjectionMatrix;
		}

		// Token: 0x060024E7 RID: 9447 RVA: 0x0003E88C File Offset: 0x0003CA8C
		public static bool operator ==(CameraProperties left, CameraProperties right)
		{
			return left.Equals(right);
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x0003E8A8 File Offset: 0x0003CAA8
		public static bool operator !=(CameraProperties left, CameraProperties right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000DBB RID: 3515
		private const int k_NumLayers = 32;

		// Token: 0x04000DBC RID: 3516
		private Rect screenRect;

		// Token: 0x04000DBD RID: 3517
		private Vector3 viewDir;

		// Token: 0x04000DBE RID: 3518
		private float projectionNear;

		// Token: 0x04000DBF RID: 3519
		private float projectionFar;

		// Token: 0x04000DC0 RID: 3520
		private float cameraNear;

		// Token: 0x04000DC1 RID: 3521
		private float cameraFar;

		// Token: 0x04000DC2 RID: 3522
		private float cameraAspect;

		// Token: 0x04000DC3 RID: 3523
		private Matrix4x4 cameraToWorld;

		// Token: 0x04000DC4 RID: 3524
		private Matrix4x4 actualWorldToClip;

		// Token: 0x04000DC5 RID: 3525
		private Matrix4x4 cameraClipToWorld;

		// Token: 0x04000DC6 RID: 3526
		private Matrix4x4 cameraWorldToClip;

		// Token: 0x04000DC7 RID: 3527
		private Matrix4x4 implicitProjection;

		// Token: 0x04000DC8 RID: 3528
		private Matrix4x4 stereoWorldToClipLeft;

		// Token: 0x04000DC9 RID: 3529
		private Matrix4x4 stereoWorldToClipRight;

		// Token: 0x04000DCA RID: 3530
		private Matrix4x4 worldToCamera;

		// Token: 0x04000DCB RID: 3531
		private Vector3 up;

		// Token: 0x04000DCC RID: 3532
		private Vector3 right;

		// Token: 0x04000DCD RID: 3533
		private Vector3 transformDirection;

		// Token: 0x04000DCE RID: 3534
		private Vector3 cameraEuler;

		// Token: 0x04000DCF RID: 3535
		private Vector3 velocity;

		// Token: 0x04000DD0 RID: 3536
		private float farPlaneWorldSpaceLength;

		// Token: 0x04000DD1 RID: 3537
		private uint rendererCount;

		// Token: 0x04000DD2 RID: 3538
		private const int k_PlaneCount = 6;

		// Token: 0x04000DD3 RID: 3539
		[FixedBuffer(typeof(byte), 96)]
		internal CameraProperties.<m_ShadowCullPlanes>e__FixedBuffer m_ShadowCullPlanes;

		// Token: 0x04000DD4 RID: 3540
		[FixedBuffer(typeof(byte), 96)]
		internal CameraProperties.<m_CameraCullPlanes>e__FixedBuffer m_CameraCullPlanes;

		// Token: 0x04000DD5 RID: 3541
		private float baseFarDistance;

		// Token: 0x04000DD6 RID: 3542
		private Vector3 shadowCullCenter;

		// Token: 0x04000DD7 RID: 3543
		[FixedBuffer(typeof(float), 32)]
		internal CameraProperties.<layerCullDistances>e__FixedBuffer layerCullDistances;

		// Token: 0x04000DD8 RID: 3544
		private int layerCullSpherical;

		// Token: 0x04000DD9 RID: 3545
		private CoreCameraValues coreCameraValues;

		// Token: 0x04000DDA RID: 3546
		private uint cameraType;

		// Token: 0x04000DDB RID: 3547
		private int projectionIsOblique;

		// Token: 0x04000DDC RID: 3548
		private int isImplicitProjectionMatrix;

		// Token: 0x0200044E RID: 1102
		[UnsafeValueType]
		[CompilerGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 128)]
		public struct <layerCullDistances>e__FixedBuffer
		{
			// Token: 0x04000DDD RID: 3549
			public float FixedElementField;
		}

		// Token: 0x0200044F RID: 1103
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 96)]
		public struct <m_CameraCullPlanes>e__FixedBuffer
		{
			// Token: 0x04000DDE RID: 3550
			public byte FixedElementField;
		}

		// Token: 0x02000450 RID: 1104
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 96)]
		public struct <m_ShadowCullPlanes>e__FixedBuffer
		{
			// Token: 0x04000DDF RID: 3551
			public byte FixedElementField;
		}
	}
}
