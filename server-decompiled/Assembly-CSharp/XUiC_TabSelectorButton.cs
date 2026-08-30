using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E9C RID: 3740
[Preserve]
public class XUiC_TabSelectorButton : XUiController
{
	// Token: 0x17000D9A RID: 3482
	// (get) Token: 0x06007465 RID: 29797 RVA: 0x002C00AF File Offset: 0x002BE2AF
	// (set) Token: 0x06007466 RID: 29798 RVA: 0x002C00B8 File Offset: 0x002BE2B8
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

	// Token: 0x17000D9B RID: 3483
	// (get) Token: 0x06007467 RID: 29799 RVA: 0x002C0124 File Offset: 0x002BE324
	[XuiXmlBinding("tab_selected")]
	public bool TabSelected
	{
		get
		{
			XUiC_TabSelectorTab xuiC_TabSelectorTab = this.Tab;
			return xuiC_TabSelectorTab != null && xuiC_TabSelectorTab.TabSelected;
		}
	}

	// Token: 0x17000D9C RID: 3484
	// (get) Token: 0x06007468 RID: 29800 RVA: 0x002C0137 File Offset: 0x002BE337
	[XuiXmlBinding("tab_name")]
	public string TabName
	{
		get
		{
			XUiC_TabSelectorTab xuiC_TabSelectorTab = this.Tab;
			return ((xuiC_TabSelectorTab != null) ? xuiC_TabSelectorTab.TabKey : null) ?? "";
		}
	}

	// Token: 0x17000D9D RID: 3485
	// (get) Token: 0x06007469 RID: 29801 RVA: 0x002C0154 File Offset: 0x002BE354
	[XuiXmlBinding("tab_name_localized")]
	public string TabNameLocalized
	{
		get
		{
			XUiC_TabSelectorTab xuiC_TabSelectorTab = this.Tab;
			return ((xuiC_TabSelectorTab != null) ? xuiC_TabSelectorTab.TabHeaderText : null) ?? "";
		}
	}

	// Token: 0x17000D9E RID: 3486
	// (get) Token: 0x0600746A RID: 29802 RVA: 0x002C0171 File Offset: 0x002BE371
	[XuiXmlBinding("tab_sprite")]
	public string TabSprite
	{
		get
		{
			XUiC_TabSelectorTab xuiC_TabSelectorTab = this.Tab;
			return ((xuiC_TabSelectorTab != null) ? xuiC_TabSelectorTab.TabHeaderSprite : null) ?? "";
		}
	}

	// Token: 0x17000D9F RID: 3487
	// (get) Token: 0x0600746B RID: 29803 RVA: 0x002C018E File Offset: 0x002BE38E
	[XuiXmlBinding("tab_highlight")]
	public bool TabHighlight
	{
		get
		{
			XUiC_TabSelectorTab xuiC_TabSelectorTab = this.Tab;
			return xuiC_TabSelectorTab != null && xuiC_TabSelectorTab.TabHighlight;
		}
	}

	// Token: 0x17000DA0 RID: 3488
	// (get) Token: 0x0600746C RID: 29804 RVA: 0x002C01A1 File Offset: 0x002BE3A1
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

	// Token: 0x0600746D RID: 29805 RVA: 0x002C01C0 File Offset: 0x002BE3C0
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

	// Token: 0x0600746E RID: 29806 RVA: 0x002C0260 File Offset: 0x002BE460
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

	// Token: 0x0600746F RID: 29807 RVA: 0x0028DC1B File Offset: 0x0028BE1B
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06007470 RID: 29808 RVA: 0x0008B2BB File Offset: 0x000894BB
	[PublicizedFrom(EAccessModifier.Private)]
	public void tabCustomAttributesListener(object _sender, DictionaryChangedEventArgs<string, object> _e)
	{
		base.RefreshBindings();
	}

	// Token: 0x06007471 RID: 29809 RVA: 0x002C02D4 File Offset: 0x002BE4D4
	public void UpdateSelectionState()
	{
		if (this.simpleButton != null)
		{
			this.simpleButton.Button.Selected = this.Tab.TabSelected;
		}
		base.RefreshBindings();
	}

	// Token: 0x06007472 RID: 29810 RVA: 0x002C02FF File Offset: 0x002BE4FF
	public void UpdateVisibilityState()
	{
		base.ViewComponent.IsVisible = this.Tab.TabVisible;
	}

	// Token: 0x06007473 RID: 29811 RVA: 0x002C0317 File Offset: 0x002BE517
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

	// Token: 0x04005694 RID: 22164
	[XuiBindParent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TabSelector parentSelector;

	// Token: 0x04005695 RID: 22165
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TabSelectorTab tab;

	// Token: 0x04005696 RID: 22166
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SimpleButton simpleButton;

	// Token: 0x04005697 RID: 22167
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView interactionView;

	// Token: 0x04005698 RID: 22168
	public Transform OuterDimensionsTransform;
}
