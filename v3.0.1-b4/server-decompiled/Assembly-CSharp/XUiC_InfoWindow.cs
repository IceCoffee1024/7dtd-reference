using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000F2B RID: 3883
[Preserve]
public class XUiC_InfoWindow : XUiController
{
	// Token: 0x060079E2 RID: 31202 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Deselect()
	{
	}

	// Token: 0x060079E3 RID: 31203 RVA: 0x002DFF0A File Offset: 0x002DE10A
	public override void Init()
	{
		base.Init();
		base.OnVisiblity += this.OnVisibilityChanged;
	}

	// Token: 0x060079E4 RID: 31204 RVA: 0x002DFF24 File Offset: 0x002DE124
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnVisiblity -= this.OnVisibilityChanged;
	}

	// Token: 0x060079E5 RID: 31205 RVA: 0x002DFF40 File Offset: 0x002DE140
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
