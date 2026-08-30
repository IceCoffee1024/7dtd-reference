using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000130 RID: 304
[Preserve]
public class BlockMotionSensor : BlockPowered
{
	// Token: 0x06000835 RID: 2101 RVA: 0x0003A874 File Offset: 0x00038A74
	public BlockMotionSensor()
	{
		this.HasTileEntity = true;
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x0003A8CC File Offset: 0x00038ACC
	public override void Init()
	{
		base.Init();
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x0003A8D4 File Offset: 0x00038AD4
	public override void OnBlockEntityTransformAfterActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
		base.OnBlockEntityTransformAfterActivated(_world, _blockPos, _blockValue, _ebcd);
		TileEntityPoweredTrigger tileEntityPoweredTrigger = _world.GetTileEntity(_blockPos) as TileEntityPoweredTrigger;
		MotionSensorController component = _ebcd.transform.gameObject.GetComponent<MotionSensorController>();
		if (component != null)
		{
			component.Init(base.Properties);
			component.TileEntity = tileEntityPoweredTrigger;
		}
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x0003A928 File Offset: 0x00038B28
	[PublicizedFrom(EAccessModifier.Private)]
	public bool updateState(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, bool _bChangeState = false)
	{
		ChunkCluster chunkCache = _world.ChunkCache;
		if (chunkCache == null)
		{
			return false;
		}
		IChunk chunkSync = chunkCache.GetChunkSync(World.toChunkXZ(_blockPos.x), World.toChunkY(_blockPos.y), World.toChunkXZ(_blockPos.z));
		if (chunkSync == null)
		{
			return false;
		}
		BlockEntityData blockEntity = chunkSync.GetBlockEntity(_blockPos);
		if (blockEntity == null || !blockEntity.bHasTransform)
		{
			return false;
		}
		bool flag = (_blockValue.meta & 1) > 0;
		bool flag2 = (_blockValue.meta & 2) > 0;
		if (_bChangeState)
		{
			flag2 = !flag2;
			_blockValue.meta = (byte)(((int)_blockValue.meta & -3) | (flag2 ? 2 : 0));
			_world.SetBlockRPC(_blockPos, _blockValue);
			if (flag2)
			{
				Manager.PlayInsidePlayerHead("switch_up", -1, 0f, false, false);
			}
			else
			{
				Manager.PlayInsidePlayerHead("switch_down", -1, 0f, false, false);
			}
		}
		TileEntityPoweredTrigger tileEntityPoweredTrigger = _world.GetTileEntity(_blockPos) as TileEntityPoweredTrigger;
		MotionSensorController component = blockEntity.transform.gameObject.GetComponent<MotionSensorController>();
		if (component != null)
		{
			component.Init(base.Properties);
			component.TileEntity = tileEntityPoweredTrigger;
			component.IsOn = flag;
		}
		BlockEntityData blockEntity2 = ((World)_world).ChunkCache.GetBlockEntity(_blockPos);
		if (blockEntity2 != null && blockEntity2.transform != null && blockEntity2.transform.gameObject != null)
		{
			Renderer[] componentsInChildren = blockEntity2.transform.gameObject.GetComponentsInChildren<Renderer>();
			if (componentsInChildren != null)
			{
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					if (componentsInChildren[i].material != componentsInChildren[i].sharedMaterial)
					{
						componentsInChildren[i].material = new Material(componentsInChildren[i].sharedMaterial);
					}
					if (flag)
					{
						componentsInChildren[i].material.SetColor("_EmissionColor", flag2 ? Color.green : Color.red);
					}
					else
					{
						componentsInChildren[i].material.SetColor("_EmissionColor", Color.black);
					}
					componentsInChildren[i].sharedMaterial = componentsInChildren[i].material;
				}
			}
		}
		return true;
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsMovementBlocked(IBlockAccess _world, Vector3i _blockPos, BlockValue blockDef, BlockFace face)
	{
		return false;
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x0003AB44 File Offset: 0x00038D44
	public override void OnBlockLoaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockLoaded(_world, _blockPos, _blockValue);
		this.updateState(_world, _blockPos, _blockValue, false);
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x0003AB5A File Offset: 0x00038D5A
	public override void OnBlockValueChanged(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
		base.OnBlockValueChanged(_world, _chunk, _blockPos, _oldBlockValue, _newBlockValue);
		this.updateState(_world, _blockPos, _newBlockValue, false);
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x0003AB75 File Offset: 0x00038D75
	public override bool ActivateBlock(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, bool isOn, bool isPowered)
	{
		this.updateState(_world, _blockPos, _blockValue, false);
		return true;
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x0003AB83 File Offset: 0x00038D83
	public static bool IsSwitchOn(byte _metadata)
	{
		return (_metadata & 2) > 0;
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x0003AB8B File Offset: 0x00038D8B
	public override TileEntityPowered CreateTileEntity(Chunk chunk)
	{
		return new TileEntityPoweredTrigger(chunk)
		{
			TriggerType = PowerTrigger.TriggerTypes.Motion
		};
	}

	// Token: 0x0600083F RID: 2111 RVA: 0x0003AB9C File Offset: 0x00038D9C
	public override void OnBlockAdded(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue, PlatformUserIdentifierAbs _addedByPlayer)
	{
		base.OnBlockAdded(_world, _chunk, _blockPos, _blockValue, _addedByPlayer);
		if (!(_world.GetTileEntity(_blockPos) is TileEntityPoweredTrigger))
		{
			TileEntityPoweredTrigger tileEntityPoweredTrigger = this.CreateTileEntity(_chunk) as TileEntityPoweredTrigger;
			tileEntityPoweredTrigger.SetDisableModifiedCheck(true);
			tileEntityPoweredTrigger.localChunkPos = World.toBlock(_blockPos);
			if (_addedByPlayer != null && tileEntityPoweredTrigger != null)
			{
				TileEntityPoweredTrigger tileEntityPoweredTrigger2 = tileEntityPoweredTrigger;
				tileEntityPoweredTrigger2.SetOwner(_addedByPlayer);
			}
			tileEntityPoweredTrigger.InitializePowerData();
			_chunk.AddTileEntity(tileEntityPoweredTrigger);
			tileEntityPoweredTrigger.SetDisableModifiedCheck(false);
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				tileEntityPoweredTrigger.SetModified();
			}
		}
	}

	// Token: 0x06000840 RID: 2112 RVA: 0x0003AC20 File Offset: 0x00038E20
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

	// Token: 0x06000841 RID: 2113 RVA: 0x0003ACB0 File Offset: 0x00038EB0
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

	// Token: 0x06000842 RID: 2114 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x06000843 RID: 2115 RVA: 0x0003AD4C File Offset: 0x00038F4C
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		bool flag = _world.CanPlaceBlockAt(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer(), false);
		bool flag2 = _world.IsMyLandProtectedBlock(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer());
		this.cmds[0].enabled = flag;
		this.cmds[1].enabled = flag2 && this.TakeDelay > 0f;
		return this.cmds;
	}

	// Token: 0x0400094B RID: 2379
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("options", "tool", true, false, null),
		new BlockActivationCommand("take", "hand", false, false, null)
	};
}
