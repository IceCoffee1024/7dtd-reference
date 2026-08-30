using System;
using System.Collections.Generic;
using PrefabVolumes;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001092 RID: 4242
[Preserve]
public class XUiC_TriggerProperties : XUiController, ISelectionBoxCallback
{
	// Token: 0x17000FBA RID: 4026
	// (set) Token: 0x060086DA RID: 34522 RVA: 0x00336FB6 File Offset: 0x003351B6
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

	// Token: 0x17000FBB RID: 4027
	// (set) Token: 0x060086DB RID: 34523 RVA: 0x00336FE4 File Offset: 0x003351E4
	public Vector3i BlockPos
	{
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.blockPos = value;
			this.blockTrigger = this.BlockChunk.GetBlockTrigger(World.toBlock(this.blockPos));
		}
	}

	// Token: 0x17000FBC RID: 4028
	// (get) Token: 0x060086DC RID: 34524 RVA: 0x00337009 File Offset: 0x00335209
	public Chunk BlockChunk
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return (Chunk)GameManager.Instance.World.ChunkCache.GetChunkFromWorldPos(this.blockPos);
		}
	}

	// Token: 0x17000FBD RID: 4029
	// (get) Token: 0x060086DD RID: 34525 RVA: 0x0033702C File Offset: 0x0033522C
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

	// Token: 0x17000FBE RID: 4030
	// (get) Token: 0x060086DE RID: 34526 RVA: 0x00337086 File Offset: 0x00335286
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

	// Token: 0x060086DF RID: 34527 RVA: 0x0033709C File Offset: 0x0033529C
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

	// Token: 0x060086E0 RID: 34528 RVA: 0x00337247 File Offset: 0x00335447
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

	// Token: 0x060086E1 RID: 34529 RVA: 0x00337277 File Offset: 0x00335477
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

	// Token: 0x060086E2 RID: 34530 RVA: 0x003372A7 File Offset: 0x003354A7
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

	// Token: 0x060086E3 RID: 34531 RVA: 0x003372D7 File Offset: 0x003354D7
	[PublicizedFrom(EAccessModifier.Private)]
	public void setChunkModified()
	{
		PrefabEditModeManager.Instance.NeedsSaving = true;
		this.BlockChunk.isModified = true;
	}

	// Token: 0x060086E4 RID: 34532 RVA: 0x003372F0 File Offset: 0x003354F0
	public override void Cleanup()
	{
		base.Cleanup();
		if (SelectionBoxManager.Instance != null)
		{
			SelectionBoxManager.Instance.CategoryTriggerVolume.SetCallback(null);
		}
	}

	// Token: 0x060086E5 RID: 34533 RVA: 0x00337315 File Offset: 0x00335515
	[PublicizedFrom(EAccessModifier.Private)]
	public void TriggerOnAddTriggersPressed(XUiController _sender, int _mouseButton)
	{
		this.prefab.AddNewTriggerLayer();
		this.triggersList.RebuildList(false);
		this.triggeredByList.RebuildList(false);
	}

	// Token: 0x060086E6 RID: 34534 RVA: 0x0033733C File Offset: 0x0033553C
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

	// Token: 0x060086E7 RID: 34535 RVA: 0x00337388 File Offset: 0x00335588
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

	// Token: 0x060086E8 RID: 34536 RVA: 0x00295FE1 File Offset: 0x002941E1
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x060086E9 RID: 34537 RVA: 0x003373D2 File Offset: 0x003355D2
	public override void OnClose()
	{
		base.OnClose();
		this.Prefab = null;
		this.blockTrigger = null;
	}

	// Token: 0x060086EA RID: 34538 RVA: 0x0008A73D File Offset: 0x0008893D
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x060086EB RID: 34539 RVA: 0x003373E8 File Offset: 0x003355E8
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

	// Token: 0x060086EC RID: 34540 RVA: 0x003374CC File Offset: 0x003356CC
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

	// Token: 0x060086ED RID: 34541 RVA: 0x003375B8 File Offset: 0x003357B8
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

	// Token: 0x060086EE RID: 34542 RVA: 0x00337618 File Offset: 0x00335818
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

	// Token: 0x060086EF RID: 34543 RVA: 0x00020021 File Offset: 0x0001E221
	public bool OnSelectionBoxActivated(SelectionBox _box, bool _bActivated)
	{
		return true;
	}

	// Token: 0x060086F0 RID: 34544 RVA: 0x00337690 File Offset: 0x00335890
	public void OnSelectionBoxMoved(SelectionBox _box, Vector3 _moveVector)
	{
		PrefabVolumeManager.Instance.SelectionBoxMoved(PrefabVolumeAbs.EVolumeType.Trigger, _box, _moveVector);
	}

	// Token: 0x060086F1 RID: 34545 RVA: 0x003376A0 File Offset: 0x003358A0
	public void OnSelectionBoxSized(SelectionBox _box, int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest)
	{
		PrefabVolumeManager.Instance.SelectionBoxSized(PrefabVolumeAbs.EVolumeType.Trigger, _box, _dTop, _dBottom, _dNorth, _dSouth, _dEast, _dWest);
	}

	// Token: 0x060086F2 RID: 34546 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxMirrored(Vector3i _axis)
	{
	}

	// Token: 0x060086F3 RID: 34547 RVA: 0x003376C4 File Offset: 0x003358C4
	public bool OnSelectionBoxDelete(SelectionBox _box, bool _checkCanDeleteOnly)
	{
		int num;
		PrefabInstance prefabInstance;
		return PrefabVolumeManager.GetPrefabIdAndVolumeId(_box.name, out num, out prefabInstance) && PrefabVolumeManager.Instance.SelectionBoxDelete(PrefabVolumeAbs.EVolumeType.Trigger, _box, _checkCanDeleteOnly);
	}

	// Token: 0x060086F4 RID: 34548 RVA: 0x003376F1 File Offset: 0x003358F1
	public bool OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities _criteria)
	{
		return _criteria == EnumSelectionBoxAvailabilities.CanShowProperties || _criteria == EnumSelectionBoxAvailabilities.CanResize;
	}

	// Token: 0x060086F5 RID: 34549 RVA: 0x003376FC File Offset: 0x003358FC
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

	// Token: 0x060086F6 RID: 34550 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxRotated(SelectionBox _box)
	{
	}

	// Token: 0x060086F7 RID: 34551 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxUserDataChanged(SelectionBox _box)
	{
	}

	// Token: 0x04006443 RID: 25667
	public static string ID = "";

	// Token: 0x04006444 RID: 25668
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PrefabTriggerEditorList triggersList;

	// Token: 0x04006445 RID: 25669
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PrefabTriggerEditorList triggeredByList;

	// Token: 0x04006446 RID: 25670
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TriggerProperties.EOpenedFor openedFor;

	// Token: 0x04006447 RID: 25671
	[PublicizedFrom(EAccessModifier.Private)]
	public Prefab prefab;

	// Token: 0x04006448 RID: 25672
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x04006449 RID: 25673
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockTrigger blockTrigger;

	// Token: 0x0400644A RID: 25674
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showTriggers = true;

	// Token: 0x0400644B RID: 25675
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showTriggeredBy = true;

	// Token: 0x02001093 RID: 4243
	[PublicizedFrom(EAccessModifier.Private)]
	public enum EOpenedFor
	{
		// Token: 0x0400644D RID: 25677
		Block,
		// Token: 0x0400644E RID: 25678
		TriggerVolume
	}
}
