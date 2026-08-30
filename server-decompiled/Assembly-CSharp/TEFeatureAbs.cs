using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200016F RID: 367
[Preserve]
public abstract class TEFeatureAbs : ITileEntityFeature, ITileEntity, ILockTarget
{
	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x06000A83 RID: 2691 RVA: 0x000456C1 File Offset: 0x000438C1
	// (set) Token: 0x06000A84 RID: 2692 RVA: 0x000456C9 File Offset: 0x000438C9
	public TileEntityFeatureData FeatureData
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x06000A85 RID: 2693 RVA: 0x000456D2 File Offset: 0x000438D2
	// (set) Token: 0x06000A86 RID: 2694 RVA: 0x000456DA File Offset: 0x000438DA
	public TileEntityComposite Parent
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x000456E3 File Offset: 0x000438E3
	public virtual void Init(TileEntityComposite _parent, TileEntityFeatureData _featureData)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.Init (on " + base.GetType().Name + ")");
		}
		this.Parent = _parent;
		this.FeatureData = _featureData;
	}

	// Token: 0x06000A88 RID: 2696
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void CopyFromInternal(TileEntityComposite _other);

	// Token: 0x06000A89 RID: 2697 RVA: 0x00045719 File Offset: 0x00043919
	public void CopyFrom(TileEntityComposite _other)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.CopyFrom (on " + base.GetType().Name + ")");
		}
		this.CopyFromInternal(_other);
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x00045748 File Offset: 0x00043948
	public virtual void OnRemove(World _world)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.OnRemove (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x00045770 File Offset: 0x00043970
	public virtual void OnLoad()
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.OnLoad (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x00045798 File Offset: 0x00043998
	public virtual void OnUnload(World _world)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.OnUnload (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000A8D RID: 2701 RVA: 0x000457C0 File Offset: 0x000439C0
	public virtual void OnDestroy()
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.OnDestroy (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000A8E RID: 2702 RVA: 0x000457E8 File Offset: 0x000439E8
	public virtual void OnAdded(Vector3i _blockPos, BlockValue _blockValue)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.OnAdded (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000A8F RID: 2703 RVA: 0x00045810 File Offset: 0x00043A10
	public virtual void OnBlockValueChanged(Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.OnBlockValueChanged (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000A90 RID: 2704 RVA: 0x00045838 File Offset: 0x00043A38
	public virtual void OnBlockReset(Vector3i _blockPos, BlockValue _blockValue)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.OnBlockReset (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000A91 RID: 2705 RVA: 0x00045860 File Offset: 0x00043A60
	public virtual void OnBlockStartsToFall(Vector3i _blockPos, BlockValue _blockValue)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.OnBlockStartsToFall (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000A92 RID: 2706 RVA: 0x00045888 File Offset: 0x00043A88
	public virtual Block.DestroyedResult OnBlockDestroyedBy(Vector3i _blockPos, BlockValue _blockValue, int _entityId, bool _bUseHarvestTool)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.OnBlockDestroyedBy (on " + base.GetType().Name + ")");
		}
		return Block.DestroyedResult.None;
	}

	// Token: 0x06000A93 RID: 2707 RVA: 0x000458B1 File Offset: 0x00043AB1
	public virtual Block.DestroyedResult OnBlockDestroyedByExplosion(Vector3i _blockPos, BlockValue _blockValue, int _playerThatStartedExpl)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.OnBlockDestroyedByExplosion (on " + base.GetType().Name + ")");
		}
		return Block.DestroyedResult.None;
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x000458DA File Offset: 0x00043ADA
	public virtual void SetBlockEntityData(BlockEntityData _blockEntityData)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.SetBlockEntityData (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000A95 RID: 2709 RVA: 0x00045902 File Offset: 0x00043B02
	public virtual void UpgradeDowngradeFrom(TileEntityComposite _other)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.UpgradeDowngradeFrom (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000A96 RID: 2710 RVA: 0x0004592A File Offset: 0x00043B2A
	public virtual void ReplacedBy(BlockValue _bvOld, BlockValue _bvNew, TileEntity _teNew)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.ReplacedBy (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x00045952 File Offset: 0x00043B52
	public virtual void Reset(FastTags<TagGroup.Global> _questTags)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.Reset (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void UpdateTick(World _world)
	{
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public virtual string GetActivationText(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing, string _activateHotkeyMarkup, string _focusedTileEntityName)
	{
		return null;
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x0004597A File Offset: 0x00043B7A
	public virtual void InitBlockActivationCommands(Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> _addCallback)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.InitBlockActivationCommands (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x000459A2 File Offset: 0x00043BA2
	public virtual bool AllowBlockActivationCommand(ITileEntityFeature _module, ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing)
	{
		this.Equals(_module);
		return true;
	}

	// Token: 0x06000A9C RID: 2716 RVA: 0x000459B0 File Offset: 0x00043BB0
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

	// Token: 0x06000A9D RID: 2717 RVA: 0x00045A15 File Offset: 0x00043C15
	public virtual void Read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.Read (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x00045A3D File Offset: 0x00043C3D
	public virtual void Write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
	{
		if (TEFeatureAbs.DebugLogCTE)
		{
			Log.Warning("TEFeatureAbs.Write (on " + base.GetType().Name + ")");
		}
	}

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x06000A9F RID: 2719 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual LockTargetType LockTargetType
	{
		get
		{
			return LockTargetType.TEFeature;
		}
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsSharedLock(ushort _channel)
	{
		return false;
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool CanLockLocally(ILockContext _context, ushort _channel)
	{
		return true;
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool CanLockOnServer(int _lockingPlayerID, ILockContext _context, ushort _channel)
	{
		return true;
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnLockedServer(bool _success, int _lockingPlayerID, ILockContext _context, ushort _channel)
	{
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnLockedLocal(bool _success, ILockContext _context, ushort _channel)
	{
	}

	// Token: 0x06000AA5 RID: 2725 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnUnlockedServer(int _unlockingPlayerID, ushort _channel)
	{
	}

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x06000AA6 RID: 2726 RVA: 0x00045A65 File Offset: 0x00043C65
	// (remove) Token: 0x06000AA7 RID: 2727 RVA: 0x00045A73 File Offset: 0x00043C73
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

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x00045A81 File Offset: 0x00043C81
	public List<ITileEntityChangedListener> listeners
	{
		get
		{
			return this.Parent.listeners;
		}
	}

	// Token: 0x06000AA9 RID: 2729 RVA: 0x00045A8E File Offset: 0x00043C8E
	public void SetUserAccessing(bool _bUserAccessing)
	{
		this.Parent.SetUserAccessing(_bUserAccessing);
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x00045A9C File Offset: 0x00043C9C
	public bool IsUserAccessing()
	{
		return this.Parent.IsUserAccessing();
	}

	// Token: 0x06000AAB RID: 2731 RVA: 0x00045AA9 File Offset: 0x00043CA9
	public void SetModified()
	{
		this.Parent.SetModified();
	}

	// Token: 0x06000AAC RID: 2732 RVA: 0x00045AB6 File Offset: 0x00043CB6
	public Chunk GetChunk()
	{
		return this.Parent.GetChunk();
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x00045AC3 File Offset: 0x00043CC3
	public Vector3i ToWorldPos()
	{
		return this.Parent.ToWorldPos();
	}

	// Token: 0x06000AAE RID: 2734 RVA: 0x00045AD0 File Offset: 0x00043CD0
	public Vector3 ToWorldCenterPos()
	{
		return this.Parent.ToWorldCenterPos();
	}

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x06000AAF RID: 2735 RVA: 0x00045ADD File Offset: 0x00043CDD
	public BlockValue blockValue
	{
		get
		{
			return this.Parent.blockValue;
		}
	}

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x00045AEA File Offset: 0x00043CEA
	// (set) Token: 0x06000AB1 RID: 2737 RVA: 0x00045AF7 File Offset: 0x00043CF7
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

	// Token: 0x06000AB2 RID: 2738 RVA: 0x00010E62 File Offset: 0x0000F062
	public LockTargetType GetLockTargetType()
	{
		return LockTargetType.TileEntity;
	}

	// Token: 0x06000AB3 RID: 2739 RVA: 0x00045B05 File Offset: 0x00043D05
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool CommandIs(ReadOnlySpan<char> _givenCommand, string _compareCommand)
	{
		return _givenCommand.Equals(_compareCommand, StringComparison.Ordinal);
	}

	// Token: 0x06000AB4 RID: 2740 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public TEFeatureAbs()
	{
	}

	// Token: 0x04000A26 RID: 2598
	public static bool DebugLogCTE;
}
