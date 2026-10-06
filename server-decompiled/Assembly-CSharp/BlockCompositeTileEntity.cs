using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200016C RID: 364
[Preserve]
public class BlockCompositeTileEntity : Block
{
	// Token: 0x170000CC RID: 204
	// (get) Token: 0x06000A56 RID: 2646 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowBlockTriggers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x06000A57 RID: 2647 RVA: 0x00044DE8 File Offset: 0x00042FE8
	// (set) Token: 0x06000A58 RID: 2648 RVA: 0x00044DF0 File Offset: 0x00042FF0
	public TileEntityCompositeData CompositeData
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06000A59 RID: 2649 RVA: 0x00044DF9 File Offset: 0x00042FF9
	public BlockCompositeTileEntity()
	{
		this.HasTileEntity = true;
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x00044E08 File Offset: 0x00043008
	public override void Init()
	{
		base.Init();
		this.CompositeData = TileEntityCompositeData.ParseBlock(this);
		if (TEFeatureAbs.DebugLogCTE)
		{
			this.CompositeData.PrintConfig();
		}
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x00044E30 File Offset: 0x00043030
	public override void OnBlockAdded(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue, PlatformUserIdentifierAbs _addedByPlayer)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Out(string.Format("BlockComposite.OnBlockAdded (_, {0}, {1}, {2}) from {3}", new object[]
			{
				_chunk.ChunkPos,
				_blockPos,
				_blockValue,
				StackTraceUtility.ExtractStackTrace()
			}));
		}
		base.OnBlockAdded(_world, _chunk, _blockPos, _blockValue, _addedByPlayer);
		if (_blockValue.ischild)
		{
			return;
		}
		TileEntityComposite tileEntityComposite = _world.GetTileEntity(_blockPos) as TileEntityComposite;
		if (tileEntityComposite == null)
		{
			tileEntityComposite = new TileEntityComposite(_chunk, _blockValue)
			{
				localChunkPos = World.toBlock(_blockPos)
			};
		}
		tileEntityComposite.OnBlockAdded(_blockPos, _blockValue, _addedByPlayer);
		_chunk.AddTileEntity(tileEntityComposite);
	}

