using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000C0B RID: 3083
public static class DecoUtils
{
	// Token: 0x06005E1F RID: 24095 RVA: 0x0024BD00 File Offset: 0x00249F00
	public static bool IsBigDeco(BlockValue blockValue, Block block)
	{
		return block.SmallDecorationRadius > 0 || block.BigDecorationRadius > 0 || block.isOversized;
	}

	// Token: 0x06005E20 RID: 24096 RVA: 0x0024BD1C File Offset: 0x00249F1C
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

	// Token: 0x06005E21 RID: 24097 RVA: 0x0024BD76 File Offset: 0x00249F76
	public static bool CanPlaceDeco(Chunk cX0Z0, Chunk cX1Z0, Chunk cX0Z1, Chunk cX1Z1, Vector3i blockPos, BlockValue blockValue, DecoUtils.DecoAllowedTest additionalTest = null)
	{
		return DecoUtils.CanPlaceDeco(cX0Z0, blockPos, blockValue, additionalTest) && DecoUtils.CanPlaceDeco(cX1Z0, blockPos, blockValue, additionalTest) && DecoUtils.CanPlaceDeco(cX0Z1, blockPos, blockValue, additionalTest) && DecoUtils.CanPlaceDeco(cX1Z1, blockPos, blockValue, additionalTest);
	}

	// Token: 0x06005E22 RID: 24098 RVA: 0x0024BDB0 File Offset: 0x00249FB0
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

	// Token: 0x06005E23 RID: 24099 RVA: 0x0024BEF0 File Offset: 0x0024A0F0
	public static bool HasDecoAllowed(BlockValue blockValue)
	{
		if (blockValue.isair)
		{
			return false;
		}
		Block block = blockValue.Block;
		return (!block.isMultiBlock || !blockValue.ischild) && (block.SmallDecorationRadius > 0 || block.BigDecorationRadius > 0 || block.isOversized);
	}

	// Token: 0x06005E24 RID: 24100 RVA: 0x0024BF3D File Offset: 0x0024A13D
	public static void ApplyDecoAllowed(Chunk cX0Z0, Chunk cX1Z0, Chunk cX0Z1, Chunk cX1Z1, Vector3i blockPos, BlockValue blockValue)
	{
		DecoUtils.ApplyDecoAllowed(cX0Z0, blockPos, blockValue);
		DecoUtils.ApplyDecoAllowed(cX1Z0, blockPos, blockValue);
		DecoUtils.ApplyDecoAllowed(cX0Z1, blockPos, blockValue);
		DecoUtils.ApplyDecoAllowed(cX1Z1, blockPos, blockValue);
	}

	// Token: 0x06005E25 RID: 24101 RVA: 0x0024BF68 File Offset: 0x0024A168
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

	// Token: 0x06005E26 RID: 24102 RVA: 0x0024BFC8 File Offset: 0x0024A1C8
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

	// Token: 0x06005E27 RID: 24103 RVA: 0x0024C0A8 File Offset: 0x0024A2A8
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

	// Token: 0x06005E28 RID: 24104 RVA: 0x0024C19C File Offset: 0x0024A39C
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

	// Token: 0x06005E29 RID: 24105 RVA: 0x0024C254 File Offset: 0x0024A454
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

	// Token: 0x06005E2A RID: 24106 RVA: 0x0024C2F0 File Offset: 0x0024A4F0
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

	// Token: 0x02000C0C RID: 3084
	// (Invoke) Token: 0x06005E2C RID: 24108
	public delegate bool DecoAllowedTest(EnumDecoAllowed decoAllowed);
}
