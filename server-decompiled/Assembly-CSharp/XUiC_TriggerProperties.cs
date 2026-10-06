using System;
using System.Collections.Generic;
using PrefabVolumes;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010A3 RID: 4259
[Preserve]
public class XUiC_TriggerProperties : XUiController, ISelectionBoxCallback
{
	// Token: 0x17000FEF RID: 4079
	// (set) Token: 0x06008729 RID: 34601 RVA: 0x0033ADCE File Offset: 0x00338FCE
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

	// Token: 0x17000FF0 RID: 4080
	// (set) Token: 0x0600872A RID: 34602 RVA: 0x0033ADFC File Offset: 0x00338FFC
	public Vector3i BlockPos
	{
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.blockPos = value;
			this.blockTrigger = this.BlockChunk.GetBlockTrigger(World.toBlock(this.blockPos));
		}
	}

	// Token: 0x17000FF1 RID: 4081
	// (get) Token: 0x0600872B RID: 34603 RVA: 0x0033AE21 File Offset: 0x00339021
	public Chunk BlockChunk
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return (Chunk)GameManager.Instance.World.ChunkCache.GetChunkFromWorldPos(this.blockPos);
		}
	}

	// Token: 0x17000FF2 RID: 4082
	// (get) Token: 0x0600872C RID: 34604 RVA: 0x0033AE44 File Offset: 0x00339044
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

	// Token: 0x17000FF3 RID: 4083
	// (get) Token: 0x0600872D RID: 34605 RVA: 0x0033AE9E File Offset: 0x0033909E
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

	// Token: 0x0600872E RID: 34606 RVA: 0x0033AEB4 File Offset: 0x003390B4
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

	// Token: 0x0600872F RID: 34607 RVA: 0x0033B05F File Offset: 0x0033925F
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

	// Token: 0x06008730 RID: 34608 RVA: 0x0033B08F File Offset: 0x0033928F
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

	// Token: 0x06008731 RID: 34609 RVA: 0x0033B0BF File Offset: 0x003392BF
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

	// Token: 0x06008732 RID: 34610 RVA: 0x0033B0EF File Offset: 0x003392EF
	[PublicizedFrom(EAccessModifier.Private)]
	public void setChunkModified()
	{
		PrefabEditModeManager.Instance.NeedsSaving = true;
		this.BlockChunk.isModified = true;
	}

	// Token: 0x06008733 RID: 34611 RVA: 0x0033B108 File Offset: 0x00339308
	public override void Cleanup()
	{
		base.Cleanup();
		if (SelectionBoxManager.Instance != null)
		{
			SelectionBoxManager.Instance.CategoryTriggerVolume.SetCallback(null);
		}
	}

	// Token: 0x06008734 RID: 34612 RVA: 0x0033B12D File Offset: 0x0033932D
	[PublicizedFrom(EAccessModifier.Private)]
	public void TriggerOnAddTriggersPressed(XUiController _sender, int _mouseButton)
	{
		this.prefab.AddNewTriggerLayer();
		this.triggersList.RebuildList(false);
		this.triggeredByList.RebuildList(false);
	}

	// Token: 0x06008735 RID: 34613 RVA: 0x0033B154 File Offset: 0x00339354
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

	// Token: 0x06008736 RID: 34614 RVA: 0x0033B1A0 File Offset: 0x003393A0
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

	// Token: 0x06008737 RID: 34615 RVA: 0x0029936D File Offset: 0x0029756D
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x06008738 RID: 34616 RVA: 0x0033B1EA File Offset: 0x003393EA
	public override void OnClose()
	{
		base.OnClose();
		this.Prefab = null;
		this.blockTrigger = null;
	}

	// Token: 0x06008739 RID: 34617 RVA: 0x0008A9BD File Offset: 0x00088BBD
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x0600873A RID: 34618 RVA: 0x0033B200 File Offset: 0x00339400
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

	// Token: 0x0600873B RID: 34619 RVA: 0x0033B2E4 File Offset: 0x003394E4
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

	// Token: 0x0600873C RID: 34620 RVA: 0x0033B3D0 File Offset: 0x003395D0
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

	// Token: 0x0600873D RID: 34621 RVA: 0x0033B430 File Offset: 0x00339630
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

	// Token: 0x0600873E RID: 34622 RVA: 0x00020119 File Offset: 0x0001E319
	public bool OnSelectionBoxActivated(SelectionBox _box, bool _bActivated)
	{
		return true;
	}

	// Token: 0x0600873F RID: 34623 RVA: 0x0033B4A8 File Offset: 0x003396A8
	public void OnSelectionBoxMoved(SelectionBox _box, Vector3 _moveVector)
	{
		PrefabVolumeManager.Instance.SelectionBoxMoved(PrefabVolumeAbs.EVolumeType.Trigger, _box, _moveVector);
	}

	// Token: 0x06008740 RID: 34624 RVA: 0x0033B4B8 File Offset: 0x003396B8
	public void OnSelectionBoxSized(SelectionBox _box, int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest)
	{
		PrefabVolumeManager.Instance.SelectionBoxSized(PrefabVolumeAbs.EVolumeType.Trigger, _box, _dTop, _dBottom, _dNorth, _dSouth, _dEast, _dWest);
	}

	// Token: 0x06008741 RID: 34625 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxMirrored(Vector3i _axis)
	{
	}

	// Token: 0x06008742 RID: 34626 RVA: 0x0033B4DC File Offset: 0x003396DC
	public bool OnSelectionBoxDelete(SelectionBox _box, bool _checkCanDeleteOnly)
	{
		int num;
		PrefabInstance prefabInstance;
		return PrefabVolumeManager.GetPrefabIdAndVolumeId(_box.name, out num, out prefabInstance) && PrefabVolumeManager.Instance.SelectionBoxDelete(PrefabVolumeAbs.EVolumeType.Trigger, _box, _checkCanDeleteOnly);
	}

	// Token: 0x06008743 RID: 34627 RVA: 0x0033B509 File Offset: 0x00339709
	public bool OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities _criteria)
	{
		return _criteria == EnumSelectionBoxAvailabilities.CanShowProperties || _criteria == EnumSelectionBoxAvailabilities.CanResize;
	}

	// Token: 0x06008744 RID: 34628 RVA: 0x0033B514 File Offset: 0x00339714
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

	// Token: 0x06008745 RID: 34629 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxRotated(SelectionBox _box)
	{
	}

	// Token: 0x06008746 RID: 34630 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxUserDataChanged(SelectionBox _box)
	{
	}

	// Token: 0x04006485 RID: 25733
	public static string ID = "";

	// Token: 0x04006486 RID: 25734
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PrefabTriggerEditorList triggersList;

	// Token: 0x04006487 RID: 25735
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PrefabTriggerEditorList triggeredByList;

	// Token: 0x04006488 RID: 25736
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TriggerProperties.EOpenedFor openedFor;

	// Token: 0x04006489 RID: 25737
	[PublicizedFrom(EAccessModifier.Private)]
	public Prefab prefab;

	// Token: 0x0400648A RID: 25738
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x0400648B RID: 25739
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockTrigger blockTrigger;

	// Token: 0x0400648C RID: 25740
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showTriggers = true;

	// Token: 0x0400648D RID: 25741
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showTriggeredBy = true;

	// Token: 0x020010A4 RID: 4260
	[PublicizedFrom(EAccessModifier.Private)]
	public enum EOpenedFor
	{
		// Token: 0x0400648F RID: 25743
		Block,
		// Token: 0x04006490 RID: 25744
		TriggerVolume
	}
}
