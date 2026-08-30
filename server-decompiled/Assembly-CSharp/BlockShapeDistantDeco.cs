using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020001A5 RID: 421
[Preserve]
public class BlockShapeDistantDeco : BlockShapeModelEntity
{
	// Token: 0x06000CBE RID: 3262 RVA: 0x000502F0 File Offset: 0x0004E4F0
	public override void Init(Block _block)
	{
		base.Init(_block);
	}

	// Token: 0x06000CBF RID: 3263 RVA: 0x000502F9 File Offset: 0x0004E4F9
	public override void OnBlockValueChanged(WorldBase _world, Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
		if (!DecoManager.Instance.IsEnabled)
		{
			base.OnBlockValueChanged(_world, _blockPos, _oldBlockValue, _newBlockValue);
			return;
		}
	}

	// Token: 0x06000CC0 RID: 3264 RVA: 0x00050313 File Offset: 0x0004E513
	public override void OnBlockAdded(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		if (!DecoManager.Instance.IsEnabled)
		{
			base.OnBlockAdded(_world, _chunk, _blockPos, _blockValue);
			return;
		}
		if (!_blockValue.Block.IsDistantDecoration)
		{
			return;
		}
		DecoManager.Instance.AddDecorationAt((World)_world, _blockValue, _blockPos, true);
	}

	// Token: 0x06000CC1 RID: 3265 RVA: 0x00050350 File Offset: 0x0004E550
	public override void OnBlockRemoved(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		if (!DecoManager.Instance.IsEnabled)
		{
			base.OnBlockRemoved(_world, _chunk, _blockPos, _blockValue);
		}
		DecoManager.Instance.RemoveDecorationAt(_blockPos);
	}

	// Token: 0x06000CC2 RID: 3266 RVA: 0x00050378 File Offset: 0x0004E578
	public override void OnBlockLoaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		if (!DecoManager.Instance.IsEnabled)
		{
			base.OnBlockLoaded(_world, _blockPos, _blockValue);
			return;
		}
		if (_world.IsRemote())
		{
			if (!_blockValue.Block.IsDistantDecoration)
			{
				return;
			}
			DecoManager.Instance.AddDecorationAt((World)_world, _blockValue, _blockPos, true);
		}
	}

	// Token: 0x06000CC3 RID: 3267 RVA: 0x000503C8 File Offset: 0x0004E5C8
	public override void OnBlockEntityTransformAfterActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
		if (!DecoManager.Instance.IsEnabled)
		{
			base.OnBlockEntityTransformAfterActivated(_world, _blockPos, _blockValue, _ebcd);
			return;
		}
		_ebcd.transform.tag = "T_Deco";
		Collider[] componentsInChildren = _ebcd.transform.GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			GameObject gameObject = componentsInChildren[i].gameObject;
			gameObject.tag = "T_Deco";
			gameObject.layer = 16;
		}
	}
}
