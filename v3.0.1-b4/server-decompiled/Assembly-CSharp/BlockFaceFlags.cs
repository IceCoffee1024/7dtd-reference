using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200138F RID: 5007
public static class BlockFaceFlags
{
	// Token: 0x06009DA4 RID: 40356 RVA: 0x003B83E4 File Offset: 0x003B65E4
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

	// Token: 0x06009DA5 RID: 40357 RVA: 0x003B845C File Offset: 0x003B665C
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

	// Token: 0x06009DA6 RID: 40358 RVA: 0x003B84BE File Offset: 0x003B66BE
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

	// Token: 0x06009DA7 RID: 40359 RVA: 0x003B84F1 File Offset: 0x003B66F1
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static BlockFaceFlag FromBlockFace(BlockFace face)
	{
		if (face == BlockFace.None)
		{
			return BlockFaceFlag.None;
		}
		return (BlockFaceFlag)(1 << (int)face);
	}

	// Token: 0x06009DA8 RID: 40360 RVA: 0x003B8503 File Offset: 0x003B6703
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

	// Token: 0x06009DA9 RID: 40361 RVA: 0x003B8538 File Offset: 0x003B6738
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

	// Token: 0x06009DAA RID: 40362 RVA: 0x003B8580 File Offset: 0x003B6780
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

	// Token: 0x06009DAB RID: 40363 RVA: 0x003B85D8 File Offset: 0x003B67D8
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

	// Token: 0x06009DAC RID: 40364 RVA: 0x003B862E File Offset: 0x003B682E
	public static BlockFaceFlag OppositeFaceFlag(BlockFace face)
	{
		return BlockFaceFlags.FromBlockFace(BlockFaceFlags.OppositeFace(face));
	}

	// Token: 0x06009DAD RID: 40365 RVA: 0x003B863B File Offset: 0x003B683B
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

	// Token: 0x06009DAE RID: 40366 RVA: 0x003B866C File Offset: 0x003B686C
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

	// Token: 0x06009DAF RID: 40367 RVA: 0x003B86F8 File Offset: 0x003B68F8
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

	// Token: 0x04007768 RID: 30568
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly int[] faceRotShiftValues = new int[144];

	// Token: 0x04007769 RID: 30569
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

	// Token: 0x0400776A RID: 30570
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly char[] cubeSideFaceChars = new char[] { 'T', 'B', 'N', 'W', 'S', 'E' };
}
