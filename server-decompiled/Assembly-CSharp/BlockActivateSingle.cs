using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000111 RID: 273
[Preserve]
public class BlockActivateSingle : Block
{
	// Token: 0x17000078 RID: 120
	// (get) Token: 0x0600071D RID: 1821 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowBlockTriggers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600071F RID: 1823 RVA: 0x00033778 File Offset: 0x00031978
	public override void Init()
	{
		base.Init();
		base.Properties.ParseString(BlockActivateSingle.PropActivateSound, ref this.activateSound);
		base.Properties.ParseString(BlockActivateSingle.PropActivatedAnimBool, ref this.AnimActivatedBool);
		base.Properties.ParseString(BlockActivateSingle.PropActivatedAnimTrigger, ref this.AnimActivatedTrigger);
		base.Properties.ParseString(BlockActivateSingle.PropActivatedAnimState, ref this.AnimActivatedState);
	}

	// Token: 0x06000720 RID: 1824 RVA: 0x000337E3 File Offset: 0x000319E3
	public override void LateInit()
	{
		base.LateInit();
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x000337EB File Offset: 0x000319EB
	public override void OnBlockValueChanged(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
		base.OnBlockValueChanged(_world, _chunk, _blockPos, _oldBlockValue, _newBlockValue);
		this.Refresh(_world, _chunk, _blockPos, _newBlockValue);
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x00033805 File Offset: 0x00031A05
	public override void OnBlockEntityTransformAfterActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
		base.OnBlockEntityTransformAfterActivated(_world, _blockPos, _blockValue, _ebcd);
		this.Refresh(_world, null, _blockPos, _blockValue);
	}

	// Token: 0x06000723 RID: 1827 RVA: 0x0003381C File Offset: 0x00031A1C
	public override void OnBlockRemoved(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		BlockUtilityNavIcon.RemoveNavObject(_blockPos);
		base.OnBlockRemoved(_world, _chunk, _blockPos, _blockValue);
	}

	// Token: 0x06000724 RID: 1828 RVA: 0x0003382F File Offset: 0x00031A2F
	public override void OnBlockUnloaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		BlockUtilityNavIcon.RemoveNavObject(_blockPos);
		base.OnBlockUnloaded(_world, _blockPos, _blockValue);
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x00033840 File Offset: 0x00031A40
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

	// Token: 0x06000726 RID: 1830 RVA: 0x000338BA File Offset: 0x00031ABA
	public override void OnBlockAdded(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue, PlatformUserIdentifierAbs _addedByPlayer)
	{
		base.OnBlockAdded(_world, _chunk, _blockPos, _blockValue, _addedByPlayer);
		_chunk.GetBlockTrigger(World.toBlock(_blockPos));
		this.Refresh(_world, _chunk, _blockPos, _blockValue);
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x000338E4 File Offset: 0x00031AE4
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

	// Token: 0x06000728 RID: 1832 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x000339E8 File Offset: 0x00031BE8
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		((Chunk)_world.ChunkCache.GetChunkSync(World.toChunkXZ(_blockPos.x), _blockPos.y, World.toChunkXZ(_blockPos.z))).GetBlockTrigger(World.toBlock(_blockPos));
		this.cmds[0].enabled = !_world.IsEditor();
		this.cmds[1].enabled = _world.IsEditor() && !GameUtils.IsWorldEditor();
		return this.cmds;
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x00033A74 File Offset: 0x00031C74
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

	// Token: 0x0600072B RID: 1835 RVA: 0x00033B20 File Offset: 0x00031D20
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

	// Token: 0x0600072C RID: 1836 RVA: 0x00033B9C File Offset: 0x00031D9C
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

	// Token: 0x04000869 RID: 2153
	public const int cMetaOn = 2;

	// Token: 0x0400086A RID: 2154
	[PublicizedFrom(EAccessModifier.Private)]
	public string activateSound;

	// Token: 0x0400086B RID: 2155
	[PublicizedFrom(EAccessModifier.Protected)]
	public string AnimActivatedBool = "";

	// Token: 0x0400086C RID: 2156
	[PublicizedFrom(EAccessModifier.Protected)]
	public string AnimActivatedTrigger = "";

	// Token: 0x0400086D RID: 2157
	[PublicizedFrom(EAccessModifier.Protected)]
	public string AnimActivatedState = "";

	// Token: 0x0400086E RID: 2158
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("activate", "electric_switch", true, false, null),
		new BlockActivationCommand("trigger", "wrench", true, false, null)
	};

	// Token: 0x0400086F RID: 2159
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropActivateSound = "ActivateSound";

	// Token: 0x04000870 RID: 2160
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropActivatedAnimBool = "ActivatedAnimBool";

	// Token: 0x04000871 RID: 2161
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropActivatedAnimTrigger = "ActivatedAnimTrigger";

	// Token: 0x04000872 RID: 2162
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropActivatedAnimState = "ActivatedAnimState";
}
