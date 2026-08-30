using System;
using UnityEngine;

// Token: 0x020012EA RID: 4842
public class WorldRayHitInfo
{
	// Token: 0x06009921 RID: 39201 RVA: 0x003A036C File Offset: 0x0039E56C
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

	// Token: 0x06009922 RID: 39202 RVA: 0x003A03D8 File Offset: 0x0039E5D8
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

	// Token: 0x06009923 RID: 39203 RVA: 0x003A045C File Offset: 0x0039E65C
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

	// Token: 0x0400734F RID: 29519
	public Ray ray;

	// Token: 0x04007350 RID: 29520
	public bool bHitValid;

	// Token: 0x04007351 RID: 29521
	public string tag;

	// Token: 0x04007352 RID: 29522
	public Transform transform;

	// Token: 0x04007353 RID: 29523
	public HitInfoDetails hit;

	// Token: 0x04007354 RID: 29524
	public HitInfoDetails fmcHit;

	// Token: 0x04007355 RID: 29525
	public Vector3i lastBlockPos;

	// Token: 0x04007356 RID: 29526
	public Collider hitCollider;

	// Token: 0x04007357 RID: 29527
	public int hitTriangleIdx;
}
