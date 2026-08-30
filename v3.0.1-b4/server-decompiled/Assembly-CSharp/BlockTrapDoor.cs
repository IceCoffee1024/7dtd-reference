using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200015C RID: 348
[Preserve]
public class BlockTrapDoor : Block
{
	// Token: 0x1700009F RID: 159
	// (get) Token: 0x0600098E RID: 2446 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowBlockTriggers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600098F RID: 2447 RVA: 0x000420E0 File Offset: 0x000402E0
	public BlockTrapDoor()
	{
		this.IsCheckCollideWithEntity = true;
	}

	// Token: 0x06000990 RID: 2448 RVA: 0x00042138 File Offset: 0x00040338
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

	// Token: 0x06000991 RID: 2449 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x06000992 RID: 2450 RVA: 0x000421BB File Offset: 0x000403BB
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		this.cmds[0].enabled = _world.IsEditor() && !GameUtils.IsWorldEditor();
		return this.cmds;
	}

	// Token: 0x06000993 RID: 2451 RVA: 0x000421E7 File Offset: 0x000403E7
	public override bool OnBlockActivated(string _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		XUiC_TriggerProperties.Show(_player.PlayerUI.xui, _blockPos, true, true);
		return true;
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x00042200 File Offset: 0x00040400
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

	// Token: 0x06000995 RID: 2453 RVA: 0x00042384 File Offset: 0x00040584
	public override Block.DestroyedResult OnBlockDestroyedBy(WorldBase _world, BlockValueRef _bvRef, BlockValue _blockValue, int _entityId, bool _bUseHarvestTool)
	{
		EntityPlayer entityPlayer = ((World)_world).GetEntity(_entityId) as EntityPlayer;
		if (entityPlayer != null)
		{
			base.HandleTrigger(entityPlayer, (World)_world, _bvRef, _blockValue);
		}
		return base.OnBlockDestroyedBy(_world, _bvRef, _blockValue, _entityId, _bUseHarvestTool);
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x000423C8 File Offset: 0x000405C8
	public override Block.DestroyedResult OnBlockDestroyedByExplosion(WorldBase _world, BlockValueRef _bvRef, BlockValue _blockValue, int _playerThatStartedExpl)
	{
		EntityPlayer entityPlayer = ((World)_world).GetEntity(_playerThatStartedExpl) as EntityPlayer;
		if (entityPlayer != null)
		{
			base.HandleTrigger(entityPlayer, (World)_world, _bvRef, _blockValue);
		}
		return base.OnBlockDestroyedByExplosion(_world, _bvRef, _blockValue, _playerThatStartedExpl);
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x0004240A File Offset: 0x0004060A
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

	// Token: 0x06000998 RID: 2456 RVA: 0x00042448 File Offset: 0x00040648
	public override void OnTriggered(EntityPlayer _player, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, List<BlockChangeInfo> _blockChanges, BlockTrigger _triggeredBy)
	{
		base.OnTriggered(_player, _world, _blockPos, _blockValue, _blockChanges, _triggeredBy);
		this.DamageBlock(_world, _blockPos, _blockValue, _blockValue.Block.MaxDamage - _blockValue.damage, -1, null, false, false);
	}

	// Token: 0x040009D4 RID: 2516
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropTriggerDelay = "TriggerDelay";

	// Token: 0x040009D5 RID: 2517
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropTriggerSound = "TriggerSound";

	// Token: 0x040009D6 RID: 2518
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("trigger", "wrench", true, false, null)
	};

	// Token: 0x040009D7 RID: 2519
	[PublicizedFrom(EAccessModifier.Private)]
	public float TriggerDelay = 0.6f;

	// Token: 0x040009D8 RID: 2520
	[PublicizedFrom(EAccessModifier.Private)]
	public string TriggerSound = "trapdoor_trigger";
}
