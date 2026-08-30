using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020000F7 RID: 247
[Preserve]
public class BlockPlacementPineLeaves : BlockPlacement
{
	// Token: 0x06000632 RID: 1586 RVA: 0x0002C348 File Offset: 0x0002A548
	public override BlockPlacement.Result OnPlaceBlock(BlockPlacement.EnumPlacement _placement, BlockPlacement.EnumRotationMode _mode, int _localRot, WorldBase _world, BlockValue _blockValue, PropTransform _propTransform, HitInfoDetails _hitInfo, Vector3 _entityPos)
	{
		if (_mode != BlockPlacement.EnumRotationMode.Auto)
		{
			return base.OnPlaceBlock(_placement, _mode, _localRot, _world, _blockValue, _propTransform, _hitInfo, _entityPos);
		}
		BlockPlacement.Result result = new BlockPlacement.Result(_placement, _blockValue, _propTransform, _hitInfo);
		result.blockValue.rotation = 0;
		return result;
	}
}
