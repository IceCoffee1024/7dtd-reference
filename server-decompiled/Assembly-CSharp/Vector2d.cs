using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02001523 RID: 5411
public struct Vector2d : IEquatable<Vector2d>
{
	// Token: 0x0600A97A RID: 43386 RVA: 0x003F2982 File Offset: 0x003F0B82
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2d(double _x, double _y)
	{
		this.x = _x;
		this.y = _y;
	}

	// Token: 0x0600A97B RID: 43387 RVA: 0x003F2992 File Offset: 0x003F0B92
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2d(Vector2 _v)
	{
		this.x = (double)_v.x;
		this.y = (double)_v.y;
	}

	// Token: 0x0600A97C RID: 43388 RVA: 0x003F29AE File Offset: 0x003F0BAE
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2d(Vector2i _v)
	{
		this.x = (double)_v.x;
		this.y = (double)_v.y;
	}

	// Token: 0x0600A97D RID: 43389 RVA: 0x003F29CA File Offset: 0x003F0BCA
	public bool Equals(double _x, double _y)
	{
		return this.x == _x && this.y == _y;
	}

	// Token: 0x0600A97E RID: 43390 RVA: 0x003F29E0 File Offset: 0x003F0BE0
	public override bool Equals(object obj)
	{
		Vector2d vector2d = (Vector2d)obj;
		return this.Equals(vector2d);
	}

	// Token: 0x0600A97F RID: 43391 RVA: 0x003F29FB File Offset: 0x003F0BFB
	public bool Equals(Vector2d other)
	{
		return other.x == this.x && other.y == this.y;
	}

	// Token: 0x0600A980 RID: 43392 RVA: 0x003F2A1B File Offset: 0x003F0C1B
	public static bool operator ==(Vector2d one, Vector2d other)
	{
		return one.x == other.x && one.y == other.y;
	}

	// Token: 0x0600A981 RID: 43393 RVA: 0x003F2A3B File Offset: 0x003F0C3B
	public override int GetHashCode()
	{
		return this.x.GetHashCode() ^ (this.y.GetHashCode() << 2);
	}

	// Token: 0x0600A982 RID: 43394 RVA: 0x003F2A56 File Offset: 0x003F0C56
	public static bool operator !=(Vector2d one, Vector2d other)
	{
		return !(one == other);
	}

	// Token: 0x0600A983 RID: 43395 RVA: 0x003F2A62 File Offset: 0x003F0C62
	public static Vector2d operator -(Vector2d one, Vector2d other)
	{
		return new Vector2d(one.x - other.x, one.y - other.y);
	}

	// Token: 0x0600A984 RID: 43396 RVA: 0x003F2A83 File Offset: 0x003F0C83
	public static Vector2d operator +(Vector2d one, Vector2d other)
	{
		return new Vector2d(one.x + other.x, one.y + other.y);
	}

	// Token: 0x0600A985 RID: 43397 RVA: 0x003F2AA4 File Offset: 0x003F0CA4
	public static Vector2d operator *(Vector2d a, double d)
	{
		return new Vector2d(a.x * d, a.y * d);
	}

	// Token: 0x0600A986 RID: 43398 RVA: 0x003F2ABB File Offset: 0x003F0CBB
	public static Vector2d operator *(double d, Vector2d a)
	{
		return new Vector2d(a.x * d, a.y * d);
	}

	// Token: 0x0600A987 RID: 43399 RVA: 0x003F2AD2 File Offset: 0x003F0CD2
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double Dot(Vector2d lhs, Vector2d rhs)
	{
		return lhs.x * rhs.x + lhs.y * rhs.y;
	}

	// Token: 0x0600A988 RID: 43400 RVA: 0x003F2AEF File Offset: 0x003F0CEF
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double Dot(Vector2 rhs)
	{
		return this.x * (double)rhs.x + this.y * (double)rhs.y;
	}

	// Token: 0x0600A989 RID: 43401 RVA: 0x003F2B0E File Offset: 0x003F0D0E
	public override string ToString()
	{
		return this.ToCultureInvariantString();
	}

	// Token: 0x0600A98A RID: 43402 RVA: 0x003F2B18 File Offset: 0x003F0D18
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

	// Token: 0x04007D98 RID: 32152
	public double x;

	// Token: 0x04007D99 RID: 32153
	public double y;

	// Token: 0x04007D9A RID: 32154
	public static readonly Vector2d Zero = new Vector2d(0.0, 0.0);
}
