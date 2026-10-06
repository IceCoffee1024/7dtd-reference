using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000F51 RID: 3921
[Preserve]
public class XUiC_InfoWindow : XUiController
{
	// Token: 0x06007A98 RID: 31384 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Deselect()
	{
	}

	// Token: 0x06007A99 RID: 31385 RVA: 0x002E723E File Offset: 0x002E543E
	public override void Init()
	{
		base.Init();
		base.OnVisiblity += this.OnVisibilityChanged;
	}

	// Token: 0x06007A9A RID: 31386 RVA: 0x002E7258 File Offset: 0x002E5458
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnVisiblity -= this.OnVisibilityChanged;
	}

	// Token: 0x06007A9B RID: 31387 RVA: 0x002E7274 File Offset: 0x002E5474
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
