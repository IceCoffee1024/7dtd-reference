using System;

// Token: 0x02000AD0 RID: 2768
public readonly struct SignBakeRequest : IComparable<SignBakeRequest>
{
	// Token: 0x0600528E RID: 21134 RVA: 0x001FA414 File Offset: 0x001F8614
	public SignBakeRequest(int groupIndex, int tier, float groupMinDistanceSquared)
	{
		this.GroupIndex = groupIndex;
		this.Tier = tier;
		this.GroupMinDistanceSquared = groupMinDistanceSquared;
	}

	// Token: 0x0600528F RID: 21135 RVA: 0x001FA42C File Offset: 0x001F862C
	public int CompareTo(SignBakeRequest other)
	{
		int num = this.Tier.CompareTo(other.Tier);
		if (num != 0)
		{
			return num;
		}
		return this.GroupMinDistanceSquared.CompareTo(other.GroupMinDistanceSquared);
	}

	// Token: 0x0400404A RID: 16458
	public readonly int GroupIndex;

	// Token: 0x0400404B RID: 16459
	public readonly int Tier;

	// Token: 0x0400404C RID: 16460
	public readonly float GroupMinDistanceSquared;
}
