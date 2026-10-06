using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005AC RID: 1452
[Preserve]
public class ItemActionDumpWater : ItemAction
{
	// Token: 0x06002FBB RID: 12219 RVA: 0x0012FF3A File Offset: 0x0012E13A
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		if (_props.Values.ContainsKey("Change_item_to"))
		{
			this.changeItemToItem = _props.Values["Change_item_to"];
		}
	}

	// Token: 0x06002FBC RID: 12220 RVA: 0x0012FF6B File Offset: 0x0012E16B
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionDumpWater.DumpWaterActionData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002FBD RID: 12221 RVA: 0x0012FF74 File Offset: 0x0012E174
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

	// Token: 0x06002FBE RID: 12222 RVA: 0x00130100 File Offset: 0x0012E300
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

	// Token: 0x06002FBF RID: 12223 RVA: 0x0012B1A4 File Offset: 0x001293A4
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return _actionData.lastUseTime != 0f && Time.time - _actionData.lastUseTime < this.Delay;
	}

	// Token: 0x06002FC0 RID: 12224 RVA: 0x00130274 File Offset: 0x0012E474
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
			_actionData.invData.itemStack.Set(item, _actionData.invData.itemStack.count);
		}
	}

	// Token: 0x040024CB RID: 9419
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropChangeItemTo = "Change_item_to";

	// Token: 0x040024CC RID: 9420
	[PublicizedFrom(EAccessModifier.Private)]
	public string changeItemToItem;

	// Token: 0x020005AD RID: 1453
	[PublicizedFrom(EAccessModifier.Private)]
	public class DumpWaterActionData : ItemActionAttackData
	{
		// Token: 0x06002FC2 RID: 12226 RVA: 0x0012B224 File Offset: 0x00129424
		public DumpWaterActionData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x040024CD RID: 9421
		public Vector3i targetPosition;
	}
}
