using System;

// Token: 0x02000ADA RID: 2778
public readonly struct SignBakeRequest : IComparable<SignBakeRequest>
{
	// Token: 0x060052B2 RID: 21170 RVA: 0x001FB4F0 File Offset: 0x001F96F0
	public SignBakeRequest(int groupIndex, int tier, float groupMinDistanceSquared)
	{
		this.GroupIndex = groupIndex;
		this.Tier = tier;
		this.GroupMinDistanceSquared = groupMinDistanceSquared;
	}

	// Token: 0x060052B3 RID: 21171 RVA: 0x001FB508 File Offset: 0x001F9708
	public int CompareTo(SignBakeRequest other)
	{
		int num = this.Tier.CompareTo(other.Tier);
		if (num != 0)
		{
			return num;
		}
		return this.GroupMinDistanceSquared.CompareTo(other.GroupMinDistanceSquared);
	}

	// Token: 0x0400406B RID: 16491
	public readonly int GroupIndex;

	// Token: 0x0400406C RID: 16492
	public readonly int Tier;

	// Token: 0x0400406D RID: 16493
	public readonly float GroupMinDistanceSquared;
}
