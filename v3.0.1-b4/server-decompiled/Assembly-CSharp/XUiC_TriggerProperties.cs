using System;
using System.Collections.Generic;
using PrefabVolumes;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001079 RID: 4217
[Preserve]
public class XUiC_TriggerProperties : XUiController, ISelectionBoxCallback
{
	// Token: 0x17000F81 RID: 3969
	// (set) Token: 0x060085E8 RID: 34280 RVA: 0x0033278E File Offset: 0x0033098E
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

	// Token: 0x17000F82 RID: 3970
	// (set) Token: 0x060085E9 RID: 34281 RVA: 0x003327BC File Offset: 0x003309BC
	public Vector3i BlockPos
	{
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.blockPos = value;
			this.blockTrigger = this.BlockChunk.GetBlockTrigger(World.toBlock(this.blockPos));
		}
	}

	// Token: 0x17000F83 RID: 3971
	// (get) Token: 0x060085EA RID: 34282 RVA: 0x003327E1 File Offset: 0x003309E1
	public Chunk BlockChunk
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return (Chunk)GameManager.Instance.World.ChunkCache.GetChunkFromWorldPos(this.blockPos);
		}
	}

	// Token: 0x17000F84 RID: 3972
	// (get) Token: 0x060085EB RID: 34283 RVA: 0x00332804 File Offset: 0x00330A04
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

	// Token: 0x17000F85 RID: 3973
	// (get) Token: 0x060085EC RID: 34284 RVA: 0x0033285E File Offset: 0x00330A5E
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

	// Token: 0x060085ED RID: 34285 RVA: 0x00332874 File Offset: 0x00330A74
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

	// Token: 0x060085EE RID: 34286 RVA: 0x00332A1F File Offset: 0x00330C1F
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

	// Token: 0x060085EF RID: 34287 RVA: 0x00332A4F File Offset: 0x00330C4F
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

	// Token: 0x060085F0 RID: 34288 RVA: 0x00332A7F File Offset: 0x00330C7F
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

	// Token: 0x060085F1 RID: 34289 RVA: 0x00332AAF File Offset: 0x00330CAF
	[PublicizedFrom(EAccessModifier.Private)]
	public void setChunkModified()
	{
		PrefabEditModeManager.Instance.NeedsSaving = true;
		this.BlockChunk.isModified = true;
	}

	// Token: 0x060085F2 RID: 34290 RVA: 0x00332AC8 File Offset: 0x00330CC8
	public override void Cleanup()
	{
		base.Cleanup();
		if (SelectionBoxManager.Instance != null)
		{
			SelectionBoxManager.Instance.CategoryTriggerVolume.SetCallback(null);
		}
	}

	// Token: 0x060085F3 RID: 34291 RVA: 0x00332AED File Offset: 0x00330CED
	[PublicizedFrom(EAccessModifier.Private)]
	public void TriggerOnAddTriggersPressed(XUiController _sender, int _mouseButton)
	{
		this.prefab.AddNewTriggerLayer();
		this.triggersList.RebuildList(false);
		this.triggeredByList.RebuildList(false);
	}

	// Token: 0x060085F4 RID: 34292 RVA: 0x00332B14 File Offset: 0x00330D14
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

	// Token: 0x060085F5 RID: 34293 RVA: 0x00332B60 File Offset: 0x00330D60
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

	// Token: 0x060085F6 RID: 34294 RVA: 0x00292E8D File Offset: 0x0029108D
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x060085F7 RID: 34295 RVA: 0x00332BAA File Offset: 0x00330DAA
	public override void OnClose()
	{
		base.OnClose();
		this.Prefab = null;
		this.blockTrigger = null;
	}

	// Token: 0x060085F8 RID: 34296 RVA: 0x0008A091 File Offset: 0x00088291
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x060085F9 RID: 34297 RVA: 0x00332BC0 File Offset: 0x00330DC0
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

	// Token: 0x060085FA RID: 34298 RVA: 0x00332CA4 File Offset: 0x00330EA4
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

	// Token: 0x060085FB RID: 34299 RVA: 0x00332D90 File Offset: 0x00330F90
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

	// Token: 0x060085FC RID: 34300 RVA: 0x00332DF0 File Offset: 0x00330FF0
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

	// Token: 0x060085FD RID: 34301 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public bool OnSelectionBoxActivated(SelectionBox _box, bool _bActivated)
	{
		return true;
	}

	// Token: 0x060085FE RID: 34302 RVA: 0x00332E68 File Offset: 0x00331068
	public void OnSelectionBoxMoved(SelectionBox _box, Vector3 _moveVector)
	{
		PrefabVolumeManager.Instance.SelectionBoxMoved(PrefabVolumeAbs.EVolumeType.Trigger, _box, _moveVector);
	}

	// Token: 0x060085FF RID: 34303 RVA: 0x00332E78 File Offset: 0x00331078
	public void OnSelectionBoxSized(SelectionBox _box, int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest)
	{
		PrefabVolumeManager.Instance.SelectionBoxSized(PrefabVolumeAbs.EVolumeType.Trigger, _box, _dTop, _dBottom, _dNorth, _dSouth, _dEast, _dWest);
	}

	// Token: 0x06008600 RID: 34304 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxMirrored(Vector3i _axis)
	{
	}

	// Token: 0x06008601 RID: 34305 RVA: 0x00332E9C File Offset: 0x0033109C
	public bool OnSelectionBoxDelete(SelectionBox _box, bool _checkCanDeleteOnly)
	{
		int num;
		PrefabInstance prefabInstance;
		return PrefabVolumeManager.GetPrefabIdAndVolumeId(_box.name, out num, out prefabInstance) && PrefabVolumeManager.Instance.SelectionBoxDelete(PrefabVolumeAbs.EVolumeType.Trigger, _box, _checkCanDeleteOnly);
	}

	// Token: 0x06008602 RID: 34306 RVA: 0x00332EC9 File Offset: 0x003310C9
	public bool OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities _criteria)
	{
		return _criteria == EnumSelectionBoxAvailabilities.CanShowProperties || _criteria == EnumSelectionBoxAvailabilities.CanResize;
	}

	// Token: 0x06008603 RID: 34307 RVA: 0x00332ED4 File Offset: 0x003310D4
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

	// Token: 0x06008604 RID: 34308 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxRotated(SelectionBox _box)
	{
	}

	// Token: 0x06008605 RID: 34309 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxUserDataChanged(SelectionBox _box)
	{
	}

	// Token: 0x0400637C RID: 25468
	public static string ID = "";

	// Token: 0x0400637D RID: 25469
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PrefabTriggerEditorList triggersList;

	// Token: 0x0400637E RID: 25470
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PrefabTriggerEditorList triggeredByList;

	// Token: 0x0400637F RID: 25471
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TriggerProperties.EOpenedFor openedFor;

	// Token: 0x04006380 RID: 25472
	[PublicizedFrom(EAccessModifier.Private)]
	public Prefab prefab;

	// Token: 0x04006381 RID: 25473
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x04006382 RID: 25474
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockTrigger blockTrigger;

	// Token: 0x04006383 RID: 25475
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showTriggers = true;

	// Token: 0x04006384 RID: 25476
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showTriggeredBy = true;

	// Token: 0x0200107A RID: 4218
	[PublicizedFrom(EAccessModifier.Private)]
	public enum EOpenedFor
	{
		// Token: 0x04006386 RID: 25478
		Block,
		// Token: 0x04006387 RID: 25479
		TriggerVolume
	}
}
