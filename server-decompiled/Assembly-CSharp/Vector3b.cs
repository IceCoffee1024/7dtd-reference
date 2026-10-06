using System;
using System.Runtime.CompilerServices;

// Token: 0x02001514 RID: 5396
public struct Vector3b : IEquatable<Vector3b>
{
	// Token: 0x0600A959 RID: 43353 RVA: 0x003F0C29 File Offset: 0x003EEE29
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3b(byte _x, byte _y, byte _z)
	{
		this.x = _x;
		this.y = _y;
		this.z = _z;
	}

	// Token: 0x0600A95A RID: 43354 RVA: 0x003F0C40 File Offset: 0x003EEE40
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3b(int _x, int _y, int _z)
	{
		this.x = (byte)_x;
		this.y = (byte)_y;
		this.z = (byte)_z;
	}

	// Token: 0x0600A95B RID: 43355 RVA: 0x003F0C5A File Offset: 0x003EEE5A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3i ToVector3i()
	{
		return new Vector3i((int)this.x, (int)this.y, (int)this.z);
	}

	// Token: 0x0600A95C RID: 43356 RVA: 0x003F0C73 File Offset: 0x003EEE73
	public override int GetHashCode()
	{
		return ((int)this.x << 16) | ((int)this.y << 8) | (int)this.z;
	}

	// Token: 0x0600A95D RID: 43357 RVA: 0x003F0C8E File Offset: 0x003EEE8E
	public override bool Equals(object obj)
	{
		return obj != null && obj is Vector3b && this.Equals((Vector3b)obj);
	}

	// Token: 0x0600A95E RID: 43358 RVA: 0x003F0CAB File Offset: 0x003EEEAB
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(Vector3b other)
	{
		return this.x == other.x && this.y == other.y && this.z == other.z;
	}

	// Token: 0x04007D67 RID: 32103
	public byte x;

	// Token: 0x04007D68 RID: 32104
	public byte y;

	// Token: 0x04007D69 RID: 32105
	public byte z;
}
