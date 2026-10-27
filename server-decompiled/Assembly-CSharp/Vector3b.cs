using System;
using System.Runtime.CompilerServices;

// Token: 0x02001526 RID: 5414
public struct Vector3b : IEquatable<Vector3b>
{
	// Token: 0x0600A9B1 RID: 43441 RVA: 0x003F302D File Offset: 0x003F122D
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3b(byte _x, byte _y, byte _z)
	{
		this.x = _x;
		this.y = _y;
		this.z = _z;
	}

	// Token: 0x0600A9B2 RID: 43442 RVA: 0x003F3044 File Offset: 0x003F1244
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3b(int _x, int _y, int _z)
	{
		this.x = (byte)_x;
		this.y = (byte)_y;
		this.z = (byte)_z;
	}

	// Token: 0x0600A9B3 RID: 43443 RVA: 0x003F305E File Offset: 0x003F125E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3i ToVector3i()
	{
		return new Vector3i((int)this.x, (int)this.y, (int)this.z);
	}

	// Token: 0x0600A9B4 RID: 43444 RVA: 0x003F3077 File Offset: 0x003F1277
	public override int GetHashCode()
	{
		return ((int)this.x << 16) | ((int)this.y << 8) | (int)this.z;
	}

	// Token: 0x0600A9B5 RID: 43445 RVA: 0x003F3092 File Offset: 0x003F1292
	public override bool Equals(object obj)
	{
		return obj != null && obj is Vector3b && this.Equals((Vector3b)obj);
	}

	// Token: 0x0600A9B6 RID: 43446 RVA: 0x003F30AF File Offset: 0x003F12AF
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(Vector3b other)
	{
		return this.x == other.x && this.y == other.y && this.z == other.z;
	}

	// Token: 0x04007DA8 RID: 32168
	public byte x;

	// Token: 0x04007DA9 RID: 32169
	public byte y;

	// Token: 0x04007DAA RID: 32170
	public byte z;
}
