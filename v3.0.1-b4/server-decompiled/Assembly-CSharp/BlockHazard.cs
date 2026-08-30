using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000120 RID: 288
[Preserve]
public class BlockHazard : BlockParticle
{
	// Token: 0x17000093 RID: 147
	// (get) Token: 0x06000793 RID: 1939 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowBlockTriggers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000795 RID: 1941 RVA: 0x00035F08 File Offset: 0x00034108
	public override void Init()
	{
		base.Init();
		if (base.Properties.Values.ContainsKey(BlockHazard.PropDamageBuffs))
		{
			if (this.buffActions == null)
			{
				this.buffActions = new List<string>();
			}
			string[] array = base.Properties.Values[BlockHazard.PropDamageBuffs].Split(',', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				this.buffActions.Add(array[i]);
			}
		}
		base.Properties.ParseVec(BlockHazard.PropDamageOffset, ref this.DamageOffset);
		base.Properties.ParseVec(BlockHazard.PropDamageSize, ref this.DamageSize);
		if (base.Properties.Values.ContainsKey(BlockHazard.PropSecondaryBuffs))
		{
			if (this.buffSecondaryActions == null)
			{
				this.buffSecondaryActions = new List<string>();
			}
			string[] array2 = base.Properties.Values[BlockHazard.PropSecondaryBuffs].Split(',', StringSplitOptions.None);
			for (int j = 0; j < array2.Length; j++)
			{
				this.buffSecondaryActions.Add(array2[j]);
			}
		}
		base.Properties.ParseVec(BlockHazard.PropSecondaryOffset, ref this.SecondaryOffset);
		base.Properties.ParseVec(BlockHazard.PropSecondarySize, ref this.SecondarySize);
		if (base.Properties.Values.ContainsKey("Model"))
		{
			DataLoader.PreloadBundle(base.Properties.Values["Model"]);
		}
		base.Properties.ParseString(BlockHazard.PropStartSound, ref this.StartSound);
		base.Properties.ParseString(BlockHazard.PropStopSound, ref this.StopSound);
	}

