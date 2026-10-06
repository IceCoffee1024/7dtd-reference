using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EA0 RID: 3744
[Preserve]
public class XUiC_Paging : XUiController
{
	// Token: 0x17000D9E RID: 3486
	// (get) Token: 0x060073EE RID: 29678 RVA: 0x002C200A File Offset: 0x002C020A
	// (set) Token: 0x060073EF RID: 29679 RVA: 0x002C2012 File Offset: 0x002C0212
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

	// Token: 0x17000D9F RID: 3487
	// (get) Token: 0x060073F0 RID: 29680 RVA: 0x002C203B File Offset: 0x002C023B
	[XuiXmlBinding("pagenumber")]
	public int CurrentPageNumberNatural
	{
		get
		{
			return this.CurrentPageNumber + 1;
		}
	}

	// Token: 0x17000DA0 RID: 3488
	// (get) Token: 0x060073F1 RID: 29681 RVA: 0x002C2045 File Offset: 0x002C0245
	// (set) Token: 0x060073F2 RID: 29682 RVA: 0x002C2050 File Offset: 0x002C0250
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

	// Token: 0x17000DA1 RID: 3489
	// (get) Token: 0x060073F3 RID: 29683 RVA: 0x002C209F File Offset: 0x002C029F
	[XuiXmlBinding("maxpagenumber")]
	public int LastPageNumberNatural
	{
		get
		{
			return this.LastPageNumber + 1;
		}
	}

	// Token: 0x17000DA2 RID: 3490
	// (get) Token: 0x060073F4 RID: 29684 RVA: 0x002C20A9 File Offset: 0x002C02A9
	// (set) Token: 0x060073F5 RID: 29685 RVA: 0x002C20B1 File Offset: 0x002C02B1
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

	// Token: 0x17000DA3 RID: 3491
	// (get) Token: 0x060073F6 RID: 29686 RVA: 0x002C20CB File Offset: 0x002C02CB
	// (set) Token: 0x060073F7 RID: 29687 RVA: 0x002C20D3 File Offset: 0x002C02D3
	[XuiXmlAttribute("show_max_page", false)]
	[XuiXmlBinding("showmaxpage")]
	public bool ShowMaxPage { get; set; }

	// Token: 0x17000DA4 RID: 3492
	// (get) Token: 0x060073F8 RID: 29688 RVA: 0x002C20DC File Offset: 0x002C02DC
	// (set) Token: 0x060073F9 RID: 29689 RVA: 0x002C20E4 File Offset: 0x002C02E4
	[XuiXmlAttribute("separator", false)]
	[XuiXmlBinding("separator")]
	public string Separator { get; set; } = "/";

	// Token: 0x17000DA5 RID: 3493
	// (get) Token: 0x060073FA RID: 29690 RVA: 0x002C20ED File Offset: 0x002C02ED
	// (set) Token: 0x060073FB RID: 29691 RVA: 0x002C20F5 File Offset: 0x002C02F5
	[XuiXmlAttribute("contents_parent", false)]
	public string ContentParentName { get; set; }

	// Token: 0x17000DA6 RID: 3494
	// (get) Token: 0x060073FC RID: 29692 RVA: 0x002C20FE File Offset: 0x002C02FE
	// (set) Token: 0x060073FD RID: 29693 RVA: 0x002C2106 File Offset: 0x002C0306
	[XuiXmlAttribute("hotkeys_enabled", false)]
	public bool HotkeysEnabled { get; set; } = true;

	// Token: 0x140000B3 RID: 179
	// (add) Token: 0x060073FE RID: 29694 RVA: 0x002C2110 File Offset: 0x002C0310
	// (remove) Token: 0x060073FF RID: 29695 RVA: 0x002C2148 File Offset: 0x002C0348
	public event XUiEvent_PageChangedEventHandler OnPageChanged;

	// Token: 0x06007400 RID: 29696 RVA: 0x002C2180 File Offset: 0x002C0380
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

	// Token: 0x06007401 RID: 29697 RVA: 0x002C2268 File Offset: 0x002C0468
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

	// Token: 0x06007402 RID: 29698 RVA: 0x002C22A8 File Offset: 0x002C04A8
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

	// Token: 0x06007403 RID: 29699 RVA: 0x002C22E2 File Offset: 0x002C04E2
	public int GetPage()
	{
		return this.CurrentPageNumber;
	}

	// Token: 0x06007404 RID: 29700 RVA: 0x002C22EA File Offset: 0x002C04EA
	public void SetPage(int _page)
	{
		this.CurrentPageNumber = _page;
	}

	// Token: 0x06007405 RID: 29701 RVA: 0x002C22F3 File Offset: 0x002C04F3
	public int GetLastPage()
	{
		return this.LastPageNumber;
	}

	// Token: 0x06007406 RID: 29702 RVA: 0x002C22FB File Offset: 0x002C04FB
	public void SetLastPageByElementsAndPageLength(int _elementCount, int _pageLength)
	{
		this.LastPageNumber = Math.Max(0, Mathf.CeilToInt((float)_elementCount / (float)_pageLength) - 1);
	}

	// Token: 0x06007407 RID: 29703 RVA: 0x002C2315 File Offset: 0x002C0515
	public void Reset()
	{
		this.CurrentPageNumber = 0;
	}

	// Token: 0x06007408 RID: 29704 RVA: 0x002C231E File Offset: 0x002C051E
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

	// Token: 0x06007409 RID: 29705 RVA: 0x002C235C File Offset: 0x002C055C
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

	// Token: 0x0400569C RID: 22172
	public static bool IsOverPagingOverrideElement;

	// Token: 0x0400569D RID: 22173
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentPageNumber;

	// Token: 0x0400569E RID: 22174
	[PublicizedFrom(EAccessModifier.Private)]
	public bool controllerOverContentArea;

	// Token: 0x040056A4 RID: 22180
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> handlePageDownAction;

	// Token: 0x040056A5 RID: 22181
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> handlePageUpAction;

	// Token: 0x040056A6 RID: 22182
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastPageNumber;

	// Token: 0x040056A7 RID: 22183
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController contentsParent;

	// Token: 0x040056A8 RID: 22184
	[XuiBindComponent("pageUp", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController btnPageUp;

	// Token: 0x040056A9 RID: 22185
	[XuiBindComponent("pageDown", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController btnPageDown;
}
