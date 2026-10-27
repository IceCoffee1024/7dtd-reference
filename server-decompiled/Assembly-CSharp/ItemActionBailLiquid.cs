using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200059F RID: 1439
[Preserve]
public class ItemActionBailLiquid : ItemAction
{
	// Token: 0x06002F5A RID: 12122 RVA: 0x0012BCBB File Offset: 0x00129EBB
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionBailLiquid.MyInventoryData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002F5B RID: 12123 RVA: 0x0012BCC4 File Offset: 0x00129EC4
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

	// Token: 0x06002F5C RID: 12124 RVA: 0x0012BD98 File Offset: 0x00129F98
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return _actionData.lastUseTime != 0f && Time.time - _actionData.lastUseTime < this.Delay;
	}

	// Token: 0x06002F5D RID: 12125 RVA: 0x0012BDC0 File Offset: 0x00129FC0
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

	// Token: 0x020005A0 RID: 1440
	[PublicizedFrom(EAccessModifier.Private)]
	public class MyInventoryData : ItemActionAttackData
	{
		// Token: 0x06002F5F RID: 12127 RVA: 0x0012BE18 File Offset: 0x0012A018
		public MyInventoryData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x04002493 RID: 9363
		public Vector3i targetPosition;
	}
}
