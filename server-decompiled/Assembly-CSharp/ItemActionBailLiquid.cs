using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000592 RID: 1426
[Preserve]
public class ItemActionBailLiquid : ItemAction
{
	// Token: 0x06002EDC RID: 11996 RVA: 0x00129537 File Offset: 0x00127737
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionBailLiquid.MyInventoryData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002EDD RID: 11997 RVA: 0x00129540 File Offset: 0x00127740
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		if (!_bReleased)
		{
			return;
		}
		if (_actionData.lastUseTime > 0f)
		{
			return;
		}
		ItemInventoryData invData = _actionData.invData;
		if (!Voxel.Raycast(invData.world, ((EntityPlayerLocal)invData.holdingEntity).HitInfo.ray, Constants.cDigAndBuildDistance, 16, 4095, 0f))
		{
			return;
		}
		if (Voxel.voxelRayHitInfo.bHitValid && Voxel.voxelRayHitInfo.hit.voxelData.WaterValue.HasMass())
		{
			_actionData.lastUseTime = Time.time;
			((ItemActionBailLiquid.MyInventoryData)_actionData).targetPosition = Voxel.voxelRayHitInfo.hit.blockPos;
			invData.holdingEntity.RightArmAnimationUse = true;
			if (this.soundStart != null)
			{
				invData.holdingEntity.PlayOneShot(this.soundStart, false, false, false, null, 1f);
			}
		}
	}

	// Token: 0x06002EDE RID: 11998 RVA: 0x00129614 File Offset: 0x00127814
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return _actionData.lastUseTime != 0f && Time.time - _actionData.lastUseTime < this.Delay;
	}

	// Token: 0x06002EDF RID: 11999 RVA: 0x0012963C File Offset: 0x0012783C
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
		if (_actionData.lastUseTime == 0f || this.IsActionRunning(_actionData))
		{
			return;
		}
		_actionData.lastUseTime = 0f;
		Vector3i targetPosition = ((ItemActionBailLiquid.MyInventoryData)_actionData).targetPosition;
		NetPackageWaterSet package = NetPackageManager.GetPackage<NetPackageWaterSet>();
		package.AddChange(targetPosition, WaterValue.Empty);
		GameManager.Instance.SetWaterRPC(package);
	}

	// Token: 0x02000593 RID: 1427
	[PublicizedFrom(EAccessModifier.Private)]
	public class MyInventoryData : ItemActionAttackData
	{
		// Token: 0x06002EE1 RID: 12001 RVA: 0x00129694 File Offset: 0x00127894
		public MyInventoryData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x0400244B RID: 9291
		public Vector3i targetPosition;
	}
}
