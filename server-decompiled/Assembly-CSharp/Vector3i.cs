using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020014D9 RID: 5337
public struct Vector3i : IEquatable<Vector3i>
{
	// Token: 0x1700137C RID: 4988
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

	// Token: 0x0600A7C2 RID: 42946 RVA: 0x003E6E8B File Offset: 0x003E508B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3i(int _x, int _y, int _z)
	{
		this.x = _x;
		this.y = _y;
		this.z = _z;
	}

	// Token: 0x0600A7C3 RID: 42947 RVA: 0x003E6EA2 File Offset: 0x003E50A2
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3i(Vector3 _v)
	{
		this.x = (int)_v.x;
		this.y = (int)_v.y;
		this.z = (int)_v.z;
	}

	// Token: 0x0600A7C4 RID: 42948 RVA: 0x003E6ECB File Offset: 0x003E50CB
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3i(Vector3Int _v)
	{
		this.x = _v.x;
		this.y = _v.y;
		this.z = _v.z;
	}

	// Token: 0x0600A7C5 RID: 42949 RVA: 0x003E6EF4 File Offset: 0x003E50F4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3i(float _x, float _y, float _z)
	{
		this.x = (int)_x;
		this.y = (int)_y;
		this.z = (int)_z;
	}

	// Token: 0x0600A7C6 RID: 42950 RVA: 0x003E6F0E File Offset: 0x003E510E
	public void FloorToInt(Vector3 _v)
	{
		this.x = Utils.Fastfloor(_v.x);
		this.y = Utils.Fastfloor(_v.y);
		this.z = Utils.Fastfloor(_v.z);
	}

	// Token: 0x0600A7C7 RID: 42951 RVA: 0x003E6F43 File Offset: 0x003E5143
	public void RoundToInt(Vector3 _v)
	{
		this.x = Mathf.RoundToInt(_v.x);
		this.y = Mathf.RoundToInt(_v.y);
		this.z = Mathf.RoundToInt(_v.z);
	}

	// Token: 0x0600A7C8 RID: 42952 RVA: 0x003E6F78 File Offset: 0x003E5178
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(int _x, int _y, int _z)
	{
		return this.x == _x && this.y == _y && this.z == _z;
	}

	// Token: 0x0600A7C9 RID: 42953 RVA: 0x003E6F98 File Offset: 0x003E5198
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals(object obj)
	{
		Vector3i vector3i = (Vector3i)obj;
		return this.Equals(vector3i);
	}

	// Token: 0x0600A7CA RID: 42954 RVA: 0x003E6FB3 File Offset: 0x003E51B3
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(Vector3i other)
	{
		return other.x == this.x && other.y == this.y && other.z == this.z;
	}

	// Token: 0x0600A7CB RID: 42955 RVA: 0x003E6FE1 File Offset: 0x003E51E1
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(Vector3i one, Vector3i other)
	{
		return one.x == other.x && one.y == other.y && one.z == other.z;
	}

