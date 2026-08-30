using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000178 RID: 376
[Preserve]
public class TEFeatureCombine : TEFeatureAbs, IFeatureSavedInPrefab
{
	// Token: 0x06000AED RID: 2797 RVA: 0x00046A18 File Offset: 0x00044C18
	public override void Init(TileEntityComposite _parent, TileEntityFeatureData _featureData)
	{
		base.Init(_parent, _featureData);
		DynamicProperties props = _featureData.Props;
		props.ParseString(TEFeatureCombine.PropOpenSound, ref this.openSound);
		props.ParseString(TEFeatureCombine.PropCloseSound, ref this.closeSound);
		props.ParseString(TEFeatureCombine.PropCombineCompleteSound, ref this.combineCompleteSound);
	}

	// Token: 0x06000AEE RID: 2798 RVA: 0x00046A65 File Offset: 0x00044C65
	public override string GetActivationText(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing, string _activateHotkeyMarkup, string _focusedTileEntityName)
	{
		base.GetActivationText(_world, _blockPos, _blockValue, _entityFocusing, _activateHotkeyMarkup, _focusedTileEntityName);
		return _blockValue.Block.GetLocalizedBlockName() + "\n" + Localization.Get("useWorkstation", false, null);
	}

	// Token: 0x06000AEF RID: 2799 RVA: 0x00046A99 File Offset: 0x00044C99
	public override void InitBlockActivationCommands(Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> _addCallback)
	{
		base.InitBlockActivationCommands(_addCallback);
		_addCallback(new BlockActivationCommand("Open", "campfire", false, false, null), TileEntityComposite.EBlockCommandOrder.Normal, base.FeatureData);
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x00046AC1 File Offset: 0x00044CC1
	public override bool AllowBlockActivationCommand(ITileEntityFeature _module, ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing)
	{
		if (!base.AllowBlockActivationCommand(_module, _commandName, _world, _blockPos, _blockValue, _entityFocusing))
		{
			return false;
		}
		this.Equals(_module);
		return true;
	}

	// Token: 0x06000AF1 RID: 2801 RVA: 0x00046ADF File Offset: 0x00044CDF
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

	// Token: 0x06000AF2 RID: 2802 RVA: 0x00046B15 File Offset: 0x00044D15
	public override void OnLockedLocal(bool _success, ILockContext _context, ushort _channel)
	{
		this.ShowUI(_success);
	}

	// Token: 0x06000AF3 RID: 2803 RVA: 0x00046B20 File Offset: 0x00044D20
	public override void OnUnlockedServer(int _unlockingPlayerID, ushort _channel)
	{
		base.OnUnlockedServer(_unlockingPlayerID, _channel);
		Manager.BroadcastPlayByLocalPlayer(base.ToWorldPos().ToVector3() + Vector3.one * 0.5f, this.closeSound);
	}

	// Token: 0x06000AF4 RID: 2804 RVA: 0x00046B64 File Offset: 0x00044D64
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

	// Token: 0x06000AF5 RID: 2805 RVA: 0x00046C08 File Offset: 0x00044E08
	public void HandlePlayComplete()
	{
		Manager.BroadcastPlayByLocalPlayer(base.ToWorldPos().ToVector3() + Vector3.one * 0.5f, this.combineCompleteSound);
	}

	// Token: 0x06000AF6 RID: 2806 RVA: 0x00046C44 File Offset: 0x00044E44
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyFromInternal(TileEntityComposite _other)
	{
		TEFeatureCombine tefeatureCombine;
		_other.TryGetSelfOrFeature<TEFeatureCombine>(out tefeatureCombine);
	}

	// Token: 0x04000A58 RID: 2648
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 1;

	// Token: 0x04000A59 RID: 2649
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropOpenSound = "OpenSound";

	// Token: 0x04000A5A RID: 2650
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropCloseSound = "CloseSound";

	// Token: 0x04000A5B RID: 2651
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropCombineCompleteSound = "CombineCompleteSound";

	// Token: 0x04000A5C RID: 2652
	[PublicizedFrom(EAccessModifier.Protected)]
	public string openSound;

	// Token: 0x04000A5D RID: 2653
	[PublicizedFrom(EAccessModifier.Protected)]
	public string closeSound;

	// Token: 0x04000A5E RID: 2654
	[PublicizedFrom(EAccessModifier.Protected)]
	public string combineCompleteSound;
}
