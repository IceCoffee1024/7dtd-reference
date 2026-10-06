using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02001511 RID: 5393
public struct Vector2d : IEquatable<Vector2d>
{
	// Token: 0x0600A922 RID: 43298 RVA: 0x003F057E File Offset: 0x003EE77E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2d(double _x, double _y)
	{
		this.x = _x;
		this.y = _y;
	}

	// Token: 0x0600A923 RID: 43299 RVA: 0x003F058E File Offset: 0x003EE78E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2d(Vector2 _v)
	{
		this.x = (double)_v.x;
		this.y = (double)_v.y;
	}

	// Token: 0x0600A924 RID: 43300 RVA: 0x003F05AA File Offset: 0x003EE7AA
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2d(Vector2i _v)
	{
		this.x = (double)_v.x;
		this.y = (double)_v.y;
	}

	// Token: 0x0600A925 RID: 43301 RVA: 0x003F05C6 File Offset: 0x003EE7C6
	public bool Equals(double _x, double _y)
	{
		return this.x == _x && this.y == _y;
	}

	// Token: 0x0600A926 RID: 43302 RVA: 0x003F05DC File Offset: 0x003EE7DC
	public override bool Equals(object obj)
	{
		Vector2d vector2d = (Vector2d)obj;
		return this.Equals(vector2d);
	}

	// Token: 0x0600A927 RID: 43303 RVA: 0x003F05F7 File Offset: 0x003EE7F7
	public bool Equals(Vector2d other)
	{
		return other.x == this.x && other.y == this.y;
	}

	// Token: 0x0600A928 RID: 43304 RVA: 0x003F0617 File Offset: 0x003EE817
	public static bool operator ==(Vector2d one, Vector2d other)
	{
		return one.x == other.x && one.y == other.y;
	}

	// Token: 0x0600A929 RID: 43305 RVA: 0x003F0637 File Offset: 0x003EE837
	public override int GetHashCode()
	{
		return this.x.GetHashCode() ^ (this.y.GetHashCode() << 2);
	}

	// Token: 0x0600A92A RID: 43306 RVA: 0x003F0652 File Offset: 0x003EE852
	public static bool operator !=(Vector2d one, Vector2d other)
	{
		return !(one == other);
	}

	// Token: 0x0600A92B RID: 43307 RVA: 0x003F065E File Offset: 0x003EE85E
	public static Vector2d operator -(Vector2d one, Vector2d other)
	{
		return new Vector2d(one.x - other.x, one.y - other.y);
	}

	// Token: 0x0600A92C RID: 43308 RVA: 0x003F067F File Offset: 0x003EE87F
	public static Vector2d operator +(Vector2d one, Vector2d other)
	{
		return new Vector2d(one.x + other.x, one.y + other.y);
	}

	// Token: 0x0600A92D RID: 43309 RVA: 0x003F06A0 File Offset: 0x003EE8A0
	public static Vector2d operator *(Vector2d a, double d)
	{
		return new Vector2d(a.x * d, a.y * d);
	}

	// Token: 0x0600A92E RID: 43310 RVA: 0x003F06B7 File Offset: 0x003EE8B7
	public static Vector2d operator *(double d, Vector2d a)
	{
		return new Vector2d(a.x * d, a.y * d);
	}

	// Token: 0x0600A92F RID: 43311 RVA: 0x003F06CE File Offset: 0x003EE8CE
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double Dot(Vector2d lhs, Vector2d rhs)
	{
		return lhs.x * rhs.x + lhs.y * rhs.y;
	}

	// Token: 0x0600A930 RID: 43312 RVA: 0x003F06EB File Offset: 0x003EE8EB
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double Dot(Vector2 rhs)
	{
		return this.x * (double)rhs.x + this.y * (double)rhs.y;
	}

	// Token: 0x0600A931 RID: 43313 RVA: 0x003F070A File Offset: 0x003EE90A
	public override string ToString()
	{
		return this.ToCultureInvariantString();
	}

	// Token: 0x0600A932 RID: 43314 RVA: 0x003F0714 File Offset: 0x003EE914
	public string ToCultureInvariantString()
	{
		return string.Concat(new string[]
		{
			"(",
			this.x.ToCultureInvariantString("F1"),
			", ",
			this.y.ToCultureInvariantString("F1"),
			")"
		});
	}

	// Token: 0x04007D57 RID: 32087
	public double x;

	// Token: 0x04007D58 RID: 32088
	public double y;

	// Token: 0x04007D59 RID: 32089
	public static readonly Vector2d Zero = new Vector2d(0.0, 0.0);
}
