using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E87 RID: 3719
[Preserve]
public class XUiC_TabSelectorButton : XUiController
{
	// Token: 0x17000D81 RID: 3457
	// (get) Token: 0x060073C5 RID: 29637 RVA: 0x002BCB83 File Offset: 0x002BAD83
	// (set) Token: 0x060073C6 RID: 29638 RVA: 0x002BCB8C File Offset: 0x002BAD8C
	public XUiC_TabSelectorTab Tab
	{
		get
		{
			return this.tab;
		}
		set
		{
			if (this.tab == value)
			{
				return;
			}
			if (this.tab != null)
			{
				this.tab.CustomAttributes.EntryModified -= this.tabCustomAttributesListener;
			}
			this.tab = value;
			if (this.tab != null)
			{
				this.tab.CustomAttributes.EntryModified += this.tabCustomAttributesListener;
			}
			base.RefreshBindings();
		}
	}

	// Token: 0x17000D82 RID: 3458
	// (get) Token: 0x060073C7 RID: 29639 RVA: 0x002BCBF8 File Offset: 0x002BADF8
	[XuiXmlBinding("tab_selected")]
	public bool TabSelected
	{
		get
		{
			XUiC_TabSelectorTab xuiC_TabSelectorTab = this.Tab;
			return xuiC_TabSelectorTab != null && xuiC_TabSelectorTab.TabSelected;
		}
	}

	// Token: 0x17000D83 RID: 3459
	// (get) Token: 0x060073C8 RID: 29640 RVA: 0x002BCC0B File Offset: 0x002BAE0B
	[XuiXmlBinding("tab_name")]
	public string TabName
	{
		get
		{
			XUiC_TabSelectorTab xuiC_TabSelectorTab = this.Tab;
			return ((xuiC_TabSelectorTab != null) ? xuiC_TabSelectorTab.TabKey : null) ?? "";
		}
	}

	// Token: 0x17000D84 RID: 3460
	// (get) Token: 0x060073C9 RID: 29641 RVA: 0x002BCC28 File Offset: 0x002BAE28
	[XuiXmlBinding("tab_name_localized")]
	public string TabNameLocalized
	{
		get
		{
			XUiC_TabSelectorTab xuiC_TabSelectorTab = this.Tab;
			return ((xuiC_TabSelectorTab != null) ? xuiC_TabSelectorTab.TabHeaderText : null) ?? "";
		}
	}

	// Token: 0x17000D85 RID: 3461
	// (get) Token: 0x060073CA RID: 29642 RVA: 0x002BCC45 File Offset: 0x002BAE45
	[XuiXmlBinding("tab_sprite")]
	public string TabSprite
	{
		get
		{
			XUiC_TabSelectorTab xuiC_TabSelectorTab = this.Tab;
			return ((xuiC_TabSelectorTab != null) ? xuiC_TabSelectorTab.TabHeaderSprite : null) ?? "";
		}
	}

	// Token: 0x17000D86 RID: 3462
	// (get) Token: 0x060073CB RID: 29643 RVA: 0x002BCC62 File Offset: 0x002BAE62
	[XuiXmlBinding("tab_highlight")]
	public bool TabHighlight
	{
		get
		{
			XUiC_TabSelectorTab xuiC_TabSelectorTab = this.Tab;
			return xuiC_TabSelectorTab != null && xuiC_TabSelectorTab.TabHighlight;
		}
	}

	// Token: 0x17000D87 RID: 3463
	// (get) Token: 0x060073CC RID: 29644 RVA: 0x002BCC75 File Offset: 0x002BAE75
	[XuiXmlBinding("tab_custom_attributes")]
	public ObservableDictionary<string, object> TabCustomAttributes
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_TabSelectorTab xuiC_TabSelectorTab = this.Tab;
			return ((xuiC_TabSelectorTab != null) ? xuiC_TabSelectorTab.CustomAttributes : null) ?? this.CustomAttributes;
		}
	}

	// Token: 0x060073CD RID: 29645 RVA: 0x002BCC94 File Offset: 0x002BAE94
	public override void Init()
	{
		base.Init();
		this.OuterDimensionsTransform = base.ViewComponent.UiTransform.FindRecursive("border");
		if (this.simpleButton != null)
		{
			this.simpleButton.OnPressed += delegate(XUiController _, int _)
			{
				this.parentSelector.TabButtonClicked(this);
			};
			return;
		}
		this.interactionView = this.findClickableChild(this);
		if (this.interactionView == null)
		{
			Log.Error("[XUi] TabSelectorButton without SimpleButton or other view with 'on_press=true' in windowGroup '" + this.windowGroup.Id + "'");
			return;
		}
		this.interactionView.Controller.OnPress += delegate(XUiController _, int _)
		{
			this.parentSelector.TabButtonClicked(this);
		};
	}

	// Token: 0x060073CE RID: 29646 RVA: 0x002BCD34 File Offset: 0x002BAF34
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView findClickableChild(XUiController _controller)
	{
		if (_controller.ViewComponent.EventOnPress)
		{
			return _controller.ViewComponent;
		}
		foreach (XUiController xuiController in _controller.Children)
		{
			XUiView xuiView = this.findClickableChild(xuiController);
			if (xuiView != null)
			{
				return xuiView;
			}
		}
		return null;
	}

	// Token: 0x060073CF RID: 29647 RVA: 0x0028AD37 File Offset: 0x00288F37
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x060073D0 RID: 29648 RVA: 0x0008AB7B File Offset: 0x00088D7B
	[PublicizedFrom(EAccessModifier.Private)]
	public void tabCustomAttributesListener(object _sender, DictionaryChangedEventArgs<string, object> _e)
	{
		base.RefreshBindings();
	}

	// Token: 0x060073D1 RID: 29649 RVA: 0x002BCDA8 File Offset: 0x002BAFA8
	public void UpdateSelectionState()
	{
		if (this.simpleButton != null)
		{
			this.simpleButton.Button.Selected = this.Tab.TabSelected;
		}
		base.RefreshBindings();
	}

	// Token: 0x060073D2 RID: 29650 RVA: 0x002BCDD3 File Offset: 0x002BAFD3
	public void UpdateVisibilityState()
	{
		base.ViewComponent.IsVisible = this.Tab.TabVisible;
	}

	// Token: 0x060073D3 RID: 29651 RVA: 0x002BCDEB File Offset: 0x002BAFEB
	public void PlayClickSound()
	{
		if (this.simpleButton != null)
		{
			this.simpleButton.Button.PlayClickSound();
			return;
		}
		if (this.interactionView != null)
		{
			this.interactionView.PlayClickSound();
		}
	}

	// Token: 0x040055F0 RID: 22000
	[XuiBindParent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TabSelector parentSelector;

	// Token: 0x040055F1 RID: 22001
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TabSelectorTab tab;

	// Token: 0x040055F2 RID: 22002
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SimpleButton simpleButton;

	// Token: 0x040055F3 RID: 22003
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView interactionView;

	// Token: 0x040055F4 RID: 22004
	public Transform OuterDimensionsTransform;
}
