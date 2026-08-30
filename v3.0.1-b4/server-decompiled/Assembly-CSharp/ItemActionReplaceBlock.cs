using System;
using System.Collections;
using System.Collections.Generic;
using GUI_2;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005C5 RID: 1477
[Preserve]
public class ItemActionReplaceBlock : ItemActionRanged
{
	// Token: 0x0600301C RID: 12316 RVA: 0x00136FF9 File Offset: 0x001351F9
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionReplaceBlock.ItemActionReplaceBlockData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x0600301D RID: 12317 RVA: 0x00137002 File Offset: 0x00135202
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		if (_props.Values.ContainsKey("CopyBlock"))
		{
			this.bCopyBlock = StringParsers.ParseBool(_props.Values["CopyBlock"], 0, -1, true);
		}
	}

	// Token: 0x0600301E RID: 12318 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ConsumeScrollWheel(ItemActionData _actionData, float _scrollWheelInput, PlayerActionsLocal _playerInput)
	{
		return false;
	}

	// Token: 0x0600301F RID: 12319 RVA: 0x0001FF5D File Offset: 0x0001E15D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool checkAmmo(ItemActionData _actionData)
	{
		return true;
	}

	// Token: 0x06003020 RID: 12320 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsAmmoUsableUnderwater(EntityAlive holdingEntity)
	{
		return true;
	}

	// Token: 0x06003021 RID: 12321 RVA: 0x0013703C File Offset: 0x0013523C
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		AnimationDelayData.AnimationDelays animationDelays = AnimationDelayData.AnimationDelay[_actionData.invData.item.HoldType.Value];
		this.rayCastDelay = (((double)_actionData.invData.holdingEntity.speedForward > 0.009) ? animationDelays.RayCastMoving : animationDelays.RayCast);
		base.ExecuteAction(_actionData, _bReleased);
	}

	// Token: 0x06003022 RID: 12322 RVA: 0x001370A1 File Offset: 0x001352A1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override Vector3 fireShot(int _shotIdx, ItemActionRanged.ItemActionDataRanged _actionData, ref bool hitEntity)
	{
		hitEntity = true;
		GameManager.Instance.StartCoroutine(this.fireShotLater(_shotIdx, _actionData));
		return Vector3.zero;
	}

	// Token: 0x06003023 RID: 12323 RVA: 0x001370C0 File Offset: 0x001352C0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool checkBlockCanBeChanged(World _world, Vector3i _blockPos, int _entityId)
	{
		PersistentPlayerData playerDataFromEntityID = GameManager.Instance.GetPersistentPlayerList().GetPlayerDataFromEntityID(_entityId);
		return _world.CanPlaceBlockAt(_blockPos, playerDataFromEntityID, false);
	}

	// Token: 0x06003024 RID: 12324 RVA: 0x001370E7 File Offset: 0x001352E7
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator fireShotLater(int _shotIdx, ItemActionRanged.ItemActionDataRanged _actionData)
	{
		yield return new WaitForSeconds(this.rayCastDelay);
		EntityPlayerLocal entityPlayerLocal = (EntityPlayerLocal)_actionData.invData.holdingEntity;
		Vector3i vector3i;
		BlockValue blockValue;
		WorldRayHitInfo worldRayHitInfo;
		if (!this.GetHitBlock(_actionData, out vector3i, out blockValue, out worldRayHitInfo) || worldRayHitInfo == null || !worldRayHitInfo.bHitValid)
		{
			yield break;
		}
		ChunkCluster chunkCache = GameManager.Instance.World.ChunkCache;
		if (chunkCache == null)
		{
			yield break;
		}
		ItemActionReplaceBlock.ItemActionReplaceBlockData itemActionReplaceBlockData = (ItemActionReplaceBlock.ItemActionReplaceBlockData)_actionData;
		if (this.bCopyBlock)
		{
			int num = 1 - _actionData.indexInEntityOfAction;
			if (_actionData.invData.actionData[num] != null)
			{
				ItemActionReplaceBlock.ItemActionReplaceBlockData itemActionReplaceBlockData2 = (ItemActionReplaceBlock.ItemActionReplaceBlockData)_actionData.invData.actionData[num];
				itemActionReplaceBlockData2.Block = new BlockValue?(blockValue);
				itemActionReplaceBlockData2.PaintTextures = chunkCache.GetTextureFullArray(vector3i);
				itemActionReplaceBlockData2.Density = chunkCache.GetDensity(vector3i);
				this.isHUDDirty = true;
			}
			yield break;
		}
		if (itemActionReplaceBlockData.ReplaceBlockClass == null)
		{
			GameManager.ShowTooltip(entityPlayerLocal, Localization.Get("xuiReplaceBlockNoBlockCopied", false, null), false, false, 0f);
			yield break;
		}
		if (!this.checkBlockCanBeChanged(GameManager.Instance.World, vector3i, entityPlayerLocal.entityId))
		{
			yield break;
		}
		if (itemActionReplaceBlockData.ReplaceMode == ItemActionReplaceBlock.EnumReplaceMode.SingleBlock)
		{
			BlockToolSelection.Instance.BeginUndo();
			GameManager.Instance.SetBlocksRPC(new List<BlockChangeInfo> { this.replaceSingleBlock(chunkCache, vector3i, itemActionReplaceBlockData) }, null);
			BlockToolSelection.Instance.EndUndo(false);
		}
		else
		{
			BlockToolSelection blockToolSelection = GameManager.Instance.GetActiveBlockTool() as BlockToolSelection;
			Vector3i vector3i2;
			Vector3i vector3i3;
			if (blockToolSelection == null || !blockToolSelection.SelectionActive)
			{
				if (PrefabEditModeManager.Instance == null || !PrefabEditModeManager.Instance.IsActive())
				{
					GameManager.ShowTooltip(entityPlayerLocal, Localization.Get("xuiReplaceBlockRequiresSelection", false, null), false, false, 0f);
					yield break;
				}
				PrefabEditModeManager.Instance.UpdateMinMax();
				vector3i2 = PrefabEditModeManager.Instance.minPos;
				vector3i3 = PrefabEditModeManager.Instance.maxPos;
			}
			else
			{
				vector3i2 = blockToolSelection.SelectionStart;
				vector3i3 = blockToolSelection.SelectionEnd;
			}
			BlockToolSelection.Instance.BeginUndo();
			this.replace(chunkCache, itemActionReplaceBlockData, blockValue, vector3i2, vector3i3);
			BlockToolSelection.Instance.EndUndo(false);
		}
		yield break;
	}

	// Token: 0x06003025 RID: 12325 RVA: 0x00137100 File Offset: 0x00135300
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockChangeInfo replaceSingleBlock(ChunkCluster _cc, Vector3i _blockPos, ItemActionReplaceBlock.ItemActionReplaceBlockData _actionData)
	{
		BlockValue block = _cc.GetBlock(_blockPos);
		BlockChangeInfo blockChangeInfo = new BlockChangeInfo
		{
			blockValueRef = _blockPos,
			bChangeBlockValue = true
		};
		if (_actionData.ReplacePaintMode == ItemActionReplaceBlock.EnumReplacePaintMode.ReplaceWithAirBlocks)
		{
			blockChangeInfo.blockValue = BlockValue.Air;
			blockChangeInfo.bChangeDensity = true;
			blockChangeInfo.density = MarchingCubes.DensityAir;
		}
		else
		{
			blockChangeInfo.blockValue = _actionData.Block.Value;
			blockChangeInfo.blockValue.rotation = block.rotation;
			Block block2 = block.Block;
			Block replaceBlockClass = _actionData.ReplaceBlockClass;
			if (block2.shape.IsTerrain() != replaceBlockClass.shape.IsTerrain())
			{
				blockChangeInfo.bChangeDensity = true;
				blockChangeInfo.density = _actionData.Density;
			}
			blockChangeInfo.bChangeTexture = true;
			switch (_actionData.ReplacePaintMode)
			{
			case ItemActionReplaceBlock.EnumReplacePaintMode.KeepCurrentPaint:
				blockChangeInfo.textureFull = _cc.GetTextureFullArray(_blockPos);
				break;
			case ItemActionReplaceBlock.EnumReplacePaintMode.RemoveCurrentPaint:
				blockChangeInfo.textureFull.Fill(0L);
				break;
			case ItemActionReplaceBlock.EnumReplacePaintMode.UseNewPaint:
				blockChangeInfo.textureFull = _actionData.PaintTextures;
				break;
			}
		}
		return blockChangeInfo;
	}

	// Token: 0x06003026 RID: 12326 RVA: 0x00137204 File Offset: 0x00135404
	[PublicizedFrom(EAccessModifier.Private)]
	public void replace(ChunkCluster _cc, ItemActionReplaceBlock.ItemActionReplaceBlockData _actionData, BlockValue _srcBlock, Vector3i _startPos, Vector3i _endPos)
	{
		List<BlockChangeInfo> list = new List<BlockChangeInfo>();
		Vector3i.SortBoundingBoxEdges(ref _startPos, ref _endPos);
		for (int i = _startPos.x; i <= _endPos.x; i++)
		{
			for (int j = _startPos.z; j <= _endPos.z; j++)
			{
				for (int k = _startPos.y; k <= _endPos.y; k++)
				{
					Vector3i vector3i = new Vector3i(i, k, j);
					BlockValue block = _cc.GetBlock(vector3i);
					if (!block.ischild && block.type == _srcBlock.type)
					{
						list.Add(this.replaceSingleBlock(_cc, vector3i, _actionData));
					}
				}
			}
		}
		GameManager.Instance.SetBlocksRPC(list, null);
	}

	// Token: 0x06003027 RID: 12327 RVA: 0x001372B4 File Offset: 0x001354B4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool GetHitBlock(ItemActionRanged.ItemActionDataRanged _actionData, out Vector3i _blockPos, out BlockValue _bv, out WorldRayHitInfo _hitInfo)
	{
		_bv = BlockValue.Air;
		_hitInfo = null;
		_blockPos = Vector3i.zero;
		_hitInfo = this.GetExecuteActionTarget(_actionData);
		if (_hitInfo == null || !_hitInfo.bHitValid || _hitInfo.tag == null || !GameUtils.IsBlockOrTerrain(_hitInfo.tag))
		{
			return false;
		}
		ChunkCluster chunkCache = GameManager.Instance.World.ChunkCache;
		if (chunkCache == null)
		{
			return false;
		}
		_bv = _hitInfo.hit.blockValue;
		_blockPos = _hitInfo.hit.blockPos;
		Block block = _bv.Block;
		if (_bv.ischild)
		{
			_blockPos = block.multiBlockPos.GetParentPos(_blockPos, _bv);
			_bv = chunkCache.GetBlock(_blockPos);
		}
		return true;
	}

	// Token: 0x06003028 RID: 12328 RVA: 0x00137389 File Offset: 0x00135589
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void onHoldingEntityFired(ItemActionData _actionData)
	{
		if (_actionData.indexInEntityOfAction == 0)
		{
			_actionData.invData.holdingEntity.RightArmAnimationUse = true;
			return;
		}
		_actionData.invData.holdingEntity.RightArmAnimationAttack = true;
	}

	// Token: 0x06003029 RID: 12329 RVA: 0x00010E62 File Offset: 0x0000F062
	public override EnumCameraShake GetCameraShakeType(ItemActionData _actionData)
	{
		return EnumCameraShake.None;
	}

	// Token: 0x0600302A RID: 12330 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsEditingTool()
	{
		return true;
	}

	// Token: 0x0600302B RID: 12331 RVA: 0x001373B8 File Offset: 0x001355B8
	public override string GetStat(ItemActionData _data)
	{
		Block replaceBlockClass = ((ItemActionReplaceBlock.ItemActionReplaceBlockData)_data).ReplaceBlockClass;
		if (replaceBlockClass == null)
		{
			return "No Block";
		}
		return replaceBlockClass.GetLocalizedBlockName();
	}

	// Token: 0x0600302C RID: 12332 RVA: 0x001373E0 File Offset: 0x001355E0
	public override bool IsStatChanged()
	{
		bool flag = this.isHUDDirty;
		this.isHUDDirty = false;
		return flag;
	}

	// Token: 0x0600302D RID: 12333 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool HasRadial()
	{
		return true;
	}

	// Token: 0x0600302E RID: 12334 RVA: 0x001373F0 File Offset: 0x001355F0
	public override void SetupRadial(XUiC_Radial _xuiRadialWindow, EntityPlayerLocal _epl)
	{
		ItemActionReplaceBlock.ItemActionReplaceBlockData itemActionReplaceBlockData = (ItemActionReplaceBlock.ItemActionReplaceBlockData)_epl.inventory.holdingItemData.actionData[1];
		_xuiRadialWindow.ResetRadialEntries();
		_xuiRadialWindow.CreateRadialEntry(0, "ui_game_symbol_paint_brush", "UIAtlas", "", Localization.Get("xuiReplaceBlockSingle", false, null), itemActionReplaceBlockData.ReplaceMode == ItemActionReplaceBlock.EnumReplaceMode.SingleBlock);
		_xuiRadialWindow.CreateRadialEntry(1, "ui_game_symbol_paint_spraygun", "UIAtlas", "", Localization.Get("xuiReplaceBlockMulti", false, null), itemActionReplaceBlockData.ReplaceMode == ItemActionReplaceBlock.EnumReplaceMode.AllIdenticalBlocks);
		_xuiRadialWindow.CreateRadialEntry(2, "ui_game_symbol_brick", "UIAtlas", "", Localization.Get("xuiReplaceBlockKeepPaint", false, null), itemActionReplaceBlockData.ReplacePaintMode == ItemActionReplaceBlock.EnumReplacePaintMode.KeepCurrentPaint);
		_xuiRadialWindow.CreateRadialEntry(3, "ui_game_symbol_destruction", "UIAtlas", "", Localization.Get("xuiReplaceBlockRemovePaint", false, null), itemActionReplaceBlockData.ReplacePaintMode == ItemActionReplaceBlock.EnumReplacePaintMode.RemoveCurrentPaint);
		_xuiRadialWindow.CreateRadialEntry(4, "ui_game_symbol_paint_copy_block", "UIAtlas", "", Localization.Get("xuiReplaceBlockUseNewPaint", false, null), itemActionReplaceBlockData.ReplacePaintMode == ItemActionReplaceBlock.EnumReplacePaintMode.UseNewPaint);
		_xuiRadialWindow.CreateRadialEntry(5, "ui_game_symbol_x", "UIAtlas", "", Localization.Get("xuiReplaceBlockPlaceAir", false, null), itemActionReplaceBlockData.ReplacePaintMode == ItemActionReplaceBlock.EnumReplacePaintMode.ReplaceWithAirBlocks);
		_xuiRadialWindow.SetCommonData(UIUtils.GetButtonIconForAction(_epl.playerInput.Activate), new XUiC_Radial.CommandHandlerDelegate(this.handleRadialCommand), new XUiC_Radial.RadialContextHoldingSlotIndex(_epl.inventory.holdingItemIdx), -1, false, new XUiC_Radial.RadialStillValidDelegate(XUiC_Radial.RadialValidSameHoldingSlotIndex));
	}

	// Token: 0x0600302F RID: 12335 RVA: 0x00137564 File Offset: 0x00135764
	[PublicizedFrom(EAccessModifier.Private)]
	public new void handleRadialCommand(XUiC_Radial _sender, int _commandIndex, XUiC_Radial.RadialContextAbs _context)
	{
		EntityPlayerLocal entityPlayer = _sender.xui.playerUI.entityPlayer;
		ItemClass holdingItem = entityPlayer.inventory.holdingItem;
		ItemInventoryData holdingItemData = entityPlayer.inventory.holdingItemData;
		ItemActionReplaceBlock itemActionReplaceBlock = (ItemActionReplaceBlock)holdingItem.Actions[0];
		ItemActionReplaceBlock itemActionReplaceBlock2 = (ItemActionReplaceBlock)holdingItem.Actions[1];
		ItemActionReplaceBlock.ItemActionReplaceBlockData itemActionReplaceBlockData = (ItemActionReplaceBlock.ItemActionReplaceBlockData)holdingItemData.actionData[0];
		ItemActionReplaceBlock.ItemActionReplaceBlockData itemActionReplaceBlockData2 = (ItemActionReplaceBlock.ItemActionReplaceBlockData)holdingItemData.actionData[1];
		switch (_commandIndex)
		{
		case 0:
			itemActionReplaceBlockData2.ReplaceMode = ItemActionReplaceBlock.EnumReplaceMode.SingleBlock;
			break;
		case 1:
			itemActionReplaceBlockData2.ReplaceMode = ItemActionReplaceBlock.EnumReplaceMode.AllIdenticalBlocks;
			break;
		case 2:
			itemActionReplaceBlockData2.ReplacePaintMode = ItemActionReplaceBlock.EnumReplacePaintMode.KeepCurrentPaint;
			break;
		case 3:
			itemActionReplaceBlockData2.ReplacePaintMode = ItemActionReplaceBlock.EnumReplacePaintMode.RemoveCurrentPaint;
			break;
		case 4:
			itemActionReplaceBlockData2.ReplacePaintMode = ItemActionReplaceBlock.EnumReplacePaintMode.UseNewPaint;
			break;
		case 5:
			itemActionReplaceBlockData2.ReplacePaintMode = ItemActionReplaceBlock.EnumReplacePaintMode.ReplaceWithAirBlocks;
			break;
		}
		this.isHUDDirty = true;
	}

	// Token: 0x04002553 RID: 9555
	[PublicizedFrom(EAccessModifier.Private)]
	public float rayCastDelay;

	// Token: 0x04002554 RID: 9556
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bCopyBlock;

	// Token: 0x04002555 RID: 9557
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isHUDDirty = true;

	// Token: 0x020005C6 RID: 1478
	public enum EnumReplaceMode
	{
		// Token: 0x04002557 RID: 9559
		SingleBlock,
		// Token: 0x04002558 RID: 9560
		AllIdenticalBlocks
	}

	// Token: 0x020005C7 RID: 1479
	public enum EnumReplacePaintMode
	{
		// Token: 0x0400255A RID: 9562
		KeepCurrentPaint,
		// Token: 0x0400255B RID: 9563
		RemoveCurrentPaint,
		// Token: 0x0400255C RID: 9564
		UseNewPaint,
		// Token: 0x0400255D RID: 9565
		ReplaceWithAirBlocks
	}

	// Token: 0x020005C8 RID: 1480
	public class ItemActionReplaceBlockData : ItemActionRanged.ItemActionDataRanged
	{
		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06003031 RID: 12337 RVA: 0x00137640 File Offset: 0x00135840
		public Block ReplaceBlockClass
		{
			get
			{
				if (this.ReplacePaintMode == ItemActionReplaceBlock.EnumReplacePaintMode.ReplaceWithAirBlocks)
				{
					return global::Block.GetBlockByName("air", true);
				}
				if (this.Block == null)
				{
					return null;
				}
				return this.Block.Value.Block;
			}
		}

		// Token: 0x06003032 RID: 12338 RVA: 0x00137684 File Offset: 0x00135884
		public ItemActionReplaceBlockData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x0400255E RID: 9566
		public BlockValue? Block;

		// Token: 0x0400255F RID: 9567
		public TextureFullArray PaintTextures;

		// Token: 0x04002560 RID: 9568
		public sbyte Density;

		// Token: 0x04002561 RID: 9569
		public ItemActionReplaceBlock.EnumReplaceMode ReplaceMode;

		// Token: 0x04002562 RID: 9570
		public ItemActionReplaceBlock.EnumReplacePaintMode ReplacePaintMode;
	}
}