	// Token: 0x06000796 RID: 1942 RVA: 0x00036098 File Offset: 0x00034298
	public override byte GetLightValue(BlockValue _blockValue)
	{
		if ((_blockValue.meta & 2) == 0)
		{
			return 0;
		}
		return base.GetLightValue(_blockValue);
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x000360B0 File Offset: 0x000342B0
	public override string GetActivationText(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		if (!_world.IsEditor())
		{
			return null;
		}
		PlayerActionsLocal playerInput = ((EntityPlayerLocal)_entityFocusing).playerInput;
		string text = playerInput.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null) + playerInput.PermanentActions.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null);
		if ((_blockValue.meta & 2) != 0)
		{
			return string.Format(Localization.Get("useSwitchLightOff", false, null), text);
		}
		return string.Format(Localization.Get("useSwitchLightOn", false, null), text);
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x00036130 File Offset: 0x00034330
	public override bool OnBlockActivated(string _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		if (!(_commandName == "light"))
		{
			if (_commandName == "trigger")
			{
				XUiC_TriggerProperties.Show(_player.PlayerUI.xui, _blockPos, false, true);
			}
		}
		else if (_world.IsEditor() && this.toggleHazardStateForEditor(_world, _blockPos, _blockValue))
		{
			return true;
		}
		return false;
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x00036184 File Offset: 0x00034384
	[PublicizedFrom(EAccessModifier.Private)]
	public bool toggleHazardStateForEditor(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		bool flag = (_blockValue.meta & 2) > 0;
		flag = !flag;
		_blockValue.meta = (byte)(((int)_blockValue.meta & -3) | (flag ? 2 : 0));
		_blockValue.meta = (byte)(((int)_blockValue.meta & -2) | (flag ? 1 : 0));
		_world.SetBlockRPC(_blockPos, _blockValue);
		return true;
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x000361E8 File Offset: 0x000343E8
	public bool IsHazardOn(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		if (this.isMultiBlock && _blockValue.ischild)
		{
			Vector3i parentPos = this.multiBlockPos.GetParentPos(_blockPos, _blockValue);
			BlockValue block = _world.GetBlock(parentPos);
			return this.IsHazardOn(_world, parentPos, block);
		}
		return (_blockValue.meta & 2) > 0;
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x00036234 File Offset: 0x00034434
	public bool OriginalHazardState(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		if (this.isMultiBlock && _blockValue.ischild)
		{
			Vector3i parentPos = this.multiBlockPos.GetParentPos(_blockPos, _blockValue);
			BlockValue block = _world.GetBlock(parentPos);
			return this.IsHazardOn(_world, parentPos, block);
		}
		return (_blockValue.meta & 1) > 0;
	}

	// Token: 0x0600079C RID: 1948 RVA: 0x0003627E File Offset: 0x0003447E
	public BlockValue SetHazardState(BlockValue _blockValue, bool isOn)
	{
		_blockValue.meta = (byte)(((int)_blockValue.meta & -3) | (isOn ? 2 : 0));
		return _blockValue;
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x0003629C File Offset: 0x0003449C
	public override void OnBlockValueChanged(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
		if (_newBlockValue.ischild)
		{
			return;
		}
		this.IsHazardOn(_world, _blockPos, _newBlockValue);
		this.OriginalHazardState(_world, _blockPos, _newBlockValue);
		base.OnBlockValueChanged(_world, _chunk, _blockPos, _oldBlockValue, _newBlockValue);
		this.updateHazardState(_world, _chunk, _blockPos, _newBlockValue);
		this.checkParticles(_world, _blockPos, _newBlockValue);
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x000362EC File Offset: 0x000344EC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool updateHazardState(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		IChunk chunk = _chunk;
		if (chunk == null)
		{
			ChunkCluster chunkCache = _world.ChunkCache;
			if (chunkCache == null)
			{
				return false;
			}
			chunk = chunkCache.GetChunkSync(World.toChunkXZ(_blockPos.x), World.toChunkY(_blockPos.y), World.toChunkXZ(_blockPos.z));
			if (chunk == null)
			{
				return false;
			}
		}
		if (chunk == null)
		{
			return false;
		}
		BlockEntityData blockEntity = chunk.GetBlockEntity(_blockPos);
		if (blockEntity == null || !blockEntity.bHasTransform)
		{
			return false;
		}
		Transform transform = blockEntity.transform.Find("HazardDamage");
		if (transform == null)
		{
			GameObject gameObject = new GameObject("HazardDamage");
			gameObject.AddComponent<HazardDamageController>();
			transform = gameObject.transform;
			gameObject.AddComponent<BoxCollider>().isTrigger = true;
			transform.SetParent(blockEntity.transform);
		}
		transform.GetComponent<BoxCollider>().size = this.DamageSize;
		transform.localPosition = this.DamageOffset;
		transform.localRotation = Quaternion.identity;
		HazardDamageController hazardDamageController = transform.GetComponent<HazardDamageController>();
		if (hazardDamageController)
		{
			hazardDamageController.IsActive = this.IsHazardOn(_world, _blockPos, _blockValue);
			hazardDamageController.buffActions = this.buffActions;
		}
		if (this.buffSecondaryActions != null && this.buffSecondaryActions.Count != 0)
		{
			transform = blockEntity.transform.Find("SecondaryDamage");
			if (transform == null)
			{
				GameObject gameObject2 = new GameObject("SecondaryDamage");
				gameObject2.AddComponent<HazardDamageController>();
				transform = gameObject2.transform;
				gameObject2.AddComponent<BoxCollider>().isTrigger = true;
				transform.SetParent(blockEntity.transform);
			}
			transform.GetComponent<BoxCollider>().size = this.SecondarySize;
			transform.localPosition = this.SecondaryOffset;
			transform.localRotation = Quaternion.identity;
			hazardDamageController = transform.GetComponent<HazardDamageController>();
			if (hazardDamageController)
			{
				hazardDamageController.IsActive = this.IsHazardOn(_world, _blockPos, _blockValue);
				hazardDamageController.buffActions = this.buffSecondaryActions;
			}
		}
		return true;
	}

	// Token: 0x0600079F RID: 1951 RVA: 0x000364A8 File Offset: 0x000346A8
	public override void OnBlockEntityTransformAfterActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
		if (_blockValue.ischild)
		{
			return;
		}
		base.OnBlockEntityTransformAfterActivated(_world, _blockPos, _blockValue, _ebcd);
		this.updateHazardState(_world, null, _blockPos, _blockValue);
		this.checkParticles(_world, _blockPos, _blockValue);
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x000364D4 File Offset: 0x000346D4
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		this.cmds[0].enabled = _world.IsEditor();
		this.cmds[1].enabled = _world.IsEditor() && !GameUtils.IsWorldEditor();
		return this.cmds;
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x00036524 File Offset: 0x00034724
	public override void OnBlockReset(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		if (_blockValue.ischild)
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			bool flag = this.IsHazardOn(_world, _blockPos, _blockValue);
			bool flag2 = this.OriginalHazardState(_world, _blockPos, _blockValue);
			if (flag2 != flag)
			{
				_blockValue = this.SetHazardState(_blockValue, flag2);
				_world.SetBlockRPC(_blockPos, _blockValue);
			}
		}
	}

	// Token: 0x060007A3 RID: 1955 RVA: 0x0003657C File Offset: 0x0003477C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void checkParticles(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		if (_blockValue.ischild)
		{
			return;
		}
		bool flag = _world.GetGameManager().HasBlockParticleEffect(_blockPos);
		if (this.IsHazardOn(_world, _blockPos, _blockValue) && !flag)
		{
			this.addParticles(_world, _blockPos.x, _blockPos.y, _blockPos.z, _blockValue);
			return;
		}
		if (!this.IsHazardOn(_world, _blockPos, _blockValue) && flag)
		{
			this.removeParticles(_world, _blockPos.x, _blockPos.y, _blockPos.z, _blockValue);
		}
	}

	// Token: 0x060007A4 RID: 1956 RVA: 0x000365F4 File Offset: 0x000347F4
	public override void OnTriggered(EntityPlayer _player, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, List<BlockChangeInfo> _blockChanges, BlockTrigger _triggeredBy)
	{
		base.OnTriggered(_player, _world, _blockPos, _blockValue, _blockChanges, _triggeredBy);
		bool flag = !this.IsHazardOn(_world, _blockPos, _blockValue);
		if (flag)
		{
			Manager.BroadcastPlayByLocalPlayer(_blockPos.ToVector3() + Vector3.one * 0.5f, this.StartSound);
		}
		else
		{
			Manager.BroadcastPlayByLocalPlayer(_blockPos.ToVector3() + Vector3.one * 0.5f, this.StopSound);
		}
		_blockValue = this.SetHazardState(_blockValue, flag);
		_blockChanges.Add(new BlockChangeInfo(_blockPos, _blockValue));
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x00036690 File Offset: 0x00034890
	public override int OnBlockDamaged(WorldBase _world, BlockValueRef _bvRef, BlockValue _blockValue, int _damagePoints, int _entityIdThatDamaged, ItemActionAttack.AttackHitInfo _attackHitInfo, bool _bUseHarvestTool, bool _bBypassMaxDamage, int _recDepth = 0)
	{
		if (this.IsHazardOn(_world, _bvRef, _blockValue) && _damagePoints > 0 && this.buffActions != null && this.buffActions.Count > 0)
		{
			EntityAlive entityAlive = _world.GetEntity(_entityIdThatDamaged) as EntityAlive;
			if (entityAlive != null && entityAlive as EntityTurret == null)
			{
				ItemAction itemAction = entityAlive.inventory.holdingItemData.item.Actions[0];
				if (entityAlive != null)
				{
					if (itemAction is ItemActionRanged)
					{
						ItemActionRanged itemActionRanged = itemAction as ItemActionRanged;
						if (itemActionRanged == null || (itemActionRanged.Hitmask & 128) == 0)
						{
							goto IL_00DB;
						}
					}
					for (int i = 0; i < this.buffActions.Count; i++)
					{
						entityAlive.Buffs.AddBuff(this.buffActions[i], _bvRef, entityAlive.entityId, true, false, -1f);
					}
				}
			}
		}
		IL_00DB:
		return base.OnBlockDamaged(_world, _bvRef, _blockValue, _damagePoints, _entityIdThatDamaged, _attackHitInfo, _bUseHarvestTool, _bBypassMaxDamage, _recDepth);
	}

	// Token: 0x040008F7 RID: 2295
	public const int cMetaOriginalState = 1;

	// Token: 0x040008F8 RID: 2296
	public const int cMetaOn = 2;

	// Token: 0x040008F9 RID: 2297
	public Vector3 DamageOffset = Vector3.zero;

	// Token: 0x040008FA RID: 2298
	public Vector3 DamageSize = Vector3.one;

	// Token: 0x040008FB RID: 2299
	public Vector3 SecondaryOffset = Vector3.zero;

	// Token: 0x040008FC RID: 2300
	public Vector3 SecondarySize = Vector3.one;

	// Token: 0x040008FD RID: 2301
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> buffActions;

	// Token: 0x040008FE RID: 2302
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> buffSecondaryActions;

	// Token: 0x040008FF RID: 2303
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropDamageOffset = "DamageOffset";

	// Token: 0x04000900 RID: 2304
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropDamageSize = "DamageSize";

	// Token: 0x04000901 RID: 2305
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropDamageBuffs = "DamageBuffs";

	// Token: 0x04000902 RID: 2306
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropSecondaryOffset = "SecondaryOffset";

	// Token: 0x04000903 RID: 2307
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropSecondarySize = "SecondarySize";

	// Token: 0x04000904 RID: 2308
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropSecondaryBuffs = "SecondaryBuffs";

	// Token: 0x04000905 RID: 2309
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropStartSound = "StartSound";

	// Token: 0x04000906 RID: 2310
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropStopSound = "StopSound";

	// Token: 0x04000907 RID: 2311
	[PublicizedFrom(EAccessModifier.Protected)]
	public string StartSound;

	// Token: 0x04000908 RID: 2312
	[PublicizedFrom(EAccessModifier.Protected)]
	public string StopSound;

	// Token: 0x04000909 RID: 2313
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("light", "electric_switch", true, false, null),
		new BlockActivationCommand("trigger", "wrench", true, false, null)
	};
}
