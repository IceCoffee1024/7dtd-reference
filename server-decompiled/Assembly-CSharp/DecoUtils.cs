using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000C2F RID: 3119
public static class DecoUtils
{
	// Token: 0x06005EC2 RID: 24258 RVA: 0x00251EAC File Offset: 0x002500AC
	public static bool IsBigDeco(BlockValue blockValue, Block block)
	{
		return block.SmallDecorationRadius > 0 || block.BigDecorationRadius > 0 || block.isOversized;
	}

	// Token: 0x06005EC3 RID: 24259 RVA: 0x00251EC8 File Offset: 0x002500C8
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

	// Token: 0x06005EC4 RID: 24260 RVA: 0x00251F22 File Offset: 0x00250122
	public static bool CanPlaceDeco(Chunk cX0Z0, Chunk cX1Z0, Chunk cX0Z1, Chunk cX1Z1, Vector3i blockPos, BlockValue blockValue, DecoUtils.DecoAllowedTest additionalTest = null)
	{
		return DecoUtils.CanPlaceDeco(cX0Z0, blockPos, blockValue, additionalTest) && DecoUtils.CanPlaceDeco(cX1Z0, blockPos, blockValue, additionalTest) && DecoUtils.CanPlaceDeco(cX0Z1, blockPos, blockValue, additionalTest) && DecoUtils.CanPlaceDeco(cX1Z1, blockPos, blockValue, additionalTest);
	}

	// Token: 0x06005EC5 RID: 24261 RVA: 0x00251F5C File Offset: 0x0025015C
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

	// Token: 0x06005EC6 RID: 24262 RVA: 0x0025209C File Offset: 0x0025029C
	public static bool HasDecoAllowed(BlockValue blockValue)
	{
		if (blockValue.isair)
		{
			return false;
		}
		Block block = blockValue.Block;
		return (!block.isMultiBlock || !blockValue.ischild) && (block.SmallDecorationRadius > 0 || block.BigDecorationRadius > 0 || block.isOversized);
	}

	// Token: 0x06005EC7 RID: 24263 RVA: 0x002520E9 File Offset: 0x002502E9
	public static void ApplyDecoAllowed(Chunk cX0Z0, Chunk cX1Z0, Chunk cX0Z1, Chunk cX1Z1, Vector3i blockPos, BlockValue blockValue)
	{
		DecoUtils.ApplyDecoAllowed(cX0Z0, blockPos, blockValue);
		DecoUtils.ApplyDecoAllowed(cX1Z0, blockPos, blockValue);
		DecoUtils.ApplyDecoAllowed(cX0Z1, blockPos, blockValue);
		DecoUtils.ApplyDecoAllowed(cX1Z1, blockPos, blockValue);
	}

	// Token: 0x06005EC8 RID: 24264 RVA: 0x00252114 File Offset: 0x00250314
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

	// Token: 0x06005EC9 RID: 24265 RVA: 0x00252174 File Offset: 0x00250374
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

	// Token: 0x06005ECA RID: 24266 RVA: 0x00252254 File Offset: 0x00250454
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

	// Token: 0x06005ECB RID: 24267 RVA: 0x00252348 File Offset: 0x00250548
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

	// Token: 0x06005ECC RID: 24268 RVA: 0x00252400 File Offset: 0x00250600
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

	// Token: 0x06005ECD RID: 24269 RVA: 0x0025249C File Offset: 0x0025069C
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

	// Token: 0x02000C30 RID: 3120
	// (Invoke) Token: 0x06005ECF RID: 24271
	public delegate bool DecoAllowedTest(EnumDecoAllowed decoAllowed);
}
