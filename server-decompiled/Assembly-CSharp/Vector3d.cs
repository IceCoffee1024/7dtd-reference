using System;
using UnityEngine;

// Token: 0x02001527 RID: 5415
public struct Vector3d : IEquatable<Vector3d>
{
	// Token: 0x0600A9B7 RID: 43447 RVA: 0x003F30DD File Offset: 0x003F12DD
	public Vector3d(double _x, double _y, double _z)
	{
		this.x = _x;
		this.y = _y;
		this.z = _z;
	}

	// Token: 0x0600A9B8 RID: 43448 RVA: 0x003F30F4 File Offset: 0x003F12F4
	public Vector3d(Vector3 _v)
	{
		this.x = (double)_v.x;
		this.y = (double)_v.y;
		this.z = (double)_v.z;
	}

	// Token: 0x0600A9B9 RID: 43449 RVA: 0x003F311D File Offset: 0x003F131D
	public Vector3d(Vector3i _v)
	{
		this.x = (double)_v.x;
		this.y = (double)_v.y;
		this.z = (double)_v.z;
	}

	// Token: 0x0600A9BA RID: 43450 RVA: 0x003F3146 File Offset: 0x003F1346
	public bool Equals(double _x, double _y, double _z)
	{
		return this.x == _x && this.y == _y && this.z == _z;
	}

	// Token: 0x0600A9BB RID: 43451 RVA: 0x003F3168 File Offset: 0x003F1368
	public override bool Equals(object obj)
	{
		Vector3d vector3d = (Vector3d)obj;
		return this.Equals(vector3d);
	}

	// Token: 0x0600A9BC RID: 43452 RVA: 0x003F3183 File Offset: 0x003F1383
	public bool Equals(Vector3d other)
	{
		return other.x == this.x && other.y == this.y && other.z == this.z;
	}

	// Token: 0x0600A9BD RID: 43453 RVA: 0x003F31B1 File Offset: 0x003F13B1
	public static bool operator ==(Vector3d one, Vector3d other)
	{
		return one.x == other.x && one.y == other.y && one.z == other.z;
	}

	// Token: 0x0600A9BE RID: 43454 RVA: 0x003F31DF File Offset: 0x003F13DF
	public override int GetHashCode()
	{
		return this.x.GetHashCode() ^ (this.y.GetHashCode() << 2) ^ (this.z.GetHashCode() >> 2);
	}

	// Token: 0x0600A9BF RID: 43455 RVA: 0x003F3208 File Offset: 0x003F1408
	public static bool operator !=(Vector3d one, Vector3d other)
	{
		return !(one == other);
	}

	// Token: 0x0600A9C0 RID: 43456 RVA: 0x003F3214 File Offset: 0x003F1414
	public static Vector3d operator -(Vector3d one, Vector3d other)
	{
		return new Vector3d(one.x - other.x, one.y - other.y, one.z - other.z);
	}

	// Token: 0x0600A9C1 RID: 43457 RVA: 0x003F3242 File Offset: 0x003F1442
	public static Vector3d operator +(Vector3d one, Vector3d other)
	{
		return new Vector3d(one.x + other.x, one.y + other.y, one.z + other.z);
	}

	// Token: 0x0600A9C2 RID: 43458 RVA: 0x003F3270 File Offset: 0x003F1470
	public static Vector3d operator *(Vector3d a, double d)
	{
		return new Vector3d(a.x * d, a.y * d, a.z * d);
	}

	// Token: 0x0600A9C3 RID: 43459 RVA: 0x003F328F File Offset: 0x003F148F
	public static Vector3d operator *(double d, Vector3d a)
	{
		return new Vector3d(a.x * d, a.y * d, a.z * d);
	}

	// Token: 0x0600A9C4 RID: 43460 RVA: 0x003F32AE File Offset: 0x003F14AE
	public override string ToString()
	{
		return this.ToCultureInvariantString();
	}

	// Token: 0x0600A9C5 RID: 43461 RVA: 0x003F32B8 File Offset: 0x003F14B8
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

	// Token: 0x0600A9C6 RID: 43462 RVA: 0x003F332C File Offset: 0x003F152C
	public static Vector3d Cross(Vector3d _a, Vector3d _b)
	{
		return new Vector3d(_a.y * _b.z - _a.z * _b.y, _a.z * _b.x - _a.x * _b.z, _a.x * _b.y - _a.y * _b.x);
	}

	// Token: 0x04007DAB RID: 32171
	public double x;

	// Token: 0x04007DAC RID: 32172
	public double y;

	// Token: 0x04007DAD RID: 32173
	public double z;

	// Token: 0x04007DAE RID: 32174
	public static readonly Vector3d Zero = new Vector3d(0.0, 0.0, 0.0);
}
