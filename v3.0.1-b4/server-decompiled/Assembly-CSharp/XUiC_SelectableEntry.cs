using System;

// Token: 0x02000E7E RID: 3710
public class XUiC_SelectableEntry : XUiController
{
	// Token: 0x17000D74 RID: 3444
	// (get) Token: 0x06007381 RID: 29569 RVA: 0x002BBE9B File Offset: 0x002BA09B
	// (set) Token: 0x06007382 RID: 29570 RVA: 0x002BBEA4 File Offset: 0x002BA0A4
	public bool IsSelected
	{
		get
		{
			return this.isSelected;
		}
		set
		{
			if (value)
			{
				if (this.xui.CurrentSelectedEntry != null)
				{
					this.xui.CurrentSelectedEntry.SelectedChanged(false);
					this.xui.CurrentSelectedEntry.isSelected = false;
				}
			}
			else if (this.xui.CurrentSelectedEntry == this)
			{
				this.xui.CurrentSelectedEntry.SelectedChanged(false);
				this.xui.CurrentSelectedEntry.isSelected = false;
				this.xui.CurrentSelectedEntry = null;
			}
			this.isSelected = value;
			if (this.isSelected)
			{
				this.xui.CurrentSelectedEntry = this;
			}
			this.SelectedChanged(this.isSelected);
		}
	}

	// Token: 0x06007383 RID: 29571 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SelectedChanged(bool _isSelected)
	{
	}

	// Token: 0x040055D4 RID: 21972
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSelected;
}
