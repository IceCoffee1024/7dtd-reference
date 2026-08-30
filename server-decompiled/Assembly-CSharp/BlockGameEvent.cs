using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200011E RID: 286
[Preserve]
public class BlockGameEvent : Block
{
	// Token: 0x17000092 RID: 146
	// (get) Token: 0x06000783 RID: 1923 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowBlockTriggers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x0003585C File Offset: 0x00033A5C
	public override void Init()
	{
		base.Init();
		base.Properties.ParseString(BlockGameEvent.PropOnActivateEvent, ref this.onActivateEvent);
		base.Properties.ParseString(BlockGameEvent.PropOnDamageEvent, ref this.onDamageEvent);
		base.Properties.ParseString(BlockGameEvent.PropOnTriggeredEvent, ref this.onTriggeredEvent);
		base.Properties.ParseString(BlockGameEvent.PropOnAddedEvent, ref this.onAddedEvent);
		base.Properties.ParseBool(BlockGameEvent.PropDestroyOnEvent, ref this.destroyOnEvent);
		base.Properties.ParseBool(BlockGameEvent.PropSendDamageUpdate, ref this.sendDamageUpdate);
		base.Properties.ParseFloat(BlockGameEvent.PropActivateTime, ref this.activateTime);
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x0003590C File Offset: 0x00033B0C
	public override string GetActivationText(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		if (this.onActivateEvent == "" && !_world.IsEditor())
		{
			return "";
		}
		PlayerActionsLocal playerInput = ((EntityPlayerLocal)_entityFocusing).playerInput;
		string text = playerInput.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null) + playerInput.PermanentActions.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null);
		string localizedBlockName = _blockValue.Block.GetLocalizedBlockName();
		return string.Format(Localization.Get("questBlockActivate", false, null), text, localizedBlockName);
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x00035990 File Offset: 0x00033B90
	public override void OnBlockAdded(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue, PlatformUserIdentifierAbs _addedByPlayer)
	{
		base.OnBlockAdded(_world, _chunk, _blockPos, _blockValue, _addedByPlayer);
		GameEventManager.Current.HandleAction(this.onAddedEvent, null, null, false, _blockPos, "", "", false, true, "", null, null);
		if (this.destroyOnEvent)
		{
			this.DamageBlock(_world, _blockPos, _blockValue, _blockValue.Block.MaxDamage - _blockValue.damage, -1, null, false, false);
		}
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x00035A08 File Offset: 0x00033C08
	public override bool OnBlockActivated(string _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		if (_commandName == "activate")
		{
			if (this.onActivateEvent != "")
			{
				if (this.activateTime > 0f)
				{
					LocalPlayerUI playerUI = _player.PlayerUI;
					TimerEventData timerEventData = new TimerEventData();
					timerEventData.Data = new object[] { _blockPos, _player };
					timerEventData.CloseOnHit = true;
					timerEventData.FullTimeFinishEvent += this.EventData_Event;
					XUiC_Timer.OpenTimer(playerUI.xui, this.activateTime, timerEventData, -1f, "", true);
				}
				else
				{
					GameEventManager.Current.HandleAction(this.onActivateEvent, null, _player, false, _blockPos, "", "", false, true, "", null, null);
				}
				if (this.destroyOnEvent)
				{
					this.DamageBlock(_world, _blockPos, _blockValue, _blockValue.Block.MaxDamage - _blockValue.damage, -1, null, false, false);
				}
			}
			return true;
		}
		if (!(_commandName == "trigger"))
		{
			return false;
		}
		XUiC_TriggerProperties.Show(_player.PlayerUI.xui, _blockPos, false, true);
		return true;
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x00035B30 File Offset: 0x00033D30
	[PublicizedFrom(EAccessModifier.Private)]
	public void EventData_Event(TimerEventData timerData)
	{
		object[] array = (object[])timerData.Data;
		Vector3i vector3i = (Vector3i)array[0];
		EntityPlayerLocal entityPlayerLocal = array[1] as EntityPlayerLocal;
		GameEventManager.Current.HandleAction(this.onActivateEvent, null, entityPlayerLocal, false, vector3i, "", "", false, true, "", null, null);
	}

	// Token: 0x0600078A RID: 1930 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x00035B88 File Offset: 0x00033D88
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		this.cmds[0].enabled = !_world.IsEditor() && this.onActivateEvent != "";
		this.cmds[1].enabled = _world.IsEditor() && !GameUtils.IsWorldEditor();
		return this.cmds;
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x00035BEC File Offset: 0x00033DEC
	public override int OnBlockDamaged(WorldBase _world, BlockValueRef _bvRef, BlockValue _blockValue, int _damagePoints, int _entityIdThatDamaged, ItemActionAttack.AttackHitInfo _attackHitInfo, bool _bUseHarvestTool, bool _bBypassMaxDamage, int _recDepth = 0)
	{
		if (this.onDamageEvent != "")
		{
			if (GameEventManager.Current.GetTargetType(this.onDamageEvent) != GameEventActionSequence.TargetTypes.Block)
			{
				Debug.LogError("Game Event Target Type must be set to 'Block' to be used in BlockGameEvent.");
			}
			else
			{
				Entity entity = _world.GetEntity(_entityIdThatDamaged);
				EntityVehicle entityVehicle = entity as EntityVehicle;
				if (entityVehicle != null)
				{
					entity = entityVehicle.GetFirstAttached();
				}
				GameEventManager.Current.HandleAction(this.onDamageEvent, null, entity as EntityPlayer, false, _bvRef, "", "", false, true, "", null, null);
				if (this.destroyOnEvent)
				{
					this.DamageBlock(_world, _bvRef, _blockValue, _blockValue.Block.MaxDamage - _blockValue.damage, -1, null, false, false);
				}
			}
		}
		int num = base.OnBlockDamaged(_world, _bvRef, _blockValue, _damagePoints, _entityIdThatDamaged, _attackHitInfo, _bUseHarvestTool, _bBypassMaxDamage, _recDepth);
		if (num > 0 && this.sendDamageUpdate && GameManager.Instance.World.GetEntity(_entityIdThatDamaged) is EntityPlayer)
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				GameEventManager.Current.SendBlockDamageUpdate(_bvRef);
				return num;
			}
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageGameEventResponse>().Setup(NetPackageGameEventResponse.ResponseTypes.BlockDamaged, _bvRef), false);
		}
		return num;
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x00035D18 File Offset: 0x00033F18
	public override void OnTriggered(EntityPlayer _player, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, List<BlockChangeInfo> _blockChanges, BlockTrigger _triggeredBy)
	{
		base.OnTriggered(_player, _world, _blockPos, _blockValue, _blockChanges, _triggeredBy);
		if (this.onTriggeredEvent != "")
		{
			if (GameEventManager.Current.GetTargetType(this.onTriggeredEvent) != GameEventActionSequence.TargetTypes.Block)
			{
				Debug.LogError("Game Event Target Type must be set to 'Block' to be used in BlockGameEvent.");
				return;
			}
			GameEventManager.Current.HandleAction(this.onTriggeredEvent, null, _player, false, _blockPos, "", "", false, true, "", null, null);
			if (this.destroyOnEvent)
			{
				this.DamageBlock(_world, _blockPos, _blockValue, _blockValue.Block.MaxDamage - _blockValue.damage, -1, null, false, false);
			}
		}
	}

