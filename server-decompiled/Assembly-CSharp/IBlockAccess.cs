using System;

// Token: 0x02000B54 RID: 2900
public interface IBlockAccess
{
	// Token: 0x0600576D RID: 22381
	BlockValue GetBlock(int x, int y, int z);

	// Token: 0x0600576E RID: 22382 RVA: 0x001B2790 File Offset: 0x001B0990
	BlockValue GetBlock(Vector3i pos)
	{
		return IBlockAccess.DefaultGetBlock(this, pos);
	}

	// Token: 0x0600576F RID: 22383 RVA: 0x001B2799 File Offset: 0x001B0999
	BlockValue GetBlock(BlockValueRef bvRef)
	{
		return IBlockAccess.DefaultGetBlock(this, bvRef);
	}

	// Token: 0x06005770 RID: 22384
	PropValue GetProp(int chunkX, int chunkZ, int propId);

	// Token: 0x06005771 RID: 22385
	PropValue GetProp(long chunkKey, int propId);

	// Token: 0x06005772 RID: 22386 RVA: 0x001B2925 File Offset: 0x001B0B25
	PropValue GetProp(Vector2i chunkPos, int propId)
	{
		return IBlockAccess.DefaultGetProp(this, chunkPos, propId);
	}

	// Token: 0x06005773 RID: 22387 RVA: 0x001B292F File Offset: 0x001B0B2F
	PropValue GetProp(PropRef propRef)
	{
		return IBlockAccess.DefaultGetProp(this, propRef);
	}

	// Token: 0x06005774 RID: 22388 RVA: 0x0021B53F File Offset: 0x0021973F
	[PublicizedFrom(EAccessModifier.Protected)]
	public static BlockValue DefaultGetBlock(IBlockAccess iba, Vector3i pos)
	{
		return iba.GetBlock(pos.x, pos.y, pos.z);
	}

	// Token: 0x06005775 RID: 22389 RVA: 0x0021B55C File Offset: 0x0021975C
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

	// Token: 0x06005776 RID: 22390 RVA: 0x0021B5B6 File Offset: 0x002197B6
	[PublicizedFrom(EAccessModifier.Protected)]
	public static PropValue DefaultGetProp(IBlockAccess iba, Vector2i chunkPos, int propId)
	{
		return iba.GetProp(chunkPos.x, chunkPos.y, propId);
	}

	// Token: 0x06005777 RID: 22391 RVA: 0x0021B5CB File Offset: 0x002197CB
	[PublicizedFrom(EAccessModifier.Protected)]
	public static PropValue DefaultGetProp(IBlockAccess iba, PropRef propRef)
	{
		return iba.GetProp(propRef.ChunkPos, propRef.PropId);
	}
}
