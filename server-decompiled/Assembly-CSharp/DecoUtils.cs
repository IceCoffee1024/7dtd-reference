using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000C3A RID: 3130
public static class DecoUtils
{
	// Token: 0x06005EE7 RID: 24295 RVA: 0x002533D4 File Offset: 0x002515D4
	public static bool IsBigDeco(BlockValue blockValue, Block block)
	{
		return block.SmallDecorationRadius > 0 || block.BigDecorationRadius > 0 || block.isOversized;
	}

	// Token: 0x06005EE8 RID: 24296 RVA: 0x002533F0 File Offset: 0x002515F0
	public static int GetDecoRadius(BlockValue blockValue, Block block)
	{
		int num = Math.Max(block.SmallDecorationRadius, block.BigDecorationRadius);
		if (block.isOversized)
		{
			Vector3 extents = block.oversizedBounds.extents;
			num = Math.Max(num, Math.Max((int)(extents.x + 0.5f), (int)(extents.z + 0.5f)));
		}
		return num;
	}

	// Token: 0x06005EE9 RID: 24297 RVA: 0x0025344A File Offset: 0x0025164A
	public static bool CanPlaceDeco(Chunk cX0Z0, Chunk cX1Z0, Chunk cX0Z1, Chunk cX1Z1, Vector3i blockPos, BlockValue blockValue, DecoUtils.DecoAllowedTest additionalTest = null)
	{
		return DecoUtils.CanPlaceDeco(cX0Z0, blockPos, blockValue, additionalTest) && DecoUtils.CanPlaceDeco(cX1Z0, blockPos, blockValue, additionalTest) && DecoUtils.CanPlaceDeco(cX0Z1, blockPos, blockValue, additionalTest) && DecoUtils.CanPlaceDeco(cX1Z1, blockPos, blockValue, additionalTest);
	}

	// Token: 0x06005EEA RID: 24298 RVA: 0x00253484 File Offset: 0x00251684
	public static bool CanPlaceDeco(Chunk chunk, Vector3i blockPos, BlockValue blockValue, DecoUtils.DecoAllowedTest additionalTest = null)
	{
		DecoUtils.<>c__DisplayClass4_0 CS$<>8__locals1;
		CS$<>8__locals1.chunk = chunk;
		CS$<>8__locals1.additionalTest = additionalTest;
		if (blockValue.isair)
		{
			return false;
		}
		Block block = blockValue.Block;
		if (block.isMultiBlock && blockValue.ischild)
		{
			return false;
		}
		int num = CS$<>8__locals1.chunk.X * 16;
		int num2 = CS$<>8__locals1.chunk.Z * 16;
		CS$<>8__locals1.x = blockPos.x - num;
		CS$<>8__locals1.z = blockPos.z - num2;
		if (DecoUtils.IsBigDeco(blockValue, block))
		{
			int num3 = num + 16 - 1;
			int num4 = num2 + 16 - 1;
			return DecoUtils.<CanPlaceDeco>g__CanPlaceBigDecoForBlockPos|4_0(ref CS$<>8__locals1) && DecoUtils.CanPlaceBigDecoForBlockDecorationRadius(CS$<>8__locals1.chunk, num, num2, num3, num4, blockPos, blockValue, block, CS$<>8__locals1.additionalTest) && DecoUtils.CanPlaceBigDecoForBlockOversized(CS$<>8__locals1.chunk, num, num2, num3, num4, blockPos, blockValue, block, CS$<>8__locals1.additionalTest);
		}
		if (CS$<>8__locals1.x < 0 || CS$<>8__locals1.x >= 16 || CS$<>8__locals1.z < 0 || CS$<>8__locals1.z >= 16)
		{
			return true;
		}
		EnumDecoAllowed decoAllowedAt = CS$<>8__locals1.chunk.GetDecoAllowedAt(CS$<>8__locals1.x, CS$<>8__locals1.z);
		return decoAllowedAt.AllowSmallDeco() && (CS$<>8__locals1.additionalTest == null || CS$<>8__locals1.additionalTest(decoAllowedAt));
	}

