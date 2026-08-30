using System;
using UnityEngine;

// Token: 0x020001C4 RID: 452
public interface IBlockTool
{
	// Token: 0x06000DDD RID: 3549
	void CheckKeys(ItemInventoryData _data, WorldRayHitInfo _hitInfo, PlayerActionsLocal playerActions);

	// Token: 0x06000DDE RID: 3550
	void CheckSpecialKeys(Event _event, PlayerActionsLocal playerActions);

	// Token: 0x06000DDF RID: 3551
	bool ConsumeScrollWheel(ItemInventoryData _data, float _scrollWheelInput, PlayerActionsLocal _playerInput);

	// Token: 0x06000DE0 RID: 3552
	bool ExecuteAttackAction(ItemInventoryData _data, bool _bReleased, PlayerActionsLocal playerActions);

	// Token: 0x06000DE1 RID: 3553
	bool ExecuteUseAction(ItemInventoryData _data, bool _bReleased, PlayerActionsLocal playerActions);

	// Token: 0x06000DE2 RID: 3554
	string GetDebugOutput();
}
