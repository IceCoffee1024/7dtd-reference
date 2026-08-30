using System;

// Token: 0x020014D5 RID: 5333
public struct Vector2F : IEquatable<Vector2F>
{
	// Token: 0x0600A784 RID: 42884 RVA: 0x003E65F4 File Offset: 0x003E47F4
	public Vector2F(double angle)
	{
		this.X = (float)Math.Sin(angle);
		this.Y = (float)Math.Cos(angle);
	}

	// Token: 0x0600A785 RID: 42885 RVA: 0x003E6610 File Offset: 0x003E4810
	public Vector2F(float x, float y)
	{
		this.X = x;
		this.Y = y;
	}

	// Token: 0x1700137A RID: 4986
	// (get) Token: 0x0600A786 RID: 42886 RVA: 0x003E6620 File Offset: 0x003E4820
	public double Length
	{
		get
		{
			return Math.Sqrt((double)(this.X * this.X + this.Y * this.Y));
		}
	}

	// Token: 0x0600A787 RID: 42887 RVA: 0x003E6643 File Offset: 0x003E4843
	public static double Lengthsquared(Vector2F a)
	{
		return (double)(a.X * a.X + a.Y * a.Y);
	}

	// Token: 0x0600A788 RID: 42888 RVA: 0x003E6661 File Offset: 0x003E4861
	public static Vector2F operator -(Vector2F a, Vector2F b)
	{
		return new Vector2F(a.X - b.X, a.Y - b.Y);
	}

	// Token: 0x0600A789 RID: 42889 RVA: 0x003E6682 File Offset: 0x003E4882
	public static bool operator ==(Vector2F a, Vector2F b)
	{
		return a.X == b.X && a.Y == b.Y;
	}

	// Token: 0x0600A78A RID: 42890 RVA: 0x003E66A2 File Offset: 0x003E48A2
	public static bool operator !=(Vector2F a, Vector2F b)
	{
		return a.X != b.X || a.Y != b.Y;
	}

	// Token: 0x0600A78B RID: 42891 RVA: 0x003E66C5 File Offset: 0x003E48C5
	public override int GetHashCode()
	{
		return this.X.GetHashCode() * this.Y.GetHashCode();
	}

	// Token: 0x0600A78C RID: 42892 RVA: 0x003E6643 File Offset: 0x003E4843
	public double Lengthsquared()
	{
		return (double)(this.X * this.X + this.Y * this.Y);
	}

	// Token: 0x0600A78D RID: 42893 RVA: 0x003E66DE File Offset: 0x003E48DE
	public override bool Equals(object obj)
	{
		return this == (Vector2F)obj;
	}

	// Token: 0x0600A78E RID: 42894 RVA: 0x003E66F1 File Offset: 0x003E48F1
	public bool Equals(Vector2F other)
	{
		return other.X == this.X && other.Y == this.Y;
	}

	// Token: 0x0600A78F RID: 42895 RVA: 0x003E6711 File Offset: 0x003E4911
	public override string ToString()
	{
		return this.X.ToCultureInvariantString() + ", " + this.Y.ToCultureInvariantString();
	}

	// Token: 0x04007BFE RID: 31742
	public float Y;

	// Token: 0x04007BFF RID: 31743
	public float X;

	// Token: 0x04007C00 RID: 31744
	public static readonly Vector2F Zero = new Vector2F(0f, 0f);
}
