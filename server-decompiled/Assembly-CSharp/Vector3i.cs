using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020014F4 RID: 5364
public struct Vector3i : IEquatable<Vector3i>
{
	// Token: 0x170013B5 RID: 5045
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

	// Token: 0x0600A8BF RID: 43199 RVA: 0x003EC1CB File Offset: 0x003EA3CB
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3i(int _x, int _y, int _z)
	{
		this.x = _x;
		this.y = _y;
		this.z = _z;
	}

	// Token: 0x0600A8C0 RID: 43200 RVA: 0x003EC1E2 File Offset: 0x003EA3E2
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3i(Vector3 _v)
	{
		this.x = (int)_v.x;
		this.y = (int)_v.y;
		this.z = (int)_v.z;
	}

	// Token: 0x0600A8C1 RID: 43201 RVA: 0x003EC20B File Offset: 0x003EA40B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3i(Vector3Int _v)
	{
		this.x = _v.x;
		this.y = _v.y;
		this.z = _v.z;
	}

	// Token: 0x0600A8C2 RID: 43202 RVA: 0x003EC234 File Offset: 0x003EA434
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3i(float _x, float _y, float _z)
	{
		this.x = (int)_x;
		this.y = (int)_y;
		this.z = (int)_z;
	}

	// Token: 0x0600A8C3 RID: 43203 RVA: 0x003EC24E File Offset: 0x003EA44E
	public void FloorToInt(Vector3 _v)
	{
		this.x = Utils.Fastfloor(_v.x);
		this.y = Utils.Fastfloor(_v.y);
		this.z = Utils.Fastfloor(_v.z);
	}

	// Token: 0x0600A8C4 RID: 43204 RVA: 0x003EC283 File Offset: 0x003EA483
	public void RoundToInt(Vector3 _v)
	{
		this.x = Mathf.RoundToInt(_v.x);
		this.y = Mathf.RoundToInt(_v.y);
		this.z = Mathf.RoundToInt(_v.z);
	}

	// Token: 0x0600A8C5 RID: 43205 RVA: 0x003EC2B8 File Offset: 0x003EA4B8
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(int _x, int _y, int _z)
	{
		return this.x == _x && this.y == _y && this.z == _z;
	}

	// Token: 0x0600A8C6 RID: 43206 RVA: 0x003EC2D8 File Offset: 0x003EA4D8
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals(object obj)
	{
		Vector3i vector3i = (Vector3i)obj;
		return this.Equals(vector3i);
	}

	// Token: 0x0600A8C7 RID: 43207 RVA: 0x003EC2F3 File Offset: 0x003EA4F3
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(Vector3i other)
	{
		return other.x == this.x && other.y == this.y && other.z == this.z;
	}

	// Token: 0x0600A8C8 RID: 43208 RVA: 0x003EC321 File Offset: 0x003EA521
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(Vector3i one, Vector3i other)
	{
		return one.x == other.x && one.y == other.y && one.z == other.z;
	}

