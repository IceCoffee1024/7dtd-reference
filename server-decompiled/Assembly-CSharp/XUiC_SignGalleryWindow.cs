using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02001051 RID: 4177
[Preserve]
public class XUiC_SignGalleryWindow : XUiController
{
	// Token: 0x17000F7D RID: 3965
	// (get) Token: 0x060083AF RID: 33711 RVA: 0x00324650 File Offset: 0x00322850
	// (set) Token: 0x060083B0 RID: 33712 RVA: 0x00324658 File Offset: 0x00322858
	public bool openedInEditMode
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F7E RID: 3966
	// (get) Token: 0x060083B1 RID: 33713 RVA: 0x00324661 File Offset: 0x00322861
	// (set) Token: 0x060083B2 RID: 33714 RVA: 0x00324669 File Offset: 0x00322869
	public SignCanvas targetCanvas
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F7F RID: 3967
	// (get) Token: 0x060083B3 RID: 33715 RVA: 0x00324672 File Offset: 0x00322872
	// (set) Token: 0x060083B4 RID: 33716 RVA: 0x0032467A File Offset: 0x0032287A
	public TEFeatureCanvas targetEntity
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F80 RID: 3968
	// (get) Token: 0x060083B5 RID: 33717 RVA: 0x00324683 File Offset: 0x00322883
	// (set) Token: 0x060083B6 RID: 33718 RVA: 0x0032468B File Offset: 0x0032288B
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

	// Token: 0x060083B7 RID: 33719 RVA: 0x003246C4 File Offset: 0x003228C4
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

	// Token: 0x060083B8 RID: 33720 RVA: 0x003249F9 File Offset: 0x00322BF9
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

	// Token: 0x060083B9 RID: 33721 RVA: 0x00324A11 File Offset: 0x00322C11
	public static string GetCurrentSaveTargetLibraryName(bool openedInEditMode)
	{
		if (openedInEditMode)
		{
			return PrefabEditModeManager.Instance.LoadedPrefab.FileNameNoExtension;
		}
		return PlatformManager.InternalLocalUserIdentifier.CombinedString;
	}

	// Token: 0x060083BA RID: 33722 RVA: 0x00324A30 File Offset: 0x00322C30
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

	// Token: 0x060083BB RID: 33723 RVA: 0x00324A93 File Offset: 0x00322C93
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnNewSignOverwritePromptResult(XUiC_ConfirmationPrompt.Result result)
	{
		if (result == XUiC_ConfirmationPrompt.Result.Confirmed)
		{
			this.overwriteWarningPending = false;
			this.CreateNewSign();
		}
	}

	// Token: 0x060083BC RID: 33724 RVA: 0x00324AA8 File Offset: 0x00322CA8
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

	// Token: 0x060083BD RID: 33725 RVA: 0x00324AF4 File Offset: 0x00322CF4
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

	// Token: 0x060083BE RID: 33726 RVA: 0x00324B64 File Offset: 0x00322D64
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

	// Token: 0x060083BF RID: 33727 RVA: 0x00324C00 File Offset: 0x00322E00
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

	// Token: 0x060083C0 RID: 33728 RVA: 0x00324C48 File Offset: 0x00322E48
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

	// Token: 0x060083C1 RID: 33729 RVA: 0x00324C84 File Offset: 0x00322E84
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ShouldWarnBeforeOverwrite()
	{
		return !(this.targetCanvas == null) && this.currentSignId.IsValid && SignDataManager.IsPlayerLibraryId(this.currentSignId.libraryId) && !(this.currentSignId.libraryId == PlatformManager.InternalLocalUserIdentifier.CombinedString) && (!SignDataManager.IsLocalPlayerAlliedWith(this.currentSignId.libraryId) || SignDataManager.Instance.GetEffectiveSharing(this.currentSignId) == SignData.SignSharing.Private);
	}

