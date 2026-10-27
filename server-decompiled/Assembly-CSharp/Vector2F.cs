using System;

// Token: 0x02001524 RID: 5412
public struct Vector2F : IEquatable<Vector2F>
{
	// Token: 0x0600A98C RID: 43404 RVA: 0x003F2B8C File Offset: 0x003F0D8C
	public Vector2F(double angle)
	{
		this.X = (float)Math.Sin(angle);
		this.Y = (float)Math.Cos(angle);
	}

	// Token: 0x0600A98D RID: 43405 RVA: 0x003F2BA8 File Offset: 0x003F0DA8
	public Vector2F(float x, float y)
	{
		this.X = x;
		this.Y = y;
	}

	// Token: 0x17001404 RID: 5124
	// (get) Token: 0x0600A98E RID: 43406 RVA: 0x003F2BB8 File Offset: 0x003F0DB8
	public double Length
	{
		get
		{
			return Math.Sqrt((double)(this.X * this.X + this.Y * this.Y));
		}
	}

	// Token: 0x0600A98F RID: 43407 RVA: 0x003F2BDB File Offset: 0x003F0DDB
	public static double Lengthsquared(Vector2F a)
	{
		return (double)(a.X * a.X + a.Y * a.Y);
	}

	// Token: 0x0600A990 RID: 43408 RVA: 0x003F2BF9 File Offset: 0x003F0DF9
	public static Vector2F operator -(Vector2F a, Vector2F b)
	{
		return new Vector2F(a.X - b.X, a.Y - b.Y);
	}

	// Token: 0x0600A991 RID: 43409 RVA: 0x003F2C1A File Offset: 0x003F0E1A
	public static bool operator ==(Vector2F a, Vector2F b)
	{
		return a.X == b.X && a.Y == b.Y;
	}

	// Token: 0x0600A992 RID: 43410 RVA: 0x003F2C3A File Offset: 0x003F0E3A
	public static bool operator !=(Vector2F a, Vector2F b)
	{
		return a.X != b.X || a.Y != b.Y;
	}

	// Token: 0x0600A993 RID: 43411 RVA: 0x003F2C5D File Offset: 0x003F0E5D
	public override int GetHashCode()
	{
		return this.X.GetHashCode() * this.Y.GetHashCode();
	}

	// Token: 0x0600A994 RID: 43412 RVA: 0x003F2BDB File Offset: 0x003F0DDB
	public double Lengthsquared()
	{
		return (double)(this.X * this.X + this.Y * this.Y);
	}

	// Token: 0x0600A995 RID: 43413 RVA: 0x003F2C76 File Offset: 0x003F0E76
	public override bool Equals(object obj)
	{
		return this == (Vector2F)obj;
	}

	// Token: 0x0600A996 RID: 43414 RVA: 0x003F2C89 File Offset: 0x003F0E89
	public bool Equals(Vector2F other)
	{
		return other.X == this.X && other.Y == this.Y;
	}

	// Token: 0x0600A997 RID: 43415 RVA: 0x003F2CA9 File Offset: 0x003F0EA9
	public override string ToString()
	{
		return this.X.ToCultureInvariantString() + ", " + this.Y.ToCultureInvariantString();
	}

	// Token: 0x04007D9B RID: 32155
	public float Y;

	// Token: 0x04007D9C RID: 32156
	public float X;

	// Token: 0x04007D9D RID: 32157
	public static readonly Vector2F Zero = new Vector2F(0f, 0f);
}
