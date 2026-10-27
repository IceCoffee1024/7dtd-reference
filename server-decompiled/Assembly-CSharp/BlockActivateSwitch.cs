using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000112 RID: 274
[Preserve]
public class BlockActivateSwitch : Block
{
	// Token: 0x17000079 RID: 121
	// (get) Token: 0x0600072E RID: 1838 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowBlockTriggers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000730 RID: 1840 RVA: 0x00033C81 File Offset: 0x00031E81
	public override void Init()
	{
		base.Init();
		base.Properties.ParseBool(BlockActivateSwitch.PropSingleUse, ref this.singleUse);
		base.Properties.ParseString(BlockActivateSwitch.PropActivateSound, ref this.activateSound);
	}

	// Token: 0x06000731 RID: 1841 RVA: 0x000337A3 File Offset: 0x000319A3
	public override void LateInit()
	{
		base.LateInit();
	}

	// Token: 0x06000732 RID: 1842 RVA: 0x000337AB File Offset: 0x000319AB
	public override void OnBlockValueChanged(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
		base.OnBlockValueChanged(_world, _chunk, _blockPos, _oldBlockValue, _newBlockValue);
		this.Refresh(_world, _chunk, _blockPos, _newBlockValue);
	}

	// Token: 0x06000733 RID: 1843 RVA: 0x000337C5 File Offset: 0x000319C5
	public override void OnBlockEntityTransformAfterActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
		base.OnBlockEntityTransformAfterActivated(_world, _blockPos, _blockValue, _ebcd);
		this.Refresh(_world, null, _blockPos, _blockValue);
	}

	// Token: 0x06000734 RID: 1844 RVA: 0x000337DC File Offset: 0x000319DC
	public override void OnBlockRemoved(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		BlockUtilityNavIcon.RemoveNavObject(_blockPos);
		base.OnBlockRemoved(_world, _chunk, _blockPos, _blockValue);
	}

	// Token: 0x06000735 RID: 1845 RVA: 0x000337EF File Offset: 0x000319EF
	public override void OnBlockUnloaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		BlockUtilityNavIcon.RemoveNavObject(_blockPos);
		base.OnBlockUnloaded(_world, _blockPos, _blockValue);
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x00033CB8 File Offset: 0x00031EB8
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

	// Token: 0x06000737 RID: 1847 RVA: 0x00033D4C File Offset: 0x00031F4C
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

	// Token: 0x06000738 RID: 1848 RVA: 0x00033DB0 File Offset: 0x00031FB0
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

	// Token: 0x06000739 RID: 1849 RVA: 0x00033E34 File Offset: 0x00032034
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

	// Token: 0x0600073A RID: 1850 RVA: 0x00033F60 File Offset: 0x00032160
	public override void OnTriggerRefresh(BlockTrigger _trigger, BlockValue _bv, FastTags<TagGroup.Global> questTag)
	{
		GameManager gm = GameManager.Instance;
		gm.StartCoroutine(gm.World.triggerManager.CheckPowerState(_trigger, questTag, delegate(bool powered)
		{
			_bv.meta = (byte)(((int)_bv.meta & -2) | (powered ? 1 : 0));
			gm.World.SetBlockRPC(_trigger.ToWorldPos(), _bv);
		}));
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x0600073C RID: 1852 RVA: 0x00033FC0 File Offset: 0x000321C0
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		((Chunk)_world.ChunkCache.GetChunkSync(World.toChunkXZ(_blockPos.x), _blockPos.y, World.toChunkXZ(_blockPos.z))).GetBlockTrigger(World.toBlock(_blockPos));
		this.cmds[0].enabled = !_world.IsEditor();
		this.cmds[1].enabled = _world.IsEditor() && !GameUtils.IsWorldEditor();
		return this.cmds;
	}

	// Token: 0x0600073D RID: 1853 RVA: 0x0003404C File Offset: 0x0003224C
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

	// Token: 0x0600073E RID: 1854 RVA: 0x00034104 File Offset: 0x00032304
	public override void OnTriggered(EntityPlayer _player, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, List<BlockChangeInfo> _blockChanges, BlockTrigger _triggeredBy)
	{
		base.OnTriggered(_player, _world, _blockPos, _blockValue, _blockChanges, _triggeredBy);
		_blockValue.meta = (byte)(((int)_blockValue.meta & -2) | 1);
		_blockChanges.Add(new BlockChangeInfo(_blockPos, _blockValue));
	}

	// Token: 0x04000873 RID: 2163
	public const int cMetaPowered = 1;

	// Token: 0x04000874 RID: 2164
	public const int cMetaOn = 2;

	// Token: 0x04000875 RID: 2165
	[PublicizedFrom(EAccessModifier.Private)]
	public bool singleUse;

	// Token: 0x04000876 RID: 2166
	[PublicizedFrom(EAccessModifier.Private)]
	public string activateSound;

	// Token: 0x04000877 RID: 2167
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("activate", "electric_switch", true, false, null),
		new BlockActivationCommand("trigger", "wrench", true, false, null)
	};

	// Token: 0x04000878 RID: 2168
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropSingleUse = "SingleUse";

	// Token: 0x04000879 RID: 2169
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropActivateSound = "ActivateSound";
}
