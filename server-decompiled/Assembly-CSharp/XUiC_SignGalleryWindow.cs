using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

// Token: 0x02001019 RID: 4121
[Preserve]
public class XUiC_SignGalleryWindow : XUiController
{
	// Token: 0x17000F12 RID: 3858
	// (get) Token: 0x0600825C RID: 33372 RVA: 0x0031B06C File Offset: 0x0031926C
	// (set) Token: 0x0600825D RID: 33373 RVA: 0x0031B074 File Offset: 0x00319274
	public SignCanvas targetCanvas
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F13 RID: 3859
	// (get) Token: 0x0600825E RID: 33374 RVA: 0x0031B07D File Offset: 0x0031927D
	// (set) Token: 0x0600825F RID: 33375 RVA: 0x0031B085 File Offset: 0x00319285
	public TEFeatureCanvas targetEntity
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F14 RID: 3860
	// (get) Token: 0x06008260 RID: 33376 RVA: 0x0031B08E File Offset: 0x0031928E
	// (set) Token: 0x06008261 RID: 33377 RVA: 0x0031B096 File Offset: 0x00319296
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

	// Token: 0x06008262 RID: 33378 RVA: 0x0031B0D0 File Offset: 0x003192D0
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

	// Token: 0x06008263 RID: 33379 RVA: 0x0031B30C File Offset: 0x0031950C
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

	// Token: 0x06008264 RID: 33380 RVA: 0x0031B3A4 File Offset: 0x003195A4
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

	// Token: 0x06008265 RID: 33381 RVA: 0x0031B416 File Offset: 0x00319616
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangedHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.Page = 0;
		this.RefreshItems(this.txtInput.Text);
		this.UpdateGrid(false);
	}

	// Token: 0x06008266 RID: 33382 RVA: 0x0031B437 File Offset: 0x00319637
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

	// Token: 0x06008267 RID: 33383 RVA: 0x0031B464 File Offset: 0x00319664
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

	// Token: 0x06008268 RID: 33384 RVA: 0x0031B5C4 File Offset: 0x003197C4
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

	// Token: 0x06008269 RID: 33385 RVA: 0x0031B668 File Offset: 0x00319868
	[PublicizedFrom(EAccessModifier.Private)]
	public void AutoPage()
	{
		int num = this.currentItems.IndexOf(this.currentSignId);
		if (num >= 0)
		{
			this.Page = num / this.signGrid.Length;
		}
	}

	// Token: 0x0600826A RID: 33386 RVA: 0x0031B6A0 File Offset: 0x003198A0
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

	// Token: 0x0600826B RID: 33387 RVA: 0x0031B72C File Offset: 0x0031992C
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

	// Token: 0x0600826C RID: 33388 RVA: 0x0031B7A2 File Offset: 0x003199A2
	public static void Open(LocalPlayerUI _playerUi, TEFeatureCanvas _feature)
	{
		XUiC_SignGalleryWindow childByType = _playerUi.xui.FindWindowGroupByName(XUiC_SignGalleryWindow.ID).GetChildByType<XUiC_SignGalleryWindow>();
		childByType.targetCanvas = ((_feature != null) ? _feature.Canvas : null);
		childByType.targetEntity = _feature;
		_playerUi.windowManager.Open(XUiC_SignGalleryWindow.ID, true);
	}

	// Token: 0x0600826D RID: 33389 RVA: 0x0031B7E4 File Offset: 0x003199E4
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

	// Token: 0x0600826E RID: 33390 RVA: 0x0031B83C File Offset: 0x00319A3C
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateGrid(bool forceRefreshInfoPanel)
	{
		this.signGrid.SetSignIds(this.currentItems, this.currentSignId);
		if (forceRefreshInfoPanel && !this.currentItems.Contains(this.currentSignId))
		{
			this.infoWindow.SetSignInfo(this.currentSignId);
		}
	}

	// Token: 0x0600826F RID: 33391 RVA: 0x0031B87C File Offset: 0x00319A7C
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshFilterButtons()
	{
		this.filterDefaultButton.Selected = this.includeDefaultLibrary;
		this.filterPrefabButton.Selected = this.includePrefabLibrary;
	}

	// Token: 0x06008270 RID: 33392 RVA: 0x0031B8A0 File Offset: 0x00319AA0
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnFilterDefault_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.includeDefaultLibrary = !this.includeDefaultLibrary;
		this.updateFilters = true;
		this.IsDirty = true;
	}

	// Token: 0x06008271 RID: 33393 RVA: 0x0031B8BF File Offset: 0x00319ABF
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnFilterPrefab_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.includePrefabLibrary = !this.includePrefabLibrary;
		this.updateFilters = true;
		this.IsDirty = true;
	}

	// Token: 0x06008276 RID: 33398 RVA: 0x0031B978 File Offset: 0x00319B78
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

	// Token: 0x040060A4 RID: 24740
	public static string ID = "";

	// Token: 0x040060A5 RID: 24741
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label resultCount;

	// Token: 0x040060A6 RID: 24742
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignStackGrid signGrid;

	// Token: 0x040060A7 RID: 24743
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x040060A8 RID: 24744
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x040060A9 RID: 24745
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x040060AA RID: 24746
	[PublicizedFrom(EAccessModifier.Private)]
	public bool updateFilters;

	// Token: 0x040060AB RID: 24747
	[PublicizedFrom(EAccessModifier.Private)]
	public bool includeDefaultLibrary = true;

	// Token: 0x040060AC RID: 24748
	[PublicizedFrom(EAccessModifier.Private)]
	public bool includePrefabLibrary = true;

	// Token: 0x040060AD RID: 24749
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x040060AE RID: 24750
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnNew;

	// Token: 0x040060AF RID: 24751
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblTotal;

	// Token: 0x040060B2 RID: 24754
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId currentSignId = GlobalSignId.InvalidId;

	// Token: 0x040060B3 RID: 24755
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignInfoWindow infoWindow;

	// Token: 0x040060B4 RID: 24756
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignInstanceWindow instanceWindow;

	// Token: 0x040060B5 RID: 24757
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Button filterDefaultButton;

	// Token: 0x040060B6 RID: 24758
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Button filterPrefabButton;

	// Token: 0x040060B7 RID: 24759
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<GlobalSignId> currentItems = new List<GlobalSignId>();

	// Token: 0x040060B8 RID: 24760
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<GlobalSignId, string> signNamesById = new Dictionary<GlobalSignId, string>();
}
