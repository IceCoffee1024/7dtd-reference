using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000148 RID: 328
[Preserve]
public class BlockQuestActivate : Block
{
	// Token: 0x1700009C RID: 156
	// (get) Token: 0x06000900 RID: 2304 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowBlockTriggers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000902 RID: 2306 RVA: 0x0003EDE0 File Offset: 0x0003CFE0
	public override string GetActivationText(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		if (!_world.IsEditor())
		{
			if (_blockValue.meta2 != 1)
			{
				return "";
			}
			if (!QuestEventManager.Current.ActiveQuestBlocks.Contains(_blockPos))
			{
				return "";
			}
		}
		PlayerActionsLocal playerInput = ((EntityPlayerLocal)_entityFocusing).playerInput;
		string text = playerInput.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null) + playerInput.PermanentActions.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null);
		string localizedBlockName = _blockValue.Block.GetLocalizedBlockName();
		return string.Format(Localization.Get("questBlockActivate", false, null), text, localizedBlockName);
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x0003EE74 File Offset: 0x0003D074
	public override bool OnBlockActivated(string _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		if (!_world.IsEditor() && _blockValue.meta2 != 1)
		{
			return true;
		}
		if (!(_commandName == "activate"))
		{
			if (!(_commandName == "trigger"))
			{
				return false;
			}
			XUiC_TriggerProperties.Show(_player.PlayerUI.xui, _blockPos, true, false);
			return true;
		}
		else
		{
			if (!QuestEventManager.Current.ActiveQuestBlocks.Contains(_blockPos))
			{
				return false;
			}
			if (this.activateTime > 0f)
			{
				LocalPlayerUI playerUI = _player.PlayerUI;
				TimerEventData timerEventData = new TimerEventData();
				timerEventData.Data = new object[] { _blockValue, _blockPos, _player };
				timerEventData.CloseOnHit = true;
				timerEventData.FullTimeFinishEvent += this.EventData_Event;
				if ((_blockValue.meta & 1) == 0)
				{
					timerEventData.AlternateTime = _player.rand.RandomRange(this.activateTime * 0.25f, this.activateTime * 0.5f);
					timerEventData.AlternateEvent += this.EventData_AlternateEvent;
				}
				timerEventData.CloseEvent += this.EventData_CloseEvent;
				XUiC_Timer.OpenTimer(playerUI.xui, this.activateTime, timerEventData, -1f, "", true);
				Manager.BroadcastPlay(_blockPos.ToVector3(), "generator_start", 0f);
				_blockValue.meta2 = 2;
				(_world as World).SetBlockRPC(_blockPos, _blockValue);
				this.setControllerState(_world, null, _blockPos, QuestGeneratorController.GeneratorStates.RebootState, false);
			}
			else
			{
				QuestEventManager.Current.BlockActivated(_blockValue.Block.GetBlockName(), _blockPos);
				base.HandleTrigger(_player, (World)_world, _blockPos, _blockValue);
				Manager.BroadcastPlay(_blockPos.ToVector3(), "generator_start", 0f);
				_blockValue.meta2 = 4;
				(_world as World).SetBlockRPC(_blockPos, _blockValue);
			}
			return true;
		}
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x0003F04C File Offset: 0x0003D24C
	[PublicizedFrom(EAccessModifier.Private)]
	public void EventData_AlternateEvent(TimerEventData timerData)
	{
		object[] array = (object[])timerData.Data;
		BlockValue blockValue = (BlockValue)array[0];
		Vector3i vector3i = (Vector3i)array[1];
		EntityPlayer entityPlayer = (EntityPlayer)array[2];
		WorldBase world = GameManager.Instance.World;
		GameEventManager.Current.HandleAction("quest_restorepower_generator", entityPlayer, entityPlayer, false, vector3i, "", "", false, true, "", null, null);
		blockValue.meta |= 1;
		blockValue.meta2 = 1;
		world.SetBlockRPC(vector3i, blockValue);
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x0003F0DC File Offset: 0x0003D2DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void EventData_CloseEvent(TimerEventData timerData)
	{
		object[] array = (object[])timerData.Data;
		BlockValue blockValue = (BlockValue)array[0];
		Vector3i vector3i = (Vector3i)array[1];
		World world = GameManager.Instance.World;
		blockValue.meta2 = 1;
		world.SetBlockRPC(vector3i, blockValue);
		this.setControllerState(world, null, vector3i, QuestGeneratorController.GeneratorStates.Off, false);
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x0003F130 File Offset: 0x0003D330
	[PublicizedFrom(EAccessModifier.Private)]
	public void EventData_Event(TimerEventData timerData)
	{
		object[] array = (object[])timerData.Data;
		BlockValue blockValue = (BlockValue)array[0];
		Vector3i vector3i = (Vector3i)array[1];
		EntityPlayerLocal entityPlayerLocal = array[2] as EntityPlayerLocal;
		World world = GameManager.Instance.World;
		QuestEventManager.Current.BlockActivated(blockValue.Block.GetBlockName(), vector3i);
		base.HandleTrigger(entityPlayerLocal, world, vector3i, blockValue);
		blockValue.meta2 = 4;
		world.SetBlockRPC(vector3i, blockValue);
		this.setControllerState(world, null, vector3i, QuestGeneratorController.GeneratorStates.On, false);
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x0003F1AE File Offset: 0x0003D3AE
	public override void OnBlockRemoved(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockRemoved(_world, _chunk, _blockPos, _blockValue);
		QuestEventManager.Current.BlockDestroyed(_blockValue.Block, _blockPos, null);
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x0003F1D4 File Offset: 0x0003D3D4
	public override void OnBlockValueChanged(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
		base.OnBlockValueChanged(_world, _chunk, _blockPos, _oldBlockValue, _newBlockValue);
		QuestGeneratorController.GeneratorStates meta = (QuestGeneratorController.GeneratorStates)_newBlockValue.meta2;
		if (meta == QuestGeneratorController.GeneratorStates.On)
		{
			QuestEventManager.Current.BlockActivated(_newBlockValue.Block.GetBlockName(), _blockPos);
		}
		this.setControllerState(_world, _chunk, _blockPos, meta, false);
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x0003F21C File Offset: 0x0003D41C
	public override void Init()
	{
		base.Init();
		base.Properties.ParseFloat(BlockQuestActivate.PropActivateTime, ref this.activateTime);
		if (base.Properties.Values.ContainsKey(BlockQuestActivate.PropQuestTags))
		{
			this.ValidQuestTags = FastTags<TagGroup.Global>.Parse(base.Properties.Values[BlockQuestActivate.PropQuestTags]);
		}
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x0003F27C File Offset: 0x0003D47C
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		this.cmds[0].enabled = !_world.IsEditor();
		this.cmds[1].enabled = _world.IsEditor() && !GameUtils.IsWorldEditor();
		return this.cmds;
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x0003F2D0 File Offset: 0x0003D4D0
	public override void OnTriggerAddedFromPrefab(BlockTrigger _trigger, Vector3i _blockPos, BlockValue _blockValue, FastTags<TagGroup.Global> _questTags)
	{
		if (GameManager.Instance.World.IsEditor())
		{
			return;
		}
		World world = GameManager.Instance.World;
		base.OnTriggerAddedFromPrefab(_trigger, _blockPos, _blockValue, _questTags);
		if (!this.ValidQuestTags.IsEmpty)
		{
			if (_questTags.IsEmpty)
			{
				_blockValue.meta2 = 0;
				world.SetBlock(_trigger.ToWorldPos(), _blockValue, true, false);
				this.setControllerState(world, _trigger.Chunk, _trigger.LocalChunkPos, QuestGeneratorController.GeneratorStates.OnNoQuest, false);
				return;
			}
			if (!_questTags.Test_AnySet(this.ValidQuestTags))
			{
				_blockValue.meta2 = 0;
				world.SetBlock(_trigger.ToWorldPos(), _blockValue, true, false);
				this.setControllerState(world, _trigger.Chunk, _trigger.LocalChunkPos, QuestGeneratorController.GeneratorStates.OnNoQuest, false);
			}
		}
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x0003F388 File Offset: 0x0003D588
	public override void OnBlockLoaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockLoaded(_world, _blockPos, _blockValue);
		ChunkCluster chunkCache = _world.ChunkCache;
		if (chunkCache == null)
		{
			return;
		}
		Chunk chunk = chunkCache.GetChunkSync(World.toChunkXZ(_blockPos.x), World.toChunkY(_blockPos.y), World.toChunkXZ(_blockPos.z)) as Chunk;
		if (chunk == null)
		{
			return;
		}
		BlockTrigger blockTrigger = chunk.GetBlockTrigger(World.toBlock(_blockPos));
		if (blockTrigger != null)
		{
			GameManager.Instance.StartCoroutine(this.resetTriggerLater(blockTrigger, _blockValue, FastTags<TagGroup.Global>.none));
		}
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x0003F404 File Offset: 0x0003D604
	public override void OnBlockEntityTransformAfterActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
		base.OnBlockEntityTransformAfterActivated(_world, _blockPos, _blockValue, _ebcd);
		ChunkCluster chunkCache = _world.ChunkCache;
		if (chunkCache == null)
		{
			return;
		}
		Chunk chunk = chunkCache.GetChunkSync(World.toChunkXZ(_blockPos.x), World.toChunkY(_blockPos.y), World.toChunkXZ(_blockPos.z)) as Chunk;
		if (chunk == null)
		{
			return;
		}
		QuestGeneratorController.GeneratorStates meta = (QuestGeneratorController.GeneratorStates)_blockValue.meta2;
		this.setControllerState(_world, chunk, _blockPos, meta, true);
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x0003F46B File Offset: 0x0003D66B
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator resetTriggerLater(BlockTrigger _trigger, BlockValue _blockValue, FastTags<TagGroup.Global> _questTag)
	{
		EntityPlayerLocal player = GameManager.Instance.World.GetPrimaryPlayer();
		if (player != null)
		{
			while (!player.QuestJournal.CheckRallyMarkerActivation())
			{
				yield return new WaitForSeconds(0.1f);
			}
		}
		if (!_questTag.Test_AnySet(QuestEventManager.restorePowerTag))
		{
			if (_trigger.NeedsTriggered == BlockTrigger.TriggeredStates.NotTriggered || _trigger.NeedsTriggered == BlockTrigger.TriggeredStates.NeedsTriggered)
			{
				_trigger.NeedsTriggered = BlockTrigger.TriggeredStates.NeedsTriggered;
				if (_trigger.TriggerDataOwner != null && !_trigger.TriggerDataOwner.NeedsTriggerUpdate)
				{
					_trigger.TriggerDataOwner.NeedsTriggerUpdate = true;
				}
			}
		}
		else
		{
			_trigger.NeedsTriggered = BlockTrigger.TriggeredStates.HasTriggered;
		}
		yield break;
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x0003F481 File Offset: 0x0003D681
	public override void OnTriggerRefresh(BlockTrigger _trigger, BlockValue _bv, FastTags<TagGroup.Global> _questTag)
	{
		base.OnTriggerRefresh(_trigger, _bv, _questTag);
		GameManager.Instance.StartCoroutine(this.resetTriggerLater(_trigger, _bv, _questTag));
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x0003F4A0 File Offset: 0x0003D6A0
	public void SetupForQuest(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue, List<BlockChangeInfo> blockChanges)
	{
		if (_world.IsEditor())
		{
			return;
		}
		if (_blockValue.ischild)
		{
			return;
		}
		_blockValue.meta2 = 1;
		blockChanges.Add(new BlockChangeInfo(_blockPos, _blockValue));
		this.setControllerState(_world, _chunk, _blockPos, QuestGeneratorController.GeneratorStates.Off, false);
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x0003F4DC File Offset: 0x0003D6DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void setControllerState(WorldBase _world, Chunk _chunk, Vector3i _blockPos, QuestGeneratorController.GeneratorStates generatorState, bool isInit = false)
	{
		if (_chunk == null)
		{
			ChunkCluster chunkCache = _world.ChunkCache;
			if (chunkCache == null)
			{
				return;
			}
			_chunk = chunkCache.GetChunkSync(World.toChunkXZ(_blockPos.x), World.toChunkY(_blockPos.y), World.toChunkXZ(_blockPos.z)) as Chunk;
			if (_chunk == null)
			{
				return;
			}
		}
		BlockEntityData blockEntity = _chunk.GetBlockEntity(_blockPos);
		if (blockEntity == null || !blockEntity.bHasTransform)
		{
			return;
		}
		QuestGeneratorController component = blockEntity.transform.GetComponent<QuestGeneratorController>();
		if (component != null)
		{
			component.SetGeneratorState(generatorState, isInit);
		}
	}

	// Token: 0x04000999 RID: 2457
	public const int cMetaSpawned = 1;

	// Token: 0x0400099A RID: 2458
	[PublicizedFrom(EAccessModifier.Private)]
	public float activateTime;

	// Token: 0x0400099B RID: 2459
	public FastTags<TagGroup.Global> ValidQuestTags = FastTags<TagGroup.Global>.none;

	// Token: 0x0400099C RID: 2460
	public static string PropActivateTime = "ActivateTime";

	// Token: 0x0400099D RID: 2461
	public static string PropQuestTags = "ValidQuestTags";

	// Token: 0x0400099E RID: 2462
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("activate", "electric_switch", false, false, null),
		new BlockActivationCommand("trigger", "wrench", true, false, null)
	};
}
