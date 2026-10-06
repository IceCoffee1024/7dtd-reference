using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200017A RID: 378
[Preserve]
public class TEFeatureCombine : TEFeatureAbs, IFeatureSavedInPrefab
{
	// Token: 0x06000B00 RID: 2816 RVA: 0x00046E5C File Offset: 0x0004505C
	public override void Init(TileEntityComposite _parent, TileEntityFeatureData _featureData)
	{
		base.Init(_parent, _featureData);
		DynamicProperties props = _featureData.Props;
		props.ParseString(TEFeatureCombine.PropOpenSound, ref this.openSound);
		props.ParseString(TEFeatureCombine.PropCloseSound, ref this.closeSound);
		props.ParseString(TEFeatureCombine.PropCombineCompleteSound, ref this.combineCompleteSound);
	}

	// Token: 0x06000B01 RID: 2817 RVA: 0x00046EA9 File Offset: 0x000450A9
	public override string GetActivationText(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing, string _activateHotkeyMarkup, string _focusedTileEntityName)
	{
		base.GetActivationText(_world, _blockPos, _blockValue, _entityFocusing, _activateHotkeyMarkup, _focusedTileEntityName);
		return _blockValue.Block.GetLocalizedBlockName() + "\n" + Localization.Get("useWorkstation", false, null);
	}

	// Token: 0x06000B02 RID: 2818 RVA: 0x00046EDD File Offset: 0x000450DD
	public override void InitBlockActivationCommands(Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> _addCallback)
	{
		base.InitBlockActivationCommands(_addCallback);
		_addCallback(new BlockActivationCommand("Open", "campfire", false, false, null), TileEntityComposite.EBlockCommandOrder.Normal, base.FeatureData);
	}

	// Token: 0x06000B03 RID: 2819 RVA: 0x00046F05 File Offset: 0x00045105
	public override bool AllowBlockActivationCommand(ITileEntityFeature _module, ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing)
	{
		if (!base.AllowBlockActivationCommand(_module, _commandName, _world, _blockPos, _blockValue, _entityFocusing))
		{
			return false;
		}
		this.Equals(_module);
		return true;
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x00046F23 File Offset: 0x00045123
	public override bool OnBlockActivated(ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		base.OnBlockActivated(_commandName, _world, _blockPos, _blockValue, _player);
		if (base.CommandIs(_commandName, "Open"))
		{
			_player.AimingGun = false;
			LockManager.Instance.LockRequestLocal(this, 0);
			return true;
		}
		return false;
	}

	// Token: 0x06000B05 RID: 2821 RVA: 0x00046F58 File Offset: 0x00045158
	public override bool OnLockResponseLocal(bool _success, PooledBinaryReader _brServerContext, ushort _channel)
	{
		this.ShowUI(_success);
		return true;
	}

	// Token: 0x06000B06 RID: 2822 RVA: 0x00046F64 File Offset: 0x00045164
	public override void OnUnlockedServer(int _unlockingPlayerID, ushort _channel)
	{
		Manager.BroadcastPlayByLocalPlayer(base.ToWorldPos().ToVector3() + Vector3.one * 0.5f, this.closeSound);
	}

	// Token: 0x06000B07 RID: 2823 RVA: 0x00046FA0 File Offset: 0x000451A0
	[PublicizedFrom(EAccessModifier.Private)]
	public void ShowUI(bool _lockGranted)
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		if (!_lockGranted)
		{
			GameManager.ShowTooltip(uiforPrimaryPlayer.entityPlayer, Localization.Get("ttNoInteractItem", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			return;
		}
		((XUiC_CombineWindowGroup)((XUiWindowGroup)uiforPrimaryPlayer.windowManager.GetWindow("combine")).Controller).SetTileEntity(this);
		uiforPrimaryPlayer.windowManager.Open("combine", true);
		Manager.BroadcastPlayByLocalPlayer(base.ToWorldPos().ToVector3() + Vector3.one * 0.5f, this.openSound);
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x00047044 File Offset: 0x00045244
	public void HandlePlayComplete()
	{
		Manager.BroadcastPlayByLocalPlayer(base.ToWorldPos().ToVector3() + Vector3.one * 0.5f, this.combineCompleteSound);
	}

	// Token: 0x06000B09 RID: 2825 RVA: 0x00047080 File Offset: 0x00045280
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyFromInternal(TileEntityComposite _other)
	{
		TEFeatureCombine tefeatureCombine;
		_other.TryGetSelfOrFeature<TEFeatureCombine>(out tefeatureCombine);
	}

	// Token: 0x04000A59 RID: 2649
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 1;

	// Token: 0x04000A5A RID: 2650
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropOpenSound = "OpenSound";

	// Token: 0x04000A5B RID: 2651
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropCloseSound = "CloseSound";

	// Token: 0x04000A5C RID: 2652
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropCombineCompleteSound = "CombineCompleteSound";

	// Token: 0x04000A5D RID: 2653
	[PublicizedFrom(EAccessModifier.Protected)]
	public string openSound;

	// Token: 0x04000A5E RID: 2654
	[PublicizedFrom(EAccessModifier.Protected)]
	public string closeSound;

	// Token: 0x04000A5F RID: 2655
	[PublicizedFrom(EAccessModifier.Protected)]
	public string combineCompleteSound;
}
