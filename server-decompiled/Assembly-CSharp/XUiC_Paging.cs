using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E91 RID: 3729
[Preserve]
public class XUiC_Paging : XUiController
{
	// Token: 0x17000D83 RID: 3459
	// (get) Token: 0x060073F6 RID: 29686 RVA: 0x002BEC5A File Offset: 0x002BCE5A
	// (set) Token: 0x060073F7 RID: 29687 RVA: 0x002BEC62 File Offset: 0x002BCE62
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

	// Token: 0x17000D84 RID: 3460
	// (get) Token: 0x060073F8 RID: 29688 RVA: 0x002BEC8B File Offset: 0x002BCE8B
	[XuiXmlBinding("pagenumber")]
	public int CurrentPageNumberNatural
	{
		get
		{
			return this.CurrentPageNumber + 1;
		}
	}

	// Token: 0x17000D85 RID: 3461
	// (get) Token: 0x060073F9 RID: 29689 RVA: 0x002BEC95 File Offset: 0x002BCE95
	// (set) Token: 0x060073FA RID: 29690 RVA: 0x002BECA0 File Offset: 0x002BCEA0
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

	// Token: 0x17000D86 RID: 3462
	// (get) Token: 0x060073FB RID: 29691 RVA: 0x002BECEF File Offset: 0x002BCEEF
	[XuiXmlBinding("maxpagenumber")]
	public int LastPageNumberNatural
	{
		get
		{
			return this.LastPageNumber + 1;
		}
	}

	// Token: 0x17000D87 RID: 3463
	// (get) Token: 0x060073FC RID: 29692 RVA: 0x002BECF9 File Offset: 0x002BCEF9
	// (set) Token: 0x060073FD RID: 29693 RVA: 0x002BED01 File Offset: 0x002BCF01
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

	// Token: 0x17000D88 RID: 3464
	// (get) Token: 0x060073FE RID: 29694 RVA: 0x002BED1B File Offset: 0x002BCF1B
	// (set) Token: 0x060073FF RID: 29695 RVA: 0x002BED23 File Offset: 0x002BCF23
	[XuiXmlAttribute("show_max_page", false)]
	[XuiXmlBinding("showmaxpage")]
	public bool ShowMaxPage { get; set; }

	// Token: 0x17000D89 RID: 3465
	// (get) Token: 0x06007400 RID: 29696 RVA: 0x002BED2C File Offset: 0x002BCF2C
	// (set) Token: 0x06007401 RID: 29697 RVA: 0x002BED34 File Offset: 0x002BCF34
	[XuiXmlAttribute("separator", false)]
	[XuiXmlBinding("separator")]
	public string Separator { get; set; } = "/";

	// Token: 0x17000D8A RID: 3466
	// (get) Token: 0x06007402 RID: 29698 RVA: 0x002BED3D File Offset: 0x002BCF3D
	// (set) Token: 0x06007403 RID: 29699 RVA: 0x002BED45 File Offset: 0x002BCF45
	[XuiXmlAttribute("contents_parent", false)]
	public string ContentParentName { get; set; }

	// Token: 0x17000D8B RID: 3467
	// (get) Token: 0x06007404 RID: 29700 RVA: 0x002BED4E File Offset: 0x002BCF4E
	// (set) Token: 0x06007405 RID: 29701 RVA: 0x002BED56 File Offset: 0x002BCF56
	[XuiXmlAttribute("hotkeys_enabled", false)]
	public bool HotkeysEnabled { get; set; } = true;

	// Token: 0x140000B4 RID: 180
	// (add) Token: 0x06007406 RID: 29702 RVA: 0x002BED60 File Offset: 0x002BCF60
	// (remove) Token: 0x06007407 RID: 29703 RVA: 0x002BED98 File Offset: 0x002BCF98
	public event XUiEvent_PageChangedEventHandler OnPageChanged;

	// Token: 0x06007408 RID: 29704 RVA: 0x002BEDD0 File Offset: 0x002BCFD0
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

	// Token: 0x06007409 RID: 29705 RVA: 0x002BEEB8 File Offset: 0x002BD0B8
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

	// Token: 0x0600740A RID: 29706 RVA: 0x002BEEF8 File Offset: 0x002BD0F8
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

	// Token: 0x0600740B RID: 29707 RVA: 0x002BEF32 File Offset: 0x002BD132
	public int GetPage()
	{
		return this.CurrentPageNumber;
	}

	// Token: 0x0600740C RID: 29708 RVA: 0x002BEF3A File Offset: 0x002BD13A
	public void SetPage(int _page)
	{
		this.CurrentPageNumber = _page;
	}

	// Token: 0x0600740D RID: 29709 RVA: 0x002BEF43 File Offset: 0x002BD143
	public int GetLastPage()
	{
		return this.LastPageNumber;
	}

	// Token: 0x0600740E RID: 29710 RVA: 0x002BEF4B File Offset: 0x002BD14B
	public void SetLastPageByElementsAndPageLength(int _elementCount, int _pageLength)
	{
		this.LastPageNumber = Math.Max(0, Mathf.CeilToInt((float)_elementCount / (float)_pageLength) - 1);
	}

	// Token: 0x0600740F RID: 29711 RVA: 0x002BEF65 File Offset: 0x002BD165
	public void Reset()
	{
		this.CurrentPageNumber = 0;
	}

	// Token: 0x06007410 RID: 29712 RVA: 0x002BEF6E File Offset: 0x002BD16E
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

	// Token: 0x06007411 RID: 29713 RVA: 0x002BEFAC File Offset: 0x002BD1AC
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

	// Token: 0x04005665 RID: 22117
	public static bool IsOverPagingOverrideElement;

	// Token: 0x04005666 RID: 22118
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentPageNumber;

	// Token: 0x04005667 RID: 22119
	[PublicizedFrom(EAccessModifier.Private)]
	public bool controllerOverContentArea;

	// Token: 0x0400566D RID: 22125
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> handlePageDownAction;

	// Token: 0x0400566E RID: 22126
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> handlePageUpAction;

	// Token: 0x0400566F RID: 22127
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastPageNumber;

	// Token: 0x04005670 RID: 22128
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController contentsParent;

	// Token: 0x04005671 RID: 22129
	[XuiBindComponent("pageUp", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController btnPageUp;

	// Token: 0x04005672 RID: 22130
	[XuiBindComponent("pageDown", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController btnPageDown;
}
