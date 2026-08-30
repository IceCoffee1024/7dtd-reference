using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020014F1 RID: 5361
public struct Vector2i : IEquatable<Vector2i>
{
	// Token: 0x170013B4 RID: 5044
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

	// Token: 0x0600A890 RID: 43152 RVA: 0x003EBAD1 File Offset: 0x003E9CD1
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2i(int _x, int _y)
	{
		this.x = _x;
		this.y = _y;
	}

	// Token: 0x0600A891 RID: 43153 RVA: 0x003EBAE1 File Offset: 0x003E9CE1
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2i(Vector2 vector2)
	{
		this = default(Vector2i);
		this.x = Mathf.FloorToInt(vector2.x);
		this.y = Mathf.FloorToInt(vector2.y);
	}

	// Token: 0x0600A892 RID: 43154 RVA: 0x003EBB0C File Offset: 0x003E9D0C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2i(Vector2Int vector2)
	{
		this = default(Vector2i);
		this.x = vector2.x;
		this.y = vector2.y;
	}

	// Token: 0x0600A893 RID: 43155 RVA: 0x003EBAD1 File Offset: 0x003E9CD1
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Set(int _x, int _y)
	{
		this.x = _x;
		this.y = _y;
	}

	// Token: 0x0600A894 RID: 43156 RVA: 0x003EBB2F File Offset: 0x003E9D2F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2Int AsVector2Int()
	{
		return new Vector2Int(this.x, this.y);
	}

	// Token: 0x0600A895 RID: 43157 RVA: 0x003EBB42 File Offset: 0x003E9D42
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2 AsVector2()
	{
		return new Vector2((float)this.x, (float)this.y);
	}

	// Token: 0x0600A896 RID: 43158 RVA: 0x003EBB58 File Offset: 0x003E9D58
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals(object obj)
	{
		Vector2i vector2i = (Vector2i)obj;
		return this.Equals(vector2i);
	}

	// Token: 0x0600A897 RID: 43159 RVA: 0x003EBB73 File Offset: 0x003E9D73
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(Vector2i other)
	{
		return other.x == this.x && other.y == this.y;
	}

	// Token: 0x0600A898 RID: 43160 RVA: 0x003EBB94 File Offset: 0x003E9D94
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Distance(Vector2i a, Vector2i b)
	{
		double num = (double)(a.x - b.x);
		double num2 = (double)(a.y - b.y);
		return (float)Math.Sqrt(num * num + num2 * num2);
	}

	// Token: 0x0600A899 RID: 43161 RVA: 0x003EBBCC File Offset: 0x003E9DCC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float DistanceSqr(Vector2i a, Vector2i b)
	{
		float num = (float)(a.x - b.x);
		float num2 = (float)(a.y - b.y);
		return num * num + num2 * num2;
	}

	// Token: 0x0600A89A RID: 43162 RVA: 0x003EBBFC File Offset: 0x003E9DFC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int DistanceSqrInt(Vector2i a, Vector2i b)
	{
		int num = a.x - b.x;
		int num2 = a.y - b.y;
		return num * num + num2 * num2;
	}

	// Token: 0x0600A89B RID: 43163 RVA: 0x003EBC2C File Offset: 0x003E9E2C
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

	// Token: 0x0600A89C RID: 43164 RVA: 0x003EBC7C File Offset: 0x003E9E7C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(Vector2i one, Vector2i other)
	{
		return one.x == other.x && one.y == other.y;
	}

	// Token: 0x0600A89D RID: 43165 RVA: 0x003EBC9C File Offset: 0x003E9E9C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return this.x * 8976890 + this.y * 981131;
	}

	// Token: 0x0600A89E RID: 43166 RVA: 0x003EBCB7 File Offset: 0x003E9EB7
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(Vector2i one, Vector2i other)
	{
		return !(one == other);
	}

	// Token: 0x0600A89F RID: 43167 RVA: 0x003EBCC3 File Offset: 0x003E9EC3
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i operator +(Vector2i one, Vector2i other)
	{
		return new Vector2i(one.x + other.x, one.y + other.y);
	}

	// Token: 0x0600A8A0 RID: 43168 RVA: 0x003EBCE4 File Offset: 0x003E9EE4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i operator -(Vector2i one, Vector2i other)
	{
		return new Vector2i(one.x - other.x, one.y - other.y);
	}

	// Token: 0x0600A8A1 RID: 43169 RVA: 0x003EBD05 File Offset: 0x003E9F05
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i operator /(Vector2i one, int div)
	{
		return new Vector2i(one.x / div, one.y / div);
	}

	// Token: 0x0600A8A2 RID: 43170 RVA: 0x003EBD1C File Offset: 0x003E9F1C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i operator *(Vector2i a, int i)
	{
		return new Vector2i(a.x * i, a.y * i);
	}

	// Token: 0x0600A8A3 RID: 43171 RVA: 0x003EBD33 File Offset: 0x003E9F33
	public override string ToString()
	{
		return string.Format("{0}, {1}", this.x, this.y);
	}

	// Token: 0x0600A8A4 RID: 43172 RVA: 0x003EBB2F File Offset: 0x003E9D2F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vector2Int(Vector2i _v2i)
	{
		return new Vector2Int(_v2i.x, _v2i.y);
	}

	// Token: 0x04007CDA RID: 31962
	public static readonly Vector2i zero = new Vector2i(0, 0);

	// Token: 0x04007CDB RID: 31963
	public static readonly Vector2i one = new Vector2i(1, 1);

	// Token: 0x04007CDC RID: 31964
	public static readonly Vector2i min = new Vector2i(int.MinValue, int.MinValue);

	// Token: 0x04007CDD RID: 31965
	public static readonly Vector2i max = new Vector2i(int.MaxValue, int.MaxValue);

	// Token: 0x04007CDE RID: 31966
	public static readonly Vector2i up = new Vector2i(0, 1);

	// Token: 0x04007CDF RID: 31967
	public static readonly Vector2i down = new Vector2i(0, -1);

	// Token: 0x04007CE0 RID: 31968
	public static readonly Vector2i right = new Vector2i(1, 0);

	// Token: 0x04007CE1 RID: 31969
	public static readonly Vector2i left = new Vector2i(-1, 0);

	// Token: 0x04007CE2 RID: 31970
	public int x;

	// Token: 0x04007CE3 RID: 31971
	public int y;
}
