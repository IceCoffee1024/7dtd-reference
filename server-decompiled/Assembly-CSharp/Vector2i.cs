using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02001513 RID: 5395
public struct Vector2i : IEquatable<Vector2i>
{
	// Token: 0x170013F9 RID: 5113
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

	// Token: 0x0600A943 RID: 43331 RVA: 0x003F0925 File Offset: 0x003EEB25
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2i(int _x, int _y)
	{
		this.x = _x;
		this.y = _y;
	}

	// Token: 0x0600A944 RID: 43332 RVA: 0x003F0935 File Offset: 0x003EEB35
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2i(Vector2 vector2)
	{
		this = default(Vector2i);
		this.x = Mathf.FloorToInt(vector2.x);
		this.y = Mathf.FloorToInt(vector2.y);
	}

	// Token: 0x0600A945 RID: 43333 RVA: 0x003F0960 File Offset: 0x003EEB60
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2i(Vector2Int vector2)
	{
		this = default(Vector2i);
		this.x = vector2.x;
		this.y = vector2.y;
	}

	// Token: 0x0600A946 RID: 43334 RVA: 0x003F0925 File Offset: 0x003EEB25
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Set(int _x, int _y)
	{
		this.x = _x;
		this.y = _y;
	}

	// Token: 0x0600A947 RID: 43335 RVA: 0x003F0983 File Offset: 0x003EEB83
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2Int AsVector2Int()
	{
		return new Vector2Int(this.x, this.y);
	}

	// Token: 0x0600A948 RID: 43336 RVA: 0x003F0996 File Offset: 0x003EEB96
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2 AsVector2()
	{
		return new Vector2((float)this.x, (float)this.y);
	}

	// Token: 0x0600A949 RID: 43337 RVA: 0x003F09AC File Offset: 0x003EEBAC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals(object obj)
	{
		Vector2i vector2i = (Vector2i)obj;
		return this.Equals(vector2i);
	}

	// Token: 0x0600A94A RID: 43338 RVA: 0x003F09C7 File Offset: 0x003EEBC7
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(Vector2i other)
	{
		return other.x == this.x && other.y == this.y;
	}

	// Token: 0x0600A94B RID: 43339 RVA: 0x003F09E8 File Offset: 0x003EEBE8
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Distance(Vector2i a, Vector2i b)
	{
		double num = (double)(a.x - b.x);
		double num2 = (double)(a.y - b.y);
		return (float)Math.Sqrt(num * num + num2 * num2);
	}

	// Token: 0x0600A94C RID: 43340 RVA: 0x003F0A20 File Offset: 0x003EEC20
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float DistanceSqr(Vector2i a, Vector2i b)
	{
		float num = (float)(a.x - b.x);
		float num2 = (float)(a.y - b.y);
		return num * num + num2 * num2;
	}

	// Token: 0x0600A94D RID: 43341 RVA: 0x003F0A50 File Offset: 0x003EEC50
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int DistanceSqrInt(Vector2i a, Vector2i b)
	{
		int num = a.x - b.x;
		int num2 = a.y - b.y;
		return num * num + num2 * num2;
	}

	// Token: 0x0600A94E RID: 43342 RVA: 0x003F0A80 File Offset: 0x003EEC80
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

	// Token: 0x0600A94F RID: 43343 RVA: 0x003F0AD0 File Offset: 0x003EECD0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(Vector2i one, Vector2i other)
	{
		return one.x == other.x && one.y == other.y;
	}

	// Token: 0x0600A950 RID: 43344 RVA: 0x003F0AF0 File Offset: 0x003EECF0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return this.x * 8976890 + this.y * 981131;
	}

	// Token: 0x0600A951 RID: 43345 RVA: 0x003F0B0B File Offset: 0x003EED0B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(Vector2i one, Vector2i other)
	{
		return !(one == other);
	}

	// Token: 0x0600A952 RID: 43346 RVA: 0x003F0B17 File Offset: 0x003EED17
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i operator +(Vector2i one, Vector2i other)
	{
		return new Vector2i(one.x + other.x, one.y + other.y);
	}

	// Token: 0x0600A953 RID: 43347 RVA: 0x003F0B38 File Offset: 0x003EED38
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i operator -(Vector2i one, Vector2i other)
	{
		return new Vector2i(one.x - other.x, one.y - other.y);
	}

	// Token: 0x0600A954 RID: 43348 RVA: 0x003F0B59 File Offset: 0x003EED59
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i operator /(Vector2i one, int div)
	{
		return new Vector2i(one.x / div, one.y / div);
	}

	// Token: 0x0600A955 RID: 43349 RVA: 0x003F0B70 File Offset: 0x003EED70
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i operator *(Vector2i a, int i)
	{
		return new Vector2i(a.x * i, a.y * i);
	}

	// Token: 0x0600A956 RID: 43350 RVA: 0x003F0B87 File Offset: 0x003EED87
	public override string ToString()
	{
		return string.Format("{0}, {1}", this.x, this.y);
	}

	// Token: 0x0600A957 RID: 43351 RVA: 0x003F0983 File Offset: 0x003EEB83
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vector2Int(Vector2i _v2i)
	{
		return new Vector2Int(_v2i.x, _v2i.y);
	}

	// Token: 0x04007D5D RID: 32093
	public static readonly Vector2i zero = new Vector2i(0, 0);

	// Token: 0x04007D5E RID: 32094
	public static readonly Vector2i one = new Vector2i(1, 1);

	// Token: 0x04007D5F RID: 32095
	public static readonly Vector2i min = new Vector2i(int.MinValue, int.MinValue);

	// Token: 0x04007D60 RID: 32096
	public static readonly Vector2i max = new Vector2i(int.MaxValue, int.MaxValue);

	// Token: 0x04007D61 RID: 32097
	public static readonly Vector2i up = new Vector2i(0, 1);

	// Token: 0x04007D62 RID: 32098
	public static readonly Vector2i down = new Vector2i(0, -1);

	// Token: 0x04007D63 RID: 32099
	public static readonly Vector2i right = new Vector2i(1, 0);

	// Token: 0x04007D64 RID: 32100
	public static readonly Vector2i left = new Vector2i(-1, 0);

	// Token: 0x04007D65 RID: 32101
	public int x;

	// Token: 0x04007D66 RID: 32102
	public int y;
}
