using System;
using UnityEngine;

// Token: 0x0200127C RID: 4732
public class NGuiButtonOnClickHandler : MonoBehaviour
{
	// Token: 0x06009618 RID: 38424 RVA: 0x00387256 File Offset: 0x00385456
	public virtual void OnClick()
	{
		if (this.OnClickDelegate != null)
		{
			this.OnClickDelegate.NGuiButtonOnClick(base.transform);
		}
	}

	// Token: 0x06009619 RID: 38425 RVA: 0x00387271 File Offset: 0x00385471
	public virtual void OnDoubleClick()
	{
		if (this.OnDoubleClickDelegate != null)
		{
			this.OnDoubleClickDelegate.NGuiButtonOnDoubleClick(base.transform);
		}
	}

	// Token: 0x0600961A RID: 38426 RVA: 0x0038728C File Offset: 0x0038548C
	public void OnHover(bool _isOver)
	{
		if (this.OnHoverDelegate != null)
		{
			this.OnHoverDelegate.NGuiButtonOnHover(base.transform, _isOver);
		}
	}

	// Token: 0x0600961B RID: 38427 RVA: 0x003872A8 File Offset: 0x003854A8
	public void OnIsHeld()
	{
		if (this.OnIsHeldDelegate != null)
		{
			this.OnIsHeldDelegate.NGuiButtonOnIsHeld(base.transform);
		}
	}

	// Token: 0x04007036 RID: 28726
	public INGuiButtonOnClick OnClickDelegate;

	// Token: 0x04007037 RID: 28727
	public INGuiButtonOnDoubleClick OnDoubleClickDelegate;

	// Token: 0x04007038 RID: 28728
	public INGuiButtonOnHover OnHoverDelegate;

	// Token: 0x04007039 RID: 28729
	public INGuiButtonOnIsHeld OnIsHeldDelegate;
}
