using System;
using System.Runtime.CompilerServices;

// Token: 0x020014F2 RID: 5362
public struct Vector3b : IEquatable<Vector3b>
{
	// Token: 0x0600A8A6 RID: 43174 RVA: 0x003EBDD5 File Offset: 0x003E9FD5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3b(byte _x, byte _y, byte _z)
	{
		this.x = _x;
		this.y = _y;
		this.z = _z;
	}

	// Token: 0x0600A8A7 RID: 43175 RVA: 0x003EBDEC File Offset: 0x003E9FEC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3b(int _x, int _y, int _z)
	{
		this.x = (byte)_x;
		this.y = (byte)_y;
		this.z = (byte)_z;
	}

	// Token: 0x0600A8A8 RID: 43176 RVA: 0x003EBE06 File Offset: 0x003EA006
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector3i ToVector3i()
	{
		return new Vector3i((int)this.x, (int)this.y, (int)this.z);
	}

	// Token: 0x0600A8A9 RID: 43177 RVA: 0x003EBE1F File Offset: 0x003EA01F
	public override int GetHashCode()
	{
		return ((int)this.x << 16) | ((int)this.y << 8) | (int)this.z;
	}

	// Token: 0x0600A8AA RID: 43178 RVA: 0x003EBE3A File Offset: 0x003EA03A
	public override bool Equals(object obj)
	{
		return obj != null && obj is Vector3b && this.Equals((Vector3b)obj);
	}

	// Token: 0x0600A8AB RID: 43179 RVA: 0x003EBE57 File Offset: 0x003EA057
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(Vector3b other)
	{
		return this.x == other.x && this.y == other.y && this.z == other.z;
	}

	// Token: 0x04007CE4 RID: 31972
	public byte x;

	// Token: 0x04007CE5 RID: 31973
	public byte y;

	// Token: 0x04007CE6 RID: 31974
	public byte z;
}
