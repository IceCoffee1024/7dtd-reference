using System;
using UnityEngine;

// Token: 0x02000057 RID: 87
[RequireComponent(typeof(UIWidget))]
public class SetColorPickerColor : MonoBehaviour
{
	// Token: 0x060001C1 RID: 449 RVA: 0x000107B3 File Offset: 0x0000E9B3
	public void SetToCurrent()
	{
		if (this.mWidget == null)
		{
			this.mWidget = base.GetComponent<UIWidget>();
		}
		if (UIColorPicker.current != null)
		{
			this.mWidget.color = UIColorPicker.current.value;
		}
	}

	// Token: 0x0400026E RID: 622
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UIWidget mWidget;
}
