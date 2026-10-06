using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02001516 RID: 5398
public struct Vector3i : IEquatable<Vector3i>
{
	// Token: 0x170013FA RID: 5114
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

	// Token: 0x0600A972 RID: 43378 RVA: 0x003F101F File Offset: 0x003EF21F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3i(int _x, int _y, int _z)
	{
		this.x = _x;
		this.y = _y;
		this.z = _z;
	}

	// Token: 0x0600A973 RID: 43379 RVA: 0x003F1036 File Offset: 0x003EF236
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3i(Vector3 _v)
	{
		this.x = (int)_v.x;
		this.y = (int)_v.y;
		this.z = (int)_v.z;
	}

	// Token: 0x0600A974 RID: 43380 RVA: 0x003F105F File Offset: 0x003EF25F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3i(Vector3Int _v)
	{
		this.x = _v.x;
		this.y = _v.y;
		this.z = _v.z;
	}

	// Token: 0x0600A975 RID: 43381 RVA: 0x003F1088 File Offset: 0x003EF288
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3i(float _x, float _y, float _z)
	{
		this.x = (int)_x;
		this.y = (int)_y;
		this.z = (int)_z;
	}

	// Token: 0x0600A976 RID: 43382 RVA: 0x003F10A2 File Offset: 0x003EF2A2
	public void FloorToInt(Vector3 _v)
	{
		this.x = Utils.Fastfloor(_v.x);
		this.y = Utils.Fastfloor(_v.y);
		this.z = Utils.Fastfloor(_v.z);
	}

	// Token: 0x0600A977 RID: 43383 RVA: 0x003F10D7 File Offset: 0x003EF2D7
	public void RoundToInt(Vector3 _v)
	{
		this.x = Mathf.RoundToInt(_v.x);
		this.y = Mathf.RoundToInt(_v.y);
		this.z = Mathf.RoundToInt(_v.z);
	}

	// Token: 0x0600A978 RID: 43384 RVA: 0x003F110C File Offset: 0x003EF30C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(int _x, int _y, int _z)
	{
		return this.x == _x && this.y == _y && this.z == _z;
	}

	// Token: 0x0600A979 RID: 43385 RVA: 0x003F112C File Offset: 0x003EF32C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals(object obj)
	{
		Vector3i vector3i = (Vector3i)obj;
		return this.Equals(vector3i);
	}

	// Token: 0x0600A97A RID: 43386 RVA: 0x003F1147 File Offset: 0x003EF347
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(Vector3i other)
	{
		return other.x == this.x && other.y == this.y && other.z == this.z;
	}

	// Token: 0x0600A97B RID: 43387 RVA: 0x003F1175 File Offset: 0x003EF375
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(Vector3i one, Vector3i other)
	{
		return one.x == other.x && one.y == other.y && one.z == other.z;
	}

