using System;

// Token: 0x02000ABE RID: 2750
public readonly struct SignBakeRequest : IComparable<SignBakeRequest>
{
	// Token: 0x06005275 RID: 21109 RVA: 0x001F7218 File Offset: 0x001F5418
	public SignBakeRequest(int groupIndex, int tier, float groupMinDistanceSquared)
	{
		this.GroupIndex = groupIndex;
		this.Tier = tier;
		this.GroupMinDistanceSquared = groupMinDistanceSquared;
	}

	// Token: 0x06005276 RID: 21110 RVA: 0x001F7230 File Offset: 0x001F5430
	public int CompareTo(SignBakeRequest other)
	{
		int num = this.Tier.CompareTo(other.Tier);
		if (num != 0)
		{
			return num;
		}
		return this.GroupMinDistanceSquared.CompareTo(other.GroupMinDistanceSquared);
	}

	// Token: 0x0400400D RID: 16397
	public readonly int GroupIndex;

	// Token: 0x0400400E RID: 16398
	public readonly int Tier;

	// Token: 0x0400400F RID: 16399
	public readonly float GroupMinDistanceSquared;
}
