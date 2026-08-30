using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000C1D RID: 3101
public static class DecoUtils
{
	// Token: 0x06005EA9 RID: 24233 RVA: 0x0024EBB0 File Offset: 0x0024CDB0
	public static bool IsBigDeco(BlockValue blockValue, Block block)
	{
		return block.SmallDecorationRadius > 0 || block.BigDecorationRadius > 0 || block.isOversized;
	}

	// Token: 0x06005EAA RID: 24234 RVA: 0x0024EBCC File Offset: 0x0024CDCC
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

	// Token: 0x06005EAB RID: 24235 RVA: 0x0024EC26 File Offset: 0x0024CE26
	public static bool CanPlaceDeco(Chunk cX0Z0, Chunk cX1Z0, Chunk cX0Z1, Chunk cX1Z1, Vector3i blockPos, BlockValue blockValue, DecoUtils.DecoAllowedTest additionalTest = null)
	{
		return DecoUtils.CanPlaceDeco(cX0Z0, blockPos, blockValue, additionalTest) && DecoUtils.CanPlaceDeco(cX1Z0, blockPos, blockValue, additionalTest) && DecoUtils.CanPlaceDeco(cX0Z1, blockPos, blockValue, additionalTest) && DecoUtils.CanPlaceDeco(cX1Z1, blockPos, blockValue, additionalTest);
	}

	// Token: 0x06005EAC RID: 24236 RVA: 0x0024EC60 File Offset: 0x0024CE60
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

	// Token: 0x06005EAD RID: 24237 RVA: 0x0024EDA0 File Offset: 0x0024CFA0
	public static bool HasDecoAllowed(BlockValue blockValue)
	{
		if (blockValue.isair)
		{
			return false;
		}
		Block block = blockValue.Block;
		return (!block.isMultiBlock || !blockValue.ischild) && (block.SmallDecorationRadius > 0 || block.BigDecorationRadius > 0 || block.isOversized);
	}

	// Token: 0x06005EAE RID: 24238 RVA: 0x0024EDED File Offset: 0x0024CFED
	public static void ApplyDecoAllowed(Chunk cX0Z0, Chunk cX1Z0, Chunk cX0Z1, Chunk cX1Z1, Vector3i blockPos, BlockValue blockValue)
	{
		DecoUtils.ApplyDecoAllowed(cX0Z0, blockPos, blockValue);
		DecoUtils.ApplyDecoAllowed(cX1Z0, blockPos, blockValue);
		DecoUtils.ApplyDecoAllowed(cX0Z1, blockPos, blockValue);
		DecoUtils.ApplyDecoAllowed(cX1Z1, blockPos, blockValue);
	}

	// Token: 0x06005EAF RID: 24239 RVA: 0x0024EE18 File Offset: 0x0024D018
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

	// Token: 0x06005EB0 RID: 24240 RVA: 0x0024EE78 File Offset: 0x0024D078
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

	// Token: 0x06005EB1 RID: 24241 RVA: 0x0024EF58 File Offset: 0x0024D158
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

	// Token: 0x06005EB2 RID: 24242 RVA: 0x0024F04C File Offset: 0x0024D24C
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

	// Token: 0x06005EB3 RID: 24243 RVA: 0x0024F104 File Offset: 0x0024D304
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

	// Token: 0x06005EB4 RID: 24244 RVA: 0x0024F1A0 File Offset: 0x0024D3A0
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

	// Token: 0x02000C1E RID: 3102
	// (Invoke) Token: 0x06005EB6 RID: 24246
	public delegate bool DecoAllowedTest(EnumDecoAllowed decoAllowed);
}
