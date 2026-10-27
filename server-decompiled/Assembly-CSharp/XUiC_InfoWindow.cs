using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000F60 RID: 3936
[Preserve]
public class XUiC_InfoWindow : XUiController
{
	// Token: 0x06007ADB RID: 31451 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Deselect()
	{
	}

	// Token: 0x06007ADC RID: 31452 RVA: 0x002E8B3A File Offset: 0x002E6D3A
	public override void Init()
	{
		base.Init();
		base.OnVisiblity += this.OnVisibilityChanged;
	}

	// Token: 0x06007ADD RID: 31453 RVA: 0x002E8B54 File Offset: 0x002E6D54
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnVisiblity -= this.OnVisibilityChanged;
	}

	// Token: 0x06007ADE RID: 31454 RVA: 0x002E8B70 File Offset: 0x002E6D70
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
