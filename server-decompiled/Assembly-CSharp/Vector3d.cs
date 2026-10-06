using System;
using UnityEngine;

// Token: 0x02001515 RID: 5397
public struct Vector3d : IEquatable<Vector3d>
{
	// Token: 0x0600A95F RID: 43359 RVA: 0x003F0CD9 File Offset: 0x003EEED9
	public Vector3d(double _x, double _y, double _z)
	{
		this.x = _x;
		this.y = _y;
		this.z = _z;
	}

	// Token: 0x0600A960 RID: 43360 RVA: 0x003F0CF0 File Offset: 0x003EEEF0
	public Vector3d(Vector3 _v)
	{
		this.x = (double)_v.x;
		this.y = (double)_v.y;
		this.z = (double)_v.z;
	}

	// Token: 0x0600A961 RID: 43361 RVA: 0x003F0D19 File Offset: 0x003EEF19
	public Vector3d(Vector3i _v)
	{
		this.x = (double)_v.x;
		this.y = (double)_v.y;
		this.z = (double)_v.z;
	}

	// Token: 0x0600A962 RID: 43362 RVA: 0x003F0D42 File Offset: 0x003EEF42
	public bool Equals(double _x, double _y, double _z)
	{
		return this.x == _x && this.y == _y && this.z == _z;
	}

	// Token: 0x0600A963 RID: 43363 RVA: 0x003F0D64 File Offset: 0x003EEF64
	public override bool Equals(object obj)
	{
		Vector3d vector3d = (Vector3d)obj;
		return this.Equals(vector3d);
	}

	// Token: 0x0600A964 RID: 43364 RVA: 0x003F0D7F File Offset: 0x003EEF7F
	public bool Equals(Vector3d other)
	{
		return other.x == this.x && other.y == this.y && other.z == this.z;
	}

	// Token: 0x0600A965 RID: 43365 RVA: 0x003F0DAD File Offset: 0x003EEFAD
	public static bool operator ==(Vector3d one, Vector3d other)
	{
		return one.x == other.x && one.y == other.y && one.z == other.z;
	}

	// Token: 0x0600A966 RID: 43366 RVA: 0x003F0DDB File Offset: 0x003EEFDB
	public override int GetHashCode()
	{
		return this.x.GetHashCode() ^ (this.y.GetHashCode() << 2) ^ (this.z.GetHashCode() >> 2);
	}

	// Token: 0x0600A967 RID: 43367 RVA: 0x003F0E04 File Offset: 0x003EF004
	public static bool operator !=(Vector3d one, Vector3d other)
	{
		return !(one == other);
	}

	// Token: 0x0600A968 RID: 43368 RVA: 0x003F0E10 File Offset: 0x003EF010
	public static Vector3d operator -(Vector3d one, Vector3d other)
	{
		return new Vector3d(one.x - other.x, one.y - other.y, one.z - other.z);
	}

	// Token: 0x0600A969 RID: 43369 RVA: 0x003F0E3E File Offset: 0x003EF03E
	public static Vector3d operator +(Vector3d one, Vector3d other)
	{
		return new Vector3d(one.x + other.x, one.y + other.y, one.z + other.z);
	}

	// Token: 0x0600A96A RID: 43370 RVA: 0x003F0E6C File Offset: 0x003EF06C
	public static Vector3d operator *(Vector3d a, double d)
	{
		return new Vector3d(a.x * d, a.y * d, a.z * d);
	}

	// Token: 0x0600A96B RID: 43371 RVA: 0x003F0E8B File Offset: 0x003EF08B
	public static Vector3d operator *(double d, Vector3d a)
	{
		return new Vector3d(a.x * d, a.y * d, a.z * d);
	}

	// Token: 0x0600A96C RID: 43372 RVA: 0x003F0EAA File Offset: 0x003EF0AA
	public override string ToString()
	{
		return this.ToCultureInvariantString();
	}

	// Token: 0x0600A96D RID: 43373 RVA: 0x003F0EB4 File Offset: 0x003EF0B4
	public string ToCultureInvariantString()
	{
		return string.Concat(new string[]
		{
			"(",
			this.x.ToCultureInvariantString("F1"),
			", ",
			this.y.ToCultureInvariantString("F1"),
			", ",
			this.z.ToCultureInvariantString("F1"),
			")"
		});
	}

	// Token: 0x0600A96E RID: 43374 RVA: 0x003F0F28 File Offset: 0x003EF128
	public static Vector3d Cross(Vector3d _a, Vector3d _b)
	{
		return new Vector3d(_a.y * _b.z - _a.z * _b.y, _a.z * _b.x - _a.x * _b.z, _a.x * _b.y - _a.y * _b.x);
	}

	// Token: 0x04007D6A RID: 32106
	public double x;

	// Token: 0x04007D6B RID: 32107
	public double y;

	// Token: 0x04007D6C RID: 32108
	public double z;

	// Token: 0x04007D6D RID: 32109
	public static readonly Vector3d Zero = new Vector3d(0.0, 0.0, 0.0);
}
