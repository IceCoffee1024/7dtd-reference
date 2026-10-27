using System;

// Token: 0x02000B5F RID: 2911
public interface IBlockAccess
{
	// Token: 0x06005792 RID: 22418
	BlockValue GetBlock(int x, int y, int z);

	// Token: 0x06005793 RID: 22419 RVA: 0x001B3814 File Offset: 0x001B1A14
	BlockValue GetBlock(Vector3i pos)
	{
		return IBlockAccess.DefaultGetBlock(this, pos);
	}

	// Token: 0x06005794 RID: 22420 RVA: 0x001B381D File Offset: 0x001B1A1D
	BlockValue GetBlock(BlockValueRef bvRef)
	{
		return IBlockAccess.DefaultGetBlock(this, bvRef);
	}

	// Token: 0x06005795 RID: 22421
	PropValue GetProp(int chunkX, int chunkZ, int propId);

	// Token: 0x06005796 RID: 22422
	PropValue GetProp(long chunkKey, int propId);

	// Token: 0x06005797 RID: 22423 RVA: 0x001B39A9 File Offset: 0x001B1BA9
	PropValue GetProp(Vector2i chunkPos, int propId)
	{
		return IBlockAccess.DefaultGetProp(this, chunkPos, propId);
	}

	// Token: 0x06005798 RID: 22424 RVA: 0x001B39B3 File Offset: 0x001B1BB3
	PropValue GetProp(PropRef propRef)
	{
		return IBlockAccess.DefaultGetProp(this, propRef);
	}

	// Token: 0x06005799 RID: 22425 RVA: 0x0021CA9B File Offset: 0x0021AC9B
	[PublicizedFrom(EAccessModifier.Protected)]
	public static BlockValue DefaultGetBlock(IBlockAccess iba, Vector3i pos)
	{
		return iba.GetBlock(pos.x, pos.y, pos.z);
	}

	// Token: 0x0600579A RID: 22426 RVA: 0x0021CAB8 File Offset: 0x0021ACB8
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

	// Token: 0x0600579B RID: 22427 RVA: 0x0021CB12 File Offset: 0x0021AD12
	[PublicizedFrom(EAccessModifier.Protected)]
	public static PropValue DefaultGetProp(IBlockAccess iba, Vector2i chunkPos, int propId)
	{
		return iba.GetProp(chunkPos.x, chunkPos.y, propId);
	}

	// Token: 0x0600579C RID: 22428 RVA: 0x0021CB27 File Offset: 0x0021AD27
	[PublicizedFrom(EAccessModifier.Protected)]
	public static PropValue DefaultGetProp(IBlockAccess iba, PropRef propRef)
	{
		return iba.GetProp(propRef.ChunkPos, propRef.PropId);
	}
}
