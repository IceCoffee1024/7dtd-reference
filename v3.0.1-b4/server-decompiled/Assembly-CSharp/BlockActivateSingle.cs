using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200010E RID: 270
[Preserve]
public class BlockActivateSingle : Block
{
	// Token: 0x17000078 RID: 120
	// (get) Token: 0x0600070D RID: 1805 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowBlockTriggers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x000333C4 File Offset: 0x000315C4
	public override void Init()
	{
		base.Init();
		base.Properties.ParseString(BlockActivateSingle.PropActivateSound, ref this.activateSound);
		base.Properties.ParseString(BlockActivateSingle.PropActivatedAnimBool, ref this.AnimActivatedBool);
		base.Properties.ParseString(BlockActivateSingle.PropActivatedAnimTrigger, ref this.AnimActivatedTrigger);
		base.Properties.ParseString(BlockActivateSingle.PropActivatedAnimState, ref this.AnimActivatedState);
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x0003342F File Offset: 0x0003162F
	public override void LateInit()
	{
		base.LateInit();
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x00033437 File Offset: 0x00031637
	public override void OnBlockValueChanged(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
		base.OnBlockValueChanged(_world, _chunk, _blockPos, _oldBlockValue, _newBlockValue);
		this.Refresh(_world, _chunk, _blockPos, _newBlockValue);
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x00033451 File Offset: 0x00031651
	public override void OnBlockEntityTransformAfterActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
		base.OnBlockEntityTransformAfterActivated(_world, _blockPos, _blockValue, _ebcd);
		this.Refresh(_world, null, _blockPos, _blockValue);
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x00033468 File Offset: 0x00031668
	public override void OnBlockRemoved(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		BlockUtilityNavIcon.RemoveNavObject(_blockPos);
		base.OnBlockRemoved(_world, _chunk, _blockPos, _blockValue);
	}

	// Token: 0x06000714 RID: 1812 RVA: 0x0003347B File Offset: 0x0003167B
	public override void OnBlockUnloaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		BlockUtilityNavIcon.RemoveNavObject(_blockPos);
		base.OnBlockUnloaded(_world, _blockPos, _blockValue);
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x0003348C File Offset: 0x0003168C
	public override string GetActivationText(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		if (!_world.IsEditor() && (_blockValue.meta & 2) != 0)
		{
			return "";
		}
		PlayerActionsLocal playerInput = ((EntityPlayerLocal)_entityFocusing).playerInput;
		string text = playerInput.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null) + playerInput.PermanentActions.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null);
		string localizedBlockName = _blockValue.Block.GetLocalizedBlockName();
		return string.Format(Localization.Get("questBlockActivate", false, null), text, localizedBlockName);
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x00033506 File Offset: 0x00031706
	public override void OnBlockAdded(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue, PlatformUserIdentifierAbs _addedByPlayer)
	{
		base.OnBlockAdded(_world, _chunk, _blockPos, _blockValue, _addedByPlayer);
		_chunk.GetBlockTrigger(World.toBlock(_blockPos));
		this.Refresh(_world, _chunk, _blockPos, _blockValue);
	}

	// Token: 0x06000717 RID: 1815 RVA: 0x00033530 File Offset: 0x00031730
	public override bool OnBlockActivated(string _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		if (!_world.IsEditor())
		{
			if ((_blockValue.meta & 2) != 0)
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
				XUiC_TriggerProperties.Show(_player.PlayerUI.xui, _blockPos, true, false);
			}
		}
		else if (!_world.IsEditor())
		{
			bool flag = (_blockValue.meta & 2) > 0;
			if (!flag)
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

	// Token: 0x06000718 RID: 1816 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x00033634 File Offset: 0x00031834
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		((Chunk)_world.ChunkCache.GetChunkSync(World.toChunkXZ(_blockPos.x), _blockPos.y, World.toChunkXZ(_blockPos.z))).GetBlockTrigger(World.toBlock(_blockPos));
		this.cmds[0].enabled = !_world.IsEditor();
		this.cmds[1].enabled = _world.IsEditor() && !GameUtils.IsWorldEditor();
		return this.cmds;
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x000336C0 File Offset: 0x000318C0
	public override void Refresh(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.Refresh(_world, _chunk, _blockPos, _blockValue);
		bool flag = (_blockValue.meta & 2) > 0;
		BlockUtilityNavIcon.UpdateNavIcon(!flag, _blockPos);
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
		BlockSwitchSingleController component = blockEntity.transform.GetComponent<BlockSwitchSingleController>();
		if (component)
		{
			component.SetState(flag);
		}
		this.updateAnimState(blockEntity, flag);
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x0003376C File Offset: 0x0003196C
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateAnimState(BlockEntityData _ebcd, bool _bOpen)
	{
		if (this.AnimActivatedBool == "" || this.AnimActivatedTrigger == "")
		{
			return;
		}
		Animator[] componentsInChildren;
		if (_ebcd != null && _ebcd.bHasTransform && (componentsInChildren = _ebcd.transform.GetComponentsInChildren<Animator>()) != null)
		{
			foreach (Animator animator in componentsInChildren)
			{
				animator.SetBool(this.AnimActivatedBool, _bOpen);
				animator.SetTrigger(this.AnimActivatedTrigger);
			}
		}
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x000337E8 File Offset: 0x000319E8
	public override void ForceAnimationState(BlockValue _blockValue, BlockEntityData _ebcd)
	{
		if (this.AnimActivatedState == "" || this.AnimActivatedBool == "")
		{
			return;
		}
		Animator[] componentsInChildren;
		if (_ebcd != null && _ebcd.bHasTransform && (componentsInChildren = _ebcd.transform.GetComponentsInChildren<Animator>(false)) != null)
		{
			bool flag = (_blockValue.meta & 2) > 0;
			foreach (Animator animator in componentsInChildren)
			{
				animator.SetBool(this.AnimActivatedBool, flag);
				if (flag)
				{
					animator.CrossFade(this.AnimActivatedState, 0f);
				}
				else
				{
					animator.CrossFade(this.AnimActivatedState, 0f);
				}
			}
		}
	}

	// Token: 0x04000864 RID: 2148
	public const int cMetaOn = 2;

	// Token: 0x04000865 RID: 2149
	[PublicizedFrom(EAccessModifier.Private)]
	public string activateSound;

	// Token: 0x04000866 RID: 2150
	[PublicizedFrom(EAccessModifier.Protected)]
	public string AnimActivatedBool = "";

	// Token: 0x04000867 RID: 2151
	[PublicizedFrom(EAccessModifier.Protected)]
	public string AnimActivatedTrigger = "";

	// Token: 0x04000868 RID: 2152
	[PublicizedFrom(EAccessModifier.Protected)]
	public string AnimActivatedState = "";

	// Token: 0x04000869 RID: 2153
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("activate", "electric_switch", true, false, null),
		new BlockActivationCommand("trigger", "wrench", true, false, null)
	};

	// Token: 0x0400086A RID: 2154
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropActivateSound = "ActivateSound";

	// Token: 0x0400086B RID: 2155
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropActivatedAnimBool = "ActivatedAnimBool";

	// Token: 0x0400086C RID: 2156
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropActivatedAnimTrigger = "ActivatedAnimTrigger";

	// Token: 0x0400086D RID: 2157
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropActivatedAnimState = "ActivatedAnimState";
}