	// Token: 0x040008E8 RID: 2280
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropOnActivateEvent = "ActivateEvent";

	// Token: 0x040008E9 RID: 2281
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropOnDamageEvent = "DamageEvent";

	// Token: 0x040008EA RID: 2282
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropOnTriggeredEvent = "TriggeredEvent";

	// Token: 0x040008EB RID: 2283
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropOnAddedEvent = "AddedEvent";

	// Token: 0x040008EC RID: 2284
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropDestroyOnEvent = "DestroyOnEvent";

	// Token: 0x040008ED RID: 2285
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropSendDamageUpdate = "SendDamageUpdate";

	// Token: 0x040008EE RID: 2286
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropActivateTime = "ActivateTime";

	// Token: 0x040008EF RID: 2287
	[PublicizedFrom(EAccessModifier.Protected)]
	public string onActivateEvent = "";

	// Token: 0x040008F0 RID: 2288
	[PublicizedFrom(EAccessModifier.Protected)]
	public string onDamageEvent = "";

	// Token: 0x040008F1 RID: 2289
	[PublicizedFrom(EAccessModifier.Protected)]
	public string onTriggeredEvent = "";

	// Token: 0x040008F2 RID: 2290
	[PublicizedFrom(EAccessModifier.Protected)]
	public string onAddedEvent = "";

	// Token: 0x040008F3 RID: 2291
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool destroyOnEvent;

	// Token: 0x040008F4 RID: 2292
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool sendDamageUpdate;

	// Token: 0x040008F5 RID: 2293
	[PublicizedFrom(EAccessModifier.Private)]
	public float activateTime;

	// Token: 0x040008F6 RID: 2294
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("activate", "electric_switch", false, false, null),
		new BlockActivationCommand("trigger", "wrench", true, false, null)
	};
}