	// Token: 0x06000A5C RID: 2652 RVA: 0x00044ED0 File Offset: 0x000430D0
	public override void OnBlockRemoved(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Out(string.Format("BlockComposite.OnBlockRemoved (_, {0}, {1}, {2}) from {3}", new object[]
			{
				_chunk.ChunkPos,
				_blockPos,
				_blockValue,
				StackTraceUtility.ExtractStackTrace()
			}));
		}
		base.OnBlockRemoved(_world, _chunk, _blockPos, _blockValue);
		if (_blockValue.ischild)
		{
			return;
		}
		_chunk.RemoveTileEntityAt<TileEntityComposite>((World)_world, World.toBlock(_blockPos));
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x00044F4C File Offset: 0x0004314C
	public override void OnBlockLoaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockLoaded(_world, _blockPos, _blockValue);
		if (_blockValue.ischild)
		{
			return;
		}
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Out(string.Format("BlockComposite.OnBlockLoaded (_, _, {0}, {1}), HasTE: {2}, from {3}", new object[]
			{
				_blockPos,
				_blockValue,
				_world.GetTileEntity(_blockPos) is TileEntityComposite,
				StackTraceUtility.ExtractStackTrace()
			}));
		}
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x00044FB9 File Offset: 0x000431B9
	public override void OnBlockUnloaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Out(string.Format("BlockComposite.OnBlockUnloaded (_, _, {0}, {1}) from {2}", _blockPos, _blockValue, StackTraceUtility.ExtractStackTrace()));
		}
		base.OnBlockUnloaded(_world, _blockPos, _blockValue);
	}

	// Token: 0x06000A5F RID: 2655 RVA: 0x00044FEB File Offset: 0x000431EB
	public override BlockValue OnBlockPlaced(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, GameRandom _rnd)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Out(string.Format("BlockComposite.OnBlockPlaced (_, _, {0}, {1}, _) from {2}", _blockPos, _blockValue, StackTraceUtility.ExtractStackTrace()));
		}
		return base.OnBlockPlaced(_world, _blockPos, _blockValue, _rnd);
	}

	// Token: 0x06000A60 RID: 2656 RVA: 0x00045020 File Offset: 0x00043220
	public override void OnBlockValueChanged(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Out(string.Format("BlockComposite.OnBlockValueChanged (_, {0}, _, {1}, {2}, {3}) from {4}", new object[]
			{
				_chunk.ChunkPos,
				_blockPos,
				_oldBlockValue,
				_newBlockValue,
				StackTraceUtility.ExtractStackTrace()
			}));
		}
		TileEntityComposite tileEntityComposite;
		if (this.TryGetParentBlockAndTileEntity(ref _blockPos, ref _newBlockValue, out tileEntityComposite))
		{
			tileEntityComposite.OnBlockValueChanged(_blockPos, _oldBlockValue, _newBlockValue);
		}
		base.OnBlockValueChanged(_world, _chunk, _blockPos, _oldBlockValue, _newBlockValue);
	}

	// Token: 0x06000A61 RID: 2657 RVA: 0x000450A4 File Offset: 0x000432A4
	public override void OnBlockReset(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Out(string.Format("BlockComposite.OnBlockReset (_, {0}, {1}, {2}) from {3}", new object[]
			{
				_chunk.ChunkPos,
				_blockPos,
				_blockValue,
				StackTraceUtility.ExtractStackTrace()
			}));
		}
		TileEntityComposite tileEntityComposite;
		if (this.TryGetParentBlockAndTileEntity(ref _blockPos, ref _blockValue, out tileEntityComposite))
		{
			tileEntityComposite.OnBlockReset(_blockPos, _blockValue);
		}
		base.OnBlockReset(_world, _chunk, _blockPos, _blockValue);
	}

	// Token: 0x06000A62 RID: 2658 RVA: 0x00045118 File Offset: 0x00043318
	public override void OnBlockStartsToFall(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Out(string.Format("BlockComposite.OnBlockStartsToFall ({0}, {1}) from {2}", _blockPos, _blockValue, StackTraceUtility.ExtractStackTrace()));
		}
		TileEntityComposite tileEntityComposite;
		if (this.TryGetParentBlockAndTileEntity(ref _blockPos, ref _blockValue, out tileEntityComposite))
		{
			tileEntityComposite.OnBlockStartsToFall(_blockPos, _blockValue);
		}
		base.OnBlockStartsToFall(_world, _blockPos, _blockValue);
	}

	// Token: 0x06000A63 RID: 2659 RVA: 0x0004516C File Offset: 0x0004336C
	public override Block.DestroyedResult OnBlockDestroyedBy(WorldBase _world, BlockValueRef _bvRef, BlockValue _blockValue, int _entityId, bool _bUseHarvestTool)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Out(string.Format("BlockComposite.OnBlockDestroyedBy (_, _, {0}, {1}, {2}, {3}) from {4}", new object[]
			{
				_bvRef,
				_blockValue,
				_entityId,
				_bUseHarvestTool,
				StackTraceUtility.ExtractStackTrace()
			}));
		}
		Block.DestroyedResult destroyedResult = Block.DestroyedResult.None;
		TileEntityComposite tileEntityComposite;
		if (this.TryGetParentBlockAndTileEntity(ref _bvRef, ref _blockValue, out tileEntityComposite))
		{
			destroyedResult = tileEntityComposite.OnBlockDestroyedBy(_bvRef, _blockValue, _entityId, _bUseHarvestTool);
		}
		if (destroyedResult != Block.DestroyedResult.None)
		{
			return destroyedResult;
		}
		return Block.DestroyedResult.Downgrade;
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x000451EC File Offset: 0x000433EC
	public override Block.DestroyedResult OnBlockDestroyedByExplosion(WorldBase _world, BlockValueRef _bvRef, BlockValue _blockValue, int _playerThatStartedExpl)
	{
		Block.DestroyedResult destroyedResult = base.OnBlockDestroyedByExplosion(_world, _bvRef, _blockValue, _playerThatStartedExpl);
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Out(string.Format("BlockComposite.OnBlockDestroyedByExplosion (_, _, {0}, {1}, {2}) from {3}", new object[]
			{
				_bvRef,
				_blockValue,
				_playerThatStartedExpl,
				StackTraceUtility.ExtractStackTrace()
			}));
		}
		Block.DestroyedResult destroyedResult2 = Block.DestroyedResult.None;
		TileEntityComposite tileEntityComposite;
		if (this.TryGetParentBlockAndTileEntity(ref _bvRef, ref _blockValue, out tileEntityComposite))
		{
			destroyedResult2 = tileEntityComposite.OnBlockDestroyedByExplosion(_bvRef, _blockValue, _playerThatStartedExpl);
		}
		if (destroyedResult2 != Block.DestroyedResult.None)
		{
			return destroyedResult2;
		}
		return destroyedResult;
	}

	// Token: 0x06000A65 RID: 2661 RVA: 0x0004526C File Offset: 0x0004346C
	public override int OnBlockDamaged(WorldBase _world, BlockValueRef _bvRef, BlockValue _blockValue, int _damagePoints, int _entityIdThatDamaged, ItemActionAttack.AttackHitInfo _attackHitInfo, bool _bUseHarvestTool, bool _bBypassMaxDamage, int _recDepth = 0)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Out(string.Format("BlockComposite.OnBlockDamaged (_, _, {0}, {1}, {2}, {3}, _, {4}, {5}, {6}) from {7}", new object[]
			{
				_bvRef,
				_blockValue,
				_damagePoints,
				_entityIdThatDamaged,
				_bUseHarvestTool,
				_bBypassMaxDamage,
				_recDepth,
				StackTraceUtility.ExtractStackTrace()
			}));
		}
		return base.OnBlockDamaged(_world, _bvRef, _blockValue, _damagePoints, _entityIdThatDamaged, _attackHitInfo, _bUseHarvestTool, _bBypassMaxDamage, _recDepth);
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x000452F8 File Offset: 0x000434F8
	public override void OnBlockEntityTransformAfterActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _bed)
	{
		if (_bed == null)
		{
			return;
		}
		Chunk chunk = (Chunk)_world.GetChunkFromWorldPos(_blockPos);
		TileEntityComposite tileEntityComposite = _world.GetTileEntity(_blockPos) as TileEntityComposite;
		if (tileEntityComposite == null)
		{
			tileEntityComposite = new TileEntityComposite(chunk, _blockValue)
			{
				localChunkPos = World.toBlock(_blockPos)
			};
			chunk.AddTileEntity(tileEntityComposite);
		}
		tileEntityComposite.SetBlockEntityData(_bed);
		base.OnBlockEntityTransformAfterActivated(_world, _blockPos, _blockValue, _bed);
	}

	// Token: 0x06000A67 RID: 2663 RVA: 0x00045358 File Offset: 0x00043558
	public override void OnTriggered(EntityPlayer _player, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, List<BlockChangeInfo> _blockChanges, BlockTrigger _triggeredBy)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Out(string.Format("BlockComposite.OnBlockTriggered ({0}, {1}, {2}, {3}) from {4}", new object[]
			{
				_player,
				_blockPos,
				_blockValue,
				_blockChanges.Count,
				StackTraceUtility.ExtractStackTrace()
			}));
		}
		TileEntityComposite tileEntityComposite;
		if (this.TryGetParentBlockAndTileEntity(ref _blockPos, ref _blockValue, out tileEntityComposite))
		{
			tileEntityComposite.OnBlockTriggered(_player, _blockPos, _blockValue, _blockChanges, _triggeredBy);
		}
		base.OnTriggered(_player, _world, _blockPos, _blockValue, _blockChanges, _triggeredBy);
	}

	// Token: 0x06000A68 RID: 2664 RVA: 0x000453DC File Offset: 0x000435DC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryGetParentBlockAndTileEntity(ref BlockValueRef _bvRef, ref BlockValue _blockValue, out TileEntityComposite _te)
	{
		switch (_bvRef.Type)
		{
		case BlockValueRefType.None:
			_te = null;
			return false;
		case BlockValueRefType.Block:
		{
			Vector3i blockPosition = _bvRef.BlockPosition;
			bool flag = this.TryGetParentBlockAndTileEntity(ref blockPosition, ref _blockValue, out _te);
			_bvRef = new BlockValueRef(blockPosition);
			return flag;
		}
		case BlockValueRefType.Prop:
			_te = null;
			return false;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x00045434 File Offset: 0x00043634
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryGetParentBlockAndTileEntity(ref Vector3i _blockPos, ref BlockValue _blockValue, out TileEntityComposite _te)
	{
		_te = null;
		World world = GameManager.Instance.World;
		if (this.isMultiBlock && _blockValue.ischild)
		{
			if (world.ChunkCache == null)
			{
				throw new Exception("ChunkCluster null in " + StackTraceUtility.ExtractStackTrace());
			}
			Vector3i parentPos = this.multiBlockPos.GetParentPos(_blockPos, _blockValue);
			BlockValue block = world.GetBlock(parentPos);
			if (block.ischild)
			{
				Log.Error(string.Format("Block on position {0} with name '{1}' should be a parent but is not! (6)", parentPos, block.Block.GetBlockName()));
				return false;
			}
			_blockPos = parentPos;
			_blockValue = block;
		}
		_te = world.GetTileEntity(_blockPos) as TileEntityComposite;
		return _te != null;
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x000454F0 File Offset: 0x000436F0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryGetTileEntity(Vector3i _blockPos, BlockValue _blockValue, out TileEntityComposite _te)
	{
		_te = null;
		World world = GameManager.Instance.World;
		if (this.isMultiBlock && _blockValue.ischild)
		{
			if (world.ChunkCache == null)
			{
				throw new Exception("ChunkCluster null in " + StackTraceUtility.ExtractStackTrace());
			}
			_blockPos = this.multiBlockPos.GetParentPos(_blockPos, _blockValue);
			_blockValue = world.GetBlock(_blockPos);
			if (_blockValue.ischild)
			{
				Log.Error(string.Format("Block on position {0} with name '{1}' should be a parent but is not! (6)", _blockPos, _blockValue.Block.GetBlockName()));
				return false;
			}
		}
		_te = world.GetTileEntity(_blockPos) as TileEntityComposite;
		return _te != null;
	}

	// Token: 0x06000A6B RID: 2667 RVA: 0x00045590 File Offset: 0x00043790
	public override string GetActivationText(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Out(string.Format("BlockComposite.GetActivationText (_, {0}, _, {1}, {2}) from {3}", new object[]
			{
				_blockValue,
				_blockPos,
				_entityFocusing,
				StackTraceUtility.ExtractStackTrace()
			}));
		}
		TileEntityComposite tileEntityComposite;
		if (!this.TryGetParentBlockAndTileEntity(ref _blockPos, ref _blockValue, out tileEntityComposite))
		{
			return "";
		}
		if (this.commands == null)
		{
			this.commands = tileEntityComposite.InitBlockActivationCommands();
		}
		if (this.commands.Length == 0)
		{
			return null;
		}
		PlayerActionsLocal playerInput = ((EntityPlayerLocal)_entityFocusing).playerInput;
		string text = playerInput.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null) + playerInput.PermanentActions.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null);
		string localizedBlockName = _blockValue.Block.GetLocalizedBlockName();
		return tileEntityComposite.GetActivationText(_world, _blockPos, _blockValue, _entityFocusing, text, localizedBlockName);
	}

	// Token: 0x06000A6C RID: 2668 RVA: 0x0004565C File Offset: 0x0004385C
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Out(string.Format("BlockComposite.HasBlockActivationCommands (_, {0}, _, {1}, {2}) from {3}", new object[]
			{
				_blockValue,
				_blockPos,
				_entityFocusing,
				StackTraceUtility.ExtractStackTrace()
			}));
		}
		TileEntityComposite tileEntityComposite;
		if (!this.TryGetParentBlockAndTileEntity(ref _blockPos, ref _blockValue, out tileEntityComposite))
		{
			return false;
		}
		if (this.commands == null)
		{
			this.commands = tileEntityComposite.InitBlockActivationCommands();
		}
		return this.commands.Length != 0 && tileEntityComposite.UpdateBlockActivationCommands(this.commands, _world, _blockPos, _blockValue, _entityFocusing);
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x000456E4 File Offset: 0x000438E4
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Out(string.Format("BlockComposite.GetBlockActivationCommands (_, {0}, _, {1}, {2}) from {3}", new object[]
			{
				_blockValue,
				_blockPos,
				_entityFocusing,
				StackTraceUtility.ExtractStackTrace()
			}));
		}
		TileEntityComposite tileEntityComposite;
		if (!this.TryGetParentBlockAndTileEntity(ref _blockPos, ref _blockValue, out tileEntityComposite))
		{
			return BlockActivationCommand.Empty;
		}
		if (this.commands == null)
		{
			this.commands = tileEntityComposite.InitBlockActivationCommands();
		}
		if (this.commands.Length == 0)
		{
			return this.commands;
		}
		tileEntityComposite.UpdateBlockActivationCommands(this.commands, _world, _blockPos, _blockValue, _entityFocusing);
		return this.commands;
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x0004577C File Offset: 0x0004397C
	public override bool OnBlockActivated(string _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Out(string.Format("BlockComposite.OnBlockActivated ({0}, _, _, {1}, {2}, {3}) from {4}", new object[]
			{
				_commandName,
				_blockPos,
				_blockValue,
				_player,
				StackTraceUtility.ExtractStackTrace()
			}));
		}
		TileEntityComposite tileEntityComposite;
		if (!this.TryGetParentBlockAndTileEntity(ref _blockPos, ref _blockValue, out tileEntityComposite))
		{
			return false;
		}
		if (this.commands == null)
		{
			this.commands = tileEntityComposite.InitBlockActivationCommands();
		}
		return tileEntityComposite.OnBlockActivated(this.commands, _commandName, _world, _blockPos, _blockValue, _player);
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x00045800 File Offset: 0x00043A00
	public override bool IsMovementBlocked(IBlockAccess _world, Vector3i _blockPos, BlockValue _blockValue, BlockFace _face)
	{
		TileEntityComposite tileEntityComposite;
		if (this.TryGetTileEntity(_blockPos, _blockValue, out tileEntityComposite) && tileEntityComposite.OverridesPhysicalChecks)
		{
			using (IEnumerator<IFeaturePhysicalCapabilities> enumerator = tileEntityComposite.GetOverridesPhysicalChecksModules().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsMovementBlocked(_blockPos, _blockValue, _face))
					{
						return true;
					}
				}
			}
			return false;
		}
		return base.IsMovementBlocked(_world, _blockPos, _blockValue, _face);
	}

	// Token: 0x06000A70 RID: 2672 RVA: 0x00045878 File Offset: 0x00043A78
	public override bool IsMovementBlocked(IBlockAccess _world, Vector3i _blockPos, BlockValue _blockValue, BlockFaceFlag _sides)
	{
		TileEntityComposite tileEntityComposite;
		if (this.TryGetTileEntity(_blockPos, _blockValue, out tileEntityComposite) && tileEntityComposite.OverridesPhysicalChecks)
		{
			using (IEnumerator<IFeaturePhysicalCapabilities> enumerator = tileEntityComposite.GetOverridesPhysicalChecksModules().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsMovementBlocked(_blockPos, _blockValue, _sides))
					{
						return true;
					}
				}
			}
			return false;
		}
		return base.IsMovementBlocked(_world, _blockPos, _blockValue, _sides);
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x000458F0 File Offset: 0x00043AF0
	public override bool IsSeeThrough(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		TileEntityComposite tileEntityComposite;
		if (this.TryGetTileEntity(_blockPos, _blockValue, out tileEntityComposite) && tileEntityComposite.OverridesPhysicalChecks)
		{
			using (IEnumerator<IFeaturePhysicalCapabilities> enumerator = tileEntityComposite.GetOverridesPhysicalChecksModules().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsSeeThrough(_blockPos, _blockValue))
					{
						return false;
					}
				}
			}
			return true;
		}
		return base.IsSeeThrough(_world, _blockPos, _blockValue);
	}

	// Token: 0x06000A72 RID: 2674 RVA: 0x00045964 File Offset: 0x00043B64
	public override float GetStepHeight(IBlockAccess _world, Vector3i _blockPos, BlockValue _blockValue, BlockFace _crossingFace)
	{
		TileEntityComposite tileEntityComposite;
		if (this.TryGetTileEntity(_blockPos, _blockValue, out tileEntityComposite) && tileEntityComposite.OverridesPhysicalChecks)
		{
			float num = 0f;
			foreach (IFeaturePhysicalCapabilities featurePhysicalCapabilities in tileEntityComposite.GetOverridesPhysicalChecksModules())
			{
				num = Utils.FastMax(num, featurePhysicalCapabilities.GetStepHeight(_blockPos, _blockValue, _crossingFace));
			}
			return num;
		}
		return base.GetStepHeight(_world, _blockPos, _blockValue, _crossingFace);
	}

	// Token: 0x06000A73 RID: 2675 RVA: 0x000459E4 File Offset: 0x00043BE4
	public override bool IsTileEntitySavedInPrefab()
	{
		return this.CompositeData.HasFeature<IFeatureSavedInPrefab>();
	}

	// Token: 0x04000A25 RID: 2597
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockActivationCommand[] commands;
}
