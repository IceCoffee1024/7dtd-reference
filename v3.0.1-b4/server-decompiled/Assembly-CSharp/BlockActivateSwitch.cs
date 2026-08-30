using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200010F RID: 271
[Preserve]
public class BlockActivateSwitch : Block
{
	// Token: 0x17000079 RID: 121
	// (get) Token: 0x0600071E RID: 1822 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowBlockTriggers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000720 RID: 1824 RVA: 0x0003390D File Offset: 0x00031B0D
	public override void Init()
	{
		base.Init();
		base.Properties.ParseBool(BlockActivateSwitch.PropSingleUse, ref this.singleUse);
		base.Properties.ParseString(BlockActivateSwitch.PropActivateSound, ref this.activateSound);
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x0003342F File Offset: 0x0003162F
	public override void LateInit()
	{
		base.LateInit();
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x00033437 File Offset: 0x00031637
	public override void OnBlockValueChanged(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
		base.OnBlockValueChanged(_world, _chunk, _blockPos, _oldBlockValue, _newBlockValue);
		this.Refresh(_world, _chunk, _blockPos, _newBlockValue);
	}

	// Token: 0x06000723 RID: 1827 RVA: 0x00033451 File Offset: 0x00031651
	public override void OnBlockEntityTransformAfterActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
		base.OnBlockEntityTransformAfterActivated(_world, _blockPos, _blockValue, _ebcd);
		this.Refresh(_world, null, _blockPos, _blockValue);
	}

	// Token: 0x06000724 RID: 1828 RVA: 0x00033468 File Offset: 0x00031668
	public override void OnBlockRemoved(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		BlockUtilityNavIcon.RemoveNavObject(_blockPos);
		base.OnBlockRemoved(_world, _chunk, _blockPos, _blockValue);
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x0003347B File Offset: 0x0003167B
	public override void OnBlockUnloaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		BlockUtilityNavIcon.RemoveNavObject(_blockPos);
		base.OnBlockUnloaded(_world, _blockPos, _blockValue);
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x00033944 File Offset: 0x00031B44
	public override string GetActivationText(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		if (!_world.IsEditor())
		{
			if (this.singleUse && (_blockValue.meta & 2) != 0)
			{
				return "";
			}
			if ((_blockValue.meta & 1) == 0)
			{
				return "";
			}
		}
		PlayerActionsLocal playerInput = ((EntityPlayerLocal)_entityFocusing).playerInput;
		string text = playerInput.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null) + playerInput.PermanentActions.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null);
		string localizedBlockName = _blockValue.Block.GetLocalizedBlockName();
		return string.Format(Localization.Get("questBlockActivate", false, null), text, localizedBlockName);
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x000339D8 File Offset: 0x00031BD8
	public override void OnBlockAdded(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue, PlatformUserIdentifierAbs _addedByPlayer)
	{
		base.OnBlockAdded(_world, _chunk, _blockPos, _blockValue, _addedByPlayer);
		BlockTrigger blockTrigger = _chunk.GetBlockTrigger(World.toBlock(_blockPos));
		if (blockTrigger != null)
		{
			bool flag = blockTrigger.HasAnyTriggeredBy();
			_blockValue.meta = (byte)(((int)_blockValue.meta & -2) | (flag ? 0 : 1));
			_world.SetBlockRPC(_blockPos, _blockValue);
		}
		this.Refresh(_world, _chunk, _blockPos, _blockValue);
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x00033A3C File Offset: 0x00031C3C
	public override void OnTriggerAddedFromPrefab(BlockTrigger _trigger, Vector3i _blockPos, BlockValue _blockValue, FastTags<TagGroup.Global> _questTags)
	{
		if (GameManager.Instance.World.IsEditor())
		{
			return;
		}
		World world = GameManager.Instance.World;
		base.OnTriggerAddedFromPrefab(_trigger, _blockPos, _blockValue, _questTags);
		bool flag = _trigger.HasAnyTriggeredBy();
		_blockValue.meta = (byte)(((int)_blockValue.meta & -2) | (flag ? 0 : 1));
		world.SetBlock(_trigger.ToWorldPos(), _blockValue, true, false);
		this.Refresh(GameManager.Instance.World, _trigger.Chunk, _trigger.LocalChunkPos, _blockValue);
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x00033AC0 File Offset: 0x00031CC0
	public override bool OnBlockActivated(string _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		if (!_world.IsEditor())
		{
			if (this.singleUse && (_blockValue.meta & 2) != 0)
			{
				return false;
			}
			if ((_blockValue.meta & 1) == 0)
			{
				return false;
			}
			if (_player.prefab == null && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				return false;
			}
		}
		if (!(_commandName == "activate"))
		{
			if (_commandName == "trigger")
			{
				XUiC_TriggerProperties.Show(_player.PlayerUI.xui, _blockPos, true, true);
			}
		}
		else if (!_world.IsEditor())
		{
			bool flag = (_blockValue.meta & 2) > 0;
			if ((_blockValue.meta & 1) > 0 && (!flag || !this.singleUse))
			{
				base.HandleTrigger(_player, (World)_world, _blockPos, _blockValue);
				Manager.BroadcastPlay(_blockPos.ToVector3() + Vector3.one * 0.5f, this.activateSound, 0f);
				flag = !flag;
				_blockValue.meta = (byte)(((int)_blockValue.meta & -3) | (flag ? 2 : 0));
				_world.SetBlockRPC(_blockPos, _blockValue);
				this.Refresh(_world, null, _blockPos, _blockValue);
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x00033BEC File Offset: 0x00031DEC
	public override void OnTriggerRefresh(BlockTrigger _trigger, BlockValue _bv, FastTags<TagGroup.Global> questTag)
	{
		GameManager gm = GameManager.Instance;
		gm.StartCoroutine(gm.World.triggerManager.CheckPowerState(_trigger, questTag, delegate(bool powered)
		{
			_bv.meta = (byte)(((int)_bv.meta & -2) | (powered ? 1 : 0));
			gm.World.SetBlockRPC(_trigger.ToWorldPos(), _bv);
		}));
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x00033C4C File Offset: 0x00031E4C
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		((Chunk)_world.ChunkCache.GetChunkSync(World.toChunkXZ(_blockPos.x), _blockPos.y, World.toChunkXZ(_blockPos.z))).GetBlockTrigger(World.toBlock(_blockPos));
		this.cmds[0].enabled = !_world.IsEditor();
		this.cmds[1].enabled = _world.IsEditor() && !GameUtils.IsWorldEditor();
		return this.cmds;
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x00033CD8 File Offset: 0x00031ED8
	public override void Refresh(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.Refresh(_world, _chunk, _blockPos, _blockValue);
		bool flag = (_blockValue.meta & 2) > 0;
		bool flag2 = (_blockValue.meta & 1) > 0;
		BlockUtilityNavIcon.UpdateNavIcon(flag2 && !flag, _blockPos);
		IChunk chunk = _chunk;
		if (chunk == null)
		{
			ChunkCluster chunkCache = _world.ChunkCache;
			if (chunkCache == null)
			{
				return;
			}
			chunk = chunkCache.GetChunkSync(World.toChunkXZ(_blockPos.x), World.toChunkY(_blockPos.y), World.toChunkXZ(_blockPos.z));
			if (chunk == null)
			{
				return;
			}
		}
		if (chunk == null)
		{
			return;
		}
		BlockEntityData blockEntity = chunk.GetBlockEntity(_blockPos);
		if (blockEntity == null || !blockEntity.bHasTransform)
		{
			return;
		}
		BlockSwitchController component = blockEntity.transform.GetComponent<BlockSwitchController>();
		if (component)
		{
			component.SetState(flag2, flag);
		}
	}

	// Token: 0x0600072E RID: 1838 RVA: 0x00033D90 File Offset: 0x00031F90
	public override void OnTriggered(EntityPlayer _player, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, List<BlockChangeInfo> _blockChanges, BlockTrigger _triggeredBy)
	{
		base.OnTriggered(_player, _world, _blockPos, _blockValue, _blockChanges, _triggeredBy);
		_blockValue.meta = (byte)(((int)_blockValue.meta & -2) | 1);
		_blockChanges.Add(new BlockChangeInfo(_blockPos, _blockValue));
	}

	// Token: 0x0400086E RID: 2158
	public const int cMetaPowered = 1;

	// Token: 0x0400086F RID: 2159
	public const int cMetaOn = 2;

	// Token: 0x04000870 RID: 2160
	[PublicizedFrom(EAccessModifier.Private)]
	public bool singleUse;

	// Token: 0x04000871 RID: 2161
	[PublicizedFrom(EAccessModifier.Private)]
	public string activateSound;

	// Token: 0x04000872 RID: 2162
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("activate", "electric_switch", true, false, null),
		new BlockActivationCommand("trigger", "wrench", true, false, null)
	};

	// Token: 0x04000873 RID: 2163
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropSingleUse = "SingleUse";

	// Token: 0x04000874 RID: 2164
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropActivateSound = "ActivateSound";
}
