using System;

// Token: 0x02001443 RID: 5187
public struct IntVect
{
	// Token: 0x0600A2B3 RID: 41651 RVA: 0x003D0A63 File Offset: 0x003CEC63
	public IntVect(int x, int y, int z)
	{
		this.m_X = x;
		this.m_Y = y;
		this.m_Z = z;
	}

	// Token: 0x1700133F RID: 4927
	// (get) Token: 0x0600A2B4 RID: 41652 RVA: 0x003D0A7A File Offset: 0x003CEC7A
	public int X
	{
		get
		{
			return this.m_X;
		}
	}

	// Token: 0x17001340 RID: 4928
	// (get) Token: 0x0600A2B5 RID: 41653 RVA: 0x003D0A82 File Offset: 0x003CEC82
	public int Y
	{
		get
		{
			return this.m_Y;
		}
	}

	// Token: 0x17001341 RID: 4929
	// (get) Token: 0x0600A2B6 RID: 41654 RVA: 0x003D0A8A File Offset: 0x003CEC8A
	public int Z
	{
		get
		{
			return this.m_Z;
		}
	}

	// Token: 0x0600A2B7 RID: 41655 RVA: 0x003D0A94 File Offset: 0x003CEC94
	public override bool Equals(object obj)
	{
		IntVect intVect = (IntVect)obj;
		return intVect.X == this.m_X && intVect.Y == this.m_Y && intVect.Z == this.m_Z;
	}

	// Token: 0x0600A2B8 RID: 41656 RVA: 0x003D0AD7 File Offset: 0x003CECD7
	public override int GetHashCode()
	{
		return this.m_X * 8976890 + this.m_Y * 981131 + this.m_Z;
	}

	// Token: 0x0600A2B9 RID: 41657 RVA: 0x003D0AF9 File Offset: 0x003CECF9
	public static bool operator ==(IntVect one, IntVect other)
	{
		return one.X == other.X && one.Y == other.Y && one.Z == other.Z;
	}

	// Token: 0x0600A2BA RID: 41658 RVA: 0x003D0B2D File Offset: 0x003CED2D
	public static bool operator !=(IntVect one, IntVect other)
	{
		return !(one == other);
	}

	// Token: 0x04007A34 RID: 31284
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_X;

	// Token: 0x04007A35 RID: 31285
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_Y;

	// Token: 0x04007A36 RID: 31286
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_Z;
}
