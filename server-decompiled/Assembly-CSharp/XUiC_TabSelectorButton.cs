using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EAB RID: 3755
[Preserve]
public class XUiC_TabSelectorButton : XUiController
{
	// Token: 0x17000DB5 RID: 3509
	// (get) Token: 0x0600745D RID: 29789 RVA: 0x002C3466 File Offset: 0x002C1666
	// (set) Token: 0x0600745E RID: 29790 RVA: 0x002C3470 File Offset: 0x002C1670
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

	// Token: 0x17000DB6 RID: 3510
	// (get) Token: 0x0600745F RID: 29791 RVA: 0x002C34DC File Offset: 0x002C16DC
	[XuiXmlBinding("tab_selected")]
	public bool TabSelected
	{
		get
		{
			XUiC_TabSelectorTab xuiC_TabSelectorTab = this.Tab;
			return xuiC_TabSelectorTab != null && xuiC_TabSelectorTab.TabSelected;
		}
	}

	// Token: 0x17000DB7 RID: 3511
	// (get) Token: 0x06007460 RID: 29792 RVA: 0x002C34EF File Offset: 0x002C16EF
	[XuiXmlBinding("tab_name")]
	public string TabName
	{
		get
		{
			XUiC_TabSelectorTab xuiC_TabSelectorTab = this.Tab;
			return ((xuiC_TabSelectorTab != null) ? xuiC_TabSelectorTab.TabKey : null) ?? "";
		}
	}

	// Token: 0x17000DB8 RID: 3512
	// (get) Token: 0x06007461 RID: 29793 RVA: 0x002C350C File Offset: 0x002C170C
	[XuiXmlBinding("tab_name_localized")]
	public string TabNameLocalized
	{
		get
		{
			XUiC_TabSelectorTab xuiC_TabSelectorTab = this.Tab;
			return ((xuiC_TabSelectorTab != null) ? xuiC_TabSelectorTab.TabHeaderText : null) ?? "";
		}
	}

	// Token: 0x17000DB9 RID: 3513
	// (get) Token: 0x06007462 RID: 29794 RVA: 0x002C3529 File Offset: 0x002C1729
	[XuiXmlBinding("tab_sprite")]
	public string TabSprite
	{
		get
		{
			XUiC_TabSelectorTab xuiC_TabSelectorTab = this.Tab;
			return ((xuiC_TabSelectorTab != null) ? xuiC_TabSelectorTab.TabHeaderSprite : null) ?? "";
		}
	}

	// Token: 0x17000DBA RID: 3514
	// (get) Token: 0x06007463 RID: 29795 RVA: 0x002C3546 File Offset: 0x002C1746
	[XuiXmlBinding("tab_highlight")]
	public bool TabHighlight
	{
		get
		{
			XUiC_TabSelectorTab xuiC_TabSelectorTab = this.Tab;
			return xuiC_TabSelectorTab != null && xuiC_TabSelectorTab.TabHighlight;
		}
	}

	// Token: 0x17000DBB RID: 3515
	// (get) Token: 0x06007464 RID: 29796 RVA: 0x002C3559 File Offset: 0x002C1759
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

	// Token: 0x06007465 RID: 29797 RVA: 0x002C3578 File Offset: 0x002C1778
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

	// Token: 0x06007466 RID: 29798 RVA: 0x002C3618 File Offset: 0x002C1818
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

	// Token: 0x06007467 RID: 29799 RVA: 0x00291183 File Offset: 0x0028F383
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06007468 RID: 29800 RVA: 0x0008B53B File Offset: 0x0008973B
	[PublicizedFrom(EAccessModifier.Private)]
	public void tabCustomAttributesListener(object _sender, DictionaryChangedEventArgs<string, object> _e)
	{
		base.RefreshBindings();
	}

	// Token: 0x06007469 RID: 29801 RVA: 0x002C368C File Offset: 0x002C188C
	public void UpdateSelectionState()
	{
		if (this.simpleButton != null)
		{
			this.simpleButton.Button.Selected = this.Tab.TabSelected;
		}
		base.RefreshBindings();
	}

	// Token: 0x0600746A RID: 29802 RVA: 0x002C36B7 File Offset: 0x002C18B7
	public void UpdateVisibilityState()
	{
		base.ViewComponent.IsVisible = this.Tab.TabVisible;
	}

	// Token: 0x0600746B RID: 29803 RVA: 0x002C36CF File Offset: 0x002C18CF
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

	// Token: 0x040056CB RID: 22219
	[XuiBindParent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TabSelector parentSelector;

	// Token: 0x040056CC RID: 22220
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TabSelectorTab tab;

	// Token: 0x040056CD RID: 22221
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SimpleButton simpleButton;

	// Token: 0x040056CE RID: 22222
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView interactionView;

	// Token: 0x040056CF RID: 22223
	public Transform OuterDimensionsTransform;
}
