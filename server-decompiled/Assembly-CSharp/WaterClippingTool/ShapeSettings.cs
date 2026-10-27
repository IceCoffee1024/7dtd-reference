using System;
using UnityEngine;

namespace WaterClippingTool
{
	// Token: 0x02001693 RID: 5779
	[Serializable]
	public class ShapeSettings : IEquatable<ShapeSettings>
	{
		// Token: 0x170015E1 RID: 5601
		// (get) Token: 0x0600B4E2 RID: 46306 RVA: 0x0043A284 File Offset: 0x00438484
		public bool hasPlane
		{
			get
			{
				return this.plane != WaterClippingPlanePlacer.DisabledPlaneVec;
			}
		}

		// Token: 0x0600B4E3 RID: 46307 RVA: 0x0043A296 File Offset: 0x00438496
		public ShapeSettings()
		{
			this.ResetToDefault();
		}

		// Token: 0x0600B4E4 RID: 46308 RVA: 0x0043A2AC File Offset: 0x004384AC
		public void ResetToDefault()
		{
			this.shapeName = string.Empty;
			this.shapeModel = null;
			this.modelOffset = WaterClippingPlanePlacer.DefaultModelOffset;
			this.plane = WaterClippingPlanePlacer.DisabledPlaneVec;
			this.waterFlowMask = BlockFaceFlag.All;
		}

		// Token: 0x0600B4E5 RID: 46309 RVA: 0x0043A2DE File Offset: 0x004384DE
		public void CopyFrom(ShapeSettings other)
		{
			this.shapeName = other.shapeName;
			this.shapeModel = other.shapeModel;
			this.modelOffset = other.modelOffset;
			this.plane = other.plane;
			this.waterFlowMask = other.waterFlowMask;
		}

		// Token: 0x0600B4E6 RID: 46310 RVA: 0x0043A31C File Offset: 0x0043851C
		public bool Equals(ShapeSettings other)
		{
			return !(this.plane != other.plane) && !(this.shapeName != other.shapeName) && !(this.shapeModel != other.shapeModel) && !(this.modelOffset != other.modelOffset) && this.waterFlowMask == other.waterFlowMask;
		}

		// Token: 0x04008788 RID: 34696
		public string shapeName;

		// Token: 0x04008789 RID: 34697
		public GameObject shapeModel;

		// Token: 0x0400878A RID: 34698
		public Vector3 modelOffset;

		// Token: 0x0400878B RID: 34699
		public Vector4 plane;

		// Token: 0x0400878C RID: 34700
		public BlockFaceFlag waterFlowMask = BlockFaceFlag.All;
	}
}
