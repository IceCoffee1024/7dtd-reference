using System;

// Token: 0x02001408 RID: 5128
public struct IntVect
{
	// Token: 0x0600A10B RID: 41227 RVA: 0x003C6B83 File Offset: 0x003C4D83
	public IntVect(int x, int y, int z)
	{
		this.m_X = x;
		this.m_Y = y;
		this.m_Z = z;
	}

	// Token: 0x170012C1 RID: 4801
	// (get) Token: 0x0600A10C RID: 41228 RVA: 0x003C6B9A File Offset: 0x003C4D9A
	public int X
	{
		get
		{
			return this.m_X;
		}
	}

	// Token: 0x170012C2 RID: 4802
	// (get) Token: 0x0600A10D RID: 41229 RVA: 0x003C6BA2 File Offset: 0x003C4DA2
	public int Y
	{
		get
		{
			return this.m_Y;
		}
	}

	// Token: 0x170012C3 RID: 4803
	// (get) Token: 0x0600A10E RID: 41230 RVA: 0x003C6BAA File Offset: 0x003C4DAA
	public int Z
	{
		get
		{
			return this.m_Z;
		}
	}

	// Token: 0x0600A10F RID: 41231 RVA: 0x003C6BB4 File Offset: 0x003C4DB4
	public override bool Equals(object obj)
	{
		IntVect intVect = (IntVect)obj;
		return intVect.X == this.m_X && intVect.Y == this.m_Y && intVect.Z == this.m_Z;
	}

	// Token: 0x0600A110 RID: 41232 RVA: 0x003C6BF7 File Offset: 0x003C4DF7
	public override int GetHashCode()
	{
		return this.m_X * 8976890 + this.m_Y * 981131 + this.m_Z;
	}

	// Token: 0x0600A111 RID: 41233 RVA: 0x003C6C19 File Offset: 0x003C4E19
	public static bool operator ==(IntVect one, IntVect other)
	{
		return one.X == other.X && one.Y == other.Y && one.Z == other.Z;
	}

	// Token: 0x0600A112 RID: 41234 RVA: 0x003C6C4D File Offset: 0x003C4E4D
	public static bool operator !=(IntVect one, IntVect other)
	{
		return !(one == other);
	}

	// Token: 0x040078E0 RID: 30944
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_X;

	// Token: 0x040078E1 RID: 30945
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_Y;

	// Token: 0x040078E2 RID: 30946
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_Z;
}
