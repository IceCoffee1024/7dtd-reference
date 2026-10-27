using System;
using UnityEngine.Scripting;

// Token: 0x0200014A RID: 330
[Preserve]
public class BlockRallyMarker : Block
{
	// Token: 0x0600091A RID: 2330 RVA: 0x0003F640 File Offset: 0x0003D840
	public BlockRallyMarker()
	{
		this.StabilityIgnore = true;
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x0003F680 File Offset: 0x0003D880
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

	// Token: 0x0600091C RID: 2332 RVA: 0x0003F72C File Offset: 0x0003D92C
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

	// Token: 0x0600091D RID: 2333 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x0003F786 File Offset: 0x0003D986
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		this.cmds[0].enabled = true;
		return this.cmds;
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x0003F7A0 File Offset: 0x0003D9A0
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

	// Token: 0x06000920 RID: 2336 RVA: 0x0003F7F2 File Offset: 0x0003D9F2
	public override void OnBlockEntityTransformAfterActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
		base.OnBlockEntityTransformAfterActivated(_world, _blockPos, _blockValue, _ebcd);
		_world.IsEditor();
	}

	// Token: 0x040009A4 RID: 2468
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("activate", "electric_switch", false, false, null)
	};
}
