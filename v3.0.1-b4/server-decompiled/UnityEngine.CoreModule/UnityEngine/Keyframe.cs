using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020000E5 RID: 229
	[RequiredByNativeCode]
	public struct Keyframe
	{
		// Token: 0x06000404 RID: 1028 RVA: 0x00006D90 File Offset: 0x00004F90
		public Keyframe(float time, float value)
		{
			this.m_Time = time;
			this.m_Value = value;
			this.m_InTangent = 0f;
			this.m_OutTangent = 0f;
			this.m_WeightedMode = 0;
			this.m_InWeight = 0f;
			this.m_OutWeight = 0f;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00006DDF File Offset: 0x00004FDF
		public Keyframe(float time, float value, float inTangent, float outTangent)
		{
			this.m_Time = time;
			this.m_Value = value;
			this.m_InTangent = inTangent;
			this.m_OutTangent = outTangent;
			this.m_WeightedMode = 0;
			this.m_InWeight = 0f;
			this.m_OutWeight = 0f;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00006E1C File Offset: 0x0000501C
		public Keyframe(float time, float value, float inTangent, float outTangent, float inWeight, float outWeight)
		{
			this.m_Time = time;
			this.m_Value = value;
			this.m_InTangent = inTangent;
			this.m_OutTangent = outTangent;
			this.m_WeightedMode = 3;
			this.m_InWeight = inWeight;
			this.m_OutWeight = outWeight;
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00006E54 File Offset: 0x00005054
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x00006E6C File Offset: 0x0000506C
		public float time
		{
			get
			{
				return this.m_Time;
			}
			set
			{
				this.m_Time = value;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x00006E78 File Offset: 0x00005078
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x00006E90 File Offset: 0x00005090
		public float value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				this.m_Value = value;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x00006E9C File Offset: 0x0000509C
		// (set) Token: 0x0600040C RID: 1036 RVA: 0x00006EB4 File Offset: 0x000050B4
		public float inTangent
		{
			get
			{
				return this.m_InTangent;
			}
			set
			{
				this.m_InTangent = value;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x00006EC0 File Offset: 0x000050C0
		// (set) Token: 0x0600040E RID: 1038 RVA: 0x00006ED8 File Offset: 0x000050D8
		public float outTangent
		{
			get
			{
				return this.m_OutTangent;
			}
			set
			{
				this.m_OutTangent = value;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x00006EE4 File Offset: 0x000050E4
		// (set) Token: 0x06000410 RID: 1040 RVA: 0x00006EFC File Offset: 0x000050FC
		public float inWeight
		{
			get
			{
				return this.m_InWeight;
			}
			set
			{
				this.m_InWeight = value;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x00006F08 File Offset: 0x00005108
		// (set) Token: 0x06000412 RID: 1042 RVA: 0x00006F20 File Offset: 0x00005120
		public float outWeight
		{
			get
			{
				return this.m_OutWeight;
			}
			set
			{
				this.m_OutWeight = value;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x00006F2C File Offset: 0x0000512C
		// (set) Token: 0x06000414 RID: 1044 RVA: 0x00006F44 File Offset: 0x00005144
		public WeightedMode weightedMode
		{
			get
			{
				return (WeightedMode)this.m_WeightedMode;
			}
			set
			{
				this.m_WeightedMode = (int)value;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x00006F50 File Offset: 0x00005150
		// (set) Token: 0x06000416 RID: 1046 RVA: 0x00006F68 File Offset: 0x00005168
		[Obsolete("Use AnimationUtility.SetKeyLeftTangentMode, AnimationUtility.SetKeyRightTangentMode, AnimationUtility.GetKeyLeftTangentMode or AnimationUtility.GetKeyRightTangentMode instead.")]
		public int tangentMode
		{
			get
			{
				return this.tangentModeInternal;
			}
			set
			{
				this.tangentModeInternal = value;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x00006F74 File Offset: 0x00005174
		// (set) Token: 0x06000418 RID: 1048 RVA: 0x00002669 File Offset: 0x00000869
		internal int tangentModeInternal
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		// Token: 0x0400027F RID: 639
		private float m_Time;

		// Token: 0x04000280 RID: 640
		private float m_Value;

		// Token: 0x04000281 RID: 641
		private float m_InTangent;

		// Token: 0x04000282 RID: 642
		private float m_OutTangent;

		// Token: 0x04000283 RID: 643
		private int m_WeightedMode;

		// Token: 0x04000284 RID: 644
		private float m_InWeight;

		// Token: 0x04000285 RID: 645
		private float m_OutWeight;
	}
}
