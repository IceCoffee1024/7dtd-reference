using System;
using UnityEngine;

// Token: 0x02001336 RID: 4918
public class WorldRayHitInfo
{
	// Token: 0x06009B20 RID: 39712 RVA: 0x003AC5B8 File Offset: 0x003AA7B8
	public virtual void Clear()
	{
		this.ray = new Ray(Vector3.zero, Vector3.zero);
		this.bHitValid = false;
		this.tag = string.Empty;
		this.transform = null;
		this.lastBlockPos = Vector3i.zero;
		this.hit.Clear();
		this.fmcHit.Clear();
		this.hitCollider = null;
		this.hitTriangleIdx = 0;
	}

	// Token: 0x06009B21 RID: 39713 RVA: 0x003AC624 File Offset: 0x003AA824
	public virtual void CopyFrom(WorldRayHitInfo _other)
	{
		this.ray = _other.ray;
		this.bHitValid = _other.bHitValid;
		this.tag = _other.tag;
		this.transform = _other.transform;
		this.lastBlockPos = _other.lastBlockPos;
		this.hit.CopyFrom(_other.hit);
		this.fmcHit.CopyFrom(_other.fmcHit);
		this.hitCollider = _other.hitCollider;
		this.hitTriangleIdx = _other.hitTriangleIdx;
	}

	// Token: 0x06009B22 RID: 39714 RVA: 0x003AC6A8 File Offset: 0x003AA8A8
	public virtual WorldRayHitInfo Clone()
	{
		return new WorldRayHitInfo
		{
			ray = this.ray,
			bHitValid = this.bHitValid,
			tag = this.tag,
			transform = this.transform,
			lastBlockPos = this.lastBlockPos,
			hit = this.hit.Clone(),
			fmcHit = this.fmcHit.Clone(),
			hitCollider = this.hitCollider,
			hitTriangleIdx = this.hitTriangleIdx
		};
	}

	// Token: 0x040074E3 RID: 29923
	public Ray ray;

	// Token: 0x040074E4 RID: 29924
	public bool bHitValid;

	// Token: 0x040074E5 RID: 29925
	public string tag;

	// Token: 0x040074E6 RID: 29926
	public Transform transform;

	// Token: 0x040074E7 RID: 29927
	public HitInfoDetails hit;

	// Token: 0x040074E8 RID: 29928
	public HitInfoDetails fmcHit;

	// Token: 0x040074E9 RID: 29929
	public Vector3i lastBlockPos;

	// Token: 0x040074EA RID: 29930
	public Collider hitCollider;

	// Token: 0x040074EB RID: 29931
	public int hitTriangleIdx;
}