	// Token: 0x0600A97C RID: 43388 RVA: 0x003F11A3 File Offset: 0x003EF3A3
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return this.x * 8976890 + this.y * 981131 + this.z;
	}

	// Token: 0x0600A97D RID: 43389 RVA: 0x003F11C5 File Offset: 0x003EF3C5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(Vector3i one, Vector3i other)
	{
		return !(one == other);
	}

	// Token: 0x0600A97E RID: 43390 RVA: 0x003F11D1 File Offset: 0x003EF3D1
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator -(Vector3i one, Vector3i other)
	{
		return new Vector3i(one.x - other.x, one.y - other.y, one.z - other.z);
	}

	// Token: 0x0600A97F RID: 43391 RVA: 0x003F11FF File Offset: 0x003EF3FF
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator +(Vector3i one, Vector3i other)
	{
		return new Vector3i(one.x + other.x, one.y + other.y, one.z + other.z);
	}

	// Token: 0x0600A980 RID: 43392 RVA: 0x003F122D File Offset: 0x003EF42D
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator *(Vector3i a, float d)
	{
		return new Vector3i((float)a.x * d, (float)a.y * d, (float)a.z * d);
	}

	// Token: 0x0600A981 RID: 43393 RVA: 0x003F124F File Offset: 0x003EF44F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator *(float d, Vector3i a)
	{
		return new Vector3i((float)a.x * d, (float)a.y * d, (float)a.z * d);
	}

	// Token: 0x0600A982 RID: 43394 RVA: 0x003F1271 File Offset: 0x003EF471
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator *(Vector3i a, int i)
	{
		return new Vector3i(a.x * i, a.y * i, a.z * i);
	}

	// Token: 0x0600A983 RID: 43395 RVA: 0x003F1290 File Offset: 0x003EF490
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator *(int i, Vector3i a)
	{
		return new Vector3i(a.x * i, a.y * i, a.z * i);
	}

	// Token: 0x0600A984 RID: 43396 RVA: 0x003F12AF File Offset: 0x003EF4AF
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator /(Vector3i a, int i)
	{
		return new Vector3i(a.x / i, a.y / i, a.z / i);
	}

	// Token: 0x0600A985 RID: 43397 RVA: 0x003F12CE File Offset: 0x003EF4CE
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator &(Vector3i a, int i)
	{
		return new Vector3i(a.x & i, a.y & i, a.z & i);
	}

	// Token: 0x0600A986 RID: 43398 RVA: 0x003F12ED File Offset: 0x003EF4ED
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator &(int i, Vector3i a)
	{
		return new Vector3i(a.x & i, a.y & i, a.z & i);
	}

	// Token: 0x0600A987 RID: 43399 RVA: 0x003F130C File Offset: 0x003EF50C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int Volume()
	{
		return this.x * this.y * this.z;
	}

	// Token: 0x0600A988 RID: 43400 RVA: 0x003F1322 File Offset: 0x003EF522
	public override string ToString()
	{
		return string.Format("{0}, {1}, {2}", this.x, this.y, this.z);
	}

	// Token: 0x0600A989 RID: 43401 RVA: 0x003F134F File Offset: 0x003EF54F
	public string ToStringNoBlanks()
	{
		return string.Format("{0},{1},{2}", this.x, this.y, this.z);
	}

	// Token: 0x0600A98A RID: 43402 RVA: 0x003F137C File Offset: 0x003EF57C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public readonly Vector3 ToVector3()
	{
		return new Vector3((float)this.x, (float)this.y, (float)this.z);
	}

	// Token: 0x0600A98B RID: 43403 RVA: 0x003F1398 File Offset: 0x003EF598
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public readonly Vector3 ToVector3Center()
	{
		return new Vector3((float)this.x + 0.5f, (float)this.y + 0.5f, (float)this.z + 0.5f);
	}

	// Token: 0x0600A98C RID: 43404 RVA: 0x00240894 File Offset: 0x0023EA94
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public readonly Vector3 ToVector3CenterXZ()
	{
		return new Vector3((float)this.x + 0.5f, (float)this.y, (float)this.z + 0.5f);
	}

	// Token: 0x0600A98D RID: 43405 RVA: 0x003F13C8 File Offset: 0x003EF5C8
	public static Vector3i Parse(string _s)
	{
		string[] array = _s.Split(',', StringSplitOptions.None);
		if (array.Length != 3)
		{
			return Vector3i.zero;
		}
		return new Vector3i(int.Parse(array[0]), int.Parse(array[1]), int.Parse(array[2]));
	}

	// Token: 0x0600A98E RID: 43406 RVA: 0x003F1408 File Offset: 0x003EF608
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i Cross(Vector3i _a, Vector3i _b)
	{
		return new Vector3i(_a.y * _b.z - _a.z * _b.y, _a.z * _b.x - _a.x * _b.z, _a.x * _b.y - _a.y * _b.x);
	}

	// Token: 0x0600A98F RID: 43407 RVA: 0x003F146B File Offset: 0x003EF66B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i FromVector3Rounded(Vector3 _v)
	{
		return new Vector3i(Mathf.RoundToInt(_v.x), Mathf.RoundToInt(_v.y), Mathf.RoundToInt(_v.z));
	}

	// Token: 0x0600A990 RID: 43408 RVA: 0x003F1493 File Offset: 0x003EF693
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i Min(Vector3i v1, Vector3i v2)
	{
		return new Vector3i(Math.Min(v1.x, v2.x), Math.Min(v1.y, v2.y), Math.Min(v1.z, v2.z));
	}

	// Token: 0x0600A991 RID: 43409 RVA: 0x003F14CD File Offset: 0x003EF6CD
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i Max(Vector3i v1, Vector3i v2)
	{
		return new Vector3i(Math.Max(v1.x, v2.x), Math.Max(v1.y, v2.y), Math.Max(v1.z, v2.z));
	}

	// Token: 0x0600A992 RID: 43410 RVA: 0x003F1508 File Offset: 0x003EF708
	public static void SortBoundingBoxEdges(ref Vector3i _minEdge, ref Vector3i _maxEdge)
	{
		Vector3i vector3i = _minEdge;
		Vector3i vector3i2 = _maxEdge;
		_minEdge = new Vector3i(Math.Min(vector3i.x, vector3i2.x), Math.Min(vector3i.y, vector3i2.y), Math.Min(vector3i.z, vector3i2.z));
		_maxEdge = new Vector3i(Math.Max(vector3i.x, vector3i2.x), Math.Max(vector3i.y, vector3i2.y), Math.Max(vector3i.z, vector3i2.z));
	}

	// Token: 0x170013FB RID: 5115
	// (get) Token: 0x0600A993 RID: 43411 RVA: 0x003F159F File Offset: 0x003EF79F
	public bool IsValid
	{
		get
		{
			return this != Vector3i.invalid;
		}
	}

	// Token: 0x0600A994 RID: 43412 RVA: 0x003F15B4 File Offset: 0x003EF7B4
	public static Vector3i Floor(Vector3 _v)
	{
		Vector3i vector3i;
		vector3i.x = Utils.Fastfloor(_v.x);
		vector3i.y = Utils.Fastfloor(_v.y);
		vector3i.z = Utils.Fastfloor(_v.z);
		return vector3i;
	}

	// Token: 0x0600A995 RID: 43413 RVA: 0x003F15F8 File Offset: 0x003EF7F8
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vector3(Vector3i _v3i)
	{
		return _v3i.ToVector3();
	}

	// Token: 0x0600A996 RID: 43414 RVA: 0x003F1601 File Offset: 0x003EF801
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vector3Int(Vector3i _v3i)
	{
		return new Vector3Int(_v3i.x, _v3i.y, _v3i.z);
	}

	// Token: 0x0600A997 RID: 43415 RVA: 0x003F161A File Offset: 0x003EF81A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vector3i(Vector3Int _v3i)
	{
		return new Vector3i(_v3i.x, _v3i.y, _v3i.z);
	}

	// Token: 0x04007D6E RID: 32110
	public static readonly Vector3i invalid = new Vector3i(int.MinValue, int.MinValue, int.MinValue);

	// Token: 0x04007D6F RID: 32111
	public static readonly Vector3i up = new Vector3i(0, 1, 0);

	// Token: 0x04007D70 RID: 32112
	public static readonly Vector3i down = new Vector3i(0, -1, 0);

	// Token: 0x04007D71 RID: 32113
	public static readonly Vector3i zero = new Vector3i(0, 0, 0);

	// Token: 0x04007D72 RID: 32114
	public static readonly Vector3i left = new Vector3i(-1, 0, 0);

	// Token: 0x04007D73 RID: 32115
	public static readonly Vector3i right = new Vector3i(1, 0, 0);

	// Token: 0x04007D74 RID: 32116
	public static readonly Vector3i forward = new Vector3i(0, 0, 1);

	// Token: 0x04007D75 RID: 32117
	public static readonly Vector3i back = new Vector3i(0, 0, -1);

	// Token: 0x04007D76 RID: 32118
	public static readonly Vector3i one = new Vector3i(1, 1, 1);

	// Token: 0x04007D77 RID: 32119
	public static readonly Vector3i min = new Vector3i(int.MinValue, int.MinValue, int.MinValue);

	// Token: 0x04007D78 RID: 32120
	public static readonly Vector3i max = new Vector3i(int.MaxValue, int.MaxValue, int.MaxValue);

	// Token: 0x04007D79 RID: 32121
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

	// Token: 0x04007D7A RID: 32122
	public static readonly Vector3i[] AllDirections = new Vector3i[]
	{
		Vector3i.right,
		Vector3i.left,
		Vector3i.up,
		Vector3i.down,
		Vector3i.forward,
		Vector3i.back
	};

	// Token: 0x04007D7B RID: 32123
	public static readonly Vector3i[] AllDirectionsShuffled = new Vector3i[]
	{
		Vector3i.right,
		Vector3i.left,
		Vector3i.up,
		Vector3i.down,
		Vector3i.forward,
		Vector3i.back
	};

	// Token: 0x04007D7C RID: 32124
	public static readonly Vector3i[] MIDDLE_AND_HORIZONTAL_DIRECTIONS = new Vector3i[]
	{
		Vector3i.zero,
		new Vector3i(1, 0, 0),
		new Vector3i(-1, 0, 0),
		new Vector3i(0, 0, 1),
		new Vector3i(0, 0, -1)
	};

	// Token: 0x04007D7D RID: 32125
	public static readonly Vector3i[] HORIZONTAL_DIRECTIONS = new Vector3i[]
	{
		new Vector3i(1, 0, 0),
		new Vector3i(-1, 0, 0),
		new Vector3i(0, 0, 1),
		new Vector3i(0, 0, -1)
	};

	// Token: 0x04007D7E RID: 32126
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

	// Token: 0x04007D7F RID: 32127
	public int x;

	// Token: 0x04007D80 RID: 32128
	public int y;

	// Token: 0x04007D81 RID: 32129
	public int z;
}
