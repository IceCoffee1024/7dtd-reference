using System;

// Token: 0x02001423 RID: 5155
public struct IntVect
{
	// Token: 0x0600A208 RID: 41480 RVA: 0x003CBEC7 File Offset: 0x003CA0C7
	public IntVect(int x, int y, int z)
	{
		this.m_X = x;
		this.m_Y = y;
		this.m_Z = z;
	}

	// Token: 0x170012FA RID: 4858
	// (get) Token: 0x0600A209 RID: 41481 RVA: 0x003CBEDE File Offset: 0x003CA0DE
	public int X
	{
		get
		{
			return this.m_X;
		}
	}

	// Token: 0x170012FB RID: 4859
	// (get) Token: 0x0600A20A RID: 41482 RVA: 0x003CBEE6 File Offset: 0x003CA0E6
	public int Y
	{
		get
		{
			return this.m_Y;
		}
	}

	// Token: 0x170012FC RID: 4860
	// (get) Token: 0x0600A20B RID: 41483 RVA: 0x003CBEEE File Offset: 0x003CA0EE
	public int Z
	{
		get
		{
			return this.m_Z;
		}
	}

	// Token: 0x0600A20C RID: 41484 RVA: 0x003CBEF8 File Offset: 0x003CA0F8
	public override bool Equals(object obj)
	{
		IntVect intVect = (IntVect)obj;
		return intVect.X == this.m_X && intVect.Y == this.m_Y && intVect.Z == this.m_Z;
	}

	// Token: 0x0600A20D RID: 41485 RVA: 0x003CBF3B File Offset: 0x003CA13B
	public override int GetHashCode()
	{
		return this.m_X * 8976890 + this.m_Y * 981131 + this.m_Z;
	}

	// Token: 0x0600A20E RID: 41486 RVA: 0x003CBF5D File Offset: 0x003CA15D
	public static bool operator ==(IntVect one, IntVect other)
	{
		return one.X == other.X && one.Y == other.Y && one.Z == other.Z;
	}

	// Token: 0x0600A20F RID: 41487 RVA: 0x003CBF91 File Offset: 0x003CA191
	public static bool operator !=(IntVect one, IntVect other)
	{
		return !(one == other);
	}

	// Token: 0x040079B9 RID: 31161
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_X;

	// Token: 0x040079BA RID: 31162
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_Y;

	// Token: 0x040079BB RID: 31163
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_Z;
}
