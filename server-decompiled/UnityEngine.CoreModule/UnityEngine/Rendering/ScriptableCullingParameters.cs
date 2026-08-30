using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000452 RID: 1106
	[UsedByNativeCode]
	public struct ScriptableCullingParameters : IEquatable<ScriptableCullingParameters>
	{
		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x060024E9 RID: 9449 RVA: 0x0003E8C8 File Offset: 0x0003CAC8
		// (set) Token: 0x060024EA RID: 9450 RVA: 0x0003E8E0 File Offset: 0x0003CAE0
		public int maximumVisibleLights
		{
			get
			{
				return this.m_maximumVisibleLights;
			}
			set
			{
				this.m_maximumVisibleLights = value;
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x060024EB RID: 9451 RVA: 0x0003E8EC File Offset: 0x0003CAEC
		// (set) Token: 0x060024EC RID: 9452 RVA: 0x0003E904 File Offset: 0x0003CB04
		public bool conservativeEnclosingSphere
		{
			get
			{
				return this.m_ConservativeEnclosingSphere;
			}
			set
			{
				this.m_ConservativeEnclosingSphere = value;
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x060024ED RID: 9453 RVA: 0x0003E910 File Offset: 0x0003CB10
		// (set) Token: 0x060024EE RID: 9454 RVA: 0x0003E928 File Offset: 0x0003CB28
		public int numIterationsEnclosingSphere
		{
			get
			{
				return this.m_NumIterationsEnclosingSphere;
			}
			set
			{
				this.m_NumIterationsEnclosingSphere = value;
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x060024EF RID: 9455 RVA: 0x0003E934 File Offset: 0x0003CB34
		// (set) Token: 0x060024F0 RID: 9456 RVA: 0x0003E94C File Offset: 0x0003CB4C
		public int cullingPlaneCount
		{
			get
			{
				return this.m_CullingPlaneCount;
			}
			set
			{
				bool flag = value < 0 || value > 10;
				if (flag)
				{
					throw new ArgumentOutOfRangeException(string.Format("{0} was {1}, but must be at least 0 and less than {2}", "value", value, 10));
				}
				this.m_CullingPlaneCount = value;
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x060024F1 RID: 9457 RVA: 0x0003E994 File Offset: 0x0003CB94
		// (set) Token: 0x060024F2 RID: 9458 RVA: 0x0003E9B1 File Offset: 0x0003CBB1
		public bool isOrthographic
		{
			get
			{
				return Convert.ToBoolean(this.m_IsOrthographic);
			}
			set
			{
				this.m_IsOrthographic = Convert.ToInt32(value);
			}
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x060024F3 RID: 9459 RVA: 0x0003E9C0 File Offset: 0x0003CBC0
		// (set) Token: 0x060024F4 RID: 9460 RVA: 0x0003E9D8 File Offset: 0x0003CBD8
		public LODParameters lodParameters
		{
			get
			{
				return this.m_LODParameters;
			}
			set
			{
				this.m_LODParameters = value;
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x060024F5 RID: 9461 RVA: 0x0003E9E4 File Offset: 0x0003CBE4
		// (set) Token: 0x060024F6 RID: 9462 RVA: 0x0003E9FC File Offset: 0x0003CBFC
		public uint cullingMask
		{
			get
			{
				return this.m_CullingMask;
			}
			set
			{
				this.m_CullingMask = value;
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x060024F7 RID: 9463 RVA: 0x0003EA08 File Offset: 0x0003CC08
		// (set) Token: 0x060024F8 RID: 9464 RVA: 0x0003EA20 File Offset: 0x0003CC20
		public Matrix4x4 cullingMatrix
		{
			get
			{
				return this.m_CullingMatrix;
			}
			set
			{
				this.m_CullingMatrix = value;
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x060024F9 RID: 9465 RVA: 0x0003EA2C File Offset: 0x0003CC2C
		// (set) Token: 0x060024FA RID: 9466 RVA: 0x0003EA44 File Offset: 0x0003CC44
		public Vector3 origin
		{
			get
			{
				return this.m_Origin;
			}
			set
			{
				this.m_Origin = value;
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x060024FB RID: 9467 RVA: 0x0003EA50 File Offset: 0x0003CC50
		// (set) Token: 0x060024FC RID: 9468 RVA: 0x0003EA68 File Offset: 0x0003CC68
		public float shadowDistance
		{
			get
			{
				return this.m_ShadowDistance;
			}
			set
			{
				this.m_ShadowDistance = value;
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x060024FD RID: 9469 RVA: 0x0003EA74 File Offset: 0x0003CC74
		// (set) Token: 0x060024FE RID: 9470 RVA: 0x0003EA8C File Offset: 0x0003CC8C
		public float shadowNearPlaneOffset
		{
			get
			{
				return this.m_ShadowNearPlaneOffset;
			}
			set
			{
				this.m_ShadowNearPlaneOffset = value;
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x060024FF RID: 9471 RVA: 0x0003EA98 File Offset: 0x0003CC98
		// (set) Token: 0x06002500 RID: 9472 RVA: 0x0003EAB0 File Offset: 0x0003CCB0
		public CullingOptions cullingOptions
		{
			get
			{
				return this.m_CullingOptions;
			}
			set
			{
				this.m_CullingOptions = value;
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06002501 RID: 9473 RVA: 0x0003EABC File Offset: 0x0003CCBC
		// (set) Token: 0x06002502 RID: 9474 RVA: 0x0003EAD4 File Offset: 0x0003CCD4
		public ReflectionProbeSortingCriteria reflectionProbeSortingCriteria
		{
			get
			{
				return this.m_ReflectionProbeSortingCriteria;
			}
			set
			{
				this.m_ReflectionProbeSortingCriteria = value;
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06002503 RID: 9475 RVA: 0x0003EAE0 File Offset: 0x0003CCE0
		// (set) Token: 0x06002504 RID: 9476 RVA: 0x0003EAF8 File Offset: 0x0003CCF8
		public CameraProperties cameraProperties
		{
			get
			{
				return this.m_CameraProperties;
			}
			set
			{
				this.m_CameraProperties = value;
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06002505 RID: 9477 RVA: 0x0003EB04 File Offset: 0x0003CD04
		// (set) Token: 0x06002506 RID: 9478 RVA: 0x0003EB1C File Offset: 0x0003CD1C
		public Matrix4x4 stereoViewMatrix
		{
			get
			{
				return this.m_StereoViewMatrix;
			}
			set
			{
				this.m_StereoViewMatrix = value;
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06002507 RID: 9479 RVA: 0x0003EB28 File Offset: 0x0003CD28
		// (set) Token: 0x06002508 RID: 9480 RVA: 0x0003EB40 File Offset: 0x0003CD40
		public Matrix4x4 stereoProjectionMatrix
		{
			get
			{
				return this.m_StereoProjectionMatrix;
			}
			set
			{
				this.m_StereoProjectionMatrix = value;
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06002509 RID: 9481 RVA: 0x0003EB4C File Offset: 0x0003CD4C
		// (set) Token: 0x0600250A RID: 9482 RVA: 0x0003EB64 File Offset: 0x0003CD64
		public float stereoSeparationDistance
		{
			get
			{
				return this.m_StereoSeparationDistance;
			}
			set
			{
				this.m_StereoSeparationDistance = value;
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x0600250B RID: 9483 RVA: 0x0003EB70 File Offset: 0x0003CD70
		// (set) Token: 0x0600250C RID: 9484 RVA: 0x0003EB88 File Offset: 0x0003CD88
		public float accurateOcclusionThreshold
		{
			get
			{
				return this.m_AccurateOcclusionThreshold;
			}
			set
			{
				this.m_AccurateOcclusionThreshold = Mathf.Max(-1f, value);
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x0600250D RID: 9485 RVA: 0x0003EB9C File Offset: 0x0003CD9C
		// (set) Token: 0x0600250E RID: 9486 RVA: 0x0003EBB4 File Offset: 0x0003CDB4
		public int maximumPortalCullingJobs
		{
			get
			{
				return this.m_MaximumPortalCullingJobs;
			}
			set
			{
				bool flag = value < 1 || value > 16;
				if (flag)
				{
					throw new ArgumentOutOfRangeException(string.Format("{0} was {1}, but must be in range {2} to {3}", new object[] { "maximumPortalCullingJobs", this.maximumPortalCullingJobs, 1, 16 }));
				}
				this.m_MaximumPortalCullingJobs = value;
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x0600250F RID: 9487 RVA: 0x0003EC18 File Offset: 0x0003CE18
		public static int cullingJobsLowerLimit
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06002510 RID: 9488 RVA: 0x0003EC2C File Offset: 0x0003CE2C
		public static int cullingJobsUpperLimit
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x06002511 RID: 9489 RVA: 0x0003EC40 File Offset: 0x0003CE40
		public unsafe float GetLayerCullingDistance(int layerIndex)
		{
			bool flag = layerIndex < 0 || layerIndex >= 32;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("{0} was {1}, but must be at least 0 and less than {2}", "layerIndex", layerIndex, 32));
			}
			fixed (float* ptr = &this.m_LayerFarCullDistances.FixedElementField)
			{
				float* ptr2 = ptr;
				return ptr2[layerIndex];
			}
		}

		// Token: 0x06002512 RID: 9490 RVA: 0x0003ECA0 File Offset: 0x0003CEA0
		public unsafe void SetLayerCullingDistance(int layerIndex, float distance)
		{
			bool flag = layerIndex < 0 || layerIndex >= 32;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("{0} was {1}, but must be at least 0 and less than {2}", "layerIndex", layerIndex, 32));
			}
			fixed (float* ptr = &this.m_LayerFarCullDistances.FixedElementField)
			{
				float* ptr2 = ptr;
				ptr2[layerIndex] = distance;
			}
		}

		// Token: 0x06002513 RID: 9491 RVA: 0x0003ED00 File Offset: 0x0003CF00
		public unsafe Plane GetCullingPlane(int index)
		{
			bool flag = index < 0 || index >= this.cullingPlaneCount;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("{0} was {1}, but must be at least 0 and less than {2}", "index", index, this.cullingPlaneCount));
			}
			fixed (byte* ptr = &this.m_CullingPlanes.FixedElementField)
			{
				byte* ptr2 = ptr;
				Plane* ptr3 = (Plane*)ptr2;
				return ptr3[index];
			}
		}

		// Token: 0x06002514 RID: 9492 RVA: 0x0003ED74 File Offset: 0x0003CF74
		public unsafe void SetCullingPlane(int index, Plane plane)
		{
			bool flag = index < 0 || index >= this.cullingPlaneCount;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("{0} was {1}, but must be at least 0 and less than {2}", "index", index, this.cullingPlaneCount));
			}
			fixed (byte* ptr = &this.m_CullingPlanes.FixedElementField)
			{
				byte* ptr2 = ptr;
				Plane* ptr3 = (Plane*)ptr2;
				ptr3[index] = plane;
			}
		}

		// Token: 0x06002515 RID: 9493 RVA: 0x0003EDE8 File Offset: 0x0003CFE8
		public bool Equals(ScriptableCullingParameters other)
		{
			for (int i = 0; i < 32; i++)
			{
				bool flag = !this.GetLayerCullingDistance(i).Equals(other.GetLayerCullingDistance(i));
				if (flag)
				{
					return false;
				}
			}
			for (int j = 0; j < this.cullingPlaneCount; j++)
			{
				bool flag2 = !this.GetCullingPlane(j).Equals(other.GetCullingPlane(j));
				if (flag2)
				{
					return false;
				}
			}
			return this.m_IsOrthographic == other.m_IsOrthographic && this.m_LODParameters.Equals(other.m_LODParameters) && this.m_CullingPlaneCount == other.m_CullingPlaneCount && this.m_CullingMask == other.m_CullingMask && this.m_SceneMask == other.m_SceneMask && this.m_ViewID == other.m_ViewID && this.m_LayerCull == other.m_LayerCull && this.m_CullingMatrix.Equals(other.m_CullingMatrix) && this.m_Origin.Equals(other.m_Origin) && this.m_ShadowDistance.Equals(other.m_ShadowDistance) && this.m_ShadowNearPlaneOffset.Equals(other.m_ShadowNearPlaneOffset) && this.m_CullingOptions == other.m_CullingOptions && this.m_ReflectionProbeSortingCriteria == other.m_ReflectionProbeSortingCriteria && this.m_CameraProperties.Equals(other.m_CameraProperties) && this.m_AccurateOcclusionThreshold.Equals(other.m_AccurateOcclusionThreshold) && this.m_StereoViewMatrix.Equals(other.m_StereoViewMatrix) && this.m_StereoProjectionMatrix.Equals(other.m_StereoProjectionMatrix) && this.m_StereoSeparationDistance.Equals(other.m_StereoSeparationDistance) && this.m_maximumVisibleLights == other.m_maximumVisibleLights && this.m_ConservativeEnclosingSphere == other.m_ConservativeEnclosingSphere && this.m_NumIterationsEnclosingSphere == other.m_NumIterationsEnclosingSphere;
		}

		// Token: 0x06002516 RID: 9494 RVA: 0x0003F008 File Offset: 0x0003D208
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is ScriptableCullingParameters && this.Equals((ScriptableCullingParameters)obj);
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x0003F040 File Offset: 0x0003D240
		public override int GetHashCode()
		{
			int num = this.m_IsOrthographic;
			num = (num * 397) ^ this.m_LODParameters.GetHashCode();
			num = (num * 397) ^ this.m_CullingPlaneCount;
			num = (num * 397) ^ (int)this.m_CullingMask;
			num = (num * 397) ^ this.m_SceneMask.GetHashCode();
			num = (num * 397) ^ this.m_ViewID.GetHashCode();
			num = (num * 397) ^ this.m_LayerCull;
			num = (num * 397) ^ this.m_CullingMatrix.GetHashCode();
			num = (num * 397) ^ this.m_Origin.GetHashCode();
			num = (num * 397) ^ this.m_ShadowDistance.GetHashCode();
			num = (num * 397) ^ this.m_ShadowNearPlaneOffset.GetHashCode();
			num = (num * 397) ^ (int)this.m_CullingOptions;
			num = (num * 397) ^ (int)this.m_ReflectionProbeSortingCriteria;
			num = (num * 397) ^ this.m_CameraProperties.GetHashCode();
			num = (num * 397) ^ this.m_AccurateOcclusionThreshold.GetHashCode();
			num = (num * 397) ^ this.m_MaximumPortalCullingJobs.GetHashCode();
			num = (num * 397) ^ this.m_StereoViewMatrix.GetHashCode();
			num = (num * 397) ^ this.m_StereoProjectionMatrix.GetHashCode();
			num = (num * 397) ^ this.m_StereoSeparationDistance.GetHashCode();
			num = (num * 397) ^ this.m_maximumVisibleLights;
			num = (num * 397) ^ this.m_ConservativeEnclosingSphere.GetHashCode();
			return (num * 397) ^ this.m_NumIterationsEnclosingSphere.GetHashCode();
		}

		// Token: 0x06002518 RID: 9496 RVA: 0x0003F208 File Offset: 0x0003D408
		public static bool operator ==(ScriptableCullingParameters left, ScriptableCullingParameters right)
		{
			return left.Equals(right);
		}

		// Token: 0x06002519 RID: 9497 RVA: 0x0003F224 File Offset: 0x0003D424
		public static bool operator !=(ScriptableCullingParameters left, ScriptableCullingParameters right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000DE9 RID: 3561
		private int m_IsOrthographic;

		// Token: 0x04000DEA RID: 3562
		private LODParameters m_LODParameters;

		// Token: 0x04000DEB RID: 3563
		private const int k_MaximumCullingPlaneCount = 10;

		// Token: 0x04000DEC RID: 3564
		public static readonly int maximumCullingPlaneCount = 10;

		// Token: 0x04000DED RID: 3565
		[FixedBuffer(typeof(byte), 160)]
		internal ScriptableCullingParameters.<m_CullingPlanes>e__FixedBuffer m_CullingPlanes;

		// Token: 0x04000DEE RID: 3566
		private int m_CullingPlaneCount;

		// Token: 0x04000DEF RID: 3567
		private uint m_CullingMask;

		// Token: 0x04000DF0 RID: 3568
		private ulong m_SceneMask;

		// Token: 0x04000DF1 RID: 3569
		private ulong m_ViewID;

		// Token: 0x04000DF2 RID: 3570
		private const int k_LayerCount = 32;

		// Token: 0x04000DF3 RID: 3571
		public static readonly int layerCount = 32;

		// Token: 0x04000DF4 RID: 3572
		[FixedBuffer(typeof(float), 32)]
		internal ScriptableCullingParameters.<m_LayerFarCullDistances>e__FixedBuffer m_LayerFarCullDistances;

		// Token: 0x04000DF5 RID: 3573
		private int m_LayerCull;

		// Token: 0x04000DF6 RID: 3574
		private Matrix4x4 m_CullingMatrix;

		// Token: 0x04000DF7 RID: 3575
		private Vector3 m_Origin;

		// Token: 0x04000DF8 RID: 3576
		private float m_ShadowDistance;

		// Token: 0x04000DF9 RID: 3577
		private float m_ShadowNearPlaneOffset;

		// Token: 0x04000DFA RID: 3578
		private CullingOptions m_CullingOptions;

		// Token: 0x04000DFB RID: 3579
		private ReflectionProbeSortingCriteria m_ReflectionProbeSortingCriteria;

		// Token: 0x04000DFC RID: 3580
		private CameraProperties m_CameraProperties;

		// Token: 0x04000DFD RID: 3581
		private float m_AccurateOcclusionThreshold;

		// Token: 0x04000DFE RID: 3582
		private int m_MaximumPortalCullingJobs;

		// Token: 0x04000DFF RID: 3583
		private const int k_CullingJobCountLowerLimit = 1;

		// Token: 0x04000E00 RID: 3584
		private const int k_CullingJobCountUpperLimit = 16;

		// Token: 0x04000E01 RID: 3585
		private Matrix4x4 m_StereoViewMatrix;

		// Token: 0x04000E02 RID: 3586
		private Matrix4x4 m_StereoProjectionMatrix;

		// Token: 0x04000E03 RID: 3587
		private float m_StereoSeparationDistance;

		// Token: 0x04000E04 RID: 3588
		private int m_maximumVisibleLights;

		// Token: 0x04000E05 RID: 3589
		private bool m_ConservativeEnclosingSphere;

		// Token: 0x04000E06 RID: 3590
		private int m_NumIterationsEnclosingSphere;

		// Token: 0x02000453 RID: 1107
		[UnsafeValueType]
		[CompilerGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 160)]
		public struct <m_CullingPlanes>e__FixedBuffer
		{
			// Token: 0x04000E07 RID: 3591
			public byte FixedElementField;
		}

		// Token: 0x02000454 RID: 1108
		[UnsafeValueType]
		[CompilerGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 128)]
		public struct <m_LayerFarCullDistances>e__FixedBuffer
		{
			// Token: 0x04000E08 RID: 3592
			public float FixedElementField;
		}
	}
}
