using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005A4 RID: 1444
[Preserve]
public class ItemActionCollectWater : ItemAction
{
	// Token: 0x06002F6F RID: 12143 RVA: 0x0012C477 File Offset: 0x0012A677
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		this.changeItemToItem = _props.GetString("Change_item_to");
		this.isReduceWater = _props.GetBool("ReduceWater");
	}

	// Token: 0x06002F70 RID: 12144 RVA: 0x0012C4A2 File Offset: 0x0012A6A2
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionCollectWater.CollectWaterActionData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002F71 RID: 12145 RVA: 0x0012C4AC File Offset: 0x0012A6AC
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

	// Token: 0x06002F72 RID: 12146 RVA: 0x0012BD98 File Offset: 0x00129F98
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return _actionData.lastUseTime != 0f && Time.time - _actionData.lastUseTime < this.Delay;
	}

	// Token: 0x06002F73 RID: 12147 RVA: 0x0012C5CA File Offset: 0x0012A7CA
	public override void StopHolding(ItemActionData _actionDatadata)
	{
		base.StopHolding(_actionDatadata);
		_actionDatadata.lastUseTime = 0f;
	}

	// Token: 0x06002F74 RID: 12148 RVA: 0x0012C5E0 File Offset: 0x0012A7E0
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

	// Token: 0x0400249B RID: 9371
	[PublicizedFrom(EAccessModifier.Private)]
	public string changeItemToItem;

	// Token: 0x0400249C RID: 9372
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isReduceWater;

	// Token: 0x0400249D RID: 9373
	[PublicizedFrom(EAccessModifier.Private)]
	public List<CollectWaterUtils.WaterPoint> waterPoints = new List<CollectWaterUtils.WaterPoint>();

	// Token: 0x020005A5 RID: 1445
	[PublicizedFrom(EAccessModifier.Private)]
	public class CollectWaterActionData : ItemActionAttackData
	{
		// Token: 0x06002F76 RID: 12150 RVA: 0x0012BE18 File Offset: 0x0012A018
		public CollectWaterActionData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x0400249E RID: 9374
		public Vector3i targetPosition;

		// Token: 0x0400249F RID: 9375
		public int targetMass;
	}
}