	// Token: 0x0600A8C9 RID: 43209 RVA: 0x003EC34F File Offset: 0x003EA54F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return this.x * 8976890 + this.y * 981131 + this.z;
	}

	// Token: 0x0600A8CA RID: 43210 RVA: 0x003EC371 File Offset: 0x003EA571
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(Vector3i one, Vector3i other)
	{
		return !(one == other);
	}

	// Token: 0x0600A8CB RID: 43211 RVA: 0x003EC37D File Offset: 0x003EA57D
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator -(Vector3i one, Vector3i other)
	{
		return new Vector3i(one.x - other.x, one.y - other.y, one.z - other.z);
	}

	// Token: 0x0600A8CC RID: 43212 RVA: 0x003EC3AB File Offset: 0x003EA5AB
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator +(Vector3i one, Vector3i other)
	{
		return new Vector3i(one.x + other.x, one.y + other.y, one.z + other.z);
	}

	// Token: 0x0600A8CD RID: 43213 RVA: 0x003EC3D9 File Offset: 0x003EA5D9
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator *(Vector3i a, float d)
	{
		return new Vector3i((float)a.x * d, (float)a.y * d, (float)a.z * d);
	}

	// Token: 0x0600A8CE RID: 43214 RVA: 0x003EC3FB File Offset: 0x003EA5FB
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator *(float d, Vector3i a)
	{
		return new Vector3i((float)a.x * d, (float)a.y * d, (float)a.z * d);
	}

	// Token: 0x0600A8CF RID: 43215 RVA: 0x003EC41D File Offset: 0x003EA61D
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator *(Vector3i a, int i)
	{
		return new Vector3i(a.x * i, a.y * i, a.z * i);
	}

	// Token: 0x0600A8D0 RID: 43216 RVA: 0x003EC43C File Offset: 0x003EA63C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator *(int i, Vector3i a)
	{
		return new Vector3i(a.x * i, a.y * i, a.z * i);
	}

	// Token: 0x0600A8D1 RID: 43217 RVA: 0x003EC45B File Offset: 0x003EA65B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator /(Vector3i a, int i)
	{
		return new Vector3i(a.x / i, a.y / i, a.z / i);
	}

	// Token: 0x0600A8D2 RID: 43218 RVA: 0x003EC47A File Offset: 0x003EA67A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator &(Vector3i a, int i)
	{
		return new Vector3i(a.x & i, a.y & i, a.z & i);
	}

	// Token: 0x0600A8D3 RID: 43219 RVA: 0x003EC499 File Offset: 0x003EA699
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i operator &(int i, Vector3i a)
	{
		return new Vector3i(a.x & i, a.y & i, a.z & i);
	}

	// Token: 0x0600A8D4 RID: 43220 RVA: 0x003EC4B8 File Offset: 0x003EA6B8
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int Volume()
	{
		return this.x * this.y * this.z;
	}

	// Token: 0x0600A8D5 RID: 43221 RVA: 0x003EC4CE File Offset: 0x003EA6CE
	public override string ToString()
	{
		return string.Format("{0}, {1}, {2}", this.x, this.y, this.z);
	}

	// Token: 0x0600A8D6 RID: 43222 RVA: 0x003EC4FB File Offset: 0x003EA6FB
	public string ToStringNoBlanks()
	{
		return string.Format("{0},{1},{2}", this.x, this.y, this.z);
	}

	// Token: 0x0600A8D7 RID: 43223 RVA: 0x003EC528 File Offset: 0x003EA728
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public readonly Vector3 ToVector3()
	{
		return new Vector3((float)this.x, (float)this.y, (float)this.z);
	}

	// Token: 0x0600A8D8 RID: 43224 RVA: 0x003EC544 File Offset: 0x003EA744
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public readonly Vector3 ToVector3Center()
	{
		return new Vector3((float)this.x + 0.5f, (float)this.y + 0.5f, (float)this.z + 0.5f);
	}

	// Token: 0x0600A8D9 RID: 43225 RVA: 0x0023D598 File Offset: 0x0023B798
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public readonly Vector3 ToVector3CenterXZ()
	{
		return new Vector3((float)this.x + 0.5f, (float)this.y, (float)this.z + 0.5f);
	}

	// Token: 0x0600A8DA RID: 43226 RVA: 0x003EC574 File Offset: 0x003EA774
	public static Vector3i Parse(string _s)
	{
		string[] array = _s.Split(',', StringSplitOptions.None);
		if (array.Length != 3)
		{
			return Vector3i.zero;
		}
		return new Vector3i(int.Parse(array[0]), int.Parse(array[1]), int.Parse(array[2]));
	}

	// Token: 0x0600A8DB RID: 43227 RVA: 0x003EC5B4 File Offset: 0x003EA7B4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i Cross(Vector3i _a, Vector3i _b)
	{
		return new Vector3i(_a.y * _b.z - _a.z * _b.y, _a.z * _b.x - _a.x * _b.z, _a.x * _b.y - _a.y * _b.x);
	}

	// Token: 0x0600A8DC RID: 43228 RVA: 0x003EC617 File Offset: 0x003EA817
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i FromVector3Rounded(Vector3 _v)
	{
		return new Vector3i(Mathf.RoundToInt(_v.x), Mathf.RoundToInt(_v.y), Mathf.RoundToInt(_v.z));
	}

	// Token: 0x0600A8DD RID: 43229 RVA: 0x003EC63F File Offset: 0x003EA83F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i Min(Vector3i v1, Vector3i v2)
	{
		return new Vector3i(Math.Min(v1.x, v2.x), Math.Min(v1.y, v2.y), Math.Min(v1.z, v2.z));
	}

	// Token: 0x0600A8DE RID: 43230 RVA: 0x003EC679 File Offset: 0x003EA879
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i Max(Vector3i v1, Vector3i v2)
	{
		return new Vector3i(Math.Max(v1.x, v2.x), Math.Max(v1.y, v2.y), Math.Max(v1.z, v2.z));
	}

	// Token: 0x0600A8DF RID: 43231 RVA: 0x003EC6B4 File Offset: 0x003EA8B4
	public static void SortBoundingBoxEdges(ref Vector3i _minEdge, ref Vector3i _maxEdge)
	{
		Vector3i vector3i = _minEdge;
		Vector3i vector3i2 = _maxEdge;
		_minEdge = new Vector3i(Math.Min(vector3i.x, vector3i2.x), Math.Min(vector3i.y, vector3i2.y), Math.Min(vector3i.z, vector3i2.z));
		_maxEdge = new Vector3i(Math.Max(vector3i.x, vector3i2.x), Math.Max(vector3i.y, vector3i2.y), Math.Max(vector3i.z, vector3i2.z));
	}

	// Token: 0x170013B6 RID: 5046
	// (get) Token: 0x0600A8E0 RID: 43232 RVA: 0x003EC74B File Offset: 0x003EA94B
	public bool IsValid
	{
		get
		{
			return this != Vector3i.invalid;
		}
	}

	// Token: 0x0600A8E1 RID: 43233 RVA: 0x003EC760 File Offset: 0x003EA960
	public static Vector3i Floor(Vector3 _v)
	{
		Vector3i vector3i;
		vector3i.x = Utils.Fastfloor(_v.x);
		vector3i.y = Utils.Fastfloor(_v.y);
		vector3i.z = Utils.Fastfloor(_v.z);
		return vector3i;
	}

	// Token: 0x0600A8E2 RID: 43234 RVA: 0x003EC7A4 File Offset: 0x003EA9A4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vector3(Vector3i _v3i)
	{
		return _v3i.ToVector3();
	}

	// Token: 0x0600A8E3 RID: 43235 RVA: 0x003EC7AD File Offset: 0x003EA9AD
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vector3Int(Vector3i _v3i)
	{
		return new Vector3Int(_v3i.x, _v3i.y, _v3i.z);
	}

	// Token: 0x0600A8E4 RID: 43236 RVA: 0x003EC7C6 File Offset: 0x003EA9C6
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vector3i(Vector3Int _v3i)
	{
		return new Vector3i(_v3i.x, _v3i.y, _v3i.z);
	}

	// Token: 0x04007CEB RID: 31979
	public static readonly Vector3i invalid = new Vector3i(int.MinValue, int.MinValue, int.MinValue);

	// Token: 0x04007CEC RID: 31980
	public static readonly Vector3i up = new Vector3i(0, 1, 0);

	// Token: 0x04007CED RID: 31981
	public static readonly Vector3i down = new Vector3i(0, -1, 0);

	// Token: 0x04007CEE RID: 31982
	public static readonly Vector3i zero = new Vector3i(0, 0, 0);

	// Token: 0x04007CEF RID: 31983
	public static readonly Vector3i left = new Vector3i(-1, 0, 0);

	// Token: 0x04007CF0 RID: 31984
	public static readonly Vector3i right = new Vector3i(1, 0, 0);

	// Token: 0x04007CF1 RID: 31985
	public static readonly Vector3i forward = new Vector3i(0, 0, 1);

	// Token: 0x04007CF2 RID: 31986
	public static readonly Vector3i back = new Vector3i(0, 0, -1);

	// Token: 0x04007CF3 RID: 31987
	public static readonly Vector3i one = new Vector3i(1, 1, 1);

	// Token: 0x04007CF4 RID: 31988
	public static readonly Vector3i min = new Vector3i(int.MinValue, int.MinValue, int.MinValue);

	// Token: 0x04007CF5 RID: 31989
	public static readonly Vector3i max = new Vector3i(int.MaxValue, int.MaxValue, int.MaxValue);

	// Token: 0x04007CF6 RID: 31990
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

	// Token: 0x04007CF7 RID: 31991
	public static readonly Vector3i[] AllDirections = new Vector3i[]
	{
		Vector3i.right,
		Vector3i.left,
		Vector3i.up,
		Vector3i.down,
		Vector3i.forward,
		Vector3i.back
	};

	// Token: 0x04007CF8 RID: 31992
	public static readonly Vector3i[] AllDirectionsShuffled = new Vector3i[]
	{
		Vector3i.right,
		Vector3i.left,
		Vector3i.up,
		Vector3i.down,
		Vector3i.forward,
		Vector3i.back
	};

	// Token: 0x04007CF9 RID: 31993
	public static readonly Vector3i[] MIDDLE_AND_HORIZONTAL_DIRECTIONS = new Vector3i[]
	{
		Vector3i.zero,
		new Vector3i(1, 0, 0),
		new Vector3i(-1, 0, 0),
		new Vector3i(0, 0, 1),
		new Vector3i(0, 0, -1)
	};

	// Token: 0x04007CFA RID: 31994
	public static readonly Vector3i[] HORIZONTAL_DIRECTIONS = new Vector3i[]
	{
		new Vector3i(1, 0, 0),
		new Vector3i(-1, 0, 0),
		new Vector3i(0, 0, 1),
		new Vector3i(0, 0, -1)
	};

	// Token: 0x04007CFB RID: 31995
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

	// Token: 0x04007CFC RID: 31996
	public int x;

	// Token: 0x04007CFD RID: 31997
	public int y;

	// Token: 0x04007CFE RID: 31998
	public int z;
}
