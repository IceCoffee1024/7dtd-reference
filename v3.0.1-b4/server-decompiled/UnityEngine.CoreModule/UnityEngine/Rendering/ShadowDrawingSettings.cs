using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000474 RID: 1140
	[UsedByNativeCode]
	public struct ShadowDrawingSettings : IEquatable<ShadowDrawingSettings>
	{
		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x060026B8 RID: 9912 RVA: 0x00042598 File Offset: 0x00040798
		// (set) Token: 0x060026B9 RID: 9913 RVA: 0x000425B0 File Offset: 0x000407B0
		public CullingResults cullingResults
		{
			get
			{
				return this.m_CullingResults;
			}
			set
			{
				this.m_CullingResults = value;
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x060026BA RID: 9914 RVA: 0x000425BC File Offset: 0x000407BC
		// (set) Token: 0x060026BB RID: 9915 RVA: 0x000425D4 File Offset: 0x000407D4
		public int lightIndex
		{
			get
			{
				return this.m_LightIndex;
			}
			set
			{
				this.m_LightIndex = value;
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x060026BC RID: 9916 RVA: 0x000425E0 File Offset: 0x000407E0
		// (set) Token: 0x060026BD RID: 9917 RVA: 0x000425FB File Offset: 0x000407FB
		public bool useRenderingLayerMaskTest
		{
			get
			{
				return this.m_UseRenderingLayerMaskTest != 0;
			}
			set
			{
				this.m_UseRenderingLayerMaskTest = (value ? 1 : 0);
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x060026BE RID: 9918 RVA: 0x0004260C File Offset: 0x0004080C
		// (set) Token: 0x060026BF RID: 9919 RVA: 0x00042624 File Offset: 0x00040824
		public ShadowSplitData splitData
		{
			get
			{
				return this.m_SplitData;
			}
			set
			{
				this.m_SplitData = value;
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x060026C0 RID: 9920 RVA: 0x00042630 File Offset: 0x00040830
		// (set) Token: 0x060026C1 RID: 9921 RVA: 0x00042648 File Offset: 0x00040848
		public ShadowObjectsFilter objectsFilter
		{
			get
			{
				return this.m_ObjectsFilter;
			}
			set
			{
				this.m_ObjectsFilter = value;
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x060026C2 RID: 9922 RVA: 0x00042654 File Offset: 0x00040854
		// (set) Token: 0x060026C3 RID: 9923 RVA: 0x0004266C File Offset: 0x0004086C
		public BatchCullingProjectionType projectionType
		{
			get
			{
				return this.m_ProjectionType;
			}
			set
			{
				this.m_ProjectionType = value;
			}
		}

		// Token: 0x060026C4 RID: 9924 RVA: 0x00042676 File Offset: 0x00040876
		[Obsolete("ShadowDrawingSettings(CullingResults, int) is deprecated. Use ShadowDrawingSettings(CullingResults, int, BatchCullingProjectionType) instead.")]
		public ShadowDrawingSettings(CullingResults cullingResults, int lightIndex)
		{
			this = new ShadowDrawingSettings(cullingResults, lightIndex, BatchCullingProjectionType.Unknown);
		}

		// Token: 0x060026C5 RID: 9925 RVA: 0x00042684 File Offset: 0x00040884
		public ShadowDrawingSettings(CullingResults cullingResults, int lightIndex, BatchCullingProjectionType projectionType)
		{
			this.m_CullingResults = cullingResults;
			this.m_LightIndex = lightIndex;
			this.m_UseRenderingLayerMaskTest = 0;
			this.m_SplitData = default(ShadowSplitData);
			this.m_SplitData.shadowCascadeBlendCullingFactor = 1f;
			this.m_ObjectsFilter = ShadowObjectsFilter.AllObjects;
			this.m_ProjectionType = projectionType;
		}

		// Token: 0x060026C6 RID: 9926 RVA: 0x000426D4 File Offset: 0x000408D4
		public bool Equals(ShadowDrawingSettings other)
		{
			return this.m_CullingResults.Equals(other.m_CullingResults) && this.m_LightIndex == other.m_LightIndex && this.m_SplitData.Equals(other.m_SplitData) && this.m_UseRenderingLayerMaskTest.Equals(other.m_UseRenderingLayerMaskTest) && this.m_ObjectsFilter.Equals(other.m_ObjectsFilter);
		}

		// Token: 0x060026C7 RID: 9927 RVA: 0x0004274C File Offset: 0x0004094C
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is ShadowDrawingSettings && this.Equals((ShadowDrawingSettings)obj);
		}

		// Token: 0x060026C8 RID: 9928 RVA: 0x00042784 File Offset: 0x00040984
		public override int GetHashCode()
		{
			int num = this.m_CullingResults.GetHashCode();
			num = (num * 397) ^ this.m_LightIndex;
			num = (num * 397) ^ this.m_UseRenderingLayerMaskTest;
			num = (num * 397) ^ this.m_SplitData.GetHashCode();
			return (num * 397) ^ (int)this.m_ObjectsFilter;
		}

		// Token: 0x060026C9 RID: 9929 RVA: 0x000427F4 File Offset: 0x000409F4
		public static bool operator ==(ShadowDrawingSettings left, ShadowDrawingSettings right)
		{
			return left.Equals(right);
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x00042810 File Offset: 0x00040A10
		public static bool operator !=(ShadowDrawingSettings left, ShadowDrawingSettings right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000E9D RID: 3741
		private CullingResults m_CullingResults;

		// Token: 0x04000E9E RID: 3742
		private int m_LightIndex;

		// Token: 0x04000E9F RID: 3743
		private int m_UseRenderingLayerMaskTest;

		// Token: 0x04000EA0 RID: 3744
		private ShadowSplitData m_SplitData;

		// Token: 0x04000EA1 RID: 3745
		private ShadowObjectsFilter m_ObjectsFilter;

		// Token: 0x04000EA2 RID: 3746
		private BatchCullingProjectionType m_ProjectionType;
	}
}
