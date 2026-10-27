using System;
using Unity.Mathematics;

// Token: 0x02000CE1 RID: 3297
public struct WaterVoxelState : IEquatable<WaterVoxelState>
{
	// Token: 0x06006490 RID: 25744 RVA: 0x0027955B File Offset: 0x0027775B
	public WaterVoxelState(byte stateBits)
	{
		this.stateBits = stateBits;
	}

	// Token: 0x06006491 RID: 25745 RVA: 0x00279564 File Offset: 0x00277764
	public WaterVoxelState(WaterVoxelState other)
	{
		this.stateBits = other.stateBits;
	}

	// Token: 0x06006492 RID: 25746 RVA: 0x00279572 File Offset: 0x00277772
	public bool IsDefault()
	{
		return this.stateBits == 0;
	}

	// Token: 0x06006493 RID: 25747 RVA: 0x0027957D File Offset: 0x0027777D
	public bool IsSolidYPos()
	{
		return (this.stateBits & 1) > 0;
	}

	// Token: 0x06006494 RID: 25748 RVA: 0x0027958A File Offset: 0x0027778A
	public bool IsSolidYNeg()
	{
		return (this.stateBits & 2) > 0;
	}

	// Token: 0x06006495 RID: 25749 RVA: 0x00279597 File Offset: 0x00277797
	public bool IsSolidXPos()
	{
		return (this.stateBits & 32) > 0;
	}

	// Token: 0x06006496 RID: 25750 RVA: 0x002795A5 File Offset: 0x002777A5
	public bool IsSolidXNeg()
	{
		return (this.stateBits & 8) > 0;
	}

	// Token: 0x06006497 RID: 25751 RVA: 0x002795B2 File Offset: 0x002777B2
	public bool IsSolidZPos()
	{
		return (this.stateBits & 4) > 0;
	}

	// Token: 0x06006498 RID: 25752 RVA: 0x002795BF File Offset: 0x002777BF
	public bool IsSolidZNeg()
	{
		return (this.stateBits & 16) > 0;
	}

	// Token: 0x06006499 RID: 25753 RVA: 0x002795D0 File Offset: 0x002777D0
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

	// Token: 0x0600649A RID: 25754 RVA: 0x00279623 File Offset: 0x00277823
	public bool IsSolid()
	{
		return this.stateBits != 0 && (~this.stateBits & 63) == 0;
	}

	// Token: 0x0600649B RID: 25755 RVA: 0x0027963C File Offset: 0x0027783C
	public void SetSolid(BlockFaceFlag flags)
	{
		this.stateBits = (byte)flags;
	}

	// Token: 0x0600649C RID: 25756 RVA: 0x00279646 File Offset: 0x00277846
	public void SetSolidMask(BlockFaceFlag mask, bool value)
	{
		if (value)
		{
			this.stateBits |= (byte)mask;
			return;
		}
		this.stateBits &= (byte)(~(byte)mask);
	}

	// Token: 0x0600649D RID: 25757 RVA: 0x0027966D File Offset: 0x0027786D
	public bool Equals(WaterVoxelState other)
	{
		return this.stateBits == other.stateBits;
	}

	// Token: 0x04004E86 RID: 20102
	[PublicizedFrom(EAccessModifier.Private)]
	public byte stateBits;
}
