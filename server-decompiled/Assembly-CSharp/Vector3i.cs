using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02001528 RID: 5416
public struct Vector3i : IEquatable<Vector3i>
{
	// Token: 0x17001406 RID: 5126
	public int this[int _index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			switch (_index)
			{
			case 0:
				return this.x;
			case 1:
				return this.y;
			case 2:
				return this.z;
			default:
				throw new IndexOutOfRangeException("Invalid Vector3i index!");
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			switch (_index)
			{
			case 0:
				this.x = value;
				return;
			case 1:
				this.y = value;
				return;
			case 2:
				this.z = value;
				return;
			default:
				throw new IndexOutOfRangeException("Invalid Vector3i index!");
			}
		}
	}

	// Token: 0x0600A9CA RID: 43466 RVA: 0x003F3423 File Offset: 0x003F1623
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3i(int _x, int _y, int _z)
	{
		this.x = _x;
		this.y = _y;
		this.z = _z;
	}

	// Token: 0x0600A9CB RID: 43467 RVA: 0x003F343A File Offset: 0x003F163A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3i(Vector3 _v)
	{
		this.x = (int)_v.x;
		this.y = (int)_v.y;
		this.z = (int)_v.z;
	}

	// Token: 0x0600A9CC RID: 43468 RVA: 0x003F3463 File Offset: 0x003F1663
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3i(Vector3Int _v)
	{
		this.x = _v.x;
		this.y = _v.y;
		this.z = _v.z;
	}

	// Token: 0x0600A9CD RID: 43469 RVA: 0x003F348C File Offset: 0x003F168C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3i(float _x, float _y, float _z)
	{
		this.x = (int)_x;
		this.y = (int)_y;
		this.z = (int)_z;
	}

	// Token: 0x0600A9CE RID: 43470 RVA: 0x003F34A6 File Offset: 0x003F16A6
	public void FloorToInt(Vector3 _v)
	{
		this.x = Utils.Fastfloor(_v.x);
		this.y = Utils.Fastfloor(_v.y);
		this.z = Utils.Fastfloor(_v.z);
	}

	// Token: 0x0600A9CF RID: 43471 RVA: 0x003F34DB File Offset: 0x003F16DB
	public void RoundToInt(Vector3 _v)
	{
		this.x = Mathf.RoundToInt(_v.x);
		this.y = Mathf.RoundToInt(_v.y);
		this.z = Mathf.RoundToInt(_v.z);
	}

	// Token: 0x0600A9D0 RID: 43472 RVA: 0x003F3510 File Offset: 0x003F1710
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(int _x, int _y, int _z)
	{
		return this.x == _x && this.y == _y && this.z == _z;
	}

	// Token: 0x0600A9D1 RID: 43473 RVA: 0x003F3530 File Offset: 0x003F1730
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals(object obj)
	{
		Vector3i vector3i = (Vector3i)obj;
		return this.Equals(vector3i);
	}

	// Token: 0x0600A9D2 RID: 43474 RVA: 0x003F354B File Offset: 0x003F174B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(Vector3i other)
	{
		return other.x == this.x && other.y == this.y && other.z == this.z;
	}

	// Token: 0x0600A9D3 RID: 43475 RVA: 0x003F3579 File Offset: 0x003F1779
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(Vector3i one, Vector3i other)
	{
		return one.x == other.x && one.y == other.y && one.z == other.z;
	}

