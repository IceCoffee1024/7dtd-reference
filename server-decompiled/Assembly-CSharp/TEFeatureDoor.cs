using System;
using System.Collections.Generic;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200017B RID: 379
[Preserve]
public class TEFeatureDoor : TEFeatureAbs, IFeatureTriggerCapability, IFeaturePhysicalCapabilities, IFeatureSavedInPrefab
{
	// Token: 0x170000DE RID: 222
	// (get) Token: 0x06000B0C RID: 2828 RVA: 0x000470B6 File Offset: 0x000452B6
	public IFeatureTriggerCapability.ETriggerRole TriggerRole
	{
		get
		{
			return IFeatureTriggerCapability.ETriggerRole.Both;
		}
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x000470BC File Offset: 0x000452BC
	public override void Init(TileEntityComposite _parent, TileEntityFeatureData _featureData)
	{
		base.Init(_parent, _featureData);
		this.lockFeature = base.Parent.GetFeature<TEFeatureLockable>();
		this.lockpickFeature = base.Parent.GetFeature<TEFeatureLockPickable>();
		this.isDrawBridge = false;
		DynamicProperties props = _featureData.Props;
		if (base.Parent.TeData.CompositeProps.Values.ContainsKey(Block.PropMultiBlockDim))
		{
			Log.Error(string.Concat(new string[]
			{
				"Block with name ",
				base.Parent.TeData.Block.GetBlockName(),
				" requires a ",
				Block.PropMultiBlockDim,
				" property when including a Door feature"
			}));
		}
		props.ParseString(TEFeatureDoor.PropOpenSound, ref this.openSound);
		props.ParseString(TEFeatureDoor.PropCloseSound, ref this.closeSound);
		props.ParseString(TEFeatureDoor.PropLockedSound, ref this.lockedSound);
		props.ParseBool(TEFeatureDoor.PropIsDrawBridge, ref this.isDrawBridge);
		props.ParseFloat(TEFeatureDoor.PropAutoCloseTime, ref this.autoCloseTime);
		props.ParseEnum<TEFeatureDoor.HonkOpenTypes>(TEFeatureDoor.PropHonkOpenType, ref this.HonkOpenType);
		props.ParseFloat(TEFeatureDoor.PropHonkOpenDistance, ref this.HonkOpenDistance);
		if (props.Values.ContainsKey("TintableMaterials"))
		{
			foreach (string text in props.Values["TintableMaterials"].Split(',', StringSplitOptions.None))
			{
				this.tintableMaterials.Add(text + " (Instance)");
			}
		}
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x00047238 File Offset: 0x00045438
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyFromInternal(TileEntityComposite _other)
	{
		TEFeatureDoor tefeatureDoor;
		if (_other.TryGetSelfOrFeature<TEFeatureDoor>(out tefeatureDoor))
		{
			this.isOpen = tefeatureDoor.isOpen;
			this.openSound = tefeatureDoor.openSound;
			this.closeSound = tefeatureDoor.closeSound;
			this.lockedSound = tefeatureDoor.lockedSound;
			this.isDrawBridge = tefeatureDoor.isDrawBridge;
		}
	}

	// Token: 0x06000B0F RID: 2831 RVA: 0x0004728C File Offset: 0x0004548C
	public override void UpgradeDowngradeFrom(TileEntityComposite _other)
	{
		base.UpgradeDowngradeFrom(_other);
		TEFeatureDoor feature = _other.GetFeature<TEFeatureDoor>();
		if (feature != null)
		{
			this.isOpen = feature.isOpen;
			this.openSound = feature.openSound;
			this.closeSound = feature.closeSound;
			this.lockedSound = feature.lockedSound;
			this.isDrawBridge = feature.isDrawBridge;
		}
	}

	// Token: 0x06000B10 RID: 2832 RVA: 0x000472E8 File Offset: 0x000454E8
	public void HandleOpenCloseSound(Vector3i _blockPos)
	{
		if (!ThreadManager.IsMainThread())
		{
			return;
		}
		string text = (this.isOpen ? this.openSound : this.closeSound);
		Manager.BroadcastPlayByLocalPlayer(_blockPos.ToVector3() + Vector3.one * 0.5f, text);
	}

	// Token: 0x06000B11 RID: 2833 RVA: 0x00047338 File Offset: 0x00045538
	public void UpdateAnimState(BlockEntityData _ebcd)
	{
		if (_ebcd != null && _ebcd.bHasTransform)
		{
			Animator[] componentsInChildren = _ebcd.transform.GetComponentsInChildren<Animator>();
			if (componentsInChildren != null)
			{
				for (int i = componentsInChildren.Length - 1; i >= 0; i--)
				{
					Animator animator = componentsInChildren[i];
					animator.enabled = true;
					animator.SetBool(AnimatorDoorState.IsOpenHash, this.isOpen);
					animator.SetTrigger(AnimatorDoorState.OpenTriggerHash);
				}
			}
		}
	}

	// Token: 0x06000B12 RID: 2834 RVA: 0x00047394 File Offset: 0x00045594
	public void ForceAnimationState(BlockEntityData _ebcd)
	{
		if (_ebcd != null && _ebcd.bHasTransform)
		{
			Animator[] componentsInChildren = _ebcd.transform.GetComponentsInChildren<Animator>();
			if (componentsInChildren != null)
			{
				for (int i = componentsInChildren.Length - 1; i >= 0; i--)
				{
					Animator animator = componentsInChildren[i];
					animator.enabled = true;
					animator.keepAnimatorStateOnDisable = true;
					animator.SetBool(AnimatorDoorState.IsOpenHash, this.isOpen);
					animator.Play(this.isOpen ? AnimatorDoorState.OpenHash : AnimatorDoorState.CloseHash, 0, 1f);
				}
			}
		}
	}

	// Token: 0x06000B13 RID: 2835 RVA: 0x0004740C File Offset: 0x0004560C
	public bool IsOpen()
	{
		return this.isOpen;
	}

	// Token: 0x06000B14 RID: 2836 RVA: 0x00047414 File Offset: 0x00045614
	public void SetOpen(bool _open, bool _animate = false)
	{
		if (this.isOpen == _open)
		{
			return;
		}
		this.isOpen = _open;
		this.animateOnSync = _animate;
		if (this.isOpen && this.autoCloseTime > 0f)
		{
			this.autoCloseAtTickTime = GameTimer.Instance.ticks + (ulong)(this.autoCloseTime * 20f);
		}
		else
		{
			this.autoCloseAtTickTime = 0UL;
		}
		this.HandleDoorAnimation(_animate);
		if (_animate)
		{
			this.HandleOpenCloseSound(base.ToWorldPos());
		}
		GameManager.Instance.World.SetBlockRPC(base.ToWorldPos(), base.Parent.blockValue);
		base.SetModified();
	}

	// Token: 0x06000B15 RID: 2837 RVA: 0x000474B7 File Offset: 0x000456B7
	public bool CanOpen(out bool canPickToOpen)
	{
		canPickToOpen = false;
		if (this.IsOpen())
		{
			return true;
		}
		TEFeatureLockPickable tefeatureLockPickable = this.lockpickFeature;
		canPickToOpen = tefeatureLockPickable != null && tefeatureLockPickable.NeedsLockpicking();
		return this.lockFeature == null || !this.lockFeature.IsLocked();
	}

	// Token: 0x06000B16 RID: 2838 RVA: 0x000474F4 File Offset: 0x000456F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleDoorAnimation(bool _animate)
	{
		BlockEntityData blockEntity = GameManager.Instance.World.ChunkCache.GetBlockEntity(base.Parent.ToWorldPos());
		if (_animate)
		{
			this.UpdateAnimState(blockEntity);
			return;
		}
		this.ForceAnimationState(blockEntity);
	}

	// Token: 0x06000B17 RID: 2839 RVA: 0x00047534 File Offset: 0x00045734
	public override void SetBlockEntityData(BlockEntityData _blockEntityData)
	{
		if (_blockEntityData == null || !_blockEntityData.bHasTransform)
		{
			return;
		}
		Vector3 vector = base.ToWorldPos();
		Vector3i vector3i = new Vector3i(vector);
		World world = GameManager.Instance.World;
		bool flag = false;
		switch (this.HonkOpenType)
		{
		case TEFeatureDoor.HonkOpenTypes.Trader:
			flag = world.GetTraderAreaAt(vector3i) != null;
			break;
		case TEFeatureDoor.HonkOpenTypes.TraderOuter:
			flag = world.GetTraderAreaOuterAt(vector3i, 3) != null;
			break;
		case TEFeatureDoor.HonkOpenTypes.LandClaim:
			flag = world.CanPlaceBlockAt(vector3i, null, false);
			break;
		case TEFeatureDoor.HonkOpenTypes.Both:
			flag = world.CanPlaceBlockAt(vector3i, null, true);
			break;
		case TEFeatureDoor.HonkOpenTypes.All:
			flag = true;
			break;
		}
		if (flag)
		{
			GameObject gameObject = new GameObject("TraderDoor " + vector.ToString());
			Transform transform = gameObject.transform;
			gameObject.AddComponent<BoxCollider>().isTrigger = true;
			TraderDoorController.AddTraderDoorController(vector, gameObject).OwnerDoor = this;
			transform.SetParent(_blockEntityData.transform);
			float num = this.HonkOpenDistance * 2f;
			transform.localPosition = new Vector3(0.5f, 0.01f, 0.5f);
			transform.localScale = new Vector3(10f, 6f, num);
			transform.localRotation = Quaternion.identity;
		}
		this.ForceAnimationState(_blockEntityData);
	}

	// Token: 0x06000B18 RID: 2840 RVA: 0x0004766C File Offset: 0x0004586C
	public override void UpdateTick(World world)
	{
		base.UpdateTick(world);
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer || !this.isOpen || this.autoCloseTime <= 0f)
		{
			return;
		}
		if (this.autoCloseAtTickTime == 0UL || GameTimer.Instance.ticks < this.autoCloseAtTickTime)
		{
			return;
		}
		this.SetOpen(false, true);
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x000476C8 File Offset: 0x000458C8
	public override void OnBlockValueChanged(Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
		if (_newBlockValue.Block.shape is BlockShapeModelEntity && _oldBlockValue.type != _newBlockValue.type && !_newBlockValue.ischild)
		{
			if (VehicleManager.Instance != null)
			{
				VehicleManager.Instance.PhysicsWakeNear(_blockPos.ToVector3());
			}
			BlockEntityData blockEntity = GameManager.Instance.World.ChunkCache.GetBlockEntity(_blockPos);
			this.ForceAnimationState(blockEntity);
		}
	}

	// Token: 0x06000B1A RID: 2842 RVA: 0x00047736 File Offset: 0x00045936
	public override void OnBlockReset(Vector3i _blockPos, BlockValue _blockValue)
	{
		this.SetOpen(false, false);
	}

	// Token: 0x06000B1B RID: 2843 RVA: 0x00047740 File Offset: 0x00045940
	public void OnBlockTriggered(EntityPlayer _player, Vector3i _blockPos, BlockValue _blockValue, List<BlockChangeInfo> _blockChanges, BlockTrigger _triggeredBy)
	{
		if (this.lockFeature != null && _triggeredBy != null && _triggeredBy.Unlock)
		{
			this.lockFeature.SetLocked(false);
		}
		this.SetOpen(!this.isOpen, true);
	}

	// Token: 0x06000B1C RID: 2844 RVA: 0x00047774 File Offset: 0x00045974
	public override void OnUnload(World _world)
	{
		base.OnUnload(_world);
		Vector3 vector = base.ToWorldPos();
		TraderDoorController traderDoorController = TraderDoorController.GetTraderDoorController(vector);
		if (traderDoorController != null)
		{
			TraderDoorController.RemoveTraderDoorController(vector);
			global::UnityEngine.Object.Destroy(traderDoorController.gameObject);
		}
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x000477B8 File Offset: 0x000459B8
	public bool IsMovementBlocked(Vector3i _blockPos, BlockValue _blockValue, BlockFace _face)
	{
		if (!this.isDrawBridge)
		{
			return !this.isOpen;
		}
		Block block = _blockValue.Block;
		if (!block.isMultiBlock || !_blockValue.ischild)
		{
			return true;
		}
		if (!this.isOpen)
		{
			return true;
		}
		Vector3i parentPos = block.multiBlockPos.GetParentPos(_blockPos, _blockValue);
		return _blockPos.y == parentPos.y;
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x00047818 File Offset: 0x00045A18
	public bool IsMovementBlocked(Vector3i _blockPos, BlockValue _blockValue, BlockFaceFlag _sides)
	{
		if (!this.isDrawBridge)
		{
			return !this.isOpen;
		}
		Block block = _blockValue.Block;
		if (!block.isMultiBlock || !_blockValue.ischild)
		{
			return true;
		}
		if (!this.isOpen)
		{
			return true;
		}
		Vector3i parentPos = block.multiBlockPos.GetParentPos(_blockPos, _blockValue);
		return _blockPos.y == parentPos.y;
	}

	// Token: 0x06000B1F RID: 2847 RVA: 0x0004740C File Offset: 0x0004560C
	public bool IsSeeThrough(Vector3i _blockPos, BlockValue _blockValue)
	{
		return this.isOpen;
	}

	// Token: 0x06000B20 RID: 2848 RVA: 0x00047878 File Offset: 0x00045A78
	public float GetStepHeight(Vector3i _blockPos, BlockValue _blockValue, BlockFace crossingFace)
	{
		if (!this.isDrawBridge)
		{
			return 0f;
		}
		if (!this.isOpen)
		{
			return 1f;
		}
		Block block = _blockValue.Block;
		if (block.isMultiBlock && _blockValue.ischild)
		{
			Vector3i parentPos = block.multiBlockPos.GetParentPos(_blockPos, _blockValue);
			if (_blockPos.y == parentPos.y)
			{
				return 1f;
			}
		}
		return 0f;
	}

	// Token: 0x06000B21 RID: 2849 RVA: 0x000478E4 File Offset: 0x00045AE4
	public override string GetActivationText(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing, string _activateHotkeyMarkup, string _focusedTileEntityName)
	{
		base.GetActivationText(_world, _blockPos, _blockValue, _entityFocusing, _activateHotkeyMarkup, _focusedTileEntityName);
		string text = Localization.Get("door", false, null);
		if (this.lockpickFeature != null && this.lockpickFeature.NeedsLockpicking())
		{
			return string.Format(Localization.Get("tooltipLocked", false, null), _activateHotkeyMarkup, text);
		}
		if (this.lockFeature != null && this.lockFeature.IsLocked())
		{
			return string.Format(Localization.Get("tooltipLocked", false, null), _activateHotkeyMarkup, text);
		}
		return string.Format(Localization.Get("tooltipUnlocked", false, null), _activateHotkeyMarkup, text);
	}

	// Token: 0x06000B22 RID: 2850 RVA: 0x00047978 File Offset: 0x00045B78
	public override void InitBlockActivationCommands(Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> _addCallback)
	{
		base.InitBlockActivationCommands(_addCallback);
		_addCallback(new BlockActivationCommand("open", "door", false, false, null), TileEntityComposite.EBlockCommandOrder.Normal, base.FeatureData);
		_addCallback(new BlockActivationCommand("close", "door", false, false, null), TileEntityComposite.EBlockCommandOrder.Normal, base.FeatureData);
	}

	// Token: 0x06000B23 RID: 2851 RVA: 0x000479CC File Offset: 0x00045BCC
	public override bool AllowBlockActivationCommand(ITileEntityFeature _module, ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing)
	{
		if (!base.AllowBlockActivationCommand(_module, _commandName, _world, _blockPos, _blockValue, _entityFocusing))
		{
			return false;
		}
		if (!this.Equals(_module))
		{
			return true;
		}
		if (base.CommandIs(_commandName, "open"))
		{
			return !this.isOpen;
		}
		return !base.CommandIs(_commandName, "close") || this.isOpen;
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x00047A25 File Offset: 0x00045C25
	public override void OnAdded(Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnAdded(_blockPos, _blockValue);
		if (this.isDrawBridge)
		{
			this.isOpen = true;
			base.SetModified();
		}
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x00047A44 File Offset: 0x00045C44
	public override bool OnBlockActivated(ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		base.OnBlockActivated(_commandName, _world, _blockPos, _blockValue, _player);
		if (!base.CommandIs(_commandName, "close") && !base.CommandIs(_commandName, "open"))
		{
			return false;
		}
		bool flag = false;
		if (this.lockFeature != null && this.lockFeature.IsLocked() && !this.lockFeature.IsUserAllowed(PlatformManager.InternalLocalUserIdentifier))
		{
			flag = true;
		}
		if (flag)
		{
			Manager.BroadcastPlayByLocalPlayer(_blockPos.ToVector3() + Vector3.one * 0.5f, this.lockedSound);
			TraderArea traderAreaAt = ((World)_world).GetTraderAreaAt(_blockPos);
			if (traderAreaAt != null)
			{
				EntityTrader trader = traderAreaAt.GetTrader();
				GameManager.ShowTooltip(_player, trader.GetNextTimeMessage(), string.Empty, null, null, true, false, 0f);
			}
			return false;
		}
		this.SetOpen(!this.isOpen, true);
		_blockValue.Block.HandleTrigger(_player, (World)_world, _blockPos, _blockValue);
		return true;
	}

	// Token: 0x06000B26 RID: 2854 RVA: 0x00047B30 File Offset: 0x00045D30
	public override void Read(PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		base.Read(_br, _eStreamMode);
		if (_eStreamMode == StreamModeRead.Persistency)
		{
			if (base.Parent.UseLocalVersioning())
			{
				_br.ReadUInt16();
			}
			else
			{
				base.Parent.GetLegacyForkVersion();
			}
		}
		bool flag = this.isOpen;
		this.isOpen = _br.ReadBoolean();
		if (_eStreamMode == StreamModeRead.Persistency && this.autoCloseTime > 0f)
		{
			this.isOpen = false;
		}
		if (_eStreamMode != StreamModeRead.Persistency)
		{
			bool flag2 = _br.ReadBoolean();
			if (flag != this.isOpen)
			{
				this.HandleDoorAnimation(flag2);
			}
		}
	}

	// Token: 0x06000B27 RID: 2855 RVA: 0x00047BAE File Offset: 0x00045DAE
	public override void Write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		base.Write(_bw, _eStreamMode);
		if (_eStreamMode == StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		_bw.Write(this.isOpen);
		if (_eStreamMode != StreamModeWrite.Persistency)
		{
			_bw.Write(this.animateOnSync);
			this.animateOnSync = false;
		}
	}

	// Token: 0x04000A60 RID: 2656
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 18;

	// Token: 0x04000A61 RID: 2657
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropOpenSound = "OpenSound";

	// Token: 0x04000A62 RID: 2658
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropCloseSound = "CloseSound";

	// Token: 0x04000A63 RID: 2659
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropLockedSound = "LockedSound";

	// Token: 0x04000A64 RID: 2660
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropIsDrawBridge = "IsDrawBridge";

	// Token: 0x04000A65 RID: 2661
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropAutoCloseTime = "AutoCloseTime";

	// Token: 0x04000A66 RID: 2662
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropHonkOpenType = "HonkOpenType";

	// Token: 0x04000A67 RID: 2663
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropHonkOpenDistance = "HonkOpenDistance";

	// Token: 0x04000A68 RID: 2664
	[PublicizedFrom(EAccessModifier.Protected)]
	public string openSound;

	// Token: 0x04000A69 RID: 2665
	[PublicizedFrom(EAccessModifier.Protected)]
	public string closeSound;

	// Token: 0x04000A6A RID: 2666
	[PublicizedFrom(EAccessModifier.Protected)]
	public string lockedSound = "Misc/locked";

	// Token: 0x04000A6B RID: 2667
	[PublicizedFrom(EAccessModifier.Protected)]
	public HashSet<string> tintableMaterials = new HashSet<string>();

	// Token: 0x04000A6C RID: 2668
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureLockable lockFeature;

	// Token: 0x04000A6D RID: 2669
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureLockPickable lockpickFeature;

	// Token: 0x04000A6E RID: 2670
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOpen;

	// Token: 0x04000A6F RID: 2671
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDrawBridge;

	// Token: 0x04000A70 RID: 2672
	[PublicizedFrom(EAccessModifier.Private)]
	public bool animateOnSync;

	// Token: 0x04000A71 RID: 2673
	[PublicizedFrom(EAccessModifier.Private)]
	public float autoCloseTime;

	// Token: 0x04000A72 RID: 2674
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong autoCloseAtTickTime;

	// Token: 0x04000A73 RID: 2675
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureDoor.HonkOpenTypes HonkOpenType;

	// Token: 0x04000A74 RID: 2676
	[PublicizedFrom(EAccessModifier.Private)]
	public float HonkOpenDistance = 13f;

	// Token: 0x0200017C RID: 380
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum HonkOpenTypes
	{
		// Token: 0x04000A76 RID: 2678
		None,
		// Token: 0x04000A77 RID: 2679
		Trader,
		// Token: 0x04000A78 RID: 2680
		TraderOuter,
		// Token: 0x04000A79 RID: 2681
		LandClaim,
		// Token: 0x04000A7A RID: 2682
		Both,
		// Token: 0x04000A7B RID: 2683
		All
	}
}
