using System;

// Token: 0x02000AAD RID: 2733
public readonly struct SignBakeRequest : IComparable<SignBakeRequest>
{
	// Token: 0x060051EF RID: 20975 RVA: 0x001F4780 File Offset: 0x001F2980
	public SignBakeRequest(int groupIndex, int tier, float groupMinDistanceSquared)
	{
		this.GroupIndex = groupIndex;
		this.Tier = tier;
		this.GroupMinDistanceSquared = groupMinDistanceSquared;
	}

	// Token: 0x060051F0 RID: 20976 RVA: 0x001F4798 File Offset: 0x001F2998
	public int CompareTo(SignBakeRequest other)
	{
		int num = this.Tier.CompareTo(other.Tier);
		if (num != 0)
		{
			return num;
		}
		return this.GroupMinDistanceSquared.CompareTo(other.GroupMinDistanceSquared);
	}

	// Token: 0x04003F7C RID: 16252
	public readonly int GroupIndex;

	// Token: 0x04003F7D RID: 16253
	public readonly int Tier;

	// Token: 0x04003F7E RID: 16254
	public readonly float GroupMinDistanceSquared;
}
