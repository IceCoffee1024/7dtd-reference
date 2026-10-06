using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000110 RID: 272
[Preserve]
public class BlockActivate : Block
{
	// Token: 0x17000077 RID: 119
	// (get) Token: 0x06000713 RID: 1811 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowBlockTriggers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x000334D0 File Offset: 0x000316D0
	public override void Init()
	{
		base.Init();
		if (base.Properties.Values.ContainsKey("ActivateSound"))
		{
			this.activateSound = base.Properties.Values["ActivateSound"];
		}
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x0003350C File Offset: 0x0003170C
	public override void LateInit()
	{
		base.LateInit();
		if (base.Properties.Values.ContainsKey(BlockActivate.PropBlockChangeTo))
		{
			this.blockChangeTo = Block.GetBlockValue(base.Properties.Values[BlockActivate.PropBlockChangeTo], false);
			this.useChangeTo = true;
		}
	}

	// Token: 0x06000717 RID: 1815 RVA: 0x0003355E File Offset: 0x0003175E
	public override void OnBlockValueChanged(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
		base.OnBlockValueChanged(_world, _chunk, _blockPos, _oldBlockValue, _newBlockValue);
	}

	// Token: 0x06000718 RID: 1816 RVA: 0x00033570 File Offset: 0x00031770
	public override string GetActivationText(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		PlayerActionsLocal playerInput = ((EntityPlayerLocal)_entityFocusing).playerInput;
		string text = playerInput.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null) + playerInput.PermanentActions.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null);
		string localizedBlockName = _blockValue.Block.GetLocalizedBlockName();
		return string.Format(Localization.Get("questBlockActivate", false, null), text, localizedBlockName);
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x000335D4 File Offset: 0x000317D4
	public override bool OnBlockActivated(string _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		if (!(_commandName == "activate"))
		{
			if (_commandName == "trigger")
			{
				XUiC_TriggerProperties.Show(_player.PlayerUI.xui, _blockPos, true, false);
			}
		}
		else if (!_world.IsEditor())
		{
			base.HandleTrigger(_player, (World)_world, _blockPos, _blockValue);
			Manager.BroadcastPlay(_blockPos.ToVector3() + Vector3.one * 0.5f, this.activateSound, 0f);
			if (this.useChangeTo)
			{
				this.blockChangeTo.rotation = _blockValue.rotation;
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x00033674 File Offset: 0x00031874
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		((Chunk)_world.ChunkCache.GetChunkSync(World.toChunkXZ(_blockPos.x), _blockPos.y, World.toChunkXZ(_blockPos.z))).GetBlockTrigger(World.toBlock(_blockPos));
		this.cmds[0].enabled = true;
		this.cmds[1].enabled = _world.IsEditor() && !GameUtils.IsWorldEditor();
		return this.cmds;
	}

	// Token: 0x04000864 RID: 2148
	[PublicizedFrom(EAccessModifier.Private)]
	public string activateSound;

	// Token: 0x04000865 RID: 2149
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue blockChangeTo = BlockValue.Air;

	// Token: 0x04000866 RID: 2150
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useChangeTo;

	// Token: 0x04000867 RID: 2151
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("activate", "electric_switch", true, false, null),
		new BlockActivationCommand("trigger", "wrench", true, false, null)
	};

	// Token: 0x04000868 RID: 2152
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropBlockChangeTo = "BlockChangeTo";
}
