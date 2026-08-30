using System;

// Token: 0x02000BDC RID: 3036
public struct VolumeKey : IEquatable<VolumeKey>
{
	// Token: 0x06005B5A RID: 23386 RVA: 0x00233486 File Offset: 0x00231686
	public VolumeKey(Vector3i boxMin, Vector3i boxMax)
	{
		this.boxMin = boxMin;
		this.boxMax = boxMax;
	}

	// Token: 0x06005B5B RID: 23387 RVA: 0x00233496 File Offset: 0x00231696
	public VolumeKey(SleeperVolume volume)
	{
		this.boxMin = volume.BoxMin;
		this.boxMax = volume.BoxMax;
	}

	// Token: 0x06005B5C RID: 23388 RVA: 0x002334B0 File Offset: 0x002316B0
	public VolumeKey(TriggerVolume volume)
	{
		this.boxMin = volume.BoxMin;
		this.boxMax = volume.BoxMax;
	}

	// Token: 0x06005B5D RID: 23389 RVA: 0x002334CA File Offset: 0x002316CA
	public VolumeKey(WallVolume volume)
	{
		this.boxMin = volume.BoxMin;
		this.boxMax = volume.BoxMax;
	}

	// Token: 0x06005B5E RID: 23390 RVA: 0x002334E4 File Offset: 0x002316E4
	public override int GetHashCode()
	{
		return HashCode.Combine<int, int>(this.boxMin.GetHashCode(), this.boxMax.GetHashCode());
	}

	// Token: 0x06005B5F RID: 23391 RVA: 0x0023350D File Offset: 0x0023170D
	public bool Equals(VolumeKey other)
	{
		return other.boxMin == this.boxMin && other.boxMax == this.boxMax;
	}

	// Token: 0x06005B60 RID: 23392 RVA: 0x00233538 File Offset: 0x00231738
	public override bool Equals(object obj)
	{
		if (obj is VolumeKey)
		{
			VolumeKey volumeKey = (VolumeKey)obj;
			return this.Equals(volumeKey);
		}
		return false;
	}

	// Token: 0x06005B61 RID: 23393 RVA: 0x0023355D File Offset: 0x0023175D
	public override string ToString()
	{
		return string.Format("({0}) ({1})", this.boxMin, this.boxMax);
	}

	// Token: 0x04004725 RID: 18213
	public Vector3i boxMin;

	// Token: 0x04004726 RID: 18214
	public Vector3i boxMax;
}
