using System;
using UnityEngine;

// Token: 0x020014F6 RID: 5366
public class WaterClippingVolume
{
	// Token: 0x0600A8EB RID: 43243 RVA: 0x003ECF5C File Offset: 0x003EB15C
	public void Prepare(Plane waterClipPlane)
	{
		this.waterClipPlane = waterClipPlane;
		this.isSliced = WaterClippingUtils.GetCubePlaneIntersectionEdgeLoop(waterClipPlane, ref this.intersectionPoints, out this.count);
	}

	// Token: 0x0600A8EC RID: 43244 RVA: 0x003ECF80 File Offset: 0x003EB180
	public void ApplyClipping(ref Vector3 vertLocalPos)
	{
		if (!this.isSliced)
		{
			return;
		}
		if (this.waterClipPlane.GetDistanceToPoint(vertLocalPos) > 0f)
		{
			vertLocalPos = this.waterClipPlane.ClosestPointOnPlane(vertLocalPos);
			if (!WaterClippingUtils.CubeBounds.Contains(vertLocalPos))
			{
				vertLocalPos = GeometryUtils.NearestPointOnEdgeLoop(vertLocalPos, this.intersectionPoints, this.count);
			}
			return;
		}
	}

	// Token: 0x04007D06 RID: 32006
	[PublicizedFrom(EAccessModifier.Private)]
	public Plane waterClipPlane;

	// Token: 0x04007D07 RID: 32007
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3[] intersectionPoints = new Vector3[6];

	// Token: 0x04007D08 RID: 32008
	[PublicizedFrom(EAccessModifier.Private)]
	public int count = -1;

	// Token: 0x04007D09 RID: 32009
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSliced;
}
