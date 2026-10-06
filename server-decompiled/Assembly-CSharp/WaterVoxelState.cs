using System;
using Unity.Mathematics;

// Token: 0x02000CD6 RID: 3286
public struct WaterVoxelState : IEquatable<WaterVoxelState>
{
	// Token: 0x0600646B RID: 25707 RVA: 0x00278033 File Offset: 0x00276233
	public WaterVoxelState(byte stateBits)
	{
		this.stateBits = stateBits;
	}

	// Token: 0x0600646C RID: 25708 RVA: 0x0027803C File Offset: 0x0027623C
	public WaterVoxelState(WaterVoxelState other)
	{
		this.stateBits = other.stateBits;
	}

	// Token: 0x0600646D RID: 25709 RVA: 0x0027804A File Offset: 0x0027624A
	public bool IsDefault()
	{
		return this.stateBits == 0;
	}

	// Token: 0x0600646E RID: 25710 RVA: 0x00278055 File Offset: 0x00276255
	public bool IsSolidYPos()
	{
		return (this.stateBits & 1) > 0;
	}

	// Token: 0x0600646F RID: 25711 RVA: 0x00278062 File Offset: 0x00276262
	public bool IsSolidYNeg()
	{
		return (this.stateBits & 2) > 0;
	}

	// Token: 0x06006470 RID: 25712 RVA: 0x0027806F File Offset: 0x0027626F
	public bool IsSolidXPos()
	{
		return (this.stateBits & 32) > 0;
	}

	// Token: 0x06006471 RID: 25713 RVA: 0x0027807D File Offset: 0x0027627D
	public bool IsSolidXNeg()
	{
		return (this.stateBits & 8) > 0;
	}

	// Token: 0x06006472 RID: 25714 RVA: 0x0027808A File Offset: 0x0027628A
	public bool IsSolidZPos()
	{
		return (this.stateBits & 4) > 0;
	}

	// Token: 0x06006473 RID: 25715 RVA: 0x00278097 File Offset: 0x00276297
	public bool IsSolidZNeg()
	{
		return (this.stateBits & 16) > 0;
	}

	// Token: 0x06006474 RID: 25716 RVA: 0x002780A8 File Offset: 0x002762A8
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

	// Token: 0x06006475 RID: 25717 RVA: 0x002780FB File Offset: 0x002762FB
	public bool IsSolid()
	{
		return this.stateBits != 0 && (~this.stateBits & 63) == 0;
	}

	// Token: 0x06006476 RID: 25718 RVA: 0x00278114 File Offset: 0x00276314
	public void SetSolid(BlockFaceFlag flags)
	{
		this.stateBits = (byte)flags;
	}

	// Token: 0x06006477 RID: 25719 RVA: 0x0027811E File Offset: 0x0027631E
	public void SetSolidMask(BlockFaceFlag mask, bool value)
	{
		if (value)
		{
			this.stateBits |= (byte)mask;
			return;
		}
		this.stateBits &= (byte)(~(byte)mask);
	}

	// Token: 0x06006478 RID: 25720 RVA: 0x00278145 File Offset: 0x00276345
	public bool Equals(WaterVoxelState other)
	{
		return this.stateBits == other.stateBits;
	}

	// Token: 0x04004E61 RID: 20065
	[PublicizedFrom(EAccessModifier.Private)]
	public byte stateBits;
}
