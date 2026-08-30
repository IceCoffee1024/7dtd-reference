using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200010D RID: 269
[Preserve]
public class BlockActivate : Block
{
	// Token: 0x17000077 RID: 119
	// (get) Token: 0x06000703 RID: 1795 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowBlockTriggers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x0003311C File Offset: 0x0003131C
	public override void Init()
	{
		base.Init();
		if (base.Properties.Values.ContainsKey("ActivateSound"))
		{
			this.activateSound = base.Properties.Values["ActivateSound"];
		}
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x00033158 File Offset: 0x00031358
	public override void LateInit()
	{
		base.LateInit();
		if (base.Properties.Values.ContainsKey(BlockActivate.PropBlockChangeTo))
		{
			this.blockChangeTo = Block.GetBlockValue(base.Properties.Values[BlockActivate.PropBlockChangeTo], false);
			this.useChangeTo = true;
		}
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x000331AA File Offset: 0x000313AA
	public override void OnBlockValueChanged(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
		base.OnBlockValueChanged(_world, _chunk, _blockPos, _oldBlockValue, _newBlockValue);
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x000331BC File Offset: 0x000313BC
	public override string GetActivationText(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		PlayerActionsLocal playerInput = ((EntityPlayerLocal)_entityFocusing).playerInput;
		string text = playerInput.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null) + playerInput.PermanentActions.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null);
		string localizedBlockName = _blockValue.Block.GetLocalizedBlockName();
		return string.Format(Localization.Get("questBlockActivate", false, null), text, localizedBlockName);
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x00033220 File Offset: 0x00031420
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

	// Token: 0x0600070A RID: 1802 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x000332C0 File Offset: 0x000314C0
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		((Chunk)_world.ChunkCache.GetChunkSync(World.toChunkXZ(_blockPos.x), _blockPos.y, World.toChunkXZ(_blockPos.z))).GetBlockTrigger(World.toBlock(_blockPos));
		this.cmds[0].enabled = true;
		this.cmds[1].enabled = _world.IsEditor() && !GameUtils.IsWorldEditor();
		return this.cmds;
	}

	// Token: 0x0400085F RID: 2143
	[PublicizedFrom(EAccessModifier.Private)]
	public string activateSound;

	// Token: 0x04000860 RID: 2144
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue blockChangeTo = BlockValue.Air;

	// Token: 0x04000861 RID: 2145
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useChangeTo;

	// Token: 0x04000862 RID: 2146
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("activate", "electric_switch", true, false, null),
		new BlockActivationCommand("trigger", "wrench", true, false, null)
	};

	// Token: 0x04000863 RID: 2147
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropBlockChangeTo = "BlockChangeTo";
}
