using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02001041 RID: 4161
[Preserve]
public class XUiC_SignGalleryWindow : XUiController
{
	// Token: 0x17000F72 RID: 3954
	// (get) Token: 0x0600835D RID: 33629 RVA: 0x0032294C File Offset: 0x00320B4C
	// (set) Token: 0x0600835E RID: 33630 RVA: 0x00322954 File Offset: 0x00320B54
	public bool openedInEditMode
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F73 RID: 3955
	// (get) Token: 0x0600835F RID: 33631 RVA: 0x0032295D File Offset: 0x00320B5D
	// (set) Token: 0x06008360 RID: 33632 RVA: 0x00322965 File Offset: 0x00320B65
	public SignCanvas targetCanvas
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F74 RID: 3956
	// (get) Token: 0x06008361 RID: 33633 RVA: 0x0032296E File Offset: 0x00320B6E
	// (set) Token: 0x06008362 RID: 33634 RVA: 0x00322976 File Offset: 0x00320B76
	public TEFeatureCanvas targetEntity
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F75 RID: 3957
	// (get) Token: 0x06008363 RID: 33635 RVA: 0x0032297F File Offset: 0x00320B7F
	// (set) Token: 0x06008364 RID: 33636 RVA: 0x00322987 File Offset: 0x00320B87
	public int Page
	{
		get
		{
			return this.page;
		}
		set
		{
			if (this.page != value)
			{
				this.page = value;
				this.signGrid.Page = this.page;
				XUiC_Paging xuiC_Paging = this.pager;
				if (xuiC_Paging == null)
				{
					return;
				}
				xuiC_Paging.SetPage(this.page);
			}
		}
	}

