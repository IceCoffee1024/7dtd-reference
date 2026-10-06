using System;

// Token: 0x02000EA2 RID: 3746
public class XUiC_SelectableEntry : XUiController
{
	// Token: 0x17000DA8 RID: 3496
	// (get) Token: 0x06007419 RID: 29721 RVA: 0x002C2777 File Offset: 0x002C0977
	// (set) Token: 0x0600741A RID: 29722 RVA: 0x002C2780 File Offset: 0x002C0980
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

	// Token: 0x0600741B RID: 29723 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SelectedChanged(bool _isSelected)
	{
	}

	// Token: 0x040056AF RID: 22191
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSelected;
}
