using System;

// Token: 0x020014F0 RID: 5360
public struct Vector2F : IEquatable<Vector2F>
{
	// Token: 0x0600A881 RID: 43137 RVA: 0x003EB934 File Offset: 0x003E9B34
	public Vector2F(double angle)
	{
		this.X = (float)Math.Sin(angle);
		this.Y = (float)Math.Cos(angle);
	}

	// Token: 0x0600A882 RID: 43138 RVA: 0x003EB950 File Offset: 0x003E9B50
	public Vector2F(float x, float y)
	{
		this.X = x;
		this.Y = y;
	}

	// Token: 0x170013B3 RID: 5043
	// (get) Token: 0x0600A883 RID: 43139 RVA: 0x003EB960 File Offset: 0x003E9B60
	public double Length
	{
		get
		{
			return Math.Sqrt((double)(this.X * this.X + this.Y * this.Y));
		}
	}

	// Token: 0x0600A884 RID: 43140 RVA: 0x003EB983 File Offset: 0x003E9B83
	public static double Lengthsquared(Vector2F a)
	{
		return (double)(a.X * a.X + a.Y * a.Y);
	}

	// Token: 0x0600A885 RID: 43141 RVA: 0x003EB9A1 File Offset: 0x003E9BA1
	public static Vector2F operator -(Vector2F a, Vector2F b)
	{
		return new Vector2F(a.X - b.X, a.Y - b.Y);
	}

	// Token: 0x0600A886 RID: 43142 RVA: 0x003EB9C2 File Offset: 0x003E9BC2
	public static bool operator ==(Vector2F a, Vector2F b)
	{
		return a.X == b.X && a.Y == b.Y;
	}

	// Token: 0x0600A887 RID: 43143 RVA: 0x003EB9E2 File Offset: 0x003E9BE2
	public static bool operator !=(Vector2F a, Vector2F b)
	{
		return a.X != b.X || a.Y != b.Y;
	}

	// Token: 0x0600A888 RID: 43144 RVA: 0x003EBA05 File Offset: 0x003E9C05
	public override int GetHashCode()
	{
		return this.X.GetHashCode() * this.Y.GetHashCode();
	}

	// Token: 0x0600A889 RID: 43145 RVA: 0x003EB983 File Offset: 0x003E9B83
	public double Lengthsquared()
	{
		return (double)(this.X * this.X + this.Y * this.Y);
	}

	// Token: 0x0600A88A RID: 43146 RVA: 0x003EBA1E File Offset: 0x003E9C1E
	public override bool Equals(object obj)
	{
		return this == (Vector2F)obj;
	}

	// Token: 0x0600A88B RID: 43147 RVA: 0x003EBA31 File Offset: 0x003E9C31
	public bool Equals(Vector2F other)
	{
		return other.X == this.X && other.Y == this.Y;
	}

	// Token: 0x0600A88C RID: 43148 RVA: 0x003EBA51 File Offset: 0x003E9C51
	public override string ToString()
	{
		return this.X.ToCultureInvariantString() + ", " + this.Y.ToCultureInvariantString();
	}

	// Token: 0x04007CD7 RID: 31959
	public float Y;

	// Token: 0x04007CD8 RID: 31960
	public float X;

	// Token: 0x04007CD9 RID: 31961
	public static readonly Vector2F Zero = new Vector2F(0f, 0f);
}
