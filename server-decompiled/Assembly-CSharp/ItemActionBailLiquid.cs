using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000597 RID: 1431
[Preserve]
public class ItemActionBailLiquid : ItemAction
{
	// Token: 0x06002F0A RID: 12042 RVA: 0x0012A5A7 File Offset: 0x001287A7
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionBailLiquid.MyInventoryData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002F0B RID: 12043 RVA: 0x0012A5B0 File Offset: 0x001287B0
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

	// Token: 0x06002F0C RID: 12044 RVA: 0x0012A684 File Offset: 0x00128884
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return _actionData.lastUseTime != 0f && Time.time - _actionData.lastUseTime < this.Delay;
	}

	// Token: 0x06002F0D RID: 12045 RVA: 0x0012A6AC File Offset: 0x001288AC
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

	// Token: 0x02000598 RID: 1432
	[PublicizedFrom(EAccessModifier.Private)]
	public class MyInventoryData : ItemActionAttackData
	{
		// Token: 0x06002F0F RID: 12047 RVA: 0x0012A704 File Offset: 0x00128904
		public MyInventoryData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x0400247F RID: 9343
		public Vector3i targetPosition;
	}
}
