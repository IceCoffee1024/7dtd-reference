using System;

// Token: 0x02000E93 RID: 3731
public class XUiC_SelectableEntry : XUiController
{
	// Token: 0x17000D8D RID: 3469
	// (get) Token: 0x06007421 RID: 29729 RVA: 0x002BF3C7 File Offset: 0x002BD5C7
	// (set) Token: 0x06007422 RID: 29730 RVA: 0x002BF3D0 File Offset: 0x002BD5D0
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

	// Token: 0x06007423 RID: 29731 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SelectedChanged(bool _isSelected)
	{
	}

	// Token: 0x04005678 RID: 22136
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSelected;
}
