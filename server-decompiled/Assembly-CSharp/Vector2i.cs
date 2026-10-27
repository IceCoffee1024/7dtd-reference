using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02001525 RID: 5413
public struct Vector2i : IEquatable<Vector2i>
{
	// Token: 0x17001405 RID: 5125
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

	// Token: 0x0600A99B RID: 43419 RVA: 0x003F2D29 File Offset: 0x003F0F29
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2i(int _x, int _y)
	{
		this.x = _x;
		this.y = _y;
	}

	// Token: 0x0600A99C RID: 43420 RVA: 0x003F2D39 File Offset: 0x003F0F39
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2i(Vector2 vector2)
	{
		this = default(Vector2i);
		this.x = Mathf.FloorToInt(vector2.x);
		this.y = Mathf.FloorToInt(vector2.y);
	}

	// Token: 0x0600A99D RID: 43421 RVA: 0x003F2D64 File Offset: 0x003F0F64
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2i(Vector2Int vector2)
	{
		this = default(Vector2i);
		this.x = vector2.x;
		this.y = vector2.y;
	}

	// Token: 0x0600A99E RID: 43422 RVA: 0x003F2D29 File Offset: 0x003F0F29
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Set(int _x, int _y)
	{
		this.x = _x;
		this.y = _y;
	}

	// Token: 0x0600A99F RID: 43423 RVA: 0x003F2D87 File Offset: 0x003F0F87
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2Int AsVector2Int()
	{
		return new Vector2Int(this.x, this.y);
	}

	// Token: 0x0600A9A0 RID: 43424 RVA: 0x003F2D9A File Offset: 0x003F0F9A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2 AsVector2()
	{
		return new Vector2((float)this.x, (float)this.y);
	}

	// Token: 0x0600A9A1 RID: 43425 RVA: 0x003F2DB0 File Offset: 0x003F0FB0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals(object obj)
	{
		Vector2i vector2i = (Vector2i)obj;
		return this.Equals(vector2i);
	}

	// Token: 0x0600A9A2 RID: 43426 RVA: 0x003F2DCB File Offset: 0x003F0FCB
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(Vector2i other)
	{
		return other.x == this.x && other.y == this.y;
	}

	// Token: 0x0600A9A3 RID: 43427 RVA: 0x003F2DEC File Offset: 0x003F0FEC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Distance(Vector2i a, Vector2i b)
	{
		double num = (double)(a.x - b.x);
		double num2 = (double)(a.y - b.y);
		return (float)Math.Sqrt(num * num + num2 * num2);
	}

	// Token: 0x0600A9A4 RID: 43428 RVA: 0x003F2E24 File Offset: 0x003F1024
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float DistanceSqr(Vector2i a, Vector2i b)
	{
		float num = (float)(a.x - b.x);
		float num2 = (float)(a.y - b.y);
		return num * num + num2 * num2;
	}

	// Token: 0x0600A9A5 RID: 43429 RVA: 0x003F2E54 File Offset: 0x003F1054
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int DistanceSqrInt(Vector2i a, Vector2i b)
	{
		int num = a.x - b.x;
		int num2 = a.y - b.y;
		return num * num + num2 * num2;
	}

	// Token: 0x0600A9A6 RID: 43430 RVA: 0x003F2E84 File Offset: 0x003F1084
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

	// Token: 0x0600A9A7 RID: 43431 RVA: 0x003F2ED4 File Offset: 0x003F10D4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(Vector2i one, Vector2i other)
	{
		return one.x == other.x && one.y == other.y;
	}

	// Token: 0x0600A9A8 RID: 43432 RVA: 0x003F2EF4 File Offset: 0x003F10F4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return this.x * 8976890 + this.y * 981131;
	}

	// Token: 0x0600A9A9 RID: 43433 RVA: 0x003F2F0F File Offset: 0x003F110F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(Vector2i one, Vector2i other)
	{
		return !(one == other);
	}

	// Token: 0x0600A9AA RID: 43434 RVA: 0x003F2F1B File Offset: 0x003F111B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i operator +(Vector2i one, Vector2i other)
	{
		return new Vector2i(one.x + other.x, one.y + other.y);
	}

	// Token: 0x0600A9AB RID: 43435 RVA: 0x003F2F3C File Offset: 0x003F113C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i operator -(Vector2i one, Vector2i other)
	{
		return new Vector2i(one.x - other.x, one.y - other.y);
	}

	// Token: 0x0600A9AC RID: 43436 RVA: 0x003F2F5D File Offset: 0x003F115D
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i operator /(Vector2i one, int div)
	{
		return new Vector2i(one.x / div, one.y / div);
	}

	// Token: 0x0600A9AD RID: 43437 RVA: 0x003F2F74 File Offset: 0x003F1174
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i operator *(Vector2i a, int i)
	{
		return new Vector2i(a.x * i, a.y * i);
	}

	// Token: 0x0600A9AE RID: 43438 RVA: 0x003F2F8B File Offset: 0x003F118B
	public override string ToString()
	{
		return string.Format("{0}, {1}", this.x, this.y);
	}

	// Token: 0x0600A9AF RID: 43439 RVA: 0x003F2D87 File Offset: 0x003F0F87
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vector2Int(Vector2i _v2i)
	{
		return new Vector2Int(_v2i.x, _v2i.y);
	}

	// Token: 0x04007D9E RID: 32158
	public static readonly Vector2i zero = new Vector2i(0, 0);

	// Token: 0x04007D9F RID: 32159
	public static readonly Vector2i one = new Vector2i(1, 1);

	// Token: 0x04007DA0 RID: 32160
	public static readonly Vector2i min = new Vector2i(int.MinValue, int.MinValue);

	// Token: 0x04007DA1 RID: 32161
	public static readonly Vector2i max = new Vector2i(int.MaxValue, int.MaxValue);

	// Token: 0x04007DA2 RID: 32162
	public static readonly Vector2i up = new Vector2i(0, 1);

	// Token: 0x04007DA3 RID: 32163
	public static readonly Vector2i down = new Vector2i(0, -1);

	// Token: 0x04007DA4 RID: 32164
	public static readonly Vector2i right = new Vector2i(1, 0);

	// Token: 0x04007DA5 RID: 32165
	public static readonly Vector2i left = new Vector2i(-1, 0);

	// Token: 0x04007DA6 RID: 32166
	public int x;

	// Token: 0x04007DA7 RID: 32167
	public int y;
}
