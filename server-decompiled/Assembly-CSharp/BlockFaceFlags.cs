using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020013DC RID: 5084
public static class BlockFaceFlags
{
	// Token: 0x06009FA4 RID: 40868 RVA: 0x003C46C8 File Offset: 0x003C28C8
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

	// Token: 0x06009FA5 RID: 40869 RVA: 0x003C4740 File Offset: 0x003C2940
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

	// Token: 0x06009FA6 RID: 40870 RVA: 0x003C47A2 File Offset: 0x003C29A2
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

	// Token: 0x06009FA7 RID: 40871 RVA: 0x003C47D5 File Offset: 0x003C29D5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static BlockFaceFlag FromBlockFace(BlockFace face)
	{
		if (face == BlockFace.None)
		{
			return BlockFaceFlag.None;
		}
		return (BlockFaceFlag)(1 << (int)face);
	}

	// Token: 0x06009FA8 RID: 40872 RVA: 0x003C47E7 File Offset: 0x003C29E7
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

	// Token: 0x06009FA9 RID: 40873 RVA: 0x003C481C File Offset: 0x003C2A1C
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

	// Token: 0x06009FAA RID: 40874 RVA: 0x003C4864 File Offset: 0x003C2A64
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

	// Token: 0x06009FAB RID: 40875 RVA: 0x003C48BC File Offset: 0x003C2ABC
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

	// Token: 0x06009FAC RID: 40876 RVA: 0x003C4912 File Offset: 0x003C2B12
	public static BlockFaceFlag OppositeFaceFlag(BlockFace face)
	{
		return BlockFaceFlags.FromBlockFace(BlockFaceFlags.OppositeFace(face));
	}

	// Token: 0x06009FAD RID: 40877 RVA: 0x003C491F File Offset: 0x003C2B1F
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

	// Token: 0x06009FAE RID: 40878 RVA: 0x003C4950 File Offset: 0x003C2B50
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

	// Token: 0x06009FAF RID: 40879 RVA: 0x003C49DC File Offset: 0x003C2BDC
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

	// Token: 0x040078FD RID: 30973
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly int[] faceRotShiftValues = new int[144];

	// Token: 0x040078FE RID: 30974
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

	// Token: 0x040078FF RID: 30975
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly char[] cubeSideFaceChars = new char[] { 'T', 'B', 'N', 'W', 'S', 'E' };
}
