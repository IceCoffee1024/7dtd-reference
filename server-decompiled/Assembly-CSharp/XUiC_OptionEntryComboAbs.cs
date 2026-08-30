using System;
using UnityEngine.Scripting;

// Token: 0x02000D94 RID: 3476
[Preserve]
public abstract class XUiC_OptionEntryComboAbs : XUiC_OptionEntryAbs
{
	// Token: 0x060069BC RID: 27068 RVA: 0x00299210 File Offset: 0x00297410
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

	// Token: 0x060069BD RID: 27069
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void immediatelyApplyCurrentSelection();

	// Token: 0x060069BE RID: 27070 RVA: 0x00299226 File Offset: 0x00297426
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_OptionEntryComboAbs()
	{
	}
}
