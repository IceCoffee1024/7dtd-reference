using System;
using Unity.Mathematics;

// Token: 0x02000CB6 RID: 3254
public struct WaterVoxelState : IEquatable<WaterVoxelState>
{
	// Token: 0x060063EC RID: 25580 RVA: 0x00271D33 File Offset: 0x0026FF33
	public WaterVoxelState(byte stateBits)
	{
		this.stateBits = stateBits;
	}

	// Token: 0x060063ED RID: 25581 RVA: 0x00271D3C File Offset: 0x0026FF3C
	public WaterVoxelState(WaterVoxelState other)
	{
		this.stateBits = other.stateBits;
	}

	// Token: 0x060063EE RID: 25582 RVA: 0x00271D4A File Offset: 0x0026FF4A
	public bool IsDefault()
	{
		return this.stateBits == 0;
	}

	// Token: 0x060063EF RID: 25583 RVA: 0x00271D55 File Offset: 0x0026FF55
	public bool IsSolidYPos()
	{
		return (this.stateBits & 1) > 0;
	}

	// Token: 0x060063F0 RID: 25584 RVA: 0x00271D62 File Offset: 0x0026FF62
	public bool IsSolidYNeg()
	{
		return (this.stateBits & 2) > 0;
	}

	// Token: 0x060063F1 RID: 25585 RVA: 0x00271D6F File Offset: 0x0026FF6F
	public bool IsSolidXPos()
	{
		return (this.stateBits & 32) > 0;
	}

	// Token: 0x060063F2 RID: 25586 RVA: 0x00271D7D File Offset: 0x0026FF7D
	public bool IsSolidXNeg()
	{
		return (this.stateBits & 8) > 0;
	}

	// Token: 0x060063F3 RID: 25587 RVA: 0x00271D8A File Offset: 0x0026FF8A
	public bool IsSolidZPos()
	{
		return (this.stateBits & 4) > 0;
	}

	// Token: 0x060063F4 RID: 25588 RVA: 0x00271D97 File Offset: 0x0026FF97
	public bool IsSolidZNeg()
	{
		return (this.stateBits & 16) > 0;
	}

	// Token: 0x060063F5 RID: 25589 RVA: 0x00271DA8 File Offset: 0x0026FFA8
	public bool IsSolidXZ(int2 side)
	{
		if (side.x > 0)
		{
			return this.IsSolidXPos();
		}
		if (side.x < 0)
		{
			return this.IsSolidXNeg();
		}
		if (side.y > 0)
		{
			return this.IsSolidZPos();
		}
		if (side.y < 0)
		{
			return this.IsSolidZNeg();
		}
		return this.IsSolid();
	}

	// Token: 0x060063F6 RID: 25590 RVA: 0x00271DFB File Offset: 0x0026FFFB
	public bool IsSolid()
	{
		return this.stateBits != 0 && (~this.stateBits & 63) == 0;
	}

	// Token: 0x060063F7 RID: 25591 RVA: 0x00271E14 File Offset: 0x00270014
	public void SetSolid(BlockFaceFlag flags)
	{
		this.stateBits = (byte)flags;
	}

	// Token: 0x060063F8 RID: 25592 RVA: 0x00271E1E File Offset: 0x0027001E
	public void SetSolidMask(BlockFaceFlag mask, bool value)
	{
		if (value)
		{
			this.stateBits |= (byte)mask;
			return;
		}
		this.stateBits &= (byte)(~(byte)mask);
	}

	// Token: 0x060063F9 RID: 25593 RVA: 0x00271E45 File Offset: 0x00270045
	public bool Equals(WaterVoxelState other)
	{
		return this.stateBits == other.stateBits;
	}

	// Token: 0x04004D8D RID: 19853
	[PublicizedFrom(EAccessModifier.Private)]
	public byte stateBits;
}
