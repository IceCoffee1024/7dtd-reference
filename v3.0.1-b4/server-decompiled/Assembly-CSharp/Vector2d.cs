using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020014D4 RID: 5332
public struct Vector2d : IEquatable<Vector2d>
{
	// Token: 0x0600A772 RID: 42866 RVA: 0x003E63EA File Offset: 0x003E45EA
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2d(double _x, double _y)
	{
		this.x = _x;
		this.y = _y;
	}

	// Token: 0x0600A773 RID: 42867 RVA: 0x003E63FA File Offset: 0x003E45FA
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2d(Vector2 _v)
	{
		this.x = (double)_v.x;
		this.y = (double)_v.y;
	}

	// Token: 0x0600A774 RID: 42868 RVA: 0x003E6416 File Offset: 0x003E4616
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2d(Vector2i _v)
	{
		this.x = (double)_v.x;
		this.y = (double)_v.y;
	}

	// Token: 0x0600A775 RID: 42869 RVA: 0x003E6432 File Offset: 0x003E4632
	public bool Equals(double _x, double _y)
	{
		return this.x == _x && this.y == _y;
	}

	// Token: 0x0600A776 RID: 42870 RVA: 0x003E6448 File Offset: 0x003E4648
	public override bool Equals(object obj)
	{
		Vector2d vector2d = (Vector2d)obj;
		return this.Equals(vector2d);
	}

	// Token: 0x0600A777 RID: 42871 RVA: 0x003E6463 File Offset: 0x003E4663
	public bool Equals(Vector2d other)
	{
		return other.x == this.x && other.y == this.y;
	}

	// Token: 0x0600A778 RID: 42872 RVA: 0x003E6483 File Offset: 0x003E4683
	public static bool operator ==(Vector2d one, Vector2d other)
	{
		return one.x == other.x && one.y == other.y;
	}

	// Token: 0x0600A779 RID: 42873 RVA: 0x003E64A3 File Offset: 0x003E46A3
	public override int GetHashCode()
	{
		return this.x.GetHashCode() ^ (this.y.GetHashCode() << 2);
	}

	// Token: 0x0600A77A RID: 42874 RVA: 0x003E64BE File Offset: 0x003E46BE
	public static bool operator !=(Vector2d one, Vector2d other)
	{
		return !(one == other);
	}

	// Token: 0x0600A77B RID: 42875 RVA: 0x003E64CA File Offset: 0x003E46CA
	public static Vector2d operator -(Vector2d one, Vector2d other)
	{
		return new Vector2d(one.x - other.x, one.y - other.y);
	}

	// Token: 0x0600A77C RID: 42876 RVA: 0x003E64EB File Offset: 0x003E46EB
	public static Vector2d operator +(Vector2d one, Vector2d other)
	{
		return new Vector2d(one.x + other.x, one.y + other.y);
	}

	// Token: 0x0600A77D RID: 42877 RVA: 0x003E650C File Offset: 0x003E470C
	public static Vector2d operator *(Vector2d a, double d)
	{
		return new Vector2d(a.x * d, a.y * d);
	}

	// Token: 0x0600A77E RID: 42878 RVA: 0x003E6523 File Offset: 0x003E4723
	public static Vector2d operator *(double d, Vector2d a)
	{
		return new Vector2d(a.x * d, a.y * d);
	}

	// Token: 0x0600A77F RID: 42879 RVA: 0x003E653A File Offset: 0x003E473A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double Dot(Vector2d lhs, Vector2d rhs)
	{
		return lhs.x * rhs.x + lhs.y * rhs.y;
	}

	// Token: 0x0600A780 RID: 42880 RVA: 0x003E6557 File Offset: 0x003E4757
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double Dot(Vector2 rhs)
	{
		return this.x * (double)rhs.x + this.y * (double)rhs.y;
	}

	// Token: 0x0600A781 RID: 42881 RVA: 0x003E6576 File Offset: 0x003E4776
	public override string ToString()
	{
		return this.ToCultureInvariantString();
	}

	// Token: 0x0600A782 RID: 42882 RVA: 0x003E6580 File Offset: 0x003E4780
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

	// Token: 0x04007BFB RID: 31739
	public double x;

	// Token: 0x04007BFC RID: 31740
	public double y;

	// Token: 0x04007BFD RID: 31741
	public static readonly Vector2d Zero = new Vector2d(0.0, 0.0);
}
