using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005B2 RID: 1458
[Preserve]
public class ItemActionMakeFertile : ItemActionMelee
{
	// Token: 0x06002F8E RID: 12174 RVA: 0x00130398 File Offset: 0x0012E598
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		if (!_props.Values.ContainsKey("Fertileblock"))
		{
			throw new Exception("Missing attribute 'fertileblock' in use_action 'MakeFertile'");
		}
		string text = _props.Values["Fertileblock"];
		this.fertileBlock = ItemClass.GetItem(text, false).ToBlockValue(false);
		if (this.fertileBlock.Equals(BlockValue.Air))
		{
			throw new Exception("Unknown block name '" + text + "' in use_action 'MakeFertile'!");
		}
		if (!_props.Values.ContainsKey("Adjacentblock"))
		{
			throw new Exception("Missing attribute 'adjacentblock' in use_action 'MakeFertile'");
		}
		text = _props.Values["Adjacentblock"];
		this.adjacentBlock = ItemClass.GetItem(text, false).ToBlockValue(false);
		if (this.adjacentBlock.Equals(BlockValue.Air))
		{
			throw new Exception("Unknown block name '" + text + "' in use_action 'MakeFertile'!");
		}
	}

	// Token: 0x06002F8F RID: 12175 RVA: 0x00130480 File Offset: 0x0012E680
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void hitTheTarget(ItemActionMelee.InventoryDataMelee _actionData, WorldRayHitInfo hitInfo, float damageScale)
	{
		ItemInventoryData invData = _actionData.invData;
		if (!hitInfo.bHitValid || !GameUtils.IsBlockOrTerrain(hitInfo.tag))
		{
			return;
		}
		Vector3i blockPos = hitInfo.hit.blockPos;
		if (invData.world.GetBlock(blockPos).Block.blockMaterial.FertileLevel < 2)
		{
			base.hitTheTarget(_actionData, hitInfo, damageScale);
			return;
		}
		Vector3 position = _actionData.invData.holdingEntity.GetPosition();
		float num = position.x - hitInfo.hit.pos.x;
		float num2 = position.z - hitInfo.hit.pos.z;
		Vector3i vector3i = Vector3i.right;
		if (Mathf.Abs(num) > Mathf.Abs(num2))
		{
			vector3i = Vector3i.forward;
		}
		List<BlockChangeInfo> list = new List<BlockChangeInfo>();
		BlockValue block = invData.world.GetBlock(blockPos + vector3i);
		if (block.Block.blockMaterial.FertileLevel >= 2 && block.Block.shape.IsTerrain())
		{
			list.Add(new BlockChangeInfo(blockPos + vector3i, this.adjacentBlock, MarchingCubes.DensityTerrain / 3));
		}
		BlockValue block2 = invData.world.GetBlock(blockPos - vector3i);
		if (block2.Block.blockMaterial.FertileLevel >= 2 && block2.Block.shape.IsTerrain())
		{
			list.Add(new BlockChangeInfo(blockPos - vector3i, this.adjacentBlock, MarchingCubes.DensityTerrain / 3));
		}
		list.Add(new BlockChangeInfo(blockPos, this.fertileBlock, MarchingCubes.DensityTerrain));
		if (_actionData.invData.holdingEntity is EntityPlayerLocal)
		{
			(_actionData.invData.holdingEntity as EntityPlayerLocal).Progression.AddLevelExp((int)this.fertileBlock.Block.blockMaterial.Experience, "_xpOther", Progression.XPTypes.Other, true, true, -1, null);
		}
		invData.world.SetBlocksRPC(list);
		if (this.soundEnd != null)
		{
			_actionData.invData.holdingEntity.PlayOneShot(this.soundEnd, false, false, false, null, 1f);
		}
	}

	// Token: 0x06002F90 RID: 12176 RVA: 0x001306A9 File Offset: 0x0012E8A9
	public override RenderCubeType GetFocusType(ItemActionData _actionData)
	{
		return RenderCubeType.FullBlockBothSides;
	}

	// Token: 0x040024C3 RID: 9411
	[PublicizedFrom(EAccessModifier.Protected)]
	public BlockValue fertileBlock;

	// Token: 0x040024C4 RID: 9412
	[PublicizedFrom(EAccessModifier.Protected)]
	public BlockValue adjacentBlock;
}
