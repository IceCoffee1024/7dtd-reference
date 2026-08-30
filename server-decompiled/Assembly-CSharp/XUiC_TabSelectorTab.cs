using System;
using UnityEngine.Scripting;

// Token: 0x02000E88 RID: 3720
[Preserve]
public class XUiC_TabSelectorTab : XUiController
{
	// Token: 0x17000D88 RID: 3464
	// (get) Token: 0x060073D7 RID: 29655 RVA: 0x002BCE27 File Offset: 0x002BB027
	// (set) Token: 0x060073D8 RID: 29656 RVA: 0x002BCE2F File Offset: 0x002BB02F
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

	// Token: 0x17000D89 RID: 3465
	// (get) Token: 0x060073D9 RID: 29657 RVA: 0x002BCE61 File Offset: 0x002BB061
	// (set) Token: 0x060073DA RID: 29658 RVA: 0x002BCE69 File Offset: 0x002BB069
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

	// Token: 0x17000D8A RID: 3466
	// (get) Token: 0x060073DB RID: 29659 RVA: 0x002BCE9C File Offset: 0x002BB09C
	// (set) Token: 0x060073DC RID: 29660 RVA: 0x002BCEA4 File Offset: 0x002BB0A4
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

	// Token: 0x17000D8B RID: 3467
	// (get) Token: 0x060073DD RID: 29661 RVA: 0x002BCED7 File Offset: 0x002BB0D7
	// (set) Token: 0x060073DE RID: 29662 RVA: 0x002BCEE0 File Offset: 0x002BB0E0
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

	// Token: 0x17000D8C RID: 3468
	// (get) Token: 0x060073DF RID: 29663 RVA: 0x002BCF2E File Offset: 0x002BB12E
	// (set) Token: 0x060073E0 RID: 29664 RVA: 0x002BCF38 File Offset: 0x002BB138
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

	// Token: 0x17000D8D RID: 3469
	// (get) Token: 0x060073E1 RID: 29665 RVA: 0x002BCF85 File Offset: 0x002BB185
	// (set) Token: 0x060073E2 RID: 29666 RVA: 0x002BCF8D File Offset: 0x002BB18D
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

	// Token: 0x060073E3 RID: 29667 RVA: 0x002BCFB3 File Offset: 0x002BB1B3
	[XuiXmlAttribute("tab_caption_key", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeTabCaptionKey(string _value)
	{
		this.TabHeaderText = Localization.Get(_value, false, null);
	}

	// Token: 0x060073E4 RID: 29668 RVA: 0x002BCFC3 File Offset: 0x002BB1C3
	public override void Init()
	{
		base.Init();
		this.parentSelector = base.GetParentByType<XUiC_TabSelector>();
	}

	// Token: 0x060073E5 RID: 29669 RVA: 0x0028AD37 File Offset: 0x00288F37
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x040055F5 RID: 22005
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TabSelector parentSelector;

	// Token: 0x040055F6 RID: 22006
	public XUiC_TabSelectorButton TabButton;

	// Token: 0x040055F7 RID: 22007
	[PublicizedFrom(EAccessModifier.Private)]
	public string tabKey;

	// Token: 0x040055F8 RID: 22008
	[PublicizedFrom(EAccessModifier.Private)]
	public string tabHeaderText;

	// Token: 0x040055F9 RID: 22009
	[PublicizedFrom(EAccessModifier.Private)]
	public string tabHeaderSprite;

	// Token: 0x040055FA RID: 22010
	[PublicizedFrom(EAccessModifier.Private)]
	public bool tabVisible = true;

	// Token: 0x040055FB RID: 22011
	[PublicizedFrom(EAccessModifier.Private)]
	public bool tabSelected;

	// Token: 0x040055FC RID: 22012
	[PublicizedFrom(EAccessModifier.Private)]
	public bool tabHighlight;
}
