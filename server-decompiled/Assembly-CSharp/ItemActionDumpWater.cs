using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005AB RID: 1451
[Preserve]
public class ItemActionDumpWater : ItemAction
{
	// Token: 0x06002F78 RID: 12152 RVA: 0x0012F46A File Offset: 0x0012D66A
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		if (_props.Values.ContainsKey("Change_item_to"))
		{
			this.changeItemToItem = _props.Values["Change_item_to"];
		}
	}

	// Token: 0x06002F79 RID: 12153 RVA: 0x0012F49B File Offset: 0x0012D69B
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionDumpWater.DumpWaterActionData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002F7A RID: 12154 RVA: 0x0012F4A4 File Offset: 0x0012D6A4
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		if (!_bReleased)
		{
			return;
		}
		if (!(_actionData.invData.item is ItemClassWaterContainer))
		{
			Debug.LogError("Cannot dump water as item is not a WaterContainer");
			return;
		}
		if (_actionData.invData.itemValue.Meta < 195)
		{
			return;
		}
		if (_actionData.lastUseTime > 0f)
		{
			return;
		}
		ItemInventoryData invData = _actionData.invData;
		if (!Voxel.Raycast(invData.world, ((EntityPlayerLocal)invData.holdingEntity).HitInfo.ray, Constants.cDigAndBuildDistance, -555266061, 4095, 0f))
		{
			return;
		}
		WorldRayHitInfo voxelRayHitInfo = Voxel.voxelRayHitInfo;
		if (voxelRayHitInfo.bHitValid)
		{
			ItemActionDumpWater.DumpWaterActionData dumpWaterActionData = (ItemActionDumpWater.DumpWaterActionData)_actionData;
			if (!this.TryFindDumpPosition(voxelRayHitInfo, out dumpWaterActionData.targetPosition))
			{
				return;
			}
			if (GameManager.Instance.World.IsWithinTraderArea(dumpWaterActionData.targetPosition) && World.SandboxUseTraderArea == TraderAreaStates.Default)
			{
				GameManager.ShowTooltip(_actionData.invData.holdingEntity as EntityPlayerLocal, "ttCannotUseAtThisTime", false, false, 0f);
				return;
			}
			if (GameManager.Instance.World.GetLandClaimOwner(dumpWaterActionData.targetPosition, GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(_actionData.invData.holdingEntity.entityId)) == EnumLandClaimOwner.Other)
			{
				GameManager.ShowTooltip(_actionData.invData.holdingEntity as EntityPlayerLocal, "ttCannotUseAtThisTime", false, false, 0f);
				return;
			}
			_actionData.lastUseTime = Time.time;
			invData.holdingEntity.RightArmAnimationUse = true;
			if (this.soundStart != null)
			{
				invData.holdingEntity.PlayOneShot(this.soundStart, false, false, false, null, 1f);
			}
		}
	}

	// Token: 0x06002F7B RID: 12155 RVA: 0x0012F630 File Offset: 0x0012D830
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryFindDumpPosition(WorldRayHitInfo hitInfo, out Vector3i blockPos)
	{
		blockPos = World.worldToBlockPos(hitInfo.hit.pos);
		if (WaterUtils.CanWaterFlowThrough(GameManager.Instance.World.GetBlock(blockPos)))
		{
			return true;
		}
		if (!hitInfo.hit.blockValue.isair)
		{
			BlockFace blockFace = hitInfo.hit.blockFace;
			switch (blockFace)
			{
			case BlockFace.Top:
				blockPos += Vector3i.up;
				goto IL_011C;
			case BlockFace.Bottom:
				blockPos += Vector3i.down;
				goto IL_011C;
			case BlockFace.North:
				blockPos += Vector3i.forward;
				goto IL_011C;
			case BlockFace.West:
				blockPos += Vector3i.right;
				goto IL_011C;
			case BlockFace.South:
				blockPos += Vector3i.back;
				goto IL_011C;
			case BlockFace.East:
				blockPos += Vector3i.left;
				goto IL_011C;
			case BlockFace.Middle:
				break;
			default:
				if (blockFace != BlockFace.None)
				{
					goto IL_011C;
				}
				break;
			}
			return false;
			IL_011C:
			if (WaterUtils.CanWaterFlowThrough(GameManager.Instance.World.GetBlock(blockPos)))
			{
				return true;
			}
		}
		blockPos = hitInfo.lastBlockPos;
		return WaterUtils.CanWaterFlowThrough(GameManager.Instance.World.GetBlock(blockPos));
	}

	// Token: 0x06002F7C RID: 12156 RVA: 0x0012A684 File Offset: 0x00128884
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return _actionData.lastUseTime != 0f && Time.time - _actionData.lastUseTime < this.Delay;
	}

	// Token: 0x06002F7D RID: 12157 RVA: 0x0012F7A4 File Offset: 0x0012D9A4
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
		if (_actionData.lastUseTime == 0f || this.IsActionRunning(_actionData))
		{
			return;
		}
		_actionData.lastUseTime = 0f;
		ItemActionDumpWater.DumpWaterActionData dumpWaterActionData = (ItemActionDumpWater.DumpWaterActionData)_actionData;
		int meta = _actionData.invData.itemValue.Meta;
		WaterValue water = GameManager.Instance.World.GetWater(dumpWaterActionData.targetPosition);
		int num = water.GetMass() + meta;
		if (num > 65535)
		{
			Debug.LogError(string.Format("Trying to dump {0} into {1} which more than the maximum mass. Mass will be clamped to {2}", meta, water.GetMass(), 65535));
			num = 65535;
		}
		NetPackageWaterSet package = NetPackageManager.GetPackage<NetPackageWaterSet>();
		package.AddChange(dumpWaterActionData.targetPosition, new WaterValue(num));
		GameManager.Instance.SetWaterRPC(package);
		if (!string.IsNullOrEmpty(this.changeItemToItem))
		{
			ItemValue item = ItemClass.GetItem(this.changeItemToItem, false);
			_actionData.invData.holdingEntity.inventory.SetItem(_actionData.invData.slotIdx, new ItemStack(item, _actionData.invData.holdingEntity.inventory.holdingCount));
		}
	}

	// Token: 0x040024D2 RID: 9426
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropChangeItemTo = "Change_item_to";

	// Token: 0x040024D3 RID: 9427
	[PublicizedFrom(EAccessModifier.Private)]
	public string changeItemToItem;

	// Token: 0x020005AC RID: 1452
	[PublicizedFrom(EAccessModifier.Private)]
	public class DumpWaterActionData : ItemActionAttackData
	{
		// Token: 0x06002F7F RID: 12159 RVA: 0x0012A704 File Offset: 0x00128904
		public DumpWaterActionData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x040024D4 RID: 9428
		public Vector3i targetPosition;
	}
}
