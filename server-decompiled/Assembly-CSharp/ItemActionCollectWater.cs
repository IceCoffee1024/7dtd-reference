using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200059C RID: 1436
[Preserve]
public class ItemActionCollectWater : ItemAction
{
	// Token: 0x06002F1F RID: 12063 RVA: 0x0012AD93 File Offset: 0x00128F93
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		this.changeItemToItem = _props.GetString("Change_item_to");
		this.isReduceWater = _props.GetBool("ReduceWater");
	}

	// Token: 0x06002F20 RID: 12064 RVA: 0x0012ADBE File Offset: 0x00128FBE
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionCollectWater.CollectWaterActionData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002F21 RID: 12065 RVA: 0x0012ADC8 File Offset: 0x00128FC8
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		if (!_bReleased)
		{
			return;
		}
		int num = 19500;
		ItemClassWaterContainer itemClassWaterContainer = _actionData.invData.item as ItemClassWaterContainer;
		if (itemClassWaterContainer != null)
		{
			int meta = _actionData.invData.itemValue.Meta;
			num = Mathf.Max(0, itemClassWaterContainer.MaxMass - meta);
		}
		if (num < 195)
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
			ItemActionCollectWater.CollectWaterActionData collectWaterActionData = (ItemActionCollectWater.CollectWaterActionData)_actionData;
			collectWaterActionData.targetPosition = Voxel.voxelRayHitInfo.hit.blockPos;
			collectWaterActionData.targetMass = num;
			invData.holdingEntity.RightArmAnimationUse = true;
			if (this.soundStart != null)
			{
				invData.holdingEntity.PlayOneShot(this.soundStart, false, false, false, null, 1f);
			}
		}
	}

	// Token: 0x06002F22 RID: 12066 RVA: 0x0012A684 File Offset: 0x00128884
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return _actionData.lastUseTime != 0f && Time.time - _actionData.lastUseTime < this.Delay;
	}

	// Token: 0x06002F23 RID: 12067 RVA: 0x0012AEE6 File Offset: 0x001290E6
	public override void StopHolding(ItemActionData _actionDatadata)
	{
		base.StopHolding(_actionDatadata);
		_actionDatadata.lastUseTime = 0f;
	}

	// Token: 0x06002F24 RID: 12068 RVA: 0x0012AEFC File Offset: 0x001290FC
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
		if (_actionData.lastUseTime == 0f || this.IsActionRunning(_actionData))
		{
			return;
		}
		_actionData.lastUseTime = 0f;
		World world = GameManager.Instance.World;
		ChunkCluster chunkCluster = ((world != null) ? world.ChunkCache : null);
		if (chunkCluster == null)
		{
			return;
		}
		ItemActionCollectWater.CollectWaterActionData collectWaterActionData = (ItemActionCollectWater.CollectWaterActionData)_actionData;
		int num = CollectWaterUtils.CollectWater(chunkCluster, collectWaterActionData.targetMass, collectWaterActionData.targetPosition, 2, this.waterPoints);
		if (num > 195)
		{
			if (this.isReduceWater)
			{
				NetPackageWaterSet package = NetPackageManager.GetPackage<NetPackageWaterSet>();
				foreach (CollectWaterUtils.WaterPoint waterPoint in this.waterPoints)
				{
					if (waterPoint.massToTake > 0)
					{
						package.AddChange(waterPoint.worldPos, new WaterValue(waterPoint.finalMass));
					}
				}
				GameManager.Instance.SetWaterRPC(package);
			}
			if (!string.IsNullOrEmpty(this.changeItemToItem))
			{
				ItemStack itemStack = new ItemStack(ItemClass.GetItem(this.changeItemToItem, false), _actionData.invData.holdingEntity.inventory.holdingCount);
				itemStack.itemValue.Meta = _actionData.invData.itemValue.Meta + num;
				_actionData.invData.holdingEntity.inventory.SetItem(_actionData.invData.slotIdx, itemStack);
			}
		}
		this.waterPoints.Clear();
	}

	// Token: 0x04002487 RID: 9351
	[PublicizedFrom(EAccessModifier.Private)]
	public string changeItemToItem;

	// Token: 0x04002488 RID: 9352
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isReduceWater;

	// Token: 0x04002489 RID: 9353
	[PublicizedFrom(EAccessModifier.Private)]
	public List<CollectWaterUtils.WaterPoint> waterPoints = new List<CollectWaterUtils.WaterPoint>();

	// Token: 0x0200059D RID: 1437
	[PublicizedFrom(EAccessModifier.Private)]
	public class CollectWaterActionData : ItemActionAttackData
	{
		// Token: 0x06002F26 RID: 12070 RVA: 0x0012A704 File Offset: 0x00128904
		public CollectWaterActionData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x0400248A RID: 9354
		public Vector3i targetPosition;

		// Token: 0x0400248B RID: 9355
		public int targetMass;
	}
}
