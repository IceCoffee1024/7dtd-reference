using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200015D RID: 349
[Preserve]
public class BlockTrapDoor : Block
{
	// Token: 0x1700009F RID: 159
	// (get) Token: 0x06000998 RID: 2456 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowBlockTriggers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x00042340 File Offset: 0x00040540
	public BlockTrapDoor()
	{
		this.IsCheckCollideWithEntity = true;
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x00042398 File Offset: 0x00040598
	public override void Init()
	{
		base.Init();
		if (base.Properties.Values.ContainsKey(BlockTrapDoor.PropTriggerDelay))
		{
			this.TriggerDelay = StringParsers.ParseFloat(base.Properties.Values[BlockTrapDoor.PropTriggerDelay], 0, -1, NumberStyles.Any);
		}
		if (base.Properties.Values.ContainsKey(BlockTrapDoor.PropTriggerSound))
		{
			this.TriggerSound = base.Properties.Values[BlockTrapDoor.PropTriggerSound];
		}
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x0004241B File Offset: 0x0004061B
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		this.cmds[0].enabled = _world.IsEditor() && !GameUtils.IsWorldEditor();
		return this.cmds;
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x00042447 File Offset: 0x00040647
	public override bool OnBlockActivated(string _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		XUiC_TriggerProperties.Show(_player.PlayerUI.xui, _blockPos, true, true);
		return true;
	}

	// Token: 0x0600099E RID: 2462 RVA: 0x00042460 File Offset: 0x00040660
	public override bool OnEntityCollidedWithBlock(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, Entity _targetEntity)
	{
		if (_targetEntity as EntityAlive == null)
		{
			return false;
		}
		EntityAlive entityAlive = (EntityAlive)_targetEntity;
		if (entityAlive.IsDead())
		{
			return false;
		}
		if (_blockValue.meta == 1)
		{
			return false;
		}
		Vector3 vector = _blockPos.ToVector3() + new Vector3(0.5f, 0f, 0.5f);
		vector.y = 0f;
		Vector3 position = entityAlive.position;
		position.y = 0f;
		float num = Utils.FastAbs(vector.x - position.x);
		float num2 = Utils.FastAbs(vector.z - position.z);
		float num3 = ((_blockValue.Block != null && _blockValue.Block.isMultiBlock) ? ((float)_blockValue.Block.multiBlockPos.dim.x / 2f * 0.45f) : 0.45f);
		if (num > num3 || num2 > num3)
		{
			return false;
		}
		_blockValue.meta = 1;
		_world.SetBlockRPC(_blockPos, _blockValue);
		float value = EffectManager.GetValue(PassiveEffects.TrapDoorTriggerDelay, null, this.TriggerDelay, _targetEntity as EntityAlive, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		if (value > 0f)
		{
			GameManager.Instance.PlaySoundAtPositionServer(_blockPos.ToVector3(), this.TriggerSound, AudioRolloffMode.Linear, 5, _targetEntity.entityId, 1f);
			GameManager.Instance.StartCoroutine(this.damageBlock(value, _world, _blockPos, _blockValue, _targetEntity as EntityPlayer, _targetEntity.entityId));
		}
		return true;
	}

	// Token: 0x0600099F RID: 2463 RVA: 0x000425E4 File Offset: 0x000407E4
	public override Block.DestroyedResult OnBlockDestroyedBy(WorldBase _world, BlockValueRef _bvRef, BlockValue _blockValue, int _entityId, bool _bUseHarvestTool)
	{
		EntityPlayer entityPlayer = ((World)_world).GetEntity(_entityId) as EntityPlayer;
		if (entityPlayer != null)
		{
			base.HandleTrigger(entityPlayer, (World)_world, _bvRef, _blockValue);
		}
		return base.OnBlockDestroyedBy(_world, _bvRef, _blockValue, _entityId, _bUseHarvestTool);
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x00042628 File Offset: 0x00040828
	public override Block.DestroyedResult OnBlockDestroyedByExplosion(WorldBase _world, BlockValueRef _bvRef, BlockValue _blockValue, int _playerThatStartedExpl)
	{
		EntityPlayer entityPlayer = ((World)_world).GetEntity(_playerThatStartedExpl) as EntityPlayer;
		if (entityPlayer != null)
		{
			base.HandleTrigger(entityPlayer, (World)_world, _bvRef, _blockValue);
		}
		return base.OnBlockDestroyedByExplosion(_world, _bvRef, _blockValue, _playerThatStartedExpl);
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x0004266A File Offset: 0x0004086A
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator damageBlock(float time, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayer _player, int _entityId)
	{
		yield return new WaitForSeconds(time);
		if (_player != null)
		{
			base.HandleTrigger(_player, (World)_world, _blockPos, _blockValue);
		}
		this.DamageBlock(_world, _blockPos, _blockValue, _blockValue.Block.MaxDamage - _blockValue.damage, _entityId, null, false, false);
		yield break;
	}

	// Token: 0x060009A2 RID: 2466 RVA: 0x000426A8 File Offset: 0x000408A8
	public override void OnTriggered(EntityPlayer _player, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, List<BlockChangeInfo> _blockChanges, BlockTrigger _triggeredBy)
	{
		base.OnTriggered(_player, _world, _blockPos, _blockValue, _blockChanges, _triggeredBy);
		this.DamageBlock(_world, _blockPos, _blockValue, _blockValue.Block.MaxDamage - _blockValue.damage, -1, null, false, false);
	}

	// Token: 0x040009D6 RID: 2518
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropTriggerDelay = "TriggerDelay";

	// Token: 0x040009D7 RID: 2519
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropTriggerSound = "TriggerSound";

	// Token: 0x040009D8 RID: 2520
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("trigger", "wrench", true, false, null)
	};

	// Token: 0x040009D9 RID: 2521
	[PublicizedFrom(EAccessModifier.Private)]
	public float TriggerDelay = 0.6f;

	// Token: 0x040009DA RID: 2522
	[PublicizedFrom(EAccessModifier.Private)]
	public string TriggerSound = "trapdoor_trigger";
}
