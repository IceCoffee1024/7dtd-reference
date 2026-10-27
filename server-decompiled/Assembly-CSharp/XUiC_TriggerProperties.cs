using System;
using System.Collections.Generic;
using PrefabVolumes;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010B3 RID: 4275
[Preserve]
public class XUiC_TriggerProperties : XUiController, ISelectionBoxCallback
{
	// Token: 0x17000FFA RID: 4090
	// (set) Token: 0x0600877C RID: 34684 RVA: 0x0033CE52 File Offset: 0x0033B052
	public Prefab Prefab
	{
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.prefab = value;
			if (this.prefab != null && this.prefab.TriggerLayers.Count == 0)
			{
				this.prefab.AddInitialTriggerLayers();
			}
		}
	}

	// Token: 0x17000FFB RID: 4091
	// (set) Token: 0x0600877D RID: 34685 RVA: 0x0033CE80 File Offset: 0x0033B080
	public Vector3i BlockPos
	{
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.blockPos = value;
			this.blockTrigger = this.BlockChunk.GetBlockTrigger(World.toBlock(this.blockPos));
		}
	}

	// Token: 0x17000FFC RID: 4092
	// (get) Token: 0x0600877E RID: 34686 RVA: 0x0033CEA5 File Offset: 0x0033B0A5
	public Chunk BlockChunk
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return (Chunk)GameManager.Instance.World.ChunkCache.GetChunkFromWorldPos(this.blockPos);
		}
	}

	// Token: 0x17000FFD RID: 4093
	// (get) Token: 0x0600877F RID: 34687 RVA: 0x0033CEC8 File Offset: 0x0033B0C8
	public List<byte> TriggersIndices
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_TriggerProperties.EOpenedFor eopenedFor = this.openedFor;
			List<byte> list;
			if (eopenedFor != XUiC_TriggerProperties.EOpenedFor.Block)
			{
				if (eopenedFor != XUiC_TriggerProperties.EOpenedFor.TriggerVolume)
				{
					list = null;
				}
				else
				{
					SelectionBox selectionBox;
					PrefabTriggerVolume prefabTriggerVolume;
					PrefabInstance prefabInstance;
					int num;
					list = (PrefabVolumeManager.TryGetSelectedVolume<PrefabTriggerVolume>(SelectionBoxManager.Instance.CategoryTriggerVolume, out selectionBox, out prefabTriggerVolume, out prefabInstance, out num) ? prefabTriggerVolume.TriggersIndices : null);
				}
			}
			else
			{
				BlockTrigger blockTrigger = this.blockTrigger;
				list = ((blockTrigger != null) ? blockTrigger.TriggersIndices : null);
			}
			return list;
		}
	}

	// Token: 0x17000FFE RID: 4094
	// (get) Token: 0x06008780 RID: 34688 RVA: 0x0033CF22 File Offset: 0x0033B122
	public List<byte> TriggeredByIndices
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			BlockTrigger blockTrigger = this.blockTrigger;
			if (blockTrigger == null)
			{
				return null;
			}
			return blockTrigger.TriggeredByIndices;
		}
	}

	// Token: 0x06008781 RID: 34689 RVA: 0x0033CF38 File Offset: 0x0033B138
	public override void Init()
	{
		base.Init();
		XUiC_TriggerProperties.ID = base.WindowGroup.Id;
		this.triggersList = base.GetChildById("triggers") as XUiC_PrefabTriggerEditorList;
		if (this.triggersList != null)
		{
			this.triggersList.SelectionChanged += this.TriggersList_SelectionChanged;
			this.triggersList.GetCurrentTriggerIndicesList = () => this.TriggersIndices;
			this.triggersList.GetCurrentPrefabTriggerLayers = delegate
			{
				Prefab prefab = this.prefab;
				if (prefab == null)
				{
					return null;
				}
				return prefab.TriggerLayers;
			};
		}
		XUiController childById = base.GetChildById("addTriggersButton");
		if (childById != null)
		{
			childById.OnPress += this.TriggerOnAddTriggersPressed;
		}
		this.triggeredByList = base.GetChildById("triggeredBy") as XUiC_PrefabTriggerEditorList;
		if (this.triggeredByList != null)
		{
			this.triggeredByList.SelectionChanged += this.TriggeredByList_SelectionChanged;
			this.triggeredByList.GetCurrentTriggerIndicesList = () => this.TriggeredByIndices;
			this.triggeredByList.GetCurrentPrefabTriggerLayers = delegate
			{
				Prefab prefab2 = this.prefab;
				if (prefab2 == null)
				{
					return null;
				}
				return prefab2.TriggerLayers;
			};
		}
		XUiController childById2 = base.GetChildById("addTriggeredByButton");
		if (childById2 != null)
		{
			childById2.OnPress += this.TriggerOnAddTriggersPressed;
		}
		XUiController xuiController = base.GetChildById("exclude");
		if (xuiController != null)
		{
			xuiController.OnPress += this.triggerExclude_OnPressed;
		}
		xuiController = base.GetChildById("operation");
		if (xuiController != null)
		{
			xuiController.OnPress += this.triggerOperation_OnPressed;
		}
		xuiController = base.GetChildById("unlock");
		if (xuiController != null)
		{
			xuiController.OnPress += this.triggerUnlock_OnPressed;
		}
		if (SelectionBoxManager.Instance != null)
		{
			SelectionBoxManager.Instance.CategoryTriggerVolume.SetCallback(this);
		}
	}

	// Token: 0x06008782 RID: 34690 RVA: 0x0033D0E3 File Offset: 0x0033B2E3
	[PublicizedFrom(EAccessModifier.Private)]
	public void triggerExclude_OnPressed(XUiController _controller, int _button)
	{
		if (this.blockTrigger == null)
		{
			return;
		}
		this.blockTrigger.ExcludeIcon = !this.blockTrigger.ExcludeIcon;
		this.setChunkModified();
		base.RefreshBindings();
	}

	// Token: 0x06008783 RID: 34691 RVA: 0x0033D113 File Offset: 0x0033B313
	[PublicizedFrom(EAccessModifier.Private)]
	public void triggerOperation_OnPressed(XUiController _controller, int _button)
	{
		if (this.blockTrigger == null)
		{
			return;
		}
		this.blockTrigger.UseOrForMultipleTriggers = !this.blockTrigger.UseOrForMultipleTriggers;
		this.setChunkModified();
		base.RefreshBindings();
	}

	// Token: 0x06008784 RID: 34692 RVA: 0x0033D143 File Offset: 0x0033B343
	[PublicizedFrom(EAccessModifier.Private)]
	public void triggerUnlock_OnPressed(XUiController _controller, int _button)
	{
		if (this.blockTrigger == null)
		{
			return;
		}
		this.blockTrigger.Unlock = !this.blockTrigger.Unlock;
		this.setChunkModified();
		base.RefreshBindings();
	}

	// Token: 0x06008785 RID: 34693 RVA: 0x0033D173 File Offset: 0x0033B373
	[PublicizedFrom(EAccessModifier.Private)]
	public void setChunkModified()
	{
		PrefabEditModeManager.Instance.NeedsSaving = true;
		this.BlockChunk.isModified = true;
	}

	// Token: 0x06008786 RID: 34694 RVA: 0x0033D18C File Offset: 0x0033B38C
	public override void Cleanup()
	{
		base.Cleanup();
		if (SelectionBoxManager.Instance != null)
		{
			SelectionBoxManager.Instance.CategoryTriggerVolume.SetCallback(null);
		}
	}

	// Token: 0x06008787 RID: 34695 RVA: 0x0033D1B1 File Offset: 0x0033B3B1
	[PublicizedFrom(EAccessModifier.Private)]
	public void TriggerOnAddTriggersPressed(XUiController _sender, int _mouseButton)
	{
		this.prefab.AddNewTriggerLayer();
		this.triggersList.RebuildList(false);
		this.triggeredByList.RebuildList(false);
	}

	// Token: 0x06008788 RID: 34696 RVA: 0x0033D1D8 File Offset: 0x0033B3D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void TriggersList_SelectionChanged(XUiC_List<XUiC_PrefabTriggerEditorList.PrefabTriggerEntry> _list, XUiC_PrefabTriggerEditorList.PrefabTriggerEntry _previousEntry, XUiC_PrefabTriggerEditorList.PrefabTriggerEntry _newEntry)
	{
		if (_newEntry == null)
		{
			return;
		}
		byte triggerLayer = _newEntry.TriggerLayer;
		if (this.openedFor == XUiC_TriggerProperties.EOpenedFor.TriggerVolume)
		{
			this.handleTriggerVolumeSetting(triggerLayer, true);
		}
		else
		{
			this.handleTriggersSetting(triggerLayer, true, GameManager.Instance.World, this.blockPos);
		}
		_newEntry.UiDirty = true;
	}

	// Token: 0x06008789 RID: 34697 RVA: 0x0033D224 File Offset: 0x0033B424
	[PublicizedFrom(EAccessModifier.Private)]
	public void TriggeredByList_SelectionChanged(XUiC_List<XUiC_PrefabTriggerEditorList.PrefabTriggerEntry> _list, XUiC_PrefabTriggerEditorList.PrefabTriggerEntry _previousEntry, XUiC_PrefabTriggerEditorList.PrefabTriggerEntry _newEntry)
	{
		if (_newEntry == null)
		{
			return;
		}
		byte triggerLayer = _newEntry.TriggerLayer;
		if (this.openedFor == XUiC_TriggerProperties.EOpenedFor.TriggerVolume)
		{
			this.handleTriggerVolumeSetting(triggerLayer, false);
		}
		else
		{
			this.handleTriggersSetting(triggerLayer, false, GameManager.Instance.World, this.blockPos);
		}
		_newEntry.UiDirty = true;
	}

	// Token: 0x0600878A RID: 34698 RVA: 0x0029A895 File Offset: 0x00298A95
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x0600878B RID: 34699 RVA: 0x0033D26E File Offset: 0x0033B46E
	public override void OnClose()
	{
		base.OnClose();
		this.Prefab = null;
		this.blockTrigger = null;
	}

	// Token: 0x0600878C RID: 34700 RVA: 0x0008ABF1 File Offset: 0x00088DF1
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x0600878D RID: 34701 RVA: 0x0033D284 File Offset: 0x0033B484
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "triggers_enabled")
		{
			_value = this.showTriggers.ToString();
			return true;
		}
		if (_bindingName == "triggeredby_enabled")
		{
			_value = this.showTriggeredBy.ToString();
			return true;
		}
		if (_bindingName == "excludeTickmarkSelected")
		{
			_value = ((this.blockTrigger != null) ? this.blockTrigger.ExcludeIcon.ToString() : "false");
			return true;
		}
		if (_bindingName == "operationTickmarkSelected")
		{
			_value = ((this.blockTrigger != null) ? this.blockTrigger.UseOrForMultipleTriggers.ToString() : "false");
			return true;
		}
		if (!(_bindingName == "unlockTickmarkSelected"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		_value = ((this.blockTrigger != null) ? this.blockTrigger.Unlock.ToString() : "false");
		return true;
	}

	// Token: 0x0600878E RID: 34702 RVA: 0x0033D368 File Offset: 0x0033B568
	[PublicizedFrom(EAccessModifier.Private)]
	public void handleTriggersSetting(byte _triggerLayer, bool _isTriggers, World _world, Vector3i _blockPos)
	{
		if (!_world.IsEditor())
		{
			return;
		}
		Chunk blockChunk = this.BlockChunk;
		this.blockTrigger = blockChunk.GetBlockTrigger(World.toBlock(_blockPos));
		if (_triggerLayer == 0)
		{
			if (this.blockTrigger != null)
			{
				blockChunk.RemoveBlockTrigger(this.blockTrigger);
				this.blockTrigger = null;
			}
		}
		else
		{
			if (this.blockTrigger == null)
			{
				this.blockTrigger = new BlockTrigger(blockChunk)
				{
					LocalChunkPos = World.toBlock(_blockPos)
				};
				blockChunk.AddBlockTrigger(this.blockTrigger);
			}
			if (_isTriggers)
			{
				this.blockTrigger.ToggleTriggersFlag(_triggerLayer);
			}
			else
			{
				this.blockTrigger.ToggleTriggeredByFlag(_triggerLayer);
			}
			if (!this.blockTrigger.HasAnyTriggers() && !this.blockTrigger.HasAnyTriggeredBy() && this.blockTrigger != null)
			{
				blockChunk.RemoveBlockTrigger(this.blockTrigger);
				this.blockTrigger = null;
			}
		}
		this.setChunkModified();
		BlockTrigger blockTrigger = this.blockTrigger;
		if (blockTrigger == null)
		{
			return;
		}
		blockTrigger.TriggerUpdated(null);
	}

	// Token: 0x0600878F RID: 34703 RVA: 0x0033D454 File Offset: 0x0033B654
	[PublicizedFrom(EAccessModifier.Private)]
	public void handleTriggerVolumeSetting(byte _triggerLayer, bool _isTriggers)
	{
		SelectionBox selectionBox;
		PrefabTriggerVolume prefabTriggerVolume;
		PrefabInstance prefabInstance;
		int num;
		if (!PrefabVolumeManager.TryGetSelectedVolume<PrefabTriggerVolume>(SelectionBoxManager.Instance.CategoryTriggerVolume, out selectionBox, out prefabTriggerVolume, out prefabInstance, out num))
		{
			return;
		}
		if (_isTriggers)
		{
			PrefabTriggerVolume prefabTriggerVolume2 = prefabTriggerVolume.CloneGeneric();
			prefabTriggerVolume2.ToggleTriggersFlag(_triggerLayer);
			PrefabVolumeManager.Instance.UpdatePropertiesServer(prefabInstance.id, num, prefabTriggerVolume2);
		}
		BlockTrigger blockTrigger = this.blockTrigger;
		if (blockTrigger == null)
		{
			return;
		}
		blockTrigger.TriggerUpdated(null);
	}

	// Token: 0x06008790 RID: 34704 RVA: 0x0033D4B4 File Offset: 0x0033B6B4
	public static void Show(XUi _xui, Vector3i _blockPos, bool _showTriggers, bool _showTriggeredBy)
	{
		XUiC_TriggerProperties childByType = ((XUiWindowGroup)_xui.playerUI.windowManager.GetWindow(XUiC_TriggerProperties.ID)).Controller.GetChildByType<XUiC_TriggerProperties>();
		childByType.openedFor = XUiC_TriggerProperties.EOpenedFor.Block;
		childByType.Prefab = PrefabEditModeManager.Instance.VoxelPrefab;
		childByType.BlockPos = _blockPos;
		childByType.showTriggers = _showTriggers;
		childByType.showTriggeredBy = _showTriggeredBy;
		childByType.RefreshBindings();
		_xui.playerUI.windowManager.Open(XUiC_TriggerProperties.ID, true);
	}

	// Token: 0x06008791 RID: 34705 RVA: 0x00020119 File Offset: 0x0001E319
	public bool OnSelectionBoxActivated(SelectionBox _box, bool _bActivated)
	{
		return true;
	}

	// Token: 0x06008792 RID: 34706 RVA: 0x0033D52C File Offset: 0x0033B72C
	public void OnSelectionBoxMoved(SelectionBox _box, Vector3 _moveVector)
	{
		PrefabVolumeManager.Instance.SelectionBoxMoved(PrefabVolumeAbs.EVolumeType.Trigger, _box, _moveVector);
	}

	// Token: 0x06008793 RID: 34707 RVA: 0x0033D53C File Offset: 0x0033B73C
	public void OnSelectionBoxSized(SelectionBox _box, int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest)
	{
		PrefabVolumeManager.Instance.SelectionBoxSized(PrefabVolumeAbs.EVolumeType.Trigger, _box, _dTop, _dBottom, _dNorth, _dSouth, _dEast, _dWest);
	}

	// Token: 0x06008794 RID: 34708 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxMirrored(Vector3i _axis)
	{
	}

	// Token: 0x06008795 RID: 34709 RVA: 0x0033D560 File Offset: 0x0033B760
	public bool OnSelectionBoxDelete(SelectionBox _box, bool _checkCanDeleteOnly)
	{
		int num;
		PrefabInstance prefabInstance;
		return PrefabVolumeManager.GetPrefabIdAndVolumeId(_box.name, out num, out prefabInstance) && PrefabVolumeManager.Instance.SelectionBoxDelete(PrefabVolumeAbs.EVolumeType.Trigger, _box, _checkCanDeleteOnly);
	}

	// Token: 0x06008796 RID: 34710 RVA: 0x0033D58D File Offset: 0x0033B78D
	public bool OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities _criteria)
	{
		return _criteria == EnumSelectionBoxAvailabilities.CanShowProperties || _criteria == EnumSelectionBoxAvailabilities.CanResize;
	}

	// Token: 0x06008797 RID: 34711 RVA: 0x0033D598 File Offset: 0x0033B798
	public void OnSelectionBoxShowProperties(bool _bVisible, GUIWindowManager _windowManager)
	{
		SelectionBox selectionBox;
		PrefabTriggerVolume prefabTriggerVolume;
		PrefabInstance prefabInstance;
		int num;
		if (!PrefabVolumeManager.TryGetSelectedVolume<PrefabTriggerVolume>(SelectionBoxManager.Instance.CategoryTriggerVolume, out selectionBox, out prefabTriggerVolume, out prefabInstance, out num))
		{
			return;
		}
		this.openedFor = XUiC_TriggerProperties.EOpenedFor.TriggerVolume;
		this.Prefab = prefabInstance.prefab;
		this.showTriggers = true;
		this.showTriggeredBy = false;
		_windowManager.Open(this.windowGroup, true);
	}

	// Token: 0x06008798 RID: 34712 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxRotated(SelectionBox _box)
	{
	}

	// Token: 0x06008799 RID: 34713 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxUserDataChanged(SelectionBox _box)
	{
	}

	// Token: 0x040064C4 RID: 25796
	public static string ID = "";

	// Token: 0x040064C5 RID: 25797
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PrefabTriggerEditorList triggersList;

	// Token: 0x040064C6 RID: 25798
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PrefabTriggerEditorList triggeredByList;

	// Token: 0x040064C7 RID: 25799
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TriggerProperties.EOpenedFor openedFor;

	// Token: 0x040064C8 RID: 25800
	[PublicizedFrom(EAccessModifier.Private)]
	public Prefab prefab;

	// Token: 0x040064C9 RID: 25801
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x040064CA RID: 25802
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockTrigger blockTrigger;

	// Token: 0x040064CB RID: 25803
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showTriggers = true;

	// Token: 0x040064CC RID: 25804
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showTriggeredBy = true;

	// Token: 0x020010B4 RID: 4276
	[PublicizedFrom(EAccessModifier.Private)]
	public enum EOpenedFor
	{
		// Token: 0x040064CE RID: 25806
		Block,
		// Token: 0x040064CF RID: 25807
		TriggerVolume
	}
}
