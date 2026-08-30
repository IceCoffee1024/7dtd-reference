using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000125 RID: 293
[Preserve]
public class BlockLauncher : BlockPowered
{
	// Token: 0x060007B5 RID: 1973 RVA: 0x00036A2C File Offset: 0x00034C2C
	public BlockLauncher()
	{
		this.HasTileEntity = true;
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x00036A84 File Offset: 0x00034C84
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

	// Token: 0x060007B7 RID: 1975 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x00036AFC File Offset: 0x00034CFC
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		bool flag = _world.CanPlaceBlockAt(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer(), false);
		bool flag2 = _world.IsMyLandProtectedBlock(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer());
		this.cmds[0].enabled = flag;
		this.cmds[1].enabled = flag2 && this.TakeDelay > 0f;
		return this.cmds;
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x00036B6C File Offset: 0x00034D6C
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

	// Token: 0x060007BA RID: 1978 RVA: 0x00036BFC File Offset: 0x00034DFC
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
			LockManager.Instance.LockRequestLocal(tileEntityPoweredRangedTrap, null, 0);
			return true;
		}
		if (!(_commandName == "take"))
		{
			return false;
		}
		base.takeItemWithTimer(_blockPos, _blockValue, _player, this.TakeDelay);
		return true;
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x00036C98 File Offset: 0x00034E98
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

	// Token: 0x060007BC RID: 1980 RVA: 0x00036D49 File Offset: 0x00034F49
	public override void OnBlockValueChanged(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
		base.OnBlockValueChanged(_world, _chunk, _blockPos, _oldBlockValue, _newBlockValue);
		this.updateState(_world, _blockPos, _newBlockValue);
	}

	// Token: 0x060007BD RID: 1981 RVA: 0x00036D64 File Offset: 0x00034F64
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

	// Token: 0x060007BE RID: 1982 RVA: 0x00036E14 File Offset: 0x00035014
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

	// Token: 0x060007BF RID: 1983 RVA: 0x00036E58 File Offset: 0x00035058
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

	// Token: 0x060007C0 RID: 1984 RVA: 0x00036EF4 File Offset: 0x000350F4
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

	// Token: 0x060007C1 RID: 1985 RVA: 0x00036F34 File Offset: 0x00035134
	public override TileEntityPowered CreateTileEntity(Chunk _chunk)
	{
		return new TileEntityPoweredRangedTrap(_chunk)
		{
			PowerItemType = PowerItem.PowerItemTypes.RangedTrap
		};
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x00036F44 File Offset: 0x00035144
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
		Transform transform = itemClass2.CloneModel((World)_world, itemValue, Vector3.zero, null, BlockShape.MeshPurpose.World, default(TextureFullArray));
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
		blockProjectileMoveScript.Fire(_blockPos.ToVector3() + new Vector3(0.5f, 0.5f, 0.5f), transform2.forward, null, 0, 0f, false);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			Manager.BroadcastPlay(_blockPos.ToVector3(), this.playSound, 0f);
		}
		return true;
	}

	// Token: 0x04000914 RID: 2324
	[PublicizedFrom(EAccessModifier.Private)]
	public string playSound;

	// Token: 0x04000915 RID: 2325
	public string AmmoItemName;

	// Token: 0x04000916 RID: 2326
	[PublicizedFrom(EAccessModifier.Private)]
	public new readonly BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("options", "tool", true, false, null),
		new BlockActivationCommand("take", "hand", false, false, null)
	};
}
