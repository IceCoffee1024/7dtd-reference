using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200011E RID: 286
[Preserve]
public class BlockForge : BlockWorkstation
{
	// Token: 0x06000788 RID: 1928 RVA: 0x0003598C File Offset: 0x00033B8C
	public BlockForge()
	{
		this.CraftingParticleLightIntensity = 1.6f;
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x0003599F File Offset: 0x00033B9F
	public override void OnBlockEntityTransformAfterActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
		base.OnBlockEntityTransformAfterActivated(_world, _blockPos, _blockValue, _ebcd);
		this.MaterialUpdate(_world, _blockPos, _blockValue);
	}

	// Token: 0x0600078A RID: 1930 RVA: 0x000359B5 File Offset: 0x00033BB5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void checkParticles(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.checkParticles(_world, _blockPos, _blockValue);
		if (_blockValue.ischild)
		{
			return;
		}
		this.MaterialUpdate(_world, _blockPos, _blockValue);
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x000359D4 File Offset: 0x00033BD4
	public override string GetActivationText(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		if (XUiM_Recipes.DisableSmelter)
		{
			TileEntityWorkstation tileEntityWorkstation = (TileEntityWorkstation)_world.GetTileEntity(_blockPos);
			if (tileEntityWorkstation.InputSlotCount > 0 && !tileEntityWorkstation.InputIsEmpty())
			{
				return Localization.Get("useForgeMaterials", false, null);
			}
		}
		return _blockValue.Block.GetLocalizedBlockName() + "\n" + Localization.Get("useWorkstation", false, null);
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x00035A38 File Offset: 0x00033C38
	[PublicizedFrom(EAccessModifier.Private)]
	public void MaterialUpdate(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		Chunk chunk = (Chunk)_world.GetChunkFromWorldPos(_blockPos);
		if (chunk != null)
		{
			BlockEntityData blockEntity = chunk.GetBlockEntity(_blockPos);
			if (blockEntity != null && blockEntity.bHasTransform)
			{
				Renderer[] componentsInChildren = blockEntity.transform.GetComponentsInChildren<MeshRenderer>(true);
				Renderer[] array = componentsInChildren;
				if (array.Length != 0)
				{
					Material material = array[0].material;
					if (material)
					{
						float num = (float)((_blockValue.meta == 0) ? 0 : 20);
						material.SetFloat("_EmissionMultiply", num);
						for (int i = 1; i < array.Length; i++)
						{
							array[i].material = material;
						}
					}
				}
			}
		}
	}
}
