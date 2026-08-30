using System;

// Token: 0x02000B30 RID: 2864
public interface IBlockAccess
{
	// Token: 0x060056CB RID: 22219
	BlockValue GetBlock(int x, int y, int z);

	// Token: 0x060056CC RID: 22220 RVA: 0x001AFD64 File Offset: 0x001ADF64
	BlockValue GetBlock(Vector3i pos)
	{
		return IBlockAccess.DefaultGetBlock(this, pos);
	}

	// Token: 0x060056CD RID: 22221 RVA: 0x001AFD6D File Offset: 0x001ADF6D
	BlockValue GetBlock(BlockValueRef bvRef)
	{
		return IBlockAccess.DefaultGetBlock(this, bvRef);
	}

	// Token: 0x060056CE RID: 22222
	PropValue GetProp(int chunkX, int chunkZ, int propId);

	// Token: 0x060056CF RID: 22223
	PropValue GetProp(long chunkKey, int propId);

	// Token: 0x060056D0 RID: 22224 RVA: 0x001AFEF9 File Offset: 0x001AE0F9
	PropValue GetProp(Vector2i chunkPos, int propId)
	{
		return IBlockAccess.DefaultGetProp(this, chunkPos, propId);
	}

	// Token: 0x060056D1 RID: 22225 RVA: 0x001AFF03 File Offset: 0x001AE103
	PropValue GetProp(PropRef propRef)
	{
		return IBlockAccess.DefaultGetProp(this, propRef);
	}

	// Token: 0x060056D2 RID: 22226 RVA: 0x00215597 File Offset: 0x00213797
	[PublicizedFrom(EAccessModifier.Protected)]
	public static BlockValue DefaultGetBlock(IBlockAccess iba, Vector3i pos)
	{
		return iba.GetBlock(pos.x, pos.y, pos.z);
	}

	// Token: 0x060056D3 RID: 22227 RVA: 0x002155B4 File Offset: 0x002137B4
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

	// Token: 0x060056D4 RID: 22228 RVA: 0x0021560E File Offset: 0x0021380E
	[PublicizedFrom(EAccessModifier.Protected)]
	public static PropValue DefaultGetProp(IBlockAccess iba, Vector2i chunkPos, int propId)
	{
		return iba.GetProp(chunkPos.x, chunkPos.y, propId);
	}

	// Token: 0x060056D5 RID: 22229 RVA: 0x00215623 File Offset: 0x00213823
	[PublicizedFrom(EAccessModifier.Protected)]
	public static PropValue DefaultGetProp(IBlockAccess iba, PropRef propRef)
	{
		return iba.GetProp(propRef.ChunkPos, propRef.PropId);
	}
}
