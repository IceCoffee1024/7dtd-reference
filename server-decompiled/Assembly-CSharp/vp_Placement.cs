using System;
using UnityEngine;

// Token: 0x020015F2 RID: 5618
public class vp_Placement
{
	// Token: 0x0600AF30 RID: 44848 RVA: 0x00415324 File Offset: 0x00413524
	public static bool AdjustPosition(vp_Placement p, float physicsRadius, int attempts = 1000)
	{
		attempts--;
		if (attempts > 0)
		{
			if (p.IsObstructed(physicsRadius))
			{
				Vector3 insideUnitSphere = global::UnityEngine.Random.insideUnitSphere;
				p.Position.x = p.Position.x + insideUnitSphere.x;
				p.Position.z = p.Position.z + insideUnitSphere.z;
				vp_Placement.AdjustPosition(p, physicsRadius, attempts);
			}
			return true;
		}
		Debug.LogWarning("(vp_Placement.AdjustPosition) Failed to find valid placement.");
		return false;
	}

	// Token: 0x0600AF31 RID: 44849 RVA: 0x0041538B File Offset: 0x0041358B
	public virtual bool IsObstructed(float physicsRadius = 1f)
	{
		return Physics.CheckSphere(this.Position, physicsRadius, 2261000);
	}

	// Token: 0x0600AF32 RID: 44850 RVA: 0x004153A4 File Offset: 0x004135A4
	public static void SnapToGround(vp_Placement p, float radius, float snapDistance)
	{
		if (snapDistance == 0f)
		{
			return;
		}
		RaycastHit raycastHit;
		Physics.SphereCast(new Ray(p.Position + Vector3.up * snapDistance, Vector3.down), radius, out raycastHit, snapDistance * 2f, 1084850184);
		if (raycastHit.collider != null)
		{
			p.Position.y = raycastHit.point.y + 0.05f;
		}
	}

	// Token: 0x04008295 RID: 33429
	public Vector3 Position = Vector3.zero;

	// Token: 0x04008296 RID: 33430
	public Quaternion Rotation = Quaternion.identity;
}
