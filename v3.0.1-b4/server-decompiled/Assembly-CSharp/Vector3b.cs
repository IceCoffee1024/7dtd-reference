using System;
using System.Runtime.CompilerServices;

// Token: 0x020014D7 RID: 5335
public struct Vector3b : IEquatable<Vector3b>
{
	// Token: 0x0600A7A9 RID: 42921 RVA: 0x003E6A95 File Offset: 0x003E4C95
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3b(byte _x, byte _y, byte _z)
	{
		this.x = _x;
		this.y = _y;
		this.z = _z;
	}

	// Token: 0x0600A7AA RID: 42922 RVA: 0x003E6AAC File Offset: 0x003E4CAC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3b(int _x, int _y, int _z)
	{
		this.x = (byte)_x;
		this.y = (byte)_y;
		this.z = (byte)_z;
	}

	// Token: 0x0600A7AB RID: 42923 RVA: 0x003E6AC6 File Offset: 0x003E4CC6
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3i ToVector3i()
	{
		return new Vector3i((int)this.x, (int)this.y, (int)this.z);
	}

	// Token: 0x0600A7AC RID: 42924 RVA: 0x003E6ADF File Offset: 0x003E4CDF
	public override int GetHashCode()
	{
		return ((int)this.x << 16) | ((int)this.y << 8) | (int)this.z;
	}

	// Token: 0x0600A7AD RID: 42925 RVA: 0x003E6AFA File Offset: 0x003E4CFA
	public override bool Equals(object obj)
	{
		return obj != null && obj is Vector3b && this.Equals((Vector3b)obj);
	}

	// Token: 0x0600A7AE RID: 42926 RVA: 0x003E6B17 File Offset: 0x003E4D17
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(Vector3b other)
	{
		return this.x == other.x && this.y == other.y && this.z == other.z;
	}

	// Token: 0x04007C0B RID: 31755
	public byte x;

	// Token: 0x04007C0C RID: 31756
	public byte y;

	// Token: 0x04007C0D RID: 31757
	public byte z;
}
