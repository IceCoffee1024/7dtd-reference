using System;
using UnityEngine;

namespace WaterClippingTool
{
	// Token: 0x02001647 RID: 5703
	[Serializable]
	public class ShapeSettings : IEquatable<ShapeSettings>
	{
		// Token: 0x17001559 RID: 5465
		// (get) Token: 0x0600B2FB RID: 45819 RVA: 0x0042EE64 File Offset: 0x0042D064
		public bool hasPlane
		{
			get
			{
				return this.plane != WaterClippingPlanePlacer.DisabledPlaneVec;
			}
		}

		// Token: 0x0600B2FC RID: 45820 RVA: 0x0042EE76 File Offset: 0x0042D076
		public ShapeSettings()
		{
			this.ResetToDefault();
		}

		// Token: 0x0600B2FD RID: 45821 RVA: 0x0042EE8C File Offset: 0x0042D08C
		public void ResetToDefault()
		{
			this.shapeName = string.Empty;
			this.shapeModel = null;
			this.modelOffset = WaterClippingPlanePlacer.DefaultModelOffset;
			this.plane = WaterClippingPlanePlacer.DisabledPlaneVec;
			this.waterFlowMask = BlockFaceFlag.All;
		}

		// Token: 0x0600B2FE RID: 45822 RVA: 0x0042EEBE File Offset: 0x0042D0BE
		public void CopyFrom(ShapeSettings other)
		{
			this.shapeName = other.shapeName;
			this.shapeModel = other.shapeModel;
			this.modelOffset = other.modelOffset;
			this.plane = other.plane;
			this.waterFlowMask = other.waterFlowMask;
		}

		// Token: 0x0600B2FF RID: 45823 RVA: 0x0042EEFC File Offset: 0x0042D0FC
		public bool Equals(ShapeSettings other)
		{
			return !(this.plane != other.plane) && !(this.shapeName != other.shapeName) && !(this.shapeModel != other.shapeModel) && !(this.modelOffset != other.modelOffset) && this.waterFlowMask == other.waterFlowMask;
		}

		// Token: 0x04008618 RID: 34328
		public string shapeName;

		// Token: 0x04008619 RID: 34329
		public GameObject shapeModel;

		// Token: 0x0400861A RID: 34330
		public Vector3 modelOffset;

		// Token: 0x0400861B RID: 34331
		public Vector4 plane;

		// Token: 0x0400861C RID: 34332
		public BlockFaceFlag waterFlowMask = BlockFaceFlag.All;
	}
}
