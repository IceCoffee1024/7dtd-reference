using System;
using UnityEngine.Scripting;

// Token: 0x02000147 RID: 327
[Preserve]
public class BlockRallyMarker : Block
{
	// Token: 0x0600090A RID: 2314 RVA: 0x0003F33C File Offset: 0x0003D53C
	public BlockRallyMarker()
	{
		this.StabilityIgnore = true;
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x0003F37C File Offset: 0x0003D57C
	public override string GetActivationText(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		PlayerActionsLocal playerInput = ((EntityPlayerLocal)_entityFocusing).playerInput;
		Quest quest = ((EntityPlayerLocal)_entityFocusing).QuestJournal.HasQuestAtRallyPosition(_blockPos.ToVector3(), true);
		if (quest != null && !quest.RallyMarkerActivated && ((EntityPlayerLocal)_entityFocusing).QuestJournal.ActiveQuest == null)
		{
			string text = playerInput.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null) + playerInput.PermanentActions.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null);
			_blockValue.Block.GetLocalizedBlockName();
			return string.Format(Localization.Get("questRallyActivate", false, null), text, quest.QuestClass.Name);
		}
		return string.Empty;
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x0003F428 File Offset: 0x0003D628
	public override bool OnBlockActivated(string _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		if (_commandName == "activate")
		{
			Quest quest = _player.QuestJournal.HasQuestAtRallyPosition(_blockPos.ToVector3(), true);
			if (quest != null && !quest.RallyMarkerActivated && _player.QuestJournal.ActiveQuest == null)
			{
				QuestEventManager.Current.HandleRallyMarkerActivate(_player, _blockPos, _blockValue);
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x0003F482 File Offset: 0x0003D682
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		this.cmds[0].enabled = true;
		return this.cmds;
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x0003F49C File Offset: 0x0003D69C
	public override void OnBlockLoaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockLoaded(_world, _blockPos, _blockValue);
		if (_blockValue.ischild)
		{
			return;
		}
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
		chunk.AddEntityBlockStub(new BlockEntityData(_blockValue, _blockPos)
		{
			bNeedsTemperature = true
		});
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x0003F4EE File Offset: 0x0003D6EE
	public override void OnBlockEntityTransformAfterActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
		base.OnBlockEntityTransformAfterActivated(_world, _blockPos, _blockValue, _ebcd);
		_world.IsEditor();
	}

	// Token: 0x0400099F RID: 2463
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("activate", "electric_switch", false, false, null)
	};
}