	// Token: 0x06005EEB RID: 24299 RVA: 0x002535C4 File Offset: 0x002517C4
	public static bool HasDecoAllowed(BlockValue blockValue)
	{
		if (blockValue.isair)
		{
			return false;
		}
		Block block = blockValue.Block;
		return (!block.isMultiBlock || !blockValue.ischild) && (block.SmallDecorationRadius > 0 || block.BigDecorationRadius > 0 || block.isOversized);
	}

	// Token: 0x06005EEC RID: 24300 RVA: 0x00253611 File Offset: 0x00251811
	public static void ApplyDecoAllowed(Chunk cX0Z0, Chunk cX1Z0, Chunk cX0Z1, Chunk cX1Z1, Vector3i blockPos, BlockValue blockValue)
	{
		DecoUtils.ApplyDecoAllowed(cX0Z0, blockPos, blockValue);
		DecoUtils.ApplyDecoAllowed(cX1Z0, blockPos, blockValue);
		DecoUtils.ApplyDecoAllowed(cX0Z1, blockPos, blockValue);
		DecoUtils.ApplyDecoAllowed(cX1Z1, blockPos, blockValue);
	}

	// Token: 0x06005EED RID: 24301 RVA: 0x0025363C File Offset: 0x0025183C
	public static void ApplyDecoAllowed(Chunk chunk, Vector3i blockPos, BlockValue blockValue)
	{
		if (!DecoUtils.HasDecoAllowed(blockValue))
		{
			return;
		}
		Block block = blockValue.Block;
		int num = chunk.X * 16;
		int num2 = chunk.Z * 16;
		int num3 = num + 16 - 1;
		int num4 = num2 + 16 - 1;
		DecoUtils.ApplyDecoAllowedForBlockDecorationRadius(chunk, num, num2, num3, num4, blockPos, blockValue, block);
		DecoUtils.ApplyDecoAllowedForBlockOversized(chunk, num, num2, num3, num4, blockPos, blockValue, block);
	}

