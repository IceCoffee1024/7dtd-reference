using System;

// Token: 0x02000B42 RID: 2882
public interface IBlockAccess
{
	// Token: 0x06005753 RID: 22355
	BlockValue GetBlock(int x, int y, int z);

	// Token: 0x06005754 RID: 22356 RVA: 0x001B233C File Offset: 0x001B053C
	BlockValue GetBlock(Vector3i pos)
	{
		return IBlockAccess.DefaultGetBlock(this, pos);
	}

	// Token: 0x06005755 RID: 22357 RVA: 0x001B2345 File Offset: 0x001B0545
	BlockValue GetBlock(BlockValueRef bvRef)
	{
		return IBlockAccess.DefaultGetBlock(this, bvRef);
	}

	// Token: 0x06005756 RID: 22358
	PropValue GetProp(int chunkX, int chunkZ, int propId);

	// Token: 0x06005757 RID: 22359
	PropValue GetProp(long chunkKey, int propId);

	// Token: 0x06005758 RID: 22360 RVA: 0x001B24D1 File Offset: 0x001B06D1
	PropValue GetProp(Vector2i chunkPos, int propId)
	{
		return IBlockAccess.DefaultGetProp(this, chunkPos, propId);
	}

	// Token: 0x06005759 RID: 22361 RVA: 0x001B24DB File Offset: 0x001B06DB
	PropValue GetProp(PropRef propRef)
	{
		return IBlockAccess.DefaultGetProp(this, propRef);
	}

	// Token: 0x0600575A RID: 22362 RVA: 0x002182B3 File Offset: 0x002164B3
	[PublicizedFrom(EAccessModifier.Protected)]
	public static BlockValue DefaultGetBlock(IBlockAccess iba, Vector3i pos)
	{
		return iba.GetBlock(pos.x, pos.y, pos.z);
	}

	// Token: 0x0600575B RID: 22363 RVA: 0x002182D0 File Offset: 0x002164D0
	[PublicizedFrom(EAccessModifier.Protected)]
	public static BlockValue DefaultGetBlock(IBlockAccess iba, BlockValueRef bvRef)
	{
		BlockValue blockValue;
		switch (bvRef.Type)
		{
		case BlockValueRefType.None:
			blockValue = BlockValue.Air;
			break;
		case BlockValueRefType.Block:
			blockValue = iba.GetBlock(bvRef.BlockPosition);
			break;
		case BlockValueRefType.Prop:
			blockValue = iba.GetProp(bvRef.PropReference).blockValue;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		return blockValue;
	}

	// Token: 0x0600575C RID: 22364 RVA: 0x0021832A File Offset: 0x0021652A
	[PublicizedFrom(EAccessModifier.Protected)]
	public static PropValue DefaultGetProp(IBlockAccess iba, Vector2i chunkPos, int propId)
	{
		return iba.GetProp(chunkPos.x, chunkPos.y, propId);
	}

	// Token: 0x0600575D RID: 22365 RVA: 0x0021833F File Offset: 0x0021653F
	[PublicizedFrom(EAccessModifier.Protected)]
	public static PropValue DefaultGetProp(IBlockAccess iba, PropRef propRef)
	{
		return iba.GetProp(propRef.ChunkPos, propRef.PropId);
	}
}
