using System;
using UnityEngine.Scripting;

// Token: 0x0200015F RID: 351
[Preserve]
public class BlockTripWire : BlockPowered
{
	// Token: 0x060009AA RID: 2474 RVA: 0x00042844 File Offset: 0x00040A44
	public BlockTripWire()
	{
		this.HasTileEntity = true;
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x0003A8CC File Offset: 0x00038ACC
	public override void Init()
	{
		base.Init();
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x0004289C File Offset: 0x00040A9C
	public override TileEntityPowered CreateTileEntity(Chunk chunk)
	{
		return new TileEntityPoweredTrigger(chunk)
		{
			PowerItemType = PowerItem.PowerItemTypes.TripWireRelay,
			TriggerType = PowerTrigger.TriggerTypes.TripWire
		};
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x000428B4 File Offset: 0x00040AB4
	public override void OnBlockAdded(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue, PlatformUserIdentifierAbs _addedByPlayer)
	{
		base.OnBlockAdded(_world, _chunk, _blockPos, _blockValue, _addedByPlayer);
		if (!(_world.GetTileEntity(_blockPos) is TileEntityPoweredTrigger))
		{
			TileEntityPowered tileEntityPowered = this.CreateTileEntity(_chunk);
			tileEntityPowered.localChunkPos = World.toBlock(_blockPos);
			tileEntityPowered.InitializePowerData();
			_chunk.AddTileEntity(tileEntityPowered);
		}
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x00042900 File Offset: 0x00040B00
	public override string GetActivationText(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		TileEntityPoweredTrigger tileEntityPoweredTrigger = _world.GetTileEntity(_blockPos) as TileEntityPoweredTrigger;
		if (!tileEntityPoweredTrigger.ShowTriggerOptions || !_world.CanPlaceBlockAt(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer(), false))
		{
			return "";
		}
		PlayerActionsLocal playerInput = ((EntityPlayerLocal)_entityFocusing).playerInput;
		if (tileEntityPoweredTrigger != null && tileEntityPoweredTrigger.ShowTriggerOptions)
		{
			string text = playerInput.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null) + playerInput.PermanentActions.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null);
			string localizedBlockName = _blockValue.Block.GetLocalizedBlockName();
			return string.Format(Localization.Get("vendingMachineActivate", false, null), text, localizedBlockName);
		}
		return "";
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x000429A4 File Offset: 0x00040BA4
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

	// Token: 0x060009B0 RID: 2480 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x00042A40 File Offset: 0x00040C40
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		TileEntityPoweredTrigger tileEntityPoweredTrigger = _world.GetTileEntity(_blockPos) as TileEntityPoweredTrigger;
		bool flag = _world.CanPlaceBlockAt(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer(), false);
		bool flag2 = _world.IsMyLandProtectedBlock(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer());
		this.cmds[0].enabled = tileEntityPoweredTrigger.ShowTriggerOptions && flag;
		this.cmds[1].enabled = flag2 && this.TakeDelay > 0f;
		return this.cmds;
	}

	// Token: 0x040009E3 RID: 2531
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("options", "tool", true, false, null),
		new BlockActivationCommand("take", "hand", false, false, null)
	};
}
