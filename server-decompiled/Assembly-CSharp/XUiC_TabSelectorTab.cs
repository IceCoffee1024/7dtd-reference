using System;
using UnityEngine.Scripting;

// Token: 0x02000EAC RID: 3756
[Preserve]
public class XUiC_TabSelectorTab : XUiController
{
	// Token: 0x17000DBC RID: 3516
	// (get) Token: 0x0600746F RID: 29807 RVA: 0x002C370B File Offset: 0x002C190B
	// (set) Token: 0x06007470 RID: 29808 RVA: 0x002C3713 File Offset: 0x002C1913
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

	// Token: 0x17000DBD RID: 3517
	// (get) Token: 0x06007471 RID: 29809 RVA: 0x002C3745 File Offset: 0x002C1945
	// (set) Token: 0x06007472 RID: 29810 RVA: 0x002C374D File Offset: 0x002C194D
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

	// Token: 0x17000DBE RID: 3518
	// (get) Token: 0x06007473 RID: 29811 RVA: 0x002C3780 File Offset: 0x002C1980
	// (set) Token: 0x06007474 RID: 29812 RVA: 0x002C3788 File Offset: 0x002C1988
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

	// Token: 0x17000DBF RID: 3519
	// (get) Token: 0x06007475 RID: 29813 RVA: 0x002C37BB File Offset: 0x002C19BB
	// (set) Token: 0x06007476 RID: 29814 RVA: 0x002C37C4 File Offset: 0x002C19C4
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

	// Token: 0x17000DC0 RID: 3520
	// (get) Token: 0x06007477 RID: 29815 RVA: 0x002C3812 File Offset: 0x002C1A12
	// (set) Token: 0x06007478 RID: 29816 RVA: 0x002C381C File Offset: 0x002C1A1C
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

	// Token: 0x17000DC1 RID: 3521
	// (get) Token: 0x06007479 RID: 29817 RVA: 0x002C3869 File Offset: 0x002C1A69
	// (set) Token: 0x0600747A RID: 29818 RVA: 0x002C3871 File Offset: 0x002C1A71
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

	// Token: 0x0600747B RID: 29819 RVA: 0x002C3897 File Offset: 0x002C1A97
	[XuiXmlAttribute("tab_caption_key", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeTabCaptionKey(string _value)
	{
		this.TabHeaderText = Localization.Get(_value, false, null);
	}

	// Token: 0x0600747C RID: 29820 RVA: 0x002C38A7 File Offset: 0x002C1AA7
	public override void Init()
	{
		base.Init();
		this.parentSelector = base.GetParentByType<XUiC_TabSelector>();
	}

	// Token: 0x0600747D RID: 29821 RVA: 0x00291183 File Offset: 0x0028F383
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x040056D0 RID: 22224
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TabSelector parentSelector;

	// Token: 0x040056D1 RID: 22225
	public XUiC_TabSelectorButton TabButton;

	// Token: 0x040056D2 RID: 22226
	[PublicizedFrom(EAccessModifier.Private)]
	public string tabKey;

	// Token: 0x040056D3 RID: 22227
	[PublicizedFrom(EAccessModifier.Private)]
	public string tabHeaderText;

	// Token: 0x040056D4 RID: 22228
	[PublicizedFrom(EAccessModifier.Private)]
	public string tabHeaderSprite;

	// Token: 0x040056D5 RID: 22229
	[PublicizedFrom(EAccessModifier.Private)]
	public bool tabVisible = true;

	// Token: 0x040056D6 RID: 22230
	[PublicizedFrom(EAccessModifier.Private)]
	public bool tabSelected;

	// Token: 0x040056D7 RID: 22231
	[PublicizedFrom(EAccessModifier.Private)]
	public bool tabHighlight;
}
