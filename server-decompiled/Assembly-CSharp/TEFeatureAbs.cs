using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000172 RID: 370
[Preserve]
public abstract class TEFeatureAbs : ITileEntityFeature, ITileEntity, ILockTarget, IHasWorldAddress
{
	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x06000A93 RID: 2707 RVA: 0x000459F1 File Offset: 0x00043BF1
	// (set) Token: 0x06000A94 RID: 2708 RVA: 0x000459F9 File Offset: 0x00043BF9
	public TileEntityFeatureData FeatureData
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x06000A95 RID: 2709 RVA: 0x00045A02 File Offset: 0x00043C02
	// (set) Token: 0x06000A96 RID: 2710 RVA: 0x00045A0A File Offset: 0x00043C0A
	public TileEntityComposite Parent
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x00045A13 File Offset: 0x00043C13
	public virtual void Init(TileEntityComposite _parent, TileEntityFeatureData _featureData)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.Init (on " + base.GetType().Name + ")");
		}
		this.Parent = _parent;
		this.FeatureData = _featureData;
	}

	// Token: 0x06000A98 RID: 2712
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void CopyFromInternal(TileEntityComposite _other);

	// Token: 0x06000A99 RID: 2713 RVA: 0x00045A49 File Offset: 0x00043C49
	public void CopyFrom(TileEntityComposite _other)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.CopyFrom (on " + base.GetType().Name + ")");
		}
		this.CopyFromInternal(_other);
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x00045A78 File Offset: 0x00043C78
	public virtual void OnRemove(World _world)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.OnRemove (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x00045AA0 File Offset: 0x00043CA0
	public virtual void OnLoad()
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.OnLoad (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000A9C RID: 2716 RVA: 0x00045AC8 File Offset: 0x00043CC8
	public virtual void OnUnload(World _world)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.OnUnload (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x00045AF0 File Offset: 0x00043CF0
	public virtual void OnDestroy()
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.OnDestroy (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x00045B18 File Offset: 0x00043D18
	public virtual void OnAdded(Vector3i _blockPos, BlockValue _blockValue)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.OnAdded (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000A9F RID: 2719 RVA: 0x00045B40 File Offset: 0x00043D40
	public virtual void OnBlockValueChanged(Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.OnBlockValueChanged (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x00045B68 File Offset: 0x00043D68
	public virtual void OnBlockReset(Vector3i _blockPos, BlockValue _blockValue)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.OnBlockReset (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x00045B90 File Offset: 0x00043D90
	public virtual void OnBlockStartsToFall(Vector3i _blockPos, BlockValue _blockValue)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.OnBlockStartsToFall (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x00045BB8 File Offset: 0x00043DB8
	public virtual Block.DestroyedResult OnBlockDestroyedBy(Vector3i _blockPos, BlockValue _blockValue, int _entityId, bool _bUseHarvestTool)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.OnBlockDestroyedBy (on " + base.GetType().Name + ")");
		}
		return Block.DestroyedResult.None;
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x00045BE1 File Offset: 0x00043DE1
	public virtual Block.DestroyedResult OnBlockDestroyedByExplosion(Vector3i _blockPos, BlockValue _blockValue, int _playerThatStartedExpl)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.OnBlockDestroyedByExplosion (on " + base.GetType().Name + ")");
		}
		return Block.DestroyedResult.None;
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x00045C0A File Offset: 0x00043E0A
	public virtual void SetBlockEntityData(BlockEntityData _blockEntityData)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.SetBlockEntityData (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000AA5 RID: 2725 RVA: 0x00045C32 File Offset: 0x00043E32
	public virtual void UpgradeDowngradeFrom(TileEntityComposite _other)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.UpgradeDowngradeFrom (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000AA6 RID: 2726 RVA: 0x00045C5A File Offset: 0x00043E5A
	public virtual void ReplacedBy(BlockValue _bvOld, BlockValue _bvNew, TileEntity _teNew)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.ReplacedBy (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x00045C82 File Offset: 0x00043E82
	public virtual void Reset(FastTags<TagGroup.Global> _questTags)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.Reset (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000AA8 RID: 2728 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void UpdateTick(World _world)
	{
	}

	// Token: 0x06000AA9 RID: 2729 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual string GetActivationText(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing, string _activateHotkeyMarkup, string _focusedTileEntityName)
	{
		return null;
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x00045CAA File Offset: 0x00043EAA
	public virtual void InitBlockActivationCommands(Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> _addCallback)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.InitBlockActivationCommands (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000AAB RID: 2731 RVA: 0x00045CD2 File Offset: 0x00043ED2
	public virtual bool AllowBlockActivationCommand(ITileEntityFeature _module, ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing)
	{
		this.Equals(_module);
		return true;
	}

	// Token: 0x06000AAC RID: 2732 RVA: 0x00045CE0 File Offset: 0x00043EE0
	public virtual bool OnBlockActivated(ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.OnBlockActivated (on " + base.GetType().Name + "), command " + _commandName.ToString());
		}
		if (this.CommandIs(_commandName, "trigger"))
		{
			XUiC_TriggerProperties.Show(_player.PlayerUI.xui, _blockPos, false, true);
			return true;
		}
		return false;
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x00045D45 File Offset: 0x00043F45
	public virtual void Read(PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.Read (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000AAE RID: 2734 RVA: 0x00045D6D File Offset: 0x00043F6D
	public virtual void Write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.Write (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000AAF RID: 2735 RVA: 0x00045D95 File Offset: 0x00043F95
	public virtual WorldAddress GetAddress()
	{
		Vector3i vector3i = this.ToWorldPos();
		TileEntityFeatureData featureData = this.FeatureData;
		return WorldAddress.CreateForTEFeature(vector3i, (featureData != null) ? featureData.Name : null);
	}

	// Token: 0x06000AB0 RID: 2736 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsSharedLock(ushort _channel)
	{
		return false;
	}

	// Token: 0x06000AB1 RID: 2737 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnLockRequestLocal(PooledBinaryWriter _bwLocalContext, ushort _channel)
	{
	}

	// Token: 0x06000AB2 RID: 2738 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool OnLockRequestServer(int _lockingPlayerID, PooledBinaryReader _brLocalContext, ushort _channel)
	{
		return true;
	}

	// Token: 0x06000AB3 RID: 2739 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnLockResponseServer(int _lockingPlayerID, PooledBinaryWriter _bwServerContext, ushort _channel)
	{
	}

	// Token: 0x06000AB4 RID: 2740 RVA: 0x00045DB4 File Offset: 0x00043FB4
	public virtual bool OnLockResponseLocal(bool _success, PooledBinaryReader _brServerContext, ushort _channel)
	{
		return LocalPlayerUI.GetUIForPrimaryPlayer() != null;
	}

	// Token: 0x06000AB5 RID: 2741 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnUnlockedServer(int _unlockingPlayerID, ushort _channel)
	{
	}

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x06000AB6 RID: 2742 RVA: 0x00045DC1 File Offset: 0x00043FC1
	// (remove) Token: 0x06000AB7 RID: 2743 RVA: 0x00045DCF File Offset: 0x00043FCF
	public event XUiEvent_TileEntityDestroyed Destroyed
	{
		add
		{
			this.Parent.Destroyed += value;
		}
		remove
		{
			this.Parent.Destroyed -= value;
		}
	}

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x00045DDD File Offset: 0x00043FDD
	public List<ITileEntityChangedListener> listeners
	{
		get
		{
			return this.Parent.listeners;
		}
	}

	// Token: 0x06000AB9 RID: 2745 RVA: 0x00045DEA File Offset: 0x00043FEA
	public void SetUserAccessing(bool _bUserAccessing)
	{
		this.Parent.SetUserAccessing(_bUserAccessing);
	}

	// Token: 0x06000ABA RID: 2746 RVA: 0x00045DF8 File Offset: 0x00043FF8
	public bool IsUserAccessing()
	{
		return this.Parent.IsUserAccessing();
	}

	// Token: 0x06000ABB RID: 2747 RVA: 0x00045E05 File Offset: 0x00044005
	public void SetModified()
	{
		this.Parent.SetModified();
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x00045E12 File Offset: 0x00044012
	public Chunk GetChunk()
	{
		return this.Parent.GetChunk();
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x00045E1F File Offset: 0x0004401F
	public Vector3i ToWorldPos()
	{
		return this.Parent.ToWorldPos();
	}

	// Token: 0x06000ABE RID: 2750 RVA: 0x00045E2C File Offset: 0x0004402C
	public Vector3 ToWorldCenterPos()
	{
		return this.Parent.ToWorldCenterPos();
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x06000ABF RID: 2751 RVA: 0x00045E39 File Offset: 0x00044039
	public BlockValue blockValue
	{
		get
		{
			return this.Parent.blockValue;
		}
	}

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x00045E46 File Offset: 0x00044046
	// (set) Token: 0x06000AC1 RID: 2753 RVA: 0x00045E53 File Offset: 0x00044053
	public virtual bool IsRemoving
	{
		get
		{
			return this.Parent.IsRemoving;
		}
		set
		{
			this.Parent.IsRemoving = value;
		}
	}

	// Token: 0x06000AC2 RID: 2754 RVA: 0x00045E61 File Offset: 0x00044061
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool CommandIs(ReadOnlySpan<char> _givenCommand, string _compareCommand)
	{
		return _givenCommand.Equals(_compareCommand, StringComparison.Ordinal);
	}

	// Token: 0x06000AC3 RID: 2755 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public TEFeatureAbs()
	{
	}

	// Token: 0x04000A2B RID: 2603
	public static bool DebugLogCTE;
}
