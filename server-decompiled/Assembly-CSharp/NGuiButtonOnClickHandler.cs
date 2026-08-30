using System;
using UnityEngine;

// Token: 0x02001296 RID: 4758
public class NGuiButtonOnClickHandler : MonoBehaviour
{
	// Token: 0x06009713 RID: 38675 RVA: 0x0038C3B2 File Offset: 0x0038A5B2
	public virtual void OnClick()
	{
		if (this.OnClickDelegate != null)
		{
			this.OnClickDelegate.NGuiButtonOnClick(base.transform);
		}
	}

	// Token: 0x06009714 RID: 38676 RVA: 0x0038C3CD File Offset: 0x0038A5CD
	public virtual void OnDoubleClick()
	{
		if (this.OnDoubleClickDelegate != null)
		{
			this.OnDoubleClickDelegate.NGuiButtonOnDoubleClick(base.transform);
		}
	}

	// Token: 0x06009715 RID: 38677 RVA: 0x0038C3E8 File Offset: 0x0038A5E8
	public void OnHover(bool _isOver)
	{
		if (this.OnHoverDelegate != null)
		{
			this.OnHoverDelegate.NGuiButtonOnHover(base.transform, _isOver);
		}
	}

	// Token: 0x06009716 RID: 38678 RVA: 0x0038C404 File Offset: 0x0038A604
	public void OnIsHeld()
	{
		if (this.OnIsHeldDelegate != null)
		{
			this.OnIsHeldDelegate.NGuiButtonOnIsHeld(base.transform);
		}
	}

	// Token: 0x0400710B RID: 28939
	public INGuiButtonOnClick OnClickDelegate;

	// Token: 0x0400710C RID: 28940
	public INGuiButtonOnDoubleClick OnDoubleClickDelegate;

	// Token: 0x0400710D RID: 28941
	public INGuiButtonOnHover OnHoverDelegate;

	// Token: 0x0400710E RID: 28942
	public INGuiButtonOnIsHeld OnIsHeldDelegate;
}
