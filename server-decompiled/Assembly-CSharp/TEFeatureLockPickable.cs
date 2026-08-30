using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000180 RID: 384
[Preserve]
public class TEFeatureLockPickable : TEFeatureAbs, IFeatureTriggerCapability
{
	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x06000B66 RID: 2918 RVA: 0x00020021 File Offset: 0x0001E221
	public IFeatureTriggerCapability.ETriggerRole TriggerRole
	{
		get
		{
			return IFeatureTriggerCapability.ETriggerRole.TriggeredBy;
		}
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x00048924 File Offset: 0x00046B24
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

	// Token: 0x06000B68 RID: 2920 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyFromInternal(TileEntityComposite _other)
	{
	}

	// Token: 0x06000B69 RID: 2921 RVA: 0x00045E42 File Offset: 0x00044042
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

	// Token: 0x06000B6A RID: 2922 RVA: 0x00045E70 File Offset: 0x00044070
	public override void Write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
	{
		base.Write(_bw, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
	}

	// Token: 0x06000B6B RID: 2923 RVA: 0x00048A9E File Offset: 0x00046C9E
	public bool NeedsLockpicking()
	{
		return this.unlockCompletion < 1f;
	}

	// Token: 0x06000B6C RID: 2924 RVA: 0x00048AAD File Offset: 0x00046CAD
	public override bool CanLockLocally(ILockContext _context, ushort _channel)
	{
		return LocalPlayerUI.GetUIForPrimaryPlayer() != null;
	}

	// Token: 0x06000B6D RID: 2925 RVA: 0x00048ABA File Offset: 0x00046CBA
	public override void OnLockedLocal(bool _success, ILockContext _context, ushort _channel)
	{
		this.ShowUI(_success);
	}

	// Token: 0x06000B6E RID: 2926 RVA: 0x00048AC4 File Offset: 0x00046CC4
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

	// Token: 0x06000B6F RID: 2927 RVA: 0x00048CB0 File Offset: 0x00046EB0
	[PublicizedFrom(EAccessModifier.Private)]
	public void EventData_CloseEvent(TimerEventData _timerData)
	{
		EntityPlayerLocal entityPlayerLocal = (EntityPlayerLocal)_timerData.Data;
		Manager.BroadcastPlayByLocalPlayer(base.ToWorldPos().ToVector3() + Vector3.one * 0.5f, "Misc/locked");
		LockManager.Instance.UnlockRequestLocal();
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x00048D00 File Offset: 0x00046F00
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

	// Token: 0x06000B71 RID: 2929 RVA: 0x00048E00 File Offset: 0x00047000
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

	// Token: 0x06000B72 RID: 2930 RVA: 0x00048EA4 File Offset: 0x000470A4
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

	// Token: 0x06000B73 RID: 2931 RVA: 0x00048F2D File Offset: 0x0004712D
	public override string GetActivationText(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing, string _activateHotkeyMarkup, string _focusedTileEntityName)
	{
		base.GetActivationText(_world, _blockPos, _blockValue, _entityFocusing, _activateHotkeyMarkup, _focusedTileEntityName);
		if (this.unlockCompletion < 1f)
		{
			return string.Format(Localization.Get("tooltipLocked", false, null), _activateHotkeyMarkup, _focusedTileEntityName);
		}
		return null;
	}

	// Token: 0x06000B74 RID: 2932 RVA: 0x00048F63 File Offset: 0x00047163
	public override void InitBlockActivationCommands(Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> _addCallback)
	{
		base.InitBlockActivationCommands(_addCallback);
		_addCallback(new BlockActivationCommand("pick", "unlock", false, false, null), TileEntityComposite.EBlockCommandOrder.First, base.FeatureData);
	}

	// Token: 0x06000B75 RID: 2933 RVA: 0x00048F8C File Offset: 0x0004718C
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

	// Token: 0x06000B76 RID: 2934 RVA: 0x00048FEE File Offset: 0x000471EE
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

	// Token: 0x06000B77 RID: 2935 RVA: 0x00049008 File Offset: 0x00047208
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

	// Token: 0x06000B7A RID: 2938 RVA: 0x00049108 File Offset: 0x00047308
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

	// Token: 0x06000B7B RID: 2939 RVA: 0x00049160 File Offset: 0x00047360
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

	// Token: 0x04000A8A RID: 2698
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 18;

	// Token: 0x04000A8B RID: 2699
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropLockPickTime = "LockPickTime";

	// Token: 0x04000A8C RID: 2700
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropLockPickItem = "LockPickItem";

	// Token: 0x04000A8D RID: 2701
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropLockPickBreakChance = "LockPickBreakChance";

	// Token: 0x04000A8E RID: 2702
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropOnLockPickSuccessEvent = "LockPickSuccessEvent";

	// Token: 0x04000A8F RID: 2703
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropOnLockPickFailedEvent = "LockPickFailedEvent";

	// Token: 0x04000A90 RID: 2704
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropLockpickDowngradeBlock = "LockPickDowngradeBlock";

	// Token: 0x04000A91 RID: 2705
	[PublicizedFrom(EAccessModifier.Private)]
	public string lockPickItem;

	// Token: 0x04000A92 RID: 2706
	[PublicizedFrom(EAccessModifier.Private)]
	public float lockPickTime;

	// Token: 0x04000A93 RID: 2707
	[PublicizedFrom(EAccessModifier.Private)]
	public float lockPickBreakChance;

	// Token: 0x04000A94 RID: 2708
	[PublicizedFrom(EAccessModifier.Private)]
	public string lockPickSuccessEvent;

	// Token: 0x04000A95 RID: 2709
	[PublicizedFrom(EAccessModifier.Private)]
	public string lockPickFailedEvent;

	// Token: 0x04000A96 RID: 2710
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue lockpickDowngradeBlock;

	// Token: 0x04000A97 RID: 2711
	[PublicizedFrom(EAccessModifier.Private)]
	public float unlockCompletion = -1f;

	// Token: 0x04000A98 RID: 2712
	[PublicizedFrom(EAccessModifier.Private)]
	public int lockPicksUsed = -1;

	// Token: 0x04000A99 RID: 2713
	[PublicizedFrom(EAccessModifier.Private)]
	public float nthPercentile = -1f;
}
