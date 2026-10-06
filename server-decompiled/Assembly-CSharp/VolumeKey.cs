using System;

// Token: 0x02000BEE RID: 3054
public struct VolumeKey : IEquatable<VolumeKey>
{
	// Token: 0x06005B73 RID: 23411 RVA: 0x00236782 File Offset: 0x00234982
	public VolumeKey(Vector3i boxMin, Vector3i boxMax)
	{
		this.boxMin = boxMin;
		this.boxMax = boxMax;
	}

	// Token: 0x06005B74 RID: 23412 RVA: 0x00236792 File Offset: 0x00234992
	public VolumeKey(SleeperVolume volume)
	{
		this.boxMin = volume.BoxMin;
		this.boxMax = volume.BoxMax;
	}

	// Token: 0x06005B75 RID: 23413 RVA: 0x002367AC File Offset: 0x002349AC
	public VolumeKey(TriggerVolume volume)
	{
		this.boxMin = volume.BoxMin;
		this.boxMax = volume.BoxMax;
	}

	// Token: 0x06005B76 RID: 23414 RVA: 0x002367C6 File Offset: 0x002349C6
	public VolumeKey(WallVolume volume)
	{
		this.boxMin = volume.BoxMin;
		this.boxMax = volume.BoxMax;
	}

	// Token: 0x06005B77 RID: 23415 RVA: 0x002367E0 File Offset: 0x002349E0
	public override int GetHashCode()
	{
		return HashCode.Combine<int, int>(this.boxMin.GetHashCode(), this.boxMax.GetHashCode());
	}

	// Token: 0x06005B78 RID: 23416 RVA: 0x00236809 File Offset: 0x00234A09
	public bool Equals(VolumeKey other)
	{
		return other.boxMin == this.boxMin && other.boxMax == this.boxMax;
	}

	// Token: 0x06005B79 RID: 23417 RVA: 0x00236834 File Offset: 0x00234A34
	public override bool Equals(object obj)
	{
		if (obj is VolumeKey)
		{
			VolumeKey volumeKey = (VolumeKey)obj;
			return this.Equals(volumeKey);
		}
		return false;
	}

	// Token: 0x06005B7A RID: 23418 RVA: 0x00236859 File Offset: 0x00234A59
	public override string ToString()
	{
		return string.Format("({0}) ({1})", this.boxMin, this.boxMax);
	}

	// Token: 0x04004764 RID: 18276
	public Vector3i boxMin;

	// Token: 0x04004765 RID: 18277
	public Vector3i boxMax;
}
