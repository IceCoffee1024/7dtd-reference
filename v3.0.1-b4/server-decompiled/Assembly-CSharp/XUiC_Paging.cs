using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E7C RID: 3708
[Preserve]
public class XUiC_Paging : XUiController
{
	// Token: 0x17000D6A RID: 3434
	// (get) Token: 0x06007356 RID: 29526 RVA: 0x002BB72E File Offset: 0x002B992E
	// (set) Token: 0x06007357 RID: 29527 RVA: 0x002BB736 File Offset: 0x002B9936
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

	// Token: 0x17000D6B RID: 3435
	// (get) Token: 0x06007358 RID: 29528 RVA: 0x002BB75F File Offset: 0x002B995F
	[XuiXmlBinding("pagenumber")]
	public int CurrentPageNumberNatural
	{
		get
		{
			return this.CurrentPageNumber + 1;
		}
	}

	// Token: 0x17000D6C RID: 3436
	// (get) Token: 0x06007359 RID: 29529 RVA: 0x002BB769 File Offset: 0x002B9969
	// (set) Token: 0x0600735A RID: 29530 RVA: 0x002BB774 File Offset: 0x002B9974
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

	// Token: 0x17000D6D RID: 3437
	// (get) Token: 0x0600735B RID: 29531 RVA: 0x002BB7C3 File Offset: 0x002B99C3
	[XuiXmlBinding("maxpagenumber")]
	public int LastPageNumberNatural
	{
		get
		{
			return this.LastPageNumber + 1;
		}
	}

	// Token: 0x17000D6E RID: 3438
	// (get) Token: 0x0600735C RID: 29532 RVA: 0x002BB7CD File Offset: 0x002B99CD
	// (set) Token: 0x0600735D RID: 29533 RVA: 0x002BB7D5 File Offset: 0x002B99D5
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

	// Token: 0x17000D6F RID: 3439
	// (get) Token: 0x0600735E RID: 29534 RVA: 0x002BB7EF File Offset: 0x002B99EF
	// (set) Token: 0x0600735F RID: 29535 RVA: 0x002BB7F7 File Offset: 0x002B99F7
	[XuiXmlAttribute("show_max_page", false)]
	[XuiXmlBinding("showmaxpage")]
	public bool ShowMaxPage { get; set; }

	// Token: 0x17000D70 RID: 3440
	// (get) Token: 0x06007360 RID: 29536 RVA: 0x002BB800 File Offset: 0x002B9A00
	// (set) Token: 0x06007361 RID: 29537 RVA: 0x002BB808 File Offset: 0x002B9A08
	[XuiXmlAttribute("separator", false)]
	[XuiXmlBinding("separator")]
	public string Separator { get; set; } = "/";

	// Token: 0x17000D71 RID: 3441
	// (get) Token: 0x06007362 RID: 29538 RVA: 0x002BB811 File Offset: 0x002B9A11
	// (set) Token: 0x06007363 RID: 29539 RVA: 0x002BB819 File Offset: 0x002B9A19
	[XuiXmlAttribute("contents_parent", false)]
	public string ContentParentName { get; set; }

	// Token: 0x17000D72 RID: 3442
	// (get) Token: 0x06007364 RID: 29540 RVA: 0x002BB822 File Offset: 0x002B9A22
	// (set) Token: 0x06007365 RID: 29541 RVA: 0x002BB82A File Offset: 0x002B9A2A
	[XuiXmlAttribute("hotkeys_enabled", false)]
	public bool HotkeysEnabled { get; set; } = true;

	// Token: 0x140000B3 RID: 179
	// (add) Token: 0x06007366 RID: 29542 RVA: 0x002BB834 File Offset: 0x002B9A34
	// (remove) Token: 0x06007367 RID: 29543 RVA: 0x002BB86C File Offset: 0x002B9A6C
	public event XUiEvent_PageChangedEventHandler OnPageChanged;

	// Token: 0x06007368 RID: 29544 RVA: 0x002BB8A4 File Offset: 0x002B9AA4
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

	// Token: 0x06007369 RID: 29545 RVA: 0x002BB98C File Offset: 0x002B9B8C
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

	// Token: 0x0600736A RID: 29546 RVA: 0x002BB9CC File Offset: 0x002B9BCC
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

	// Token: 0x0600736B RID: 29547 RVA: 0x002BBA06 File Offset: 0x002B9C06
	public int GetPage()
	{
		return this.CurrentPageNumber;
	}

	// Token: 0x0600736C RID: 29548 RVA: 0x002BBA0E File Offset: 0x002B9C0E
	public void SetPage(int _page)
	{
		this.CurrentPageNumber = _page;
	}

	// Token: 0x0600736D RID: 29549 RVA: 0x002BBA17 File Offset: 0x002B9C17
	public int GetLastPage()
	{
		return this.LastPageNumber;
	}

	// Token: 0x0600736E RID: 29550 RVA: 0x002BBA1F File Offset: 0x002B9C1F
	public void SetLastPageByElementsAndPageLength(int _elementCount, int _pageLength)
	{
		this.LastPageNumber = Math.Max(0, Mathf.CeilToInt((float)_elementCount / (float)_pageLength) - 1);
	}

	// Token: 0x0600736F RID: 29551 RVA: 0x002BBA39 File Offset: 0x002B9C39
	public void Reset()
	{
		this.CurrentPageNumber = 0;
	}

	// Token: 0x06007370 RID: 29552 RVA: 0x002BBA42 File Offset: 0x002B9C42
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

	// Token: 0x06007371 RID: 29553 RVA: 0x002BBA80 File Offset: 0x002B9C80
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

	// Token: 0x040055C1 RID: 21953
	public static bool IsOverPagingOverrideElement;

	// Token: 0x040055C2 RID: 21954
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentPageNumber;

	// Token: 0x040055C3 RID: 21955
	[PublicizedFrom(EAccessModifier.Private)]
	public bool controllerOverContentArea;

	// Token: 0x040055C9 RID: 21961
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> handlePageDownAction;

	// Token: 0x040055CA RID: 21962
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> handlePageUpAction;

	// Token: 0x040055CB RID: 21963
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastPageNumber;

	// Token: 0x040055CC RID: 21964
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController contentsParent;

	// Token: 0x040055CD RID: 21965
	[XuiBindComponent("pageUp", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController btnPageUp;

	// Token: 0x040055CE RID: 21966
	[XuiBindComponent("pageDown", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController btnPageDown;
}
