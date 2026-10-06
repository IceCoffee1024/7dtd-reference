using System;
using UnityEngine;

// Token: 0x02001324 RID: 4900
public class WorldRayHitInfo
{
	// Token: 0x06009AC8 RID: 39624 RVA: 0x003AA1B4 File Offset: 0x003A83B4
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

	// Token: 0x06009AC9 RID: 39625 RVA: 0x003AA220 File Offset: 0x003A8420
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

	// Token: 0x06009ACA RID: 39626 RVA: 0x003AA2A4 File Offset: 0x003A84A4
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

	// Token: 0x040074A2 RID: 29858
	public Ray ray;

	// Token: 0x040074A3 RID: 29859
	public bool bHitValid;

	// Token: 0x040074A4 RID: 29860
	public string tag;

	// Token: 0x040074A5 RID: 29861
	public Transform transform;

	// Token: 0x040074A6 RID: 29862
	public HitInfoDetails hit;

	// Token: 0x040074A7 RID: 29863
	public HitInfoDetails fmcHit;

	// Token: 0x040074A8 RID: 29864
	public Vector3i lastBlockPos;

	// Token: 0x040074A9 RID: 29865
	public Collider hitCollider;

	// Token: 0x040074AA RID: 29866
	public int hitTriangleIdx;
}
