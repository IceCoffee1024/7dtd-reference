using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000161 RID: 353
[Preserve]
public class BlockTriggerDowngrade : Block
{
	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x060009B0 RID: 2480 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowBlockTriggers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x000428F4 File Offset: 0x00040AF4
	public override void LateInit()
	{
		base.LateInit();
		if (!this.DowngradeBlock.isair)
		{
			BlockHazard blockHazard = this.DowngradeBlock.Block as BlockHazard;
			if (blockHazard != null)
			{
				this.DowngradeBlock = blockHazard.SetHazardState(this.DowngradeBlock, true);
			}
		}
	}

	// Token: 0x060009B3 RID: 2483 RVA: 0x0003355E File Offset: 0x0003175E
	public override void OnBlockValueChanged(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
		base.OnBlockValueChanged(_world, _chunk, _blockPos, _oldBlockValue, _newBlockValue);
	}

	// Token: 0x060009B4 RID: 2484 RVA: 0x0004293C File Offset: 0x00040B3C
	public override string GetActivationText(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		if (!_world.IsEditor())
		{
			return "";
		}
		PlayerActionsLocal playerInput = ((EntityPlayerLocal)_entityFocusing).playerInput;
		string text = playerInput.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null) + playerInput.PermanentActions.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null);
		string localizedBlockName = _blockValue.Block.GetLocalizedBlockName();
		return string.Format(Localization.Get("questBlockActivate", false, null), text, localizedBlockName);
	}

	// Token: 0x060009B5 RID: 2485 RVA: 0x000429AB File Offset: 0x00040BAB
	public override bool OnBlockActivated(string _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		if (_commandName == "trigger")
		{
			XUiC_TriggerProperties.Show(_player.PlayerUI.xui, _blockPos, false, true);
		}
		return false;
	}

	// Token: 0x060009B6 RID: 2486 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x000429D0 File Offset: 0x00040BD0
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		((Chunk)_world.ChunkCache.GetChunkSync(World.toChunkXZ(_blockPos.x), _blockPos.y, World.toChunkXZ(_blockPos.z))).GetBlockTrigger(World.toBlock(_blockPos));
		this.cmds[0].enabled = _world.IsEditor() && !GameUtils.IsWorldEditor();
		return this.cmds;
	}

	// Token: 0x060009B8 RID: 2488 RVA: 0x00042A3F File Offset: 0x00040C3F
	public override void OnTriggered(EntityPlayer _player, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, List<BlockChangeInfo> _blockChanges, BlockTrigger _triggeredBy)
	{
		base.OnTriggered(_player, _world, _blockPos, _blockValue, _blockChanges, _triggeredBy);
		this.HandleDowngrade(_world, _blockPos, _blockValue, _blockChanges);
	}

	// Token: 0x060009B9 RID: 2489 RVA: 0x00042A5C File Offset: 0x00040C5C
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleDowngrade(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, List<BlockChangeInfo> _blockChanges)
	{
		ChunkCluster chunkCache = _world.ChunkCache;
		if (chunkCache == null)
		{
			return;
		}
		if (!this.DowngradeBlock.isair)
		{
			base.SpawnDowngradeFX(_world, _blockValue, _blockPos, _blockValue.Block.tintColor, -1);
			BlockValue blockValue = this.DowngradeBlock;
			blockValue = BlockPlaceholderMap.Instance.Replace(blockValue, _world.GetGameRandom(), _blockPos.x, _blockPos.z, false);
			blockValue.rotation = _blockValue.rotation;
			if (!blockValue.Block.shape.IsTerrain())
			{
				_blockChanges.Add(new BlockChangeInfo(_blockPos, blockValue));
				if (chunkCache.GetTextureFull(_blockPos) != 0L)
				{
					if (this.RemovePaintOnDowngrade == null)
					{
						GameManager.Instance.SetBlockTextureServer(_blockPos, BlockFace.None, 0, -1, byte.MaxValue);
						return;
					}
					for (int i = 0; i < this.RemovePaintOnDowngrade.Count; i++)
					{
						GameManager.Instance.SetBlockTextureServer(_blockPos, this.RemovePaintOnDowngrade[i], 0, -1, byte.MaxValue);
					}
					return;
				}
			}
			else
			{
				_blockChanges.Add(new BlockChangeInfo(_blockPos, blockValue, blockValue.Block.Density));
			}
		}
	}

	// Token: 0x040009E7 RID: 2535
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("trigger", "wrench", true, false, null)
	};
}