	// Token: 0x06005EEE RID: 24302 RVA: 0x0025369C File Offset: 0x0025189C
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool CanPlaceBigDecoForBlockDecorationRadius(Chunk chunk, int cxMin, int czMin, int cxMax, int czMax, Vector3i blockPos, BlockValue blockValue, Block block, DecoUtils.DecoAllowedTest additionalTest)
	{
		int num = Math.Max(block.SmallDecorationRadius, block.BigDecorationRadius);
		if (num <= 0)
		{
			return true;
		}
		int num2 = blockPos.x - num;
		int num3 = blockPos.z - num;
		int num4 = blockPos.x + num;
		int num5 = blockPos.z + num;
		if (num2 > cxMax || num3 > czMax || num4 < cxMin || num5 < czMin)
		{
			return true;
		}
		int num6 = Math.Clamp(num2 - cxMin, 0, 15);
		int num7 = Math.Clamp(num3 - czMin, 0, 15);
		int num8 = Math.Clamp(num4 - cxMin, 0, 15);
		int num9 = Math.Clamp(num5 - czMin, 0, 15);
		for (int i = num7; i <= num9; i++)
		{
			for (int j = num6; j <= num8; j++)
			{
				EnumDecoAllowed decoAllowedAt = chunk.GetDecoAllowedAt(j, i);
				if (!decoAllowedAt.AllowBigDeco() && (additionalTest == null || additionalTest(decoAllowedAt)))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06005EEF RID: 24303 RVA: 0x0025377C File Offset: 0x0025197C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ApplyDecoAllowedForBlockDecorationRadius(Chunk chunk, int cxMin, int czMin, int cxMax, int czMax, Vector3i blockPos, BlockValue blockValue, Block block)
	{
		int smallDecorationRadius = block.SmallDecorationRadius;
		int num = Math.Max(smallDecorationRadius, block.BigDecorationRadius);
		if (num <= 0)
		{
			return;
		}
		int num2 = blockPos.x - num;
		int num3 = blockPos.z - num;
		int num4 = blockPos.x + num;
		int num5 = blockPos.z + num;
		if (num2 > cxMax || num3 > czMax || num4 < cxMin || num5 < czMin)
		{
			return;
		}
		int num6 = Math.Clamp(num2 - cxMin, 0, 15);
		int num7 = Math.Clamp(num3 - czMin, 0, 15);
		int num8 = Math.Clamp(num4 - cxMin, 0, 15);
		int num9 = Math.Clamp(num5 - czMin, 0, 15);
		for (int i = num7; i <= num9; i++)
		{
			for (int j = num6; j <= num8; j++)
			{
				if (smallDecorationRadius == num || (smallDecorationRadius > 0 && Math.Max(Math.Abs(i), Math.Abs(j)) <= smallDecorationRadius))
				{
					chunk.SetDecoAllowedSizeAt(j, i, EnumDecoAllowedSize.None);
				}
				else
				{
					chunk.SetDecoAllowedSizeAt(j, i, EnumDecoAllowedSize.OnlySmall);
				}
			}
		}
	}

	// Token: 0x06005EF0 RID: 24304 RVA: 0x00253870 File Offset: 0x00251A70
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool CanPlaceBigDecoForBlockOversized(Chunk chunk, int cxMin, int czMin, int cxMax, int czMax, Vector3i blockPos, BlockValue blockValue, Block block, DecoUtils.DecoAllowedTest additionalTest)
	{
		Bounds bounds = default(Bounds);
		bounds.SetMinMax(new Vector3((float)cxMin, 0f, (float)czMin), new Vector3((float)cxMax, 0f, (float)czMax));
		foreach (Vector3i vector3i in OversizedBlockUtils.EnumerateOverlappingCells(blockPos, block.oversizedBounds, blockValue.rotation, bounds))
		{
			EnumDecoAllowed decoAllowedAt = chunk.GetDecoAllowedAt(vector3i.x - cxMin, vector3i.z - czMin);
			if (!decoAllowedAt.AllowBigDeco() && (additionalTest == null || additionalTest(decoAllowedAt)))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06005EF1 RID: 24305 RVA: 0x00253928 File Offset: 0x00251B28
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ApplyDecoAllowedForBlockOversized(Chunk chunk, int cxMin, int czMin, int cxMax, int czMax, Vector3i blockPos, BlockValue blockValue, Block block)
	{
		Bounds bounds = default(Bounds);
		bounds.SetMinMax(new Vector3((float)cxMin, 0f, (float)czMin), new Vector3((float)cxMax, 0f, (float)czMax));
		foreach (Vector3i vector3i in OversizedBlockUtils.EnumerateOverlappingCells(blockPos, block.oversizedBounds, blockValue.rotation, bounds))
		{
			chunk.SetDecoAllowedSizeAt(vector3i.x - cxMin, vector3i.z - czMin, EnumDecoAllowedSize.None);
		}
	}

	// Token: 0x06005EF2 RID: 24306 RVA: 0x002539C4 File Offset: 0x00251BC4
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static bool <CanPlaceDeco>g__CanPlaceBigDecoForBlockPos|4_0(ref DecoUtils.<>c__DisplayClass4_0 A_0)
	{
		if (A_0.x < 0 || A_0.x >= 16 || A_0.z < 0 || A_0.z >= 16)
		{
			return true;
		}
		EnumDecoAllowed decoAllowedAt = A_0.chunk.GetDecoAllowedAt(A_0.x, A_0.z);
		return decoAllowedAt.AllowBigDeco() && (A_0.additionalTest == null || A_0.additionalTest(decoAllowedAt));
	}

	// Token: 0x02000C3B RID: 3131
	// (Invoke) Token: 0x06005EF4 RID: 24308
	public delegate bool DecoAllowedTest(EnumDecoAllowed decoAllowed);
}
