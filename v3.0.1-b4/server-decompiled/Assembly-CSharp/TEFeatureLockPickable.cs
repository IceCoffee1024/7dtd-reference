using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200017E RID: 382
[Preserve]
public class TEFeatureLockPickable : TEFeatureAbs, IFeatureTriggerCapability
{
	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x06000B5B RID: 2907 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public IFeatureTriggerCapability.ETriggerRole TriggerRole
	{
		get
		{
			return IFeatureTriggerCapability.ETriggerRole.TriggeredBy;
		}
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x0004850C File Offset: 0x0004670C
	public override void Init(TileEntityComposite _parent, TileEntityFeatureData _featureData)
	{
		base.Init(_parent, _featureData);
		DynamicProperties props = _featureData.Props;
		if (!props.Values.ContainsKey(TEFeatureLockPickable.PropLockPickItem))
		{
			Log.Error(string.Concat(new string[]
			{
				"Block with name ",
				base.Parent.TeData.Block.GetBlockName(),
				" does not have a ",
				TEFeatureLockPickable.PropLockPickItem,
				" property for LockPickable feature"
			}));
		}
		props.ParseString(TEFeatureLockPickable.PropLockPickItem, ref this.lockPickItem);
		props.ParseFloat(TEFeatureLockPickable.PropLockPickTime, ref this.lockPickTime);
		props.ParseFloat(TEFeatureLockPickable.PropLockPickBreakChance, ref this.lockPickBreakChance);
		props.ParseString(TEFeatureLockPickable.PropOnLockPickSuccessEvent, ref this.lockPickSuccessEvent);
		props.ParseString(TEFeatureLockPickable.PropOnLockPickFailedEvent, ref this.lockPickFailedEvent);
		if (props.Values.ContainsKey(TEFeatureLockPickable.PropLockpickDowngradeBlock))
		{
			string text = props.Values[TEFeatureLockPickable.PropLockpickDowngradeBlock];
			if (!string.IsNullOrEmpty(text))
			{
				this.lockpickDowngradeBlock = Block.GetBlockValue(text, false);
				if (this.lockpickDowngradeBlock.isair)
				{
					throw new Exception("Block with name '" + text + "' not found in block " + base.Parent.TeData.Block.GetBlockName());
				}
			}
		}
		else
		{
			this.lockpickDowngradeBlock = base.Parent.TeData.Block.DowngradeBlock;
		}
		if (this.lockPickTime > 0f)
		{
			this.unlockCompletion = 0f;
			return;
		}
		this.unlockCompletion = 1f;
	}

	// Token: 0x06000B5D RID: 2909 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyFromInternal(TileEntityComposite _other)
	{
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x00045BE2 File Offset: 0x00043DE2
	public override void Read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode)
	{
		base.Read(_br, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeRead.Persistency)
		{
			if (base.Parent.UseLocalVersioning())
			{
				_br.ReadUInt16();
				return;
			}
			base.Parent.GetLegacyForkVersion();
		}
	}

	// Token: 0x06000B5F RID: 2911 RVA: 0x00045C10 File Offset: 0x00043E10
	public override void Write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
	{
		base.Write(_bw, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x00048686 File Offset: 0x00046886
	public bool NeedsLockpicking()
	{
		return this.unlockCompletion < 1f;
	}

	// Token: 0x06000B61 RID: 2913 RVA: 0x00048695 File Offset: 0x00046895
	public override bool CanLockLocally(ILockContext _context, ushort _channel)
	{
		return LocalPlayerUI.GetUIForPrimaryPlayer() != null;
	}

	// Token: 0x06000B62 RID: 2914 RVA: 0x000486A2 File Offset: 0x000468A2
	public override void OnLockedLocal(bool _success, ILockContext _context, ushort _channel)
	{
		this.ShowUI(_success);
	}

	// Token: 0x06000B63 RID: 2915 RVA: 0x000486AC File Offset: 0x000468AC
	public void ShowUI(bool _lockGranted)
	{
		TEFeatureLockPickable.<>c__DisplayClass25_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		if (!_lockGranted)
		{
			GameManager.ShowTooltip(uiforPrimaryPlayer.entityPlayer, Localization.Get("ttNoInteractItem", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			return;
		}
		CS$<>8__locals1.player = uiforPrimaryPlayer.entityPlayer;
		if (this.nthPercentile < 0f)
		{
			this.nthPercentile = CS$<>8__locals1.player.rand.RandomRange(0f, 0.999999f);
			this.lockPicksUsed = 0;
			this.unlockCompletion = 0f;
		}
		float num = -1f;
		float value = EffectManager.GetValue(PassiveEffects.LockPickTime, CS$<>8__locals1.player.inventory.holdingItemItemValue, this.lockPickTime, CS$<>8__locals1.player, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		float num2 = value * (1f - this.unlockCompletion);
		float value2 = EffectManager.GetValue(PassiveEffects.LockPickBreakChance, CS$<>8__locals1.player.inventory.holdingItemItemValue, this.lockPickBreakChance, CS$<>8__locals1.player, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		int num3 = this.<ShowUI>g__CalculateLockpicksNeeded|25_0(1f - value2, this.nthPercentile, 15, ref CS$<>8__locals1) - this.lockPicksUsed;
		if (num3 > 1)
		{
			num = value - num2 + this.<ShowUI>g__RandomizeNextBreak|25_1(num3 - 1, ref CS$<>8__locals1) * num2;
		}
		TimerEventData timerEventData = new TimerEventData();
		timerEventData.CloseEvent += this.EventData_CloseEvent;
		timerEventData.Data = CS$<>8__locals1.player;
		timerEventData.FullTimeFinishEvent += this.EventData_Event;
		timerEventData.AlternateTime = num;
		timerEventData.AlternateEvent += this.EventData_BreakEvent;
		XUiC_Timer.OpenTimer(uiforPrimaryPlayer.xui, value, timerEventData, num2, "", true);
		Manager.BroadcastPlayByLocalPlayer(base.ToWorldPos().ToVector3() + Vector3.one * 0.5f, "Misc/unlocking");
	}

	// Token: 0x06000B64 RID: 2916 RVA: 0x00048898 File Offset: 0x00046A98
	[PublicizedFrom(EAccessModifier.Private)]
	public void EventData_CloseEvent(TimerEventData _timerData)
	{
		EntityPlayerLocal entityPlayerLocal = (EntityPlayerLocal)_timerData.Data;
		Manager.BroadcastPlayByLocalPlayer(base.ToWorldPos().ToVector3() + Vector3.one * 0.5f, "Misc/locked");
		LockManager.Instance.UnlockRequestLocal();
	}

	// Token: 0x06000B65 RID: 2917 RVA: 0x000488E8 File Offset: 0x00046AE8
	[PublicizedFrom(EAccessModifier.Private)]
	public void EventData_BreakEvent(TimerEventData _timerData)
	{
		EntityPlayerLocal entityPlayerLocal = (EntityPlayerLocal)_timerData.Data;
		Vector3i vector3i = base.ToWorldPos();
		ItemValue item = ItemClass.GetItem(this.lockPickItem, false);
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(entityPlayerLocal);
		Manager.BroadcastPlayByLocalPlayer(vector3i.ToVector3() + Vector3.one * 0.5f, "Misc/locked");
		ItemStack itemStack = new ItemStack(item, 1);
		uiforPlayer.xui.PlayerInventory.RemoveItem(itemStack);
		GameManager.ShowTooltip(entityPlayerLocal, Localization.Get("ttLockpickBroken", false, null), false, false, 0f);
		uiforPlayer.xui.CollectedItemList.RemoveItemStack(itemStack);
		this.unlockCompletion = Utils.FastMax(this.unlockCompletion, _timerData.Completion);
		this.lockPicksUsed++;
		if (this.lockPickFailedEvent != null)
		{
			GameEventManager.Current.HandleAction(this.lockPickFailedEvent, null, entityPlayerLocal, false, vector3i, "", "", false, true, "", null, null);
		}
		LockManager.Instance.UnlockRequestLocal();
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x000489E8 File Offset: 0x00046BE8
	[PublicizedFrom(EAccessModifier.Private)]
	public void EventData_Event(TimerEventData _timerData)
	{
		EntityPlayerLocal entityPlayerLocal = (EntityPlayerLocal)_timerData.Data;
		Vector3i vector3i = base.ToWorldPos();
		LockManager.Instance.UnlockRequestLocal();
		this.unlockCompletion = 1f;
		this.lockPicksUsed++;
		this.DowngradeToUnlockedVariant(vector3i);
		Manager.BroadcastPlayByLocalPlayer(vector3i.ToVector3() + Vector3.one * 0.5f, "Misc/unlocking");
		if (this.lockPickSuccessEvent != null)
		{
			GameEventManager.Current.HandleAction(this.lockPickSuccessEvent, null, entityPlayerLocal, false, vector3i, "", "", false, true, "", null, null);
		}
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x00048A8C File Offset: 0x00046C8C
	[PublicizedFrom(EAccessModifier.Private)]
	public void DowngradeToUnlockedVariant(Vector3i _blockPos)
	{
		if (this.lockpickDowngradeBlock.isair)
		{
			return;
		}
		World world = GameManager.Instance.World;
		BlockValue block = world.GetBlock(_blockPos);
		BlockValue blockValue = this.lockpickDowngradeBlock;
		blockValue = BlockPlaceholderMap.Instance.Replace(blockValue, world.GetGameRandom(), _blockPos.x, _blockPos.z, false);
		blockValue.rotation = block.rotation;
		blockValue.meta = block.meta;
		world.SetBlockRPC(_blockPos, blockValue, blockValue.Block.Density);
	}

	// Token: 0x06000B68 RID: 2920 RVA: 0x00048B15 File Offset: 0x00046D15
	public override string GetActivationText(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing, string _activateHotkeyMarkup, string _focusedTileEntityName)
	{
		base.GetActivationText(_world, _blockPos, _blockValue, _entityFocusing, _activateHotkeyMarkup, _focusedTileEntityName);
		if (this.unlockCompletion < 1f)
		{
			return string.Format(Localization.Get("tooltipLocked", false, null), _activateHotkeyMarkup, _focusedTileEntityName);
		}
		return null;
	}

	// Token: 0x06000B69 RID: 2921 RVA: 0x00048B4B File Offset: 0x00046D4B
	public override void InitBlockActivationCommands(Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> _addCallback)
	{
		base.InitBlockActivationCommands(_addCallback);
		_addCallback(new BlockActivationCommand("pick", "unlock", false, false, null), TileEntityComposite.EBlockCommandOrder.First, base.FeatureData);
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x00048B74 File Offset: 0x00046D74
	public override bool AllowBlockActivationCommand(ITileEntityFeature _module, ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing)
	{
		if (!base.AllowBlockActivationCommand(_module, _commandName, _world, _blockPos, _blockValue, _entityFocusing))
		{
			return false;
		}
		if (!this.Equals(_module))
		{
			return this.unlockCompletion == 1f || base.Parent.LocalPlayerIsOwner;
		}
		return !base.CommandIs(_commandName, "pick") || !base.Parent.LocalPlayerIsOwner;
	}

	// Token: 0x06000B6B RID: 2923 RVA: 0x00048BD6 File Offset: 0x00046DD6
	public void OnBlockTriggered(EntityPlayer _player, Vector3i _blockPos, BlockValue _blockValue, List<BlockChangeInfo> _blockChanges, BlockTrigger _triggeredBy)
	{
		if (_triggeredBy == null)
		{
			return;
		}
		if (_triggeredBy.Unlock)
		{
			this.DowngradeToUnlockedVariant(_blockPos);
		}
	}

	// Token: 0x06000B6C RID: 2924 RVA: 0x00048BF0 File Offset: 0x00046DF0
	public override bool OnBlockActivated(ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		base.OnBlockActivated(_commandName, _world, _blockPos, _blockValue, _player);
		if (!base.CommandIs(_commandName, "pick"))
		{
			return false;
		}
		LocalPlayerUI playerUI = _player.PlayerUI;
		ItemValue item = ItemClass.GetItem(this.lockPickItem, false);
		if (playerUI.xui.PlayerInventory.GetItemCount(item) == 0)
		{
			playerUI.xui.CollectedItemList.AddItemStack(new ItemStack(item, 0), true);
			GameManager.ShowTooltip(_player, Localization.Get("ttLockpickMissing", false, null), false, false, 0f);
			return true;
		}
		_player.AimingGun = false;
		LockManager.Instance.LockRequestLocal(this, null, 0);
		return true;
	}

	// Token: 0x06000B6F RID: 2927 RVA: 0x00048CF0 File Offset: 0x00046EF0
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public int <ShowUI>g__CalculateLockpicksNeeded|25_0(float _lpSuccess, float _nthPercentile, int _maxTries = 15, ref TEFeatureLockPickable.<>c__DisplayClass25_0 A_4)
	{
		if (_lpSuccess <= 0f || _lpSuccess > 1f || _maxTries <= 0)
		{
			return 1;
		}
		float num = 1f - _lpSuccess;
		return Utils.FastMin(Utils.Fastfloor(Math.Log(1.0 - (double)this.nthPercentile) / Math.Log((double)num)) + 1, _maxTries);
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x00048D48 File Offset: 0x00046F48
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public float <ShowUI>g__RandomizeNextBreak|25_1(int _breaksRemaining, ref TEFeatureLockPickable.<>c__DisplayClass25_0 A_2)
	{
		if (_breaksRemaining <= 0)
		{
			return -1f;
		}
		float num = 1f;
		for (int i = 0; i < _breaksRemaining; i++)
		{
			float num2 = A_2.player.rand.RandomRange(0f, 1f);
			if (num2 < num)
			{
				num = num2;
			}
		}
		return Utils.FastClamp(num, 0.05f, 0.95f);
	}

	// Token: 0x04000A7D RID: 2685
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 18;

	// Token: 0x04000A7E RID: 2686
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropLockPickTime = "LockPickTime";

	// Token: 0x04000A7F RID: 2687
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropLockPickItem = "LockPickItem";

	// Token: 0x04000A80 RID: 2688
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropLockPickBreakChance = "LockPickBreakChance";

	// Token: 0x04000A81 RID: 2689
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropOnLockPickSuccessEvent = "LockPickSuccessEvent";

	// Token: 0x04000A82 RID: 2690
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropOnLockPickFailedEvent = "LockPickFailedEvent";

	// Token: 0x04000A83 RID: 2691
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropLockpickDowngradeBlock = "LockPickDowngradeBlock";

	// Token: 0x04000A84 RID: 2692
	[PublicizedFrom(EAccessModifier.Private)]
	public string lockPickItem;

	// Token: 0x04000A85 RID: 2693
	[PublicizedFrom(EAccessModifier.Private)]
	public float lockPickTime;

	// Token: 0x04000A86 RID: 2694
	[PublicizedFrom(EAccessModifier.Private)]
	public float lockPickBreakChance;

	// Token: 0x04000A87 RID: 2695
	[PublicizedFrom(EAccessModifier.Private)]
	public string lockPickSuccessEvent;

	// Token: 0x04000A88 RID: 2696
	[PublicizedFrom(EAccessModifier.Private)]
	public string lockPickFailedEvent;

	// Token: 0x04000A89 RID: 2697
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue lockpickDowngradeBlock;

	// Token: 0x04000A8A RID: 2698
	[PublicizedFrom(EAccessModifier.Private)]
	public float unlockCompletion = -1f;

	// Token: 0x04000A8B RID: 2699
	[PublicizedFrom(EAccessModifier.Private)]
	public int lockPicksUsed = -1;

	// Token: 0x04000A8C RID: 2700
	[PublicizedFrom(EAccessModifier.Private)]
	public float nthPercentile = -1f;
}
