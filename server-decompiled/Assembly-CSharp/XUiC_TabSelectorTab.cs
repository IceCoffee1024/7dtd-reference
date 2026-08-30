using System;
using UnityEngine.Scripting;

// Token: 0x02000E9D RID: 3741
[Preserve]
public class XUiC_TabSelectorTab : XUiController
{
	// Token: 0x17000DA1 RID: 3489
	// (get) Token: 0x06007477 RID: 29815 RVA: 0x002C0353 File Offset: 0x002BE553
	// (set) Token: 0x06007478 RID: 29816 RVA: 0x002C035B File Offset: 0x002BE55B
	[XuiXmlAttribute("tab_key", false)]
	public string TabKey
	{
		get
		{
			return this.tabKey;
		}
		set
		{
			if (this.tabKey == value)
			{
				return;
			}
			this.tabKey = value;
			this.TabHeaderText = Localization.Get(this.tabKey, false, null);
			this.IsDirty = true;
		}
	}

	// Token: 0x17000DA2 RID: 3490
	// (get) Token: 0x06007479 RID: 29817 RVA: 0x002C038D File Offset: 0x002BE58D
	// (set) Token: 0x0600747A RID: 29818 RVA: 0x002C0395 File Offset: 0x002BE595
	[XuiXmlAttribute("tab_caption", false)]
	public string TabHeaderText
	{
		get
		{
			return this.tabHeaderText;
		}
		set
		{
			if (this.tabHeaderText == value)
			{
				return;
			}
			this.tabHeaderText = value;
			if (this.TabButton != null)
			{
				this.TabButton.IsDirty = true;
			}
			this.IsDirty = true;
		}
	}

	// Token: 0x17000DA3 RID: 3491
	// (get) Token: 0x0600747B RID: 29819 RVA: 0x002C03C8 File Offset: 0x002BE5C8
	// (set) Token: 0x0600747C RID: 29820 RVA: 0x002C03D0 File Offset: 0x002BE5D0
	[XuiXmlAttribute("tab_sprite", false)]
	public string TabHeaderSprite
	{
		get
		{
			return this.tabHeaderSprite;
		}
		set
		{
			if (this.tabHeaderSprite == value)
			{
				return;
			}
			this.tabHeaderSprite = value;
			if (this.TabButton != null)
			{
				this.TabButton.IsDirty = true;
			}
			this.IsDirty = true;
		}
	}

	// Token: 0x17000DA4 RID: 3492
	// (get) Token: 0x0600747D RID: 29821 RVA: 0x002C0403 File Offset: 0x002BE603
	// (set) Token: 0x0600747E RID: 29822 RVA: 0x002C040C File Offset: 0x002BE60C
	[XuiXmlAttribute("tab_visible", false)]
	public bool TabVisible
	{
		get
		{
			return this.tabVisible;
		}
		set
		{
			if (value == this.tabVisible)
			{
				return;
			}
			this.tabVisible = value;
			XUiC_TabSelectorButton tabButton = this.TabButton;
			if (tabButton != null)
			{
				tabButton.UpdateVisibilityState();
			}
			XUiC_TabSelector xuiC_TabSelector = this.parentSelector;
			if (xuiC_TabSelector != null)
			{
				xuiC_TabSelector.TabVisibilityChanged(this, this.tabVisible);
			}
			this.IsDirty = true;
		}
	}

	// Token: 0x17000DA5 RID: 3493
	// (get) Token: 0x0600747F RID: 29823 RVA: 0x002C045A File Offset: 0x002BE65A
	// (set) Token: 0x06007480 RID: 29824 RVA: 0x002C0464 File Offset: 0x002BE664
	[XuiXmlBinding("tab_selected")]
	public bool TabSelected
	{
		get
		{
			return this.tabSelected;
		}
		set
		{
			if (value == this.tabSelected)
			{
				return;
			}
			this.tabSelected = value;
			this.TabButton.UpdateSelectionState();
			base.ViewComponent.IsVisible = this.tabSelected;
			this.parentSelector.SelectedTab = this;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000DA6 RID: 3494
	// (get) Token: 0x06007481 RID: 29825 RVA: 0x002C04B1 File Offset: 0x002BE6B1
	// (set) Token: 0x06007482 RID: 29826 RVA: 0x002C04B9 File Offset: 0x002BE6B9
	[XuiXmlBinding("tab_highlight")]
	public bool TabHighlight
	{
		get
		{
			return this.tabHighlight;
		}
		set
		{
			if (value == this.tabHighlight)
			{
				return;
			}
			this.tabHighlight = value;
			this.TabButton.IsDirty = true;
			this.IsDirty = true;
		}
	}

	// Token: 0x06007483 RID: 29827 RVA: 0x002C04DF File Offset: 0x002BE6DF
	[XuiXmlAttribute("tab_caption_key", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeTabCaptionKey(string _value)
	{
		this.TabHeaderText = Localization.Get(_value, false, null);
	}

	// Token: 0x06007484 RID: 29828 RVA: 0x002C04EF File Offset: 0x002BE6EF
	public override void Init()
	{
		base.Init();
		this.parentSelector = base.GetParentByType<XUiC_TabSelector>();
	}

	// Token: 0x06007485 RID: 29829 RVA: 0x0028DC1B File Offset: 0x0028BE1B
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x04005699 RID: 22169
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TabSelector parentSelector;

	// Token: 0x0400569A RID: 22170
	public XUiC_TabSelectorButton TabButton;

	// Token: 0x0400569B RID: 22171
	[PublicizedFrom(EAccessModifier.Private)]
	public string tabKey;

	// Token: 0x0400569C RID: 22172
	[PublicizedFrom(EAccessModifier.Private)]
	public string tabHeaderText;

	// Token: 0x0400569D RID: 22173
	[PublicizedFrom(EAccessModifier.Private)]
	public string tabHeaderSprite;

	// Token: 0x0400569E RID: 22174
	[PublicizedFrom(EAccessModifier.Private)]
	public bool tabVisible = true;

	// Token: 0x0400569F RID: 22175
	[PublicizedFrom(EAccessModifier.Private)]
	public bool tabSelected;

	// Token: 0x040056A0 RID: 22176
	[PublicizedFrom(EAccessModifier.Private)]
	public bool tabHighlight;
}
