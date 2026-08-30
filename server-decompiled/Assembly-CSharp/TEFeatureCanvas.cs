using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000177 RID: 375
[Preserve]
public class TEFeatureCanvas : TEFeatureAbs, IFeatureSavedInPrefab
{
	// Token: 0x170000DE RID: 222
	// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x00046655 File Offset: 0x00044855
	// (set) Token: 0x06000AE5 RID: 2789 RVA: 0x0004665D File Offset: 0x0004485D
	public SignCanvas Canvas
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06000AE6 RID: 2790 RVA: 0x00046668 File Offset: 0x00044868
	public static CanvasRotationMode GetUprightRotation(byte blockRotation)
	{
		int num = (int)(blockRotation & 3);
		switch ((blockRotation >> 2) & 7)
		{
		case 0:
			return CanvasRotationMode.None;
		case 1:
			return CanvasRotationMode.Rot180;
		case 2:
		case 4:
			if ((num & 1) != 0)
			{
				return (CanvasRotationMode)num;
			}
			return CanvasRotationMode.None;
		case 3:
			if ((num & 1) != 1)
			{
				return (CanvasRotationMode)((num + 3) % 4);
			}
			return CanvasRotationMode.None;
		case 5:
			if ((num & 1) != 1)
			{
				return (CanvasRotationMode)((num + 1) % 4);
			}
			return CanvasRotationMode.None;
		default:
			return CanvasRotationMode.None;
		}
	}

