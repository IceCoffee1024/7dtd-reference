using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020001A9 RID: 425
[Preserve]
public class BlockShapeDistantDecoTree : BlockShapeDistantDeco
{
	// Token: 0x06000CC6 RID: 3270 RVA: 0x0005084F File Offset: 0x0004EA4F
	public BlockShapeDistantDecoTree()
	{
		this.Has45DegreeRotations = true;
	}

	// Token: 0x06000CC7 RID: 3271 RVA: 0x00050860 File Offset: 0x0004EA60
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
			gameObject.layer = 23;
		}
		Transform transform = _ebcd.transform.Find("rootBall");
		if (transform)
		{
			transform.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000CC8 RID: 3272 RVA: 0x000508F1 File Offset: 0x0004EAF1
	public override void OnBlockRemoved(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		if (!DecoManager.Instance.IsEnabled)
		{
			base.OnBlockRemoved(_world, _chunk, _blockPos, _blockValue);
			return;
		}
		DecoManager.Instance.RemoveDecorationAt(_blockPos);
	}
}
