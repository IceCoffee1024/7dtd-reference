using System;
using UnityEngine;

// Token: 0x020012AE RID: 4782
public class NGuiButtonOnClickHandler : MonoBehaviour
{
	// Token: 0x060097A0 RID: 38816 RVA: 0x00390BFA File Offset: 0x0038EDFA
	public virtual void OnClick()
	{
		if (this.OnClickDelegate != null)
		{
			this.OnClickDelegate.NGuiButtonOnClick(base.transform);
		}
	}

	// Token: 0x060097A1 RID: 38817 RVA: 0x00390C15 File Offset: 0x0038EE15
	public virtual void OnDoubleClick()
	{
		if (this.OnDoubleClickDelegate != null)
		{
			this.OnDoubleClickDelegate.NGuiButtonOnDoubleClick(base.transform);
		}
	}

	// Token: 0x060097A2 RID: 38818 RVA: 0x00390C30 File Offset: 0x0038EE30
	public void OnHover(bool _isOver)
	{
		if (this.OnHoverDelegate != null)
		{
			this.OnHoverDelegate.NGuiButtonOnHover(base.transform, _isOver);
		}
	}

	// Token: 0x060097A3 RID: 38819 RVA: 0x00390C4C File Offset: 0x0038EE4C
	public void OnIsHeld()
	{
		if (this.OnIsHeldDelegate != null)
		{
			this.OnIsHeldDelegate.NGuiButtonOnIsHeld(base.transform);
		}
	}

	// Token: 0x04007175 RID: 29045
	public INGuiButtonOnClick OnClickDelegate;

	// Token: 0x04007176 RID: 29046
	public INGuiButtonOnDoubleClick OnDoubleClickDelegate;

	// Token: 0x04007177 RID: 29047
	public INGuiButtonOnHover OnHoverDelegate;

	// Token: 0x04007178 RID: 29048
	public INGuiButtonOnIsHeld OnIsHeldDelegate;
}
