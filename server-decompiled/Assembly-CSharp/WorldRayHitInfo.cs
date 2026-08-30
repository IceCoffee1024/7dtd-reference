using System;
using UnityEngine;

// Token: 0x02001304 RID: 4868
public class WorldRayHitInfo
{
	// Token: 0x06009A1C RID: 39452 RVA: 0x003A5604 File Offset: 0x003A3804
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

	// Token: 0x06009A1D RID: 39453 RVA: 0x003A5670 File Offset: 0x003A3870
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

	// Token: 0x06009A1E RID: 39454 RVA: 0x003A56F4 File Offset: 0x003A38F4
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

	// Token: 0x04007427 RID: 29735
	public Ray ray;

	// Token: 0x04007428 RID: 29736
	public bool bHitValid;

	// Token: 0x04007429 RID: 29737
	public string tag;

	// Token: 0x0400742A RID: 29738
	public Transform transform;

	// Token: 0x0400742B RID: 29739
	public HitInfoDetails hit;

	// Token: 0x0400742C RID: 29740
	public HitInfoDetails fmcHit;

	// Token: 0x0400742D RID: 29741
	public Vector3i lastBlockPos;

	// Token: 0x0400742E RID: 29742
	public Collider hitCollider;

	// Token: 0x0400742F RID: 29743
	public int hitTriangleIdx;
}
