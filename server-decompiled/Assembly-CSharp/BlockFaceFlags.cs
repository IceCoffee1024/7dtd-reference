using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020013CA RID: 5066
public static class BlockFaceFlags
{
	// Token: 0x06009F4C RID: 40780 RVA: 0x003C22C4 File Offset: 0x003C04C4
	[PublicizedFrom(EAccessModifier.Private)]
	static BlockFaceFlags()
	{
		for (int i = 0; i < 24; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				BlockFaceFlags.faceRotShiftValues[i * 6 + j] = (int)(((BlockFace)j).RotateFace(i) - (BlockFace)j);
			}
		}
	}

	// Token: 0x06009F4D RID: 40781 RVA: 0x003C233C File Offset: 0x003C053C
	public static BlockFaceFlag RotateFlags(BlockFaceFlag mask, byte blockRotation)
	{
		if (mask == BlockFaceFlag.None || mask == BlockFaceFlag.All || blockRotation > 23)
		{
			return mask;
		}
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			int num2 = (int)(mask & (BlockFaceFlag)(1 << i));
			if (num2 != 0)
			{
				int num3 = BlockFaceFlags.faceRotShiftValues[(int)(blockRotation * 6) + i];
				if (num3 > 0)
				{
					num2 <<= num3;
				}
				else
				{
					num2 >>= -num3;
				}
				num |= num2;
			}
		}
		return (BlockFaceFlag)num;
	}

	// Token: 0x06009F4E RID: 40782 RVA: 0x003C239E File Offset: 0x003C059E
	public static BlockFace ToBlockFace(BlockFaceFlag flags)
	{
		if ((flags & BlockFaceFlag.Top) != BlockFaceFlag.None)
		{
			return BlockFace.Top;
		}
		if ((flags & BlockFaceFlag.Bottom) != BlockFaceFlag.None)
		{
			return BlockFace.Bottom;
		}
		if ((flags & BlockFaceFlag.North) != BlockFaceFlag.None)
		{
			return BlockFace.North;
		}
		if ((flags & BlockFaceFlag.South) != BlockFaceFlag.None)
		{
			return BlockFace.South;
		}
		if ((flags & BlockFaceFlag.East) != BlockFaceFlag.None)
		{
			return BlockFace.East;
		}
		if ((flags & BlockFaceFlag.West) != BlockFaceFlag.None)
		{
			return BlockFace.West;
		}
		return BlockFace.None;
	}

	// Token: 0x06009F4F RID: 40783 RVA: 0x003C23D1 File Offset: 0x003C05D1
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static BlockFaceFlag FromBlockFace(BlockFace face)
	{
		if (face == BlockFace.None)
		{
			return BlockFaceFlag.None;
		}
		return (BlockFaceFlag)(1 << (int)face);
	}

	// Token: 0x06009F50 RID: 40784 RVA: 0x003C23E3 File Offset: 0x003C05E3
	public static BlockFace OppositeFace(BlockFace face)
	{
		switch (face)
		{
		case BlockFace.Top:
			return BlockFace.Bottom;
		case BlockFace.Bottom:
			return BlockFace.Top;
		case BlockFace.North:
			return BlockFace.South;
		case BlockFace.West:
			return BlockFace.East;
		case BlockFace.South:
			return BlockFace.North;
		case BlockFace.East:
			return BlockFace.West;
		default:
			return BlockFace.None;
		}
	}

	// Token: 0x06009F51 RID: 40785 RVA: 0x003C2418 File Offset: 0x003C0618
	public static BlockFace NearestFaceForDirection(Vector3 direction, float dotTolerance = 0.8f)
	{
		float num = -2f;
		BlockFace blockFace = BlockFace.None;
		for (BlockFace blockFace2 = BlockFace.Top; blockFace2 <= BlockFace.East; blockFace2 += 1)
		{
			float num2 = Vector3.Dot(BlockFaceFlags.OffsetForFace(blockFace2), direction);
			if (num2 > num)
			{
				num = num2;
				blockFace = blockFace2;
			}
		}
		if (num < dotTolerance)
		{
			return BlockFace.None;
		}
		return blockFace;
	}

	// Token: 0x06009F52 RID: 40786 RVA: 0x003C2460 File Offset: 0x003C0660
	public static Vector3 OffsetForFace(BlockFace face)
	{
		switch (face)
		{
		case BlockFace.Top:
			return Vector3.up;
		case BlockFace.Bottom:
			return Vector3.down;
		case BlockFace.North:
			return Vector3.forward;
		case BlockFace.West:
			return Vector3.left;
		case BlockFace.South:
			return Vector3.back;
		case BlockFace.East:
			return Vector3.right;
		default:
			return Vector3.zero;
		}
	}

	// Token: 0x06009F53 RID: 40787 RVA: 0x003C24B8 File Offset: 0x003C06B8
	public static Vector3i OffsetIForFace(BlockFace face)
	{
		switch (face)
		{
		case BlockFace.Top:
			return Vector3i.up;
		case BlockFace.Bottom:
			return Vector3i.down;
		case BlockFace.North:
			return Vector3i.forward;
		case BlockFace.West:
			return Vector3i.left;
		case BlockFace.South:
			return Vector3i.back;
		case BlockFace.East:
			return Vector3i.right;
		default:
			return Vector3i.zero;
		}
	}

	// Token: 0x06009F54 RID: 40788 RVA: 0x003C250E File Offset: 0x003C070E
	public static BlockFaceFlag OppositeFaceFlag(BlockFace face)
	{
		return BlockFaceFlags.FromBlockFace(BlockFaceFlags.OppositeFace(face));
	}

	// Token: 0x06009F55 RID: 40789 RVA: 0x003C251B File Offset: 0x003C071B
	public static float YawForDirection(BlockFace face)
	{
		switch (face)
		{
		case BlockFace.West:
			return 270f;
		case BlockFace.South:
			return 180f;
		case BlockFace.East:
			return 90f;
		default:
			return 0f;
		}
	}

	// Token: 0x06009F56 RID: 40790 RVA: 0x003C254C File Offset: 0x003C074C
	public static BlockFaceFlag FrontSidesFromPosition(Vector3i blockPos, Vector3 entityPos)
	{
		BlockFaceFlag blockFaceFlag = BlockFaceFlag.None;
		if (entityPos.x < (float)blockPos.x)
		{
			blockFaceFlag |= BlockFaceFlag.West;
		}
		if (entityPos.x >= (float)(blockPos.x + 1))
		{
			blockFaceFlag |= BlockFaceFlag.East;
		}
		if (entityPos.y < (float)blockPos.y)
		{
			blockFaceFlag |= BlockFaceFlag.Bottom;
		}
		if (entityPos.y >= (float)(blockPos.y + 1))
		{
			blockFaceFlag |= BlockFaceFlag.Top;
		}
		if (entityPos.z < (float)blockPos.z)
		{
			blockFaceFlag |= BlockFaceFlag.South;
		}
		if (entityPos.z >= (float)(blockPos.z + 1))
		{
			blockFaceFlag |= BlockFaceFlag.North;
		}
		return blockFaceFlag;
	}

	// Token: 0x06009F57 RID: 40791 RVA: 0x003C25D8 File Offset: 0x003C07D8
	public static string SerializeFaceFlags(BlockFaceFlag coverFaceMask)
	{
		string text = string.Empty;
		bool flag = true;
		for (int i = 0; i < BlockFaceFlags.cubeSideFaceFlags.Length; i++)
		{
			if ((coverFaceMask & BlockFaceFlags.cubeSideFaceFlags[i]) != BlockFaceFlag.None)
			{
				string text2 = (flag ? "" : ",");
				text += string.Format("{0}{1}", text2, BlockFaceFlags.cubeSideFaceChars[i]);
				flag = false;
			}
		}
		return text;
	}

	// Token: 0x040078BC RID: 30908
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly int[] faceRotShiftValues = new int[144];

	// Token: 0x040078BD RID: 30909
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly BlockFaceFlag[] cubeSideFaceFlags = new BlockFaceFlag[]
	{
		BlockFaceFlag.Top,
		BlockFaceFlag.Bottom,
		BlockFaceFlag.North,
		BlockFaceFlag.West,
		BlockFaceFlag.South,
		BlockFaceFlag.East
	};

	// Token: 0x040078BE RID: 30910
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly char[] cubeSideFaceChars = new char[] { 'T', 'B', 'N', 'W', 'S', 'E' };
}
