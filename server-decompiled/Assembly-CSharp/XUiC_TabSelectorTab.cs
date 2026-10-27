using System;
using UnityEngine.Scripting;

// Token: 0x02000EB7 RID: 3767
[Preserve]
public class XUiC_TabSelectorTab : XUiController
{
	// Token: 0x17000DC2 RID: 3522
	// (get) Token: 0x06007494 RID: 29844 RVA: 0x002C4C77 File Offset: 0x002C2E77
	// (set) Token: 0x06007495 RID: 29845 RVA: 0x002C4C7F File Offset: 0x002C2E7F
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

	// Token: 0x17000DC3 RID: 3523
	// (get) Token: 0x06007496 RID: 29846 RVA: 0x002C4CB1 File Offset: 0x002C2EB1
	// (set) Token: 0x06007497 RID: 29847 RVA: 0x002C4CB9 File Offset: 0x002C2EB9
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

	// Token: 0x17000DC4 RID: 3524
	// (get) Token: 0x06007498 RID: 29848 RVA: 0x002C4CEC File Offset: 0x002C2EEC
	// (set) Token: 0x06007499 RID: 29849 RVA: 0x002C4CF4 File Offset: 0x002C2EF4
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

	// Token: 0x17000DC5 RID: 3525
	// (get) Token: 0x0600749A RID: 29850 RVA: 0x002C4D27 File Offset: 0x002C2F27
	// (set) Token: 0x0600749B RID: 29851 RVA: 0x002C4D30 File Offset: 0x002C2F30
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

	// Token: 0x17000DC6 RID: 3526
	// (get) Token: 0x0600749C RID: 29852 RVA: 0x002C4D7E File Offset: 0x002C2F7E
	// (set) Token: 0x0600749D RID: 29853 RVA: 0x002C4D88 File Offset: 0x002C2F88
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

	// Token: 0x17000DC7 RID: 3527
	// (get) Token: 0x0600749E RID: 29854 RVA: 0x002C4DD5 File Offset: 0x002C2FD5
	// (set) Token: 0x0600749F RID: 29855 RVA: 0x002C4DDD File Offset: 0x002C2FDD
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

	// Token: 0x060074A0 RID: 29856 RVA: 0x002C4E03 File Offset: 0x002C3003
	[XuiXmlAttribute("tab_caption_key", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeTabCaptionKey(string _value)
	{
		this.TabHeaderText = Localization.Get(_value, false, null);
	}

	// Token: 0x060074A1 RID: 29857 RVA: 0x002C4E13 File Offset: 0x002C3013
	public override void Init()
	{
		base.Init();
		this.parentSelector = base.GetParentByType<XUiC_TabSelector>();
	}

	// Token: 0x060074A2 RID: 29858 RVA: 0x002926AB File Offset: 0x002908AB
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x040056F5 RID: 22261
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TabSelector parentSelector;

	// Token: 0x040056F6 RID: 22262
	public XUiC_TabSelectorButton TabButton;

	// Token: 0x040056F7 RID: 22263
	[PublicizedFrom(EAccessModifier.Private)]
	public string tabKey;

	// Token: 0x040056F8 RID: 22264
	[PublicizedFrom(EAccessModifier.Private)]
	public string tabHeaderText;

	// Token: 0x040056F9 RID: 22265
	[PublicizedFrom(EAccessModifier.Private)]
	public string tabHeaderSprite;

	// Token: 0x040056FA RID: 22266
	[PublicizedFrom(EAccessModifier.Private)]
	public bool tabVisible = true;

	// Token: 0x040056FB RID: 22267
	[PublicizedFrom(EAccessModifier.Private)]
	public bool tabSelected;

	// Token: 0x040056FC RID: 22268
	[PublicizedFrom(EAccessModifier.Private)]
	public bool tabHighlight;
}
