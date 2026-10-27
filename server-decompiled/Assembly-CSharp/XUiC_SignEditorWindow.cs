using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200104A RID: 4170
[Preserve]
public class XUiC_SignEditorWindow : XUiController
{
	// Token: 0x17000F74 RID: 3956
	// (get) Token: 0x0600833E RID: 33598 RVA: 0x0031F321 File Offset: 0x0031D521
	public XUiC_SignEditorWindow.AffectMode CurrentAffectMode
	{
		get
		{
			return this.currentAffectMode;
		}
	}

	// Token: 0x0600833F RID: 33599 RVA: 0x0031F32C File Offset: 0x0031D52C
	public override void Init()
	{
		base.Init();
		XUiC_SignEditorWindow.ID = base.WindowGroup.Id;
		base.WindowGroup.isEscClosable = false;
		this.canvasView = base.GetChildById("canvasView");
		this.canvasView.OnDrag += this.HandleCanvasDragged;
		this.canvasView.OnScroll += this.HandleCanvasScrolled;
		this.newLayerPanel = base.GetChildByType<XUiC_SignNewLayerPanel>();
		XUiC_SignNewLayerPanel xuiC_SignNewLayerPanel = this.newLayerPanel;
		xuiC_SignNewLayerPanel.OnLayerTypeSelected = (Action<SignData.SignLayer>)Delegate.Combine(xuiC_SignNewLayerPanel.OnLayerTypeSelected, new Action<SignData.SignLayer>(this.HandleLayerTypeSelected));
		this.signMaterial = base.GetChildById("canvasMaterial").ViewComponent as XUiV_Texture;
		this.signMaterial.CreateMaterial("Game/SignTech/UI");
		XUiV_Texture xuiV_Texture = this.signMaterial;
		xuiV_Texture.OnRenderTexture = (UIDrawCall.OnRenderCallback)Delegate.Combine(xuiV_Texture.OnRenderTexture, new UIDrawCall.OnRenderCallback(this.OnWillRender));
		this.signMaterial.Texture = Texture2D.whiteTexture;
		this.previewAspect = (float)this.signMaterial.Size.x / (float)this.signMaterial.Size.y;
		this.pulseOverlayMaterial = base.GetChildById("pulseOverlay").ViewComponent as XUiV_Texture;
		this.pulseOverlayMaterial.CreateMaterial("Game/SignTech/UI");
		XUiV_Texture xuiV_Texture2 = this.pulseOverlayMaterial;
		xuiV_Texture2.OnRenderTexture = (UIDrawCall.OnRenderCallback)Delegate.Combine(xuiV_Texture2.OnRenderTexture, new UIDrawCall.OnRenderCallback(this.OnPulseOverlayWillRender));
		this.pulseOverlayMaterial.Texture = null;
		this.hoverOverlayMaterial = base.GetChildById("hoverOverlay").ViewComponent as XUiV_Texture;
		this.hoverOverlayMaterial.CreateMaterial("Game/SignTech/UI");
		XUiV_Texture xuiV_Texture3 = this.hoverOverlayMaterial;
		xuiV_Texture3.OnRenderTexture = (UIDrawCall.OnRenderCallback)Delegate.Combine(xuiV_Texture3.OnRenderTexture, new UIDrawCall.OnRenderCallback(this.OnHoverOverlayWillRender));
		this.hoverOverlayMaterial.Texture = null;
		this.categoryList = base.GetChildByType<XUiC_CategoryList>();
		this.categoryList.CategoryChanged += this.CategoryList_CategoryChanged;
		this.signLayerGrid = base.GetChildByType<XUiC_SignLayerGrid>();
		XUiC_SignLayerGrid xuiC_SignLayerGrid = this.signLayerGrid;
		xuiC_SignLayerGrid.OnLayerSelected = (Action<int>)Delegate.Combine(xuiC_SignLayerGrid.OnLayerSelected, new Action<int>(this.HandleLayerSelected));
		XUiC_SignLayerGrid xuiC_SignLayerGrid2 = this.signLayerGrid;
		xuiC_SignLayerGrid2.OnLayerHovered = (Action<int>)Delegate.Combine(xuiC_SignLayerGrid2.OnLayerHovered, new Action<int>(this.HandleLayerHovered));
		XUiC_SignLayerGrid xuiC_SignLayerGrid3 = this.signLayerGrid;
		xuiC_SignLayerGrid3.OnMultiSelectionDeselect = (XUiC_SignLayerGrid.MultiSelectionDeselectCallback)Delegate.Combine(xuiC_SignLayerGrid3.OnMultiSelectionDeselect, new XUiC_SignLayerGrid.MultiSelectionDeselectCallback(this.HandleMultiSelectionDeselect));
		this.addWarpSettings = base.GetChildByType<XUiC_AddWarpSettings>();
		XUiC_AddWarpSettings xuiC_AddWarpSettings = this.addWarpSettings;
		xuiC_AddWarpSettings.OnWarpAdded = (Action<string>)Delegate.Combine(xuiC_AddWarpSettings.OnWarpAdded, new Action<string>(this.HandleWarpAdded));
		this.btnSave = (XUiC_SimpleButton)base.GetChildById("btnSave");
		this.btnSaveCopy = (XUiC_SimpleButton)base.GetChildById("btnSaveCopy");
		this.btnQuit = (XUiC_SimpleButton)base.GetChildById("btnQuit");
		this.btnSave.OnPressed += this.BtnSave_OnPressed;
		this.btnSaveCopy.OnPressed += this.BtnSaveCopy_OnPressed;
		this.btnQuit.OnPressed += this.BtnQuit_OnPressed;
		this.txtAspectHorizontal = (XUiC_TextInput)base.GetChildById("txtRatio").GetChildById("valueA");
		this.txtAspectVertical = (XUiC_TextInput)base.GetChildById("txtRatio").GetChildById("valueB");
		this.txtAspectHorizontal.OnChangeHandler += this.OnAspectText_OnChangeHandler;
		this.txtAspectVertical.OnChangeHandler += this.OnAspectText_OnChangeHandler;
		XUiC_SignEditorControl xuiC_SignEditorControl = base.GetChildById("txtRatio") as XUiC_SignEditorControl;
		if (xuiC_SignEditorControl != null)
		{
			xuiC_SignEditorControl.defaultValue = new ValueTuple<float, float>(1f, 1f);
		}
		this.btnAspectSquare = (XUiC_SimpleButton)base.GetChildById("btnAspectSquare");
		this.btnAspectFit = (XUiC_SimpleButton)base.GetChildById("btnAspectFit");
		this.btnAspectFit.OnPressed += this.BtnAspectFit_OnPressed;
		this.btnAspectSquare.OnPressed += this.BtnAspectSquare_OnPressed;
		this.confirmationPrompt = base.GetChildById("confirmation_prompt_controller") as XUiC_ConfirmationPrompt;
		this.txtSignName = base.GetChildById("txtSignName") as XUiC_TextInput;
		this.txtSignName.OnChangeHandler += this.TxtSignName_OnChangeHandler;
		this.txtLayerName = base.GetChildById("txtLayerName") as XUiC_TextInput;
		this.txtLayerName.OnChangeHandler += this.TxtLayerName_OnChangeHandler;
		this.sizeBar = base.GetChildByType<XUiC_SizeBar>();
		this.sizeBar.SetAllowance(600f);
		base.GetChildrenByType<XUiC_SignLayerSettings>(this.allSettings);
		base.GetChildrenByType<XUiC_SignWarpSettings>(this.allWarpSettings);
		base.GetChildrenByType<XUiC_SignColorSettings>(this.allColorSettings);
		this.transformSettings = base.GetChildByType<XUiC_SignTransformSettings>();
		foreach (XUiC_SignLayerSettings xuiC_SignLayerSettings in this.allSettings)
		{
			xuiC_SignLayerSettings.OnPreLayerSettingsChanged = (Action<string, bool>)Delegate.Combine(xuiC_SignLayerSettings.OnPreLayerSettingsChanged, new Action<string, bool>(delegate(string changeDescription, bool forceDirty)
			{
				this.OnBeforeApplyingChange(changeDescription, forceDirty);
			}));
			xuiC_SignLayerSettings.OnLayerSettingsChanged = (Action)Delegate.Combine(xuiC_SignLayerSettings.OnLayerSettingsChanged, new Action(this.MarkChanged));
			XUiC_SignGroupSettings xuiC_SignGroupSettings = xuiC_SignLayerSettings as XUiC_SignGroupSettings;
			if (xuiC_SignGroupSettings != null)
			{
				XUiC_SignGroupSettings xuiC_SignGroupSettings2 = xuiC_SignGroupSettings;
				xuiC_SignGroupSettings2.OnAffectModePressed = (Action<XUiC_SignEditorWindow.AffectMode>)Delegate.Combine(xuiC_SignGroupSettings2.OnAffectModePressed, new Action<XUiC_SignEditorWindow.AffectMode>(this.RequestAffectMode));
				this.OnAffectModeChanged = (Action<XUiC_SignEditorWindow.AffectMode>)Delegate.Combine(this.OnAffectModeChanged, new Action<XUiC_SignEditorWindow.AffectMode>(xuiC_SignGroupSettings.SetAffectModeVisual));
			}
		}
		foreach (XUiC_SignWarpSettings xuiC_SignWarpSettings in this.allWarpSettings)
		{
			xuiC_SignWarpSettings.OnWarpRemoved = (Action)Delegate.Combine(xuiC_SignWarpSettings.OnWarpRemoved, new Action(this.HandleWarpRemoved));
		}
		foreach (XUiC_SignColorSettings xuiC_SignColorSettings in this.allColorSettings)
		{
			xuiC_SignColorSettings.OnMaskModeChanged = (Action)Delegate.Combine(xuiC_SignColorSettings.OnMaskModeChanged, new Action(this.HandleMaskModeChanged));
		}
		this.debugPanel = base.GetChildByType<XUiC_SignDebugPanel>();
		this.sourceSignData = SignData.GetTestSignData("Sign Editor", 0f);
		this.sourceSignData.UnpackGroups(false);
		this.sourceSignId = SignDataManager.Instance.AddSignToLibrary("[I]", this.sourceSignData);
		this.dragAndDropIcon = base.GetChildByType<XUiC_SignLayerDragAndDropIcon>();
		XUiC_SignLayerGrid xuiC_SignLayerGrid4 = this.signLayerGrid;
		xuiC_SignLayerGrid4.OnDragAndDropStarted = (Action<int>)Delegate.Combine(xuiC_SignLayerGrid4.OnDragAndDropStarted, new Action<int>(this.OnDragAndDropStarted));
		XUiC_SignLayerGrid xuiC_SignLayerGrid5 = this.signLayerGrid;
		xuiC_SignLayerGrid5.OnDragAndDropReleased = (Action<int>)Delegate.Combine(xuiC_SignLayerGrid5.OnDragAndDropReleased, new Action<int>(this.OnDragAndDropReleased));
		XUiC_SignLayerGrid xuiC_SignLayerGrid6 = this.signLayerGrid;
		xuiC_SignLayerGrid6.OnAddLayerPressed = (Action<int>)Delegate.Combine(xuiC_SignLayerGrid6.OnAddLayerPressed, new Action<int>(this.BeginAddLayerFlow));
	}

	// Token: 0x06008340 RID: 33600 RVA: 0x0031FA7C File Offset: 0x0031DC7C
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtLayerName_OnChangeHandler(XUiController sender, string text, bool changeFromCode)
	{
		if (changeFromCode)
		{
			return;
		}
		this.signLayerGrid.SetLayerName(text);
		this.MarkChanged();
	}

	// Token: 0x06008341 RID: 33601 RVA: 0x0031FA94 File Offset: 0x0031DC94
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleMultiSelectionDeselect(bool wasPrimary)
	{
		if (!wasPrimary)
		{
			this.RefreshComplexityInfo();
		}
	}

	// Token: 0x06008342 RID: 33602 RVA: 0x0031FAA0 File Offset: 0x0031DCA0
	public static void Open(LocalPlayerUI _playerUi, SignData signData, GlobalSignId signId)
	{
		XUiC_SignEditorWindow childByType = _playerUi.xui.FindWindowGroupByName(XUiC_SignEditorWindow.ID).GetChildByType<XUiC_SignEditorWindow>();
		childByType.targetCanvas = null;
		childByType.targetEntity = null;
		childByType.sourceSignId = signId;
		childByType.sourceSignData = signData;
		_playerUi.windowManager.Open(XUiC_SignEditorWindow.ID, true);
	}

	// Token: 0x06008343 RID: 33603 RVA: 0x0031FAF0 File Offset: 0x0031DCF0
	public static void Open(LocalPlayerUI _playerUi, TEFeatureCanvas _feature)
	{
		XUiC_SignEditorWindow childByType = _playerUi.xui.FindWindowGroupByName(XUiC_SignEditorWindow.ID).GetChildByType<XUiC_SignEditorWindow>();
		SignCanvas canvas = _feature.Canvas;
		childByType.targetCanvas = canvas;
		childByType.targetEntity = _feature;
		if (canvas != null && SignDataManager.Instance.TryGetSignData(canvas.SignId, out childByType.sourceSignData))
		{
			childByType.sourceSignId = canvas.SignId;
		}
		else
		{
			childByType.sourceSignId = GlobalSignId.InvalidId;
			childByType.sourceSignData = null;
		}
		_playerUi.windowManager.Open(XUiC_SignEditorWindow.ID, true);
	}

	// Token: 0x06008344 RID: 33604 RVA: 0x0031FB7C File Offset: 0x0031DD7C
	public override void OnOpen()
	{
		base.OnOpen();
		this.releaseLockOnClose = this.targetEntity != null;
		this.openedInEditMode = PrefabEditModeManager.Instance.IsActive();
		this.CreateWorkingCopy();
		this.UpdateAspect();
		this.ResetView();
		this.hasUnsavedChanges = false;
		this.showNewLayerPanel = false;
		this.txtSignName.Text = this.workingSignData.name;
		int count = this.workingSignData.layers.Count;
		if (count > 0)
		{
			this.signLayerGrid.MultiSelectedLayerIndices.Add(0);
		}
		this.signLayerGrid.UpdateLayers(this.workingSignId, 0, count == 0, true);
		this.Refresh();
		this.RefreshComplexityInfo();
		this.historyStateManager.Init("Editor Opened", this.signLayerGrid, this.txtSignName, this.workingSignId, this.workingSignData, this);
		this.xui.playerUI.windowManager.IsInputLocked = true;
	}

	// Token: 0x06008345 RID: 33605 RVA: 0x0031FC6C File Offset: 0x0031DE6C
	public override void OnClose()
	{
		base.OnClose();
		this.openedInEditMode = false;
		this.xui.playerUI.windowManager.IsInputLocked = false;
		if (this.releaseLockOnClose)
		{
			Log.Warning("Unexpected exit flow from Sign Editor. Force-releasing lock on target entity.");
			this.targetEntity.ReleaseLock();
		}
		this.historyStateManager.Reset();
	}

	// Token: 0x06008346 RID: 33606 RVA: 0x0031FCC4 File Offset: 0x0031DEC4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnBeforeApplyingChange(string changeDescription, bool forceDirty = false)
	{
		this.historyStateManager.ProcessPendingChange(changeDescription, forceDirty);
	}

