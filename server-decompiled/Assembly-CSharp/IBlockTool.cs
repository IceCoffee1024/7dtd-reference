using System;
using UnityEngine;

// Token: 0x020001C7 RID: 455
public interface IBlockTool
{
	// Token: 0x06000DDF RID: 3551
	void CheckKeys(ItemInventoryData _data, WorldRayHitInfo _hitInfo, PlayerActionsLocal playerActions);

	// Token: 0x06000DE0 RID: 3552
	void CheckSpecialKeys(Event _event, PlayerActionsLocal playerActions);

	// Token: 0x06000DE1 RID: 3553
	bool ConsumeScrollWheel(ItemInventoryData _data, float _scrollWheelInput, PlayerActionsLocal _playerInput);

	// Token: 0x06000DE2 RID: 3554
	bool ExecuteAttackAction(ItemInventoryData _data, bool _bReleased, PlayerActionsLocal playerActions);

	// Token: 0x06000DE3 RID: 3555
	bool ExecuteUseAction(ItemInventoryData _data, bool _bReleased, PlayerActionsLocal playerActions);

	// Token: 0x06000DE4 RID: 3556
	string GetDebugOutput();
}
