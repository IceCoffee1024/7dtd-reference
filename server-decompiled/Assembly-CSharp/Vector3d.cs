using System;
using UnityEngine;

// Token: 0x020014D8 RID: 5336
public struct Vector3d : IEquatable<Vector3d>
{
	// Token: 0x0600A7AF RID: 42927 RVA: 0x003E6B45 File Offset: 0x003E4D45
	public Vector3d(double _x, double _y, double _z)
	{
		this.x = _x;
		this.y = _y;
		this.z = _z;
	}

	// Token: 0x0600A7B0 RID: 42928 RVA: 0x003E6B5C File Offset: 0x003E4D5C
	public Vector3d(Vector3 _v)
	{
		this.x = (double)_v.x;
		this.y = (double)_v.y;
		this.z = (double)_v.z;
	}

	// Token: 0x0600A7B1 RID: 42929 RVA: 0x003E6B85 File Offset: 0x003E4D85
	public Vector3d(Vector3i _v)
	{
		this.x = (double)_v.x;
		this.y = (double)_v.y;
		this.z = (double)_v.z;
	}

	// Token: 0x0600A7B2 RID: 42930 RVA: 0x003E6BAE File Offset: 0x003E4DAE
	public bool Equals(double _x, double _y, double _z)
	{
		return this.x == _x && this.y == _y && this.z == _z;
	}

	// Token: 0x0600A7B3 RID: 42931 RVA: 0x003E6BD0 File Offset: 0x003E4DD0
	public override bool Equals(object obj)
	{
		Vector3d vector3d = (Vector3d)obj;
		return this.Equals(vector3d);
	}

	// Token: 0x0600A7B4 RID: 42932 RVA: 0x003E6BEB File Offset: 0x003E4DEB
	public bool Equals(Vector3d other)
	{
		return other.x == this.x && other.y == this.y && other.z == this.z;
	}

	// Token: 0x0600A7B5 RID: 42933 RVA: 0x003E6C19 File Offset: 0x003E4E19
	public static bool operator ==(Vector3d one, Vector3d other)
	{
		return one.x == other.x && one.y == other.y && one.z == other.z;
	}

	// Token: 0x0600A7B6 RID: 42934 RVA: 0x003E6C47 File Offset: 0x003E4E47
	public override int GetHashCode()
	{
		return this.x.GetHashCode() ^ (this.y.GetHashCode() << 2) ^ (this.z.GetHashCode() >> 2);
	}

	// Token: 0x0600A7B7 RID: 42935 RVA: 0x003E6C70 File Offset: 0x003E4E70
	public static bool operator !=(Vector3d one, Vector3d other)
	{
		return !(one == other);
	}

	// Token: 0x0600A7B8 RID: 42936 RVA: 0x003E6C7C File Offset: 0x003E4E7C
	public static Vector3d operator -(Vector3d one, Vector3d other)
	{
		return new Vector3d(one.x - other.x, one.y - other.y, one.z - other.z);
	}

	// Token: 0x0600A7B9 RID: 42937 RVA: 0x003E6CAA File Offset: 0x003E4EAA
	public static Vector3d operator +(Vector3d one, Vector3d other)
	{
		return new Vector3d(one.x + other.x, one.y + other.y, one.z + other.z);
	}

	// Token: 0x0600A7BA RID: 42938 RVA: 0x003E6CD8 File Offset: 0x003E4ED8
	public static Vector3d operator *(Vector3d a, double d)
	{
		return new Vector3d(a.x * d, a.y * d, a.z * d);
	}

	// Token: 0x0600A7BB RID: 42939 RVA: 0x003E6CF7 File Offset: 0x003E4EF7
	public static Vector3d operator *(double d, Vector3d a)
	{
		return new Vector3d(a.x * d, a.y * d, a.z * d);
	}

	// Token: 0x0600A7BC RID: 42940 RVA: 0x003E6D16 File Offset: 0x003E4F16
	public override string ToString()
	{
		return this.ToCultureInvariantString();
	}

	// Token: 0x0600A7BD RID: 42941 RVA: 0x003E6D20 File Offset: 0x003E4F20
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

	// Token: 0x0600A7BE RID: 42942 RVA: 0x003E6D94 File Offset: 0x003E4F94
	public static Vector3d Cross(Vector3d _a, Vector3d _b)
	{
		return new Vector3d(_a.y * _b.z - _a.z * _b.y, _a.z * _b.x - _a.x * _b.z, _a.x * _b.y - _a.y * _b.x);
	}

	// Token: 0x04007C0E RID: 31758
	public double x;

	// Token: 0x04007C0F RID: 31759
	public double y;

	// Token: 0x04007C10 RID: 31760
	public double z;

	// Token: 0x04007C11 RID: 31761
	public static readonly Vector3d Zero = new Vector3d(0.0, 0.0, 0.0);
}
