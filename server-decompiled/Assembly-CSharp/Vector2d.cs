using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020014EF RID: 5359
public struct Vector2d : IEquatable<Vector2d>
{
	// Token: 0x0600A86F RID: 43119 RVA: 0x003EB72A File Offset: 0x003E992A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2d(double _x, double _y)
	{
		this.x = _x;
		this.y = _y;
	}

	// Token: 0x0600A870 RID: 43120 RVA: 0x003EB73A File Offset: 0x003E993A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2d(Vector2 _v)
	{
		this.x = (double)_v.x;
		this.y = (double)_v.y;
	}

	// Token: 0x0600A871 RID: 43121 RVA: 0x003EB756 File Offset: 0x003E9956
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2d(Vector2i _v)
	{
		this.x = (double)_v.x;
		this.y = (double)_v.y;
	}

	// Token: 0x0600A872 RID: 43122 RVA: 0x003EB772 File Offset: 0x003E9972
	public bool Equals(double _x, double _y)
	{
		return this.x == _x && this.y == _y;
	}

	// Token: 0x0600A873 RID: 43123 RVA: 0x003EB788 File Offset: 0x003E9988
	public override bool Equals(object obj)
	{
		Vector2d vector2d = (Vector2d)obj;
		return this.Equals(vector2d);
	}

	// Token: 0x0600A874 RID: 43124 RVA: 0x003EB7A3 File Offset: 0x003E99A3
	public bool Equals(Vector2d other)
	{
		return other.x == this.x && other.y == this.y;
	}

	// Token: 0x0600A875 RID: 43125 RVA: 0x003EB7C3 File Offset: 0x003E99C3
	public static bool operator ==(Vector2d one, Vector2d other)
	{
		return one.x == other.x && one.y == other.y;
	}

	// Token: 0x0600A876 RID: 43126 RVA: 0x003EB7E3 File Offset: 0x003E99E3
	public override int GetHashCode()
	{
		return this.x.GetHashCode() ^ (this.y.GetHashCode() << 2);
	}

	// Token: 0x0600A877 RID: 43127 RVA: 0x003EB7FE File Offset: 0x003E99FE
	public static bool operator !=(Vector2d one, Vector2d other)
	{
		return !(one == other);
	}

	// Token: 0x0600A878 RID: 43128 RVA: 0x003EB80A File Offset: 0x003E9A0A
	public static Vector2d operator -(Vector2d one, Vector2d other)
	{
		return new Vector2d(one.x - other.x, one.y - other.y);
	}

	// Token: 0x0600A879 RID: 43129 RVA: 0x003EB82B File Offset: 0x003E9A2B
	public static Vector2d operator +(Vector2d one, Vector2d other)
	{
		return new Vector2d(one.x + other.x, one.y + other.y);
	}

	// Token: 0x0600A87A RID: 43130 RVA: 0x003EB84C File Offset: 0x003E9A4C
	public static Vector2d operator *(Vector2d a, double d)
	{
		return new Vector2d(a.x * d, a.y * d);
	}

	// Token: 0x0600A87B RID: 43131 RVA: 0x003EB863 File Offset: 0x003E9A63
	public static Vector2d operator *(double d, Vector2d a)
	{
		return new Vector2d(a.x * d, a.y * d);
	}

	// Token: 0x0600A87C RID: 43132 RVA: 0x003EB87A File Offset: 0x003E9A7A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double Dot(Vector2d lhs, Vector2d rhs)
	{
		return lhs.x * rhs.x + lhs.y * rhs.y;
	}

	// Token: 0x0600A87D RID: 43133 RVA: 0x003EB897 File Offset: 0x003E9A97
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double Dot(Vector2 rhs)
	{
		return this.x * (double)rhs.x + this.y * (double)rhs.y;
	}

	// Token: 0x0600A87E RID: 43134 RVA: 0x003EB8B6 File Offset: 0x003E9AB6
	public override string ToString()
	{
		return this.ToCultureInvariantString();
	}

	// Token: 0x0600A87F RID: 43135 RVA: 0x003EB8C0 File Offset: 0x003E9AC0
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

	// Token: 0x04007CD4 RID: 31956
	public double x;

	// Token: 0x04007CD5 RID: 31957
	public double y;

	// Token: 0x04007CD6 RID: 31958
	public static readonly Vector2d Zero = new Vector2d(0.0, 0.0);
}
