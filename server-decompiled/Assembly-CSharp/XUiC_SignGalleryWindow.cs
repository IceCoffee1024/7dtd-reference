using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

// Token: 0x02001032 RID: 4146
[Preserve]
public class XUiC_SignGalleryWindow : XUiController
{
	// Token: 0x17000F4B RID: 3915
	// (get) Token: 0x0600834E RID: 33614 RVA: 0x0031F894 File Offset: 0x0031DA94
	// (set) Token: 0x0600834F RID: 33615 RVA: 0x0031F89C File Offset: 0x0031DA9C
	public SignCanvas targetCanvas
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F4C RID: 3916
	// (get) Token: 0x06008350 RID: 33616 RVA: 0x0031F8A5 File Offset: 0x0031DAA5
	// (set) Token: 0x06008351 RID: 33617 RVA: 0x0031F8AD File Offset: 0x0031DAAD
	public TEFeatureCanvas targetEntity
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F4D RID: 3917
	// (get) Token: 0x06008352 RID: 33618 RVA: 0x0031F8B6 File Offset: 0x0031DAB6
	// (set) Token: 0x06008353 RID: 33619 RVA: 0x0031F8BE File Offset: 0x0031DABE
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

	// Token: 0x06008354 RID: 33620 RVA: 0x0031F8F8 File Offset: 0x0031DAF8
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
		this.lblTotal = Localization.Get("lblTotalItems", false, null);
		XUiController childById = base.GetChildById("filterDefault");
		this.filterDefaultButton = childById.ViewComponent as XUiV_Button;
		childById.OnPress += this.BtnFilterDefault_OnPressed;
		XUiController childById2 = base.GetChildById("filterPrefab");
		this.filterPrefabButton = childById2.ViewComponent as XUiV_Button;
		childById2.OnPress += this.BtnFilterPrefab_OnPressed;
	}

	// Token: 0x06008355 RID: 33621 RVA: 0x0031FB34 File Offset: 0x0031DD34
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnNew_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (!PrefabEditModeManager.Instance.IsActive())
		{
			Log.Error("New signs are saved to prefabs and can only be created in the Prefab Editor.");
			return;
		}
		string fileNameNoExtension = PrefabEditModeManager.Instance.LoadedPrefab.FileNameNoExtension;
		SignData newSignData = SignData.GetNewSignData(Localization.Get("lblSignNewSign", false, null));
		GlobalSignId globalSignId = SignDataManager.Instance.AddSignToLibrary(fileNameNoExtension, newSignData);
		PrefabEditModeManager.Instance.NeedsSaving = true;
		this.RefreshAndSelect(globalSignId);
		if (this.targetEntity != null)
		{
			XUiC_SignEditorWindow.Open(this.xui.playerUI, this.targetEntity);
			return;
		}
		XUiC_SignEditorWindow.Open(this.xui.playerUI, newSignData, globalSignId);
	}

	// Token: 0x06008356 RID: 33622 RVA: 0x0031FBCC File Offset: 0x0031DDCC
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnSelect(XUiC_SignStack _sender)
	{
		if (this.currentSignId != _sender.SignId)
		{
			this.currentSignId = _sender.SignId;
			if (this.targetCanvas != null)
			{
				this.targetCanvas.SignId = this.currentSignId;
			}
			this.instanceWindow.RefreshPreviewRenderers();
			PrefabEditModeManager.Instance.NeedsSaving = true;
		}
		this.infoWindow.SetSignInfo(_sender.SignId);
	}

	// Token: 0x06008357 RID: 33623 RVA: 0x0031FC3E File Offset: 0x0031DE3E
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangedHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.Page = 0;
		this.RefreshItems(this.txtInput.Text);
		this.UpdateGrid(false);
	}

	// Token: 0x06008358 RID: 33624 RVA: 0x0031FC5F File Offset: 0x0031DE5F
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

	// Token: 0x06008359 RID: 33625 RVA: 0x0031FC8C File Offset: 0x0031DE8C
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
		if (PrefabEditModeManager.Instance.IsActive() && this.includePrefabLibrary)
		{
			string fileNameNoExtension = PrefabEditModeManager.Instance.LoadedPrefab.FileNameNoExtension;
			if (fileNameNoExtension != null && SignDataManager.Instance.HasSignLibrary(fileNameNoExtension))
			{
				SignDataManager.Instance.AddFilteredSignIdsToList(PrefabEditModeManager.Instance.LoadedPrefab.FileNameNoExtension, _filter, this.currentItems);
			}
		}
		this.signNamesById.Clear();
		foreach (GlobalSignId globalSignId in this.currentItems)
		{
			this.signNamesById[globalSignId] = XUiC_SignGalleryWindow.<RefreshItems>g__GetNiceName|35_1(globalSignId);
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

	// Token: 0x0600835A RID: 33626 RVA: 0x0031FDEC File Offset: 0x0031DFEC
	public override void OnOpen()
	{
		base.OnOpen();
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
		this.AutoPage();
		this.UpdateGrid(true);
		this.instanceWindow.InitialiseTo(this.targetEntity);
		this.RefreshFilterButtons();
		this.windowGroup.Controller.GetChildByType<XUiC_WindowNonPagingHeader>().SetHeader(Localization.Get("xuiSigns", false, null).ToUpper());
	}

	// Token: 0x0600835B RID: 33627 RVA: 0x0031FE90 File Offset: 0x0031E090
	[PublicizedFrom(EAccessModifier.Private)]
	public void AutoPage()
	{
		int num = this.currentItems.IndexOf(this.currentSignId);
		if (num >= 0)
		{
			this.Page = num / this.signGrid.Length;
		}
	}

	// Token: 0x0600835C RID: 33628 RVA: 0x0031FEC8 File Offset: 0x0031E0C8
	public override void OnClose()
	{
		base.OnClose();
		this.xui.ToolTipWindow.ToolTip = "";
		if (this.xui.playerUI.windowManager.IsWindowOpen("windowpaging"))
		{
			this.xui.playerUI.windowManager.Close("windowpaging");
		}
		if (this.targetCanvas != null)
		{
			this.targetCanvas.SignId = this.currentSignId;
			this.targetCanvas = null;
		}
		this.targetEntity = null;
	}

	// Token: 0x0600835D RID: 33629 RVA: 0x0031FF54 File Offset: 0x0031E154
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

	// Token: 0x0600835E RID: 33630 RVA: 0x0031FFCA File Offset: 0x0031E1CA
	public static void Open(LocalPlayerUI _playerUi, TEFeatureCanvas _feature)
	{
		XUiC_SignGalleryWindow childByType = _playerUi.xui.FindWindowGroupByName(XUiC_SignGalleryWindow.ID).GetChildByType<XUiC_SignGalleryWindow>();
		childByType.targetCanvas = ((_feature != null) ? _feature.Canvas : null);
		childByType.targetEntity = _feature;
		_playerUi.windowManager.Open(XUiC_SignGalleryWindow.ID, true);
	}

	// Token: 0x0600835F RID: 33631 RVA: 0x0032000C File Offset: 0x0031E20C
	public void RefreshAndSelect(GlobalSignId newSignId)
	{
		this.RefreshItems(this.txtInput.Text);
		this.IsDirty = true;
		this.currentSignId = newSignId;
		if (this.targetCanvas != null)
		{
			this.targetCanvas.SignId = this.currentSignId;
		}
		this.AutoPage();
		this.UpdateGrid(true);
	}

	// Token: 0x06008360 RID: 33632 RVA: 0x00320064 File Offset: 0x0031E264
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateGrid(bool forceRefreshInfoPanel)
	{
		this.signGrid.SetSignIds(this.currentItems, this.currentSignId);
		if (forceRefreshInfoPanel && !this.currentItems.Contains(this.currentSignId))
		{
			this.infoWindow.SetSignInfo(this.currentSignId);
		}
	}

	// Token: 0x06008361 RID: 33633 RVA: 0x003200A4 File Offset: 0x0031E2A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshFilterButtons()
	{
		this.filterDefaultButton.Selected = this.includeDefaultLibrary;
		this.filterPrefabButton.Selected = this.includePrefabLibrary;
	}

	// Token: 0x06008362 RID: 33634 RVA: 0x003200C8 File Offset: 0x0031E2C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnFilterDefault_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.includeDefaultLibrary = !this.includeDefaultLibrary;
		this.updateFilters = true;
		this.IsDirty = true;
	}

	// Token: 0x06008363 RID: 33635 RVA: 0x003200E7 File Offset: 0x0031E2E7
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnFilterPrefab_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.includePrefabLibrary = !this.includePrefabLibrary;
		this.updateFilters = true;
		this.IsDirty = true;
	}

	// Token: 0x06008368 RID: 33640 RVA: 0x003201A0 File Offset: 0x0031E3A0
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static string <RefreshItems>g__GetNiceName|35_1(GlobalSignId signId)
	{
		SignData signData;
		if (SignDataManager.Instance.TryGetSignData(signId, out signData))
		{
			return signData.name;
		}
		Log.Error("Failed to retrieve sign data for global id: '" + signId.ToString() + "'.");
		return string.Empty;
	}

	// Token: 0x0400616B RID: 24939
	public static string ID = "";

	// Token: 0x0400616C RID: 24940
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label resultCount;

	// Token: 0x0400616D RID: 24941
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignStackGrid signGrid;

	// Token: 0x0400616E RID: 24942
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x0400616F RID: 24943
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04006170 RID: 24944
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x04006171 RID: 24945
	[PublicizedFrom(EAccessModifier.Private)]
	public bool updateFilters;

	// Token: 0x04006172 RID: 24946
	[PublicizedFrom(EAccessModifier.Private)]
	public bool includeDefaultLibrary = true;

	// Token: 0x04006173 RID: 24947
	[PublicizedFrom(EAccessModifier.Private)]
	public bool includePrefabLibrary = true;

	// Token: 0x04006174 RID: 24948
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x04006175 RID: 24949
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnNew;

	// Token: 0x04006176 RID: 24950
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblTotal;

	// Token: 0x04006179 RID: 24953
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId currentSignId = GlobalSignId.InvalidId;

	// Token: 0x0400617A RID: 24954
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignInfoWindow infoWindow;

	// Token: 0x0400617B RID: 24955
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignInstanceWindow instanceWindow;

	// Token: 0x0400617C RID: 24956
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Button filterDefaultButton;

	// Token: 0x0400617D RID: 24957
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Button filterPrefabButton;

	// Token: 0x0400617E RID: 24958
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<GlobalSignId> currentItems = new List<GlobalSignId>();

	// Token: 0x0400617F RID: 24959
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<GlobalSignId, string> signNamesById = new Dictionary<GlobalSignId, string>();
}
