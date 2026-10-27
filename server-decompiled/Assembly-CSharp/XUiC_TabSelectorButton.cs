using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EB6 RID: 3766
[Preserve]
public class XUiC_TabSelectorButton : XUiController
{
	// Token: 0x17000DBB RID: 3515
	// (get) Token: 0x06007482 RID: 29826 RVA: 0x002C49D2 File Offset: 0x002C2BD2
	// (set) Token: 0x06007483 RID: 29827 RVA: 0x002C49DC File Offset: 0x002C2BDC
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

	// Token: 0x17000DBC RID: 3516
	// (get) Token: 0x06007484 RID: 29828 RVA: 0x002C4A48 File Offset: 0x002C2C48
	[XuiXmlBinding("tab_selected")]
	public bool TabSelected
	{
		get
		{
			XUiC_TabSelectorTab xuiC_TabSelectorTab = this.Tab;
			return xuiC_TabSelectorTab != null && xuiC_TabSelectorTab.TabSelected;
		}
	}

	// Token: 0x17000DBD RID: 3517
	// (get) Token: 0x06007485 RID: 29829 RVA: 0x002C4A5B File Offset: 0x002C2C5B
	[XuiXmlBinding("tab_name")]
	public string TabName
	{
		get
		{
			XUiC_TabSelectorTab xuiC_TabSelectorTab = this.Tab;
			return ((xuiC_TabSelectorTab != null) ? xuiC_TabSelectorTab.TabKey : null) ?? "";
		}
	}

	// Token: 0x17000DBE RID: 3518
	// (get) Token: 0x06007486 RID: 29830 RVA: 0x002C4A78 File Offset: 0x002C2C78
	[XuiXmlBinding("tab_name_localized")]
	public string TabNameLocalized
	{
		get
		{
			XUiC_TabSelectorTab xuiC_TabSelectorTab = this.Tab;
			return ((xuiC_TabSelectorTab != null) ? xuiC_TabSelectorTab.TabHeaderText : null) ?? "";
		}
	}

	// Token: 0x17000DBF RID: 3519
	// (get) Token: 0x06007487 RID: 29831 RVA: 0x002C4A95 File Offset: 0x002C2C95
	[XuiXmlBinding("tab_sprite")]
	public string TabSprite
	{
		get
		{
			XUiC_TabSelectorTab xuiC_TabSelectorTab = this.Tab;
			return ((xuiC_TabSelectorTab != null) ? xuiC_TabSelectorTab.TabHeaderSprite : null) ?? "";
		}
	}

	// Token: 0x17000DC0 RID: 3520
	// (get) Token: 0x06007488 RID: 29832 RVA: 0x002C4AB2 File Offset: 0x002C2CB2
	[XuiXmlBinding("tab_highlight")]
	public bool TabHighlight
	{
		get
		{
			XUiC_TabSelectorTab xuiC_TabSelectorTab = this.Tab;
			return xuiC_TabSelectorTab != null && xuiC_TabSelectorTab.TabHighlight;
		}
	}

	// Token: 0x17000DC1 RID: 3521
	// (get) Token: 0x06007489 RID: 29833 RVA: 0x002C4AC5 File Offset: 0x002C2CC5
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

	// Token: 0x0600748A RID: 29834 RVA: 0x002C4AE4 File Offset: 0x002C2CE4
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

	// Token: 0x0600748B RID: 29835 RVA: 0x002C4B84 File Offset: 0x002C2D84
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

	// Token: 0x0600748C RID: 29836 RVA: 0x002926AB File Offset: 0x002908AB
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x0600748D RID: 29837 RVA: 0x0008B76F File Offset: 0x0008996F
	[PublicizedFrom(EAccessModifier.Private)]
	public void tabCustomAttributesListener(object _sender, DictionaryChangedEventArgs<string, object> _e)
	{
		base.RefreshBindings();
	}

	// Token: 0x0600748E RID: 29838 RVA: 0x002C4BF8 File Offset: 0x002C2DF8
	public void UpdateSelectionState()
	{
		if (this.simpleButton != null)
		{
			this.simpleButton.Button.Selected = this.Tab.TabSelected;
		}
		base.RefreshBindings();
	}

	// Token: 0x0600748F RID: 29839 RVA: 0x002C4C23 File Offset: 0x002C2E23
	public void UpdateVisibilityState()
	{
		base.ViewComponent.IsVisible = this.Tab.TabVisible;
	}

	// Token: 0x06007490 RID: 29840 RVA: 0x002C4C3B File Offset: 0x002C2E3B
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

	// Token: 0x040056F0 RID: 22256
	[XuiBindParent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TabSelector parentSelector;

	// Token: 0x040056F1 RID: 22257
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TabSelectorTab tab;

	// Token: 0x040056F2 RID: 22258
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SimpleButton simpleButton;

	// Token: 0x040056F3 RID: 22259
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView interactionView;

	// Token: 0x040056F4 RID: 22260
	public Transform OuterDimensionsTransform;
}
