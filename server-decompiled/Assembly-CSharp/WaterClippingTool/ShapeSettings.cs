using System;
using UnityEngine;

namespace WaterClippingTool
{
	// Token: 0x02001662 RID: 5730
	[Serializable]
	public class ShapeSettings : IEquatable<ShapeSettings>
	{
		// Token: 0x17001592 RID: 5522
		// (get) Token: 0x0600B3F8 RID: 46072 RVA: 0x004341A4 File Offset: 0x004323A4
		public bool hasPlane
		{
			get
			{
				return this.plane != WaterClippingPlanePlacer.DisabledPlaneVec;
			}
		}

		// Token: 0x0600B3F9 RID: 46073 RVA: 0x004341B6 File Offset: 0x004323B6
		public ShapeSettings()
		{
			this.ResetToDefault();
		}

		// Token: 0x0600B3FA RID: 46074 RVA: 0x004341CC File Offset: 0x004323CC
		public void ResetToDefault()
		{
			this.shapeName = string.Empty;
			this.shapeModel = null;
			this.modelOffset = WaterClippingPlanePlacer.DefaultModelOffset;
			this.plane = WaterClippingPlanePlacer.DisabledPlaneVec;
			this.waterFlowMask = BlockFaceFlag.All;
		}

		// Token: 0x0600B3FB RID: 46075 RVA: 0x004341FE File Offset: 0x004323FE
		public void CopyFrom(ShapeSettings other)
		{
			this.shapeName = other.shapeName;
			this.shapeModel = other.shapeModel;
			this.modelOffset = other.modelOffset;
			this.plane = other.plane;
			this.waterFlowMask = other.waterFlowMask;
		}

		// Token: 0x0600B3FC RID: 46076 RVA: 0x0043423C File Offset: 0x0043243C
		public bool Equals(ShapeSettings other)
		{
			return !(this.plane != other.plane) && !(this.shapeName != other.shapeName) && !(this.shapeModel != other.shapeModel) && !(this.modelOffset != other.modelOffset) && this.waterFlowMask == other.waterFlowMask;
		}

		// Token: 0x040086F1 RID: 34545
		public string shapeName;

		// Token: 0x040086F2 RID: 34546
		public GameObject shapeModel;

		// Token: 0x040086F3 RID: 34547
		public Vector3 modelOffset;

		// Token: 0x040086F4 RID: 34548
		public Vector4 plane;

		// Token: 0x040086F5 RID: 34549
		public BlockFaceFlag waterFlowMask = BlockFaceFlag.All;
	}
}
