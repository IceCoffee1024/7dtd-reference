using System;
using UnityEngine;

namespace WaterClippingTool
{
	// Token: 0x02001681 RID: 5761
	[Serializable]
	public class ShapeSettings : IEquatable<ShapeSettings>
	{
		// Token: 0x170015D5 RID: 5589
		// (get) Token: 0x0600B48A RID: 46218 RVA: 0x00437E80 File Offset: 0x00436080
		public bool hasPlane
		{
			get
			{
				return this.plane != WaterClippingPlanePlacer.DisabledPlaneVec;
			}
		}

		// Token: 0x0600B48B RID: 46219 RVA: 0x00437E92 File Offset: 0x00436092
		public ShapeSettings()
		{
			this.ResetToDefault();
		}

		// Token: 0x0600B48C RID: 46220 RVA: 0x00437EA8 File Offset: 0x004360A8
		public void ResetToDefault()
		{
			this.shapeName = string.Empty;
			this.shapeModel = null;
			this.modelOffset = WaterClippingPlanePlacer.DefaultModelOffset;
			this.plane = WaterClippingPlanePlacer.DisabledPlaneVec;
			this.waterFlowMask = BlockFaceFlag.All;
		}

		// Token: 0x0600B48D RID: 46221 RVA: 0x00437EDA File Offset: 0x004360DA
		public void CopyFrom(ShapeSettings other)
		{
			this.shapeName = other.shapeName;
			this.shapeModel = other.shapeModel;
			this.modelOffset = other.modelOffset;
			this.plane = other.plane;
			this.waterFlowMask = other.waterFlowMask;
		}

		// Token: 0x0600B48E RID: 46222 RVA: 0x00437F18 File Offset: 0x00436118
		public bool Equals(ShapeSettings other)
		{
			return !(this.plane != other.plane) && !(this.shapeName != other.shapeName) && !(this.shapeModel != other.shapeModel) && !(this.modelOffset != other.modelOffset) && this.waterFlowMask == other.waterFlowMask;
		}

		// Token: 0x04008747 RID: 34631
		public string shapeName;

		// Token: 0x04008748 RID: 34632
		public GameObject shapeModel;

		// Token: 0x04008749 RID: 34633
		public Vector3 modelOffset;

		// Token: 0x0400874A RID: 34634
		public Vector4 plane;

		// Token: 0x0400874B RID: 34635
		public BlockFaceFlag waterFlowMask = BlockFaceFlag.All;
	}
}