	// Token: 0x0600A9D4 RID: 43476 RVA: 0x003F35A7 File Offset: 0x003F17A7
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return this.x * 8976890 + this.y * 981131 + this.z;
	}

	// Token: 0x0600A9D5 RID: 43477 RVA: 0x003F35C9 File Offset: 0x003F17C9
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(Vector3i one, Vector3i other)
	{
		return !(one == other);
	}

	// Token: 0x0600A9D6 RID: 43478 RVA: 0x003F35D5 File Offset: 0x003F17D5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator -(Vector3i one, Vector3i other)
	{
		return new Vector3i(one.x - other.x, one.y - other.y, one.z - other.z);
	}

	// Token: 0x0600A9D7 RID: 43479 RVA: 0x003F3603 File Offset: 0x003F1803
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator +(Vector3i one, Vector3i other)
	{
		return new Vector3i(one.x + other.x, one.y + other.y, one.z + other.z);
	}

	// Token: 0x0600A9D8 RID: 43480 RVA: 0x003F3631 File Offset: 0x003F1831
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator *(Vector3i a, float d)
	{
		return new Vector3i((float)a.x * d, (float)a.y * d, (float)a.z * d);
	}

	// Token: 0x0600A9D9 RID: 43481 RVA: 0x003F3653 File Offset: 0x003F1853
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator *(float d, Vector3i a)
	{
		return new Vector3i((float)a.x * d, (float)a.y * d, (float)a.z * d);
	}

	// Token: 0x0600A9DA RID: 43482 RVA: 0x003F3675 File Offset: 0x003F1875
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator *(Vector3i a, int i)
	{
		return new Vector3i(a.x * i, a.y * i, a.z * i);
	}

	// Token: 0x0600A9DB RID: 43483 RVA: 0x003F3694 File Offset: 0x003F1894
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator *(int i, Vector3i a)
	{
		return new Vector3i(a.x * i, a.y * i, a.z * i);
	}

	// Token: 0x0600A9DC RID: 43484 RVA: 0x003F36B3 File Offset: 0x003F18B3
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator /(Vector3i a, int i)
	{
		return new Vector3i(a.x / i, a.y / i, a.z / i);
	}

	// Token: 0x0600A9DD RID: 43485 RVA: 0x003F36D2 File Offset: 0x003F18D2
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator &(Vector3i a, int i)
	{
		return new Vector3i(a.x & i, a.y & i, a.z & i);
	}

	// Token: 0x0600A9DE RID: 43486 RVA: 0x003F36F1 File Offset: 0x003F18F1
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator &(int i, Vector3i a)
	{
		return new Vector3i(a.x & i, a.y & i, a.z & i);
	}

	// Token: 0x0600A9DF RID: 43487 RVA: 0x003F3710 File Offset: 0x003F1910
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int Volume()
	{
		return this.x * this.y * this.z;
	}

	// Token: 0x0600A9E0 RID: 43488 RVA: 0x003F3726 File Offset: 0x003F1926
	public override string ToString()
	{
		return string.Format("{0}, {1}, {2}", this.x, this.y, this.z);
	}

	// Token: 0x0600A9E1 RID: 43489 RVA: 0x003F3753 File Offset: 0x003F1953
	public string ToStringNoBlanks()
	{
		return string.Format("{0},{1},{2}", this.x, this.y, this.z);
	}

	// Token: 0x0600A9E2 RID: 43490 RVA: 0x003F3780 File Offset: 0x003F1980
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public readonly Vector3 ToVector3()
	{
		return new Vector3((float)this.x, (float)this.y, (float)this.z);
	}

	// Token: 0x0600A9E3 RID: 43491 RVA: 0x003F379C File Offset: 0x003F199C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public readonly Vector3 ToVector3Center()
	{
		return new Vector3((float)this.x + 0.5f, (float)this.y + 0.5f, (float)this.z + 0.5f);
	}

	// Token: 0x0600A9E4 RID: 43492 RVA: 0x00241DE0 File Offset: 0x0023FFE0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public readonly Vector3 ToVector3CenterXZ()
	{
		return new Vector3((float)this.x + 0.5f, (float)this.y, (float)this.z + 0.5f);
	}

	// Token: 0x0600A9E5 RID: 43493 RVA: 0x003F37CC File Offset: 0x003F19CC
	public static Vector3i Parse(string _s)
	{
		string[] array = _s.Split(',', StringSplitOptions.None);
		if (array.Length != 3)
		{
			return Vector3i.zero;
		}
		return new Vector3i(int.Parse(array[0]), int.Parse(array[1]), int.Parse(array[2]));
	}

	// Token: 0x0600A9E6 RID: 43494 RVA: 0x003F380C File Offset: 0x003F1A0C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i Cross(Vector3i _a, Vector3i _b)
	{
		return new Vector3i(_a.y * _b.z - _a.z * _b.y, _a.z * _b.x - _a.x * _b.z, _a.x * _b.y - _a.y * _b.x);
	}

	// Token: 0x0600A9E7 RID: 43495 RVA: 0x003F386F File Offset: 0x003F1A6F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i FromVector3Rounded(Vector3 _v)
	{
		return new Vector3i(Mathf.RoundToInt(_v.x), Mathf.RoundToInt(_v.y), Mathf.RoundToInt(_v.z));
	}

	// Token: 0x0600A9E8 RID: 43496 RVA: 0x003F3897 File Offset: 0x003F1A97
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i Min(Vector3i v1, Vector3i v2)
	{
		return new Vector3i(Math.Min(v1.x, v2.x), Math.Min(v1.y, v2.y), Math.Min(v1.z, v2.z));
	}

	// Token: 0x0600A9E9 RID: 43497 RVA: 0x003F38D1 File Offset: 0x003F1AD1
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i Max(Vector3i v1, Vector3i v2)
	{
		return new Vector3i(Math.Max(v1.x, v2.x), Math.Max(v1.y, v2.y), Math.Max(v1.z, v2.z));
	}

	// Token: 0x0600A9EA RID: 43498 RVA: 0x003F390C File Offset: 0x003F1B0C
	public static void SortBoundingBoxEdges(ref Vector3i _minEdge, ref Vector3i _maxEdge)
	{
		Vector3i vector3i = _minEdge;
		Vector3i vector3i2 = _maxEdge;
		_minEdge = new Vector3i(Math.Min(vector3i.x, vector3i2.x), Math.Min(vector3i.y, vector3i2.y), Math.Min(vector3i.z, vector3i2.z));
		_maxEdge = new Vector3i(Math.Max(vector3i.x, vector3i2.x), Math.Max(vector3i.y, vector3i2.y), Math.Max(vector3i.z, vector3i2.z));
	}

	// Token: 0x17001407 RID: 5127
	// (get) Token: 0x0600A9EB RID: 43499 RVA: 0x003F39A3 File Offset: 0x003F1BA3
	public bool IsValid
	{
		get
		{
			return this != Vector3i.invalid;
		}
	}

	// Token: 0x0600A9EC RID: 43500 RVA: 0x003F39B8 File Offset: 0x003F1BB8
	public static Vector3i Floor(Vector3 _v)
	{
		Vector3i vector3i;
		vector3i.x = Utils.Fastfloor(_v.x);
		vector3i.y = Utils.Fastfloor(_v.y);
		vector3i.z = Utils.Fastfloor(_v.z);
		return vector3i;
	}

	// Token: 0x0600A9ED RID: 43501 RVA: 0x003F39FC File Offset: 0x003F1BFC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vector3(Vector3i _v3i)
	{
		return _v3i.ToVector3();
	}

	// Token: 0x0600A9EE RID: 43502 RVA: 0x003F3A05 File Offset: 0x003F1C05
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vector3Int(Vector3i _v3i)
	{
		return new Vector3Int(_v3i.x, _v3i.y, _v3i.z);
	}

	// Token: 0x0600A9EF RID: 43503 RVA: 0x003F3A1E File Offset: 0x003F1C1E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vector3i(Vector3Int _v3i)
	{
		return new Vector3i(_v3i.x, _v3i.y, _v3i.z);
	}

	// Token: 0x04007DAF RID: 32175
	public static readonly Vector3i invalid = new Vector3i(int.MinValue, int.MinValue, int.MinValue);

	// Token: 0x04007DB0 RID: 32176
	public static readonly Vector3i up = new Vector3i(0, 1, 0);

	// Token: 0x04007DB1 RID: 32177
	public static readonly Vector3i down = new Vector3i(0, -1, 0);

	// Token: 0x04007DB2 RID: 32178
	public static readonly Vector3i zero = new Vector3i(0, 0, 0);

	// Token: 0x04007DB3 RID: 32179
	public static readonly Vector3i left = new Vector3i(-1, 0, 0);

	// Token: 0x04007DB4 RID: 32180
	public static readonly Vector3i right = new Vector3i(1, 0, 0);

	// Token: 0x04007DB5 RID: 32181
	public static readonly Vector3i forward = new Vector3i(0, 0, 1);

	// Token: 0x04007DB6 RID: 32182
	public static readonly Vector3i back = new Vector3i(0, 0, -1);

	// Token: 0x04007DB7 RID: 32183
	public static readonly Vector3i one = new Vector3i(1, 1, 1);

	// Token: 0x04007DB8 RID: 32184
	public static readonly Vector3i min = new Vector3i(int.MinValue, int.MinValue, int.MinValue);

	// Token: 0x04007DB9 RID: 32185
	public static readonly Vector3i max = new Vector3i(int.MaxValue, int.MaxValue, int.MaxValue);

	// Token: 0x04007DBA RID: 32186
	public static readonly Vector3i[] MIDDLE_AND_ADJACENT_DIRECTIONS = new Vector3i[]
	{
		Vector3i.zero,
		new Vector3i(1, 0, 0),
		new Vector3i(-1, 0, 0),
		new Vector3i(0, 1, 0),
		new Vector3i(0, -1, 0),
		new Vector3i(0, 0, 1),
		new Vector3i(0, 0, -1)
	};

	// Token: 0x04007DBB RID: 32187
	public static readonly Vector3i[] AllDirections = new Vector3i[]
	{
		Vector3i.right,
		Vector3i.left,
		Vector3i.up,
		Vector3i.down,
		Vector3i.forward,
		Vector3i.back
	};

	// Token: 0x04007DBC RID: 32188
	public static readonly Vector3i[] AllDirectionsShuffled = new Vector3i[]
	{
		Vector3i.right,
		Vector3i.left,
		Vector3i.up,
		Vector3i.down,
		Vector3i.forward,
		Vector3i.back
	};

	// Token: 0x04007DBD RID: 32189
	public static readonly Vector3i[] MIDDLE_AND_HORIZONTAL_DIRECTIONS = new Vector3i[]
	{
		Vector3i.zero,
		new Vector3i(1, 0, 0),
		new Vector3i(-1, 0, 0),
		new Vector3i(0, 0, 1),
		new Vector3i(0, 0, -1)
	};

	// Token: 0x04007DBE RID: 32190
	public static readonly Vector3i[] HORIZONTAL_DIRECTIONS = new Vector3i[]
	{
		new Vector3i(1, 0, 0),
		new Vector3i(-1, 0, 0),
		new Vector3i(0, 0, 1),
		new Vector3i(0, 0, -1)
	};

	// Token: 0x04007DBF RID: 32191
	public static readonly Vector3i[] MIDDLE_AND_HORIZONTAL_DIRECTIONS_DIAGONAL = new Vector3i[]
	{
		Vector3i.zero,
		new Vector3i(1, 0, 0),
		new Vector3i(1, 0, -1),
		new Vector3i(0, 0, -1),
		new Vector3i(-1, 0, -1),
		new Vector3i(-1, 0, 0),
		new Vector3i(-1, 0, 1),
		new Vector3i(0, 0, 1),
		new Vector3i(1, 0, 1)
	};

	// Token: 0x04007DC0 RID: 32192
	public int x;

	// Token: 0x04007DC1 RID: 32193
	public int y;

	// Token: 0x04007DC2 RID: 32194
	public int z;
}
