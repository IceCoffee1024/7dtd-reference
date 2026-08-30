using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000F42 RID: 3906
[Preserve]
public class XUiC_InfoWindow : XUiController
{
	// Token: 0x06007A9B RID: 31387 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Deselect()
	{
	}

	// Token: 0x06007A9C RID: 31388 RVA: 0x002E3EBE File Offset: 0x002E20BE
	public override void Init()
	{
		base.Init();
		base.OnVisiblity += this.OnVisibilityChanged;
	}

	// Token: 0x06007A9D RID: 31389 RVA: 0x002E3ED8 File Offset: 0x002E20D8
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnVisiblity -= this.OnVisibilityChanged;
	}

	// Token: 0x06007A9E RID: 31390 RVA: 0x002E3EF4 File Offset: 0x002E20F4
	public void OnVisibilityChanged(XUiController _sender, bool _visibleSelf, bool _visibleInScene)
	{
		bool flag = this.windowGroup == null || this.windowGroup.isShowing;
		if (_visibleInScene)
		{
			List<XUiC_InfoWindow> childrenByType = this.xui.GetChildrenByType<XUiC_InfoWindow>();
			for (int i = 0; i < childrenByType.Count; i++)
			{
				if (childrenByType[i] != this && (flag || !childrenByType[i].windowGroup.isShowing))
				{
					childrenByType[i].ViewComponent.IsVisible = false;
				}
			}
			return;
		}
		this.Deselect();
	}
}