	// Token: 0x06008347 RID: 33607 RVA: 0x0031FCD3 File Offset: 0x0031DED3
	public void RequestAffectMode(XUiC_SignEditorWindow.AffectMode requestedMode)
	{
		if (!(this.selectedLayer is SignData.GroupSignLayer))
		{
			this.SetAffectMode(XUiC_SignEditorWindow.AffectMode.None);
			return;
		}
		if (this.currentAffectMode == requestedMode)
		{
			this.SetAffectMode(XUiC_SignEditorWindow.AffectMode.None);
			return;
		}
		this.SetAffectMode(requestedMode);
	}

	// Token: 0x06008348 RID: 33608 RVA: 0x0031FD02 File Offset: 0x0031DF02
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetAffectMode(XUiC_SignEditorWindow.AffectMode newMode)
	{
		if (this.currentAffectMode == newMode)
		{
			return;
		}
		this.currentAffectMode = newMode;
		this.historyStateManager.ForceDirty();
		Action<XUiC_SignEditorWindow.AffectMode> onAffectModeChanged = this.OnAffectModeChanged;
		if (onAffectModeChanged == null)
		{
			return;
		}
		onAffectModeChanged(this.currentAffectMode);
	}

	// Token: 0x06008349 RID: 33609 RVA: 0x0031FD36 File Offset: 0x0031DF36
	public void RestoreAffectModeFromHistory(XUiC_SignEditorWindow.AffectMode mode)
	{
		if (!(this.selectedLayer is SignData.GroupSignLayer))
		{
			mode = XUiC_SignEditorWindow.AffectMode.None;
		}
		if (this.currentAffectMode != mode)
		{
			this.currentAffectMode = mode;
			Action<XUiC_SignEditorWindow.AffectMode> onAffectModeChanged = this.OnAffectModeChanged;
			if (onAffectModeChanged == null)
			{
				return;
			}
			onAffectModeChanged(this.currentAffectMode);
		}
	}

	// Token: 0x0600834A RID: 33610 RVA: 0x0031FD70 File Offset: 0x0031DF70
	[PublicizedFrom(EAccessModifier.Private)]
	public static Vector2 WorldDeltaToGroupLocalDelta(SignData.GroupSignLayer group, Vector2 worldDelta)
	{
		float num = -group.transform.rotation * 0.017453292f;
		float num2 = Mathf.Cos(num);
		float num3 = Mathf.Sin(num);
		Vector2 vector = new Vector2(worldDelta.x * num2 - worldDelta.y * num3, worldDelta.x * num3 + worldDelta.y * num2);
		Vector2 scale = group.transform.scale;
		float num4 = (Mathf.Approximately(scale.x, 0f) ? 0f : (1f / scale.x));
		float num5 = (Mathf.Approximately(scale.y, 0f) ? 0f : (1f / scale.y));
		return new Vector2(vector.x * num4, vector.y * num5);
	}

	// Token: 0x0600834B RID: 33611 RVA: 0x0031FE38 File Offset: 0x0031E038
	[PublicizedFrom(EAccessModifier.Private)]
	public static void RotateGroupChildrenAroundOrigin(SignData.GroupSignLayer group, float deltaDegrees)
	{
		float num = deltaDegrees * 0.017453292f;
		float num2 = Mathf.Cos(num);
		float num3 = Mathf.Sin(num);
		for (int i = 0; i < group.layers.Count; i++)
		{
			SignData.SignLayer signLayer = group.layers[i];
			Vector2 position = signLayer.transform.position;
			signLayer.transform.position = new Vector2(position.x * num2 - position.y * num3, position.x * num3 + position.y * num2);
			signLayer.transform.rotation = signLayer.transform.rotation + deltaDegrees;
		}
	}

	// Token: 0x0600834C RID: 33612 RVA: 0x0031FEC8 File Offset: 0x0031E0C8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ScaleGroupChildrenAroundOrigin(SignData.GroupSignLayer group, Vector2 factor)
	{
		for (int i = 0; i < group.layers.Count; i++)
		{
			SignData.SignLayer signLayer = group.layers[i];
			signLayer.transform.position = new Vector2(signLayer.transform.position.x * factor.x, signLayer.transform.position.y * factor.y);
			signLayer.transform.scale = new Vector2(signLayer.transform.scale.x * factor.x, signLayer.transform.scale.y * factor.y);
		}
	}

	// Token: 0x0600834D RID: 33613 RVA: 0x0031FF7C File Offset: 0x0031E17C
	[PublicizedFrom(EAccessModifier.Private)]
	public string DecorateChangeDescription(string baseDescription)
	{
		XUiC_SignEditorWindow.AffectMode affectMode = this.currentAffectMode;
		if (affectMode == XUiC_SignEditorWindow.AffectMode.Children)
		{
			return baseDescription + " [Affect Children]";
		}
		if (affectMode != XUiC_SignEditorWindow.AffectMode.Pivot)
		{
			return baseDescription;
		}
		return baseDescription + " [Affect Pivot]";
	}

	// Token: 0x0600834E RID: 33614 RVA: 0x0031FFB4 File Offset: 0x0031E1B4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryGetAffectTargetGroup(out SignData.GroupSignLayer group)
	{
		group = null;
		if (this.currentAffectMode == XUiC_SignEditorWindow.AffectMode.None)
		{
			return false;
		}
		SignData.GroupSignLayer groupSignLayer = this.selectedLayer as SignData.GroupSignLayer;
		if (groupSignLayer == null || groupSignLayer.layers == null || groupSignLayer.layers.Count == 0)
		{
			return false;
		}
		group = groupSignLayer;
		return true;
	}

	// Token: 0x0600834F RID: 33615 RVA: 0x0031FFF8 File Offset: 0x0031E1F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateAspect()
	{
		if (this.targetCanvas != null && this.aspectMaskEnabled)
		{
			this.canvasAspect = ((this.targetCanvas.CanvasAspect > 1f) ? new Vector2(1f, 1f / this.targetCanvas.CanvasAspect) : new Vector2(this.targetCanvas.CanvasAspect, 1f));
			if (this.targetCanvas != null && (this.targetCanvas.CanvasRotation == CanvasRotationMode.Rot90 || this.targetCanvas.CanvasRotation == CanvasRotationMode.Rot270))
			{
				this.canvasAspect = new Vector2(this.canvasAspect.y, this.canvasAspect.x);
			}
		}
		else
		{
			this.canvasAspect = Vector2.one;
		}
		this.OnFrameAspectChanged();
	}

	// Token: 0x06008350 RID: 33616 RVA: 0x003200CC File Offset: 0x0031E2CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnAspectText_OnChangeHandler(XUiController sender, string text, bool changeFromCode)
	{
		if (changeFromCode)
		{
			return;
		}
		float num;
		float num2;
		if (!float.TryParse(this.txtAspectHorizontal.Text, out num) || !float.TryParse(this.txtAspectVertical.Text, out num2))
		{
			return;
		}
		if (num <= 0f || num2 <= 0f)
		{
			return;
		}
		float num3 = num / num2;
		this.canvasAspect = ((num3 > 1f) ? new Vector2(1f, 1f / num3) : new Vector2(num3, 1f));
	}

	// Token: 0x06008351 RID: 33617 RVA: 0x00320148 File Offset: 0x0031E348
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnAspectFit_OnPressed(XUiController sender, int mouseButton)
	{
		this.canvasAspect = ((this.targetCanvas.CanvasAspect > 1f) ? new Vector2(1f, 1f / this.targetCanvas.CanvasAspect) : new Vector2(this.targetCanvas.CanvasAspect, 1f));
		if (this.targetCanvas != null && (this.targetCanvas.CanvasRotation == CanvasRotationMode.Rot90 || this.targetCanvas.CanvasRotation == CanvasRotationMode.Rot270))
		{
			this.canvasAspect = new Vector2(this.canvasAspect.y, this.canvasAspect.x);
		}
		this.OnFrameAspectChanged();
	}

	// Token: 0x06008352 RID: 33618 RVA: 0x003201F0 File Offset: 0x0031E3F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnAspectSquare_OnPressed(XUiController sender, int mouseButton)
	{
		this.canvasAspect = Vector2.one;
		this.OnFrameAspectChanged();
	}

	// Token: 0x06008353 RID: 33619 RVA: 0x00320203 File Offset: 0x0031E403
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnFrameAspectChanged()
	{
		this.txtAspectHorizontal.Text = this.canvasAspect.x.ToString();
		this.txtAspectVertical.Text = this.canvasAspect.y.ToString();
	}

	// Token: 0x06008354 RID: 33620 RVA: 0x0032023C File Offset: 0x0031E43C
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateWorkingCopy()
	{
		this.workingSignData = SignData.Duplicate(this.sourceSignData, "");
		this.workingSignId = SignDataManager.Instance.AddSignToLibrary("[I]", this.workingSignData);
		SignDataManager.Instance.TryGetSignComplexityInfo(this.workingSignId, out this.signComplexityInfo);
	}

	// Token: 0x06008355 RID: 33621 RVA: 0x00320291 File Offset: 0x0031E491
	[PublicizedFrom(EAccessModifier.Private)]
	public void MarkChanged()
	{
		this.hasUnsavedChanges = true;
		SignDataManager.Instance.MarkSignDirty(this.workingSignId);
		this.IsDirty = true;
	}

	// Token: 0x06008356 RID: 33622 RVA: 0x003202B4 File Offset: 0x0031E4B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshComplexityInfo()
	{
		float num = 0f;
		if (this.signComplexityInfo.IsValid)
		{
			this.sizeBar.SetUsed(this.signComplexityInfo.TotalComplexity);
			foreach (int num2 in this.signLayerGrid.MultiSelectedLayerIndices)
			{
				SignData.SignLayer signLayer = this.workingSignData.layers[num2];
				LayerComplexityInfo layerComplexityInfo;
				if (this.signComplexityInfo.TryGetLayerComplexityInfo(signLayer, out layerComplexityInfo))
				{
					if (signLayer == this.selectedLayer)
					{
						this.debugPanel.Populate(in layerComplexityInfo);
					}
					num += layerComplexityInfo.Complexity;
				}
			}
			this.sizeBar.SetSelectedRegion(new XUiC_SizeBar.BarRegionFloat(0f, num));
			this.complexityHoverOffset = num;
			return;
		}
		this.sizeBar.SetUsed(0f);
		this.sizeBar.SetSelectedRegion(new XUiC_SizeBar.BarRegionFloat(0f, 0f));
		this.complexityHoverOffset = 0f;
		this.debugPanel.Clear();
		Log.Error(string.Format("Failed to retrieve valid complexity info for sign: {0}", this.workingSignId));
	}

	// Token: 0x06008357 RID: 33623 RVA: 0x003203EC File Offset: 0x0031E5EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetLayerSelected(SignData.SignLayer layer)
	{
		if (layer != null)
		{
			this.<SetLayerSelected>g__ToggleNewLayerPopup|95_0(false);
			if (this.signLayerGrid.HasPlaceholder)
			{
				int num = this.workingSignData.layers.IndexOf(layer);
				this.signLayerGrid.MultiSelectedLayerIndices.Clear();
				this.signLayerGrid.MultiSelectedLayerIndices.Add(num);
				this.signLayerGrid.UpdateLayers(this.workingSignId, num, false, true);
				return;
			}
		}
		else
		{
			this.<SetLayerSelected>g__ToggleNewLayerPopup|95_0(true);
		}
		if (this.selectedLayer == layer)
		{
			return;
		}
		this.selectedLayer = layer;
		if (this.currentAffectMode != XUiC_SignEditorWindow.AffectMode.None)
		{
			this.SetAffectMode(XUiC_SignEditorWindow.AffectMode.None);
		}
		if (layer != null)
		{
			this.highlightStartTime = Time.time;
			this.pulseOverlayMaterial.Texture = Texture2D.whiteTexture;
		}
		else
		{
			this.pulseOverlayMaterial.Texture = null;
		}
		XUiC_CategoryList xuiC_CategoryList = this.categoryList;
		string text;
		if (xuiC_CategoryList == null)
		{
			text = null;
		}
		else
		{
			XUiC_CategoryEntry currentCategory = xuiC_CategoryList.CurrentCategory;
			text = ((currentCategory != null) ? currentCategory.CategoryName : null);
		}
		this.RefreshLayerSettings(text ?? string.Empty);
	}

