using System;

// Token: 0x02000BF9 RID: 3065
public struct VolumeKey : IEquatable<VolumeKey>
{
	// Token: 0x06005B98 RID: 23448 RVA: 0x00237CCE File Offset: 0x00235ECE
	public VolumeKey(Vector3i boxMin, Vector3i boxMax)
	{
		this.boxMin = boxMin;
		this.boxMax = boxMax;
	}

	// Token: 0x06005B99 RID: 23449 RVA: 0x00237CDE File Offset: 0x00235EDE
	public VolumeKey(SleeperVolume volume)
	{
		this.boxMin = volume.BoxMin;
		this.boxMax = volume.BoxMax;
	}

	// Token: 0x06005B9A RID: 23450 RVA: 0x00237CF8 File Offset: 0x00235EF8
	public VolumeKey(TriggerVolume volume)
	{
		this.boxMin = volume.BoxMin;
		this.boxMax = volume.BoxMax;
	}

	// Token: 0x06005B9B RID: 23451 RVA: 0x00237D12 File Offset: 0x00235F12
	public VolumeKey(WallVolume volume)
	{
		this.boxMin = volume.BoxMin;
		this.boxMax = volume.BoxMax;
	}

	// Token: 0x06005B9C RID: 23452 RVA: 0x00237D2C File Offset: 0x00235F2C
	public override int GetHashCode()
	{
		return HashCode.Combine<int, int>(this.boxMin.GetHashCode(), this.boxMax.GetHashCode());
	}

	// Token: 0x06005B9D RID: 23453 RVA: 0x00237D55 File Offset: 0x00235F55
	public bool Equals(VolumeKey other)
	{
		return other.boxMin == this.boxMin && other.boxMax == this.boxMax;
	}

	// Token: 0x06005B9E RID: 23454 RVA: 0x00237D80 File Offset: 0x00235F80
	public override bool Equals(object obj)
	{
		if (obj is VolumeKey)
		{
			VolumeKey volumeKey = (VolumeKey)obj;
			return this.Equals(volumeKey);
		}
		return false;
	}

	// Token: 0x06005B9F RID: 23455 RVA: 0x00237DA5 File Offset: 0x00235FA5
	public override string ToString()
	{
		return string.Format("({0}) ({1})", this.boxMin, this.boxMax);
	}

	// Token: 0x04004789 RID: 18313
	public Vector3i boxMin;

	// Token: 0x0400478A RID: 18314
	public Vector3i boxMax;
}
