using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000179 RID: 377
[Preserve]
public class TEFeatureCombine : TEFeatureAbs, IFeatureSavedInPrefab
{
	// Token: 0x06000AF7 RID: 2807 RVA: 0x00046C78 File Offset: 0x00044E78
	public override void Init(TileEntityComposite _parent, TileEntityFeatureData _featureData)
	{
		base.Init(_parent, _featureData);
		DynamicProperties props = _featureData.Props;
		props.ParseString(TEFeatureCombine.PropOpenSound, ref this.openSound);
		props.ParseString(TEFeatureCombine.PropCloseSound, ref this.closeSound);
		props.ParseString(TEFeatureCombine.PropCombineCompleteSound, ref this.combineCompleteSound);
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x00046CC5 File Offset: 0x00044EC5
	public override string GetActivationText(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing, string _activateHotkeyMarkup, string _focusedTileEntityName)
	{
		base.GetActivationText(_world, _blockPos, _blockValue, _entityFocusing, _activateHotkeyMarkup, _focusedTileEntityName);
		return _blockValue.Block.GetLocalizedBlockName() + "\n" + Localization.Get("useWorkstation", false, null);
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x00046CF9 File Offset: 0x00044EF9
	public override void InitBlockActivationCommands(Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> _addCallback)
	{
		base.InitBlockActivationCommands(_addCallback);
		_addCallback(new BlockActivationCommand("Open", "campfire", false, false, null), TileEntityComposite.EBlockCommandOrder.Normal, base.FeatureData);
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x00046D21 File Offset: 0x00044F21
	public override bool AllowBlockActivationCommand(ITileEntityFeature _module, ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing)
	{
		if (!base.AllowBlockActivationCommand(_module, _commandName, _world, _blockPos, _blockValue, _entityFocusing))
		{
			return false;
		}
		this.Equals(_module);
		return true;
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x00046D3F File Offset: 0x00044F3F
	public override bool OnBlockActivated(ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		base.OnBlockActivated(_commandName, _world, _blockPos, _blockValue, _player);
		if (base.CommandIs(_commandName, "Open"))
		{
			_player.AimingGun = false;
			LockManager.Instance.LockRequestLocal(this, null, 0);
			return true;
		}
		return false;
	}

	// Token: 0x06000AFC RID: 2812 RVA: 0x00046D75 File Offset: 0x00044F75
	public override void OnLockedLocal(bool _success, ILockContext _context, ushort _channel)
	{
		this.ShowUI(_success);
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x00046D80 File Offset: 0x00044F80
	public override void OnUnlockedServer(int _unlockingPlayerID, ushort _channel)
	{
		base.OnUnlockedServer(_unlockingPlayerID, _channel);
		Manager.BroadcastPlayByLocalPlayer(base.ToWorldPos().ToVector3() + Vector3.one * 0.5f, this.closeSound);
	}

	// Token: 0x06000AFE RID: 2814 RVA: 0x00046DC4 File Offset: 0x00044FC4
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

	// Token: 0x06000AFF RID: 2815 RVA: 0x00046E68 File Offset: 0x00045068
	public void HandlePlayComplete()
	{
		Manager.BroadcastPlayByLocalPlayer(base.ToWorldPos().ToVector3() + Vector3.one * 0.5f, this.combineCompleteSound);
	}

	// Token: 0x06000B00 RID: 2816 RVA: 0x00046EA4 File Offset: 0x000450A4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyFromInternal(TileEntityComposite _other)
	{
		TEFeatureCombine tefeatureCombine;
		_other.TryGetSelfOrFeature<TEFeatureCombine>(out tefeatureCombine);
	}

	// Token: 0x04000A5A RID: 2650
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 1;

	// Token: 0x04000A5B RID: 2651
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropOpenSound = "OpenSound";

	// Token: 0x04000A5C RID: 2652
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropCloseSound = "CloseSound";

	// Token: 0x04000A5D RID: 2653
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropCombineCompleteSound = "CombineCompleteSound";

	// Token: 0x04000A5E RID: 2654
	[PublicizedFrom(EAccessModifier.Protected)]
	public string openSound;

	// Token: 0x04000A5F RID: 2655
	[PublicizedFrom(EAccessModifier.Protected)]
	public string closeSound;

	// Token: 0x04000A60 RID: 2656
	[PublicizedFrom(EAccessModifier.Protected)]
	public string combineCompleteSound;
}
