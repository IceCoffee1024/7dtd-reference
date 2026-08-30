using System;
using UnityEngine;

// Token: 0x020014F3 RID: 5363
public struct Vector3d : IEquatable<Vector3d>
{
	// Token: 0x0600A8AC RID: 43180 RVA: 0x003EBE85 File Offset: 0x003EA085
	public Vector3d(double _x, double _y, double _z)
	{
		this.x = _x;
		this.y = _y;
		this.z = _z;
	}

	// Token: 0x0600A8AD RID: 43181 RVA: 0x003EBE9C File Offset: 0x003EA09C
	public Vector3d(Vector3 _v)
	{
		this.x = (double)_v.x;
		this.y = (double)_v.y;
		this.z = (double)_v.z;
	}

	// Token: 0x0600A8AE RID: 43182 RVA: 0x003EBEC5 File Offset: 0x003EA0C5
	public Vector3d(Vector3i _v)
	{
		this.x = (double)_v.x;
		this.y = (double)_v.y;
		this.z = (double)_v.z;
	}

	// Token: 0x0600A8AF RID: 43183 RVA: 0x003EBEEE File Offset: 0x003EA0EE
	public bool Equals(double _x, double _y, double _z)
	{
		return this.x == _x && this.y == _y && this.z == _z;
	}

	// Token: 0x0600A8B0 RID: 43184 RVA: 0x003EBF10 File Offset: 0x003EA110
	public override bool Equals(object obj)
	{
		Vector3d vector3d = (Vector3d)obj;
		return this.Equals(vector3d);
	}

	// Token: 0x0600A8B1 RID: 43185 RVA: 0x003EBF2B File Offset: 0x003EA12B
	public bool Equals(Vector3d other)
	{
		return other.x == this.x && other.y == this.y && other.z == this.z;
	}

	// Token: 0x0600A8B2 RID: 43186 RVA: 0x003EBF59 File Offset: 0x003EA159
	public static bool operator ==(Vector3d one, Vector3d other)
	{
		return one.x == other.x && one.y == other.y && one.z == other.z;
	}

	// Token: 0x0600A8B3 RID: 43187 RVA: 0x003EBF87 File Offset: 0x003EA187
	public override int GetHashCode()
	{
		return this.x.GetHashCode() ^ (this.y.GetHashCode() << 2) ^ (this.z.GetHashCode() >> 2);
	}

	// Token: 0x0600A8B4 RID: 43188 RVA: 0x003EBFB0 File Offset: 0x003EA1B0
	public static bool operator !=(Vector3d one, Vector3d other)
	{
		return !(one == other);
	}

	// Token: 0x0600A8B5 RID: 43189 RVA: 0x003EBFBC File Offset: 0x003EA1BC
	public static Vector3d operator -(Vector3d one, Vector3d other)
	{
		return new Vector3d(one.x - other.x, one.y - other.y, one.z - other.z);
	}

	// Token: 0x0600A8B6 RID: 43190 RVA: 0x003EBFEA File Offset: 0x003EA1EA
	public static Vector3d operator +(Vector3d one, Vector3d other)
	{
		return new Vector3d(one.x + other.x, one.y + other.y, one.z + other.z);
	}

	// Token: 0x0600A8B7 RID: 43191 RVA: 0x003EC018 File Offset: 0x003EA218
	public static Vector3d operator *(Vector3d a, double d)
	{
		return new Vector3d(a.x * d, a.y * d, a.z * d);
	}

	// Token: 0x0600A8B8 RID: 43192 RVA: 0x003EC037 File Offset: 0x003EA237
	public static Vector3d operator *(double d, Vector3d a)
	{
		return new Vector3d(a.x * d, a.y * d, a.z * d);
	}

	// Token: 0x0600A8B9 RID: 43193 RVA: 0x003EC056 File Offset: 0x003EA256
	public override string ToString()
	{
		return this.ToCultureInvariantString();
	}

	// Token: 0x0600A8BA RID: 43194 RVA: 0x003EC060 File Offset: 0x003EA260
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

	// Token: 0x0600A8BB RID: 43195 RVA: 0x003EC0D4 File Offset: 0x003EA2D4
	public static Vector3d Cross(Vector3d _a, Vector3d _b)
	{
		return new Vector3d(_a.y * _b.z - _a.z * _b.y, _a.z * _b.x - _a.x * _b.z, _a.x * _b.y - _a.y * _b.x);
	}

	// Token: 0x04007CE7 RID: 31975
	public double x;

	// Token: 0x04007CE8 RID: 31976
	public double y;

	// Token: 0x04007CE9 RID: 31977
	public double z;

	// Token: 0x04007CEA RID: 31978
	public static readonly Vector3d Zero = new Vector3d(0.0, 0.0, 0.0);
}
