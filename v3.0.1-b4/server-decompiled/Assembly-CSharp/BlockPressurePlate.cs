using System;
using UnityEngine.Scripting;

// Token: 0x02000144 RID: 324
[Preserve]
public class BlockPressurePlate : BlockPowered
{
	// Token: 0x060008E2 RID: 2274 RVA: 0x0003E6C8 File Offset: 0x0003C8C8
	public BlockPressurePlate()
	{
		this.HasTileEntity = true;
		this.IsCheckCollideWithEntity = true;
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x0003A8CC File Offset: 0x00038ACC
	public override void Init()
	{
		base.Init();
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x0003E728 File Offset: 0x0003C928
	[PublicizedFrom(EAccessModifier.Private)]
	public bool updateState(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, bool _bChangeState = false)
	{
		ChunkCluster chunkCache = _world.ChunkCache;
		if (chunkCache == null)
		{
			return false;
		}
		if (chunkCache.GetChunkSync(World.toChunkXZ(_blockPos.x), World.toChunkY(_blockPos.y), World.toChunkXZ(_blockPos.z)) == null)
		{
			return false;
		}
		byte meta = _blockValue.meta;
		bool flag = (_blockValue.meta & 2) > 0;
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			TileEntityPoweredTrigger tileEntityPoweredTrigger = _world.GetTileEntity(_blockPos) as TileEntityPoweredTrigger;
			if (tileEntityPoweredTrigger != null)
			{
				tileEntityPoweredTrigger.IsTriggered = flag;
			}
		}
		return true;
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x0003E7A8 File Offset: 0x0003C9A8
	public override bool OnEntityCollidedWithBlock(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, Entity _targetEntity)
	{
		if (!(_targetEntity is EntityAlive))
		{
			return false;
		}
		if (((EntityAlive)_targetEntity).IsDead())
		{
			return false;
		}
		if (this.isMultiBlock && _blockValue.ischild)
		{
			Vector3i parentPos = this.multiBlockPos.GetParentPos(_blockPos, _blockValue);
			_blockValue = _world.GetBlock(parentPos);
		}
		_blockValue.meta = (byte)(((int)_blockValue.meta & -3) | 2);
		_world.SetBlockRPC(_blockPos, _blockValue);
		return true;
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsMovementBlocked(IBlockAccess _world, Vector3i _blockPos, BlockValue blockDef, BlockFace face)
	{
		return false;
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x0003E81A File Offset: 0x0003CA1A
	public override void OnBlockValueChanged(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
		base.OnBlockValueChanged(_world, _chunk, _blockPos, _oldBlockValue, _newBlockValue);
		if (_newBlockValue.ischild)
		{
			return;
		}
		this.updateState(_world, _blockPos, _newBlockValue, false);
	}

	// Token: 0x060008E8 RID: 2280 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool ActivateBlock(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, bool isOn, bool isPowered)
	{
		return true;
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x0003AB83 File Offset: 0x00038D83
	public static bool IsSwitchOn(byte _metadata)
	{
		return (_metadata & 2) > 0;
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x0003E83F File Offset: 0x0003CA3F
	public override TileEntityPowered CreateTileEntity(Chunk chunk)
	{
		return new TileEntityPoweredTrigger(chunk)
		{
			TriggerType = PowerTrigger.TriggerTypes.PressurePlate
		};
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x0003E850 File Offset: 0x0003CA50
	public override void OnBlockAdded(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue, PlatformUserIdentifierAbs _addedByPlayer)
	{
		base.OnBlockAdded(_world, _chunk, _blockPos, _blockValue, _addedByPlayer);
		if (_blockValue.ischild)
		{
			return;
		}
		if (!(_world.GetTileEntity(_blockPos) is TileEntityPoweredTrigger))
		{
			TileEntityPowered tileEntityPowered = this.CreateTileEntity(_chunk);
			tileEntityPowered.localChunkPos = World.toBlock(_blockPos);
			tileEntityPowered.InitializePowerData();
			_chunk.AddTileEntity(tileEntityPowered);
		}
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x0003E8A4 File Offset: 0x0003CAA4
	public override string GetActivationText(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		if (!_world.CanPlaceBlockAt(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer(), false))
		{
			return "";
		}
		bool flag = _world.GetTileEntity(_blockPos) is TileEntityPoweredTrigger;
		PlayerActionsLocal playerInput = ((EntityPlayerLocal)_entityFocusing).playerInput;
		if (flag)
		{
			string text = playerInput.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null) + playerInput.PermanentActions.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null);
			string localizedBlockName = _blockValue.Block.GetLocalizedBlockName();
			return string.Format(Localization.Get("vendingMachineActivate", false, null), text, localizedBlockName);
		}
		return "";
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x0003E934 File Offset: 0x0003CB34
	public override bool OnBlockActivated(string _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		if (_blockValue.ischild)
		{
			Vector3i parentPos = _blockValue.Block.multiBlockPos.GetParentPos(_blockPos, _blockValue);
			BlockValue block = _world.GetBlock(parentPos);
			return this.OnBlockActivated(_commandName, _world, parentPos, block, _player);
		}
		TileEntityPoweredTrigger tileEntityPoweredTrigger = _world.GetTileEntity(_blockPos) as TileEntityPoweredTrigger;
		if (tileEntityPoweredTrigger == null)
		{
			return false;
		}
		if (_commandName == "options")
		{
			_player.AimingGun = false;
			LockManager.Instance.LockRequestLocal(tileEntityPoweredTrigger, null, 0);
			return true;
		}
		if (!(_commandName == "take"))
		{
			return false;
		}
		base.takeItemWithTimer(_blockPos, _blockValue, _player, this.TakeDelay);
		return true;
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x0003E9D0 File Offset: 0x0003CBD0
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		bool flag = _world.CanPlaceBlockAt(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer(), false);
		bool flag2 = _world.IsMyLandProtectedBlock(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer());
		this.cmds[0].enabled = flag;
		this.cmds[1].enabled = flag2 && this.TakeDelay > 0f;
		return this.cmds;
	}

	// Token: 0x04000993 RID: 2451
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("options", "tool", true, false, null),
		new BlockActivationCommand("take", "hand", false, false, null)
	};
}
