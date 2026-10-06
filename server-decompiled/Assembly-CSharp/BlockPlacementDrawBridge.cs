using System;
using UnityEngine.Scripting;

// Token: 0x020000F9 RID: 249
[Preserve]
public class BlockPlacementDrawBridge : BlockPlacementTowardsPlacer
{
	// Token: 0x06000640 RID: 1600 RVA: 0x0002C6B4 File Offset: 0x0002A8B4
	public override byte LimitRotation(BlockPlacement.EnumRotationMode _mode, ref int _localRot, HitInfoDetails _hitInfo, bool _bAdd, BlockValue _bv, byte _rotation)
	{
		if (_mode != BlockPlacement.EnumRotationMode.Auto)
		{
			return base.LimitRotation(_mode, ref _localRot, _hitInfo, _bAdd, _bv, _rotation);
		}
		int num = (int)(_bAdd ? (_rotation + 1) : (_rotation - 1));
		if (num > 3)
		{
			return 0;
		}
		if (num < 0)
		{
			return 3;
		}
		return (byte)num;
	}
}
