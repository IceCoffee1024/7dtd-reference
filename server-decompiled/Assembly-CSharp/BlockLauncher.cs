using System;
using AssetManagement;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000128 RID: 296
[Preserve]
public class BlockLauncher : BlockPowered
{
	// Token: 0x060007C5 RID: 1989 RVA: 0x00036E10 File Offset: 0x00035010
	public BlockLauncher()
	{
		this.HasTileEntity = true;
	}

	// Token: 0x060007C6 RID: 1990 RVA: 0x00036E68 File Offset: 0x00035068
	public override void Init()
	{
		base.Init();
		if (base.Properties.Values.ContainsKey("PlaySound"))
		{
			this.playSound = base.Properties.Values["PlaySound"];
		}
		if (base.Properties.Values.ContainsKey("AmmoItem"))
		{
			this.AmmoItemName = base.Properties.Values["AmmoItem"];
		}
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x00036EE0 File Offset: 0x000350E0
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		bool flag = _world.CanPlaceBlockAt(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer(), false);
		bool flag2 = _world.IsMyLandProtectedBlock(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer());
		this.cmds[0].enabled = flag;
		this.cmds[1].enabled = flag2 && this.TakeDelay > 0f;
		return this.cmds;
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x00036F50 File Offset: 0x00035150
	public override string GetActivationText(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		if (!_world.CanPlaceBlockAt(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer(), false))
		{
			return "";
		}
		bool flag = _world.GetTileEntity(_blockPos) is TileEntityPoweredRangedTrap;
		PlayerActionsLocal playerInput = ((EntityPlayerLocal)_entityFocusing).playerInput;
		if (!flag)
		{
			return "";
		}
		string text = playerInput.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null) + playerInput.PermanentActions.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null);
		string localizedBlockName = _blockValue.Block.GetLocalizedBlockName();
		return string.Format(Localization.Get("vendingMachineActivate", false, null), text, localizedBlockName);
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x00036FE0 File Offset: 0x000351E0
	public override bool OnBlockActivated(string _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		if (_blockValue.ischild)
		{
			Vector3i parentPos = _blockValue.Block.multiBlockPos.GetParentPos(_blockPos, _blockValue);
			BlockValue block = _world.GetBlock(parentPos);
			return this.OnBlockActivated(_commandName, _world, parentPos, block, _player);
		}
		TileEntityPoweredRangedTrap tileEntityPoweredRangedTrap = _world.GetTileEntity(_blockPos) as TileEntityPoweredRangedTrap;
		if (tileEntityPoweredRangedTrap == null)
		{
			return false;
		}
		if (_commandName == "options")
		{
			_player.AimingGun = false;
			LockManager.Instance.LockRequestLocal(tileEntityPoweredRangedTrap, 0);
			return true;
		}
		if (!(_commandName == "take"))
		{
			return false;
		}
		base.takeItemWithTimer(_blockPos, _blockValue, _player, this.TakeDelay);
		return true;
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x00037078 File Offset: 0x00035278
	[PublicizedFrom(EAccessModifier.Private)]
	public bool updateState(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
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
		bool flag = (_blockValue.meta & 2) > 0;
		TileEntityPoweredBlock tileEntityPoweredBlock = _world.GetTileEntity(_blockPos) as TileEntityPoweredBlock;
		if (tileEntityPoweredBlock != null)
		{
			PowerManager.Instance.SetTileEntityUpdate(tileEntityPoweredBlock, flag);
		}
		Transform transform = blockEntity.transform.Find("Activated");
		if (transform != null)
		{
			transform.gameObject.SetActive(flag);
		}
		return true;
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x00037129 File Offset: 0x00035329
	public override void OnBlockValueChanged(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
		base.OnBlockValueChanged(_world, _chunk, _blockPos, _oldBlockValue, _newBlockValue);
		this.updateState(_world, _blockPos, _newBlockValue);
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x00037144 File Offset: 0x00035344
	public override void OnBlockEntityTransformAfterActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
		base.OnBlockEntityTransformAfterActivated(_world, _blockPos, _blockValue, _ebcd);
		TileEntityPowered tileEntityPowered = _world.GetTileEntity(_blockPos) as TileEntityPowered;
		if (tileEntityPowered == null)
		{
			ChunkCluster chunkCache = _world.ChunkCache;
			if (chunkCache == null)
			{
				return;
			}
			Chunk chunk = (Chunk)chunkCache.GetChunkFromWorldPos(_blockPos);
			if (chunk == null)
			{
				return;
			}
			tileEntityPowered = this.CreateTileEntity(chunk);
			tileEntityPowered.localChunkPos = World.toBlock(_blockPos);
			tileEntityPowered.InitializePowerData();
			chunk.AddTileEntity(tileEntityPowered);
		}
		tileEntityPowered.BlockTransform = _ebcd.transform;
		tileEntityPowered.MarkWireDirty();
		if (tileEntityPowered.GetParent().y != -9999)
		{
			IPowered powered = _world.GetTileEntity(tileEntityPowered.GetParent()) as IPowered;
			if (powered != null)
			{
				powered.DrawWires();
			}
		}
		this.updateState(_world, _blockPos, _blockValue);
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x000371F4 File Offset: 0x000353F4
	public override bool ActivateBlock(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, bool _isOn, bool _isPowered)
	{
		byte b = (byte)(((int)_blockValue.meta & -3) | (_isOn ? 2 : 0));
		if (_blockValue.meta != b)
		{
			_blockValue.meta = b;
			_world.SetBlockRPC(_blockPos, _blockValue);
		}
		return true;
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x00037238 File Offset: 0x00035438
	public override void OnBlockAdded(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue, PlatformUserIdentifierAbs _addedByPlayer)
	{
		base.OnBlockAdded(_world, _chunk, _blockPos, _blockValue, _addedByPlayer);
		if (_blockValue.ischild)
		{
			return;
		}
		if (!(_world.GetTileEntity(_blockPos) is TileEntityPoweredRangedTrap))
		{
			TileEntityPoweredRangedTrap tileEntityPoweredRangedTrap = (TileEntityPoweredRangedTrap)this.CreateTileEntity(_chunk);
			tileEntityPoweredRangedTrap.SetDisableModifiedCheck(true);
			tileEntityPoweredRangedTrap.localChunkPos = World.toBlock(_blockPos);
			if (_addedByPlayer != null)
			{
				tileEntityPoweredRangedTrap.SetOwner(_addedByPlayer);
			}
			tileEntityPoweredRangedTrap.InitializePowerData();
			_chunk.AddTileEntity(tileEntityPoweredRangedTrap);
			tileEntityPoweredRangedTrap.SetDisableModifiedCheck(false);
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				tileEntityPoweredRangedTrap.SetModified();
			}
		}
		BlockEntityData blockEntityData = new BlockEntityData(_blockValue, _blockPos)
		{
			bNeedsTemperature = true
		};
		_chunk.AddEntityBlockStub(blockEntityData);
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x000372D4 File Offset: 0x000354D4
	public override void OnBlockLoaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockLoaded(_world, _blockPos, _blockValue);
		bool flag = (_blockValue.meta & 2) > 0;
		TileEntityPoweredBlock tileEntityPoweredBlock = _world.GetTileEntity(_blockPos) as TileEntityPoweredBlock;
		if (tileEntityPoweredBlock == null)
		{
			return;
		}
		PowerManager.Instance.SetTileEntityUpdate(tileEntityPoweredBlock, flag);
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x00037314 File Offset: 0x00035514
	public override TileEntityPowered CreateTileEntity(Chunk _chunk)
	{
		return new TileEntityPoweredRangedTrap(_chunk)
		{
			PowerItemType = PowerItem.PowerItemTypes.RangedTrap
		};
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x00037324 File Offset: 0x00035524
	public bool InstantiateProjectile(WorldBase _world, Vector3i _blockPos)
	{
		TileEntityPoweredRangedTrap tileEntityPoweredRangedTrap = _world.GetTileEntity(_blockPos) as TileEntityPoweredRangedTrap;
		if (tileEntityPoweredRangedTrap == null)
		{
			return false;
		}
		if (!tileEntityPoweredRangedTrap.IsLocked)
		{
			return false;
		}
		ItemClass itemClass = null;
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && !tileEntityPoweredRangedTrap.DecrementAmmo(out itemClass))
		{
			tileEntityPoweredRangedTrap.IsLocked = false;
			tileEntityPoweredRangedTrap.SetModified();
			return false;
		}
		ItemClass itemClass2 = itemClass ?? tileEntityPoweredRangedTrap.CurrentAmmoItem();
		if (itemClass2 == null)
		{
			return false;
		}
		ItemValue itemValue = new ItemValue(itemClass2.Id, 2, 2, false, null, 1f);
		InstanceHandle<GameObject> instanceHandle = itemClass2.CloneModel(itemValue, null, BlockShape.MeshPurpose.World);
		Transform transform = instanceHandle.Instance.transform;
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
		Transform transform2 = blockEntity.transform;
		if (transform2 != null)
		{
			transform.parent = transform2;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
		}
		else
		{
			transform.parent = null;
		}
		Utils.SetLayerRecursively(transform.gameObject, (transform2 != null) ? transform2.gameObject.layer : 0);
		BlockProjectileMoveScript blockProjectileMoveScript = transform.gameObject.AddComponent<BlockProjectileMoveScript>();
		blockProjectileMoveScript.itemProjectile = itemClass2;
		blockProjectileMoveScript.itemValueProjectile = itemValue;
		blockProjectileMoveScript.itemValueLauncher = ItemValue.None.Clone();
		blockProjectileMoveScript.itemActionProjectile = (ItemActionProjectile)((itemClass2.Actions[0] is ItemActionProjectile) ? itemClass2.Actions[0] : itemClass2.Actions[1]);
		blockProjectileMoveScript.ProjectileOwnerID = tileEntityPoweredRangedTrap.OwnerEntityID;
		blockProjectileMoveScript.assetHandle = instanceHandle;
		blockProjectileMoveScript.Fire(_blockPos.ToVector3() + new Vector3(0.5f, 0.5f, 0.5f), transform2.forward, null, 0, 0f, false);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			Manager.BroadcastPlay(_blockPos.ToVector3(), this.playSound, 0f);
		}
		return true;
	}

	// Token: 0x04000919 RID: 2329
	[PublicizedFrom(EAccessModifier.Private)]
	public string playSound;

	// Token: 0x0400091A RID: 2330
	public string AmmoItemName;

	// Token: 0x0400091B RID: 2331
	[PublicizedFrom(EAccessModifier.Private)]
	public new readonly BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("options", "tool", true, false, null),
		new BlockActivationCommand("take", "hand", false, false, null)
	};
}
