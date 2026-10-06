using System;

// Token: 0x02001512 RID: 5394
public struct Vector2F : IEquatable<Vector2F>
{
	// Token: 0x0600A934 RID: 43316 RVA: 0x003F0788 File Offset: 0x003EE988
	public Vector2F(double angle)
	{
		this.X = (float)Math.Sin(angle);
		this.Y = (float)Math.Cos(angle);
	}

	// Token: 0x0600A935 RID: 43317 RVA: 0x003F07A4 File Offset: 0x003EE9A4
	public Vector2F(float x, float y)
	{
		this.X = x;
		this.Y = y;
	}

	// Token: 0x170013F8 RID: 5112
	// (get) Token: 0x0600A936 RID: 43318 RVA: 0x003F07B4 File Offset: 0x003EE9B4
	public double Length
	{
		get
		{
			return Math.Sqrt((double)(this.X * this.X + this.Y * this.Y));
		}
	}

	// Token: 0x0600A937 RID: 43319 RVA: 0x003F07D7 File Offset: 0x003EE9D7
	public static double Lengthsquared(Vector2F a)
	{
		return (double)(a.X * a.X + a.Y * a.Y);
	}

	// Token: 0x0600A938 RID: 43320 RVA: 0x003F07F5 File Offset: 0x003EE9F5
	public static Vector2F operator -(Vector2F a, Vector2F b)
	{
		return new Vector2F(a.X - b.X, a.Y - b.Y);
	}

	// Token: 0x0600A939 RID: 43321 RVA: 0x003F0816 File Offset: 0x003EEA16
	public static bool operator ==(Vector2F a, Vector2F b)
	{
		return a.X == b.X && a.Y == b.Y;
	}

	// Token: 0x0600A93A RID: 43322 RVA: 0x003F0836 File Offset: 0x003EEA36
	public static bool operator !=(Vector2F a, Vector2F b)
	{
		return a.X != b.X || a.Y != b.Y;
	}

	// Token: 0x0600A93B RID: 43323 RVA: 0x003F0859 File Offset: 0x003EEA59
	public override int GetHashCode()
	{
		return this.X.GetHashCode() * this.Y.GetHashCode();
	}

	// Token: 0x0600A93C RID: 43324 RVA: 0x003F07D7 File Offset: 0x003EE9D7
	public double Lengthsquared()
	{
		return (double)(this.X * this.X + this.Y * this.Y);
	}

	// Token: 0x0600A93D RID: 43325 RVA: 0x003F0872 File Offset: 0x003EEA72
	public override bool Equals(object obj)
	{
		return this == (Vector2F)obj;
	}

	// Token: 0x0600A93E RID: 43326 RVA: 0x003F0885 File Offset: 0x003EEA85
	public bool Equals(Vector2F other)
	{
		return other.X == this.X && other.Y == this.Y;
	}

	// Token: 0x0600A93F RID: 43327 RVA: 0x003F08A5 File Offset: 0x003EEAA5
	public override string ToString()
	{
		return this.X.ToCultureInvariantString() + ", " + this.Y.ToCultureInvariantString();
	}

	// Token: 0x04007D5A RID: 32090
	public float Y;

	// Token: 0x04007D5B RID: 32091
	public float X;

	// Token: 0x04007D5C RID: 32092
	public static readonly Vector2F Zero = new Vector2F(0f, 0f);
}
