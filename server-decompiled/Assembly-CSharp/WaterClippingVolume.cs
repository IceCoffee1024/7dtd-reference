using System;
using UnityEngine;

// Token: 0x0200152A RID: 5418
public class WaterClippingVolume
{
	// Token: 0x0600A9F6 RID: 43510 RVA: 0x003F41B4 File Offset: 0x003F23B4
	public void Prepare(Plane waterClipPlane)
	{
		this.waterClipPlane = waterClipPlane;
		this.isSliced = WaterClippingUtils.GetCubePlaneIntersectionEdgeLoop(waterClipPlane, ref this.intersectionPoints, out this.count);
	}

	// Token: 0x0600A9F7 RID: 43511 RVA: 0x003F41D8 File Offset: 0x003F23D8
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

	// Token: 0x04007DCA RID: 32202
	[PublicizedFrom(EAccessModifier.Private)]
	public Plane waterClipPlane;

	// Token: 0x04007DCB RID: 32203
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3[] intersectionPoints = new Vector3[6];

	// Token: 0x04007DCC RID: 32204
	[PublicizedFrom(EAccessModifier.Private)]
	public int count = -1;

	// Token: 0x04007DCD RID: 32205
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSliced;
}
