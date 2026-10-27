using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000179 RID: 377
[Preserve]
public class TEFeatureCanvas : TEFeatureAbs, IFeatureSavedInPrefab
{
	// Token: 0x170000DD RID: 221
	// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x00046711 File Offset: 0x00044911
	// (set) Token: 0x06000AEA RID: 2794 RVA: 0x00046719 File Offset: 0x00044919
	public SignCanvas Canvas
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06000AEB RID: 2795 RVA: 0x00046724 File Offset: 0x00044924
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

	// Token: 0x06000AEC RID: 2796 RVA: 0x00046788 File Offset: 0x00044988
	public override void Init(TileEntityComposite _parent, TileEntityFeatureData _featureData)
	{
		base.Init(_parent, _featureData);
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x00046794 File Offset: 0x00044994
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
			if (this.pendingCanvasState.SignId.libraryId != "[D]" && this.pendingCanvasState.SignId.libraryId != fileNameNoExtension && SignDataManager.Instance.CopySignToLibrary(this.pendingCanvasState.SignId, fileNameNoExtension, out globalSignId))
			{
				this.pendingCanvasState.SignId = globalSignId;
			}
		}
	}

	// Token: 0x06000AEE RID: 2798 RVA: 0x0004687C File Offset: 0x00044A7C
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
		this.Canvas.Initialize(null);
		this.Canvas.State = this.pendingCanvasState;
		LODGroup componentInChildren = _blockEntityData.transform.GetComponentInChildren<LODGroup>();
		if (componentInChildren)
		{
			LOD[] lods = componentInChildren.GetLODs();
			lods[lods.Length - 1].screenRelativeTransitionHeight = 0.003f;
			componentInChildren.SetLODs(lods);
		}
	}

	// Token: 0x06000AEF RID: 2799 RVA: 0x0004692C File Offset: 0x00044B2C
	public override void OnAdded(Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnAdded(_blockPos, _blockValue);
		this.pendingCanvasState.CanvasRotation = TEFeatureCanvas.GetUprightRotation(_blockValue.rotation);
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x00046950 File Offset: 0x00044B50
	public bool TryReplaceSignReference(GlobalSignId from, GlobalSignId to)
	{
		SignCanvas.CanvasState canvasState = ((this.Canvas != null) ? this.Canvas.State : this.pendingCanvasState);
		if (canvasState == null || !canvasState.SignId.Equals(from))
		{
			return false;
		}
		if (this.Canvas != null)
		{
			this.Canvas.SignId = to;
		}
		else
		{
			this.pendingCanvasState.SignId = to;
		}
		return true;
	}

	// Token: 0x06000AF1 RID: 2801 RVA: 0x000469BC File Offset: 0x00044BBC
	public override void OnLoad()
	{
		base.OnLoad();
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		SignCanvas.CanvasState canvasState = ((this.Canvas != null) ? this.Canvas.State : this.pendingCanvasState);
		if (canvasState == null)
		{
			return;
		}
		GlobalSignId signId = canvasState.SignId;
		SignData signData;
		if (SignDataManager.Instance.TryGetSignData(signId, out signData))
		{
			return;
		}
		if (SignDataManager.Instance.IsKnownDeleted(signId))
		{
			this.TryReplaceSignReference(signId, SignData.defaultSignDataID);
			base.Parent.SetChunkModified();
			return;
		}
		Log.Error(string.Format("TEFeatureCanvas at {0} references sign '{1}', which has no data but is not recorded as ", base.ToWorldPos(), signId) + "deleted. The reference has been left intact in case the data can be restored.");
	}

	// Token: 0x06000AF2 RID: 2802 RVA: 0x00046A69 File Offset: 0x00044C69
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

	// Token: 0x06000AF3 RID: 2803 RVA: 0x00046A81 File Offset: 0x00044C81
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

	// Token: 0x06000AF4 RID: 2804 RVA: 0x00046A9A File Offset: 0x00044C9A
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

	// Token: 0x06000AF5 RID: 2805 RVA: 0x00046AB3 File Offset: 0x00044CB3
	public override bool OnLockResponseLocal(bool _success, PooledBinaryReader _brServerContext, ushort _channel)
	{
		this.ShowUI(_success);
		return true;
	}

	// Token: 0x06000AF6 RID: 2806 RVA: 0x00046ABD File Offset: 0x00044CBD
	public void ReleaseLock()
	{
		LockManager.Instance.UnlockRequestLocal();
	}

	// Token: 0x06000AF7 RID: 2807 RVA: 0x00046ACC File Offset: 0x00044CCC
	[PublicizedFrom(EAccessModifier.Private)]
	public void ShowUI(bool _lockGranted)
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		if (!_lockGranted)
		{
			GameManager.ShowTooltip(uiforPrimaryPlayer.entityPlayer, Localization.Get("ttNoInteractItem", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			return;
		}
		XUiC_SignGalleryWindow.Open(uiforPrimaryPlayer, this);
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x00046B13 File Offset: 0x00044D13
	public override string GetActivationText(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing, string _activateHotkeyMarkup, string _focusedTileEntityName)
	{
		base.GetActivationText(_world, _blockPos, _blockValue, _entityFocusing, _activateHotkeyMarkup, _focusedTileEntityName);
		return Localization.Get("useWorkstation", false, null);
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x00046B31 File Offset: 0x00044D31
	public override void InitBlockActivationCommands(Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> _addCallback)
	{
		base.InitBlockActivationCommands(_addCallback);
		_addCallback(new BlockActivationCommand("edit", "pen", true, false, null), TileEntityComposite.EBlockCommandOrder.Normal, base.FeatureData);
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x00046B5C File Offset: 0x00044D5C
	public override bool AllowBlockActivationCommand(ITileEntityFeature _module, ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing)
	{
		if (!base.AllowBlockActivationCommand(_module, _commandName, _world, _blockPos, _blockValue, _entityFocusing))
		{
			return false;
		}
		EntityPlayerLocal entityPlayerLocal = _entityFocusing as EntityPlayerLocal;
		bool flag = entityPlayerLocal != null && GameManager.Instance.World.GetLandClaimOwnerInParty(entityPlayerLocal, entityPlayerLocal.persistentPlayerData);
		if (!_world.IsEditor() && !flag)
		{
			return false;
		}
		if (!this.Equals(_module))
		{
			return true;
		}
		if (base.CommandIs(_commandName, "edit"))
		{
			return true;
		}
		base.CommandIs(_commandName, "report");
		return true;
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x00046BD8 File Offset: 0x00044DD8
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
								LockManager.Instance.LockRequestLocal(this, 0);
							}
						}, XUiC_SaveDirtyPrefab.EMode.ForceSave);
					}
					else
					{
						LockManager.Instance.LockRequestLocal(this, 0);
					}
				}
				else
				{
					GameManager.ShowTooltip(GameManager.Instance.World.GetPrimaryPlayer(), "Cannot edit signs as client", false, false, 0f);
				}
			}
			else if (GameManager.Instance.World.GetLandClaimOwnerInParty(_player, _player.persistentPlayerData))
			{
				LockManager.Instance.LockRequestLocal(this, 0);
			}
			return true;
		}
		return base.CommandIs(_commandName, "report");
	}

	// Token: 0x06000AFC RID: 2812 RVA: 0x00046CCC File Offset: 0x00044ECC
	public override void Read(PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		base.Read(_br, _eStreamMode);
		int num;
		if (_eStreamMode != StreamModeRead.Persistency)
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
		SignData signData;
		if (_eStreamMode == StreamModeRead.FromClient && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && !SignDataManager.Instance.TryGetSignData(canvasState.SignId, out signData) && SignDataManager.Instance.IsKnownDeleted(canvasState.SignId))
		{
			canvasState.SignId = SignData.defaultSignDataID;
		}
		if (this.Canvas != null)
		{
			this.Canvas.State = canvasState;
			return;
		}
		this.pendingCanvasState = canvasState;
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x00046DBC File Offset: 0x00044FBC
	public override void Write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		base.Write(_bw, _eStreamMode);
		if (_eStreamMode == StreamModeWrite.Persistency)
		{
			_bw.Write(21);
		}
		((this.Canvas != null) ? this.Canvas.State : this.pendingCanvasState).Write(_bw);
	}

	// Token: 0x04000A56 RID: 2646
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 21;

	// Token: 0x04000A57 RID: 2647
	[PublicizedFrom(EAccessModifier.Private)]
	public SignCanvas.CanvasState pendingCanvasState = new SignCanvas.CanvasState();
}
