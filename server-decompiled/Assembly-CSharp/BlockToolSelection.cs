using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Platform;
using UnityEngine;

// Token: 0x020001BF RID: 447
public class BlockToolSelection : ISelectionBoxCallback, IBlockTool
{
	// Token: 0x170000FB RID: 251
	// (get) Token: 0x06000D63 RID: 3427 RVA: 0x00057F30 File Offset: 0x00056130
	public SelectionBox SelectionBox
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			SelectionBox selectionBox;
			if (SelectionBoxManager.Instance.CategorySelection.TryGetBox("SingleInstance", out selectionBox))
			{
				return selectionBox;
			}
			selectionBox = SelectionBoxManager.Instance.CategorySelection.AddBox("SingleInstance", Vector3i.zero, Vector3i.one, false, false);
			selectionBox.SetVisible(false);
			selectionBox.SetSizeVisibility(true);
			return selectionBox;
		}
	}

	// Token: 0x06000D64 RID: 3428 RVA: 0x00057F88 File Offset: 0x00056188
	public BlockToolSelection()
	{
		BlockToolSelection.Instance = this;
		SelectionBoxManager.Instance.CategorySelection.SetCallback(this);
		PlayerActionsLocal primaryPlayer = PlatformManager.NativePlatform.Input.PrimaryPlayer;
		NGuiAction nguiAction = new NGuiAction(Localization.Get("selectionToolsEditBlocksVolume", false, null), null, true);
		nguiAction.SetClickActionDelegate(delegate
		{
			GameManager.bVolumeBlocksEditing = !GameManager.bVolumeBlocksEditing;
		});
		nguiAction.SetIsCheckedDelegate(() => GameManager.bVolumeBlocksEditing);
		nguiAction.SetIsVisibleDelegate(() => GameManager.Instance.IsEditMode());
		NGuiAction nguiAction2 = new NGuiAction(Localization.Get("selectionToolsCopyAirBlocks", false, null), null, true);
		nguiAction2.SetClickActionDelegate(delegate
		{
			this.copyPasteAirBlocks = !this.copyPasteAirBlocks;
		});
		nguiAction2.SetIsCheckedDelegate(() => this.copyPasteAirBlocks);
		nguiAction2.SetIsVisibleDelegate(new NGuiAction.IsVisibleDelegate(GameManager.Instance.IsEditMode));
		NGuiAction nguiAction3 = new NGuiAction(Localization.Get("selectionToolsClearSelection", false, null), primaryPlayer.SelectionClear);
		nguiAction3.SetClickActionDelegate(delegate
		{
			if (this.SelectionLockMode == 2)
			{
				this.SelectionLockMode = 0;
				this.SelectionActive = false;
				return;
			}
			this.BeginUndo();
			BlockTools.CubeRPC(GameManager.Instance, this.SelectionStart, this.SelectionEnd, BlockValue.Air, MarchingCubes.DensityAir, 0, TextureFullArray.Default);
			BlockTools.CubeWaterRPC(GameManager.Instance, this.SelectionStart, this.SelectionEnd, WaterValue.Empty);
			this.EndUndo(false);
		});
		nguiAction3.SetIsEnabledDelegate(() => GameManager.Instance.IsEditMode() && this.SelectionActive);
		nguiAction3.SetIsVisibleDelegate(() => GameManager.Instance.IsEditMode());
		nguiAction3.SetTooltip("selectionToolsClearSelectionTip");
		NGuiAction nguiAction4 = new NGuiAction(Localization.Get("selectionToolsFillSelection", false, null), primaryPlayer.SelectionFill);
		nguiAction4.SetClickActionDelegate(delegate
		{
			this.BeginUndo();
			EntityPlayerLocal primaryPlayer2 = GameManager.Instance.World.GetPrimaryPlayer();
			ItemValue holdingItemItemValue = primaryPlayer2.inventory.holdingItemItemValue;
			BlockValue blockValue = holdingItemItemValue.ToBlockValue(false);
			if (blockValue.isair)
			{
				return;
			}
			Block block = blockValue.Block;
			BlockPlacement.Result result = new BlockPlacement.Result(BlockPlacement.EnumPlacement.Voxel, Vector3.one * 0.5f, Vector3i.zero, BlockFace.None, blockValue, PropTransform.identity);
			block.OnBlockPlaceBefore(GameManager.Instance.World, ref result, primaryPlayer2, GameManager.Instance.World.GetGameRandom());
			blockValue = result.blockValue;
			blockValue.rotation = (primaryPlayer2.inventory.holdingItemData.IsBlock ? primaryPlayer2.inventory.holdingItemData.rotation : blockValue.rotation);
			BlockTools.CubeRPC(GameManager.Instance, this.m_selectionStartPoint, this.m_SelectionEndPoint, blockValue, blockValue.Block.shape.IsTerrain() ? MarchingCubes.DensityTerrain : MarchingCubes.DensityAir, 0, holdingItemItemValue.TextureFullArray);
			this.EndUndo(false);
		});
		nguiAction4.SetIsEnabledDelegate(() => GameManager.Instance.IsEditMode() && this.SelectionActive);
		nguiAction4.SetIsVisibleDelegate(() => GameManager.Instance.IsEditMode());
		nguiAction4.SetTooltip("selectionToolsFillSelectionTip");
		NGuiAction nguiAction5 = new NGuiAction(Localization.Get("selectionToolsRandomFillSelection", false, null), null);
		nguiAction5.SetClickActionDelegate(delegate
		{
			this.BeginUndo();
			BlockTools.CubeRandomRPC(GameManager.Instance, this.m_selectionStartPoint, this.m_SelectionEndPoint, GameManager.Instance.World.GetPrimaryPlayer().inventory.holdingItemItemValue.ToBlockValue(false), 0.1f, new EBlockRotationClasses?(EBlockRotationClasses.Basic90));
			this.EndUndo(false);
		});
		nguiAction5.SetIsEnabledDelegate(() => this.SelectionActive);
		nguiAction5.SetIsVisibleDelegate(() => GameManager.Instance.IsEditMode());
		nguiAction5.SetTooltip("selectionToolsRandomFillSelectionTip");
		NGuiAction nguiAction6 = new NGuiAction(Localization.Get("selectionToolsUndo", false, null), null);
		nguiAction6.SetClickActionDelegate(delegate
		{
			this.blockUndoRedo(false);
		});
		nguiAction6.SetIsEnabledDelegate(() => this.undoQueue.Count > 0);
		nguiAction6.SetIsVisibleDelegate(() => GameManager.Instance.IsEditMode());
		nguiAction6.SetTooltip("selectionToolsUndoTip");
		NGuiAction nguiAction7 = new NGuiAction(Localization.Get("selectionToolsRedo", false, null), null);
		nguiAction7.SetClickActionDelegate(delegate
		{
			this.blockUndoRedo(true);
		});
		nguiAction7.SetIsEnabledDelegate(() => this.redoQueue.Count > 0);
		nguiAction7.SetIsVisibleDelegate(() => GameManager.Instance.IsEditMode());
		nguiAction7.SetTooltip("selectionToolsRedoTip");
		this.actions = new Dictionary<string, NGuiAction>
		{
			{ "volumeBlocksEditing", nguiAction },
			{ "copyAirBlocks", nguiAction2 },
			{
				"sep1",
				NGuiAction.Separator
			},
			{ "clearSelection", nguiAction3 },
			{ "fillSelection", nguiAction4 },
			{ "randomFillSelection", nguiAction5 },
			{
				"sep2",
				NGuiAction.Separator
			},
			{ "undo", nguiAction6 },
			{ "redo", nguiAction7 }
		};
		foreach (KeyValuePair<string, NGuiAction> keyValuePair in this.actions)
		{
			string text;
			NGuiAction nguiAction8;
			keyValuePair.Deconstruct(out text, out nguiAction8);
			NGuiAction nguiAction9 = nguiAction8;
			LocalPlayerUI.primaryUI.windowManager.AddGlobalAction(nguiAction9);
		}
		Origin.OriginChanged = (Action<Vector3>)Delegate.Combine(Origin.OriginChanged, new Action<Vector3>(this.OnOriginChanged));
	}

	// Token: 0x06000D65 RID: 3429 RVA: 0x00058404 File Offset: 0x00056604
	public void CheckSpecialKeys(Event ev, PlayerActionsLocal playerActions)
	{
		if (this.hitInfo == null)
		{
			return;
		}
		Vector3i vector3i = ((GameManager.Instance.IsEditMode() && playerActions.Run.IsPressed) ? this.hitInfo.hit.blockPos : this.hitInfo.lastBlockPos);
		bool flag = (InputUtils.IsMac ? ((ev.modifiers & EventModifiers.Command) > EventModifiers.None) : ((ev.modifiers & EventModifiers.Control) > EventModifiers.None));
		bool flag2 = (ev.modifiers & EventModifiers.Shift) > EventModifiers.None;
		KeyCode keyCode = ev.keyCode;
		if (keyCode != KeyCode.C)
		{
			if (keyCode != KeyCode.V)
			{
				if (keyCode != KeyCode.Z)
				{
					return;
				}
				if (flag)
				{
					this.blockUndoRedo(false);
				}
			}
			else if (flag)
			{
				if (!flag2 && this.SelectionLockMode != 2)
				{
					if (this.SelectionActive && this.clipboard.size.Equals(Vector3i.one) && !this.SelectionSize.Equals(this.clipboard.size))
					{
						this.BeginUndo();
						BlockValue block = this.clipboard.GetBlock(0, 0, 0);
						WaterValue water = this.clipboard.GetWater(0, 0, 0);
						TextureFullArray texture = this.clipboard.GetTexture(0, 0, 0);
						BlockTools.CubeRPC(GameManager.Instance, this.m_selectionStartPoint, this.m_SelectionEndPoint, block, block.Block.shape.IsTerrain() ? MarchingCubes.DensityTerrain : MarchingCubes.DensityAir, 0, texture);
						BlockTools.CubeWaterRPC(GameManager.Instance, this.m_selectionStartPoint, this.m_SelectionEndPoint, water);
						this.EndUndo(false);
						return;
					}
					if (this.SelectionActive && !this.SelectionSize.Equals(this.clipboard.size))
					{
						this.SelectionEnd = this.SelectionStart + this.clipboard.size - Vector3i.one;
						return;
					}
					if (!this.SelectionActive)
					{
						this.SelectionStart = vector3i;
						this.SelectionEnd = this.SelectionStart + this.clipboard.size - Vector3i.one;
						this.SelectionActive = true;
						return;
					}
					if (this.SelectionActive && this.SelectionSize.Equals(this.clipboard.size))
					{
						this.blockPaste(this.SelectionMin, this.clipboard);
						return;
					}
				}
				else
				{
					if (this.SelectionLockMode != 2)
					{
						if (this.SelectionSize != this.clipboard.size)
						{
							this.SelectionEnd = this.SelectionStart + this.clipboard.size - Vector3i.one;
						}
						this.SelectionActive = true;
						this.SelectionLockMode = 2;
						this.createBlockPreviewFrom(this.clipboard);
						return;
					}
					this.SelectionLockMode = 0;
					this.blockPaste(this.SelectionMin, this.clipboard);
					return;
				}
			}
		}
		else if (flag)
		{
			if (!this.SelectionActive)
			{
				this.SelectionStart = vector3i;
				this.SelectionEnd = vector3i;
			}
			this.blockCopy(this.clipboard);
			return;
		}
	}

	// Token: 0x06000D66 RID: 3430 RVA: 0x000586FC File Offset: 0x000568FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void rotatePreviewAroundY()
	{
		if (this.previewGORot2 == null)
		{
			return;
		}
		this.previewGORot2.transform.localRotation = Quaternion.AngleAxis(90f, Vector3.up) * this.previewGORot2.transform.localRotation;
		this.clipboard.RotateY(false, 1);
		Vector3 vector = this.previewGORot2.transform.localRotation * this.offsetToMin;
		Vector3 vector2 = this.selectionRotCenter + vector;
		Vector3 vector3 = this.previewGORot2.transform.localRotation * this.offsetToMax;
		Vector3 vector4 = this.selectionRotCenter + vector3;
		Vector3i vector3i = new Vector3i(Utils.Fastfloor(Utils.FastMin(vector2.x, vector4.x)), Utils.Fastfloor(Utils.FastMin(vector2.y, vector4.y)), Utils.Fastfloor(Utils.FastMin(vector2.z, vector4.z)));
		this.SelectionStart = vector3i;
		this.SelectionEnd = vector3i + this.clipboard.size - Vector3i.one;
	}

	// Token: 0x06000D67 RID: 3431 RVA: 0x0005881E File Offset: 0x00056A1E
	[PublicizedFrom(EAccessModifier.Private)]
	public void removeBlockPreview()
	{
		this.previewGORot3.transform.DestroyChildren();
	}

	// Token: 0x06000D68 RID: 3432 RVA: 0x00058830 File Offset: 0x00056A30
	[PublicizedFrom(EAccessModifier.Private)]
	public void createBlockPreviewFrom(Prefab _prefab)
	{
		if (this.previewGOParent == null)
		{
			this.previewGOParent = new GameObject("Preview");
			this.previewGOParent.transform.parent = null;
			this.previewGOParent.transform.localPosition = Vector3.zero;
			this.previewGORot1 = new GameObject("Rot1");
			this.previewGORot1.transform.parent = this.previewGOParent.transform;
			this.previewGORot2 = new GameObject("Rot2");
			this.previewGORot2.transform.parent = this.previewGORot1.transform;
			this.previewGORot3 = new GameObject("Rot3");
			this.previewGORot3.transform.parent = this.previewGORot2.transform;
		}
		else
		{
			this.removeBlockPreview();
		}
		ThreadManager.RunCoroutineSync(_prefab.ToTransform(true, true, true, false, this.previewGORot3.transform, "PrefabImposter", Vector3.zero, DynamicPrefabDecorator.PrefabPreviewLimit));
		Transform transform = this.previewGORot3.transform.Find("PrefabImposter");
		transform.localRotation = Quaternion.identity;
		transform.localPosition = Vector3.zero;
		Vector3 vector = new Vector3((float)(_prefab.size.x / 2), 0f, (float)(_prefab.size.z / 2));
		this.previewGORot1.transform.position = this.SelectionMin.ToVector3() - Origin.position;
		this.previewGORot1.transform.rotation = Quaternion.identity;
		this.previewGORot2.transform.localPosition = vector;
		this.previewGORot2.transform.localRotation = Quaternion.identity;
		this.previewGORot3.transform.localPosition = -vector;
		this.previewGORot3.transform.localRotation = Quaternion.identity;
		vector = -vector;
		vector.y = (float)(-(float)_prefab.size.y / 2);
		this.offsetToMax = vector + (_prefab.size - Vector3i.one).ToVector3() + Vector3.one * 0.5f;
		this.offsetToMin = vector + Vector3.one * 0.5f;
		this.selectionRotCenter = this.SelectionMin.ToVector3() - vector;
	}

	// Token: 0x06000D69 RID: 3433 RVA: 0x00058AA0 File Offset: 0x00056CA0
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnOriginChanged(Vector3 _newOrigin)
	{
		if (this.previewGORot1 == null)
		{
			return;
		}
		this.previewGORot1.transform.position = this.SelectionMin.ToVector3() - Origin.position;
	}

	// Token: 0x06000D6A RID: 3434 RVA: 0x00058AE4 File Offset: 0x00056CE4
	public void RotateFocusedBlock(WorldRayHitInfo _hitInfo, PlayerActionsLocal _playerActions)
	{
		if (!_hitInfo.bHitValid)
		{
			return;
		}
		Vector3i vector3i = ((GameManager.Instance.World.IsEditor() && _playerActions.Run.IsPressed) ? _hitInfo.hit.blockPos : _hitInfo.lastBlockPos);
		BlockValue block = GameManager.Instance.World.ChunkCache.GetBlock(vector3i);
		if (block.Block.shape.IsRotatable)
		{
			block.rotation = block.Block.shape.Rotate(false, (int)block.rotation);
			this.setBlock(vector3i, block);
		}
	}

	// Token: 0x06000D6B RID: 3435 RVA: 0x00058B84 File Offset: 0x00056D84
	public void CheckKeys(ItemInventoryData _data, WorldRayHitInfo _hitInfo, PlayerActionsLocal playerActions)
	{
		if (LocalPlayerUI.primaryUI.windowManager.IsInputActive())
		{
			return;
		}
		this.hitInfo = _hitInfo;
		bool flag = _data.world.IsEditor() && playerActions.Run.IsPressed;
		Vector3i vector3i = (flag ? _hitInfo.hit.blockPos : _hitInfo.lastBlockPos);
		BlockValueRef blockValueRef = (flag ? _hitInfo.hit.blockValueRef : new BlockValueRef(_hitInfo.lastBlockPos));
		if (_data.IsBlock)
		{
			BlockValue blockValue = _data.itemValue.ToBlockValue(false);
			blockValue.rotation = _data.rotation;
			_data.rotation = blockValue.Block.BlockPlacementHelper.OnPlaceBlock(_data.Placement, _data.mode, _data.localRot, GameManager.Instance.World, blockValue, _data.propTransform, this.hitInfo.hit, _data.holdingEntity.position).blockValue.rotation;
		}
		if (!GameManager.Instance.IsEditMode() && !GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled))
		{
			return;
		}
		if (playerActions.SelectionSet.IsPressed)
		{
			if (GameManager.Instance.World.ChunkCache == null)
			{
				return;
			}
			if (InputUtils.ControlKeyPressed)
			{
				return;
			}
			this.SelectionLockMode = 0;
			Vector3i vector3i2 = vector3i;
			if (!this.SelectionActive)
			{
				Vector3i selectionSize = this.SelectionSize;
				this.SelectionStart = vector3i2;
				if (this.SelectionLockMode == 1)
				{
					this.SelectionEnd = this.SelectionStart + selectionSize - Vector3i.one;
				}
				else
				{
					this.SelectionEnd = this.SelectionStart;
				}
				this.SelectionActive = true;
			}
			else
			{
				this.SelectionEnd = vector3i2;
			}
		}
		if (!GameManager.Instance.IsEditMode())
		{
			return;
		}
		if (playerActions.DensityM1.WasPressed || playerActions.DensityP1.WasPressed || playerActions.DensityM10.WasPressed || playerActions.DensityP10.WasPressed)
		{
			int num = ((playerActions.DensityM1.WasPressed || playerActions.DensityP1.WasPressed) ? 1 : 10);
			if (playerActions.DensityM1.WasPressed || playerActions.DensityM10.WasPressed)
			{
				num = -num;
			}
			if (InputUtils.ControlKeyPressed)
			{
				num *= 50;
			}
			BlockValue block = GameManager.Instance.World.GetBlock(blockValueRef);
			Block block2 = block.Block;
			if (block2.BlockTag == BlockTags.Door)
			{
				if (num > 0)
				{
					num = ((block.damage + num >= block2.MaxDamagePlusDowngrades) ? (block2.MaxDamagePlusDowngrades - block.damage - 1) : num);
				}
				block2.DamageBlock(GameManager.Instance.World, blockValueRef, block, num, -1, null, false, false);
			}
			else
			{
				int num2;
				if (!this.SelectionActive)
				{
					num2 = (int)GameManager.Instance.World.GetDensity(blockValueRef);
				}
				else
				{
					num2 = (int)GameManager.Instance.World.GetDensity(this.m_selectionStartPoint);
				}
				num2 += num;
				num2 = Utils.FastClamp(num2, (int)MarchingCubes.DensityTerrain, (int)MarchingCubes.DensityAir);
				if (!this.SelectionActive)
				{
					GameManager.Instance.World.SetBlocksRPC(new List<BlockChangeInfo>
					{
						new BlockChangeInfo(blockValueRef, (sbyte)num2, false)
					});
				}
				else
				{
					BlockTools.CubeDensityRPC(GameManager.Instance, this.m_selectionStartPoint, this.m_SelectionEndPoint, (sbyte)num2);
				}
			}
		}
		if ((playerActions.FocusCopyBlock.WasPressed || (playerActions.Secondary.WasPressed && InputUtils.ControlKeyPressed)) && GameManager.Instance.IsEditMode() && _hitInfo.bHitValid && !_hitInfo.hit.blockValue.isair)
		{
			EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
			BlockValue blockValue2 = _hitInfo.hit.blockValue;
			if (blockValue2.ischild)
			{
				Vector3i parentPos = blockValue2.Block.multiBlockPos.GetParentPos(_hitInfo.hit.blockPos, blockValue2);
				blockValue2 = GameManager.Instance.World.GetBlock(parentPos);
			}
			ItemStack itemStack = new ItemStack(blockValue2.ToItemValue(), 99);
			if (blockValue2.Block.GetAutoShapeType() != EAutoShapeType.Helper)
			{
				itemStack.itemValue.TextureFullArray = GameManager.Instance.World.ChunkCache.GetTextureFullArray(_hitInfo.hit.blockValueRef);
			}
			if (primaryPlayer.inventory.GetItemCount(itemStack.itemValue, true, -1, -1, true) == 0 && primaryPlayer.inventory.CanTakeItem(itemStack))
			{
				int num3;
				if (primaryPlayer.inventory.AddItem(itemStack, out num3))
				{
					primaryPlayer.inventory.SetHoldingItemIdx(num3);
					ItemInventoryData holdingItemData = primaryPlayer.inventory.holdingItemData;
					if (holdingItemData.IsBlock)
					{
						holdingItemData.damage = blockValue2.damage;
						return;
					}
				}
			}
			else if (_data.IsBlock && this.hasSameShape(blockValue2.type, primaryPlayer.inventory.holdingItemItemValue.type))
			{
				_data.rotation = blockValue2.rotation;
				_data.damage = blockValue2.damage;
			}
		}
	}

	// Token: 0x06000D6C RID: 3436 RVA: 0x0005906C File Offset: 0x0005726C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasSameShape(int _blockId1, int _blockId2)
	{
		Block block = Block.list[_blockId1];
		Block block2 = Block.list[_blockId2];
		return !(block.shape.GetType() != block2.shape.GetType()) && (!(block.shape is BlockShapeNew) || block.Properties.GetValue("Model") == block2.Properties.GetValue("Model"));
	}

	// Token: 0x06000D6D RID: 3437 RVA: 0x000590DC File Offset: 0x000572DC
	public bool ConsumeScrollWheel(ItemInventoryData _data, float _scrollWheelInput, PlayerActionsLocal _playerInput)
	{
		if ((_playerInput.Reload.IsPressed || _playerInput.PermanentActions.Reload.IsPressed) && _data.IsBlock && Mathf.Abs(_scrollWheelInput) >= 0.001f)
		{
			_data.rotation = _data.itemValue.ToBlockValue(false).Block.BlockPlacementHelper.LimitRotation(_data.mode, ref _data.localRot, ((EntityPlayerLocal)_data.holdingEntity).HitInfo.hit, _scrollWheelInput > 0f, _data.itemValue.ToBlockValue(false), _data.rotation);
			return true;
		}
		return false;
	}

	// Token: 0x06000D6E RID: 3438 RVA: 0x00059184 File Offset: 0x00057384
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i createBlockMoveVector(Vector3 _relPlayerAxis)
	{
		Vector3i zero = Vector3i.zero;
		if (Math.Abs(_relPlayerAxis.x) > Math.Abs(_relPlayerAxis.z))
		{
			zero = new Vector3i(Mathf.Sign(_relPlayerAxis.x), 0f, 0f);
		}
		else
		{
			zero = new Vector3i(0f, 0f, Mathf.Sign(_relPlayerAxis.z));
		}
		return zero;
	}

	// Token: 0x06000D6F RID: 3439 RVA: 0x000591EC File Offset: 0x000573EC
	public bool ExecuteUseAction(ItemInventoryData _data, bool _bReleased, PlayerActionsLocal playerActions)
	{
		if (!_data.IsBlock)
		{
			return false;
		}
		bool flag = GameManager.Instance.IsEditMode() || GameStats.GetInt(EnumGameStats.GameModeId) == 2;
		if (flag && playerActions.Drop.IsPressed)
		{
			return false;
		}
		if (_bReleased)
		{
			return false;
		}
		if (Time.time - this.lastBuildTime < Constants.cBuildIntervall)
		{
			return true;
		}
		this.lastBuildTime = Time.time;
		EntityAlive holdingEntity = _data.holdingEntity;
		FastTags<TagGroup.Global> fastTags = FastTags<TagGroup.Global>.none;
		ItemClassBlock itemClassBlock = _data.item as ItemClassBlock;
		if (itemClassBlock != null)
		{
			fastTags = itemClassBlock.GetBlock().Tags;
		}
		if (EffectManager.GetValue(PassiveEffects.DisableItem, holdingEntity.inventory.holdingItemItemValue, 0f, holdingEntity, null, fastTags, true, true, true, true, true, 1, true, false) > 0f)
		{
			this.lastBuildTime = Time.time + 1f;
			Manager.PlayInsidePlayerHead("twitch_no_attack", -1, 0f, false, false);
			return false;
		}
		WorldRayHitInfo worldRayHitInfo = ((EntityPlayerLocal)_data.holdingEntity).HitInfo;
		HitInfoDetails hitInfoDetails = worldRayHitInfo.hit.Clone();
		if (!worldRayHitInfo.bHitValid)
		{
			return false;
		}
		hitInfoDetails.blockPos = ((flag && playerActions.Run.IsPressed) ? worldRayHitInfo.hit.blockPos : worldRayHitInfo.lastBlockPos);
		BlockValue blockValue = _data.itemValue.ToBlockValue(false);
		Block block = blockValue.Block;
		blockValue.damage = _data.damage;
		blockValue.rotation = _data.rotation;
		World world = GameManager.Instance.World;
		if (!GameManager.Instance.IsEditMode())
		{
			int placementDistanceSq = block.GetPlacementDistanceSq();
			if (hitInfoDetails.distanceSq > (float)placementDistanceSq)
			{
				return true;
			}
			Vector3i freePlacementPosition = block.GetFreePlacementPosition(world, hitInfoDetails.blockPos, blockValue, holdingEntity);
			if (!holdingEntity.IsGodMode.Value && GameUtils.IsColliderWithinBlock(freePlacementPosition, blockValue))
			{
				return true;
			}
			if (hitInfoDetails.blockPos == Vector3i.zero)
			{
				return true;
			}
		}
		_data.holdingEntity.RightArmAnimationUse = true;
		BlockPlacement.Result result = block.BlockPlacementHelper.OnPlaceBlock(_data.Placement, _data.mode, _data.localRot, GameManager.Instance.World, blockValue, _data.propTransform, hitInfoDetails, _data.holdingEntity.position);
		block.OnBlockPlaceBefore(_data.world, ref result, _data.holdingEntity, _data.world.GetGameRandom());
		blockValue = result.blockValue;
		block = blockValue.Block;
		if (blockValue.damage == 0)
		{
			blockValue.damage = block.StartDamage;
			result.blockValue.damage = block.StartDamage;
		}
		if (!playerActions.Run.IsPressed)
		{
			result.blockPos = block.GetFreePlacementPosition(_data.holdingEntity.world, result.blockPos, blockValue, _data.holdingEntity);
		}
		if (!block.CanPlaceBlockAt(_data.world, result.blockPos, blockValue, false))
		{
			_data.holdingEntity.PlayOneShot("keystone_build_warning", false, false, false, null, 1f);
			return true;
		}
		eSetBlockResponse eSetBlockResponse;
		if (!BlockLimitTracker.instance.CanAddBlock(blockValue, result.blockPos, out eSetBlockResponse))
		{
			if (eSetBlockResponse != eSetBlockResponse.PowerBlockLimitExceeded)
			{
				if (eSetBlockResponse == eSetBlockResponse.StorageBlockLimitExceeded)
				{
					GameManager.ShowTooltip(GameManager.Instance.World.GetPrimaryPlayer(), "uicannotaddstorageblock", false, false, 0f);
				}
			}
			else
			{
				GameManager.ShowTooltip(GameManager.Instance.World.GetPrimaryPlayer(), "uicannotaddpowerblock", false, false, 0f);
			}
			return true;
		}
		if (!GameManager.Instance.IsEditMode())
		{
			if (block.IndexName == "lpblock")
			{
				if (!_data.world.CanPlaceLandProtectionBlockAt(worldRayHitInfo.lastBlockPos, _data.world.gameManager.GetPersistentLocalPlayer()))
				{
					_data.holdingEntity.PlayOneShot("keystone_build_warning", false, false, false, null, 1f);
					return true;
				}
				_data.holdingEntity.PlayOneShot("keystone_placed", false, false, false, null, 1f);
			}
			else if (!_data.world.CanPlaceBlockAt(worldRayHitInfo.lastBlockPos, _data.world.gameManager.GetPersistentLocalPlayer(), false))
			{
				_data.holdingEntity.PlayOneShot("keystone_build_warning", false, false, false, null, 1f);
				return true;
			}
		}
		BiomeDefinition biome = _data.world.GetBiome(result.blockPos.x, result.blockPos.z);
		if (biome != null && biome.Replacements.ContainsKey(result.blockValue.type))
		{
			result.blockValue.type = biome.Replacements[result.blockValue.type];
		}
		BlockPlacement.EnumPlacement placement = result.placement;
		if (placement != BlockPlacement.EnumPlacement.Voxel)
		{
			if (placement != BlockPlacement.EnumPlacement.Free)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (!this.PlaceProp(_data, result, _data, block, blockValue))
			{
				return true;
			}
		}
		else if (!this.PlaceBlock(_data, result, _data, block, blockValue))
		{
			return true;
		}
		_data.holdingEntity.RightArmAnimationUse = true;
		_data.lastBuildTime = Time.time;
		GameManager.Instance.StartCoroutine(this.decInventoryLater(_data, _data.holdingEntity.inventory.holdingItemIdx));
		return true;
	}

	// Token: 0x06000D70 RID: 3440 RVA: 0x000596E0 File Offset: 0x000578E0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool PlaceBlock(ItemInventoryData _data, BlockPlacement.Result result, ItemInventoryData data, Block block, BlockValue blockValue)
	{
		this.addToUndo(result.blockPos, GameManager.Instance.World.GetBlock(result.blockPos));
		if (Block.list[data.itemValue.type].SelectAlternates)
		{
			if (data.itemValue.TextureFullArray.IsDefault)
			{
				block.PlaceBlock(data.world, result, data.holdingEntity);
			}
			else
			{
				BlockChangeInfo blockChangeInfo = new BlockChangeInfo(result.blockPos, blockValue, data.holdingEntity.entityId);
				blockChangeInfo.textureFull = data.itemValue.TextureFullArray;
				blockChangeInfo.bChangeTexture = true;
				GameManager.Instance.World.SetBlocksRPC(new List<BlockChangeInfo> { blockChangeInfo });
			}
		}
		else if (data.itemValue.TextureFullArray.IsDefault)
		{
			block.PlaceBlock(data.world, result, data.holdingEntity);
		}
		else
		{
			BlockChangeInfo blockChangeInfo2 = new BlockChangeInfo(result.blockPos, blockValue, data.holdingEntity.entityId);
			blockChangeInfo2.textureFull = data.itemValue.TextureFullArray;
			blockChangeInfo2.bChangeTexture = true;
			GameManager.Instance.World.SetBlocksRPC(new List<BlockChangeInfo> { blockChangeInfo2 });
		}
		QuestEventManager.Current.BlockPlaced(block.GetBlockName(), result.blockPos);
		data.holdingEntity.MinEventContext.ItemActionData = data.actionData[0];
		data.holdingEntity.MinEventContext.BlockValue = result.blockValue;
		data.holdingEntity.MinEventContext.Position = result.pos;
		data.holdingEntity.FireEvent(MinEventTypes.onSelfPlaceBlock, true);
		if (!block.shape.IsOmitTerrainSnappingUp && !block.IsTerrainDecoration)
		{
			data.world.ChunkCache.SnapTerrainToPositionAroundRPC(data.world, this.hitInfo.lastBlockPos - Vector3i.up);
		}
		return true;
	}

	// Token: 0x06000D71 RID: 3441 RVA: 0x000598D6 File Offset: 0x00057AD6
	[PublicizedFrom(EAccessModifier.Private)]
	public bool PlaceProp(ItemInventoryData _data, BlockPlacement.Result result, ItemInventoryData data, Block block, BlockValue blockValue)
	{
		block.PlaceProp(data.world, result, data.holdingEntity);
		return true;
	}

	// Token: 0x06000D72 RID: 3442 RVA: 0x000598ED File Offset: 0x00057AED
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator decInventoryLater(ItemInventoryData data, int index)
	{
		data.holdingEntity.Hand.WaitForSecondaryRelease = data.holdingEntity.inventory.holdingItemStack.count == 1;
		yield return new WaitForSeconds(0.1f);
		if (!GameManager.Instance.IsEditMode())
		{
			ItemStack itemStack = data.holdingEntity.inventory.GetItem(index).Clone();
			if (itemStack.count > 0)
			{
				itemStack.count--;
			}
			data.holdingEntity.inventory.SetItem(index, itemStack);
		}
		BlockValue blockValue = data.itemValue.ToBlockValue(false);
		string text = "placeblock";
		Block block = blockValue.Block;
		if (block.CustomPlaceSound != null)
		{
			text = block.CustomPlaceSound;
		}
		data.holdingEntity.PlayOneShot(text, false, false, false, null, 1f);
		yield break;
	}

	// Token: 0x06000D73 RID: 3443 RVA: 0x00059904 File Offset: 0x00057B04
	public bool ExecuteAttackAction(ItemInventoryData _data, bool _bReleased, PlayerActionsLocal playerActions)
	{
		if (!_bReleased)
		{
			return false;
		}
		bool flag = false;
		if (GameManager.Instance.IsEditMode() && playerActions.Drop.IsPressed)
		{
			return false;
		}
		if (!playerActions.SelectionSet.IsPressed && this.SelectionActive)
		{
			if (!playerActions.Drop.IsPressed)
			{
				flag = flag || this.SelectionActive;
				if (this.SelectionLockMode == 1)
				{
					Vector3i selectionSize = this.SelectionSize;
					this.SelectionStart = this.hitInfo.hit.blockPos;
					this.SelectionEnd = this.SelectionStart + selectionSize - Vector3i.one;
				}
				else if (this.SelectionLockMode == 0)
				{
					this.SelectionActive = false;
				}
			}
		}
		else if (GameManager.Instance.IsEditMode() && playerActions.Run.IsPressed && ((EntityPlayerLocal)_data.holdingEntity).HitInfo.bHitValid)
		{
			Vector3i vector3i = (playerActions.Run.IsPressed ? this.hitInfo.hit.blockPos : this.hitInfo.lastBlockPos);
			this.setBlock(vector3i, BlockValue.Air);
			flag = true;
		}
		else if (_data.IsBlock)
		{
			_data.itemValue.ToBlockValue(false).Block.RotateHoldingBlock(_data, true, true);
			flag = true;
		}
		return flag;
	}

	// Token: 0x06000D74 RID: 3444 RVA: 0x00059A5C File Offset: 0x00057C5C
	[PublicizedFrom(EAccessModifier.Private)]
	public void rotateSelectionAroundY()
	{
		Vector3i vector3i = new Vector3i(Mathf.Abs(this.m_selectionStartPoint.x - this.m_SelectionEndPoint.x), Mathf.Abs(this.m_selectionStartPoint.y - this.m_SelectionEndPoint.y), Mathf.Abs(this.m_selectionStartPoint.z - this.m_SelectionEndPoint.z));
		Vector3i vector3i2 = new Vector3i(Mathf.Min(this.m_selectionStartPoint.x, this.m_SelectionEndPoint.x), Mathf.Min(this.m_selectionStartPoint.y, this.m_SelectionEndPoint.y), Mathf.Min(this.m_selectionStartPoint.z, this.m_SelectionEndPoint.z));
		Prefab prefab = BlockTools.CopyIntoStorage(GameManager.Instance, vector3i2, vector3i2 + vector3i);
		this.BeginUndo();
		new Prefab(prefab.size)
		{
			bCopyAirBlocks = true
		}.CopyIntoRPC(GameManager.Instance, vector3i2, false);
		prefab.RotateY(false, 1);
		prefab.CopyIntoRPC(GameManager.Instance, vector3i2, this.copyPasteAirBlocks);
		this.SelectionStart = vector3i2;
		this.SelectionEnd = vector3i2 + prefab.size - Vector3i.one;
		this.EndUndo(false);
	}

	// Token: 0x170000FC RID: 252
	// (get) Token: 0x06000D75 RID: 3445 RVA: 0x00059B97 File Offset: 0x00057D97
	// (set) Token: 0x06000D76 RID: 3446 RVA: 0x00059BA4 File Offset: 0x00057DA4
	public bool SelectionActive
	{
		get
		{
			return this.SelectionBox.IsActive;
		}
		set
		{
			if (this.SelectionActive != value)
			{
				SelectionBoxManager.Instance.SetActive(this.SelectionBox, value);
			}
		}
	}

	// Token: 0x170000FD RID: 253
	// (get) Token: 0x06000D77 RID: 3447 RVA: 0x00059BC0 File Offset: 0x00057DC0
	// (set) Token: 0x06000D78 RID: 3448 RVA: 0x00059BC8 File Offset: 0x00057DC8
	public int SelectionLockMode
	{
		get
		{
			return this.m_iSelectionLockMode;
		}
		set
		{
			if (this.m_iSelectionLockMode != value)
			{
				this.m_iSelectionLockMode = value;
				this.SelectionBox.SetVisible(this.SelectionActive);
				Color color = BlockToolSelection.colInactive;
				if (this.m_iSelectionLockMode == 1)
				{
					color = new Color(0.5f, 0f, 1f, 0.5f);
					this.removeBlockPreview();
				}
				else if (this.m_iSelectionLockMode == 2)
				{
					color = BlockToolSelection.colActive;
				}
				else
				{
					this.removeBlockPreview();
				}
				this.SelectionBox.SetAllFacesColor(color, true);
			}
		}
	}

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x06000D79 RID: 3449 RVA: 0x00059C4C File Offset: 0x00057E4C
	public Vector3i SelectionMin
	{
		get
		{
			return new Vector3i(Utils.FastMin(this.SelectionStart.x, this.SelectionEnd.x), Utils.FastMin(this.SelectionStart.y, this.SelectionEnd.y), Utils.FastMin(this.SelectionStart.z, this.SelectionEnd.z));
		}
	}

	// Token: 0x170000FF RID: 255
	// (get) Token: 0x06000D7A RID: 3450 RVA: 0x00059CAF File Offset: 0x00057EAF
	// (set) Token: 0x06000D7B RID: 3451 RVA: 0x00059CB7 File Offset: 0x00057EB7
	public Vector3i SelectionStart
	{
		get
		{
			return this.m_selectionStartPoint;
		}
		set
		{
			if (!this.m_selectionStartPoint.Equals(value))
			{
				this.m_selectionStartPoint = value;
				this.updateSelection();
			}
		}
	}

	// Token: 0x17000100 RID: 256
	// (get) Token: 0x06000D7C RID: 3452 RVA: 0x00059CD4 File Offset: 0x00057ED4
	// (set) Token: 0x06000D7D RID: 3453 RVA: 0x00059CDC File Offset: 0x00057EDC
	public Vector3i SelectionEnd
	{
		get
		{
			return this.m_SelectionEndPoint;
		}
		set
		{
			if (!this.m_SelectionEndPoint.Equals(value))
			{
				this.m_SelectionEndPoint = value;
				this.updateSelection();
			}
		}
	}

	// Token: 0x17000101 RID: 257
	// (get) Token: 0x06000D7E RID: 3454 RVA: 0x00059CFC File Offset: 0x00057EFC
	public Vector3i SelectionSize
	{
		get
		{
			return new Vector3i(Mathf.Abs(this.m_selectionStartPoint.x - this.m_SelectionEndPoint.x) + 1, Mathf.Abs(this.m_selectionStartPoint.y - this.m_SelectionEndPoint.y) + 1, Mathf.Abs(this.m_selectionStartPoint.z - this.m_SelectionEndPoint.z) + 1);
		}
	}

	// Token: 0x06000D7F RID: 3455 RVA: 0x000027FC File Offset: 0x000009FC
	public void SelectionSizeSet(Vector3i _size)
	{
	}

	// Token: 0x06000D80 RID: 3456 RVA: 0x00059D68 File Offset: 0x00057F68
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateSelection()
	{
		Vector3 vector = this.SelectionSize.ToVector3();
		Vector3 vector2 = new Vector3((float)Mathf.Min(this.m_selectionStartPoint.x, this.m_SelectionEndPoint.x), (float)Mathf.Min(this.m_selectionStartPoint.y, this.m_SelectionEndPoint.y), (float)Mathf.Min(this.m_selectionStartPoint.z, this.m_SelectionEndPoint.z));
		this.SelectionBox.SetPositionAndSize(new Vector3i(vector2), new Vector3i(vector));
	}

	// Token: 0x06000D81 RID: 3457 RVA: 0x00059DF8 File Offset: 0x00057FF8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool setBlock(Vector3i _blockPos, BlockValue _blockValue)
	{
		BlockValue block = GameManager.Instance.World.GetBlock(_blockPos);
		if (block.rawData == _blockValue.rawData)
		{
			return false;
		}
		TextureFullArray textureFullArray = GameManager.Instance.World.GetTextureFullArray(_blockPos.x, _blockPos.y, _blockPos.z);
		this.undoQueue.Add(new List<BlockChangeInfo>
		{
			new BlockChangeInfo(_blockPos, block, MarchingCubes.DensityAir, textureFullArray)
		});
		if (this.undoQueue.Count > 100)
		{
			this.undoQueue.RemoveAt(0);
		}
		if (_blockValue.Block.shape.IsTerrain())
		{
			GameManager.Instance.World.SetBlockRPC(_blockPos, _blockValue, MarchingCubes.DensityTerrain);
		}
		else
		{
			GameManager.Instance.World.SetBlockRPC(_blockPos, _blockValue);
		}
		return true;
	}

	// Token: 0x06000D82 RID: 3458 RVA: 0x00059ED4 File Offset: 0x000580D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void addToUndo(Vector3i _blockPos, BlockValue _oldBlockValue)
	{
		TextureFullArray textureFullArray = GameManager.Instance.World.GetTextureFullArray(_blockPos.x, _blockPos.y, _blockPos.z);
		this.undoQueue.Add(new List<BlockChangeInfo>
		{
			new BlockChangeInfo(_blockPos, _oldBlockValue, MarchingCubes.DensityAir, textureFullArray)
		});
		if (this.undoQueue.Count > 100)
		{
			this.undoQueue.RemoveAt(0);
		}
	}

	// Token: 0x06000D83 RID: 3459 RVA: 0x00059F48 File Offset: 0x00058148
	[PublicizedFrom(EAccessModifier.Private)]
	public void blockUndoRedo(bool _redo)
	{
		List<List<BlockChangeInfo>> list = (_redo ? this.redoQueue : this.undoQueue);
		if (list.Count == 0)
		{
			return;
		}
		List<BlockChangeInfo> list2 = list[list.Count - 1];
		this.BeginUndo();
		GameManager.Instance.SetBlocksRPC(list2, null);
		list.RemoveAt(list.Count - 1);
		this.EndUndo(!_redo);
	}

	// Token: 0x06000D84 RID: 3460 RVA: 0x00059FAC File Offset: 0x000581AC
	public void BeginUndo()
	{
		this.undoChanges = new List<BlockChangeInfo>();
		ChunkCluster chunkCache = GameManager.Instance.World.ChunkCache;
		if (chunkCache != null)
		{
			chunkCache.OnBlockChangedDelegates += this.undoBlockChangeDelegate;
		}
	}

	// Token: 0x06000D85 RID: 3461 RVA: 0x00059FE9 File Offset: 0x000581E9
	[PublicizedFrom(EAccessModifier.Private)]
	public void undoBlockChangeDelegate(Vector3i pos, BlockValue bvOld, sbyte oldDens, TextureFullArray oldTex, BlockValue bvNew)
	{
		if (this.undoChanges != null && !bvOld.ischild)
		{
			this.undoChanges.Add(new BlockChangeInfo(pos, bvOld, oldDens, oldTex));
		}
	}

	// Token: 0x06000D86 RID: 3462 RVA: 0x0005A018 File Offset: 0x00058218
	public void EndUndo(bool _bRedo = false)
	{
		ChunkCluster chunkCache = GameManager.Instance.World.ChunkCache;
		if (chunkCache != null)
		{
			chunkCache.OnBlockChangedDelegates -= this.undoBlockChangeDelegate;
		}
		if (this.undoChanges.Count <= 0)
		{
			this.undoChanges = null;
			return;
		}
		this.undoChanges.Reverse();
		List<List<BlockChangeInfo>> list = (_bRedo ? this.redoQueue : this.undoQueue);
		list.Add(this.undoChanges);
		if (list.Count > 100)
		{
			list.RemoveAt(0);
		}
		this.undoChanges = null;
	}

	// Token: 0x06000D87 RID: 3463 RVA: 0x0005A0A1 File Offset: 0x000582A1
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockCopy(Prefab _storage)
	{
		return _storage.CopyFromWorldWithEntities(GameManager.Instance.World, this.SelectionStart, this.SelectionEnd, null);
	}

	// Token: 0x06000D88 RID: 3464 RVA: 0x0005A0C0 File Offset: 0x000582C0
	[PublicizedFrom(EAccessModifier.Private)]
	public void blockPaste(Vector3i _destPos, Prefab _storage)
	{
		this.BeginUndo();
		_storage.CopyIntoRPC(GameManager.Instance, _destPos, this.copyPasteAirBlocks);
		this.SelectionActive = true;
		this.SelectionStart = _destPos;
		this.SelectionEnd = _destPos + _storage.size - Vector3i.one;
		this.EndUndo(false);
	}

	// Token: 0x06000D89 RID: 3465 RVA: 0x0005A118 File Offset: 0x00058318
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i sizeFromPositions(Vector3i _posStart, Vector3i _posEnd)
	{
		Vector3i vector3i = new Vector3i(Math.Min(_posStart.x, _posEnd.x), Math.Min(_posStart.y, _posEnd.y), Math.Min(_posStart.z, _posEnd.z));
		Vector3i vector3i2 = new Vector3i(Math.Max(_posStart.x, _posEnd.x), Math.Max(_posStart.y, _posEnd.y), Math.Max(_posStart.z, _posEnd.z));
		return new Vector3i(Math.Abs(vector3i2.x - vector3i.x) + 1, Math.Abs(vector3i2.y - vector3i.y) + 1, Math.Abs(vector3i2.z - vector3i.z) + 1);
	}

	// Token: 0x06000D8A RID: 3466 RVA: 0x0005A1DC File Offset: 0x000583DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void union(Vector3i _pos1Start, Vector3i _pos1End, Vector3i _pos2Start, Vector3i _pos2End, out Vector3i _unionStart, out Vector3i _unionEnd)
	{
		_unionStart = new Vector3i(Utils.FastMin(_pos1Start.x, _pos1End.x, _pos2Start.x, _pos2End.x), Utils.FastMin(_pos1Start.y, _pos1End.y, _pos2Start.y, _pos2End.y), Utils.FastMin(_pos1Start.z, _pos1End.z, _pos2Start.z, _pos2End.z));
		_unionEnd = new Vector3i(Utils.FastMax(_pos1Start.x, _pos1End.x, _pos2Start.x, _pos2End.x), Utils.FastMax(_pos1Start.y, _pos1End.y, _pos2Start.y, _pos2End.y), Utils.FastMax(_pos1Start.z, _pos1End.z, _pos2Start.z, _pos2End.z));
	}

	// Token: 0x06000D8B RID: 3467 RVA: 0x0005A2B5 File Offset: 0x000584B5
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isPrefabActive()
	{
		return GameManager.Instance.GetDynamicPrefabDecorator() != null && GameManager.Instance.GetDynamicPrefabDecorator().ActivePrefab != null;
	}

	// Token: 0x06000D8C RID: 3468 RVA: 0x0005A2D7 File Offset: 0x000584D7
	public bool OnSelectionBoxActivated(SelectionBox _box, bool _bActivated)
	{
		this.SelectionBox.SetVisible(_bActivated);
		if (!_bActivated)
		{
			this.SelectionLockMode = 0;
		}
		return true;
	}

	// Token: 0x06000D8D RID: 3469 RVA: 0x0005A2F0 File Offset: 0x000584F0
	public void OnSelectionBoxMoved(SelectionBox _box, Vector3 _moveVector)
	{
		Vector3i vector3i = new Vector3i(_moveVector);
		Vector3i zero = Vector3i.zero;
		int selectionLockMode = this.SelectionLockMode;
		this.SelectionStart += vector3i;
		this.SelectionEnd += vector3i;
		this.selectionRotCenter += vector3i.ToVector3();
		if (this.SelectionLockMode == 2)
		{
			this.previewGORot1.transform.position += _moveVector;
		}
	}

	// Token: 0x06000D8E RID: 3470 RVA: 0x0005A378 File Offset: 0x00058578
	public void OnSelectionBoxSized(SelectionBox _box, int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest)
	{
		if (this.SelectionLockMode == 2)
		{
			this.SelectionLockMode = 0;
			return;
		}
		if (_dEast != 0 && (_dEast >= 0 || this.SelectionSize.x > 1))
		{
			if (this.SelectionEnd.x > this.SelectionStart.x)
			{
				this.SelectionEnd = new Vector3i(this.SelectionEnd.x + _dEast, this.SelectionEnd.y, this.SelectionEnd.z);
			}
			else
			{
				this.SelectionStart = new Vector3i(this.SelectionStart.x + _dEast, this.SelectionStart.y, this.SelectionStart.z);
			}
		}
		if (_dWest != 0 && (_dWest >= 0 || this.SelectionSize.x > 1))
		{
			if (this.SelectionEnd.x <= this.SelectionStart.x)
			{
				this.SelectionEnd = new Vector3i(this.SelectionEnd.x - _dWest, this.SelectionEnd.y, this.SelectionEnd.z);
			}
			else
			{
				this.SelectionStart = new Vector3i(this.SelectionStart.x - _dWest, this.SelectionStart.y, this.SelectionStart.z);
			}
		}
		if (_dTop != 0 && (_dTop >= 0 || this.SelectionSize.y > 1))
		{
			if (this.SelectionEnd.y > this.SelectionStart.y)
			{
				this.SelectionEnd = new Vector3i(this.SelectionEnd.x, this.SelectionEnd.y + _dTop, this.SelectionEnd.z);
			}
			else
			{
				this.SelectionStart = new Vector3i(this.SelectionStart.x, this.SelectionStart.y + _dTop, this.SelectionStart.z);
			}
		}
		if (_dBottom != 0 && (_dBottom >= 0 || this.SelectionSize.y > 1))
		{
			if (this.SelectionEnd.y <= this.SelectionStart.y)
			{
				this.SelectionEnd = new Vector3i(this.SelectionEnd.x, this.SelectionEnd.y - _dBottom, this.SelectionEnd.z);
			}
			else
			{
				this.SelectionStart = new Vector3i(this.SelectionStart.x, this.SelectionStart.y - _dBottom, this.SelectionStart.z);
			}
		}
		if (_dNorth != 0 && (_dNorth >= 0 || this.SelectionSize.z > 1))
		{
			if (this.SelectionEnd.z > this.SelectionStart.z)
			{
				this.SelectionEnd = new Vector3i(this.SelectionEnd.x, this.SelectionEnd.y, this.SelectionEnd.z + _dNorth);
			}
			else
			{
				this.SelectionStart = new Vector3i(this.SelectionStart.x, this.SelectionStart.y, this.SelectionStart.z + _dNorth);
			}
		}
		if (_dSouth != 0 && (_dSouth >= 0 || this.SelectionSize.z > 1))
		{
			if (this.SelectionEnd.z <= this.SelectionStart.z)
			{
				this.SelectionEnd = new Vector3i(this.SelectionEnd.x, this.SelectionEnd.y, this.SelectionEnd.z - _dSouth);
				return;
			}
			this.SelectionStart = new Vector3i(this.SelectionStart.x, this.SelectionStart.y, this.SelectionStart.z - _dSouth);
		}
	}

	// Token: 0x06000D8F RID: 3471 RVA: 0x0005A6FC File Offset: 0x000588FC
	public void OnSelectionBoxMirrored(Vector3i _selAxis)
	{
		EnumMirrorAlong enumMirrorAlong = EnumMirrorAlong.XAxis;
		if (_selAxis.y != 0)
		{
			enumMirrorAlong = EnumMirrorAlong.YAxis;
		}
		else if (_selAxis.z != 0)
		{
			enumMirrorAlong = EnumMirrorAlong.ZAxis;
		}
		if (this.previewGORot3 != null && this.previewGORot3.transform.childCount > 0)
		{
			this.clipboard.Mirror(enumMirrorAlong);
			this.removeBlockPreview();
			this.createBlockPreviewFrom(this.clipboard);
			return;
		}
		Prefab prefab = new Prefab();
		prefab.CopyFromWorldWithEntities(GameManager.Instance.World, this.SelectionStart, this.SelectionEnd, null);
		prefab.Mirror(enumMirrorAlong);
		prefab.CopyIntoRPC(GameManager.Instance, this.SelectionMin, this.copyPasteAirBlocks);
	}

	// Token: 0x06000D90 RID: 3472 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool OnSelectionBoxDelete(SelectionBox _box, bool _checkCanDeleteOnly)
	{
		return false;
	}

	// Token: 0x06000D91 RID: 3473 RVA: 0x0005A7A1 File Offset: 0x000589A1
	public bool OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities _criteria)
	{
		return _criteria == EnumSelectionBoxAvailabilities.CanResize || _criteria == EnumSelectionBoxAvailabilities.CanMirror;
	}

	// Token: 0x06000D92 RID: 3474 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxShowProperties(bool _bVisible, GUIWindowManager _windowManager)
	{
	}

	// Token: 0x06000D93 RID: 3475 RVA: 0x0005A7AD File Offset: 0x000589AD
	public void OnSelectionBoxRotated(SelectionBox _box)
	{
		if (this.SelectionLockMode == 2)
		{
			this.rotatePreviewAroundY();
			return;
		}
		this.rotateSelectionAroundY();
	}

	// Token: 0x06000D94 RID: 3476 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxUserDataChanged(SelectionBox _box)
	{
	}

	// Token: 0x06000D95 RID: 3477 RVA: 0x0005A7C8 File Offset: 0x000589C8
	public string GetDebugOutput()
	{
		if (this.SelectionActive)
		{
			return string.Format("Selection pos/size: {0}/{1}", this.SelectionStart.ToString(), this.SelectionSize.ToString());
		}
		return "-";
	}

	// Token: 0x06000D96 RID: 3478 RVA: 0x0005A815 File Offset: 0x00058A15
	public Dictionary<string, NGuiAction> GetActions()
	{
		return this.actions;
	}

	// Token: 0x06000D97 RID: 3479 RVA: 0x0005A820 File Offset: 0x00058A20
	public void LoadPrefabIntoClipboard(Prefab _prefab)
	{
		this.clipboard = _prefab;
		this.SelectionLockMode = 2;
		if (this.SelectionSize != this.clipboard.size)
		{
			this.SelectionEnd = this.SelectionStart + this.clipboard.size - Vector3i.one;
		}
		this.SelectionActive = true;
		this.createBlockPreviewFrom(this.clipboard);
	}

	// Token: 0x04000BB9 RID: 3001
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cBlockUndoRedoCount = 100;

	// Token: 0x04000BBA RID: 3002
	[PublicizedFrom(EAccessModifier.Private)]
	public static Color colActive = new Color(1f, 0f, 0f, 0.5f);

	// Token: 0x04000BBB RID: 3003
	[PublicizedFrom(EAccessModifier.Private)]
	public static Color colInactive = new Color(0f, 0f, 1f, 0.5f);

	// Token: 0x04000BBC RID: 3004
	public static BlockToolSelection Instance;

	// Token: 0x04000BBD RID: 3005
	public Prefab clipboard = new Prefab();

	// Token: 0x04000BBE RID: 3006
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i m_selectionStartPoint;

	// Token: 0x04000BBF RID: 3007
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i m_SelectionEndPoint;

	// Token: 0x04000BC0 RID: 3008
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_iSelectionLockMode;

	// Token: 0x04000BC1 RID: 3009
	[PublicizedFrom(EAccessModifier.Private)]
	public List<List<BlockChangeInfo>> undoQueue = new List<List<BlockChangeInfo>>();

	// Token: 0x04000BC2 RID: 3010
	[PublicizedFrom(EAccessModifier.Private)]
	public List<List<BlockChangeInfo>> redoQueue = new List<List<BlockChangeInfo>>();

	// Token: 0x04000BC3 RID: 3011
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastBuildTime;

	// Token: 0x04000BC4 RID: 3012
	[PublicizedFrom(EAccessModifier.Private)]
	public const string SelectionBoxName = "SingleInstance";

	// Token: 0x04000BC5 RID: 3013
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, NGuiAction> actions;

	// Token: 0x04000BC6 RID: 3014
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject previewGOParent;

	// Token: 0x04000BC7 RID: 3015
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject previewGORot1;

	// Token: 0x04000BC8 RID: 3016
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject previewGORot2;

	// Token: 0x04000BC9 RID: 3017
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject previewGORot3;

	// Token: 0x04000BCA RID: 3018
	[PublicizedFrom(EAccessModifier.Private)]
	public bool copyPasteAirBlocks = true;

	// Token: 0x04000BCB RID: 3019
	public PlayerActionsLocal playerInput;

	// Token: 0x04000BCC RID: 3020
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 selectionRotCenter;

	// Token: 0x04000BCD RID: 3021
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 offsetToMin;

	// Token: 0x04000BCE RID: 3022
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 offsetToMax;

	// Token: 0x04000BCF RID: 3023
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldRayHitInfo hitInfo;

	// Token: 0x04000BD0 RID: 3024
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bWaitForRelease;

	// Token: 0x04000BD1 RID: 3025
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BlockChangeInfo> undoChanges = new List<BlockChangeInfo>();
}
