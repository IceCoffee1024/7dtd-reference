using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000598 RID: 1432
[Preserve]
public class ItemActionBailLiquid : ItemAction
{
	// Token: 0x06002F4C RID: 12108 RVA: 0x0012B0C7 File Offset: 0x001292C7
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionBailLiquid.MyInventoryData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002F4D RID: 12109 RVA: 0x0012B0D0 File Offset: 0x001292D0
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

	// Token: 0x06002F4E RID: 12110 RVA: 0x0012B1A4 File Offset: 0x001293A4
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return _actionData.lastUseTime != 0f && Time.time - _actionData.lastUseTime < this.Delay;
	}

	// Token: 0x06002F4F RID: 12111 RVA: 0x0012B1CC File Offset: 0x001293CC
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

	// Token: 0x02000599 RID: 1433
	[PublicizedFrom(EAccessModifier.Private)]
	public class MyInventoryData : ItemActionAttackData
	{
		// Token: 0x06002F51 RID: 12113 RVA: 0x0012B224 File Offset: 0x00129424
		public MyInventoryData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x04002478 RID: 9336
		public Vector3i targetPosition;
	}
}
