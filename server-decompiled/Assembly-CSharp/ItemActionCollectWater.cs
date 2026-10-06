using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200059D RID: 1437
[Preserve]
public class ItemActionCollectWater : ItemAction
{
	// Token: 0x06002F61 RID: 12129 RVA: 0x0012B883 File Offset: 0x00129A83
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		this.changeItemToItem = _props.GetString("Change_item_to");
		this.isReduceWater = _props.GetBool("ReduceWater");
	}

	// Token: 0x06002F62 RID: 12130 RVA: 0x0012B8AE File Offset: 0x00129AAE
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionCollectWater.CollectWaterActionData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002F63 RID: 12131 RVA: 0x0012B8B8 File Offset: 0x00129AB8
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

	// Token: 0x06002F64 RID: 12132 RVA: 0x0012B1A4 File Offset: 0x001293A4
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return _actionData.lastUseTime != 0f && Time.time - _actionData.lastUseTime < this.Delay;
	}

	// Token: 0x06002F65 RID: 12133 RVA: 0x0012B9D6 File Offset: 0x00129BD6
	public override void StopHolding(ItemActionData _actionDatadata)
	{
		base.StopHolding(_actionDatadata);
		_actionDatadata.lastUseTime = 0f;
	}

	// Token: 0x06002F66 RID: 12134 RVA: 0x0012B9EC File Offset: 0x00129BEC
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
				ItemValue item = ItemClass.GetItem(this.changeItemToItem, false);
				item.Meta = _actionData.invData.itemValue.Meta + num;
				_actionData.invData.itemStack.Set(item, _actionData.invData.itemStack.count);
			}
		}
		this.waterPoints.Clear();
	}

	// Token: 0x04002480 RID: 9344
	[PublicizedFrom(EAccessModifier.Private)]
	public string changeItemToItem;

	// Token: 0x04002481 RID: 9345
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isReduceWater;

	// Token: 0x04002482 RID: 9346
	[PublicizedFrom(EAccessModifier.Private)]
	public List<CollectWaterUtils.WaterPoint> waterPoints = new List<CollectWaterUtils.WaterPoint>();

	// Token: 0x0200059E RID: 1438
	[PublicizedFrom(EAccessModifier.Private)]
	public class CollectWaterActionData : ItemActionAttackData
	{
		// Token: 0x06002F68 RID: 12136 RVA: 0x0012B224 File Offset: 0x00129424
		public CollectWaterActionData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x04002483 RID: 9347
		public Vector3i targetPosition;

		// Token: 0x04002484 RID: 9348
		public int targetMass;
	}
}
