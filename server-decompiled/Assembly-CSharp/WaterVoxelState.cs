using System;
using Unity.Mathematics;

// Token: 0x02000CC8 RID: 3272
public struct WaterVoxelState : IEquatable<WaterVoxelState>
{
	// Token: 0x06006478 RID: 25720 RVA: 0x00274C2F File Offset: 0x00272E2F
	public WaterVoxelState(byte stateBits)
	{
		this.stateBits = stateBits;
	}

	// Token: 0x06006479 RID: 25721 RVA: 0x00274C38 File Offset: 0x00272E38
	public WaterVoxelState(WaterVoxelState other)
	{
		this.stateBits = other.stateBits;
	}

	// Token: 0x0600647A RID: 25722 RVA: 0x00274C46 File Offset: 0x00272E46
	public bool IsDefault()
	{
		return this.stateBits == 0;
	}

	// Token: 0x0600647B RID: 25723 RVA: 0x00274C51 File Offset: 0x00272E51
	public bool IsSolidYPos()
	{
		return (this.stateBits & 1) > 0;
	}

	// Token: 0x0600647C RID: 25724 RVA: 0x00274C5E File Offset: 0x00272E5E
	public bool IsSolidYNeg()
	{
		return (this.stateBits & 2) > 0;
	}

	// Token: 0x0600647D RID: 25725 RVA: 0x00274C6B File Offset: 0x00272E6B
	public bool IsSolidXPos()
	{
		return (this.stateBits & 32) > 0;
	}

	// Token: 0x0600647E RID: 25726 RVA: 0x00274C79 File Offset: 0x00272E79
	public bool IsSolidXNeg()
	{
		return (this.stateBits & 8) > 0;
	}

	// Token: 0x0600647F RID: 25727 RVA: 0x00274C86 File Offset: 0x00272E86
	public bool IsSolidZPos()
	{
		return (this.stateBits & 4) > 0;
	}

	// Token: 0x06006480 RID: 25728 RVA: 0x00274C93 File Offset: 0x00272E93
	public bool IsSolidZNeg()
	{
		return (this.stateBits & 16) > 0;
	}

	// Token: 0x06006481 RID: 25729 RVA: 0x00274CA4 File Offset: 0x00272EA4
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

	// Token: 0x06006482 RID: 25730 RVA: 0x00274CF7 File Offset: 0x00272EF7
	public bool IsSolid()
	{
		return this.stateBits != 0 && (~this.stateBits & 63) == 0;
	}

	// Token: 0x06006483 RID: 25731 RVA: 0x00274D10 File Offset: 0x00272F10
	public void SetSolid(BlockFaceFlag flags)
	{
		this.stateBits = (byte)flags;
	}

	// Token: 0x06006484 RID: 25732 RVA: 0x00274D1A File Offset: 0x00272F1A
	public void SetSolidMask(BlockFaceFlag mask, bool value)
	{
		if (value)
		{
			this.stateBits |= (byte)mask;
			return;
		}
		this.stateBits &= (byte)(~(byte)mask);
	}

	// Token: 0x06006485 RID: 25733 RVA: 0x00274D41 File Offset: 0x00272F41
	public bool Equals(WaterVoxelState other)
	{
		return this.stateBits == other.stateBits;
	}

	// Token: 0x04004E2B RID: 20011
	[PublicizedFrom(EAccessModifier.Private)]
	public byte stateBits;
}
