using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000597 RID: 1431
[Preserve]
public class ItemActionCollectWater : ItemAction
{
	// Token: 0x06002EF1 RID: 12017 RVA: 0x00129D23 File Offset: 0x00127F23
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		this.changeItemToItem = _props.GetString("Change_item_to");
		this.isReduceWater = _props.GetBool("ReduceWater");
	}

	// Token: 0x06002EF2 RID: 12018 RVA: 0x00129D4E File Offset: 0x00127F4E
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionCollectWater.CollectWaterActionData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002EF3 RID: 12019 RVA: 0x00129D58 File Offset: 0x00127F58
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

	// Token: 0x06002EF4 RID: 12020 RVA: 0x00129614 File Offset: 0x00127814
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return _actionData.lastUseTime != 0f && Time.time - _actionData.lastUseTime < this.Delay;
	}

	// Token: 0x06002EF5 RID: 12021 RVA: 0x00129E76 File Offset: 0x00128076
	public override void StopHolding(ItemActionData _actionDatadata)
	{
		base.StopHolding(_actionDatadata);
		_actionDatadata.lastUseTime = 0f;
	}

	// Token: 0x06002EF6 RID: 12022 RVA: 0x00129E8C File Offset: 0x0012808C
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

	// Token: 0x04002453 RID: 9299
	[PublicizedFrom(EAccessModifier.Private)]
	public string changeItemToItem;

	// Token: 0x04002454 RID: 9300
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isReduceWater;

	// Token: 0x04002455 RID: 9301
	[PublicizedFrom(EAccessModifier.Private)]
	public List<CollectWaterUtils.WaterPoint> waterPoints = new List<CollectWaterUtils.WaterPoint>();

	// Token: 0x02000598 RID: 1432
	[PublicizedFrom(EAccessModifier.Private)]
	public class CollectWaterActionData : ItemActionAttackData
	{
		// Token: 0x06002EF8 RID: 12024 RVA: 0x00129694 File Offset: 0x00127894
		public CollectWaterActionData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x04002456 RID: 9302
		public Vector3i targetPosition;

		// Token: 0x04002457 RID: 9303
		public int targetMass;
	}
}
