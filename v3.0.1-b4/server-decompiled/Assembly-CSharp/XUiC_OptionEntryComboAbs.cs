using System;
using UnityEngine.Scripting;

// Token: 0x02000D80 RID: 3456
[Preserve]
public abstract class XUiC_OptionEntryComboAbs : XUiC_OptionEntryAbs
{
	// Token: 0x06006920 RID: 26912 RVA: 0x002960BC File Offset: 0x002942BC
	[XuiBindEvent("OnValueChangedGeneric", "comboGeneric")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnComboValueChanged(XUiController _sender)
	{
		if (base.ApplyImmediately)
		{
			this.immediatelyApplyCurrentSelection();
		}
		base.invokeValueChanged();
	}

	// Token: 0x06006921 RID: 26913
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void immediatelyApplyCurrentSelection();

	// Token: 0x06006922 RID: 26914 RVA: 0x002960D2 File Offset: 0x002942D2
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_OptionEntryComboAbs()
	{
	}
}