	// Token: 0x06008365 RID: 33637 RVA: 0x003229C0 File Offset: 0x00320BC0
	public override void Init()
	{
		base.Init();
		XUiC_SignGalleryWindow.ID = base.WindowGroup.Id;
		this.resultCount = (XUiV_Label)base.GetChildById("resultCount").ViewComponent;
		this.pager = base.GetChildByType<XUiC_Paging>();
		if (this.pager != null)
		{
			this.pager.OnPageChanged += delegate
			{
				this.Page = this.pager.CurrentPageNumber;
			};
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].OnScroll += this.HandleOnScroll;
		}
		base.OnScroll += this.HandleOnScroll;
		this.signGrid = base.Parent.GetChildByType<XUiC_SignStackGrid>();
		XUiC_SignStack[] childrenByType = this.signGrid.GetChildrenByType<XUiC_SignStack>(null);
		for (int j = 0; j < childrenByType.Length; j++)
		{
			childrenByType[j].OnScroll += this.HandleOnScroll;
			XUiC_SignStack xuiC_SignStack = childrenByType[j];
			xuiC_SignStack.OnBecameSelected = (Action<XUiC_SignStack>)Delegate.Combine(xuiC_SignStack.OnBecameSelected, new Action<XUiC_SignStack>(this.HandleOnSelect));
		}
		this.length = childrenByType.Length;
		this.txtInput = (XUiC_TextInput)this.windowGroup.Controller.GetChildById("searchInput");
		if (this.txtInput != null)
		{
			this.txtInput.OnChangeHandler += this.HandleOnChangedHandler;
			this.txtInput.Text = "";
		}
		this.btnNew = (XUiC_SimpleButton)base.GetChildById("btnNew");
		this.btnNew.OnPressed += this.BtnNew_OnPressed;
		this.infoWindow = this.windowGroup.Controller.GetChildByType<XUiC_SignInfoWindow>();
		this.instanceWindow = this.windowGroup.Controller.GetChildByType<XUiC_SignInstanceWindow>();
		this.confirmationPrompt = base.GetChildById("confirmation_prompt_controller") as XUiC_ConfirmationPrompt;
		XUiC_SignInfoWindow xuiC_SignInfoWindow = this.infoWindow;
		xuiC_SignInfoWindow.OnSignAddOrRemove = (Action)Delegate.Combine(xuiC_SignInfoWindow.OnSignAddOrRemove, new Action(this.MarkPrefabNeedsSaving));
		XUiC_SignInstanceWindow xuiC_SignInstanceWindow = this.instanceWindow;
		xuiC_SignInstanceWindow.OnSettingsChanged = (Action)Delegate.Combine(xuiC_SignInstanceWindow.OnSettingsChanged, new Action(this.HandleInstanceSettingsChanged));
		this.lblTotal = Localization.Get("lblTotalItems", false, null);
		XUiController childById = base.GetChildById("filterDefault");
		this.filterDefaultButton = childById.ViewComponent as XUiV_Button;
		childById.OnPress += this.BtnFilterDefault_OnPressed;
		XUiController childById2 = base.GetChildById("filterOwn");
		this.filterOwnButton = childById2.ViewComponent as XUiV_Button;
		childById2.OnPress += this.BtnFilterOwn_OnPressed;
		XUiController childById3 = base.GetChildById("filterAllies");
		this.filterAlliesButton = childById3.ViewComponent as XUiV_Button;
		childById3.OnPress += this.BtnFilterAllies_OnPressed;
		XUiController childById4 = base.GetChildById("filterPrefab");
		this.filterPrefabButton = childById4.ViewComponent as XUiV_Button;
		childById4.OnPress += this.BtnFilterPrefab_OnPressed;
		XUiController childById5 = base.GetChildById("filterAllPrefabs");
		this.filterAllPrefabsButton = childById5.ViewComponent as XUiV_Button;
		childById5.OnPress += this.BtnFilterAllPrefabs_OnPressed;
	}

	// Token: 0x06008366 RID: 33638 RVA: 0x00322CF5 File Offset: 0x00320EF5
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleInstanceSettingsChanged()
	{
		this.MarkPrefabNeedsSaving();
		TEFeatureCanvas targetEntity = this.targetEntity;
		if (targetEntity == null)
		{
			return;
		}
		targetEntity.SetModified();
	}

	// Token: 0x06008367 RID: 33639 RVA: 0x00322D0D File Offset: 0x00320F0D
	public static string GetCurrentSaveTargetLibraryName(bool openedInEditMode)
	{
		if (openedInEditMode)
		{
			return PrefabEditModeManager.Instance.LoadedPrefab.FileNameNoExtension;
		}
		return PlatformManager.InternalLocalUserIdentifier.CombinedString;
	}

	// Token: 0x06008368 RID: 33640 RVA: 0x00322D2C File Offset: 0x00320F2C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnNew_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (this.overwriteWarningPending)
		{
			this.confirmationPrompt.ShowPrompt(Localization.Get("lblSignOverwriteTitle", false, null), Localization.Get("lblSignOverwriteMessage", false, null), Localization.Get("xuiCancel", false, null), Localization.Get("xuiConfirm", false, null), new Action<XUiC_ConfirmationPrompt.Result>(this.OnNewSignOverwritePromptResult));
			return;
		}
		this.CreateNewSign();
	}

	// Token: 0x06008369 RID: 33641 RVA: 0x00322D8F File Offset: 0x00320F8F
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnNewSignOverwritePromptResult(XUiC_ConfirmationPrompt.Result result)
	{
		if (result == XUiC_ConfirmationPrompt.Result.Confirmed)
		{
			this.overwriteWarningPending = false;
			this.CreateNewSign();
		}
	}

	// Token: 0x0600836A RID: 33642 RVA: 0x00322DA4 File Offset: 0x00320FA4
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateNewSign()
	{
		string currentSaveTargetLibraryName = XUiC_SignGalleryWindow.GetCurrentSaveTargetLibraryName(this.openedInEditMode);
		SignData newSignData = SignData.GetNewSignData(Localization.Get("lblSignNewSign", false, null));
		GlobalSignId globalSignId = SignDataManager.Instance.AddSignToLibrary(currentSaveTargetLibraryName, newSignData);
		this.RefreshAndSelect(globalSignId);
		this.MarkPrefabNeedsSaving();
		this.LaunchEditor();
	}

	// Token: 0x0600836B RID: 33643 RVA: 0x00322DF0 File Offset: 0x00320FF0
	public void LaunchEditor()
	{
		if (this.targetEntity != null)
		{
			this.releaseLockOnClose = false;
			XUiC_SignEditorWindow.Open(this.xui.playerUI, this.targetEntity);
			return;
		}
		SignData signData;
		if (SignDataManager.Instance.TryGetSignData(this.currentSignId, out signData))
		{
			this.releaseLockOnClose = false;
			XUiC_SignEditorWindow.Open(this.xui.playerUI, signData, this.currentSignId);
			return;
		}
		Log.Error("Failed to launch Sign Editor; block entity reference is null and the debug fallback path failed to retrieve valid sign data.");
	}

	// Token: 0x0600836C RID: 33644 RVA: 0x00322E60 File Offset: 0x00321060
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnSelect(XUiC_SignStack _sender)
	{
		if (this.currentSignId != _sender.SignId)
		{
			if (this.overwriteWarningPending)
			{
				this.pendingSelectSignId = _sender.SignId;
				this.confirmationPrompt.ShowPrompt(Localization.Get("lblSignOverwriteTitle", false, null), Localization.Get("lblSignOverwriteMessage", false, null), Localization.Get("xuiCancel", false, null), Localization.Get("xuiConfirm", false, null), new Action<XUiC_ConfirmationPrompt.Result>(this.OnOverwritePromptResult));
				return;
			}
			this.ApplySelection(_sender.SignId);
		}
		this.infoWindow.SetSignInfo(_sender.SignId);
	}

	// Token: 0x0600836D RID: 33645 RVA: 0x00322EFC File Offset: 0x003210FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnOverwritePromptResult(XUiC_ConfirmationPrompt.Result result)
	{
		GlobalSignId globalSignId = this.pendingSelectSignId;
		this.pendingSelectSignId = GlobalSignId.InvalidId;
		if (result != XUiC_ConfirmationPrompt.Result.Cancelled)
		{
			if (result == XUiC_ConfirmationPrompt.Result.Confirmed)
			{
				this.overwriteWarningPending = false;
				this.ApplySelection(globalSignId);
				this.infoWindow.SetSignInfo(globalSignId);
				return;
			}
		}
		else
		{
			this.UpdateGrid(true);
		}
	}

	// Token: 0x0600836E RID: 33646 RVA: 0x00322F44 File Offset: 0x00321144
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplySelection(GlobalSignId signId)
	{
		this.currentSignId = signId;
		if (this.targetCanvas != null)
		{
			this.targetCanvas.SignId = this.currentSignId;
			this.HandleInstanceSettingsChanged();
		}
		this.instanceWindow.RefreshPreviewRenderers();
	}

	// Token: 0x0600836F RID: 33647 RVA: 0x00322F80 File Offset: 0x00321180
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ShouldWarnBeforeOverwrite()
	{
		return !(this.targetCanvas == null) && this.currentSignId.IsValid && SignDataManager.IsPlayerLibraryId(this.currentSignId.libraryId) && !(this.currentSignId.libraryId == PlatformManager.InternalLocalUserIdentifier.CombinedString) && (!SignDataManager.IsLocalPlayerAlliedWith(this.currentSignId.libraryId) || SignDataManager.Instance.GetEffectiveSharing(this.currentSignId) == SignData.SignSharing.Private);
	}

	// Token: 0x06008370 RID: 33648 RVA: 0x00323001 File Offset: 0x00321201
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangedHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.Page = 0;
		this.RefreshItems(this.txtInput.Text);
		this.UpdateGrid(false);
	}

	// Token: 0x06008371 RID: 33649 RVA: 0x00323022 File Offset: 0x00321222
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnScroll(XUiController _sender, float _delta)
	{
		if (_delta > 0f)
		{
			XUiC_Paging xuiC_Paging = this.pager;
			if (xuiC_Paging == null)
			{
				return;
			}
			xuiC_Paging.PageDown();
			return;
		}
		else
		{
			XUiC_Paging xuiC_Paging2 = this.pager;
			if (xuiC_Paging2 == null)
			{
				return;
			}
			xuiC_Paging2.PageUp();
			return;
		}
	}

	// Token: 0x06008372 RID: 33650 RVA: 0x00323050 File Offset: 0x00321250
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshItems(string _filter)
	{
		this.currentItems.Clear();
		this.length = this.signGrid.Length;
		this.Page = 0;
		if (this.includeDefaultLibrary)
		{
			SignDataManager.Instance.AddFilteredSignIdsToList("[D]", _filter, this.currentItems);
		}
		if (PrefabEditModeManager.Instance.IsActive())
		{
			if (this.includePrefabLibrary)
			{
				string fileNameNoExtension = PrefabEditModeManager.Instance.LoadedPrefab.FileNameNoExtension;
				if (fileNameNoExtension != null && SignDataManager.Instance.HasSignLibrary(fileNameNoExtension))
				{
					SignDataManager.Instance.AddFilteredSignIdsToList(fileNameNoExtension, _filter, this.currentItems);
				}
			}
		}
		else
		{
			HashSet<string> hashSet = new HashSet<string>();
			string combinedString = PlatformManager.InternalLocalUserIdentifier.CombinedString;
			if (this.includeOwnLibrary && combinedString != null && SignDataManager.Instance.HasSignLibrary(combinedString))
			{
				SignDataManager.Instance.AddFilteredSignIdsToList(combinedString, _filter, this.currentItems);
				hashSet.Add(combinedString);
			}
			if (this.includeAlliesLibrary)
			{
				foreach (PlatformUserIdentifierAbs platformUserIdentifierAbs in GameManager.Instance.persistentPlayers.Allies.EnumerateAllies(GameManager.Instance.persistentLocalPlayer.PrimaryId))
				{
					string combinedString2 = platformUserIdentifierAbs.CombinedString;
					if (!hashSet.Contains(combinedString2) && SignDataManager.Instance.HasSignLibrary(combinedString2))
					{
						SignDataManager.Instance.AddFilteredSignIdsToList(combinedString2, _filter, this.currentItems);
						hashSet.Add(combinedString2);
					}
				}
			}
			EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
			string text;
			if (primaryPlayer == null)
			{
				text = null;
			}
			else
			{
				PrefabInstance prefab = primaryPlayer.prefab;
				text = ((prefab != null) ? prefab.location.FileNameNoExtension : null);
			}
			string text2 = text;
			if (this.includePrefabLibrary && text2 != null && SignDataManager.Instance.HasSignLibrary(text2))
			{
				SignDataManager.Instance.AddFilteredSignIdsToList(text2, _filter, this.currentItems);
				hashSet.Add(text2);
			}
			if (this.includeAllPrefabsLibrary)
			{
				World world = GameManager.Instance.World;
				DynamicPrefabDecorator dynamicPrefabDecorator = ((world != null) ? world.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator() : null);
				if (dynamicPrefabDecorator != null)
				{
					List<PrefabInstance> list = new List<PrefabInstance>();
					dynamicPrefabDecorator.GetWorldPrefabs(list);
					foreach (PrefabInstance prefabInstance in list)
					{
						string fileNameNoExtension2 = prefabInstance.location.FileNameNoExtension;
						if (!hashSet.Contains(fileNameNoExtension2) && SignDataManager.Instance.HasSignLibrary(fileNameNoExtension2))
						{
							SignDataManager.Instance.AddFilteredSignIdsToList(fileNameNoExtension2, _filter, this.currentItems);
							hashSet.Add(fileNameNoExtension2);
						}
					}
				}
			}
		}
		this.signNamesById.Clear();
		foreach (GlobalSignId globalSignId in this.currentItems)
		{
			this.signNamesById[globalSignId] = XUiC_SignGalleryWindow.<RefreshItems>g__GetNiceName|57_1(globalSignId);
		}
		this.currentItems.Sort(delegate(GlobalSignId a, GlobalSignId b)
		{
			int num = a.libraryId.CompareTo(b.libraryId);
			if (num != 0)
			{
				return num;
			}
			return string.Compare(this.signNamesById[a], this.signNamesById[b]);
		});
		XUiC_Paging xuiC_Paging = this.pager;
		if (xuiC_Paging != null)
		{
			xuiC_Paging.SetLastPageByElementsAndPageLength(this.currentItems.Count, this.length);
		}
		this.resultCount.Text = string.Format(this.lblTotal, this.currentItems.Count.ToString());
	}

	// Token: 0x06008373 RID: 33651 RVA: 0x003233AC File Offset: 0x003215AC
	public override void OnOpen()
	{
		base.OnOpen();
		this.releaseLockOnClose = this.targetEntity != null;
		this.openedInEditMode = PrefabEditModeManager.Instance.IsActive();
		this.RefreshItems(this.txtInput.Text);
		this.IsDirty = true;
		if (this.targetCanvas != null)
		{
			this.currentSignId = this.targetCanvas.SignId;
		}
		else
		{
			this.currentSignId = GlobalSignId.InvalidId;
		}
		this.overwriteWarningPending = this.ShouldWarnBeforeOverwrite();
		this.pendingSelectSignId = GlobalSignId.InvalidId;
		this.AutoPage();
		this.UpdateGrid(true);
		this.instanceWindow.InitialiseTo(this.targetEntity);
		this.RefreshFilterButtons();
		this.windowGroup.Controller.GetChildByType<XUiC_WindowNonPagingHeader>().SetHeader(Localization.Get("xuiSigns", false, null).ToUpper());
	}

	// Token: 0x06008374 RID: 33652 RVA: 0x00323484 File Offset: 0x00321684
	[PublicizedFrom(EAccessModifier.Private)]
	public void AutoPage()
	{
		int num = this.currentItems.IndexOf(this.currentSignId);
		if (num >= 0)
		{
			this.Page = num / this.signGrid.Length;
		}
	}

	// Token: 0x06008375 RID: 33653 RVA: 0x003234BC File Offset: 0x003216BC
	public override void OnClose()
	{
		base.OnClose();
		this.openedInEditMode = false;
		if (this.xui.ToolTipWindow != null)
		{
			this.xui.ToolTipWindow.ToolTip = "";
		}
		if (this.xui.playerUI.windowManager.IsWindowOpen("windowpaging"))
		{
			this.xui.playerUI.windowManager.Close("windowpaging");
		}
		if (this.releaseLockOnClose)
		{
			this.targetEntity.ReleaseLock();
		}
		this.targetCanvas = null;
		this.targetEntity = null;
	}

	// Token: 0x06008376 RID: 33654 RVA: 0x0032354F File Offset: 0x0032174F
	[PublicizedFrom(EAccessModifier.Private)]
	public void MarkPrefabNeedsSaving()
	{
		if (this.openedInEditMode)
		{
			PrefabEditModeManager.Instance.NeedsSaving = true;
		}
	}

	// Token: 0x06008377 RID: 33655 RVA: 0x00323564 File Offset: 0x00321764
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.viewComponent.IsVisible && this.IsDirty)
		{
			if (this.updateFilters)
			{
				this.RefreshFilterButtons();
				this.Page = 0;
				this.RefreshItems(this.txtInput.Text);
				this.UpdateGrid(false);
				this.updateFilters = false;
			}
			base.RefreshBindings();
			this.signGrid.IsDirty = true;
			this.IsDirty = false;
		}
	}

	// Token: 0x06008378 RID: 33656 RVA: 0x003235DA File Offset: 0x003217DA
	public static void Open(LocalPlayerUI _playerUi, TEFeatureCanvas _feature)
	{
		XUiC_SignGalleryWindow childByType = _playerUi.xui.FindWindowGroupByName(XUiC_SignGalleryWindow.ID).GetChildByType<XUiC_SignGalleryWindow>();
		childByType.targetCanvas = ((_feature != null) ? _feature.Canvas : null);
		childByType.targetEntity = _feature;
		_playerUi.windowManager.Open(XUiC_SignGalleryWindow.ID, true);
	}

	// Token: 0x06008379 RID: 33657 RVA: 0x0032361C File Offset: 0x0032181C
	public void RefreshAndSelect(GlobalSignId newSignId)
	{
		this.RefreshItems(this.txtInput.Text);
		this.IsDirty = true;
		this.currentSignId = newSignId;
		if (this.targetCanvas != null)
		{
			this.targetCanvas.SignId = this.currentSignId;
			this.HandleInstanceSettingsChanged();
		}
		this.AutoPage();
		this.UpdateGrid(true);
	}

	// Token: 0x0600837A RID: 33658 RVA: 0x0032367A File Offset: 0x0032187A
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateGrid(bool forceRefreshInfoPanel)
	{
		this.signGrid.SetSignIds(this.currentItems, this.currentSignId);
		if (forceRefreshInfoPanel && !this.currentItems.Contains(this.currentSignId))
		{
			this.infoWindow.SetSignInfo(this.currentSignId);
		}
	}

	// Token: 0x0600837B RID: 33659 RVA: 0x003236BC File Offset: 0x003218BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshFilterButtons()
	{
		this.filterDefaultButton.Selected = this.includeDefaultLibrary;
		this.filterOwnButton.Selected = this.includeOwnLibrary && !this.openedInEditMode;
		this.filterOwnButton.IsVisible = !this.openedInEditMode;
		this.filterAlliesButton.Selected = this.includeAlliesLibrary && !this.openedInEditMode;
		this.filterAlliesButton.IsVisible = !this.openedInEditMode;
		this.filterPrefabButton.Selected = this.includePrefabLibrary;
		this.filterAllPrefabsButton.Selected = this.includeAllPrefabsLibrary && !this.openedInEditMode;
		this.filterAllPrefabsButton.IsVisible = !this.openedInEditMode;
	}

	// Token: 0x0600837C RID: 33660 RVA: 0x00323784 File Offset: 0x00321984
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnFilterDefault_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.includeDefaultLibrary = !this.includeDefaultLibrary;
		this.updateFilters = true;
		this.IsDirty = true;
	}

	// Token: 0x0600837D RID: 33661 RVA: 0x003237A3 File Offset: 0x003219A3
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnFilterPrefab_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.includePrefabLibrary = !this.includePrefabLibrary;
		this.updateFilters = true;
		this.IsDirty = true;
	}

	// Token: 0x0600837E RID: 33662 RVA: 0x003237C2 File Offset: 0x003219C2
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnFilterOwn_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.includeOwnLibrary = !this.includeOwnLibrary;
		this.updateFilters = true;
		this.IsDirty = true;
	}

	// Token: 0x0600837F RID: 33663 RVA: 0x003237E1 File Offset: 0x003219E1
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnFilterAllies_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.includeAlliesLibrary = !this.includeAlliesLibrary;
		this.updateFilters = true;
		this.IsDirty = true;
	}

	// Token: 0x06008380 RID: 33664 RVA: 0x00323800 File Offset: 0x00321A00
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnFilterAllPrefabs_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.includeAllPrefabsLibrary = !this.includeAllPrefabsLibrary;
		this.updateFilters = true;
		this.IsDirty = true;
	}

	// Token: 0x06008385 RID: 33669 RVA: 0x003238EC File Offset: 0x00321AEC
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static string <RefreshItems>g__GetNiceName|57_1(GlobalSignId signId)
	{
		SignData signData;
		if (SignDataManager.Instance.TryGetSignData(signId, out signData))
		{
			return signData.name;
		}
		Log.Error("Failed to retrieve sign data for global id: '" + signId.ToString() + "'.");
		return string.Empty;
	}

	// Token: 0x04006196 RID: 24982
	public static string ID = "";

	// Token: 0x04006197 RID: 24983
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label resultCount;

	// Token: 0x04006198 RID: 24984
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignStackGrid signGrid;

	// Token: 0x04006199 RID: 24985
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x0400619A RID: 24986
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x0400619B RID: 24987
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x0400619C RID: 24988
	[PublicizedFrom(EAccessModifier.Private)]
	public bool updateFilters;

	// Token: 0x0400619D RID: 24989
	[PublicizedFrom(EAccessModifier.Private)]
	public bool includeDefaultLibrary = true;

	// Token: 0x0400619E RID: 24990
	[PublicizedFrom(EAccessModifier.Private)]
	public bool includeOwnLibrary = true;

	// Token: 0x0400619F RID: 24991
	[PublicizedFrom(EAccessModifier.Private)]
	public bool includeAlliesLibrary = true;

	// Token: 0x040061A0 RID: 24992
	[PublicizedFrom(EAccessModifier.Private)]
	public bool includePrefabLibrary = true;

	// Token: 0x040061A1 RID: 24993
	[PublicizedFrom(EAccessModifier.Private)]
	public bool includeAllPrefabsLibrary = true;

	// Token: 0x040061A2 RID: 24994
	[PublicizedFrom(EAccessModifier.Private)]
	public bool releaseLockOnClose = true;

	// Token: 0x040061A4 RID: 24996
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x040061A5 RID: 24997
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnNew;

	// Token: 0x040061A6 RID: 24998
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblTotal;

	// Token: 0x040061A9 RID: 25001
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId currentSignId = GlobalSignId.InvalidId;

	// Token: 0x040061AA RID: 25002
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignInfoWindow infoWindow;

	// Token: 0x040061AB RID: 25003
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignInstanceWindow instanceWindow;

	// Token: 0x040061AC RID: 25004
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ConfirmationPrompt confirmationPrompt;

	// Token: 0x040061AD RID: 25005
	[PublicizedFrom(EAccessModifier.Private)]
	public bool overwriteWarningPending;

	// Token: 0x040061AE RID: 25006
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId pendingSelectSignId = GlobalSignId.InvalidId;

	// Token: 0x040061AF RID: 25007
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Button filterDefaultButton;

	// Token: 0x040061B0 RID: 25008
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Button filterOwnButton;

	// Token: 0x040061B1 RID: 25009
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Button filterAlliesButton;

	// Token: 0x040061B2 RID: 25010
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Button filterPrefabButton;

	// Token: 0x040061B3 RID: 25011
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Button filterAllPrefabsButton;

	// Token: 0x040061B4 RID: 25012
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<GlobalSignId> currentItems = new List<GlobalSignId>();

	// Token: 0x040061B5 RID: 25013
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<GlobalSignId, string> signNamesById = new Dictionary<GlobalSignId, string>();
}
