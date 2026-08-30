using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020014D6 RID: 5334
public struct Vector2i : IEquatable<Vector2i>
{
	// Token: 0x1700137B RID: 4987
	public int this[int _index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (_index == 0)
			{
				return this.x;
			}
			if (_index != 1)
			{
				throw new IndexOutOfRangeException("Invalid Vector2i index!");
			}
			return this.y;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			if (_index == 0)
			{
				this.x = value;
				return;
			}
			if (_index != 1)
			{
				throw new IndexOutOfRangeException("Invalid Vector2i index!");
			}
			this.y = value;
		}
	}

	// Token: 0x0600A793 RID: 42899 RVA: 0x003E6791 File Offset: 0x003E4991
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2i(int _x, int _y)
	{
		this.x = _x;
		this.y = _y;
	}

	// Token: 0x0600A794 RID: 42900 RVA: 0x003E67A1 File Offset: 0x003E49A1
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2i(Vector2 vector2)
	{
		this = default(Vector2i);
		this.x = Mathf.FloorToInt(vector2.x);
		this.y = Mathf.FloorToInt(vector2.y);
	}

	// Token: 0x0600A795 RID: 42901 RVA: 0x003E67CC File Offset: 0x003E49CC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2i(Vector2Int vector2)
	{
		this = default(Vector2i);
		this.x = vector2.x;
		this.y = vector2.y;
	}

	// Token: 0x0600A796 RID: 42902 RVA: 0x003E6791 File Offset: 0x003E4991
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Set(int _x, int _y)
	{
		this.x = _x;
		this.y = _y;
	}

	// Token: 0x0600A797 RID: 42903 RVA: 0x003E67EF File Offset: 0x003E49EF
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2Int AsVector2Int()
	{
		return new Vector2Int(this.x, this.y);
	}

	// Token: 0x0600A798 RID: 42904 RVA: 0x003E6802 File Offset: 0x003E4A02
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2 AsVector2()
	{
		return new Vector2((float)this.x, (float)this.y);
	}

	// Token: 0x0600A799 RID: 42905 RVA: 0x003E6818 File Offset: 0x003E4A18
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals(object obj)
	{
		Vector2i vector2i = (Vector2i)obj;
		return this.Equals(vector2i);
	}

	// Token: 0x0600A79A RID: 42906 RVA: 0x003E6833 File Offset: 0x003E4A33
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(Vector2i other)
	{
		return other.x == this.x && other.y == this.y;
	}

	// Token: 0x0600A79B RID: 42907 RVA: 0x003E6854 File Offset: 0x003E4A54
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Distance(Vector2i a, Vector2i b)
	{
		double num = (double)(a.x - b.x);
		double num2 = (double)(a.y - b.y);
		return (float)Math.Sqrt(num * num + num2 * num2);
	}

	// Token: 0x0600A79C RID: 42908 RVA: 0x003E688C File Offset: 0x003E4A8C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float DistanceSqr(Vector2i a, Vector2i b)
	{
		float num = (float)(a.x - b.x);
		float num2 = (float)(a.y - b.y);
		return num * num + num2 * num2;
	}

	// Token: 0x0600A79D RID: 42909 RVA: 0x003E68BC File Offset: 0x003E4ABC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int DistanceSqrInt(Vector2i a, Vector2i b)
	{
		int num = a.x - b.x;
		int num2 = a.y - b.y;
		return num * num + num2 * num2;
	}

	// Token: 0x0600A79E RID: 42910 RVA: 0x003E68EC File Offset: 0x003E4AEC
	public void Normalize()
	{
		if (this.x < 0)
		{
			this.x = -1;
		}
		else if (this.x > 0)
		{
			this.x = 1;
		}
		if (this.y < 0)
		{
			this.y = -1;
			return;
		}
		if (this.y > 0)
		{
			this.y = 1;
		}
	}

	// Token: 0x0600A79F RID: 42911 RVA: 0x003E693C File Offset: 0x003E4B3C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(Vector2i one, Vector2i other)
	{
		return one.x == other.x && one.y == other.y;
	}

	// Token: 0x0600A7A0 RID: 42912 RVA: 0x003E695C File Offset: 0x003E4B5C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return this.x * 8976890 + this.y * 981131;
	}

	// Token: 0x0600A7A1 RID: 42913 RVA: 0x003E6977 File Offset: 0x003E4B77
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(Vector2i one, Vector2i other)
	{
		return !(one == other);
	}

	// Token: 0x0600A7A2 RID: 42914 RVA: 0x003E6983 File Offset: 0x003E4B83
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i operator +(Vector2i one, Vector2i other)
	{
		return new Vector2i(one.x + other.x, one.y + other.y);
	}

	// Token: 0x0600A7A3 RID: 42915 RVA: 0x003E69A4 File Offset: 0x003E4BA4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i operator -(Vector2i one, Vector2i other)
	{
		return new Vector2i(one.x - other.x, one.y - other.y);
	}

	// Token: 0x0600A7A4 RID: 42916 RVA: 0x003E69C5 File Offset: 0x003E4BC5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i operator /(Vector2i one, int div)
	{
		return new Vector2i(one.x / div, one.y / div);
	}

	// Token: 0x0600A7A5 RID: 42917 RVA: 0x003E69DC File Offset: 0x003E4BDC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i operator *(Vector2i a, int i)
	{
		return new Vector2i(a.x * i, a.y * i);
	}

	// Token: 0x0600A7A6 RID: 42918 RVA: 0x003E69F3 File Offset: 0x003E4BF3
	public override string ToString()
	{
		return string.Format("{0}, {1}", this.x, this.y);
	}

	// Token: 0x0600A7A7 RID: 42919 RVA: 0x003E67EF File Offset: 0x003E49EF
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vector2Int(Vector2i _v2i)
	{
		return new Vector2Int(_v2i.x, _v2i.y);
	}

	// Token: 0x04007C01 RID: 31745
	public static readonly Vector2i zero = new Vector2i(0, 0);

	// Token: 0x04007C02 RID: 31746
	public static readonly Vector2i one = new Vector2i(1, 1);

	// Token: 0x04007C03 RID: 31747
	public static readonly Vector2i min = new Vector2i(int.MinValue, int.MinValue);

	// Token: 0x04007C04 RID: 31748
	public static readonly Vector2i max = new Vector2i(int.MaxValue, int.MaxValue);

	// Token: 0x04007C05 RID: 31749
	public static readonly Vector2i up = new Vector2i(0, 1);

	// Token: 0x04007C06 RID: 31750
	public static readonly Vector2i down = new Vector2i(0, -1);

	// Token: 0x04007C07 RID: 31751
	public static readonly Vector2i right = new Vector2i(1, 0);

	// Token: 0x04007C08 RID: 31752
	public static readonly Vector2i left = new Vector2i(-1, 0);

	// Token: 0x04007C09 RID: 31753
	public int x;

	// Token: 0x04007C0A RID: 31754
	public int y;
}