	// Token: 0x06008358 RID: 33624 RVA: 0x003204D8 File Offset: 0x0031E6D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshLayerSettings(string autoSelectCategory)
	{
		XUiC_SignEditorWindow.<>c__DisplayClass96_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		int num = this.signLayerGrid.SelectedLayerIndex + 1;
		CS$<>8__locals1.i = 0;
		XUiC_TextInput xuiC_TextInput = this.txtLayerName;
		SignData.SignLayer signLayer = this.selectedLayer;
		xuiC_TextInput.Text = ((signLayer != null) ? signLayer.name : null);
		this.RefreshComplexityInfo();
		SignData.SignLayer signLayer2 = this.selectedLayer;
		int num2;
		if (!(signLayer2 is SignData.GroupSignLayer))
		{
			if (!(signLayer2 is SignData.TextSignLayer))
			{
				if (!(signLayer2 is SignData.PolygonSignLayer))
				{
					if (!(signLayer2 is SignData.NoiseSignLayer))
					{
						if (signLayer2 != null)
						{
							Log.Error("Unsupported layer type selected.");
						}
						else
						{
							this.selectedLayerName = string.Empty;
						}
					}
					else
					{
						this.selectedLayerName = string.Format(Localization.Get("lblSignLayerName", false, null), num.ToString("00")) + " [" + Localization.Get("lblSignNoise", false, null) + "]";
						XUiC_CategoryList xuiC_CategoryList = this.categoryList;
						num2 = CS$<>8__locals1.i;
						CS$<>8__locals1.i = num2 + 1;
						xuiC_CategoryList.SetCategoryEntry(num2, "Noise", "ui_game_symbol_chemistry", Localization.Get("lblSignNoise", false, null));
						XUiC_CategoryList xuiC_CategoryList2 = this.categoryList;
						num2 = CS$<>8__locals1.i;
						CS$<>8__locals1.i = num2 + 1;
						xuiC_CategoryList2.SetCategoryEntry(num2, "Color", "ui_game_symbol_paint_bucket", Localization.Get("lblSignColor", false, null));
						XUiC_CategoryList xuiC_CategoryList3 = this.categoryList;
						num2 = CS$<>8__locals1.i;
						CS$<>8__locals1.i = num2 + 1;
						xuiC_CategoryList3.SetCategoryEntry(num2, "Transform", "ui_game_symbol_wrench", Localization.Get("lblSignTransform", false, null));
						this.<RefreshLayerSettings>g__AddWarpCategories|96_0(ref CS$<>8__locals1);
					}
				}
				else
				{
					this.selectedLayerName = string.Format(Localization.Get("lblSignLayerName", false, null), num.ToString("00")) + " [" + Localization.Get("lblSignPolygon", false, null) + "]";
					XUiC_CategoryList xuiC_CategoryList4 = this.categoryList;
					num2 = CS$<>8__locals1.i;
					CS$<>8__locals1.i = num2 + 1;
					xuiC_CategoryList4.SetCategoryEntry(num2, "Polygon", "ui_game_symbol_all_blocks", Localization.Get("lblSignPolygon", false, null));
					XUiC_CategoryList xuiC_CategoryList5 = this.categoryList;
					num2 = CS$<>8__locals1.i;
					CS$<>8__locals1.i = num2 + 1;
					xuiC_CategoryList5.SetCategoryEntry(num2, "Color", "ui_game_symbol_paint_bucket", Localization.Get("lblSignColor", false, null));
					XUiC_CategoryList xuiC_CategoryList6 = this.categoryList;
					num2 = CS$<>8__locals1.i;
					CS$<>8__locals1.i = num2 + 1;
					xuiC_CategoryList6.SetCategoryEntry(num2, "Transform", "ui_game_symbol_wrench", Localization.Get("lblSignTransform", false, null));
					this.<RefreshLayerSettings>g__AddWarpCategories|96_0(ref CS$<>8__locals1);
				}
			}
			else
			{
				this.selectedLayerName = string.Format(Localization.Get("lblSignLayerName", false, null), num.ToString("00")) + " [" + Localization.Get("lblSignText", false, null) + "]";
				XUiC_CategoryList xuiC_CategoryList7 = this.categoryList;
				num2 = CS$<>8__locals1.i;
				CS$<>8__locals1.i = num2 + 1;
				xuiC_CategoryList7.SetCategoryEntry(num2, "Text", "ui_game_symbol_shape_text", Localization.Get("lblSignText", false, null));
				XUiC_CategoryList xuiC_CategoryList8 = this.categoryList;
				num2 = CS$<>8__locals1.i;
				CS$<>8__locals1.i = num2 + 1;
				xuiC_CategoryList8.SetCategoryEntry(num2, "Color", "ui_game_symbol_paint_bucket", Localization.Get("lblSignColor", false, null));
				XUiC_CategoryList xuiC_CategoryList9 = this.categoryList;
				num2 = CS$<>8__locals1.i;
				CS$<>8__locals1.i = num2 + 1;
				xuiC_CategoryList9.SetCategoryEntry(num2, "Transform", "ui_game_symbol_wrench", Localization.Get("lblSignTransform", false, null));
				this.<RefreshLayerSettings>g__AddWarpCategories|96_0(ref CS$<>8__locals1);
			}
		}
		else
		{
			this.selectedLayerName = string.Format(Localization.Get("lblSignLayerName", false, null), num.ToString("00")) + " [" + Localization.Get("lblSignGroup", false, null) + "]";
			XUiC_CategoryList xuiC_CategoryList10 = this.categoryList;
			num2 = CS$<>8__locals1.i;
			CS$<>8__locals1.i = num2 + 1;
			xuiC_CategoryList10.SetCategoryEntry(num2, "Group", "ui_game_symbol_bundle", Localization.Get("lblSignGroup", false, null));
			XUiC_CategoryList xuiC_CategoryList11 = this.categoryList;
			num2 = CS$<>8__locals1.i;
			CS$<>8__locals1.i = num2 + 1;
			xuiC_CategoryList11.SetCategoryEntry(num2, "Color", "ui_game_symbol_paint_bucket", Localization.Get("lblSignColor", false, null));
			XUiC_CategoryList xuiC_CategoryList12 = this.categoryList;
			num2 = CS$<>8__locals1.i;
			CS$<>8__locals1.i = num2 + 1;
			xuiC_CategoryList12.SetCategoryEntry(num2, "Transform", "ui_game_symbol_wrench", Localization.Get("lblSignTransform", false, null));
			this.<RefreshLayerSettings>g__AddWarpCategories|96_0(ref CS$<>8__locals1);
		}
		using (List<XUiC_SignLayerSettings>.Enumerator enumerator = this.allSettings.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				XUiC_SignLayerSettings xuiC_SignLayerSettings = enumerator.Current;
				xuiC_SignLayerSettings.SetLayer(this.selectedLayer);
			}
			goto IL_047C;
		}
		IL_045A:
		this.categoryList.SetCategoryEmpty(CS$<>8__locals1.i);
		num2 = CS$<>8__locals1.i;
		CS$<>8__locals1.i = num2 + 1;
		IL_047C:
		if (CS$<>8__locals1.i >= this.categoryList.CategoryButtons.Count)
		{
			if (string.IsNullOrEmpty(autoSelectCategory) || !this.categoryList.TrySetCategory(autoSelectCategory))
			{
				this.categoryList.SetCategoryToFirst();
			}
			base.RefreshBindings();
			return;
		}
		goto IL_045A;
	}

	// Token: 0x06008359 RID: 33625 RVA: 0x003209B0 File Offset: 0x0031EBB0
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleWarpAdded(string warpName)
	{
		SignData.SignLayer signLayer = this.selectedLayer;
		this.RefreshLayerSettings((((signLayer != null) ? signLayer.warps : null) != null) ? string.Format("{0}_{1}", warpName, this.selectedLayer.warps.Count - 1) : string.Empty);
	}

	// Token: 0x0600835A RID: 33626 RVA: 0x00320A00 File Offset: 0x0031EC00
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleWarpRemoved()
	{
		int num = this.categoryList.CategoryButtons.IndexOf(this.categoryList.CurrentCategory);
		XUiC_CategoryEntry categoryByIndex = this.categoryList.GetCategoryByIndex(num + 1);
		this.RefreshLayerSettings((categoryByIndex == null) ? string.Empty : categoryByIndex.CategoryName);
	}

	// Token: 0x0600835B RID: 33627 RVA: 0x00320A4E File Offset: 0x0031EC4E
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleMaskModeChanged()
	{
		this.signLayerGrid.RefreshSelectedLayerBindings();
	}

	// Token: 0x0600835C RID: 33628 RVA: 0x00320A5C File Offset: 0x0031EC5C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnWillRender(Material mat)
	{
		SignDataManager.Instance.TryApplyRenderingData(this.workingSignId, this.previewAspect, mat, null, SignUIStyle.MainCanvas);
		mat.SetVector(SignShaderIDs._CanvasAspect, this.canvasAspect);
		Vector4 vector = new Vector4(2f, 2f, -1f - this.viewOffset.x, -1f - this.viewOffset.y) * this.viewScale;
		mat.SetVector(SignShaderIDs._AtlasArray_ST, vector);
		if (this.currentAffectMode != XUiC_SignEditorWindow.AffectMode.None)
		{
			SignData.GroupSignLayer groupSignLayer = this.selectedLayer as SignData.GroupSignLayer;
			if (groupSignLayer != null)
			{
				mat.SetVector(SignShaderIDs._Crosshair, new Vector4(groupSignLayer.transform.position.x, groupSignLayer.transform.position.y, 0.017453292f * groupSignLayer.transform.rotation, 1f));
				return;
			}
		}
		mat.SetVector(SignShaderIDs._Crosshair, new Vector4(0f, 0f, 0f, 0f));
	}

	// Token: 0x0600835D RID: 33629 RVA: 0x00320B64 File Offset: 0x0031ED64
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPulseOverlayWillRender(Material mat)
	{
		if (this.selectedLayer == null)
		{
			return;
		}
		float num = Mathf.Clamp01((Time.time - this.highlightStartTime) / 1f);
		SignDataManager.Instance.TryApplyRenderingData(this.workingSignId, this.previewAspect, mat, this.selectedLayer, SignUIStyle.LayerSelectHighlight);
		mat.SetVector(SignShaderIDs._CanvasAspect, this.canvasAspect);
		mat.SetFloat(SignShaderIDs._HighlightPhase, num);
		Vector4 vector = new Vector4(2f, 2f, -1f - this.viewOffset.x, -1f - this.viewOffset.y) * this.viewScale;
		mat.SetVector(SignShaderIDs._AtlasArray_ST, vector);
	}

	// Token: 0x0600835E RID: 33630 RVA: 0x00320C1C File Offset: 0x0031EE1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnHoverOverlayWillRender(Material mat)
	{
		if (this.hoverOverlayLayer == null)
		{
			return;
		}
		SignDataManager.Instance.TryApplyRenderingData(this.workingSignId, this.previewAspect, mat, this.hoverOverlayLayer, SignUIStyle.LayerHoverHighlight);
		mat.SetVector(SignShaderIDs._CanvasAspect, this.canvasAspect);
		mat.SetFloat(SignShaderIDs._HighlightPhase, this.hoverOverlayOpacity);
		Vector4 vector = new Vector4(2f, 2f, -1f - this.viewOffset.x, -1f - this.viewOffset.y) * this.viewScale;
		mat.SetVector(SignShaderIDs._AtlasArray_ST, vector);
	}

	// Token: 0x0600835F RID: 33631 RVA: 0x00320CC4 File Offset: 0x0031EEC4
	[PublicizedFrom(EAccessModifier.Private)]
	public void CategoryList_CategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		int num = _categoryEntry.CategoryName.IndexOf("_");
		int num2;
		if (num > -1 && int.TryParse(_categoryEntry.CategoryName.Substring(num + 1), out num2))
		{
			foreach (XUiC_SignWarpSettings xuiC_SignWarpSettings in this.allWarpSettings)
			{
				xuiC_SignWarpSettings.SetWarp(this.selectedLayer.warps[num2]);
			}
		}
		base.RefreshBindings();
	}

	// Token: 0x06008360 RID: 33632 RVA: 0x00320D58 File Offset: 0x0031EF58
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSave_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.Save();
	}

	// Token: 0x06008361 RID: 33633 RVA: 0x00320D60 File Offset: 0x0031EF60
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSaveCopy_OnPressed(XUiController sender, int mouseButton)
	{
		this.SaveCopy();
	}

	// Token: 0x06008362 RID: 33634 RVA: 0x00320D68 File Offset: 0x0031EF68
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnQuit_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.Quit(false);
	}

	// Token: 0x06008363 RID: 33635 RVA: 0x00320D74 File Offset: 0x0031EF74
	[PublicizedFrom(EAccessModifier.Private)]
	public void Save()
	{
		if (!this.hasUnsavedChanges || !this.GetSavingAllowed())
		{
			return;
		}
		this.sourceSignData.CloneLayersFrom(this.workingSignData);
		this.sourceSignData.name = this.workingSignData.name;
		if (!SignDataManager.Instance.MarkSignDirty(this.sourceSignId))
		{
			Log.Error(string.Format("Failed to mark source sign data dirty. Source ID: {0}", this.sourceSignId));
		}
		this.hasUnsavedChanges = false;
		this.Refresh();
		if (this.openedInEditMode)
		{
			PrefabEditModeManager.Instance.NeedsSaving = true;
		}
	}

	// Token: 0x06008364 RID: 33636 RVA: 0x00320E08 File Offset: 0x0031F008
	[PublicizedFrom(EAccessModifier.Private)]
	public void SaveCopy()
	{
		if (!this.GetSavingAllowed())
		{
			return;
		}
		string currentSaveTargetLibraryName = XUiC_SignGalleryWindow.GetCurrentSaveTargetLibraryName(this.openedInEditMode);
		SignData signData = SignData.Duplicate(this.workingSignData, SignDataManager.Instance.GetDuplicateName(currentSaveTargetLibraryName, this.workingSignData.name) ?? "");
		GlobalSignId globalSignId = SignDataManager.Instance.AddSignToLibrary(currentSaveTargetLibraryName, signData);
		this.sourceSignId = globalSignId;
		this.sourceSignData = signData;
		this.CreateWorkingCopy();
		this.txtSignName.Text = this.workingSignData.name;
		if (this.targetCanvas != null)
		{
			this.targetCanvas.SignId = this.sourceSignId;
			TEFeatureCanvas tefeatureCanvas = this.targetEntity;
			if (tefeatureCanvas != null)
			{
				tefeatureCanvas.SetModified();
			}
		}
		this.signLayerGrid.UpdateLayers(this.workingSignId, 0, this.workingSignData.layers.Count == 0, true);
		this.historyStateManager.Init("Save As", this.signLayerGrid, this.txtSignName, this.workingSignId, this.workingSignData, this);
		this.hasUnsavedChanges = false;
		this.Refresh();
	}

	// Token: 0x06008365 RID: 33637 RVA: 0x00320F18 File Offset: 0x0031F118
	[PublicizedFrom(EAccessModifier.Private)]
	public void Quit(bool force = false)
	{
		if (this.hasUnsavedChanges && !force)
		{
			this.confirmationPrompt.ShowPrompt(Localization.Get("lblSignDiscardTitle", false, null), Localization.Get("lblSignDiscardMessage", false, null), Localization.Get("xuiCancel", false, null), Localization.Get("xuiConfirm", false, null), new Action<XUiC_ConfirmationPrompt.Result>(this.OnDiscardChangesPromptResult));
			return;
		}
		if (!SignDataManager.Instance.TryDeleteSign(this.workingSignId))
		{
			Log.Error(string.Format("Failed to clean up temporary editor sign data for sign ID: {0}", this.workingSignId));
		}
		this.releaseLockOnClose = false;
		XUiC_SignGalleryWindow.Open(this.xui.playerUI, this.targetEntity);
	}

	// Token: 0x06008366 RID: 33638 RVA: 0x00320FC1 File Offset: 0x0031F1C1
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDiscardChangesPromptResult(XUiC_ConfirmationPrompt.Result result)
	{
		if (result != XUiC_ConfirmationPrompt.Result.Cancelled && result == XUiC_ConfirmationPrompt.Result.Confirmed)
		{
			this.Quit(true);
		}
	}

	// Token: 0x06008367 RID: 33639 RVA: 0x00320FD1 File Offset: 0x0031F1D1
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtSignName_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		if (_changeFromCode)
		{
			return;
		}
		this.OnBeforeApplyingChange("Changed Sign Name", false);
		this.workingSignData.name = _text;
		this.MarkChanged();
	}

	// Token: 0x06008368 RID: 33640 RVA: 0x00320FF8 File Offset: 0x0031F1F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleCanvasScrolled(XUiController _sender, float _delta)
	{
		if (this.signLayerGrid.MultiSelectedLayerIndices.Count == 0 || this.signLayerGrid.HasPlaceholder)
		{
			return;
		}
		bool mouseButton = Input.GetMouseButton(1);
		float num = (mouseButton ? 0.995f : 0.95f);
		float num2 = ((_delta < 0f) ? num : (1f / num));
		if (InputUtils.ControlKeyPressed)
		{
			this.ScaleSelection(new Vector2(num2, num2));
		}
		else if (InputUtils.ShiftKeyPressed)
		{
			this.ScaleSelection(new Vector2(1f, num2));
		}
		else if (InputUtils.AltKeyPressed)
		{
			this.ScaleSelection(new Vector2(num2, 1f));
		}
		else
		{
			float num3 = (mouseButton ? 0.1f : 1f);
			this.RotateSelection(_delta * 50f * num3);
		}
		this.transformSettings.SetLayer(this.selectedLayer);
		this.MarkChanged();
	}

	// Token: 0x06008369 RID: 33641 RVA: 0x003210D0 File Offset: 0x0031F2D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleCanvasDragged(XUiController _sender, EDragType _dragType, Vector2 _mousePositionDelta)
	{
		switch (UICamera.currentKey)
		{
		case KeyCode.Mouse0:
			this.HandleLeftMouseDragged(_dragType, _mousePositionDelta);
			return;
		case KeyCode.Mouse1:
			break;
		case KeyCode.Mouse2:
			this.HandleMiddleMouseDragged(_dragType, _mousePositionDelta);
			break;
		default:
			return;
		}
	}

	// Token: 0x0600836A RID: 33642 RVA: 0x0032110D File Offset: 0x0031F30D
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetView()
	{
		this.viewOffset = Vector2.zero;
		this.viewScale = 1f;
	}

	// Token: 0x0600836B RID: 33643 RVA: 0x00321128 File Offset: 0x0031F328
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleMiddleMouseDragged(EDragType _dragType, Vector2 _mousePositionDelta)
	{
		if (_dragType == EDragType.DragStart)
		{
			this.<HandleMiddleMouseDragged>g__BeginDrag|122_1();
		}
		else
		{
			if (_dragType == EDragType.DragEnd)
			{
				this.<HandleMiddleMouseDragged>g__EndDrag|122_2();
				return;
			}
			if (this.dragStartViewScaleMode != InputUtils.ControlKeyPressed)
			{
				this.<HandleMiddleMouseDragged>g__EndDrag|122_2();
				this.<HandleMiddleMouseDragged>g__BeginDrag|122_1();
			}
		}
		Vector2 vector = this.xui.GetPixelRatioFactor() * (2f * _mousePositionDelta / (float)this.signMaterial.Size.y);
		this.middleMouseDrag.RawPosition = this.middleMouseDrag.RawPosition + vector;
		this.<HandleMiddleMouseDragged>g__UpdateViewOffset|122_0();
	}

	// Token: 0x0600836C RID: 33644 RVA: 0x003211B8 File Offset: 0x0031F3B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleLeftMouseDragged(EDragType _dragType, Vector2 _mousePositionDelta)
	{
		if (this.signLayerGrid.MultiSelectedLayerIndices.Count == 0 || this.signLayerGrid.HasPlaceholder)
		{
			this.leftMouseDrag.IsDragging = false;
			return;
		}
		Vector2 vector = this.xui.GetPixelRatioFactor() * (2f * _mousePositionDelta * this.viewScale / (float)this.signMaterial.Size.y);
		SignData.GroupSignLayer groupSignLayer;
		bool flag = this.TryGetAffectTargetGroup(out groupSignLayer);
		if (_dragType == EDragType.DragStart)
		{
			this.layerDragStartPositions.Clear();
			foreach (int num in this.signLayerGrid.MultiSelectedLayerIndices)
			{
				this.layerDragStartPositions.Add(this.workingSignData.layers[num].transform.position);
			}
			this.leftMouseDrag.StartPosition = this.selectedLayer.transform.position;
			this.leftMouseDrag.RawPosition = this.leftMouseDrag.StartPosition;
			this.leftMouseDrag.IsDragging = true;
			this.affectDragChildStartPositions.Clear();
			if (flag)
			{
				this.affectDragGroupStartPosition = groupSignLayer.transform.position;
				for (int i = 0; i < groupSignLayer.layers.Count; i++)
				{
					this.affectDragChildStartPositions.Add(groupSignLayer.layers[i].transform.position);
				}
			}
		}
		else if (_dragType == EDragType.DragEnd)
		{
			this.leftMouseDrag.IsDragging = false;
			return;
		}
		if (this.signLayerGrid.MultiSelectedLayerIndices.Count != this.layerDragStartPositions.Count)
		{
			Log.Error(string.Format("Data error in HandleCanvasDragged: Selected ({0}) and tracked ({1}) layer counts are not equal.", this.signLayerGrid.MultiSelectedLayerIndices.Count, this.layerDragStartPositions.Count));
			return;
		}
		this.leftMouseDrag.RawPosition = this.leftMouseDrag.RawPosition + vector;
		Vector2 vector2 = this.leftMouseDrag.RawPosition - this.leftMouseDrag.StartPosition;
		if (InputUtils.ShiftKeyPressed)
		{
			if (Mathf.Abs(vector2.x) > Mathf.Abs(vector2.y))
			{
				vector2.y = 0f;
			}
			else
			{
				vector2.x = 0f;
			}
		}
		this.OnBeforeApplyingChange(this.DecorateChangeDescription("Changed Transform Position"), false);
		if (flag && this.affectDragChildStartPositions.Count == groupSignLayer.layers.Count)
		{
			Vector2 vector3 = XUiC_SignEditorWindow.WorldDeltaToGroupLocalDelta(groupSignLayer, vector2);
			if (this.currentAffectMode == XUiC_SignEditorWindow.AffectMode.Children)
			{
				for (int j = 0; j < groupSignLayer.layers.Count; j++)
				{
					groupSignLayer.layers[j].transform.position = this.affectDragChildStartPositions[j] + vector3;
				}
			}
			else if (this.currentAffectMode == XUiC_SignEditorWindow.AffectMode.Pivot)
			{
				groupSignLayer.transform.position = this.affectDragGroupStartPosition + vector2;
				for (int k = 0; k < groupSignLayer.layers.Count; k++)
				{
					groupSignLayer.layers[k].transform.position = this.affectDragChildStartPositions[k] - vector3;
				}
			}
		}
		else
		{
			for (int l = 0; l < this.signLayerGrid.MultiSelectedLayerIndices.Count; l++)
			{
				int num2 = this.signLayerGrid.MultiSelectedLayerIndices[l];
				this.workingSignData.layers[num2].transform.position = this.layerDragStartPositions[l] + vector2;
			}
		}
		this.transformSettings.SetLayer(this.selectedLayer);
		this.MarkChanged();
	}

	// Token: 0x0600836D RID: 33645 RVA: 0x00321598 File Offset: 0x0031F798
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDragAndDropStarted(int dragStartIdx)
	{
		this.dragAndDropIcon.SetLayer(this.workingSignId, this.workingSignData.layers[this.signLayerGrid.draggedLayerIndices[0]]);
	}

	// Token: 0x0600836E RID: 33646 RVA: 0x003215CC File Offset: 0x0031F7CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDragAndDropReleased(int insertIndex)
	{
		if (insertIndex < 0)
		{
			this.dragAndDropIcon.SetLayer(this.workingSignId, null);
			return;
		}
		this.OnBeforeApplyingChange("Drag and dropped layer", false);
		this.originalLayerIndices.Clear();
		this.originalLayerIndices.AddRange(this.signLayerGrid.draggedLayerIndices);
		int count = this.originalLayerIndices.Count;
		this.originalLayers.Clear();
		for (int i = 0; i < count; i++)
		{
			this.originalLayers.Add(this.workingSignData.layers[this.originalLayerIndices[i]].Clone());
		}
		for (int j = count - 1; j >= 0; j--)
		{
			this.workingSignData.layers.RemoveAt(this.originalLayerIndices[j]);
		}
		int num = insertIndex;
		for (int k = 0; k < count; k++)
		{
			if (this.originalLayerIndices[k] < insertIndex)
			{
				num--;
			}
		}
		for (int l = 0; l < count; l++)
		{
			this.workingSignData.layers.Insert(num + l, this.originalLayers[l]);
		}
		this.signLayerGrid.MultiSelectedLayerIndices.Clear();
		for (int m = 0; m < count; m++)
		{
			this.signLayerGrid.MultiSelectedLayerIndices.Add(num + m);
		}
		this.MarkChanged();
		this.signLayerGrid.UpdateLayers(this.workingSignId, num, false, true);
		this.dragAndDropIcon.SetLayer(this.workingSignId, null);
	}

	// Token: 0x0600836F RID: 33647 RVA: 0x00321750 File Offset: 0x0031F950
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleLayerSelected(int selectedLayerIndex)
	{
		int num = this.signLayerGrid.GridIndexToLayerIndex(selectedLayerIndex);
		if (num < 0 || num >= this.workingSignData.layers.Count)
		{
			this.SetLayerSelected(null);
			return;
		}
		SignData.SignLayer signLayer = this.workingSignData.layers[num];
		this.SetLayerSelected(signLayer);
		if (signLayer == this.hoveredLayer)
		{
			this.HandleLayerHovered(selectedLayerIndex);
		}
	}

	// Token: 0x06008370 RID: 33648 RVA: 0x003217B4 File Offset: 0x0031F9B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleLayerHovered(int hoveredDisplayIndex)
	{
		int num = this.signLayerGrid.GridIndexToLayerIndex(hoveredDisplayIndex);
		this.hoveredLayer = ((num >= 0 && num < this.workingSignData.layers.Count) ? this.workingSignData.layers[num] : null);
		if (this.hoveredLayer != null)
		{
			this.hoverOverlayLayer = this.hoveredLayer;
		}
		if (num < 0 || num >= this.workingSignData.layers.Count)
		{
			this.sizeBar.SetHoveredRegion(new XUiC_SizeBar.BarRegionFloat(0f, 0f));
			return;
		}
		SignData.SignLayer signLayer = this.workingSignData.layers[num];
		LayerComplexityInfo layerComplexityInfo;
		if (!this.signComplexityInfo.TryGetLayerComplexityInfo(signLayer, out layerComplexityInfo))
		{
			this.sizeBar.SetHoveredRegion(new XUiC_SizeBar.BarRegionFloat(0f, 0f));
			return;
		}
		if (this.signLayerGrid.MultiSelectedLayerIndices.Contains(hoveredDisplayIndex))
		{
			this.sizeBar.SetHoveredRegion(new XUiC_SizeBar.BarRegionFloat(this.complexityHoverOffset - layerComplexityInfo.Complexity, layerComplexityInfo.Complexity));
			return;
		}
		this.sizeBar.SetHoveredRegion(new XUiC_SizeBar.BarRegionFloat(this.complexityHoverOffset, layerComplexityInfo.Complexity));
	}

	// Token: 0x06008371 RID: 33649 RVA: 0x003218D5 File Offset: 0x0031FAD5
	[PublicizedFrom(EAccessModifier.Private)]
	public void BeginAddLayerFlow(int index)
	{
		this.addLayerIndex = index;
		this.signLayerGrid.UpdateLayers(this.workingSignId, this.addLayerIndex, true, true);
	}

	// Token: 0x06008372 RID: 33650 RVA: 0x003218F8 File Offset: 0x0031FAF8
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleLayerTypeSelected(SignData.SignLayer layer)
	{
		if (layer == null)
		{
			return;
		}
		int num = this.addLayerIndex;
		this.OnBeforeApplyingChange("Added Layer", true);
		this.workingSignData.SetLayerDefaultName(layer);
		int num2 = ((this.workingSignData.layers.Count == 0) ? 0 : ((num < 0) ? (this.workingSignData.layers.Count - 1) : num));
		this.workingSignData.layers.Insert(num2, layer);
		this.signLayerGrid.MultiSelectedLayerIndices.Clear();
		this.signLayerGrid.MultiSelectedLayerIndices.Add(num2);
		this.MarkChanged();
		this.signLayerGrid.UpdateLayers(this.workingSignId, num2, false, true);
		this.addLayerIndex = -1;
	}

	// Token: 0x06008373 RID: 33651 RVA: 0x003219AC File Offset: 0x0031FBAC
	[PublicizedFrom(EAccessModifier.Private)]
	public void DeleteSelectedLayers(string changeDescription = "Deleted Layer(s)")
	{
		if (this.signLayerGrid.MultiSelectedLayerIndices.Count == 0 || this.signLayerGrid.HasPlaceholder)
		{
			return;
		}
		this.OnBeforeApplyingChange(changeDescription, true);
		this.signLayerGrid.MultiSelectedLayerIndices.Sort();
		int num = this.signLayerGrid.MultiSelectedLayerIndices[0];
		for (int i = this.signLayerGrid.MultiSelectedLayerIndices.Count - 1; i >= 0; i--)
		{
			this.workingSignData.layers.RemoveAt(this.signLayerGrid.MultiSelectedLayerIndices[i]);
		}
		this.signLayerGrid.MultiSelectedLayerIndices.Clear();
		num = Mathf.Min(num, this.workingSignData.layers.Count - 1);
		if (num >= 0)
		{
			this.signLayerGrid.MultiSelectedLayerIndices.Add(num);
			this.signLayerGrid.UpdateLayers(this.workingSignId, num, false, true);
		}
		else
		{
			this.BeginAddLayerFlow(0);
		}
		this.MarkChanged();
	}

	// Token: 0x06008374 RID: 33652 RVA: 0x00321AA4 File Offset: 0x0031FCA4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SelectAllLayers()
	{
		if (this.workingSignData.layers.Count == 0)
		{
			return;
		}
		this.signLayerGrid.MultiSelectedLayerIndices.Clear();
		for (int i = 0; i < this.workingSignData.layers.Count; i++)
		{
			this.signLayerGrid.MultiSelectedLayerIndices.Add(i);
		}
		this.signLayerGrid.UpdateLayers(this.workingSignId, 0, false, true);
	}

	// Token: 0x06008375 RID: 33653 RVA: 0x00321B14 File Offset: 0x0031FD14
	[PublicizedFrom(EAccessModifier.Private)]
	public void DuplicateSelectedLayers()
	{
		if (this.signLayerGrid.MultiSelectedLayerIndices.Count == 0 || this.signLayerGrid.HasPlaceholder)
		{
			return;
		}
		this.OnBeforeApplyingChange("Duplicated Layer(s)", true);
		this.signLayerGrid.MultiSelectedLayerIndices.Sort();
		int count = this.signLayerGrid.MultiSelectedLayerIndices.Count;
		int num = this.signLayerGrid.MultiSelectedLayerIndices[count - 1] + 1;
		for (int i = count - 1; i >= 0; i--)
		{
			this.workingSignData.layers.Insert(num, this.workingSignData.layers[this.signLayerGrid.MultiSelectedLayerIndices[i]].Clone());
		}
		this.signLayerGrid.MultiSelectedLayerIndices.Clear();
		for (int j = 0; j < count; j++)
		{
			this.signLayerGrid.MultiSelectedLayerIndices.Add(num + j);
		}
		this.MarkChanged();
		this.signLayerGrid.UpdateLayers(this.workingSignId, num + count - 1, false, true);
	}

	// Token: 0x06008376 RID: 33654 RVA: 0x00321C18 File Offset: 0x0031FE18
	[PublicizedFrom(EAccessModifier.Private)]
	public void CopySelectedLayersToClipboard()
	{
		if (this.signLayerGrid.MultiSelectedLayerIndices.Count == 0 || this.signLayerGrid.HasPlaceholder)
		{
			return;
		}
		XUiC_SignEditorWindow.layerClipboard.Clear();
		this.signLayerGrid.MultiSelectedLayerIndices.Sort();
		foreach (int num in this.signLayerGrid.MultiSelectedLayerIndices)
		{
			XUiC_SignEditorWindow.layerClipboard.Add(this.workingSignData.layers[num].Clone());
		}
	}

	// Token: 0x06008377 RID: 33655 RVA: 0x00321CC4 File Offset: 0x0031FEC4
	[PublicizedFrom(EAccessModifier.Private)]
	public void PasteLayersFromClipboard()
	{
		if (XUiC_SignEditorWindow.layerClipboard.Count == 0)
		{
			return;
		}
		this.OnBeforeApplyingChange("Pasted Layer(s)", true);
		int num = Mathf.Min(this.signLayerGrid.SelectedLayerIndex, this.workingSignData.layers.Count - 1);
		this.signLayerGrid.MultiSelectedLayerIndices.Clear();
		foreach (SignData.SignLayer signLayer in XUiC_SignEditorWindow.layerClipboard)
		{
			num++;
			this.workingSignData.layers.Insert(num, signLayer.Clone());
			this.signLayerGrid.MultiSelectedLayerIndices.Add(num);
		}
		this.MarkChanged();
		this.signLayerGrid.UpdateLayers(this.workingSignId, num, false, true);
	}

	// Token: 0x06008378 RID: 33656 RVA: 0x00321DA4 File Offset: 0x0031FFA4
	[PublicizedFrom(EAccessModifier.Private)]
	public void UngroupSelectedLayers()
	{
		if (this.signLayerGrid.MultiSelectedLayerIndices.Count < 1)
		{
			return;
		}
		this.OnBeforeApplyingChange("Ungrouped Layer(s)", true);
		this.signLayerGrid.MultiSelectedLayerIndices.Sort();
		int num = this.signLayerGrid.MultiSelectedLayerIndices[0];
		this.indicesToSelect.Clear();
		for (int i = this.signLayerGrid.MultiSelectedLayerIndices.Count - 1; i >= 0; i--)
		{
			int num2 = this.signLayerGrid.MultiSelectedLayerIndices[i];
			int num3;
			if (this.workingSignData.TryUnpackTopLevelGroup(num2, false, out num3))
			{
				for (int j = 0; j < this.indicesToSelect.Count; j++)
				{
					List<int> list = this.indicesToSelect;
					int num4 = j;
					list[num4] += num3 - 1;
				}
				for (int k = 0; k < num3; k++)
				{
					this.indicesToSelect.Add(num2 + k);
				}
			}
		}
		this.signLayerGrid.MultiSelectedLayerIndices.Clear();
		this.signLayerGrid.MultiSelectedLayerIndices.AddRange(this.indicesToSelect);
		this.MarkChanged();
		this.signLayerGrid.UpdateLayers(this.workingSignId, num, false, true);
	}

	// Token: 0x06008379 RID: 33657 RVA: 0x00321EE0 File Offset: 0x003200E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void GroupSelectedLayers()
	{
		if (this.signLayerGrid.MultiSelectedLayerIndices.Count < 1)
		{
			return;
		}
		this.OnBeforeApplyingChange("Grouped Layer(s)", true);
		this.signLayerGrid.MultiSelectedLayerIndices.Sort();
		int num = this.signLayerGrid.MultiSelectedLayerIndices[0];
		Vector2 vector = this.workingSignData.layers[num].transform.position;
		Vector2 vector2 = this.workingSignData.layers[num].transform.position;
		for (int i = 1; i < this.signLayerGrid.MultiSelectedLayerIndices.Count; i++)
		{
			int num2 = this.signLayerGrid.MultiSelectedLayerIndices[i];
			Vector2 position = this.workingSignData.layers[num2].transform.position;
			vector = Vector2.Min(vector, position);
			vector2 = Vector2.Max(vector2, position);
		}
		Vector2 vector3 = 0.5f * (vector + vector2);
		SignData.GroupSignLayer groupSignLayer = new SignData.GroupSignLayer("Group", vector3, 0f, Vector2.one, new SignData.SignRenderSettings(Color.white, SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), new List<SignData.SignLayer>(this.signLayerGrid.MultiSelectedLayerIndices.Count), SignData.GroupSignLayer.OffsetTarget.All, 0f, 0f, SignData.GroupSignLayer.ColorMode.Multiply);
		this.workingSignData.SetLayerDefaultName(groupSignLayer);
		for (int j = this.signLayerGrid.MultiSelectedLayerIndices.Count - 1; j >= 0; j--)
		{
			int num3 = this.signLayerGrid.MultiSelectedLayerIndices[j];
			SignData.SignLayer signLayer = this.workingSignData.layers[num3];
			this.workingSignData.layers.RemoveAt(num3);
			groupSignLayer.layers.Insert(0, signLayer);
			SignData.SignLayer signLayer2 = signLayer;
			signLayer2.transform.position = signLayer2.transform.position - vector3;
		}
		this.workingSignData.layers.Insert(num, groupSignLayer);
		this.signLayerGrid.MultiSelectedLayerIndices.Clear();
		this.signLayerGrid.MultiSelectedLayerIndices.Add(num);
		this.MarkChanged();
		this.signLayerGrid.UpdateLayers(this.workingSignId, num, false, true);
	}

	// Token: 0x0600837A RID: 33658 RVA: 0x00322108 File Offset: 0x00320308
	[PublicizedFrom(EAccessModifier.Private)]
	public void TranslateSelection(Vector2 delta)
	{
		this.OnBeforeApplyingChange(this.DecorateChangeDescription("Changed Transform Position"), false);
		SignData.GroupSignLayer groupSignLayer;
		if (!this.TryGetAffectTargetGroup(out groupSignLayer))
		{
			foreach (int num in this.signLayerGrid.MultiSelectedLayerIndices)
			{
				SignData.SignLayer signLayer = this.workingSignData.layers[num];
				signLayer.transform.position = signLayer.transform.position + delta;
			}
			return;
		}
		Vector2 vector = XUiC_SignEditorWindow.WorldDeltaToGroupLocalDelta(groupSignLayer, delta);
		if (this.currentAffectMode == XUiC_SignEditorWindow.AffectMode.Children)
		{
			for (int i = 0; i < groupSignLayer.layers.Count; i++)
			{
				SignData.SignLayer signLayer2 = groupSignLayer.layers[i];
				signLayer2.transform.position = signLayer2.transform.position + vector;
			}
			return;
		}
		SignData.GroupSignLayer groupSignLayer2 = groupSignLayer;
		groupSignLayer2.transform.position = groupSignLayer2.transform.position + delta;
		for (int j = 0; j < groupSignLayer.layers.Count; j++)
		{
			SignData.SignLayer signLayer3 = groupSignLayer.layers[j];
			signLayer3.transform.position = signLayer3.transform.position - vector;
		}
	}

	// Token: 0x0600837B RID: 33659 RVA: 0x00322250 File Offset: 0x00320450
	[PublicizedFrom(EAccessModifier.Private)]
	public void RotateSelection(float deltaDegrees)
	{
		this.OnBeforeApplyingChange(this.DecorateChangeDescription("Changed Transform Rotation"), false);
		SignData.GroupSignLayer groupSignLayer;
		if (!this.TryGetAffectTargetGroup(out groupSignLayer))
		{
			foreach (int num in this.signLayerGrid.MultiSelectedLayerIndices)
			{
				SignData.SignLayer signLayer = this.workingSignData.layers[num];
				this.OnBeforeApplyingChange("Changed Transform Rotation", false);
				signLayer.transform.rotation = signLayer.transform.rotation + deltaDegrees;
			}
			return;
		}
		if (this.currentAffectMode == XUiC_SignEditorWindow.AffectMode.Children)
		{
			XUiC_SignEditorWindow.RotateGroupChildrenAroundOrigin(groupSignLayer, deltaDegrees);
			return;
		}
		SignData.GroupSignLayer groupSignLayer2 = groupSignLayer;
		groupSignLayer2.transform.rotation = groupSignLayer2.transform.rotation + deltaDegrees;
		XUiC_SignEditorWindow.RotateGroupChildrenAroundOrigin(groupSignLayer, -deltaDegrees);
	}

	// Token: 0x0600837C RID: 33660 RVA: 0x00322314 File Offset: 0x00320514
	[PublicizedFrom(EAccessModifier.Private)]
	public void ScaleSelection(Vector2 scaleFactor)
	{
		this.OnBeforeApplyingChange(this.DecorateChangeDescription("Changed Transform Scale"), false);
		SignData.GroupSignLayer groupSignLayer;
		if (!this.TryGetAffectTargetGroup(out groupSignLayer))
		{
			foreach (int num in this.signLayerGrid.MultiSelectedLayerIndices)
			{
				SignData.SignLayer signLayer = this.workingSignData.layers[num];
				signLayer.transform.scale = signLayer.transform.scale * scaleFactor;
			}
			return;
		}
		if (this.currentAffectMode == XUiC_SignEditorWindow.AffectMode.Children)
		{
			XUiC_SignEditorWindow.ScaleGroupChildrenAroundOrigin(groupSignLayer, scaleFactor);
			return;
		}
		groupSignLayer.transform.scale = new Vector2(groupSignLayer.transform.scale.x * scaleFactor.x, groupSignLayer.transform.scale.y * scaleFactor.y);
		Vector2 vector = new Vector2(Mathf.Approximately(scaleFactor.x, 0f) ? 1f : (1f / scaleFactor.x), Mathf.Approximately(scaleFactor.y, 0f) ? 1f : (1f / scaleFactor.y));
		XUiC_SignEditorWindow.ScaleGroupChildrenAroundOrigin(groupSignLayer, vector);
	}

	// Token: 0x0600837D RID: 33661 RVA: 0x00322458 File Offset: 0x00320658
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.UpdateInputs();
		if (Time.time - this.highlightStartTime >= 1f && this.pulseOverlayMaterial.Texture != null)
		{
			this.pulseOverlayMaterial.Texture = null;
		}
		float num = ((this.hoveredLayer != null) ? 1f : 0f);
		float num2 = 4f;
		this.hoverOverlayOpacity = Mathf.MoveTowards(this.hoverOverlayOpacity, num, num2 * _dt);
		if (this.hoverOverlayOpacity > 0f && this.hoverOverlayMaterial.Texture == null)
		{
			this.hoverOverlayMaterial.Texture = Texture2D.whiteTexture;
		}
		else if (this.hoverOverlayOpacity <= 0f && this.hoverOverlayMaterial.Texture != null)
		{
			this.hoverOverlayMaterial.Texture = null;
			this.hoverOverlayLayer = null;
		}
		if (this.viewComponent.IsVisible && this.IsDirty)
		{
			this.Refresh();
		}
	}

	// Token: 0x0600837E RID: 33662 RVA: 0x00322558 File Offset: 0x00320758
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateInputs()
	{
		if (this.xui.playerUI.windowManager.IsInputActive())
		{
			return;
		}
		if (this.xui.playerUI.CursorController.GetMouseButtonDown(UICamera.MouseButton.LeftButton) | this.xui.playerUI.CursorController.GetMouseButtonUp(UICamera.MouseButton.LeftButton))
		{
			this.historyStateManager.ForceDirty();
		}
		if (this.xui.playerUI.playerInput != null && this.xui.playerUI.playerInput.PermanentActions.Cancel.WasPressed)
		{
			if (!this.confirmationPrompt.IsVisible)
			{
				this.Quit(false);
				return;
			}
			this.confirmationPrompt.Cancel();
		}
		if (this.confirmationPrompt.IsVisible)
		{
			return;
		}
		if (this.leftMouseDrag.IsDragging && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)))
		{
			this.HandleLeftMouseDragged(EDragType.Dragging, Vector2.zero);
		}
		if (InputUtils.ControlKeyPressed)
		{
			DateTime now = DateTime.Now;
			if (Input.GetKeyDown(KeyCode.Z))
			{
				this.heldInputRetriggerTime = now + XUiC_SignEditorWindow.firstRetriggerDelay;
				if (this.historyStateManager.TryUndo())
				{
					this.MarkChanged();
					return;
				}
			}
			else if (Input.GetKey(KeyCode.Z) && now > this.heldInputRetriggerTime)
			{
				this.heldInputRetriggerTime = now + XUiC_SignEditorWindow.subsequentRetriggerDelay;
				if (this.historyStateManager.TryUndo())
				{
					this.MarkChanged();
					return;
				}
			}
			else if (Input.GetKeyDown(KeyCode.Y))
			{
				this.heldInputRetriggerTime = now + XUiC_SignEditorWindow.firstRetriggerDelay;
				if (this.historyStateManager.TryRedo())
				{
					this.MarkChanged();
					return;
				}
			}
			else if (Input.GetKey(KeyCode.Y) && now > this.heldInputRetriggerTime)
			{
				this.heldInputRetriggerTime = now + XUiC_SignEditorWindow.subsequentRetriggerDelay;
				if (this.historyStateManager.TryRedo())
				{
					this.MarkChanged();
					return;
				}
			}
			else if (Input.GetKeyDown(KeyCode.S))
			{
				if (InputUtils.ShiftKeyPressed)
				{
					this.SaveCopy();
					return;
				}
				this.Save();
				return;
			}
			else
			{
				if (Input.GetKeyDown(KeyCode.A))
				{
					this.SelectAllLayers();
					return;
				}
				if (Input.GetKeyDown(KeyCode.D))
				{
					this.DuplicateSelectedLayers();
					return;
				}
				if (Input.GetKeyDown(KeyCode.X))
				{
					this.CopySelectedLayersToClipboard();
					this.DeleteSelectedLayers("Cut Layer(s)");
					return;
				}
				if (Input.GetKeyDown(KeyCode.C))
				{
					this.CopySelectedLayersToClipboard();
					return;
				}
				if (Input.GetKeyDown(KeyCode.V))
				{
					this.PasteLayersFromClipboard();
					return;
				}
				if (Input.GetKeyDown(KeyCode.G))
				{
					if (InputUtils.ShiftKeyPressed)
					{
						this.UngroupSelectedLayers();
						return;
					}
					this.GroupSelectedLayers();
					return;
				}
				else if (Input.GetKeyDown(KeyCode.N) && this.signLayerGrid.MultiSelectedLayerIndices.Count > 0)
				{
					if (InputUtils.ShiftKeyPressed)
					{
						this.addLayerIndex = this.signLayerGrid.MultiSelectedLayerIndices[0];
					}
					else
					{
						this.addLayerIndex = this.signLayerGrid.MultiSelectedLayerIndices[0] + 1;
					}
					this.BeginAddLayerFlow(this.addLayerIndex);
					return;
				}
			}
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				this.ResetView();
			}
			else if (Input.GetKeyDown(KeyCode.Delete))
			{
				this.DeleteSelectedLayers("Deleted Layer(s)");
			}
			Vector2 vector = Vector2.zero;
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				vector.y += 0.01f;
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				vector.y -= 0.01f;
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				vector.x += 0.01f;
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				vector.x -= 0.01f;
			}
			if (vector != Vector2.zero && !this.signLayerGrid.HasPlaceholder)
			{
				bool altKeyPressed = InputUtils.AltKeyPressed;
				if (InputUtils.ShiftKeyPressed)
				{
					vector *= (altKeyPressed ? 5f : 10f);
				}
				else if (altKeyPressed)
				{
					vector *= 0.1f;
				}
				this.TranslateSelection(vector);
				this.transformSettings.SetLayer(this.selectedLayer);
				this.MarkChanged();
			}
			if (Input.GetMouseButtonUp(0))
			{
				this.signLayerGrid.ReleaseDragAndDrop();
			}
		}
	}

	// Token: 0x0600837F RID: 33663 RVA: 0x0032296C File Offset: 0x00320B6C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Refresh()
	{
		SignDataManager.Instance.TryGetSignComplexityInfo(this.workingSignId, out this.signComplexityInfo);
		this.RefreshComplexityInfo();
		bool savingAllowed = this.GetSavingAllowed();
		this.btnSave.Enabled = this.hasUnsavedChanges && savingAllowed;
		this.btnSaveCopy.Enabled = savingAllowed;
		base.RefreshBindings();
		this.IsDirty = false;
		Action onRefreshed = this.OnRefreshed;
		if (onRefreshed == null)
		{
			return;
		}
		onRefreshed();
	}

	// Token: 0x06008380 RID: 33664 RVA: 0x003229DC File Offset: 0x00320BDC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool GetSavingAllowed()
	{
		return this.signComplexityInfo.IsValid && this.signComplexityInfo.TotalComplexity <= 600f && this.signComplexityInfo.StackInfo.MaxCompStackIndex <= 7 && this.signComplexityInfo.StackInfo.MaxUVStackIndex <= 7;
	}

	// Token: 0x06008381 RID: 33665 RVA: 0x00322A38 File Offset: 0x00320C38
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 2407419735U)
		{
			if (num <= 1429645657U)
			{
				if (num <= 279784058U)
				{
					if (num != 25739702U)
					{
						if (num != 75534831U)
						{
							if (num == 279784058U)
							{
								if (bindingName == "selectedlayername")
								{
									value = this.selectedLayerName;
									return true;
								}
							}
						}
						else if (bindingName == "shownewlayerpanel")
						{
							value = this.showNewLayerPanel.ToString();
							return true;
						}
					}
					else if (bindingName == "showstretchwarpsettings")
					{
						XUiC_CategoryList xuiC_CategoryList = this.categoryList;
						bool? flag;
						if (xuiC_CategoryList == null)
						{
							flag = null;
						}
						else
						{
							XUiC_CategoryEntry currentCategory = xuiC_CategoryList.CurrentCategory;
							if (currentCategory == null)
							{
								flag = null;
							}
							else
							{
								string categoryName = currentCategory.CategoryName;
								flag = ((categoryName != null) ? new bool?(categoryName.ContainsCaseInsensitive(typeof(SignData.StretchWarp).Name)) : null);
							}
						}
						bool? flag2 = flag;
						value = flag2.GetValueOrDefault().ToString();
						return true;
					}
				}
				else if (num <= 1369692037U)
				{
					if (num != 738251961U)
					{
						if (num == 1369692037U)
						{
							if (bindingName == "showtwirlwarpsettings")
							{
								XUiC_CategoryList xuiC_CategoryList2 = this.categoryList;
								bool? flag3;
								if (xuiC_CategoryList2 == null)
								{
									flag3 = null;
								}
								else
								{
									XUiC_CategoryEntry currentCategory2 = xuiC_CategoryList2.CurrentCategory;
									if (currentCategory2 == null)
									{
										flag3 = null;
									}
									else
									{
										string categoryName2 = currentCategory2.CategoryName;
										flag3 = ((categoryName2 != null) ? new bool?(categoryName2.ContainsCaseInsensitive(typeof(SignData.TwirlWarp).Name)) : null);
									}
								}
								bool? flag2 = flag3;
								value = flag2.GetValueOrDefault().ToString();
								return true;
							}
						}
					}
					else if (bindingName == "showperspectivewarpsettings")
					{
						XUiC_CategoryList xuiC_CategoryList3 = this.categoryList;
						bool? flag4;
						if (xuiC_CategoryList3 == null)
						{
							flag4 = null;
						}
						else
						{
							XUiC_CategoryEntry currentCategory3 = xuiC_CategoryList3.CurrentCategory;
							if (currentCategory3 == null)
							{
								flag4 = null;
							}
							else
							{
								string categoryName3 = currentCategory3.CategoryName;
								flag4 = ((categoryName3 != null) ? new bool?(categoryName3.ContainsCaseInsensitive(typeof(SignData.PerspectiveWarp).Name)) : null);
							}
						}
						bool? flag2 = flag4;
						value = flag2.GetValueOrDefault().ToString();
						return true;
					}
				}
				else if (num != 1394595989U)
				{
					if (num == 1429645657U)
					{
						if (bindingName == "showskewwarpsettings")
						{
							XUiC_CategoryList xuiC_CategoryList4 = this.categoryList;
							bool? flag5;
							if (xuiC_CategoryList4 == null)
							{
								flag5 = null;
							}
							else
							{
								XUiC_CategoryEntry currentCategory4 = xuiC_CategoryList4.CurrentCategory;
								if (currentCategory4 == null)
								{
									flag5 = null;
								}
								else
								{
									string categoryName4 = currentCategory4.CategoryName;
									flag5 = ((categoryName4 != null) ? new bool?(categoryName4.ContainsCaseInsensitive(typeof(SignData.SkewWarp).Name)) : null);
								}
							}
							bool? flag2 = flag5;
							value = flag2.GetValueOrDefault().ToString();
							return true;
						}
					}
				}
				else if (bindingName == "showsavenotice")
				{
					bool flag6 = false;
					if (this.signComplexityInfo.IsValid)
					{
						flag6 = this.signComplexityInfo.TotalComplexity > 600f || this.signComplexityInfo.StackInfo.MaxCompStackIndex > 7 || this.signComplexityInfo.StackInfo.MaxUVStackIndex > 7;
					}
					value = flag6.ToString();
					return true;
				}
			}
			else if (num <= 1574880052U)
			{
				if (num != 1475391586U)
				{
					if (num != 1543894150U)
					{
						if (num == 1574880052U)
						{
							if (bindingName == "showaddwarpsettings")
							{
								XUiC_CategoryList xuiC_CategoryList5 = this.categoryList;
								bool? flag7;
								if (xuiC_CategoryList5 == null)
								{
									flag7 = null;
								}
								else
								{
									XUiC_CategoryEntry currentCategory5 = xuiC_CategoryList5.CurrentCategory;
									if (currentCategory5 == null)
									{
										flag7 = null;
									}
									else
									{
										string categoryName5 = currentCategory5.CategoryName;
										flag7 = ((categoryName5 != null) ? new bool?(categoryName5.EqualsCaseInsensitive("AddWarp")) : null);
									}
								}
								bool? flag2 = flag7;
								value = flag2.GetValueOrDefault().ToString();
								return true;
							}
						}
					}
					else if (bindingName == "showtextsettings")
					{
						XUiC_CategoryList xuiC_CategoryList6 = this.categoryList;
						bool? flag8;
						if (xuiC_CategoryList6 == null)
						{
							flag8 = null;
						}
						else
						{
							XUiC_CategoryEntry currentCategory6 = xuiC_CategoryList6.CurrentCategory;
							if (currentCategory6 == null)
							{
								flag8 = null;
							}
							else
							{
								string categoryName6 = currentCategory6.CategoryName;
								flag8 = ((categoryName6 != null) ? new bool?(categoryName6.EqualsCaseInsensitive("Text")) : null);
							}
						}
						bool? flag2 = flag8;
						value = flag2.GetValueOrDefault().ToString();
						return true;
					}
				}
				else if (bindingName == "showgroupsettings")
				{
					XUiC_CategoryList xuiC_CategoryList7 = this.categoryList;
					bool? flag9;
					if (xuiC_CategoryList7 == null)
					{
						flag9 = null;
					}
					else
					{
						XUiC_CategoryEntry currentCategory7 = xuiC_CategoryList7.CurrentCategory;
						if (currentCategory7 == null)
						{
							flag9 = null;
						}
						else
						{
							string categoryName7 = currentCategory7.CategoryName;
							flag9 = ((categoryName7 != null) ? new bool?(categoryName7.EqualsCaseInsensitive("Group")) : null);
						}
					}
					bool? flag2 = flag9;
					value = flag2.GetValueOrDefault().ToString();
					return true;
				}
			}
			else if (num <= 2312798212U)
			{
				if (num != 2231848975U)
				{
					if (num == 2312798212U)
					{
						if (bindingName == "showkaleidowarpsettings")
						{
							XUiC_CategoryList xuiC_CategoryList8 = this.categoryList;
							bool? flag10;
							if (xuiC_CategoryList8 == null)
							{
								flag10 = null;
							}
							else
							{
								XUiC_CategoryEntry currentCategory8 = xuiC_CategoryList8.CurrentCategory;
								if (currentCategory8 == null)
								{
									flag10 = null;
								}
								else
								{
									string categoryName8 = currentCategory8.CategoryName;
									flag10 = ((categoryName8 != null) ? new bool?(categoryName8.ContainsCaseInsensitive(typeof(SignData.KaleidoWarp).Name)) : null);
								}
							}
							bool? flag2 = flag10;
							value = flag2.GetValueOrDefault().ToString();
							return true;
						}
					}
				}
				else if (bindingName == "showarcwarpsettings")
				{
					XUiC_CategoryList xuiC_CategoryList9 = this.categoryList;
					bool? flag11;
					if (xuiC_CategoryList9 == null)
					{
						flag11 = null;
					}
					else
					{
						XUiC_CategoryEntry currentCategory9 = xuiC_CategoryList9.CurrentCategory;
						if (currentCategory9 == null)
						{
							flag11 = null;
						}
						else
						{
							string categoryName9 = currentCategory9.CategoryName;
							flag11 = ((categoryName9 != null) ? new bool?(categoryName9.ContainsCaseInsensitive(typeof(SignData.ArcWarp).Name)) : null);
						}
					}
					bool? flag2 = flag11;
					value = flag2.GetValueOrDefault().ToString();
					return true;
				}
			}
			else if (num != 2347086190U)
			{
				if (num == 2407419735U)
				{
					if (bindingName == "showgridwarpsettings")
					{
						XUiC_CategoryList xuiC_CategoryList10 = this.categoryList;
						bool? flag12;
						if (xuiC_CategoryList10 == null)
						{
							flag12 = null;
						}
						else
						{
							XUiC_CategoryEntry currentCategory10 = xuiC_CategoryList10.CurrentCategory;
							if (currentCategory10 == null)
							{
								flag12 = null;
							}
							else
							{
								string categoryName10 = currentCategory10.CategoryName;
								flag12 = ((categoryName10 != null) ? new bool?(categoryName10.ContainsCaseInsensitive(typeof(SignData.GridWarp).Name)) : null);
							}
						}
						bool? flag2 = flag12;
						value = flag2.GetValueOrDefault().ToString();
						return true;
					}
				}
			}
			else if (bindingName == "warp_icon_arc")
			{
				value = this.warpIconArc;
				return true;
			}
		}
		else if (num <= 3368053736U)
		{
			if (num <= 2931353358U)
			{
				if (num != 2433615239U)
				{
					if (num != 2797060081U)
					{
						if (num == 2931353358U)
						{
							if (bindingName == "showcolorsettings")
							{
								XUiC_CategoryList xuiC_CategoryList11 = this.categoryList;
								bool? flag13;
								if (xuiC_CategoryList11 == null)
								{
									flag13 = null;
								}
								else
								{
									XUiC_CategoryEntry currentCategory11 = xuiC_CategoryList11.CurrentCategory;
									if (currentCategory11 == null)
									{
										flag13 = null;
									}
									else
									{
										string categoryName11 = currentCategory11.CategoryName;
										flag13 = ((categoryName11 != null) ? new bool?(categoryName11.EqualsCaseInsensitive("Color")) : null);
									}
								}
								bool? flag2 = flag13;
								value = flag2.GetValueOrDefault().ToString();
								return true;
							}
						}
					}
					else if (bindingName == "warp_icon_kaleido")
					{
						value = this.warpIconKaleido;
						return true;
					}
				}
				else if (bindingName == "showpolygonsettings")
				{
					XUiC_CategoryList xuiC_CategoryList12 = this.categoryList;
					bool? flag14;
					if (xuiC_CategoryList12 == null)
					{
						flag14 = null;
					}
					else
					{
						XUiC_CategoryEntry currentCategory12 = xuiC_CategoryList12.CurrentCategory;
						if (currentCategory12 == null)
						{
							flag14 = null;
						}
						else
						{
							string categoryName12 = currentCategory12.CategoryName;
							flag14 = ((categoryName12 != null) ? new bool?(categoryName12.EqualsCaseInsensitive("Polygon")) : null);
						}
					}
					bool? flag2 = flag14;
					value = flag2.GetValueOrDefault().ToString();
					return true;
				}
			}
			else if (num <= 3111165525U)
			{
				if (num != 3049165781U)
				{
					if (num == 3111165525U)
					{
						if (bindingName == "showtransformsettings")
						{
							XUiC_CategoryList xuiC_CategoryList13 = this.categoryList;
							bool? flag15;
							if (xuiC_CategoryList13 == null)
							{
								flag15 = null;
							}
							else
							{
								XUiC_CategoryEntry currentCategory13 = xuiC_CategoryList13.CurrentCategory;
								if (currentCategory13 == null)
								{
									flag15 = null;
								}
								else
								{
									string categoryName13 = currentCategory13.CategoryName;
									flag15 = ((categoryName13 != null) ? new bool?(categoryName13.EqualsCaseInsensitive("Transform")) : null);
								}
							}
							bool? flag2 = flag15;
							value = flag2.GetValueOrDefault().ToString();
							return true;
						}
					}
				}
				else if (bindingName == "warp_icon_bulge")
				{
					value = this.warpIconBulge;
					return true;
				}
			}
			else if (num != 3167422962U)
			{
				if (num == 3368053736U)
				{
					if (bindingName == "warp_icon_skew")
					{
						value = this.warpIconSkew;
						return true;
					}
				}
			}
			else if (bindingName == "warp_icon_grid")
			{
				value = this.warpIconGrid;
				return true;
			}
		}
		else if (num <= 3634381192U)
		{
			if (num != 3434567931U)
			{
				if (num != 3489864167U)
				{
					if (num == 3634381192U)
					{
						if (bindingName == "warp_icon_perspective")
						{
							value = this.warpIconPerspective;
							return true;
						}
					}
				}
				else if (bindingName == "warp_icon_stretch")
				{
					value = this.warpIconStretch;
					return true;
				}
			}
			else if (bindingName == "shownoisesettings")
			{
				XUiC_CategoryList xuiC_CategoryList14 = this.categoryList;
				bool? flag16;
				if (xuiC_CategoryList14 == null)
				{
					flag16 = null;
				}
				else
				{
					XUiC_CategoryEntry currentCategory14 = xuiC_CategoryList14.CurrentCategory;
					if (currentCategory14 == null)
					{
						flag16 = null;
					}
					else
					{
						string categoryName14 = currentCategory14.CategoryName;
						flag16 = ((categoryName14 != null) ? new bool?(categoryName14.EqualsCaseInsensitive("Noise")) : null);
					}
				}
				bool? flag2 = flag16;
				value = flag2.GetValueOrDefault().ToString();
				return true;
			}
		}
		else if (num <= 3960464195U)
		{
			if (num != 3886611224U)
			{
				if (num == 3960464195U)
				{
					if (bindingName == "showdebugpanel")
					{
						value = XUiC_SignEditorWindow.ShowDebugPanel.ToString();
						return true;
					}
				}
			}
			else if (bindingName == "showbulgewarpsettings")
			{
				XUiC_CategoryList xuiC_CategoryList15 = this.categoryList;
				bool? flag17;
				if (xuiC_CategoryList15 == null)
				{
					flag17 = null;
				}
				else
				{
					XUiC_CategoryEntry currentCategory15 = xuiC_CategoryList15.CurrentCategory;
					if (currentCategory15 == null)
					{
						flag17 = null;
					}
					else
					{
						string categoryName15 = currentCategory15.CategoryName;
						flag17 = ((categoryName15 != null) ? new bool?(categoryName15.ContainsCaseInsensitive(typeof(SignData.BulgeWarp).Name)) : null);
					}
				}
				bool? flag2 = flag17;
				value = flag2.GetValueOrDefault().ToString();
				return true;
			}
		}
		else if (num != 4088187011U)
		{
			if (num == 4265127676U)
			{
				if (bindingName == "warp_icon_twirl")
				{
					value = this.warpIconTwirl;
					return true;
				}
			}
		}
		else if (bindingName == "savenoticetext")
		{
			this.stringBuilder.Clear();
			if (this.signComplexityInfo.IsValid)
			{
				if (this.signComplexityInfo.TotalComplexity > 600f)
				{
					this.stringBuilder.AppendLine(Localization.Get("xuiSignComplexityExceeded", false, null));
				}
				if (this.signComplexityInfo.StackInfo.MaxCompStackIndex > 7)
				{
					if (this.stringBuilder.Length > 0)
					{
						this.stringBuilder.AppendLine();
					}
					this.stringBuilder.AppendLine(string.Format(Localization.Get("xuiSignNestedMaskLimit", false, null), this.signComplexityInfo.StackInfo.MaxCompStackIndex, 7));
				}
				if (this.signComplexityInfo.StackInfo.MaxUVStackIndex > 7)
				{
					if (this.stringBuilder.Length > 0)
					{
						this.stringBuilder.AppendLine();
					}
					this.stringBuilder.AppendLine(string.Format(Localization.Get("xuiSignNestedWarpLimit", false, null), this.signComplexityInfo.StackInfo.MaxUVStackIndex, 7));
				}
			}
			value = this.stringBuilder.ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x06008382 RID: 33666 RVA: 0x00323644 File Offset: 0x00321844
	public override bool ParseAttribute(string name, string value)
	{
		bool flag = base.ParseAttribute(name, value);
		if (!flag)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
			if (num <= 3167422962U)
			{
				if (num <= 2797060081U)
				{
					if (num != 2347086190U)
					{
						if (num == 2797060081U)
						{
							if (name == "warp_icon_kaleido")
							{
								this.warpIconKaleido = value;
								return true;
							}
						}
					}
					else if (name == "warp_icon_arc")
					{
						this.warpIconArc = value;
						return true;
					}
				}
				else if (num != 3049165781U)
				{
					if (num == 3167422962U)
					{
						if (name == "warp_icon_grid")
						{
							this.warpIconGrid = value;
							return true;
						}
					}
				}
				else if (name == "warp_icon_bulge")
				{
					this.warpIconBulge = value;
					return true;
				}
			}
			else if (num <= 3489864167U)
			{
				if (num != 3368053736U)
				{
					if (num == 3489864167U)
					{
						if (name == "warp_icon_stretch")
						{
							this.warpIconStretch = value;
							return true;
						}
					}
				}
				else if (name == "warp_icon_skew")
				{
					this.warpIconSkew = value;
					return true;
				}
			}
			else if (num != 3634381192U)
			{
				if (num == 4265127676U)
				{
					if (name == "warp_icon_twirl")
					{
						this.warpIconTwirl = value;
						return true;
					}
				}
			}
			else if (name == "warp_icon_perspective")
			{
				this.warpIconPerspective = value;
				return true;
			}
		}
		return flag;
	}

	// Token: 0x06008386 RID: 33670 RVA: 0x003238F3 File Offset: 0x00321AF3
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <SetLayerSelected>g__ToggleNewLayerPopup|95_0(bool visible)
	{
		if (this.showNewLayerPanel == visible)
		{
			return;
		}
		if (!visible)
		{
			this.addLayerIndex = -1;
		}
		this.showNewLayerPanel = visible;
		base.RefreshBindings();
	}

	// Token: 0x06008387 RID: 33671 RVA: 0x00323918 File Offset: 0x00321B18
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <RefreshLayerSettings>g__AddWarpCategories|96_0(ref XUiC_SignEditorWindow.<>c__DisplayClass96_0 A_1)
	{
		for (int i = 0; i < this.selectedLayer.warps.Count; i++)
		{
			SignData.SignWarp signWarp = this.selectedLayer.warps[i];
			if (!(signWarp is SignData.SkewWarp))
			{
				if (!(signWarp is SignData.BulgeWarp))
				{
					if (!(signWarp is SignData.TwirlWarp))
					{
						if (!(signWarp is SignData.KaleidoWarp))
						{
							if (!(signWarp is SignData.PerspectiveWarp))
							{
								if (!(signWarp is SignData.ArcWarp))
								{
									if (!(signWarp is SignData.StretchWarp))
									{
										if (!(signWarp is SignData.GridWarp))
										{
											Log.Error(string.Format("Unsupported layer warp type at index {0}.", i));
										}
										else
										{
											XUiC_CategoryList xuiC_CategoryList = this.categoryList;
											int num = A_1.i;
											A_1.i = num + 1;
											xuiC_CategoryList.SetCategoryEntry(num, string.Format("{0}_{1}", typeof(SignData.GridWarp).Name, i), this.warpIconGrid, Localization.Get("lblSignGridWarpHeader", false, null));
										}
									}
									else
									{
										XUiC_CategoryList xuiC_CategoryList2 = this.categoryList;
										int num = A_1.i;
										A_1.i = num + 1;
										xuiC_CategoryList2.SetCategoryEntry(num, string.Format("{0}_{1}", typeof(SignData.StretchWarp).Name, i), this.warpIconStretch, Localization.Get("lblSignStretchWarpHeader", false, null));
									}
								}
								else
								{
									XUiC_CategoryList xuiC_CategoryList3 = this.categoryList;
									int num = A_1.i;
									A_1.i = num + 1;
									xuiC_CategoryList3.SetCategoryEntry(num, string.Format("{0}_{1}", typeof(SignData.ArcWarp).Name, i), this.warpIconArc, Localization.Get("lblSignArcWarpHeader", false, null));
								}
							}
							else
							{
								XUiC_CategoryList xuiC_CategoryList4 = this.categoryList;
								int num = A_1.i;
								A_1.i = num + 1;
								xuiC_CategoryList4.SetCategoryEntry(num, string.Format("{0}_{1}", typeof(SignData.PerspectiveWarp).Name, i), this.warpIconPerspective, Localization.Get("lblSignPerspectiveWarpHeader", false, null));
							}
						}
						else
						{
							XUiC_CategoryList xuiC_CategoryList5 = this.categoryList;
							int num = A_1.i;
							A_1.i = num + 1;
							xuiC_CategoryList5.SetCategoryEntry(num, string.Format("{0}_{1}", typeof(SignData.KaleidoWarp).Name, i), this.warpIconKaleido, Localization.Get("lblSignKaleidoWarpHeader", false, null));
						}
					}
					else
					{
						XUiC_CategoryList xuiC_CategoryList6 = this.categoryList;
						int num = A_1.i;
						A_1.i = num + 1;
						xuiC_CategoryList6.SetCategoryEntry(num, string.Format("{0}_{1}", typeof(SignData.TwirlWarp).Name, i), this.warpIconTwirl, Localization.Get("lblSignTwirlWarpHeader", false, null));
					}
				}
				else
				{
					XUiC_CategoryList xuiC_CategoryList7 = this.categoryList;
					int num = A_1.i;
					A_1.i = num + 1;
					xuiC_CategoryList7.SetCategoryEntry(num, string.Format("{0}_{1}", typeof(SignData.BulgeWarp).Name, i), this.warpIconBulge, Localization.Get("lblSignPinchBulgeWarpHeader", false, null));
				}
			}
			else
			{
				XUiC_CategoryList xuiC_CategoryList8 = this.categoryList;
				int num = A_1.i;
				A_1.i = num + 1;
				xuiC_CategoryList8.SetCategoryEntry(num, string.Format("{0}_{1}", typeof(SignData.SkewWarp).Name, i), this.warpIconSkew, Localization.Get("lblSignSkewWarpHeader", false, null));
			}
		}
		if (this.selectedLayer.warps.Count < 4)
		{
			XUiC_CategoryList xuiC_CategoryList9 = this.categoryList;
			int num = A_1.i;
			A_1.i = num + 1;
			xuiC_CategoryList9.SetCategoryEntry(num, "AddWarp", "ui_game_symbol_add", Localization.Get("lblSignAddWarp", false, null));
		}
	}

	// Token: 0x06008388 RID: 33672 RVA: 0x00323C98 File Offset: 0x00321E98
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <HandleMiddleMouseDragged>g__UpdateViewOffset|122_0()
	{
		Vector2 vector = this.middleMouseDrag.RawPosition - this.middleMouseDrag.StartPosition;
		if (this.dragStartViewScaleMode)
		{
			float num = this.dragStartViewScale * Mathf.Pow(5f, vector.y);
			this.viewScale = num;
			this.viewOffset = this.dragStartViewOffset * (this.dragStartViewScale / num);
			return;
		}
		this.viewScale = this.dragStartViewScale;
		this.viewOffset = this.dragStartViewOffset + vector;
	}

	// Token: 0x06008389 RID: 33673 RVA: 0x00323D20 File Offset: 0x00321F20
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <HandleMiddleMouseDragged>g__BeginDrag|122_1()
	{
		this.middleMouseDrag.StartPosition = this.xui.GetMouseXUiPosition().AsVector2();
		this.middleMouseDrag.RawPosition = this.middleMouseDrag.StartPosition;
		this.middleMouseDrag.IsDragging = true;
		this.dragStartViewOffset = this.viewOffset;
		this.dragStartViewScale = this.viewScale;
		this.dragStartViewScaleMode = InputUtils.ControlKeyPressed;
	}

	// Token: 0x0600838A RID: 33674 RVA: 0x00323D90 File Offset: 0x00321F90
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <HandleMiddleMouseDragged>g__EndDrag|122_2()
	{
		this.middleMouseDrag.IsDragging = false;
		this.<HandleMiddleMouseDragged>g__UpdateViewOffset|122_0();
	}

	// Token: 0x04006165 RID: 24933
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cMaxWarpsPerLayer = 4;

	// Token: 0x04006166 RID: 24934
	public static string ID = "";

	// Token: 0x04006167 RID: 24935
	public static bool ShowDebugPanel = false;

	// Token: 0x04006168 RID: 24936
	public Action OnRefreshed;

	// Token: 0x04006169 RID: 24937
	[PublicizedFrom(EAccessModifier.Private)]
	public SignCanvas targetCanvas;

	// Token: 0x0400616A RID: 24938
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureCanvas targetEntity;

	// Token: 0x0400616B RID: 24939
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId sourceSignId = GlobalSignId.InvalidId;

	// Token: 0x0400616C RID: 24940
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData sourceSignData;

	// Token: 0x0400616D RID: 24941
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId workingSignId = GlobalSignId.InvalidId;

	// Token: 0x0400616E RID: 24942
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData workingSignData;

	// Token: 0x0400616F RID: 24943
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture signMaterial;

	// Token: 0x04006170 RID: 24944
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture pulseOverlayMaterial;

	// Token: 0x04006171 RID: 24945
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cHighlightPulseDuration = 1f;

	// Token: 0x04006172 RID: 24946
	[PublicizedFrom(EAccessModifier.Private)]
	public float highlightStartTime = 1f;

	// Token: 0x04006173 RID: 24947
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture hoverOverlayMaterial;

	// Token: 0x04006174 RID: 24948
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cHoverFadeDuration = 0.25f;

	// Token: 0x04006175 RID: 24949
	[PublicizedFrom(EAccessModifier.Private)]
	public float hoverOverlayOpacity;

	// Token: 0x04006176 RID: 24950
	[PublicizedFrom(EAccessModifier.Private)]
	public bool releaseLockOnClose = true;

	// Token: 0x04006177 RID: 24951
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.SignLayer hoveredLayer;

	// Token: 0x04006178 RID: 24952
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.SignLayer hoverOverlayLayer;

	// Token: 0x04006179 RID: 24953
	[PublicizedFrom(EAccessModifier.Private)]
	public float previewAspect;

	// Token: 0x0400617A RID: 24954
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;

	// Token: 0x0400617B RID: 24955
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerGrid signLayerGrid;

	// Token: 0x0400617C RID: 24956
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.SignLayer selectedLayer;

	// Token: 0x0400617D RID: 24957
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_SignLayerSettings> allSettings = new List<XUiC_SignLayerSettings>();

	// Token: 0x0400617E RID: 24958
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_SignWarpSettings> allWarpSettings = new List<XUiC_SignWarpSettings>();

	// Token: 0x0400617F RID: 24959
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_SignColorSettings> allColorSettings = new List<XUiC_SignColorSettings>();

	// Token: 0x04006180 RID: 24960
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_AddWarpSettings addWarpSettings;

	// Token: 0x04006181 RID: 24961
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignTransformSettings transformSettings;

	// Token: 0x04006182 RID: 24962
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController canvasView;

	// Token: 0x04006183 RID: 24963
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignNewLayerPanel newLayerPanel;

	// Token: 0x04006184 RID: 24964
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnSave;

	// Token: 0x04006185 RID: 24965
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnSaveCopy;

	// Token: 0x04006186 RID: 24966
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnQuit;

	// Token: 0x04006187 RID: 24967
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtAspectHorizontal;

	// Token: 0x04006188 RID: 24968
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtAspectVertical;

	// Token: 0x04006189 RID: 24969
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnAspectFit;

	// Token: 0x0400618A RID: 24970
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnAspectSquare;

	// Token: 0x0400618B RID: 24971
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtSignName;

	// Token: 0x0400618C RID: 24972
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtLayerName;

	// Token: 0x0400618D RID: 24973
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SizeBar sizeBar;

	// Token: 0x0400618E RID: 24974
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<SignData.SignLayer> layerClipboard = new List<SignData.SignLayer>();

	// Token: 0x0400618F RID: 24975
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ConfirmationPrompt confirmationPrompt;

	// Token: 0x04006190 RID: 24976
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignDebugPanel debugPanel;

	// Token: 0x04006191 RID: 24977
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerDragAndDropIcon dragAndDropIcon;

	// Token: 0x04006192 RID: 24978
	[PublicizedFrom(EAccessModifier.Private)]
	public string selectedLayerName;

	// Token: 0x04006193 RID: 24979
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showNewLayerPanel;

	// Token: 0x04006194 RID: 24980
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasUnsavedChanges;

	// Token: 0x04006195 RID: 24981
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 canvasAspect = Vector2.one;

	// Token: 0x04006196 RID: 24982
	[PublicizedFrom(EAccessModifier.Private)]
	public bool aspectMaskEnabled = true;

	// Token: 0x04006197 RID: 24983
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SignEditorWindow.HistoryStateManager historyStateManager = new XUiC_SignEditorWindow.HistoryStateManager();

	// Token: 0x04006198 RID: 24984
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignEditorWindow.AffectMode currentAffectMode;

	// Token: 0x04006199 RID: 24985
	public Action<XUiC_SignEditorWindow.AffectMode> OnAffectModeChanged;

	// Token: 0x0400619A RID: 24986
	public SignComplexityInfo signComplexityInfo;

	// Token: 0x0400619B RID: 24987
	[PublicizedFrom(EAccessModifier.Private)]
	public string warpIconSkew;

	// Token: 0x0400619C RID: 24988
	[PublicizedFrom(EAccessModifier.Private)]
	public string warpIconBulge;

	// Token: 0x0400619D RID: 24989
	[PublicizedFrom(EAccessModifier.Private)]
	public string warpIconTwirl;

	// Token: 0x0400619E RID: 24990
	[PublicizedFrom(EAccessModifier.Private)]
	public string warpIconKaleido;

	// Token: 0x0400619F RID: 24991
	[PublicizedFrom(EAccessModifier.Private)]
	public string warpIconPerspective;

	// Token: 0x040061A0 RID: 24992
	[PublicizedFrom(EAccessModifier.Private)]
	public string warpIconArc;

	// Token: 0x040061A1 RID: 24993
	[PublicizedFrom(EAccessModifier.Private)]
	public string warpIconStretch;

	// Token: 0x040061A2 RID: 24994
	[PublicizedFrom(EAccessModifier.Private)]
	public string warpIconGrid;

	// Token: 0x040061A3 RID: 24995
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly StringBuilder stringBuilder = new StringBuilder();

	// Token: 0x040061A4 RID: 24996
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> originalLayerIndices = new List<int>();

	// Token: 0x040061A5 RID: 24997
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SignData.SignLayer> originalLayers = new List<SignData.SignLayer>();

	// Token: 0x040061A6 RID: 24998
	[PublicizedFrom(EAccessModifier.Private)]
	public int addLayerIndex = -1;

	// Token: 0x040061A7 RID: 24999
	[PublicizedFrom(EAccessModifier.Private)]
	public float complexityHoverOffset;

	// Token: 0x040061A8 RID: 25000
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignEditorWindow.DragInfo middleMouseDrag;

	// Token: 0x040061A9 RID: 25001
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 viewOffset = Vector2.zero;

	// Token: 0x040061AA RID: 25002
	[PublicizedFrom(EAccessModifier.Private)]
	public float viewScale = 1f;

	// Token: 0x040061AB RID: 25003
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 dragStartViewOffset = Vector2.zero;

	// Token: 0x040061AC RID: 25004
	[PublicizedFrom(EAccessModifier.Private)]
	public float dragStartViewScale = 1f;

	// Token: 0x040061AD RID: 25005
	[PublicizedFrom(EAccessModifier.Private)]
	public bool dragStartViewScaleMode;

	// Token: 0x040061AE RID: 25006
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignEditorWindow.DragInfo leftMouseDrag;

	// Token: 0x040061AF RID: 25007
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector2> layerDragStartPositions = new List<Vector2>();

	// Token: 0x040061B0 RID: 25008
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 affectDragGroupStartPosition;

	// Token: 0x040061B1 RID: 25009
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector2> affectDragChildStartPositions = new List<Vector2>();

	// Token: 0x040061B2 RID: 25010
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> indicesToSelect = new List<int>();

	// Token: 0x040061B3 RID: 25011
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime heldInputRetriggerTime;

	// Token: 0x040061B4 RID: 25012
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openedInEditMode;

	// Token: 0x040061B5 RID: 25013
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly TimeSpan firstRetriggerDelay = TimeSpan.FromSeconds(0.3499999940395355);

	// Token: 0x040061B6 RID: 25014
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly TimeSpan subsequentRetriggerDelay = TimeSpan.FromSeconds(0.05000000074505806);

	// Token: 0x0200104B RID: 4171
	public enum AffectMode
	{
		// Token: 0x040061B8 RID: 25016
		None,
		// Token: 0x040061B9 RID: 25017
		Children,
		// Token: 0x040061BA RID: 25018
		Pivot
	}

	// Token: 0x0200104C RID: 4172
	[PublicizedFrom(EAccessModifier.Private)]
	public class HistoryStateManager
	{
		// Token: 0x0600838B RID: 33675 RVA: 0x00323DA4 File Offset: 0x00321FA4
		public void Init(string baseChangeDescription, XUiC_SignLayerGrid signLayerGrid, XUiC_TextInput txtSignName, GlobalSignId workingSignId, SignData workingSignData, XUiC_SignEditorWindow editorWindow)
		{
			this.Reset();
			this.signLayerGrid = signLayerGrid;
			this.txtSignName = txtSignName;
			this.workingSignId = workingSignId;
			this.workingSignData = workingSignData;
			this.editorWindow = editorWindow;
			XUiC_SignEditorWindow.HistoryStateManager.HistoryState historyState = this.UnpoolOrCreateHistoryState();
			historyState.InitPending(baseChangeDescription);
			this.historyStates.Add(historyState);
			this.currentStateIndex++;
		}

		// Token: 0x0600838C RID: 33676 RVA: 0x00323E05 File Offset: 0x00322005
		public void Reset()
		{
			this.PoolHistoryStates(0, this.historyStates.Count);
			this.currentStateIndex = -1;
			this.signLayerGrid = null;
			this.workingSignData = null;
			this.workingSignId = GlobalSignId.InvalidId;
			this.editorWindow = null;
		}

		// Token: 0x0600838D RID: 33677 RVA: 0x00323E40 File Offset: 0x00322040
		[PublicizedFrom(EAccessModifier.Private)]
		public void PoolHistoryStates(int index, int count)
		{
			if (index < 0 || index + count > this.historyStates.Count)
			{
				Log.Error(string.Format("Invalid history state range specified in PoolHistoryStates. Inputs were ({0}, {1}). historyStates.Count is {2}.", index, count, this.historyStates.Count));
				return;
			}
			for (int i = index; i < index + count; i++)
			{
				this.historyStates[i].Clear();
				this.historyStatePool.Push(this.historyStates[i]);
			}
			this.historyStates.RemoveRange(index, count);
		}

		// Token: 0x0600838E RID: 33678 RVA: 0x00323ED0 File Offset: 0x003220D0
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_SignEditorWindow.HistoryStateManager.HistoryState UnpoolOrCreateHistoryState()
		{
			if (this.historyStatePool.Count > 0)
			{
				return this.historyStatePool.Pop();
			}
			return new XUiC_SignEditorWindow.HistoryStateManager.HistoryState();
		}

		// Token: 0x0600838F RID: 33679 RVA: 0x00323EF4 File Offset: 0x003220F4
		public void ProcessPendingChange(string changeDescription, bool forceDirty = false)
		{
			if (this.currentStateIndex < this.historyStates.Count - 1)
			{
				this.PoolHistoryStates(this.currentStateIndex + 1, this.historyStates.Count - (this.currentStateIndex + 1));
			}
			if (this.currentStateIndex < 0)
			{
				Log.Warning(string.Format("Unexpected state in HistoryStateManager. Current index is {0}. Expected >= 0. Likely missed call to Init.", this.currentStateIndex));
			}
			else
			{
				this.historyStateDirty = this.historyStateDirty || forceDirty;
				if (!this.historyStateDirty && this.historyStates[this.currentStateIndex].Description == changeDescription)
				{
					this.historyStates[this.currentStateIndex].Refresh();
					return;
				}
				if (!this.historyStates[this.currentStateIndex].HasSnapshot)
				{
					this.historyStates[this.currentStateIndex].SetSnapshotData(this.signLayerGrid, this.workingSignData, this.editorWindow);
				}
			}
			XUiC_SignEditorWindow.HistoryStateManager.HistoryState historyState = this.UnpoolOrCreateHistoryState();
			historyState.InitPending(changeDescription);
			this.historyStates.Add(historyState);
			this.currentStateIndex++;
			if (this.historyStates.Count > 100)
			{
				int num = this.historyStates.Count - 100;
				this.PoolHistoryStates(0, num);
				this.currentStateIndex -= num;
			}
			if (this.currentStateIndex != this.historyStates.Count - 1)
			{
				Log.Error(string.Format("Invalid state index in HistoryStateManager. Current index is {0}. Expected {1}.", this.currentStateIndex, this.historyStates.Count - 1));
			}
			this.historyStateDirty = false;
		}

		// Token: 0x06008390 RID: 33680 RVA: 0x00324090 File Offset: 0x00322290
		public bool TryUndo()
		{
			if (this.currentStateIndex <= 0)
			{
				return false;
			}
			if (!this.historyStates[this.currentStateIndex].HasSnapshot)
			{
				this.historyStates[this.currentStateIndex].SetSnapshotData(this.signLayerGrid, this.workingSignData, this.editorWindow);
			}
			this.currentStateIndex--;
			this.ApplyHistoryState(this.historyStates[this.currentStateIndex]);
			return true;
		}

		// Token: 0x06008391 RID: 33681 RVA: 0x00324110 File Offset: 0x00322310
		public bool TryRedo()
		{
			if (this.currentStateIndex >= this.historyStates.Count - 1)
			{
				return false;
			}
			if (!this.historyStates[this.currentStateIndex].HasSnapshot)
			{
				Log.Error(string.Format("Unexpected state in HistoryStateManager. No snapshot exists for current state {0} which is not the most recent recorded state. ", this.currentStateIndex) + "Only the most recent pending state should lack a snapshot.");
				this.historyStates[this.currentStateIndex].SetSnapshotData(this.signLayerGrid, this.workingSignData, this.editorWindow);
			}
			this.currentStateIndex++;
			this.ApplyHistoryState(this.historyStates[this.currentStateIndex]);
			return true;
		}

		// Token: 0x06008392 RID: 33682 RVA: 0x003241C0 File Offset: 0x003223C0
		[PublicizedFrom(EAccessModifier.Private)]
		public void ApplyHistoryState(XUiC_SignEditorWindow.HistoryStateManager.HistoryState historyState)
		{
			this.workingSignData.CloneLayersFrom(historyState.SnapshotSignData);
			this.workingSignData.name = historyState.SnapshotSignData.name;
			this.signLayerGrid.MultiSelectedLayerIndices.Clear();
			int count = this.workingSignData.layers.Count;
			if (count == 0)
			{
				this.editorWindow.BeginAddLayerFlow(0);
			}
			else
			{
				this.signLayerGrid.MultiSelectedLayerIndices.AddRange(historyState.MultiSelectedLayerIndices);
				this.signLayerGrid.UpdateLayers(this.workingSignId, Mathf.Clamp(historyState.SelectedLayerIndex, 0, count - 1), false, true);
			}
			this.txtSignName.Text = historyState.SnapshotSignData.name;
			this.editorWindow.RestoreAffectModeFromHistory(historyState.AffectMode);
			this.ForceDirty();
		}

		// Token: 0x06008393 RID: 33683 RVA: 0x0032428B File Offset: 0x0032248B
		public void ForceDirty()
		{
			this.historyStateDirty = true;
		}

		// Token: 0x06008394 RID: 33684 RVA: 0x00324294 File Offset: 0x00322494
		[Conditional("DEBUG_LOG_SIGN_HISTORY")]
		public void LogUndoRedoStack()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[Sign History]\n");
			for (int i = 0; i < this.historyStates.Count; i++)
			{
				XUiC_SignEditorWindow.HistoryStateManager.HistoryState historyState = this.historyStates[i];
				if (i == this.currentStateIndex)
				{
					stringBuilder.Append("-> ");
				}
				stringBuilder.Append(string.Concat(new string[]
				{
					"[",
					historyState.Time.ToString("u"),
					"] ",
					historyState.Description,
					" (Snapshot: ",
					historyState.HasSnapshot.ToString(),
					")\n"
				}));
			}
			Log.Out(stringBuilder.ToString());
		}

		// Token: 0x040061BB RID: 25019
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cMaxHistoryStates = 100;

		// Token: 0x040061BC RID: 25020
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_SignLayerGrid signLayerGrid;

		// Token: 0x040061BD RID: 25021
		[PublicizedFrom(EAccessModifier.Private)]
		public GlobalSignId workingSignId = GlobalSignId.InvalidId;

		// Token: 0x040061BE RID: 25022
		[PublicizedFrom(EAccessModifier.Private)]
		public SignData workingSignData;

		// Token: 0x040061BF RID: 25023
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_TextInput txtSignName;

		// Token: 0x040061C0 RID: 25024
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_SignEditorWindow editorWindow;

		// Token: 0x040061C1 RID: 25025
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Stack<XUiC_SignEditorWindow.HistoryStateManager.HistoryState> historyStatePool = new Stack<XUiC_SignEditorWindow.HistoryStateManager.HistoryState>(100);

		// Token: 0x040061C2 RID: 25026
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<XUiC_SignEditorWindow.HistoryStateManager.HistoryState> historyStates = new List<XUiC_SignEditorWindow.HistoryStateManager.HistoryState>(100);

		// Token: 0x040061C3 RID: 25027
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentStateIndex = -1;

		// Token: 0x040061C4 RID: 25028
		[PublicizedFrom(EAccessModifier.Private)]
		public bool historyStateDirty;

		// Token: 0x0200104D RID: 4173
		[PublicizedFrom(EAccessModifier.Private)]
		public class HistoryState
		{
			// Token: 0x17000F75 RID: 3957
			// (get) Token: 0x06008396 RID: 33686 RVA: 0x00324393 File Offset: 0x00322593
			// (set) Token: 0x06008397 RID: 33687 RVA: 0x0032439B File Offset: 0x0032259B
			public string Description
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			}

			// Token: 0x17000F76 RID: 3958
			// (get) Token: 0x06008398 RID: 33688 RVA: 0x003243A4 File Offset: 0x003225A4
			// (set) Token: 0x06008399 RID: 33689 RVA: 0x003243AC File Offset: 0x003225AC
			public DateTime Time
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			}

			// Token: 0x17000F77 RID: 3959
			// (get) Token: 0x0600839A RID: 33690 RVA: 0x003243B5 File Offset: 0x003225B5
			// (set) Token: 0x0600839B RID: 33691 RVA: 0x003243BD File Offset: 0x003225BD
			public int SelectedLayerIndex
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			}

			// Token: 0x17000F78 RID: 3960
			// (get) Token: 0x0600839C RID: 33692 RVA: 0x003243C6 File Offset: 0x003225C6
			// (set) Token: 0x0600839D RID: 33693 RVA: 0x003243CE File Offset: 0x003225CE
			public List<int> MultiSelectedLayerIndices
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			} = new List<int>(10);

			// Token: 0x17000F79 RID: 3961
			// (get) Token: 0x0600839E RID: 33694 RVA: 0x003243D7 File Offset: 0x003225D7
			// (set) Token: 0x0600839F RID: 33695 RVA: 0x003243DF File Offset: 0x003225DF
			public SignData SnapshotSignData
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			}

			// Token: 0x17000F7A RID: 3962
			// (get) Token: 0x060083A0 RID: 33696 RVA: 0x003243E8 File Offset: 0x003225E8
			// (set) Token: 0x060083A1 RID: 33697 RVA: 0x003243F0 File Offset: 0x003225F0
			public XUiC_SignEditorWindow.AffectMode AffectMode
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			}

			// Token: 0x17000F7B RID: 3963
			// (get) Token: 0x060083A2 RID: 33698 RVA: 0x003243F9 File Offset: 0x003225F9
			public bool HasSnapshot
			{
				get
				{
					return this.SnapshotSignData != null;
				}
			}

			// Token: 0x060083A3 RID: 33699 RVA: 0x00324404 File Offset: 0x00322604
			public void InitPending(string description)
			{
				this.Description = description;
				this.Time = DateTime.Now;
			}

			// Token: 0x060083A4 RID: 33700 RVA: 0x00324418 File Offset: 0x00322618
			public void Refresh()
			{
				this.Time = DateTime.Now;
			}

			// Token: 0x060083A5 RID: 33701 RVA: 0x00324425 File Offset: 0x00322625
			public void SetSnapshotData(XUiC_SignLayerGrid signLayerGrid, SignData workingSignData, XUiC_SignEditorWindow editorWindow)
			{
				this.SetSnapshotData(signLayerGrid.SelectedLayerIndex, signLayerGrid.MultiSelectedLayerIndices, workingSignData, editorWindow.CurrentAffectMode);
			}

			// Token: 0x060083A6 RID: 33702 RVA: 0x00324440 File Offset: 0x00322640
			public void SetSnapshotData(int selectedLayerIndex, List<int> multiSelectedLayerIndices, SignData workingSignData, XUiC_SignEditorWindow.AffectMode affectMode)
			{
				this.SelectedLayerIndex = selectedLayerIndex;
				this.MultiSelectedLayerIndices.Clear();
				this.MultiSelectedLayerIndices.AddRange(multiSelectedLayerIndices);
				this.SnapshotSignData = SignData.Duplicate(workingSignData, "");
				this.AffectMode = affectMode;
			}

			// Token: 0x060083A7 RID: 33703 RVA: 0x00324479 File Offset: 0x00322679
			public void Clear()
			{
				this.MultiSelectedLayerIndices.Clear();
				this.SnapshotSignData = null;
			}
		}
	}

	// Token: 0x0200104E RID: 4174
	[PublicizedFrom(EAccessModifier.Private)]
	public struct DragInfo
	{
		// Token: 0x040061CB RID: 25035
		public Vector2 StartPosition;

		// Token: 0x040061CC RID: 25036
		public Vector2 RawPosition;

		// Token: 0x040061CD RID: 25037
		public bool IsDragging;
	}
}
