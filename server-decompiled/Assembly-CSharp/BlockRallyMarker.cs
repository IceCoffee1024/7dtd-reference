using System;
using UnityEngine.Scripting;

// Token: 0x02000148 RID: 328
[Preserve]
public class BlockRallyMarker : Block
{
	// Token: 0x06000914 RID: 2324 RVA: 0x0003F59C File Offset: 0x0003D79C
	public BlockRallyMarker()
	{
		this.StabilityIgnore = true;
	}

	// Token: 0x06000915 RID: 2325 RVA: 0x0003F5DC File Offset: 0x0003D7DC
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

	// Token: 0x06000916 RID: 2326 RVA: 0x0003F688 File Offset: 0x0003D888
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

	// Token: 0x06000917 RID: 2327 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x0003F6E2 File Offset: 0x0003D8E2
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		this.cmds[0].enabled = true;
		return this.cmds;
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x0003F6FC File Offset: 0x0003D8FC
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

	// Token: 0x0600091A RID: 2330 RVA: 0x0003F74E File Offset: 0x0003D94E
	public override void OnBlockEntityTransformAfterActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
		base.OnBlockEntityTransformAfterActivated(_world, _blockPos, _blockValue, _ebcd);
		_world.IsEditor();
	}

	// Token: 0x040009A1 RID: 2465
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("activate", "electric_switch", false, false, null)
	};
}
