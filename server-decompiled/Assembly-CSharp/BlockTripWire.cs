using System;
using UnityEngine.Scripting;

// Token: 0x02000162 RID: 354
[Preserve]
public class BlockTripWire : BlockPowered
{
	// Token: 0x060009BA RID: 2490 RVA: 0x00042B80 File Offset: 0x00040D80
	public BlockTripWire()
	{
		this.HasTileEntity = true;
	}

	// Token: 0x060009BB RID: 2491 RVA: 0x0003AC1C File Offset: 0x00038E1C
	public override void Init()
	{
		base.Init();
	}

	// Token: 0x060009BC RID: 2492 RVA: 0x00042BD8 File Offset: 0x00040DD8
	public override TileEntityPowered CreateTileEntity(Chunk chunk)
	{
		return new TileEntityPoweredTrigger(chunk)
		{
			PowerItemType = PowerItem.PowerItemTypes.TripWireRelay,
			TriggerType = PowerTrigger.TriggerTypes.TripWire
		};
	}

	// Token: 0x060009BD RID: 2493 RVA: 0x00042BF0 File Offset: 0x00040DF0
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

	// Token: 0x060009BE RID: 2494 RVA: 0x00042C3C File Offset: 0x00040E3C
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

	// Token: 0x060009BF RID: 2495 RVA: 0x00042CE0 File Offset: 0x00040EE0
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
			LockManager.Instance.LockRequestLocal(tileEntityPoweredTrigger, 0);
			return true;
		}
		if (!(_commandName == "take"))
		{
			return false;
		}
		base.takeItemWithTimer(_blockPos, _blockValue, _player, this.TakeDelay);
		return true;
	}

	// Token: 0x060009C0 RID: 2496 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x060009C1 RID: 2497 RVA: 0x00042D78 File Offset: 0x00040F78
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		TileEntityPoweredTrigger tileEntityPoweredTrigger = _world.GetTileEntity(_blockPos) as TileEntityPoweredTrigger;
		bool flag = _world.CanPlaceBlockAt(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer(), false);
		bool flag2 = _world.IsMyLandProtectedBlock(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer());
		this.cmds[0].enabled = tileEntityPoweredTrigger.ShowTriggerOptions && flag;
		this.cmds[1].enabled = flag2 && this.TakeDelay > 0f;
		return this.cmds;
	}

	// Token: 0x040009E8 RID: 2536
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("options", "tool", true, false, null),
		new BlockActivationCommand("take", "hand", false, false, null)
	};
}
