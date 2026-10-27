using System;
using UnityEngine;

// Token: 0x020012C0 RID: 4800
public class NGuiButtonOnClickHandler : MonoBehaviour
{
	// Token: 0x060097F8 RID: 38904 RVA: 0x00393002 File Offset: 0x00391202
	public virtual void OnClick()
	{
		if (this.OnClickDelegate != null)
		{
			this.OnClickDelegate.NGuiButtonOnClick(base.transform);
		}
	}

	// Token: 0x060097F9 RID: 38905 RVA: 0x0039301D File Offset: 0x0039121D
	public virtual void OnDoubleClick()
	{
		if (this.OnDoubleClickDelegate != null)
		{
			this.OnDoubleClickDelegate.NGuiButtonOnDoubleClick(base.transform);
		}
	}

	// Token: 0x060097FA RID: 38906 RVA: 0x00393038 File Offset: 0x00391238
	public void OnHover(bool _isOver)
	{
		if (this.OnHoverDelegate != null)
		{
			this.OnHoverDelegate.NGuiButtonOnHover(base.transform, _isOver);
		}
	}

	// Token: 0x060097FB RID: 38907 RVA: 0x00393054 File Offset: 0x00391254
	public void OnIsHeld()
	{
		if (this.OnIsHeldDelegate != null)
		{
			this.OnIsHeldDelegate.NGuiButtonOnIsHeld(base.transform);
		}
	}

	// Token: 0x040071B6 RID: 29110
	public INGuiButtonOnClick OnClickDelegate;

	// Token: 0x040071B7 RID: 29111
	public INGuiButtonOnDoubleClick OnDoubleClickDelegate;

	// Token: 0x040071B8 RID: 29112
	public INGuiButtonOnHover OnHoverDelegate;

	// Token: 0x040071B9 RID: 29113
	public INGuiButtonOnIsHeld OnIsHeldDelegate;
}
