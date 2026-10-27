using System;

// Token: 0x02001455 RID: 5205
public struct IntVect
{
	// Token: 0x0600A30B RID: 41739 RVA: 0x003D2E67 File Offset: 0x003D1067
	public IntVect(int x, int y, int z)
	{
		this.m_X = x;
		this.m_Y = y;
		this.m_Z = z;
	}

	// Token: 0x1700134B RID: 4939
	// (get) Token: 0x0600A30C RID: 41740 RVA: 0x003D2E7E File Offset: 0x003D107E
	public int X
	{
		get
		{
			return this.m_X;
		}
	}

	// Token: 0x1700134C RID: 4940
	// (get) Token: 0x0600A30D RID: 41741 RVA: 0x003D2E86 File Offset: 0x003D1086
	public int Y
	{
		get
		{
			return this.m_Y;
		}
	}

	// Token: 0x1700134D RID: 4941
	// (get) Token: 0x0600A30E RID: 41742 RVA: 0x003D2E8E File Offset: 0x003D108E
	public int Z
	{
		get
		{
			return this.m_Z;
		}
	}

	// Token: 0x0600A30F RID: 41743 RVA: 0x003D2E98 File Offset: 0x003D1098
	public override bool Equals(object obj)
	{
		IntVect intVect = (IntVect)obj;
		return intVect.X == this.m_X && intVect.Y == this.m_Y && intVect.Z == this.m_Z;
	}

	// Token: 0x0600A310 RID: 41744 RVA: 0x003D2EDB File Offset: 0x003D10DB
	public override int GetHashCode()
	{
		return this.m_X * 8976890 + this.m_Y * 981131 + this.m_Z;
	}

	// Token: 0x0600A311 RID: 41745 RVA: 0x003D2EFD File Offset: 0x003D10FD
	public static bool operator ==(IntVect one, IntVect other)
	{
		return one.X == other.X && one.Y == other.Y && one.Z == other.Z;
	}

	// Token: 0x0600A312 RID: 41746 RVA: 0x003D2F31 File Offset: 0x003D1131
	public static bool operator !=(IntVect one, IntVect other)
	{
		return !(one == other);
	}

	// Token: 0x04007A75 RID: 31349
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_X;

	// Token: 0x04007A76 RID: 31350
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_Y;

	// Token: 0x04007A77 RID: 31351
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_Z;
}
