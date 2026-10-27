using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EAB RID: 3755
[Preserve]
public class XUiC_Paging : XUiController
{
	// Token: 0x17000DA4 RID: 3492
	// (get) Token: 0x06007413 RID: 29715 RVA: 0x002C3576 File Offset: 0x002C1776
	// (set) Token: 0x06007414 RID: 29716 RVA: 0x002C357E File Offset: 0x002C177E
	public int CurrentPageNumber
	{
		get
		{
			return this.currentPageNumber;
		}
		set
		{
			value = Mathf.Clamp(value, 0, this.LastPageNumber);
			if (value == this.currentPageNumber)
			{
				return;
			}
			this.currentPageNumber = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000DA5 RID: 3493
	// (get) Token: 0x06007415 RID: 29717 RVA: 0x002C35A7 File Offset: 0x002C17A7
	[XuiXmlBinding("pagenumber")]
	public int CurrentPageNumberNatural
	{
		get
		{
			return this.CurrentPageNumber + 1;
		}
	}

	// Token: 0x17000DA6 RID: 3494
	// (get) Token: 0x06007416 RID: 29718 RVA: 0x002C35B1 File Offset: 0x002C17B1
	// (set) Token: 0x06007417 RID: 29719 RVA: 0x002C35BC File Offset: 0x002C17BC
	public int LastPageNumber
	{
		get
		{
			return this.lastPageNumber;
		}
		set
		{
			if (value == this.lastPageNumber)
			{
				return;
			}
			this.lastPageNumber = value;
			this.IsDirty = true;
			if (this.CurrentPageNumber > this.lastPageNumber)
			{
				this.CurrentPageNumber = this.lastPageNumber;
				XUiEvent_PageChangedEventHandler onPageChanged = this.OnPageChanged;
				if (onPageChanged == null)
				{
					return;
				}
				onPageChanged();
			}
		}
	}

	// Token: 0x17000DA7 RID: 3495
	// (get) Token: 0x06007418 RID: 29720 RVA: 0x002C360B File Offset: 0x002C180B
	[XuiXmlBinding("maxpagenumber")]
	public int LastPageNumberNatural
	{
		get
		{
			return this.LastPageNumber + 1;
		}
	}

	// Token: 0x17000DA8 RID: 3496
	// (get) Token: 0x06007419 RID: 29721 RVA: 0x002C3615 File Offset: 0x002C1815
	// (set) Token: 0x0600741A RID: 29722 RVA: 0x002C361D File Offset: 0x002C181D
	[XuiXmlBinding("over_content_area")]
	public bool ControllerOverContentArea
	{
		get
		{
			return this.controllerOverContentArea;
		}
		set
		{
			if (this.controllerOverContentArea == value)
			{
				return;
			}
			this.controllerOverContentArea = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000DA9 RID: 3497
	// (get) Token: 0x0600741B RID: 29723 RVA: 0x002C3637 File Offset: 0x002C1837
	// (set) Token: 0x0600741C RID: 29724 RVA: 0x002C363F File Offset: 0x002C183F
	[XuiXmlAttribute("show_max_page", false)]
	[XuiXmlBinding("showmaxpage")]
	public bool ShowMaxPage { get; set; }

	// Token: 0x17000DAA RID: 3498
	// (get) Token: 0x0600741D RID: 29725 RVA: 0x002C3648 File Offset: 0x002C1848
	// (set) Token: 0x0600741E RID: 29726 RVA: 0x002C3650 File Offset: 0x002C1850
	[XuiXmlAttribute("separator", false)]
	[XuiXmlBinding("separator")]
	public string Separator { get; set; } = "/";

	// Token: 0x17000DAB RID: 3499
	// (get) Token: 0x0600741F RID: 29727 RVA: 0x002C3659 File Offset: 0x002C1859
	// (set) Token: 0x06007420 RID: 29728 RVA: 0x002C3661 File Offset: 0x002C1861
	[XuiXmlAttribute("contents_parent", false)]
	public string ContentParentName { get; set; }

	// Token: 0x17000DAC RID: 3500
	// (get) Token: 0x06007421 RID: 29729 RVA: 0x002C366A File Offset: 0x002C186A
	// (set) Token: 0x06007422 RID: 29730 RVA: 0x002C3672 File Offset: 0x002C1872
	[XuiXmlAttribute("hotkeys_enabled", false)]
	public bool HotkeysEnabled { get; set; } = true;

	// Token: 0x140000B3 RID: 179
	// (add) Token: 0x06007423 RID: 29731 RVA: 0x002C367C File Offset: 0x002C187C
	// (remove) Token: 0x06007424 RID: 29732 RVA: 0x002C36B4 File Offset: 0x002C18B4
	public event XUiEvent_PageChangedEventHandler OnPageChanged;

	// Token: 0x06007425 RID: 29733 RVA: 0x002C36EC File Offset: 0x002C18EC
	public override void Init()
	{
		base.Init();
		if (this.btnPageDown != null)
		{
			this.btnPageDown.OnPress += delegate(XUiController _, int _)
			{
				this.PageDown();
			};
		}
		if (this.btnPageUp != null)
		{
			this.btnPageUp.OnPress += delegate(XUiController _, int _)
			{
				this.PageUp();
			};
		}
		if (!string.IsNullOrEmpty(this.ContentParentName))
		{
			XUiController xuiController = this;
			while (!xuiController.TryGetChildController<XUiController>(this.ContentParentName, out this.contentsParent))
			{
				xuiController = xuiController.Parent;
				if (xuiController == null)
				{
					break;
				}
			}
			if (this.contentsParent == null)
			{
				Log.Warning("[XUi] Could not find contents parent '" + this.ContentParentName + "' for pager. Hierarchy: " + base.GetXuiHierarchy());
			}
		}
		else
		{
			Log.Warning("[XUi] No contents parent set for pager. Hierarchy: " + base.GetXuiHierarchy());
		}
		this.handlePageDownAction = new Func<bool>(this.PageDown);
		this.handlePageUpAction = new Func<bool>(this.PageUp);
		this.CurrentPageNumber = 0;
	}

	// Token: 0x06007426 RID: 29734 RVA: 0x002C37D4 File Offset: 0x002C19D4
	public bool PageUp()
	{
		if (this.CurrentPageNumber >= this.LastPageNumber)
		{
			return false;
		}
		int num = this.CurrentPageNumber;
		this.CurrentPageNumber = num + 1;
		XUiEvent_PageChangedEventHandler onPageChanged = this.OnPageChanged;
		if (onPageChanged != null)
		{
			onPageChanged();
		}
		return true;
	}

	// Token: 0x06007427 RID: 29735 RVA: 0x002C3814 File Offset: 0x002C1A14
	public bool PageDown()
	{
		if (this.CurrentPageNumber <= 0)
		{
			return false;
		}
		int num = this.CurrentPageNumber;
		this.CurrentPageNumber = num - 1;
		XUiEvent_PageChangedEventHandler onPageChanged = this.OnPageChanged;
		if (onPageChanged != null)
		{
			onPageChanged();
		}
		return true;
	}

	// Token: 0x06007428 RID: 29736 RVA: 0x002C384E File Offset: 0x002C1A4E
	public int GetPage()
	{
		return this.CurrentPageNumber;
	}

	// Token: 0x06007429 RID: 29737 RVA: 0x002C3856 File Offset: 0x002C1A56
	public void SetPage(int _page)
	{
		this.CurrentPageNumber = _page;
	}

	// Token: 0x0600742A RID: 29738 RVA: 0x002C385F File Offset: 0x002C1A5F
	public int GetLastPage()
	{
		return this.LastPageNumber;
	}

	// Token: 0x0600742B RID: 29739 RVA: 0x002C3867 File Offset: 0x002C1A67
	public void SetLastPageByElementsAndPageLength(int _elementCount, int _pageLength)
	{
		this.LastPageNumber = Math.Max(0, Mathf.CeilToInt((float)_elementCount / (float)_pageLength) - 1);
	}

	// Token: 0x0600742C RID: 29740 RVA: 0x002C3881 File Offset: 0x002C1A81
	public void Reset()
	{
		this.CurrentPageNumber = 0;
	}

	// Token: 0x0600742D RID: 29741 RVA: 0x002C388A File Offset: 0x002C1A8A
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.ControllerOverContentArea = this.updateControllerState();
		if (this.ControllerOverContentArea)
		{
			XUi.HandlePaging(this.xui, this.handlePageUpAction, this.handlePageDownAction, false);
		}
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x0600742E RID: 29742 RVA: 0x002C38C8 File Offset: 0x002C1AC8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool updateControllerState()
	{
		IPlatform nativePlatform = PlatformManager.NativePlatform;
		bool flag;
		if (nativePlatform == null)
		{
			flag = false;
		}
		else
		{
			PlayerInputManager input = nativePlatform.Input;
			PlayerInputManager.InputStyle? inputStyle = ((input != null) ? new PlayerInputManager.InputStyle?(input.CurrentInputStyle) : null);
			PlayerInputManager.InputStyle inputStyle2 = PlayerInputManager.InputStyle.Keyboard;
			flag = (inputStyle.GetValueOrDefault() == inputStyle2) & (inputStyle != null);
		}
		return !flag && this.contentsParent != null && this.HotkeysEnabled && !XUiC_Paging.IsOverPagingOverrideElement && this.xui.playerUI.CursorController.CurrentTarget != null && this.xui.playerUI.CursorController.CurrentTarget.Controller.IsSelfOrChildOf(this.contentsParent);
	}

	// Token: 0x040056C1 RID: 22209
	public static bool IsOverPagingOverrideElement;

	// Token: 0x040056C2 RID: 22210
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentPageNumber;

	// Token: 0x040056C3 RID: 22211
	[PublicizedFrom(EAccessModifier.Private)]
	public bool controllerOverContentArea;

	// Token: 0x040056C9 RID: 22217
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> handlePageDownAction;

	// Token: 0x040056CA RID: 22218
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> handlePageUpAction;

	// Token: 0x040056CB RID: 22219
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastPageNumber;

	// Token: 0x040056CC RID: 22220
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController contentsParent;

	// Token: 0x040056CD RID: 22221
	[XuiBindComponent("pageUp", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController btnPageUp;

	// Token: 0x040056CE RID: 22222
	[XuiBindComponent("pageDown", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController btnPageDown;
}