	// Token: 0x06000AE7 RID: 2791 RVA: 0x000466CC File Offset: 0x000448CC
	public override void Init(TileEntityComposite _parent, TileEntityFeatureData _featureData)
	{
		base.Init(_parent, _featureData);
		this.lockFeature = base.Parent.GetFeature<TEFeatureLockable>();
		this.lockpickFeature = base.Parent.GetFeature<TEFeatureLockPickable>();
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x000466F8 File Offset: 0x000448F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyFromInternal(TileEntityComposite _other)
	{
		TEFeatureCanvas tefeatureCanvas;
		if (!_other.TryGetSelfOrFeature<TEFeatureCanvas>(out tefeatureCanvas))
		{
			Log.Error("CopyFrom failed to get Canvas feature from other entity.");
			return;
		}
		SignCanvas.CanvasState canvasState2;
		if (!(tefeatureCanvas.Canvas != null))
		{
			SignCanvas.CanvasState canvasState = tefeatureCanvas.pendingCanvasState;
			canvasState2 = ((canvasState != null) ? canvasState.Clone() : null);
		}
		else
		{
			canvasState2 = tefeatureCanvas.Canvas.State.Clone();
		}
		this.pendingCanvasState = canvasState2;
		if (this.pendingCanvasState != null && PrefabEditModeManager.Instance != null && PrefabEditModeManager.Instance.LoadedPrefab.FileNameNoExtension != null)
		{
			string fileNameNoExtension = PrefabEditModeManager.Instance.LoadedPrefab.FileNameNoExtension;
			GlobalSignId globalSignId;
			if (this.pendingCanvasState.SignId.libraryId != "[D]" && this.pendingCanvasState.SignId.libraryId != fileNameNoExtension && SignDataManager.Instance.MoveSignToLibrary(this.pendingCanvasState.SignId, fileNameNoExtension, out globalSignId))
			{
				this.pendingCanvasState.SignId = globalSignId;
			}
		}
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x000467E0 File Offset: 0x000449E0
	public override void SetBlockEntityData(BlockEntityData _blockEntityData)
	{
		if (_blockEntityData == null || !_blockEntityData.bHasTransform)
		{
			return;
		}
		if (GameManager.IsDedicatedServer)
		{
			return;
		}
		this.Canvas = _blockEntityData.transform.GetComponentInChildren<SignCanvas>(true);
		if (this.Canvas == null)
		{
			Log.Error(string.Format("TEFeatureCanvas at {0} has no SignCanvas component in its block prefab.", base.ToWorldPos()));
			return;
		}
		this.Canvas.RenderingDataUpdated -= this.SetModified;
		this.Canvas.RenderingDataUpdated += this.SetModified;
		this.Canvas.State = this.pendingCanvasState;
		this.Canvas.Initialize(null);
		LODGroup componentInChildren = _blockEntityData.transform.GetComponentInChildren<LODGroup>();
		if (componentInChildren)
		{
			LOD[] lods = componentInChildren.GetLODs();
			lods[lods.Length - 1].screenRelativeTransitionHeight = 0.003f;
			componentInChildren.SetLODs(lods);
		}
	}

	// Token: 0x06000AEA RID: 2794 RVA: 0x000468C0 File Offset: 0x00044AC0
	public override void OnAdded(Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnAdded(_blockPos, _blockValue);
		this.pendingCanvasState.CanvasRotation = TEFeatureCanvas.GetUprightRotation(_blockValue.rotation);
	}

	// Token: 0x06000AEB RID: 2795 RVA: 0x000468E1 File Offset: 0x00044AE1
	public override void OnDestroy()
	{
		base.OnDestroy();
		SignCanvas canvas = this.Canvas;
		if (canvas == null)
		{
			return;
		}
		canvas.Cleanup();
	}

	// Token: 0x06000AEC RID: 2796 RVA: 0x000468F9 File Offset: 0x00044AF9
	public override void OnUnload(World _world)
	{
		base.OnUnload(_world);
		SignCanvas canvas = this.Canvas;
		if (canvas == null)
		{
			return;
		}
		canvas.Cleanup();
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x00046912 File Offset: 0x00044B12
	public override void OnRemove(World _world)
	{
		base.OnRemove(_world);
		SignCanvas canvas = this.Canvas;
		if (canvas == null)
		{
			return;
		}
		canvas.Cleanup();
	}

	// Token: 0x06000AEE RID: 2798 RVA: 0x0004692C File Offset: 0x00044B2C
	public override string GetActivationText(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing, string _activateHotkeyMarkup, string _focusedTileEntityName)
	{
		base.GetActivationText(_world, _blockPos, _blockValue, _entityFocusing, _activateHotkeyMarkup, _focusedTileEntityName);
		if (this.lockpickFeature != null && this.lockpickFeature.NeedsLockpicking())
		{
			return string.Format(Localization.Get("tooltipLocked", false, null), _activateHotkeyMarkup, _focusedTileEntityName);
		}
		if (this.lockFeature != null && this.lockFeature.IsLocked() && !this.lockFeature.IsUserAllowed(PlatformManager.InternalLocalUserIdentifier))
		{
			return string.Format(Localization.Get("tooltipJammed", false, null), _activateHotkeyMarkup, _focusedTileEntityName);
		}
		return Localization.Get("useWorkstation", false, null);
	}

	// Token: 0x06000AEF RID: 2799 RVA: 0x000469BD File Offset: 0x00044BBD
	public override void InitBlockActivationCommands(Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> _addCallback)
	{
		base.InitBlockActivationCommands(_addCallback);
		_addCallback(new BlockActivationCommand("edit", "pen", true, false, null), TileEntityComposite.EBlockCommandOrder.Normal, base.FeatureData);
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x000469E8 File Offset: 0x00044BE8
	public override bool AllowBlockActivationCommand(ITileEntityFeature _module, ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing)
	{
		if (!base.AllowBlockActivationCommand(_module, _commandName, _world, _blockPos, _blockValue, _entityFocusing))
		{
			return false;
		}
		if (!_world.IsEditor())
		{
			return false;
		}
		if (!this.Equals(_module))
		{
			return true;
		}
		if (base.CommandIs(_commandName, "edit"))
		{
			return this.lockFeature == null || GameManager.Instance.IsEditMode() || !this.lockFeature.IsLocked() || this.lockFeature.IsUserAllowed(PlatformManager.InternalLocalUserIdentifier);
		}
		base.CommandIs(_commandName, "report");
		return true;
	}

	// Token: 0x06000AF1 RID: 2801 RVA: 0x00046A70 File Offset: 0x00044C70
	public override bool OnBlockActivated(ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		base.OnBlockActivated(_commandName, _world, _blockPos, _blockValue, _player);
		if (base.CommandIs(_commandName, "edit"))
		{
			if (_world.IsEditor())
			{
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					if (PrefabEditModeManager.Instance != null && PrefabEditModeManager.Instance.IsActive() && PrefabEditModeManager.Instance.LoadedPrefab.Type == PathAbstractions.EAbstractedLocationType.None)
					{
						XUiC_SaveDirtyPrefab.Show(_player.PlayerUI.xui, delegate(XUiC_SaveDirtyPrefab.ESelectedAction _action)
						{
							if (_action == XUiC_SaveDirtyPrefab.ESelectedAction.Save)
							{
								XUiC_SignGalleryWindow.Open(_player.PlayerUI, this);
							}
						}, XUiC_SaveDirtyPrefab.EMode.ForceSave);
					}
					else
					{
						XUiC_SignGalleryWindow.Open(_player.PlayerUI, this);
					}
				}
				else
				{
					GameManager.ShowTooltip(GameManager.Instance.World.GetPrimaryPlayer(), "Cannot edit signs as client", false, false, 0f);
				}
			}
			return true;
		}
		return base.CommandIs(_commandName, "report");
	}

	// Token: 0x06000AF2 RID: 2802 RVA: 0x00046B5C File Offset: 0x00044D5C
	public override void Read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode)
	{
		base.Read(_br, _eStreamMode);
		int num;
		if (_eStreamMode != TileEntity.StreamModeRead.Persistency)
		{
			num = 21;
		}
		else if (base.Parent.UseLocalVersioning())
		{
			num = (int)_br.ReadUInt16();
		}
		else
		{
			num = base.Parent.GetLegacyForkVersion();
		}
		SignCanvas.CanvasState canvasState;
		if (num >= 21)
		{
			canvasState = SignCanvas.CanvasState.Read(_br);
		}
		else
		{
			canvasState = new SignCanvas.CanvasState();
			canvasState.SignId = GlobalSignId.FromStream(_br);
			if (num < 18)
			{
				_br.ReadByte();
			}
			if (num >= 19)
			{
				canvasState.BlendMode = (SignCanvas.SignBlendMode)_br.ReadByte();
			}
			if (num >= 20)
			{
				canvasState.CanvasRotation = (CanvasRotationMode)_br.ReadByte();
			}
		}
		if (this.Canvas != null)
		{
			this.Canvas.State = canvasState;
			return;
		}
		this.pendingCanvasState = canvasState;
	}

	// Token: 0x06000AF3 RID: 2803 RVA: 0x00046C0B File Offset: 0x00044E0B
	public override void Write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
	{
		base.Write(_bw, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeWrite.Persistency)
		{
			_bw.Write(21);
		}
		((this.Canvas != null) ? this.Canvas.State : this.pendingCanvasState).Write(_bw);
	}

	// Token: 0x04000A53 RID: 2643
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 21;

	// Token: 0x04000A54 RID: 2644
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureLockable lockFeature;

	// Token: 0x04000A55 RID: 2645
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureLockPickable lockpickFeature;

	// Token: 0x04000A56 RID: 2646
	[PublicizedFrom(EAccessModifier.Private)]
	public SignCanvas.CanvasState pendingCanvasState = new SignCanvas.CanvasState();
}