	// Token: 0x0600A7CC RID: 42956 RVA: 0x003E700F File Offset: 0x003E520F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return this.x * 8976890 + this.y * 981131 + this.z;
	}

	// Token: 0x0600A7CD RID: 42957 RVA: 0x003E7031 File Offset: 0x003E5231
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(Vector3i one, Vector3i other)
	{
		return !(one == other);
	}

	// Token: 0x0600A7CE RID: 42958 RVA: 0x003E703D File Offset: 0x003E523D
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator -(Vector3i one, Vector3i other)
	{
		return new Vector3i(one.x - other.x, one.y - other.y, one.z - other.z);
	}

	// Token: 0x0600A7CF RID: 42959 RVA: 0x003E706B File Offset: 0x003E526B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator +(Vector3i one, Vector3i other)
	{
		return new Vector3i(one.x + other.x, one.y + other.y, one.z + other.z);
	}

	// Token: 0x0600A7D0 RID: 42960 RVA: 0x003E7099 File Offset: 0x003E5299
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator *(Vector3i a, float d)
	{
		return new Vector3i((float)a.x * d, (float)a.y * d, (float)a.z * d);
	}

	// Token: 0x0600A7D1 RID: 42961 RVA: 0x003E70BB File Offset: 0x003E52BB
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator *(float d, Vector3i a)
	{
		return new Vector3i((float)a.x * d, (float)a.y * d, (float)a.z * d);
	}

	// Token: 0x0600A7D2 RID: 42962 RVA: 0x003E70DD File Offset: 0x003E52DD
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator *(Vector3i a, int i)
	{
		return new Vector3i(a.x * i, a.y * i, a.z * i);
	}

	// Token: 0x0600A7D3 RID: 42963 RVA: 0x003E70FC File Offset: 0x003E52FC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator *(int i, Vector3i a)
	{
		return new Vector3i(a.x * i, a.y * i, a.z * i);
	}

	// Token: 0x0600A7D4 RID: 42964 RVA: 0x003E711B File Offset: 0x003E531B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator /(Vector3i a, int i)
	{
		return new Vector3i(a.x / i, a.y / i, a.z / i);
	}

	// Token: 0x0600A7D5 RID: 42965 RVA: 0x003E713A File Offset: 0x003E533A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator &(Vector3i a, int i)
	{
		return new Vector3i(a.x & i, a.y & i, a.z & i);
	}

	// Token: 0x0600A7D6 RID: 42966 RVA: 0x003E7159 File Offset: 0x003E5359
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator &(int i, Vector3i a)
	{
		return new Vector3i(a.x & i, a.y & i, a.z & i);
	}

	// Token: 0x0600A7D7 RID: 42967 RVA: 0x003E7178 File Offset: 0x003E5378
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int Volume()
	{
		return this.x * this.y * this.z;
	}

	// Token: 0x0600A7D8 RID: 42968 RVA: 0x003E718E File Offset: 0x003E538E
	public override string ToString()
	{
		return string.Format("{0}, {1}, {2}", this.x, this.y, this.z);
	}

	// Token: 0x0600A7D9 RID: 42969 RVA: 0x003E71BB File Offset: 0x003E53BB
	public string ToStringNoBlanks()
	{
		return string.Format("{0},{1},{2}", this.x, this.y, this.z);
	}

	// Token: 0x0600A7DA RID: 42970 RVA: 0x003E71E8 File Offset: 0x003E53E8
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public readonly Vector3 ToVector3()
	{
		return new Vector3((float)this.x, (float)this.y, (float)this.z);
	}

	// Token: 0x0600A7DB RID: 42971 RVA: 0x003E7204 File Offset: 0x003E5404
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public readonly Vector3 ToVector3Center()
	{
		return new Vector3((float)this.x + 0.5f, (float)this.y + 0.5f, (float)this.z + 0.5f);
	}

	// Token: 0x0600A7DC RID: 42972 RVA: 0x0023A87C File Offset: 0x00238A7C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public readonly Vector3 ToVector3CenterXZ()
	{
		return new Vector3((float)this.x + 0.5f, (float)this.y, (float)this.z + 0.5f);
	}

	// Token: 0x0600A7DD RID: 42973 RVA: 0x003E7234 File Offset: 0x003E5434
	public static Vector3i Parse(string _s)
	{
		string[] array = _s.Split(',', StringSplitOptions.None);
		if (array.Length != 3)
		{
			return Vector3i.zero;
		}
		return new Vector3i(int.Parse(array[0]), int.Parse(array[1]), int.Parse(array[2]));
	}

	// Token: 0x0600A7DE RID: 42974 RVA: 0x003E7274 File Offset: 0x003E5474
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i Cross(Vector3i _a, Vector3i _b)
	{
		return new Vector3i(_a.y * _b.z - _a.z * _b.y, _a.z * _b.x - _a.x * _b.z, _a.x * _b.y - _a.y * _b.x);
	}

	// Token: 0x0600A7DF RID: 42975 RVA: 0x003E72D7 File Offset: 0x003E54D7
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i FromVector3Rounded(Vector3 _v)
	{
		return new Vector3i(Mathf.RoundToInt(_v.x), Mathf.RoundToInt(_v.y), Mathf.RoundToInt(_v.z));
	}

	// Token: 0x0600A7E0 RID: 42976 RVA: 0x003E72FF File Offset: 0x003E54FF
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i Min(Vector3i v1, Vector3i v2)
	{
		return new Vector3i(Math.Min(v1.x, v2.x), Math.Min(v1.y, v2.y), Math.Min(v1.z, v2.z));
	}

	// Token: 0x0600A7E1 RID: 42977 RVA: 0x003E7339 File Offset: 0x003E5539
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i Max(Vector3i v1, Vector3i v2)
	{
		return new Vector3i(Math.Max(v1.x, v2.x), Math.Max(v1.y, v2.y), Math.Max(v1.z, v2.z));
	}

	// Token: 0x0600A7E2 RID: 42978 RVA: 0x003E7374 File Offset: 0x003E5574
	public static void SortBoundingBoxEdges(ref Vector3i _minEdge, ref Vector3i _maxEdge)
	{
		Vector3i vector3i = _minEdge;
		Vector3i vector3i2 = _maxEdge;
		_minEdge = new Vector3i(Math.Min(vector3i.x, vector3i2.x), Math.Min(vector3i.y, vector3i2.y), Math.Min(vector3i.z, vector3i2.z));
		_maxEdge = new Vector3i(Math.Max(vector3i.x, vector3i2.x), Math.Max(vector3i.y, vector3i2.y), Math.Max(vector3i.z, vector3i2.z));
	}

	// Token: 0x1700137D RID: 4989
	// (get) Token: 0x0600A7E3 RID: 42979 RVA: 0x003E740B File Offset: 0x003E560B
	public bool IsValid
	{
		get
		{
			return this != Vector3i.invalid;
		}
	}

	// Token: 0x0600A7E4 RID: 42980 RVA: 0x003E7420 File Offset: 0x003E5620
	public static Vector3i Floor(Vector3 _v)
	{
		Vector3i vector3i;
		vector3i.x = Utils.Fastfloor(_v.x);
		vector3i.y = Utils.Fastfloor(_v.y);
		vector3i.z = Utils.Fastfloor(_v.z);
		return vector3i;
	}

	// Token: 0x0600A7E5 RID: 42981 RVA: 0x003E7464 File Offset: 0x003E5664
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vector3(Vector3i _v3i)
	{
		return _v3i.ToVector3();
	}

	// Token: 0x0600A7E6 RID: 42982 RVA: 0x003E746D File Offset: 0x003E566D
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vector3Int(Vector3i _v3i)
	{
		return new Vector3Int(_v3i.x, _v3i.y, _v3i.z);
	}

	// Token: 0x0600A7E7 RID: 42983 RVA: 0x003E7486 File Offset: 0x003E5686
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vector3i(Vector3Int _v3i)
	{
		return new Vector3i(_v3i.x, _v3i.y, _v3i.z);
	}

	// Token: 0x04007C12 RID: 31762
	public static readonly Vector3i invalid = new Vector3i(int.MinValue, int.MinValue, int.MinValue);

	// Token: 0x04007C13 RID: 31763
	public static readonly Vector3i up = new Vector3i(0, 1, 0);

	// Token: 0x04007C14 RID: 31764
	public static readonly Vector3i down = new Vector3i(0, -1, 0);

	// Token: 0x04007C15 RID: 31765
	public static readonly Vector3i zero = new Vector3i(0, 0, 0);

	// Token: 0x04007C16 RID: 31766
	public static readonly Vector3i left = new Vector3i(-1, 0, 0);

	// Token: 0x04007C17 RID: 31767
	public static readonly Vector3i right = new Vector3i(1, 0, 0);

	// Token: 0x04007C18 RID: 31768
	public static readonly Vector3i forward = new Vector3i(0, 0, 1);

	// Token: 0x04007C19 RID: 31769
	public static readonly Vector3i back = new Vector3i(0, 0, -1);

	// Token: 0x04007C1A RID: 31770
	public static readonly Vector3i one = new Vector3i(1, 1, 1);

	// Token: 0x04007C1B RID: 31771
	public static readonly Vector3i min = new Vector3i(int.MinValue, int.MinValue, int.MinValue);

	// Token: 0x04007C1C RID: 31772
	public static readonly Vector3i max = new Vector3i(int.MaxValue, int.MaxValue, int.MaxValue);

	// Token: 0x04007C1D RID: 31773
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

	// Token: 0x04007C1E RID: 31774
	public static readonly Vector3i[] AllDirections = new Vector3i[]
	{
		Vector3i.right,
		Vector3i.left,
		Vector3i.up,
		Vector3i.down,
		Vector3i.forward,
		Vector3i.back
	};

	// Token: 0x04007C1F RID: 31775
	public static readonly Vector3i[] AllDirectionsShuffled = new Vector3i[]
	{
		Vector3i.right,
		Vector3i.left,
		Vector3i.up,
		Vector3i.down,
		Vector3i.forward,
		Vector3i.back
	};

	// Token: 0x04007C20 RID: 31776
	public static readonly Vector3i[] MIDDLE_AND_HORIZONTAL_DIRECTIONS = new Vector3i[]
	{
		Vector3i.zero,
		new Vector3i(1, 0, 0),
		new Vector3i(-1, 0, 0),
		new Vector3i(0, 0, 1),
		new Vector3i(0, 0, -1)
	};

	// Token: 0x04007C21 RID: 31777
	public static readonly Vector3i[] HORIZONTAL_DIRECTIONS = new Vector3i[]
	{
		new Vector3i(1, 0, 0),
		new Vector3i(-1, 0, 0),
		new Vector3i(0, 0, 1),
		new Vector3i(0, 0, -1)
	};

	// Token: 0x04007C22 RID: 31778
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

	// Token: 0x04007C23 RID: 31779
	public int x;

	// Token: 0x04007C24 RID: 31780
	public int y;

	// Token: 0x04007C25 RID: 31781
	public int z;
}