	// Token: 0x060083C2 RID: 33730 RVA: 0x00324D05 File Offset: 0x00322F05
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangedHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.Page = 0;
		this.RefreshItems(this.txtInput.Text);
		this.UpdateGrid(false);
	}

	// Token: 0x060083C3 RID: 33731 RVA: 0x00324D26 File Offset: 0x00322F26
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

	// Token: 0x060083C4 RID: 33732 RVA: 0x00324D54 File Offset: 0x00322F54
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

	// Token: 0x060083C5 RID: 33733 RVA: 0x003250B0 File Offset: 0x003232B0
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

	// Token: 0x060083C6 RID: 33734 RVA: 0x00325188 File Offset: 0x00323388
	[PublicizedFrom(EAccessModifier.Private)]
	public void AutoPage()
	{
		int num = this.currentItems.IndexOf(this.currentSignId);
		if (num >= 0)
		{
			this.Page = num / this.signGrid.Length;
		}
	}

	// Token: 0x060083C7 RID: 33735 RVA: 0x003251C0 File Offset: 0x003233C0
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

	// Token: 0x060083C8 RID: 33736 RVA: 0x00325253 File Offset: 0x00323453
	[PublicizedFrom(EAccessModifier.Private)]
	public void MarkPrefabNeedsSaving()
	{
		if (this.openedInEditMode)
		{
			PrefabEditModeManager.Instance.NeedsSaving = true;
		}
	}

	// Token: 0x060083C9 RID: 33737 RVA: 0x00325268 File Offset: 0x00323468
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

	// Token: 0x060083CA RID: 33738 RVA: 0x003252DE File Offset: 0x003234DE
	public static void Open(LocalPlayerUI _playerUi, TEFeatureCanvas _feature)
	{
		XUiC_SignGalleryWindow childByType = _playerUi.xui.FindWindowGroupByName(XUiC_SignGalleryWindow.ID).GetChildByType<XUiC_SignGalleryWindow>();
		childByType.targetCanvas = ((_feature != null) ? _feature.Canvas : null);
		childByType.targetEntity = _feature;
		_playerUi.windowManager.Open(XUiC_SignGalleryWindow.ID, true);
	}

	// Token: 0x060083CB RID: 33739 RVA: 0x00325320 File Offset: 0x00323520
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

	// Token: 0x060083CC RID: 33740 RVA: 0x0032537E File Offset: 0x0032357E
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateGrid(bool forceRefreshInfoPanel)
	{
		this.signGrid.SetSignIds(this.currentItems, this.currentSignId);
		if (forceRefreshInfoPanel && !this.currentItems.Contains(this.currentSignId))
		{
			this.infoWindow.SetSignInfo(this.currentSignId);
		}
	}

	// Token: 0x060083CD RID: 33741 RVA: 0x003253C0 File Offset: 0x003235C0
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

	// Token: 0x060083CE RID: 33742 RVA: 0x00325488 File Offset: 0x00323688
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnFilterDefault_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.includeDefaultLibrary = !this.includeDefaultLibrary;
		this.updateFilters = true;
		this.IsDirty = true;
	}

	// Token: 0x060083CF RID: 33743 RVA: 0x003254A7 File Offset: 0x003236A7
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnFilterPrefab_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.includePrefabLibrary = !this.includePrefabLibrary;
		this.updateFilters = true;
		this.IsDirty = true;
	}

	// Token: 0x060083D0 RID: 33744 RVA: 0x003254C6 File Offset: 0x003236C6
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnFilterOwn_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.includeOwnLibrary = !this.includeOwnLibrary;
		this.updateFilters = true;
		this.IsDirty = true;
	}

	// Token: 0x060083D1 RID: 33745 RVA: 0x003254E5 File Offset: 0x003236E5
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnFilterAllies_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.includeAlliesLibrary = !this.includeAlliesLibrary;
		this.updateFilters = true;
		this.IsDirty = true;
	}

	// Token: 0x060083D2 RID: 33746 RVA: 0x00325504 File Offset: 0x00323704
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnFilterAllPrefabs_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.includeAllPrefabsLibrary = !this.includeAllPrefabsLibrary;
		this.updateFilters = true;
		this.IsDirty = true;
	}

	// Token: 0x060083D7 RID: 33751 RVA: 0x003255F0 File Offset: 0x003237F0
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

	// Token: 0x040061D4 RID: 25044
	public static string ID = "";

	// Token: 0x040061D5 RID: 25045
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label resultCount;

	// Token: 0x040061D6 RID: 25046
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignStackGrid signGrid;

	// Token: 0x040061D7 RID: 25047
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x040061D8 RID: 25048
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x040061D9 RID: 25049
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x040061DA RID: 25050
	[PublicizedFrom(EAccessModifier.Private)]
	public bool updateFilters;

	// Token: 0x040061DB RID: 25051
	[PublicizedFrom(EAccessModifier.Private)]
	public bool includeDefaultLibrary = true;

	// Token: 0x040061DC RID: 25052
	[PublicizedFrom(EAccessModifier.Private)]
	public bool includeOwnLibrary = true;

	// Token: 0x040061DD RID: 25053
	[PublicizedFrom(EAccessModifier.Private)]
	public bool includeAlliesLibrary = true;

	// Token: 0x040061DE RID: 25054
	[PublicizedFrom(EAccessModifier.Private)]
	public bool includePrefabLibrary = true;

	// Token: 0x040061DF RID: 25055
	[PublicizedFrom(EAccessModifier.Private)]
	public bool includeAllPrefabsLibrary = true;

	// Token: 0x040061E0 RID: 25056
	[PublicizedFrom(EAccessModifier.Private)]
	public bool releaseLockOnClose = true;

	// Token: 0x040061E2 RID: 25058
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x040061E3 RID: 25059
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnNew;

	// Token: 0x040061E4 RID: 25060
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblTotal;

	// Token: 0x040061E7 RID: 25063
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId currentSignId = GlobalSignId.InvalidId;

	// Token: 0x040061E8 RID: 25064
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignInfoWindow infoWindow;

	// Token: 0x040061E9 RID: 25065
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignInstanceWindow instanceWindow;

	// Token: 0x040061EA RID: 25066
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ConfirmationPrompt confirmationPrompt;

	// Token: 0x040061EB RID: 25067
	[PublicizedFrom(EAccessModifier.Private)]
	public bool overwriteWarningPending;

	// Token: 0x040061EC RID: 25068
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId pendingSelectSignId = GlobalSignId.InvalidId;

	// Token: 0x040061ED RID: 25069
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Button filterDefaultButton;

	// Token: 0x040061EE RID: 25070
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Button filterOwnButton;

	// Token: 0x040061EF RID: 25071
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Button filterAlliesButton;

	// Token: 0x040061F0 RID: 25072
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Button filterPrefabButton;

	// Token: 0x040061F1 RID: 25073
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Button filterAllPrefabsButton;

	// Token: 0x040061F2 RID: 25074
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<GlobalSignId> currentItems = new List<GlobalSignId>();

	// Token: 0x040061F3 RID: 25075
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<GlobalSignId, string> signNamesById = new Dictionary<GlobalSignId, string>();
}
