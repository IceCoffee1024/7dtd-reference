using System;
using UnityEngine.Scripting;

// Token: 0x020000F6 RID: 246
[Preserve]
public class BlockPlacementDrawBridge : BlockPlacementTowardsPlacer
{
	// Token: 0x06000630 RID: 1584 RVA: 0x0002C300 File Offset: 0x0002A500
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
