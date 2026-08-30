using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200011D RID: 285
[Preserve]
public class BlockForge : BlockWorkstation
{
	// Token: 0x0600077E RID: 1918 RVA: 0x0003569C File Offset: 0x0003389C
	public BlockForge()
	{
		this.CraftingParticleLightIntensity = 1.6f;
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x000356AF File Offset: 0x000338AF
	public override void OnBlockEntityTransformAfterActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
		base.OnBlockEntityTransformAfterActivated(_world, _blockPos, _blockValue, _ebcd);
		this.MaterialUpdate(_world, _blockPos, _blockValue);
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x000356C5 File Offset: 0x000338C5
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

	// Token: 0x06000781 RID: 1921 RVA: 0x000356E4 File Offset: 0x000338E4
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

	// Token: 0x06000782 RID: 1922 RVA: 0x00035748 File Offset: 0x00033948
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
