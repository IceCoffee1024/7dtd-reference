using System;

// Token: 0x02000BCA RID: 3018
public struct VolumeKey : IEquatable<VolumeKey>
{
	// Token: 0x06005AD1 RID: 23249 RVA: 0x0023076A File Offset: 0x0022E96A
	public VolumeKey(Vector3i boxMin, Vector3i boxMax)
	{
		this.boxMin = boxMin;
		this.boxMax = boxMax;
	}

	// Token: 0x06005AD2 RID: 23250 RVA: 0x0023077A File Offset: 0x0022E97A
	public VolumeKey(SleeperVolume volume)
	{
		this.boxMin = volume.BoxMin;
		this.boxMax = volume.BoxMax;
	}

	// Token: 0x06005AD3 RID: 23251 RVA: 0x00230794 File Offset: 0x0022E994
	public VolumeKey(TriggerVolume volume)
	{
		this.boxMin = volume.BoxMin;
		this.boxMax = volume.BoxMax;
	}

	// Token: 0x06005AD4 RID: 23252 RVA: 0x002307AE File Offset: 0x0022E9AE
	public VolumeKey(WallVolume volume)
	{
		this.boxMin = volume.BoxMin;
		this.boxMax = volume.BoxMax;
	}

	// Token: 0x06005AD5 RID: 23253 RVA: 0x002307C8 File Offset: 0x0022E9C8
	public override int GetHashCode()
	{
		return HashCode.Combine<int, int>(this.boxMin.GetHashCode(), this.boxMax.GetHashCode());
	}

	// Token: 0x06005AD6 RID: 23254 RVA: 0x002307F1 File Offset: 0x0022E9F1
	public bool Equals(VolumeKey other)
	{
		return other.boxMin == this.boxMin && other.boxMax == this.boxMax;
	}

	// Token: 0x06005AD7 RID: 23255 RVA: 0x0023081C File Offset: 0x0022EA1C
	public override bool Equals(object obj)
	{
		if (obj is VolumeKey)
		{
			VolumeKey volumeKey = (VolumeKey)obj;
			return this.Equals(volumeKey);
		}
		return false;
	}

	// Token: 0x06005AD8 RID: 23256 RVA: 0x00230841 File Offset: 0x0022EA41
	public override string ToString()
	{
		return string.Format("({0}) ({1})", this.boxMin, this.boxMax);
	}

	// Token: 0x04004689 RID: 18057
	public Vector3i boxMin;

	// Token: 0x0400468A RID: 18058
	public Vector3i boxMax;
}
