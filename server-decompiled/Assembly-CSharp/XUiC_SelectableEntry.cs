using System;

// Token: 0x02000EAD RID: 3757
public class XUiC_SelectableEntry : XUiController
{
	// Token: 0x17000DAE RID: 3502
	// (get) Token: 0x0600743E RID: 29758 RVA: 0x002C3CE3 File Offset: 0x002C1EE3
	// (set) Token: 0x0600743F RID: 29759 RVA: 0x002C3CEC File Offset: 0x002C1EEC
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

	// Token: 0x06007440 RID: 29760 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SelectedChanged(bool _isSelected)
	{
	}

	// Token: 0x040056D4 RID: 22228
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSelected;
}
